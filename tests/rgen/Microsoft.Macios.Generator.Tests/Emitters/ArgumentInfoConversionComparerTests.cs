// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Emitters;
using Xunit;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class ArgumentInfoConversionComparerTests {

	public static IEnumerable<object []> CompareTestData ()
	{
		var arg1 = new ArgumentInfo (new Parameter (1, ReturnTypeForNSObject (isNullable: false), "p1"));
		var arg2 = new ArgumentInfo (new Parameter (2, ReturnTypeForINativeObject ("MyNativeObject", isNullable: false), "p2"));
		var arg3 = new ArgumentInfo (new Parameter (3, ReturnTypeForString (isNullable: false), "p3"));
		var arg4 = new ArgumentInfo (new Parameter (4, ReturnTypeForInt (), "p4")) { BindAs = new () };
		var arg5 = new ArgumentInfo (new Parameter (5, ReturnTypeForString (isNullable: true), "p5"));
		var arg6 = new ArgumentInfo (new Parameter (6, ReturnTypeForInt (), "p6"));

		var arg7 = new ArgumentInfo (new Parameter (1, ReturnTypeForNSObject (isNullable: false), "p7"));
		var arg8 = new ArgumentInfo (new Parameter (2, ReturnTypeForNSObject (isNullable: false), "p8"));

		yield return [
			new List<ArgumentInfo> { arg6, arg3, arg1, arg5, arg2, arg4 },
			new List<ArgumentInfo> { arg1, arg2, arg3, arg4, arg5, arg6 }
		];

		yield return [
			new List<ArgumentInfo> { arg8, arg7 },
			new List<ArgumentInfo> { arg7, arg8 }
		];
	}

	[Theory]
	[MemberData (nameof (CompareTestData))]
	void Compare (List<ArgumentInfo> unsorted, List<ArgumentInfo> expected)
	{
		var comparer = new ArgumentInfoConversionComparer ();
		unsorted.Sort (comparer);
		Assert.Equal (expected, unsorted);
	}
}
