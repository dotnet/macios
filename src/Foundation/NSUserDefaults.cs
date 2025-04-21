using System;
using ObjCRuntime;

#nullable enable

namespace Foundation {

	/// <summary>This enum is used to select how to initialize a new instance of an <see cref="NSUserDefaults" />.</summary>
	public enum NSUserDefaultsType {
		/// <summary>The name specifies a user name.</summary>
		UserName,
		/// <summary>The name specifies a suite name.</summary>
		SuiteName,
	}

	public partial class NSUserDefaults {
		/// <summary>Create a new <see cref="NSUserDefaults" /> instance.</summary>
		/// <param name="name">The user name for the new <see cref="NSUserDefaults" /> instance.</param>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios")]
		[ObsoletedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst")]
		public NSUserDefaults (string name) : this (name, NSUserDefaultsType.UserName)
		{
		}

		/// <summary>Create a new <see cref="NSUserDefaults" /> instance.</summary>
		/// <param name="name">The name for the new <see cref="NSUserDefaults" /> instance. Cannot be null if <paramref name="type" /> is <see cref="NSUserDefaultsType.UserName" />.</param>
		/// <param name="type">Specify whether the <paramref name="name" /> parameter is a user name or a suite name.</param>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public NSUserDefaults (string? name, NSUserDefaultsType type)
			: base (NSObjectFlag.Empty)
		{
			switch (type) {
			case NSUserDefaultsType.UserName:
				if (name is null)
					throw new ArgumentNullException (nameof (name), "initWithUser:");
				InitializeHandle (_InitWithUserName (name));
				break;
			case NSUserDefaultsType.SuiteName:
				InitializeHandle (_InitWithSuiteName (name), "initWithSuiteName:");
				break;
			default:
				throw new ArgumentException (nameof (type));
			}
		}

		/// <summary>Sets a string value at the specified key.</summary>
		/// <param name="value">String value to store.</param>
		/// <param name="defaultName">The key name used to store the value.</param>
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
