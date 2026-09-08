// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if !XAMCORE_5_0 && (__IOS__ || __MACCATALYST__)

using System.ComponentModel;

namespace CoreImage;

[Register ("CIFilterGenerator", true)]
[UnsupportedOSPlatform ("ios")]
[UnsupportedOSPlatform ("maccatalyst")]
[UnsupportedOSPlatform ("tvos")]
[SupportedOSPlatform ("macos")]
[Obsolete (Constants.TypeUnavailable, false)]
[EditorBrowsable (EditorBrowsableState.Never)]
public class CIFilterGenerator : NSObject, ICIFilterConstructor, INSCoding, INSCopying, INSSecureCoding {
	[EditorBrowsable (EditorBrowsableState.Never)]
	public override NativeHandle ClassHandle { get => throw new PlatformNotSupportedException (Constants.TypeUnavailable); }

	[EditorBrowsable (EditorBrowsableState.Never)]
	public CIFilterGenerator (NSCoder coder) : base (NSObjectFlag.Empty)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	protected CIFilterGenerator (NSObjectFlag t) : base (t)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	protected internal CIFilterGenerator (NativeHandle handle) : base (handle)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public CIFilterGenerator (NSUrl aURL)
		: base (NSObjectFlag.Empty)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public virtual void ConnectObject (NSObject sourceObject, string? withSourceKey, NSObject targetObject, string targetKey)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public virtual NSObject Copy (NSZone? zone)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public static CIFilterGenerator Create ()
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public virtual CIFilter CreateFilter ()
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public virtual void DisconnectObject (NSObject sourceObject, string sourceKey, NSObject targetObject, string targetKey)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public virtual void EncodeTo (NSCoder encoder)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public virtual void ExportKey (string key, NSObject targetObject, string? exportedKeyName)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public virtual CIFilter? FilterWithName (string name)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public static CIFilterGenerator? FromUrl (NSUrl aURL)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public virtual void RegisterFilterName (string name)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public virtual void RemoveExportedKey (string exportedKeyName)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public virtual bool Save (NSUrl toUrl, bool atomically)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public virtual void SetAttributesforExportedKey (NSDictionary attributes, NSString exportedKey)
	{
		throw new PlatformNotSupportedException (Constants.TypeUnavailable);
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public virtual NSDictionary ClassAttributes {
		[Export ("classAttributes")]
		get {
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}
		[Export ("setClassAttributes:")]
		set {
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	public virtual NSDictionary ExportedKeys {
		[Export ("exportedKeys")]
		get {
			throw new PlatformNotSupportedException (Constants.TypeUnavailable);
		}
	}
} /* class CIFilterGenerator */

#endif // !XAMCORE_5_0 && (__IOS__ || __MACCATALYST__)
