# Releasing a new version

1. Bump the version in **both** places (they must match, the Forge checks this):
   - `source/Pause.csproj` -> `<Version>`
   - `source/Plugin.cs` -> `PluginVersion`
   - and `VERSION` at the top of `build-release.bat`
2. Add an entry to `CHANGELOG.md`.
3. Build and test in a fresh SPT install: `.\build-release.bat "C:\SPT"`.
4. Commit, tag and push: `git tag vX.Y.Z` then `git push && git push --tags`.
5. On GitHub: Releases -> Draft a new release -> pick the tag, paste notes, attach `release\rejzbihai-Pause-X.Y.Z.zip`.
6. Upload the zip to virustotal.com and add the link to the release notes.
7. On the Forge: add a new version with the direct GitHub download link, the VirusTotal link and the changelog.
