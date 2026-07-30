using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

const string HiddenAttribute = "Keysharp.Runtime.PublicHiddenFromUser";
const string UserNameAttribute = "Keysharp.Runtime.UserDeclaredNameAttribute";

var options = ParseOptions(args);
var assemblyPath = RequiredPath(options, "assembly", File.Exists);
var sourceRepo = RequiredPath(options, "source-repo", Directory.Exists);
var docsRepo = RequiredPath(options, "docs-repo", Directory.Exists);
var docsRoot = RequiredPath(options, "docs", Directory.Exists);
var jsonOutput = RequiredOption(options, "json");
var markdownOutput = RequiredOption(options, "markdown");

var docsCatalog = BuildDocumentationCatalog(docsRoot);
var loadContext = new InspectionLoadContext(assemblyPath);

try
{
    var assembly = loadContext.LoadFromAssemblyPath(assemblyPath);
    var entries = BuildApiEntries(assembly)
        .Select(entry =>
        {
            var documentation = docsCatalog.Find(entry.Name);
            return entry with
            {
                Documentation = documentation,
                DocumentationStatus = documentation.Count > 0
                    ? "locator-found"
                    : "needs-review"
            };
        })
        .OrderBy(entry => entry.Kind, StringComparer.Ordinal)
        .ThenBy(entry => entry.Name, StringComparer.OrdinalIgnoreCase)
        .ToArray();

    var sourceState = ReadRepositoryState(sourceRepo);
    var docsState = ReadRepositoryState(docsRepo);
    var inventory = new Inventory(
        1,
        "Public built-in types, global functions, and global properties",
        new Evidence(
            sourceState.Remote,
            sourceState.Commit,
            sourceState.TreeState,
            docsState.Commit,
            docsState.TreeState,
            GetPlatformName(),
            RuntimeInformation.FrameworkDescription,
            Path.GetFileName(assemblyPath),
            Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(assemblyPath))).ToLowerInvariant()),
        BuildCounts(entries),
        entries);

    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(jsonOutput))!);
    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(markdownOutput))!);

    var json = JsonSerializer.Serialize(inventory, new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    });
    File.WriteAllText(jsonOutput, json + Environment.NewLine, new UTF8Encoding(false));
    File.WriteAllText(markdownOutput, BuildMarkdown(inventory), new UTF8Encoding(false));

    Console.WriteLine(
        $"Catalogued {entries.Length} API names: " +
        $"{inventory.Counts.WithLocator} with a documentation locator, " +
        $"{inventory.Counts.NeedsReview} needing review.");
    Console.WriteLine($"JSON: {Path.GetFullPath(jsonOutput)}");
    Console.WriteLine($"Review queue: {Path.GetFullPath(markdownOutput)}");
}
finally
{
    loadContext.Unload();
}

static IReadOnlyList<ApiEntry> BuildApiEntries(Assembly assembly)
{
    var builtinTypes = assembly
        .GetExportedTypes()
        .Where(type =>
            type.IsClass
            && type.Namespace?.StartsWith("Keysharp.Builtins", StringComparison.Ordinal) == true
            && !HasAttribute(type, HiddenAttribute))
        .ToArray();

    var declarations = new List<ApiDeclaration>();

    foreach (var type in builtinTypes)
    {
        declarations.Add(new ApiDeclaration(
            "type",
            UserVisibleName(type),
            type.FullName ?? type.Name));
    }

    var ahkType = assembly.GetType("Keysharp.Runtime.Ahk", throwOnError: false);
    if (ahkType is not null)
    {
        declarations.Add(new ApiDeclaration(
            "type",
            UserVisibleName(ahkType),
            ahkType.FullName ?? ahkType.Name));
    }

    foreach (var type in builtinTypes.Where(type => type.IsAbstract && type.IsSealed))
    {
        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
        {
            if (method.IsSpecialName || HasAttribute(method, HiddenAttribute))
                continue;

            declarations.Add(new ApiDeclaration(
                "function",
                UserVisibleName(method),
                $"{type.FullName}.{method.Name}"));
        }

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Static))
        {
            if (HasAttribute(property, HiddenAttribute))
                continue;

            declarations.Add(new ApiDeclaration(
                "property",
                UserVisibleName(property),
                $"{type.FullName}.{property.Name}"));
        }
    }

    return declarations
        .GroupBy(
            declaration => (declaration.Kind, declaration.Name),
            new ApiDeclarationKeyComparer())
        .Select(group => new ApiEntry(
            group.Key.Kind,
            group.Key.Name,
            group.Select(item => item.Declaration)
                .Distinct(StringComparer.Ordinal)
                .OrderBy(item => item, StringComparer.Ordinal)
                .ToArray(),
            "needs-review",
            []))
        .ToArray();
}

