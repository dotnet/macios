---
name: update-expected-app-size
description: >-
  Download updated expected app size files from Azure DevOps CI artifacts for
  the current branch. Use when app size tests fail in CI and the user wants to
  update the expected files locally. Trigger on "update app size", "download
  expected files", "fix app size test", or "update expected app size files".
---

# Update Expected App Size Files

Download updated expected app size files from Azure DevOps artifacts for the current branch's PR build, and apply them to the repository.

## When to Use

- App size tests failed in CI and the user wants to update the expected files
- User asks to "update app size", "download expected files", or "fix app size test failures"
- User wants to pull the updated expected files from a recent CI build

## Background

The app size tests (`tests/dotnet/UnitTests/AppSizeTest.cs`) compare the built app's size and preserved APIs against expected files stored in `tests/dotnet/UnitTests/expected/`. When the test detects a difference and `WRITE_KNOWN_FAILURES` is not set, it writes the updated expected file to `$(Build.ArtifactStagingDirectory)/updated-expected-sizes/`, and a pipeline step publishes this directory as a build artifact.

The artifact name follows the pattern `{uploadPrefix}updated-expected-sizes-{testPrefix}-{attempt}` (e.g., `updated-expected-sizes-dotnettests_ios-1`). Because the expected files can be very big, the test uploads a unified **diff** for each changed expected file rather than the whole file. Inside the artifact, files are named after the test variant with a `.diff` suffix:
- `{Platform}-{Variant}-size.txt.diff` — e.g., `iOS-MonoVM-size.txt.diff`, `iOS-MonoVM-interpreter-size.txt.diff`, `iOS-NativeAOT-TrimmableStatic-size.txt.diff`, `MacOSX-CoreCLR-Interpreter-ManagedStatic-size.txt.diff`
- `{Platform}-{Variant}-preservedapis.txt.diff` — e.g., `iOS-MonoVM-preservedapis.txt.diff`, `MacCatalyst-MonoVM-interpreter-preservedapis.txt.diff`

Each diff is a unified diff (relative to the repository root) that can be applied with `git apply -p1` or `patch -p1`. The expected files on disk are at:
- `tests/dotnet/UnitTests/expected/{Platform}-{Variant}-size.txt`
- `tests/dotnet/UnitTests/expected/{Platform}-{Variant}-preservedapis.txt`

## Workflow

### 1. Determine the current branch and PR

```bash
BRANCH=$(git branch --show-current)
# Find the PR number for this branch
gh pr list --head "$BRANCH" --repo dotnet/macios --json number,url --jq '.[0]'
```

If no PR is found, inform the user that this skill requires a PR to exist for the current branch (so that CI has run).

### 2. Find the Azure DevOps build

The CI builds for PRs in dotnet/macios run in the `devdiv` Azure DevOps organization, project `DevDiv`.

Use the GitHub PR checks to find the Azure DevOps build URL:

```bash
gh pr checks <PR_NUMBER> --repo dotnet/macios
```

Look for a check that links to Azure DevOps. The build URL will look like:
```
https://devdiv.visualstudio.com/DevDiv/_build/results?buildId=XXXXXXX
```

Extract the `buildId` from the URL.

### 3. Download the artifacts

Use the Azure DevOps REST API to list and download artifacts:

```bash
# List artifacts for the build
TOKEN=$(az account get-access-token --resource 499b84ac-1321-427f-aa17-267ca6975798 --query accessToken -o tsv)
curl -s "https://devdiv.visualstudio.com/DevDiv/_apis/build/builds/{buildId}/artifacts?api-version=7.0" \
  -H "Authorization: Bearer $TOKEN"
```

Look for artifacts whose names contain `updated-expected-sizes` (e.g., `updated-expected-sizes-dotnettests_ios-1`). Get the artifact's `downloadUrl` and download it:

```bash
# Get the download URL for a specific artifact
ARTIFACT_INFO=$(curl -s "https://devdiv.visualstudio.com/DevDiv/_apis/build/builds/{buildId}/artifacts?artifactName={artifactName}&api-version=7.0" \
  -H "Authorization: Bearer $TOKEN")
DOWNLOAD_URL=$(echo "$ARTIFACT_INFO" | python3 -c "import sys,json; print(json.load(sys.stdin)['resource']['downloadUrl'])")

# Download the artifact zip
curl -sL "$DOWNLOAD_URL" -H "Authorization: Bearer $TOKEN" -o artifact.zip
```

If `az` is not available or not authenticated, direct the user to download manually from the Azure DevOps build artifacts page.

### 4. Place the files

Extract the downloaded artifact zip and apply each diff to the expected directory from the repository root:

```bash
unzip -o artifact.zip -d /tmp/updated-sizes/
# Each '*.txt.diff' is a unified diff relative to the repository root.
for diff in /tmp/updated-sizes/*/*.txt.diff; do
  git apply -p1 "$diff"
done
```

The diffs reference `tests/dotnet/UnitTests/expected/<name>.txt` directly, so applying them updates (or creates) the expected files in place. If a diff fails to apply cleanly (e.g., the expected file changed since the CI build), re-run the failing app size test locally with `WRITE_KNOWN_FAILURES=1` to regenerate the file (see the "Run Locally" fallback).

### 5. Verify and commit

After placing the files:
1. Run `git diff` to show what changed
2. Ask the user if the changes look correct
3. If confirmed, commit the changes:
   ```bash
   git add tests/dotnet/UnitTests/expected/
   git commit -m "[tests] Update expected app size files"
   ```

## Fallback: Manual Download

If automated download fails (auth issues, etc.), provide the user with:
1. The Azure DevOps build URL
2. Instructions to navigate to the build → Summary → Artifacts section
3. Look for individual artifacts whose names contain `updated-expected-sizes`
4. Download the artifact zip, extract it, and apply the `.txt.diff` files (e.g., `iOS-MonoVM-interpreter-size.txt.diff`) from the repository root with `git apply -p1 <file>`

## Fallback: Run Locally

If the user can build locally, they can update the expected files directly:

```bash
WRITE_KNOWN_FAILURES=1 tests-dotnet AppSizeTest
```

This runs the tests, updates the expected files in place, and marks the tests as passed.

