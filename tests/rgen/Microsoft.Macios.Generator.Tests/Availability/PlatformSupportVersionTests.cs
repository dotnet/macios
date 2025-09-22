// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Microsoft.Macios.Generator.Availability;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.Availability;

public class PlatformSupportVersionTests {
	[Fact]
	public void ImplicitDefaultTest ()
	{
		var implicitDefault = PlatformSupportVersion.ImplicitDefault;
		Assert.Equal (new Version (0, 0), implicitDefault.Version);
		Assert.Equal (SupportKind.Implicit, implicitDefault.Kind);
	}

	[Fact]
	public void ExplicitDefaultTest ()
	{
		var explicitDefault = PlatformSupportVersion.ExplicitDefault;
		Assert.Equal (new Version (0, 0), explicitDefault.Version);
		Assert.Equal (SupportKind.Explicit, explicitDefault.Kind);
	}

	public static TheoryData<PlatformSupportVersion, PlatformSupportVersion, PlatformSupportVersion> MaxTestData =>
		new () {
			// Same kind, v1 > v2
			{
				new PlatformSupportVersion { Version = new Version (2, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (2, 0), Kind = SupportKind.Implicit }
			},
			// Same kind, v2 > v1
			{
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (2, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (2, 0), Kind = SupportKind.Implicit }
			},
			// Same kind, v1 == v2
			{
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit }
			},
			// Different kind, v1.Kind > v2.Kind
			{
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Explicit },
				new PlatformSupportVersion { Version = new Version (2, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Explicit }
			},
			// Different kind, v2.Kind > v1.Kind
			{
				new PlatformSupportVersion { Version = new Version (2, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Explicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Explicit }
			},
			// ExplicitDefault vs ImplicitDefault
			{
				PlatformSupportVersion.ExplicitDefault,
				PlatformSupportVersion.ImplicitDefault,
				PlatformSupportVersion.ExplicitDefault
			},
			// ExplicitDefault vs other Implicit
			{
				PlatformSupportVersion.ExplicitDefault,
				new PlatformSupportVersion { Version = new Version (10, 0), Kind = SupportKind.Implicit },
				PlatformSupportVersion.ExplicitDefault
			},
			// ImplicitDefault vs other Implicit
			{
				PlatformSupportVersion.ImplicitDefault,
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit }
			},
			// ExplicitDefault vs other Explicit
			{
				PlatformSupportVersion.ExplicitDefault,
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Explicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Explicit }
			},
		};

	[Theory]
	[MemberData (nameof (MaxTestData))]
	public void MaxTests (PlatformSupportVersion v1, PlatformSupportVersion v2, PlatformSupportVersion expected)
	{
		Assert.Equal (expected, PlatformSupportVersion.Max (v1, v2));
		// Test commutativity
		Assert.Equal (expected, PlatformSupportVersion.Max (v2, v1));
	}

	public static TheoryData<PlatformSupportVersion, PlatformSupportVersion, PlatformSupportVersion> MinTestData =>
		new () {
			// Same kind, v1 < v2
			{
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (2, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit }
			},
			// Same kind, v2 < v1
			{
				new PlatformSupportVersion { Version = new Version (2, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit }
			},
			// Same kind, v1 == v2
			{
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit }
			},
			// Different kind, v1.Kind > v2.Kind
			{
				new PlatformSupportVersion { Version = new Version (2, 0), Kind = SupportKind.Explicit },
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (2, 0), Kind = SupportKind.Explicit }
			},
			// Different kind, v2.Kind > v1.Kind
			{
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit },
				new PlatformSupportVersion { Version = new Version (2, 0), Kind = SupportKind.Explicit },
				new PlatformSupportVersion { Version = new Version (2, 0), Kind = SupportKind.Explicit }
			},
			// ExplicitDefault vs ImplicitDefault
			{
				PlatformSupportVersion.ExplicitDefault,
				PlatformSupportVersion.ImplicitDefault,
				PlatformSupportVersion.ExplicitDefault
			},
			// ExplicitDefault vs other Implicit
			{
				PlatformSupportVersion.ExplicitDefault,
				new PlatformSupportVersion { Version = new Version (10, 0), Kind = SupportKind.Implicit },
				PlatformSupportVersion.ExplicitDefault
			},
			// ImplicitDefault vs other Implicit
			{
				PlatformSupportVersion.ImplicitDefault,
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Implicit },
				PlatformSupportVersion.ImplicitDefault
			},
			// ExplicitDefault vs other Explicit
			{
				PlatformSupportVersion.ExplicitDefault,
				new PlatformSupportVersion { Version = new Version (1, 0), Kind = SupportKind.Explicit },
				PlatformSupportVersion.ExplicitDefault
			},
		};

	[Theory]
	[MemberData (nameof (MinTestData))]
	public void MinTests (PlatformSupportVersion v1, PlatformSupportVersion v2, PlatformSupportVersion expected)
	{
		Assert.Equal (expected, PlatformSupportVersion.Min (v1, v2));
		// Test commutativity
		Assert.Equal (expected, PlatformSupportVersion.Min (v2, v1));
	}
}
