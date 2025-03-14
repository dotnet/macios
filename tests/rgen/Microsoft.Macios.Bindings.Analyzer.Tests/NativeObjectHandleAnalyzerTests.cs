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

public class NativeObjectHandleAnalyzerTests : BaseGeneratorWithAnalyzerTestClass {
	class ErrorTestCases : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// Method with misuse
			yield return [
				"""
				using ObjCRuntime;
				using Foundation;

				class Test
				{
					void Method(NSUuid foo) { _ = foo.Handle; }
				}
				"""];

			// Method with misuse of INativeObject itself
			yield return [
				"""
				using ObjCRuntime;
				using Foundation;

				class Test
				{
					void Method(NSUuid foo) { _ = foo.Handle; }
				}
				"""];

			// Method with misuse of INativeObject generic parameter
			yield return [
				"""
				using ObjCRuntime;

				class Test<T> where T : INativeObject
				{
					void Method(T foo) { _ = foo.Handle; }
				}
				"""];

			// Expression with misuse
			yield return [
				"""
				using ObjCRuntime;
				using Foundation;

				class Test
				{
					void Method(NSUuid foo) => _ = foo.GetNonNullHandle();
				}
				"""];

			// Constructor with misuse
			yield return [
				"""
				using ObjCRuntime;
				using Foundation;

				class BaseTest
				{
					protected BaseTest(NativeHandle handle) { }
				}

				class Test : BaseTest
				{
					Test(NSUuid foo) : base(foo.Handle) { }
				}
				"""];

			// Constructor expression with misuse
			yield return [
				"""
				using ObjCRuntime;
				using Foundation;

				class BaseTest
				{
					protected BaseTest(NativeHandle handle) { }
				}

				class Test : BaseTest
				{
					Test(NSUuid foo) : base(foo.Handle) => {}
				}
				"""];

			// Method call with handle access and no intermediate local variable
			yield return [
				"""
				using ObjCRuntime;
				using CoreFoundation;

				class Test
				{
					void Method()
					{
						_ = CFArray.FromStrings().GetHandle();
					}
				}
				"""];

			// Constructor with handle access and no intermediate local variable
			yield return [
				"""
				using ObjCRuntime;
				using Foundation;

				class Test
				{
					void Method()
					{
						_ = (new NSUuid(new byte[16])).Handle;
					}
				}
				"""];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	class NoErrorTestCases : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"""
				using ObjCRuntime;
				using Foundation;

				class Test
				{
					void Method(NSUuid foo) { _ = foo.Handle; GC.KeepAlive(foo); }
				}
				"""];

			// Calling this.Handle is okay
			yield return [
				"""
				using ObjCRuntime;

				class Test : INativeObject
				{
					NativeHandle Handle { get; }
					void Method() { _ = this.Handle; }
				}
				"""];

			// Guard by a using block
			yield return [
				"""
				using ObjCRuntime;
				using Foundation;

				class Test
				{
					void Method()
					{
						using (NSString foo = new NSString("foo"))
							_ = foo.Handle;

						using (NSString bar = new NSString("foo")) {
							_ = bar.Handle;
						}

						NSString foo2 = new NSString("foo");
						using (foo2)
							_ = foo2.Handle;

						NSString bar2 = new NSString("foo");
						using (bar2) {
							_ = bar2.Handle;
						}

						using var foo3 = new NSString("foo");
						_ = foo3.Handle;
					}
				}
				"""];

			// Constructor with GC.KeepAlive in body
			yield return [
				"""
				using ObjCRuntime;
				using Foundation;

				class BaseTest
				{
					protected BaseTest(NativeHandle handle) { }
				}

				class Test : BaseTest
				{
					Test(NSString foo) : base(foo.Handle)
					{
						GC.KeepAlive(foo);
					}
				}
				"""];

			yield return [
				"""
				using ObjCRuntime;
				using Foundation;

				class Test
				{
					void Method(NSString foo) { _ = foo.DangerousRetain().DangerousAutorelease().Handle; }
				}
				"""];

			yield return [
				"""
				using ObjCRuntime;
				using Foundation;

				class Test
				{
					void Method(NSString foo) { _ = foo.GetConstant().Handle; }
				}
				"""];

			// Equals implementation
			yield return [
				"""
				using ObjCRuntime;

				class Test : INativeObject
				{
					NativeHandle Handle { get; }
					bool Equals(Test other) { return this.Handle == other.Handle; }
				}
				"""];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<ErrorTestCases>]
	public async Task GCHoleTests (ApplePlatform platform, string inputText)
	{
		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new NativeObjectHandleAnalyzer (), compilation);
		var analyzerDiagnotics = diagnostics.Where (d => d.Id == "RBI0014").ToArray ();
		Assert.Single (analyzerDiagnotics);
	}

	[Theory]
	[AllSupportedPlatformsClassData<NoErrorTestCases>]
	public async Task NoGCHoleTests (ApplePlatform platform, string inputText)
	{
		var (compilation, _) = CreateCompilation (platform, sources: inputText);
		var diagnostics = await RunAnalyzer (new NativeObjectHandleAnalyzer (), compilation);
		var analyzerDiagnotics = diagnostics.Where (d => d.Id == "RBI0014").ToArray ();
		Assert.Empty (analyzerDiagnotics);
	}
}
