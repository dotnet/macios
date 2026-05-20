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

The app size tests (`tests/dotnet/UnitTests/AppSizeTest.cs`) compare the built app's size and preserved APIs against expected files stored in `tests/dotnet/UnitTests/expected/`. When the test detects a difference and `WRITE_KNOWN_FAILURES` is not set, it uploads the updated expected file as an Azure DevOps artifact named `updated-expected-sizes`.

Individual artifact names follow these patterns:
- `{Platform}-{Runtime}-size` — e.g., `iOS-MonoVM-size`, `MacCatalyst-NativeAOT-size`
- `{Platform}-{Runtime}-preservedapis` — e.g., `iOS-MonoVM-preservedapis`

The expected files on disk are at:
- `tests/dotnet/UnitTests/expected/{name}-size.txt`
- `tests/dotnet/UnitTests/expected/{name}-preservedapis.txt`

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
curl -s "https://devdiv.visualstudio.com/DevDiv/_apis/build/builds/{buildId}/artifacts?api-version=7.0" \
  -H "Authorization: Bearer $(az account get-access-token --resource 499b84ac-1321-427f-aa17-267ca6975798 --query accessToken -o tsv)"
```

Look for artifacts in the `updated-expected-sizes` container folder. Download each one:

```bash
# Download a specific artifact
curl -s "https://devdiv.visualstudio.com/DevDiv/_apis/build/builds/{buildId}/artifacts?artifactName={artifactName}&api-version=7.0&%24format=zip" \
  -H "Authorization: Bearer $(az account get-access-token --resource 499b84ac-1321-427f-aa17-267ca6975798 --query accessToken -o tsv)" \
  -o artifact.zip
```

If `az` is not available or not authenticated, direct the user to download manually from the Azure DevOps build artifacts page.

### 4. Place the files

Extract the downloaded artifacts and place them in the expected directory:

```bash
EXPECTED_DIR="tests/dotnet/UnitTests/expected"
```

Map artifact names to file names:
- Artifact `{name}-size` → file `{name}-size.txt`
- Artifact `{name}-preservedapis` → file `{name}-preservedapis.txt`

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
3. Look for individual artifacts whose names match the patterns above
4. Download each file and place it as `tests/dotnet/UnitTests/expected/{artifactName}.txt`

## Fallback: Run Locally

If the user can build locally, they can update the expected files directly:

```bash
WRITE_KNOWN_FAILURES=1 tests-dotnet AppSizeTest
```

This runs the tests, updates the expected files in place, and marks the tests as passed.

