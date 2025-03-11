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

				class Test
				{
					void Method(Class foo) { _ = foo.Handle; }
				}
				"""];

			// Method with misuse of INativeObject itself
			yield return [
				"""
				using ObjCRuntime;

				class Test
				{
					void Method(INativeObject foo) { _ = foo.Handle; }
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

				class Test
				{
					void Method(Class foo) => _ = foo.Handle;
				}
				"""];

			// Constructor with misuse
			yield return [
				"""
				using ObjCRuntime;

				class BaseTest
				{
					protected BaseTest(NativeHandle handle) { }
				}

				class Test : BaseTest
				{
					Test(Class foo) : base(foo.Handle) { }
				}
				"""];

			// Constructor expression with misuse
			yield return [
				"""
				using ObjCRuntime;

				class BaseTest
				{
					protected BaseTest(NativeHandle handle) { }
				}

				class Test : BaseTest
				{
					Test(Class foo) : base(foo.Handle) => {}
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
						_ = CFArray.FromStrings().Handle;
					}
				}
				"""];

			// Constructor with handle access and no intermediate local variable
			yield return [
				"""
				using ObjCRuntime;

				class Test
				{
					void Method()
					{
						_ = (new Class("foo")).Handle;
					}
				}
				"""];

			// TODO: Test using INativeObject itself
			// TODO: Test GetHandle and other methods
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	class NoErrorTestCases : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"""
				using ObjCRuntime;

				class Test
				{
					void Method(Class foo) { _ = foo.Handle; GC.KeepAlive(foo); }
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

				class Test
				{
					void Method()
					{
						using (Class foo = new Class("foo"))
							_ = foo.Handle;
						Class foo2 = new Class("foo");
						using (foo2)
							_ = foo2.Handle;
						using var foo3 = new Class("foo");
						_ = foo3.Handle;
					}
				}
				"""];

			// Constructor with GC.KeepAlive in body
			yield return [
				"""
				using ObjCRuntime;

				class BaseTest
				{
					protected BaseTest(NativeHandle handle) { }
				}

				class Test : BaseTest
				{
					Test(Class foo) : base(foo.Handle)
					{
						GC.KeepAlive(foo);
					}
				}
				"""];

			yield return [
				"""
				using ObjCRuntime;

				class Test
				{
					void Method(Class foo) { _ = foo.DangerousRetain().DangerousAutorelease().Handle; }
				}
				"""];

			// TODO: Test ThrowOnNull, GetConstant
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
