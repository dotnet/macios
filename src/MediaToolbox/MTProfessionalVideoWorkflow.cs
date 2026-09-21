#nullable enable

#if MONOMAC

namespace MediaToolbox {
	/// <summary>Provides methods for registering professional video workflow format readers.</summary>
	[SupportedOSPlatform ("macos")]
	static public class MTProfessionalVideoWorkflow {

		/// <summary>Registers professional video workflow format readers with the system.</summary>
		[SupportedOSPlatform ("macos")]
		[DllImport (Constants.MediaToolboxLibrary, EntryPoint = "MTRegisterProfessionalVideoWorkflowFormatReaders")]
		public static extern void RegisterFormatReaders ();
	}
}

#endif