static Counts BuildCounts(IReadOnlyList<ApiEntry> entries)
{
    return new Counts(
        entries.Count(entry => entry.Kind == "type"),
        entries.Count(entry => entry.Kind == "function"),
        entries.Count(entry => entry.Kind == "property"),
        entries.Count(entry => entry.DocumentationStatus == "locator-found"),
        entries.Count(entry => entry.DocumentationStatus == "needs-review"),
        entries.Count(entry => entry.Declarations.Count > 1));
}

static string BuildMarkdown(Inventory inventory)
{
    var builder = new StringBuilder();
    builder.AppendLine("# Source-derived global API audit");
    builder.AppendLine();
    builder.AppendLine("> Generated by `scripts/Update-SourceAudit.ps1`. Do not edit by hand.");
    builder.AppendLine();
    builder.AppendLine("This is a review queue, not a capability matrix. It records names exposed by");
    builder.AppendLine("the inspected runtime and whether a strong documentation locator was found.");
    builder.AppendLine("It does not assert that an API is complete or works on every platform.");
    builder.AppendLine();
    builder.AppendLine("## Evidence identity");
    builder.AppendLine();
    builder.AppendLine($"- Source: `{inventory.Evidence.SourceRemote}`");
    builder.AppendLine($"- Source commit: `{inventory.Evidence.SourceCommit}`");
    builder.AppendLine($"- Source tree: **{inventory.Evidence.SourceTreeState}**");
    builder.AppendLine($"- Docs commit: `{inventory.Evidence.DocsCommit}`");
    builder.AppendLine($"- Docs tree at generation: **{inventory.Evidence.DocsTreeState}**");
    builder.AppendLine($"- Host platform: **{inventory.Evidence.Platform}**");
    builder.AppendLine($"- Runtime: `{inventory.Evidence.Framework}`");
    builder.AppendLine($"- Assembly SHA-256: `{inventory.Evidence.AssemblySha256}`");
    builder.AppendLine();
    if (inventory.Evidence.SourceTreeState != "clean")
    {
        builder.AppendLine("> **Provisional:** The source tree had uncommitted changes. Regenerate this");
        builder.AppendLine("> audit from the reviewed source commit before treating it as a baseline.");
        builder.AppendLine();
    }

    builder.AppendLine("## Summary");
    builder.AppendLine();
    builder.AppendLine("| Scope | Count |");
    builder.AppendLine("| --- | ---: |");
    builder.AppendLine($"| Public built-in types | {inventory.Counts.Types} |");
    builder.AppendLine($"| Global functions | {inventory.Counts.Functions} |");
    builder.AppendLine($"| Global properties | {inventory.Counts.Properties} |");
    builder.AppendLine($"| Names with a documentation locator | {inventory.Counts.WithLocator} |");
    builder.AppendLine($"| Names needing review | {inventory.Counts.NeedsReview} |");
    builder.AppendLine($"| Names with multiple runtime declarations | {inventory.Counts.Collisions} |");
    builder.AppendLine();

    var needsReview = inventory.Entries
        .Where(entry => entry.DocumentationStatus == "needs-review")
        .GroupBy(entry => entry.Kind)
        .OrderBy(group => group.Key, StringComparer.Ordinal);

    builder.AppendLine("## Review queue");
    builder.AppendLine();
    foreach (var group in needsReview)
    {
        builder.AppendLine($"### {KindHeading(group.Key)}");
        builder.AppendLine();
        builder.AppendLine("| Script name | Runtime declaration |");
        builder.AppendLine("| --- | --- |");
        foreach (var entry in group.OrderBy(entry => entry.Name, StringComparer.OrdinalIgnoreCase))
        {
            builder.Append("| `")
                .Append(EscapeMarkdown(entry.Name))
                .Append("` | `")
                .Append(EscapeMarkdown(string.Join("; ", entry.Declarations)))
                .AppendLine("` |");
        }
        builder.AppendLine();
    }

    var collisions = inventory.Entries
        .Where(entry => entry.Declarations.Count > 1)
        .OrderBy(entry => entry.Name, StringComparer.OrdinalIgnoreCase)
        .ToArray();
    if (collisions.Length > 0)
    {
        builder.AppendLine("## Runtime name collisions");
        builder.AppendLine();
        builder.AppendLine("These names have multiple public declarations. Confirm which declaration");
        builder.AppendLine("the runtime resolves and whether the duplicate surface is intentional.");
        builder.AppendLine();
        builder.AppendLine("| Kind | Script name | Runtime declarations |");
        builder.AppendLine("| --- | --- | --- |");
        foreach (var entry in collisions)
        {
            builder.Append("| ")
                .Append(entry.Kind)
                .Append(" | `")
                .Append(EscapeMarkdown(entry.Name))
                .Append("` | `")
                .Append(EscapeMarkdown(string.Join("; ", entry.Declarations)))
                .AppendLine("` |");
        }
        builder.AppendLine();
    }

    return builder.ToString().TrimEnd() + Environment.NewLine;
}

