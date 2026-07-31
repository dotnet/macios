
#nullable enable

namespace BusinessChat {

	[MacCatalyst (13, 1)]
	[Native]
	public enum BCChatButtonStyle : long {
		/// <summary>Indicates light.</summary>
		Light = 0,
		/// <summary>Indicates dark.</summary>
		Dark,
	}

	[Deprecated (PlatformName.MacOSX, 13, 1)]
	[Deprecated (PlatformName.iOS, 16, 2)]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 16, 2)]
	public enum BCParameterName {

		/// <summary>Indicates intent.</summary>
		[Field ("BCParameterNameIntent")]
		Intent,

		/// <summary>Indicates group.</summary>
		[Field ("BCParameterNameGroup")]
		Group,

		/// <summary>Indicates body.</summary>
		[Field ("BCParameterNameBody")]
		Body,
	}
}
