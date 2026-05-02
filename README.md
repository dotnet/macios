# .NET for iOS, Mac Catalyst, macOS, tvOS #

## Welcome!

This module is the main repository for:
- **.NET for iOS**
- **.NET for Mac Catalyst**
- **.NET for macOS**
- **.NET for tvOS**

These SDKs allow us to create native iOS, Mac Catalyst, macOS and tvOS applications using the same UI controls we would in Objective-C and Xcode, except with the flexibility and elegance of a modern language (C#), the power of the .NET Base Class Library (BCL), and first-class IDEs and editors&mdash;Visual Studio and Visual Studio Code&mdash;at our fingertips.

This repository is where we do development for the .NET for iOS, Mac Catalyst, macOS, and tvOS SDKs. .NET for iOS, Mac Catalyst, macOS, and tvOS are part of [.NET MAUI][maui-intro], and may also be used independently for native iOS and macOS development with .NET.

There are a few ways that you can contribute, for example:

- [Submit bugs and feature requests](https://github.com/dotnet/macios/wiki/Submitting-Bugs-&-Suggestions)
- [Review source code changes](https://github.com/dotnet/macios/pulls)
- [Submit pull requests](https://github.com/dotnet/macios/wiki/How-to-Contribute#pull-requests) to resolve issues and fix bugs

[maui-intro]: https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui

## Support

.NET for iOS, Mac Catalyst, macOS, and tvOS are part of .NET MAUI, since it was introduced in May 2022 as part of .NET 6, and is currently supported as described on the [.NET MAUI Support Policy][maui-support-policy].

Support for Xamarin.iOS and Xamarin.Mac ended on **May 1, 2024** as per the [Xamarin Support Policy][xamarin-support-policy]:

> Xamarin support ended on May 1, 2024 for all Xamarin SDKs including Xamarin.Forms. Android API 34 and Xcode 15 SDKs (iOS and iPadOS 17, macOS 14) are the final versions Xamarin targets from existing Xamarin SDKs (i.e. no new APIs are planned).

[maui-support-policy]: https://dotnet.microsoft.com/en-us/platform/support/policy/maui
[xamarin-support-policy]: https://dotnet.microsoft.com/en-us/platform/support/policy/xamarin

## Contributing

If you are interested in fixing issues and contributing directly to the code base, please see the document [How to Contribute](https://github.com/dotnet/macios/wiki/How-to-Contribute), which covers the following:

- How to [build and run](https://github.com/dotnet/macios/wiki/Build-&-Run) from source
- The [development workflow](https://github.com/dotnet/macios/wiki/How-to-Contribute#work-branches), including [debugging](https://github.com/dotnet/macios/wiki/Build-&-Run#debugging-applications-from-source) and [running tests](https://github.com/dotnet/macios/blob/main/tests/README.md)
- [Coding Guidelines](https://github.com/dotnet/macios/wiki/How-to-Contribute#coding-guidelines)
- [Submitting pull requests](https://github.com/dotnet/macios/wiki/How-to-Contribute#pull-requests)

## Downloads

Install the .NET workloads for Apple platforms using the .NET CLI:

```sh
dotnet workload install ios macos tvos maccatalyst
```

For more information, see the [.NET workload documentation][workload-docs].

For legacy Xamarin.iOS and Xamarin.Mac downloads (discontinued), see [Downloads](DOWNLOADS.md).

[workload-docs]: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-workload-install

## Feedback

- [File an issue or ask a question](https://github.com/dotnet/macios/issues) on GitHub
- [Request a new feature](https://github.com/dotnet/macios/wiki/Submitting-Bugs-&-Suggestions#writing-good-bug-reports-and-feature-requests) on GitHub
- [Vote on existing feature requests](https://github.com/dotnet/macios/wiki/Submitting-Bugs-&-Suggestions#before-submitting-an-issue)
- [Submit bugs to GitHub Issues](https://github.com/dotnet/macios/wiki/Submitting-Bugs-&-Suggestions)
- Discuss development and design on [Discord](https://discord.gg/Gw6cvsq)

[![Discord](https://img.shields.io/discord/732297728826277939.svg?label=Join%20chat&logo=discord&logoColor=ffffff&color=7389D8&labelColor=6A7EC2)](https://discord.gg/Gw6cvsq)

## License

Copyright (c) .NET Foundation Contributors. All rights reserved.
Licensed under the [MIT](https://github.com/dotnet/macios/blob/main/LICENSE) License.
