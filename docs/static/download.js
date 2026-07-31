/*
 * Download picker enhancement.
 *
 * The markup this runs against is already usable on its own: every card is a link to the latest
 * release page, which never breaks. This script improves on that when it can, by asking the GitHub
 * API which files the latest release actually carries and pointing each card straight at one.
 *
 * It has to be done at run time because every asset name embeds the version
 * (keysharp-0.0.0.16-win-x64.msi), so a direct link written into the page would 404 on the next
 * release. Any failure - offline, rate limited, blocked - simply leaves the original links in place.
 */
(function () {
	'use strict';

	var RELEASES_API = 'https://api.github.com/repos/Descolada/keysharp/releases/latest';

	// Preferred installer per platform, then the alternatives offered to anyone who wants them.
	// Windows publishes x64 only, so its architecture is not a choice a visitor has to make.
	var PLATFORMS = {
		windows: { primary: /-win-x64\.msi$/, alts: [{ re: /-win-x64\.zip$/, label: 'Portable ZIP' }] },
		macos: {
			arm64: { primary: /-osx-arm64\.dmg$/, alts: [{ re: /-osx-arm64\.pkg$/, label: 'PKG installer' }] },
			x64: { primary: /-osx-x64\.dmg$/, alts: [{ re: /-osx-x64\.pkg$/, label: 'PKG installer' }] }
		},
		linux: {
			arm64: { primary: /-linux-arm64\.deb$/, alts: [{ re: /-linux-arm64\.tar\.gz$/, label: 'Portable tarball' }] },
			x64: { primary: /-linux-x64\.deb$/, alts: [{ re: /-linux-x64\.tar\.gz$/, label: 'Portable tarball' }] }
		}
	};

	function detect() {
		var ua = navigator.userAgent || '';
		var uaData = navigator.userAgentData;
		var platform = (uaData && uaData.platform) || navigator.platform || '';
		var os = null;

		if (/Win/i.test(platform) || /Windows/i.test(ua)) os = 'windows';
		else if (/Mac/i.test(platform) || /Mac OS X/i.test(ua)) os = 'macos';
		else if (/Linux|X11/i.test(platform) || /Linux/i.test(ua)) os = 'linux';

		// Apple silicon is not reported in the user agent, so assume the current architecture for
		// Macs and let the visitor switch. Elsewhere, only an explicit aarch64/arm64 marker counts.
		var arch = /aarch64|arm64/i.test(ua) ? 'arm64' : (os === 'macos' ? 'arm64' : 'x64');
		return { os: os, arch: arch };
	}

	function bytes(n) {
		return typeof n === 'number' && n > 0 ? (n / 1048576).toFixed(1) + ' MB' : '';
	}

	function pick(assets, re) {
		for (var i = 0; i < assets.length; i++)
			if (re.test(assets[i].name)) return assets[i];
		return null;
	}

	function apply(release) {
		var found = detect();
		var version = (release.tag_name || '').replace(/^v/, '');
		var assets = release.assets || [];
		var cards = document.querySelectorAll('.dl-card');

		for (var i = 0; i < cards.length; i++) {
			var card = cards[i];
			var os = card.getAttribute('data-os');
			var spec = PLATFORMS[os];
			if (!spec) continue;

			// macOS and Linux ship per-architecture builds; Windows does not.
			var arch = spec.primary ? null : (card.getAttribute('data-arch') || found.arch);
			var choice = spec.primary ? spec : spec[arch];
			if (!choice) continue;

			var asset = pick(assets, choice.primary);
			if (!asset) continue;

			card.href = asset.browser_download_url;
			var file = card.querySelector('.dl-file');
			if (file) file.textContent = asset.name + (bytes(asset.size) ? ' · ' + bytes(asset.size) : '');

			var alts = [];
			for (var a = 0; a < choice.alts.length; a++) {
				var alt = pick(assets, choice.alts[a].re);
				if (alt) alts.push('<a href="' + alt.browser_download_url + '">' + choice.alts[a].label + '</a>');
			}
			var altBox = card.parentNode.querySelector('.dl-alt[data-for="' + os + '"]');
			if (altBox && alts.length) altBox.innerHTML = 'Also available: ' + alts.join(' &middot; ');

			if (os === found.os) {
				card.setAttribute('aria-current', 'true');
				var mark = card.querySelector('.dl-detected');
				if (mark) mark.textContent = '· your system';
			}
		}

		var status = document.getElementById('dl-status');
		if (status && version) {
			status.textContent = 'Showing Keysharp ' + version + '.'
				+ (found.os ? '' : ' Choose the package for your system.');
		}
	}

	// The landing page carries a single button through to the picker rather than the picker itself,
	// so it only needs the version and the detected system name.
	function applyLanding(release) {
		var sub = document.getElementById('ks-download-sub');
		if (!sub) return;

		var names = { windows: 'Windows', macos: 'macOS', linux: 'Linux' };
		var found = detect();
		var version = (release.tag_name || '').replace(/^v/, '');
		if (!version) return;

		sub.textContent = found.os
			? version + ' for ' + names[found.os]
			: version + ' for Windows, macOS and Linux';
	}

	function run() {
		if (!document.querySelector('.dl-card') && !document.getElementById('ks-download-sub')) return;

		try {
			var req = new XMLHttpRequest();
			req.open('GET', RELEASES_API, true);
			req.onload = function () {
				if (req.status < 200 || req.status >= 300) return;   // leave the static links alone

				try {
					var release = JSON.parse(req.responseText);
					apply(release);
					applyLanding(release);
				} catch (e) { /* keep the static links */ }
			};
			req.send();
		} catch (e) {
			/* keep the static links */
		}
	}

	if (document.readyState === 'loading')
		document.addEventListener('DOMContentLoaded', run);
	else
		run();
})();
