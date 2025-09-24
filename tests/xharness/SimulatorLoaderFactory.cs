using System;
using Microsoft.DotNet.XHarness.iOS.Shared;
using Microsoft.DotNet.XHarness.iOS.Shared.Execution;
using Microsoft.DotNet.XHarness.iOS.Shared.Hardware;

#nullable enable

namespace Xharness {

	public interface ISimulatorLoaderFactory {
		ISimulatorLoader CreateLoader ();
	}

	public class SimulatorLoaderFactory : ISimulatorLoaderFactory {
		readonly IMlaunchProcessManager processManager;
		readonly IHarness harness;

		public SimulatorLoaderFactory (IMlaunchProcessManager processManager, IHarness harness)
		{
			this.processManager = processManager ?? throw new ArgumentNullException (nameof (processManager));
			this.harness = harness ?? throw new ArgumentNullException (nameof (harness));
		}

		public ISimulatorLoader CreateLoader () => new SimulatorLoader (processManager, new SimulatorSelector (harness));
	}

	public class SimulatorSelector : DefaultSimulatorSelector {
		readonly IHarness harness;

		public SimulatorSelector (IHarness harness)
		{
			this.harness = harness ?? throw new ArgumentNullException (nameof (harness));
		}

		public override string GetDeviceType (TestTargetOs target, bool minVersion)
		{
			return target.Platform switch {
				TestTarget.Simulator_iOS64 => harness.IOS_SIMULATOR_DEVICE_TYPE,
				TestTarget.Simulator_tvOS => harness.TVOS_SIMULATOR_DEVICE_TYPE,
				_ => throw new Exception (string.Format ("Invalid simulator target: {0}", target))
			};
		}

		public override void GetCompanionRuntimeAndDeviceType (TestTargetOs target, bool minVersion, out string? companionRuntime, out string? companionDeviceType)
		{
			companionRuntime = null;
			companionDeviceType = null;
		}
	}
}
