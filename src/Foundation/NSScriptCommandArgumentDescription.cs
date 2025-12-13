// Copyright 2015 Xamarin, Inc.

#nullable enable

namespace Foundation {

#if MONOMAC || __MACCATALYST__

	// The keys are not found in any of the public headers from Apple. That is the reason
	// to use this technique.
	/// <summary>Provides keys for <see cref="NSScriptCommandArgumentDescription" /> dictionaries.</summary>
	public static class NSScriptCommandArgumentDescriptionKeys {
		/// <summary>Gets the key for the Apple event code.</summary>
		/// <value>An <see cref="NSString" /> representing the Apple event code key.</value>
		public static NSString AppleEventCodeKey {
			get { return NSScriptCommonKeys.AppleEventCodeKey; }
		}

		/// <summary>Gets the key for the type.</summary>
		/// <value>An <see cref="NSString" /> representing the type key.</value>
		public static NSString TypeKey {
			get { return NSScriptCommonKeys.TypeKey; }
		}

		static readonly NSString optional = new NSString ("Optional");
		/// <summary>Gets the key for the optional flag.</summary>
		/// <value>An <see cref="NSString" /> representing the optional key.</value>
		public static NSString OptionalKey {
			get { return optional; }
		}
	}

	/// <summary>Describes an argument for an <see cref="NSScriptCommand" />.</summary>
	public partial class NSScriptCommandArgumentDescription {
		/// <summary>Gets or sets the name of the argument.</summary>
		/// <value>The name of the argument.</value>
		public string Name { get; set; } = "";

		/// <summary>Gets or sets a value indicating whether the argument is optional.</summary>
		/// <value><see langword="true" /> if the argument is optional; otherwise, <see langword="false" />.</value>
		public bool IsOptional {
			get {
				return Optional is not null && Optional == "Yes";
			}
			set {
				Optional = (value) ? "Yes" : "No";
			}
		}

		/// <summary>Initializes a new instance of the <see cref="NSScriptCommandArgumentDescription" /> class.</summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="eventCode">The Apple event code for the argument. Must be a four-character string.</param>
		/// <param name="type">The type of the argument.</param>
		/// <param name="isOptional">If <see langword="true" />, the argument is optional; otherwise, <see langword="false" />. Default is <see langword="false" />.</param>
		/// <exception cref="ArgumentException">Thrown when <paramref name="name" />, <paramref name="eventCode" />, or <paramref name="type" /> is null or empty, or when <paramref name="eventCode" /> is not a four-character string.</exception>
		public NSScriptCommandArgumentDescription (string name, string eventCode, string type, bool isOptional = false)
		{
			if (String.IsNullOrEmpty (name))
				throw new ArgumentException ("name cannot be null or empty.");
			if (String.IsNullOrEmpty (eventCode))
				throw new ArgumentException ("eventCode cannot be null or empty");
			if (String.IsNullOrEmpty (type))
				throw new ArgumentException ("type cannot be null or empty.");

			if (eventCode.Length != 4)
				throw new ArgumentException ("eventCode must be a four char string");

			Name = name;
			AppleEventCode = eventCode;
			Type = type;
			IsOptional = isOptional;
		}

	}

#endif

}

