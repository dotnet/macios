using System;
using ObjCRuntime;

#nullable enable

namespace Foundation {

	/// <summary>An enumeration whose values specify the type of a <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=T:NSFoundation.NSUserDefaults&amp;scope=Xamarin" title="T:NSFoundation.NSUserDefaults">T:NSFoundation.NSUserDefaults</a></format> object.</summary>
	///     <remarks>To be added.</remarks>
	public enum NSUserDefaultsType {
		/// <summary>To be added.</summary>
		UserName,
		/// <summary>To be added.</summary>
		SuiteName,
	}

	public partial class NSUserDefaults {
#if NET
		/// <param name="name">The user name.</param>
		///         <summary>Developers should not use this deprecated constructor. </summary>
		///         <remarks>This method has been deprecated, avoid.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.10")]
		[ObsoletedOSPlatform ("ios7.0")]
#else
		[Deprecated (PlatformName.iOS, 7, 0)]
		[Deprecated (PlatformName.MacOSX, 10, 10)]
#endif
		public NSUserDefaults (string name) : this (name, NSUserDefaultsType.UserName)
		{
		}

#if NET
		/// <param name="name">The name for your suite (application group)</param>
		///         <param name="type">The type to create.   Notice that the <see cref="F:Foundation.NSUserDefaultsType.UserName" /> is no longer supported.</param>
		///         <summary>Returns a new NSUserDefaults for the specific suite name when passing <see cref="F:Foundation.NSUserDefaultsType.SuiteName" /> as the type. </summary>
		///         <remarks>Use this method to create an NSUserDefaults that can be used to share information across applications in a suite, or between an application and its extensions.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		public NSUserDefaults (string? name, NSUserDefaultsType type)
		{
			// two different `init*` would share the same C# signature
			switch (type) {
			case NSUserDefaultsType.UserName:
				if (name is null)
					throw new ArgumentNullException (nameof (name));
				Handle = InitWithUserName (name);
				break;
			case NSUserDefaultsType.SuiteName:
				Handle = InitWithSuiteName (name);
				break;
			default:
				throw new ArgumentException (nameof (type));
			}
		}

		/// <param name="value">String value to store.</param>
		///         <param name="defaultName">The key name used to store the value.</param>
		///         <summary>Sets a string value at the specified key.</summary>
		///         <remarks>
		///           <para>
		///           </para>
		///         </remarks>
		public void SetString (string? value, string defaultName)
		{
			using var str = (NSString?) value;
			SetObjectForKey (str, defaultName);
		}

		public NSObject? this [string key] {
			get {
				return ObjectForKey (key);
			}

			set {
				SetObjectForKey (value, key);
			}
		}
	}
}
