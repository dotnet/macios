# Contributing to .NET for iOS, Mac Catalyst, macOS, and tvOS

Thank you for your interest in contributing to .NET for iOS, Mac Catalyst, macOS, and tvOS! This document provides guidelines and information for contributors.

## Quick Links

- [How to Build and Run from Source](https://github.com/dotnet/macios/wiki/Build-&-Run)
- [Development Workflow](https://github.com/dotnet/macios/wiki/How-to-Contribute#work-branches)
- [Coding Guidelines](https://github.com/dotnet/macios/wiki/How-to-Contribute#coding-guidelines)
- [Submitting Pull Requests](https://github.com/dotnet/macios/wiki/How-to-Contribute#pull-requests)
- [Submitting Bugs & Feature Requests](https://github.com/dotnet/macios/wiki/Submitting-Bugs-&-Suggestions)

## Pull Request Guidelines

### Writing Good PR Titles

Your pull request title is important because it will appear in the automatically generated release notes. Please make your titles descriptive and clear:

**Good examples:**
- `[iOS] Fix crash in UITableView when using custom cells`
- `Add support for Xcode 15.2 and iOS 17.3`
- `[macOS] Improve memory management in NSViewController bindings`

**Avoid:**
- `Fix issue`
- `Update code`
- `Various changes`

### Using Labels

Help maintainers categorize your PR by applying appropriate labels:

- **Platform labels**: `area-ios`, `area-macos`, `area-tvos`, `area-maccatalyst`
- **Component labels**: `area-bindings`, `area-tools`, `area-build`, `area-msbuild`
- **Type labels**: `bug`, `feature`, `enhancement`, `documentation`

### Linking Issues

When your PR fixes an issue, include "Fixes #123" in the description to automatically link and close the issue.

## Release Notes Process

This repository uses automated release notes generation that follows the same approach as [dotnet/android](https://github.com/dotnet/android/releases).

### How It Works

1. **Continuous Draft**: A draft release is automatically maintained with the latest changes
2. **Auto-generation**: When a release/tag is created, comprehensive release notes are automatically generated
3. **PR Integration**: Your PR title and labels determine how changes appear in release notes

### For Contributors

To ensure your changes are properly represented in release notes:

1. **Use descriptive PR titles** - These become line items in release notes
2. **Apply appropriate labels** - These help categorize changes
3. **Reference issues** - Use "Fixes #123" to link related issues

### Categories in Release Notes

Your PRs will be automatically categorized based on labels:

- 🚀 **New Features**: `feature`, `enhancement`, `area-bindings`
- 🐛 **Bug Fixes**: `bug`, `bugfix`, `regression`
- 📱 **Platform Support**: `area-ios`, `area-macos`, `area-tvos`, `area-maccatalyst`
- 🛠️ **Tools & Infrastructure**: `area-tools`, `area-build`, `area-ci`
- 🧰 **Dependencies & Maintenance**: `dependencies`, `maintenance`, `area-msbuild`
- 📚 **Documentation**: `documentation`, `area-docs`
- 🔒 **Security**: `security`

For more details, see [Release Notes Documentation](docs/guides/RELEASE_NOTES.md).

## Building and Testing

Please refer to the [Build & Run documentation](https://github.com/dotnet/macios/wiki/Build-&-Run) for detailed instructions on:

- Setting up your development environment
- Building from source
- Running tests
- Debugging applications

## Code Style

- Follow the existing code style in the files you're modifying
- Use tabs for indentation in C# files
- Follow the formatting defined in `.editorconfig`
- Keep diffs as small as possible

## Additional Resources

- [Wiki Home](https://github.com/dotnet/macios/wiki)
- [Test Documentation](tests/README.md)
- [API Documentation](https://docs.microsoft.com/dotnet/api/?view=net-ios-latest)

## Questions?

If you have questions about contributing, please:

1. Check the [Wiki](https://github.com/dotnet/macios/wiki)
2. Search existing [Issues](https://github.com/dotnet/macios/issues)
3. Ask in [Discussions](https://github.com/dotnet/macios/discussions)

Thank you for contributing to .NET for iOS, Mac Catalyst, macOS, and tvOS!