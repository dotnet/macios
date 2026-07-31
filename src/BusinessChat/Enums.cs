
#nullable enable

namespace BusinessChat {

	[MacCatalyst (13, 1)]
	[Native]
	public enum BCChatButtonStyle : long {
		/// <summary>A light-themed chat button.</summary>
		Light = 0,
		/// <summary>A dark-themed chat button.</summary>
		Dark,
	}

	[Deprecated (PlatformName.MacOSX, 13, 1)]
	[Deprecated (PlatformName.iOS, 16, 2)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 16, 2)]
	public enum BCParameterName {

		/// <summary>The intent parameter for the chat session.</summary>
		[Field ("BCParameterNameIntent")]
		Intent,

		/// <summary>The group parameter for the chat session.</summary>
		[Field ("BCParameterNameGroup")]
		Group,

		/// <summary>The body parameter for the chat session.</summary>
		[Field ("BCParameterNameBody")]
		Body,
	}
}
