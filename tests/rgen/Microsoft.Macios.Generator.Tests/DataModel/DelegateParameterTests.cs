// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections;
using System.Collections.Generic;
using Microsoft.Macios.Generator.DataModel;
using Xunit;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Generator.Tests.DataModel;

public class DelegateParameterTests {

	class TestDataEquals : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// diff pos
			yield return [
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
				new DelegateParameter (
					position: 1,
					type: ReturnTypeForString (),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
			];

			// diff type
			yield return [
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForInt (),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
			];

			// diff name
			yield return [
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (),
					name: "arg2"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
			];

			// diff blittable 
			yield return [
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForStruct ("MyStruct", isBlittable: true),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForStruct ("MyStruct", isBlittable: false),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
			];

			// diff optional
			yield return [
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (),
					name: "arg1"
				) {
					IsOptional = true,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
			];

			// diff is params
			yield return [
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = true,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
			];

			// diff is this
			yield return [
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = true,
					ReferenceKind = ReferenceKind.None,
				},
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
			];

			// diff is nullable
			yield return [
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (isNullable: true),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
			];

			// diff ref type
			yield return [
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.In,
				},
				new DelegateParameter (
					position: 0,
					type: ReturnTypeForString (),
					name: "arg1"
				) {
					IsOptional = false,
					IsParams = false,
					IsThis = false,
					ReferenceKind = ReferenceKind.None,
				},
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataEquals))]
	void CompareDiffPosition (DelegateParameter x, DelegateParameter y)
	{
		Assert.False (x.Equals (y));
		Assert.False (y.Equals (x));
		Assert.False (x == y);
		Assert.True (x != y);
	}

}
