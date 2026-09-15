// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

global using System.IO;
global using System.Runtime.InteropServices;

global using Mono.Cecil;
global using NUnit.Framework;

global using Xamarin;
global using Xamarin.Bundler;
global using Xamarin.Build;
global using Xamarin.Tests;
global using Xamarin.Utils;

// These tests are rather memory hungry, so running them in parallel really makes my machine crawl.
// [assembly: Parallelizable (ParallelScope.Children)]