static string KindHeading(string kind) => kind switch
{
    "function" => "Global functions",
    "property" => "Global properties",
    "type" => "Public built-in types",
    _ => kind
};

static string EscapeMarkdown(string value) =>
    value.Replace("|", "\\|", StringComparison.Ordinal);

static string UserVisibleName(MemberInfo member)
{
    var attribute = member.CustomAttributes.FirstOrDefault(item =>
        item.AttributeType.FullName == UserNameAttribute);
    if (attribute?.ConstructorArguments.Count > 0
        && attribute.ConstructorArguments[0].Value is string name
        && !string.IsNullOrWhiteSpace(name))
    {
        return name;
    }

    return member.Name;
}

static bool HasAttribute(MemberInfo member, string fullName) =>
    member.CustomAttributes.Any(item => item.AttributeType.FullName == fullName);

static RepositoryState ReadRepositoryState(string repository)
{
    var commit = RunGit(repository, "rev-parse", "HEAD");
    var remote = SanitizeRemote(RunGit(repository, "remote", "get-url", "origin"));
    var status = RunGit(repository, "status", "--porcelain");
    return new RepositoryState(
        string.IsNullOrWhiteSpace(remote) ? "(no origin)" : remote,
        string.IsNullOrWhiteSpace(commit) ? "(unknown)" : commit,
        string.IsNullOrWhiteSpace(status) ? "clean" : "dirty");
}

static string SanitizeRemote(string remote)
{
    if (!Uri.TryCreate(remote, UriKind.Absolute, out var uri) || string.IsNullOrEmpty(uri.UserInfo))
        return remote;

    var builder = new UriBuilder(uri)
    {
        UserName = "",
        Password = ""
    };
    return builder.Uri.ToString();
}

static string RunGit(string repository, params string[] arguments)
{
    var startInfo = new System.Diagnostics.ProcessStartInfo("git")
    {
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false
    };
    startInfo.ArgumentList.Add("-C");
    startInfo.ArgumentList.Add(repository);
    foreach (var argument in arguments)
        startInfo.ArgumentList.Add(argument);

    using var process = System.Diagnostics.Process.Start(startInfo);
    if (process is null)
        return "";

    var output = process.StandardOutput.ReadToEnd();
    process.WaitForExit();
    return process.ExitCode == 0 ? output.Trim() : "";
}

static string GetPlatformName()
{
    if (OperatingSystem.IsWindows())
        return "Windows";
    if (OperatingSystem.IsMacOS())
        return "macOS";
    if (OperatingSystem.IsLinux())
        return "Linux";
    return RuntimeInformation.OSDescription;
}

static Dictionary<string, string> ParseOptions(string[] arguments)
{
    var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    for (var index = 0; index < arguments.Length; index += 2)
    {
        if (index + 1 >= arguments.Length || !arguments[index].StartsWith("--", StringComparison.Ordinal))
            throw new ArgumentException("Options must use --name value pairs.");
        result[arguments[index][2..]] = arguments[index + 1];
    }
    return result;
}

static string RequiredOption(IReadOnlyDictionary<string, string> options, string name)
{
    if (!options.TryGetValue(name, out var value) || string.IsNullOrWhiteSpace(value))
        throw new ArgumentException($"Missing required option --{name}.");
    return Path.GetFullPath(value);
}

static string RequiredPath(
    IReadOnlyDictionary<string, string> options,
    string name,
    Func<string, bool> exists)
{
    var path = RequiredOption(options, name);
    if (!exists(path))
        throw new FileNotFoundException($"Path supplied to --{name} does not exist.", path);
    return path;
}

