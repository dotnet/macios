#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace CoreMediaIO {

	/// <summary>Identifies a specific property of a CoreMediaIO hardware object using a selector, scope, and element.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst15.4")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CMIOObjectPropertyAddress {
		uint mSelector;
		uint mScope;
		uint mElement;

#if !COREBUILD
		/// <summary>Gets or sets the property selector.</summary>
		public uint Selector {
			get => mSelector;
			set => mSelector = value;
		}

		/// <summary>Gets or sets the property scope.</summary>
		public uint Scope {
			get => mScope;
			set => mScope = value;
		}

		/// <summary>Gets or sets the property element.</summary>
		public uint Element {
			get => mElement;
			set => mElement = value;
		}

		/// <summary>Creates a new <see cref="CMIOObjectPropertyAddress" /> with the specified selector, scope, and element.</summary>
		/// <param name="selector">The property selector.</param>
		/// <param name="scope">The property scope.</param>
		/// <param name="element">The property element.</param>
		public CMIOObjectPropertyAddress (uint selector, uint scope, uint element)
		{
			mSelector = selector;
			mScope = scope;
			mElement = element;
		}
#endif // !COREBUILD
	}
}
