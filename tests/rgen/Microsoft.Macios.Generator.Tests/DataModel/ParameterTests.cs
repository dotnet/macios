// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using Microsoft.Macios.Generator.DataModel;
using Xunit;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Generator.Tests.DataModel;

public class ParameterTests {

	class TestDataNeedsNullCheckTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				new Parameter (0, ReturnTypeForBool (), "firstParameter"),
				false,
			];

			yield return [
				new Parameter (0, ReturnTypeForInt (), "firstParameter"),
				false,
			];

			yield return [
				new Parameter (0, ReturnTypeForInt (isNullable: true), "firstParameter"),
				false,
			];

			yield return [
				new Parameter (0, ReturnTypeForString (), "firstParameter"),
				true,
			];

			yield return [
				new Parameter (0, ReturnTypeForStruct ("MyStruct"), "firstParameter"),
				false,
			];

			yield return [
				new Parameter (0, ReturnTypeForClass ("MyClass"), "firstParameter") {
					ReferenceKind = ReferenceKind.Ref
				},
				false,
			];

			yield return [
				new Parameter (0, ReturnTypeForArray ("MyClass"), "firstParameter"),
				true,
			];

			yield return [
				new Parameter (0, ReturnTypeForInterface ("IMyClass"), "firstParameter"),
				true,
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataNeedsNullCheckTests))]
	void NeedsNullCheckTests (Parameter parameter, bool expectedNeedsNullCheck)
		=> Assert.Equal (expectedNeedsNullCheck, parameter.NeedsNullCheck);
}