static DocumentationCatalog BuildDocumentationCatalog(string docsRoot)
{
    var references = new Dictionary<string, List<DocReference>>(StringComparer.OrdinalIgnoreCase);
    var archivedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "AutoHotkeyChangeLog.htm",
        "AutoHotkeyLicense.htm"
    };

    foreach (var path in Directory.EnumerateFiles(docsRoot, "*.htm", SearchOption.AllDirectories))
    {
        if (archivedNames.Contains(Path.GetFileName(path)))
            continue;

        var relativePath = Path.GetRelativePath(docsRoot, path).Replace('\\', '/');
        var pageName = Path.GetFileNameWithoutExtension(path);
        AddReference(references, pageName, new DocReference(relativePath, "page-name"));

        var content = File.ReadAllText(path);
        foreach (Match match in Regex.Matches(
            content,
            """\bid\s*=\s*["'](?<id>[^"']+)["']""",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            var id = WebUtility.HtmlDecode(match.Groups["id"].Value);
            AddReference(references, id, new DocReference($"{relativePath}#{id}", "anchor"));
        }

        foreach (Match match in Regex.Matches(
            content,
            """<h[1-6]\b[^>]*>(?<heading>.*?)</h[1-6]>""",
            RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant))
        {
            var heading = Regex.Replace(match.Groups["heading"].Value, "<[^>]+>", "");
            heading = WebUtility.HtmlDecode(heading).Trim();
            AddReference(references, heading, new DocReference(relativePath, "heading"));
        }
    }

    var indexPath = Path.Combine(docsRoot, "static", "source", "data_index.js");
    if (File.Exists(indexPath))
    {
        var indexContent = File.ReadAllText(indexPath);
        foreach (Match match in Regex.Matches(
            indexContent,
            @"^\s*\[""(?<term>(?:\\.|[^""])*)""\s*,\s*""(?<href>(?:\\.|[^""])*)""",
            RegexOptions.Multiline | RegexOptions.CultureInvariant))
        {
            var term = JsonSerializer.Deserialize<string>($"\"{match.Groups["term"].Value}\"");
            var href = JsonSerializer.Deserialize<string>($"\"{match.Groups["href"].Value}\"");
            if (!string.IsNullOrWhiteSpace(term) && !string.IsNullOrWhiteSpace(href))
                AddReference(references, term, new DocReference(href, "index"));
        }
    }

    return new DocumentationCatalog(references);
}

static void AddReference(
    IDictionary<string, List<DocReference>> references,
    string name,
    DocReference reference)
{
    if (string.IsNullOrWhiteSpace(name))
        return;

    if (!references.TryGetValue(name, out var items))
    {
        items = [];
        references[name] = items;
    }

    if (!items.Contains(reference))
        items.Add(reference);
}

sealed class DocumentationCatalog(
    IReadOnlyDictionary<string, List<DocReference>> references)
{
    public IReadOnlyList<DocReference> Find(string apiName)
    {
        var result = new HashSet<DocReference>();
        if (references.TryGetValue(apiName, out var exact))
            result.UnionWith(exact);

        return result
            .OrderBy(item => item.Href, StringComparer.OrdinalIgnoreCase)
            .ThenBy(item => item.Evidence, StringComparer.Ordinal)
            .ToArray();
    }
}

sealed class InspectionLoadContext(string assemblyPath)
    : AssemblyLoadContext(isCollectible: true)
{
    private readonly AssemblyDependencyResolver resolver = new(assemblyPath);

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var path = resolver.ResolveAssemblyToPath(assemblyName);
        return path is null ? null : LoadFromAssemblyPath(path);
    }
}

sealed class ApiDeclarationKeyComparer
    : IEqualityComparer<(string Kind, string Name)>
{
    public bool Equals(
        (string Kind, string Name) x,
        (string Kind, string Name) y) =>
        StringComparer.Ordinal.Equals(x.Kind, y.Kind)
        && StringComparer.OrdinalIgnoreCase.Equals(x.Name, y.Name);

    public int GetHashCode((string Kind, string Name) value) =>
        HashCode.Combine(
            StringComparer.Ordinal.GetHashCode(value.Kind),
            StringComparer.OrdinalIgnoreCase.GetHashCode(value.Name));
}

sealed record ApiDeclaration(string Kind, string Name, string Declaration);
sealed record DocReference(string Href, string Evidence);
sealed record ApiEntry(
    string Kind,
    string Name,
    IReadOnlyList<string> Declarations,
    string DocumentationStatus,
    IReadOnlyList<DocReference> Documentation);
sealed record Evidence(
    string SourceRemote,
    string SourceCommit,
    string SourceTreeState,
    string DocsCommit,
    string DocsTreeState,
    string Platform,
    string Framework,
    string Assembly,
    string AssemblySha256);
sealed record Counts(
    int Types,
    int Functions,
    int Properties,
    int WithLocator,
    int NeedsReview,
    int Collisions);
sealed record Inventory(
    int SchemaVersion,
    string Scope,
    Evidence Evidence,
    Counts Counts,
    IReadOnlyList<ApiEntry> Entries);
sealed record RepositoryState(string Remote, string Commit, string TreeState);
