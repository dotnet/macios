// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using Foundation;
using ObjCRuntime;

using Bindings.Test;
using NUnit.Framework;

using Xamarin.Utils;
using XamarinTests.ObjCRuntime;

namespace MonoTouchFixtures.ObjCRuntime {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class GenericRegistrationPerformanceTest {
		class Generic<T> : NSObject where T : NSObject {
			[MethodImpl (MethodImplOptions.NoInlining)]
			public override nuint GetNativeHash () => 42;
		}

		sealed class Closed : Generic<Closed> { }

		sealed class Baseline : NSObject {
			[MethodImpl (MethodImplOptions.NoInlining)]
			public override nuint GetNativeHash () => 42;
		}

		static long Measure (IntPtr handle, IntPtr selector, int iterations)
		{
			var watch = Stopwatch.StartNew ();
			for (var i = 0; i < iterations; i++)
				Messaging.IntPtr_objc_msgSend (handle, selector);
			watch.Stop ();
			return watch.ElapsedTicks;
		}

		[Test]
		public void ClosedGenericNativeDispatch ()
		{
			// Managed registrars use proxy calls instead of the native registrar's generic method lookup.
			if (RegistrarSharedTest.CurrentRegistrar != Registrars.Static)
				Assert.Ignore ("This test measures generic method lookup in the native static registrar.");

			AppContext.TryGetSwitch ("ObjCRuntime.Runtime.HotReloadCompatible", out var hotReloadCompatible);
			if (TestRuntime.IsOptimizeAll && hotReloadCompatible && !TestRuntime.IsLinkAll)
				Assert.Ignore ("Generic registrations in reloadable assemblies are not specialized.");

			using var closed = new Closed ();
			using var baseline = new Baseline ();
			var selector = Selector.GetHandle ("hash");
			var closedHandle = (IntPtr) closed.Handle;
			var baselineHandle = (IntPtr) baseline.Handle;
			Assert.That (Messaging.IntPtr_objc_msgSend (closedHandle, selector), Is.EqualTo ((IntPtr) 42), "Closed hash");
			Assert.That (Messaging.IntPtr_objc_msgSend (baselineHandle, selector), Is.EqualTo ((IntPtr) 42), "Baseline hash");

			Measure (closedHandle, selector, 10000);
			Measure (baselineHandle, selector, 10000);
			var iterations = 1000;
			while (Measure (baselineHandle, selector, iterations) < Stopwatch.Frequency / 20 && iterations < 1000000)
				iterations *= 2;

			const int samples = 9;
			var ratios = new double [samples];
			for (var sample = 0; sample < samples; sample++) {
				long closedTicks;
				long baselineTicks;
				if (sample % 2 == 0) {
					baselineTicks = Measure (baselineHandle, selector, iterations);
					closedTicks = Measure (closedHandle, selector, iterations);
				} else {
					closedTicks = Measure (closedHandle, selector, iterations);
					baselineTicks = Measure (baselineHandle, selector, iterations);
				}
				ratios [sample] = (double) closedTicks / baselineTicks;
			}
			Array.Sort (ratios);
			var ratio = ratios [samples / 2];
			TestContext.WriteLine ($"Closed generic / non-generic native dispatch: {ratio:F2}x (all optimizations: {TestRuntime.IsOptimizeAll}, {iterations} calls per sample)");

			if (TestRuntime.IsOptimizeAll) {
				Assert.That (ratio, Is.LessThan (1.2), "Specialized dispatch should be close to the non-generic baseline.");
			} else {
				Assert.That (ratio, Is.GreaterThan (1.2), "Per-call generic method lookup should be significantly slower than the non-generic baseline.");
			}
		}
	}
}
