#nullable enable

#if MONOMAC

namespace MediaToolbox {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	static public class MTProfessionalVideoWorkflow {

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[DllImport (Constants.MediaToolboxLibrary, EntryPoint = "MTRegisterProfessionalVideoWorkflowFormatReaders")]
		public static extern void RegisterFormatReaders ();
	}
}

#endif
