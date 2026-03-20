// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Bindings.Analyzer.Tests;

public class TransientDisposableAnalyzerTests : BaseGeneratorWithAnalyzerTestClass {
	class ErrorTestCases : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// TransientString without using
			yield return [
				"""
				using System;

				namespace ObjCRuntime {
					struct TransientString : IDisposable {
						public void Dispose () { }
					}
				}

				class Test
				{
					void Method ()
					{
						var s = new ObjCRuntime.TransientString ();
					}
				}
				"""];

			// TransientCFString without using
			yield return [
				"""
				using System;

				namespace ObjCRuntime {
					ref struct TransientCFString {
						public void Dispose () { }
					}
				}

				class Test
				{
					void Method ()
					{
						ObjCRuntime.TransientCFString s = new ObjCRuntime.TransientCFString ();
					}
				}
				"""];

			// TransientCFObject without using
			yield return [
				"""
				using System;

				namespace ObjCRuntime {
					ref struct TransientCFObject {
						public void Dispose () { }
					}
				}

				class Test
				{
					void Method ()
					{
						var obj = new ObjCRuntime.TransientCFObject ();
					}
				}
				"""];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	class NoErrorTestCases : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// TransientString with using var
			yield return [
				"""
				using System;

				namespace ObjCRuntime {
					struct TransientString : IDisposable {
						public void Dispose () { }
					}
				}

				class Test
				{
					void Method ()
					{
						using var s = new ObjCRuntime.TransientString ();
					}
				}
				"""];

			// TransientCFString with using var
			yield return [
				"""
				using System;

				namespace ObjCRuntime {
					ref struct TransientCFString {
						public void Dispose () { }
					}
				}

				class Test
				{
					void Method ()
					{
						using var s = new ObjCRuntime.TransientCFString ();
					}
				}
				"""];

			// TransientCFObject with using var
			yield return [
				"""
				using System;

				namespace ObjCRuntime {
					ref struct TransientCFObject {
						public void Dispose () { }
					}
				}

				class Test
				{
					void Method ()
					{
						using var obj = new ObjCRuntime.TransientCFObject ();
					}
				}
				"""];

			// Non-transient type without using is fine
			yield return [
				"""
				using System;

				struct SomeOtherStruct : IDisposable {
					public void Dispose () { }
				}

				class Test
				{
					void Method ()
					{
						var s = new SomeOtherStruct ();
					}
				}
				"""];

			// User-defined TransientString outside ObjCRuntime namespace should not trigger
			yield return [
				"""
				using System;

				struct TransientString : IDisposable {
					public void Dispose () { }
				}

				class Test
				{
					void Method ()
					{
						var s = new TransientString ();
					}
				}
				"""];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<ErrorTestCases>]
	public async Task TransientWithoutUsingTests (ApplePlatform platform, string inputText)
	{
		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new TransientDisposableAnalyzer (), compilation);
		var analyzerDiagnostics = diagnostics.Where (d => d.Id == "RBI0042").ToArray ();
		Assert.True (analyzerDiagnostics.Length != 0);
	}

	[Theory]
	[AllSupportedPlatformsClassData<NoErrorTestCases>]
	public async Task TransientWithUsingTests (ApplePlatform platform, string inputText)
	{
		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new TransientDisposableAnalyzer (), compilation);
		var analyzerDiagnostics = diagnostics.Where (d => d.Id == "RBI0042").ToArray ();
		Assert.Empty (analyzerDiagnostics);
	}
}
