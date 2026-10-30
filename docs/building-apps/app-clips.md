---
title: App Clips for .NET for iOS
description: How to build and embed App Clips in .NET for iOS apps
ms.date: 10/22/2026
---

# App Clips

An [App Clip](https://developer.apple.com/app-clips/) is a small part of an
iOS app that can be launched without installing the full app. Technically an
App Clip is a complete app bundle (`.app`) embedded in the containing app at
`MyApp.app/AppClips/MyClip.app`.

App Clips are only supported on iOS.

## Creating an App Clip project

```
dotnet new ios-appclip -n MyClip --bundleId com.mycompany.myapp
```

or create an iOS executable project manually and set `IsAppClip`:

```xml
<PropertyGroup>
  <TargetFramework>net11.0-ios</TargetFramework>
  <OutputType>Exe</OutputType>
  <IsAppClip>true</IsAppClip>
  <!-- Must be the containing app's bundle identifier + ".<something>" -->
  <ApplicationId>com.mycompany.myapp.Clip</ApplicationId>
  <PublishAot Condition="'$(Configuration)' == 'Release'">true</PublishAot>
</PropertyGroup>
```

The App Clip's `Entitlements.plist` must contain
`com.apple.developer.parent-application-identifiers` with the containing
app's application identifier (including the team prefix), and the containing
app needs `com.apple.developer.associated-appclip-app-identifiers`.

## Embedding the App Clip

Reference the App Clip project from the containing app:

```xml
<ItemGroup>
  <ProjectReference Include="../MyClip/MyClip.csproj">
    <IsAppClip>true</IsAppClip>
  </ProjectReference>
</ItemGroup>
```

To embed an App Clip built by Xcode instead, use the
[AdditionalAppClips](build-items.md#additionalappclips) item group.

## Size

The App Store limits the size of an App Clip (10 MB for iOS 15 and earlier
targets, 15 MB for iOS 17+, more for clips only launched by digital invocation).
To stay within the limit, Release builds of an App Clip project require
NativeAOT (`PublishAot=true`); the build fails otherwise.

## Running and testing

* Build and run the App Clip project directly (`dotnet build -t:Run`) to
  launch it in the simulator or on a device.
* To simulate an invocation URL, set the `_XCAppClipURL` environment variable
  (for instance `RuntimeEnvironmentVariable`) when launching.
* On a device, use *Settings > Developer > Local Experiences* to register a
  local App Clip experience.
* Signing an App Clip requires its own App ID and provisioning profile.
