// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace ObjCRuntime {
	/// <summary>Indicates that an API is supported for a specified platform. If a version is specified, the API is available starting in the specified OS version. Multiple attributes can be applied to indicate support on multiple operating systems.</summary>
	/// <remarks>
	///   <para>Contrary to standard availability attributes (such as <see cref="SupportedOSPlatformAttribute" /> attributes), the presence of this attribute for some platforms does not imply any meaning for other platforms.</para>
	///   <para>If there are no <see cref="SupportedSimulatorAttribute" /> or <see cref="UnsupportedSimulatorAttribute" /> attributes on an API, the API is assumed to be available in the simulator.</para>
	///   <para>This attribute will be trimmed away if the app is trimmed.</para>
	/// </remarks>
	[AttributeUsage (AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Field, AllowMultiple = true)]
	public sealed class SupportedSimulatorAttribute : Attribute {
		/// <summary>Initializes a new <see cref="SupportedSimulatorAttribute" /> attribute.</summary>
		/// <param name="platformName">The platform where this API is supported in the simulator.
		/// Format: platform name (e.g., "ios", "tvos") optionally followed by a version number (e.g., "ios17.0").</param>
		public SupportedSimulatorAttribute (string platformName)
		{
			PlatformName = platformName;
		}

		/// <summary>The name of the platform.</summary>
		public string PlatformName { get; init; }
	}

	/// <summary>Indicates that an API is not supported in the simulator for the specified platform. Multiple attributes can be applied to indicate lack of support on multiple platforms.</summary>
	/// <remarks>
	///   <para>Contrary to standard availability attributes (such as <see cref="SupportedOSPlatformAttribute" /> attributes), the presence of this attribute for some platforms does not imply any meaning for other platforms.</para>
	///   <para>If there are no <see cref="SupportedSimulatorAttribute" /> or <see cref="UnsupportedSimulatorAttribute" /> attributes on an API, the API is assumed to be available in the simulator.</para>
	///   <para>This attribute will be trimmed away if the app is trimmed.</para>
	/// </remarks>
	[AttributeUsage (AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Field, AllowMultiple = true)]
	public sealed class UnsupportedSimulatorAttribute : Attribute {
		/// <summary>Initializes a new <see cref="UnsupportedSimulatorAttribute" /> attribute.</summary>
		/// <param name="platformName">The platform where this API is not supported in the simulator.</param>
		public UnsupportedSimulatorAttribute (string platformName)
		{
			PlatformName = platformName;
		}

		/// <summary>The name of the platform.</summary>
		public string PlatformName { get; init; }
	}
}
