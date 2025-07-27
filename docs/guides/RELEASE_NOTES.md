# Release Notes Process

This document describes the automated release notes generation process for the dotnet/macios repository.

## Overview

We've implemented automated release notes generation that follows the same approach as [dotnet/android](https://github.com/dotnet/android/releases). The system automatically creates comprehensive release notes when new releases or tags are created.

## How It Works

### Automatic Generation

The release notes are automatically generated through two GitHub Actions workflows:

1. **Release Drafter** (`.github/workflows/release-drafter.yml`)
   - Runs on pushes to `main` and `release/*` branches
   - Continuously maintains a draft release with the latest changes
   - Auto-labels pull requests based on title and content

2. **Release Notes Generator** (`.github/workflows/release-notes.yml`)
   - Triggered when tags matching `xamarin-*` or `v*` patterns are pushed
   - Triggered when releases are created or published
   - Can be manually triggered via workflow dispatch

### Release Notes Format

The generated release notes follow a consistent format:

```markdown
## What's Changed

### Changes since [previous-tag]

- [PR Title] by @[Author] (#[PR Number])
- [Another PR Title] by @[Author] (#[PR Number])

**Full Changelog**: https://github.com/dotnet/macios/compare/[previous-tag]..[current-tag]
```

## For Maintainers

### Creating a Release

1. **Tag-based releases** (Recommended):
   ```bash
   git tag xamarin-mac-9.4.0.1
   git push origin xamarin-mac-9.4.0.1
   ```
   The release notes will be automatically generated and a GitHub release created.

2. **Manual release creation**:
   - Create a release through GitHub UI
   - The workflow will automatically populate the release notes

3. **Workflow dispatch** (For testing or corrections):
   - Go to Actions → Generate Release Notes → Run workflow
   - Provide the tag name manually

### Customizing Release Notes

#### Pull Request Labels

The system recognizes these labels for categorization:

- `feature`, `enhancement` → 🚀 Features
- `bug`, `bugfix` → 🐛 Bug Fixes  
- `maintenance`, `dependencies` → 🧰 Maintenance
- `documentation` → 📚 Documentation
- `security` → 🔒 Security

#### Configuration

Release notes behavior can be customized in `.github/release-drafter.yml`:

- **Categories**: Add/modify the labels and categories
- **Template**: Change the release notes format
- **Version resolution**: Modify how versions are determined from labels

### Manual Editing

After automatic generation, maintainers can:

1. Edit the release notes directly in GitHub
2. Add highlights or important announcements
3. Include breaking changes or migration notes
4. Add contributor callouts

## Best Practices

### For Contributors

1. **Use descriptive PR titles** - These become the line items in release notes
2. **Apply appropriate labels** - Helps categorize changes properly
3. **Reference issues** - Use "Fixes #123" to link related issues

### For Maintainers

1. **Review draft releases** - Check the continuously updated draft before creating releases
2. **Add context** - Include important context or breaking changes manually
3. **Consistent tagging** - Follow the existing tag naming convention (`xamarin-mac-X.Y.Z.W`)

## Configuration Files

- `.github/release-drafter.yml` - Release drafter configuration
- `.github/workflows/release-drafter.yml` - Continuous draft updates workflow
- `.github/workflows/release-notes.yml` - Tag-triggered release notes generation

## Troubleshooting

### Common Issues

1. **Missing PRs in release notes**:
   - Ensure PR titles contain issue/PR references (#123)
   - Check that commits between tags include merge commits

2. **Incorrect previous tag detection**:
   - Manually run the workflow with workflow_dispatch
   - Verify tag naming follows expected patterns

3. **Permission errors**:
   - Workflow requires `contents: write` and `pull-requests: read` permissions

### Manual Intervention

If the automation fails, you can:

1. Generate release notes manually using the GitHub UI
2. Run the workflow dispatch with specific parameters
3. Edit existing release notes directly

## Examples

See [dotnet/android releases](https://github.com/dotnet/android/releases) for the expected format and style that this automation aims to match.