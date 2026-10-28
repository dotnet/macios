// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CoreVideo;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreVideo;

[TestFixture]
[Preserve (AllMembers = true)]
public class CVTimeTest {
	[Test]
	public void ZeroTime ()
	{
		var zero = CVTime.ZeroTime;
		Assert.Multiple (() => {
			Assert.That (zero.TimeValue, Is.EqualTo (0), "TimeValue");
			Assert.That (zero.TimeScale, Is.GreaterThanOrEqualTo (0), "TimeScale");
			Assert.That (zero.TimeFlags, Is.EqualTo ((CVTimeFlags) 0), "TimeFlags");
			Assert.That (zero.Flags, Is.EqualTo (0), "Flags");
		});
	}

	[Test]
	public void IndefiniteTime ()
	{
		var indefinite = CVTime.IndefiniteTime;
		Assert.Multiple (() => {
			Assert.That (indefinite.TimeFlags, Is.EqualTo (CVTimeFlags.IsIndefinite), "TimeFlags");
			Assert.That (indefinite.Flags, Is.EqualTo ((int) CVTimeFlags.IsIndefinite), "Flags");
		});
	}

	[Test]
	public void PropertiesAndEquality ()
	{
		var t1 = new CVTime {
			TimeValue = 1200,
			TimeScale = 600,
			TimeFlags = (CVTimeFlags) 0,
		};
		var t2 = new CVTime {
			TimeValue = 1200,
			TimeScale = 600,
			Flags = 0,
		};
		var t3 = new CVTime {
			TimeValue = 2400,
			TimeScale = 600,
			Flags = (int) CVTimeFlags.IsIndefinite,
		};

		Assert.Multiple (() => {
			Assert.That (t1.Equals ((object) t2), Is.True, "Equals object true");
			Assert.That (t1.Equals ((object) t3), Is.False, "Equals object false");
			Assert.That (t1.Equals (null), Is.False, "Equals null");
			Assert.That (t1.GetHashCode (), Is.EqualTo (t2.GetHashCode ()), "GetHashCode");
			Assert.That (t3.TimeFlags, Is.EqualTo (CVTimeFlags.IsIndefinite), "TimeFlags from Flags");
		});
	}

#if !XAMCORE_5_0
	[Test]
	[UnconditionalSuppressMessage ("Trimming", "IL2026", Justification = "This test inspects internal CVTime2 conversion helpers via reflection.")]
	[UnconditionalSuppressMessage ("Trimming", "IL2075", Justification = "This test inspects internal CVTime2 conversion helpers via reflection.")]
	[DynamicDependency ("ToCVTime2", typeof (CVTime))]
#if __MACOS__
	[DynamicDependency (DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods, "CoreVideo.CVTime2", "Microsoft.macOS")]
#elif __TVOS__
	[DynamicDependency (DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods, "CoreVideo.CVTime2", "Microsoft.tvOS")]
#elif __MACCATALYST__
	[DynamicDependency (DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods, "CoreVideo.CVTime2", "Microsoft.MacCatalyst")]
#elif __IOS__
	[DynamicDependency (DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods, "CoreVideo.CVTime2", "Microsoft.iOS")]
#else
#error Unknown platform
#endif
	public void CVTime2Conversion ()
	{
		var cvTime2Type = typeof (NSObject).Assembly.GetType ("CoreVideo.CVTime2", throwOnError: true)!;
		var toCVTime2 = typeof (CVTime).GetMethod ("ToCVTime2", BindingFlags.Instance | BindingFlags.NonPublic);
		var toCVTime = cvTime2Type.GetMethod ("ToCVTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		Assert.That (toCVTime2, Is.Not.Null, "ToCVTime2");
		Assert.That (toCVTime, Is.Not.Null, "ToCVTime");

		var valid = new CVTime {
			TimeValue = 1234567890123L,
			TimeScale = 600,
			TimeFlags = CVTimeFlags.IsIndefinite,
		};
		var cvTime2 = toCVTime2!.Invoke (valid, null);
		Assert.That (cvTime2, Is.Not.Null, "cvTime2");
		var roundtrip = (CVTime) toCVTime!.Invoke (cvTime2, null)!;
		Assert.That (roundtrip, Is.EqualTo (valid), "Roundtrip");

		var tooLarge = new CVTime {
			TimeValue = 1,
			TimeScale = (long) int.MaxValue + 1,
		};
		var ex1 = Assert.Throws<TargetInvocationException> (() => toCVTime2.Invoke (tooLarge, null));
		Assert.That (ex1?.InnerException, Is.TypeOf<ArgumentOutOfRangeException> (), "TooLarge");

		var tooSmall = new CVTime {
			TimeValue = 1,
			TimeScale = (long) int.MinValue - 1,
		};
		var ex2 = Assert.Throws<TargetInvocationException> (() => toCVTime2.Invoke (tooSmall, null));
		Assert.That (ex2?.InnerException, Is.TypeOf<ArgumentOutOfRangeException> (), "TooSmall");
	}
#endif
}
