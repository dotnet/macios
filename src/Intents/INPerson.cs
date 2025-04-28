using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace Intents {

#if !TVOS
	public partial class INPerson {
		/// <summary>This enum is used to select how to initialize a new instance of an <see cref="INPerson" />.</summary>
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public enum INPersonType {
			/// <summary>The specified person is me.</summary>
			Me = 0,
			/// <summary>The specified person is a contact suggestion.</summary>
			ContactSuggestion = 1,
		}

		/// <summary>Create a new <see cref="INPerson" /> instance.</summary>
		/// <param name="personHandle">The person handle for the new <see cref="INPerson" /> instance.</param>
		/// <param name="nameComponents">The name components for the new <see cref="INPerson" /> instance.</param>
		/// <param name="displayName">The display name for the new <see cref="INPerson" /> instance.</param>
		/// <param name="image">The image for the new <see cref="INPerson" /> instance.</param>
		/// <param name="contactIdentifier">The contact identifier for the new <see cref="INPerson" /> instance.</param>
		/// <param name="customIdentifier">The custom identifier for the new <see cref="INPerson" /> instance.</param>
		/// <param name="isMe">Whether the new <see cref="INPerson" /> instance is me or not.</param>
		/// <param name="suggestionType">The suggestion type for the new <see cref="INPerson" /> instance.</param>
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public INPerson (INPersonHandle personHandle, NSPersonNameComponents? nameComponents, string? displayName, INImage? image, string? contactIdentifier, string? customIdentifier, bool isMe, INPersonSuggestionType suggestionType)
			: this (personHandle, nameComponents, displayName, image, contactIdentifier, customIdentifier, isMe, suggestionType, INPersonType.Me)
		{
		}

		/// <summary>Create a new <see cref="INPerson" /> instance.</summary>
		/// <param name="personHandle">The person handle for the new <see cref="INPerson" /> instance.</param>
		/// <param name="nameComponents">The name components for the new <see cref="INPerson" /> instance.</param>
		/// <param name="displayName">The display name for the new <see cref="INPerson" /> instance.</param>
		/// <param name="image">The image for the new <see cref="INPerson" /> instance.</param>
		/// <param name="contactIdentifier">The contact identifier for the new <see cref="INPerson" /> instance.</param>
		/// <param name="customIdentifier">The custom identifier for the new <see cref="INPerson" /> instance.</param>
		/// <param name="isMe">Whether the new <see cref="INPerson" /> instance is me or not, or whether it's a contact suggestion or not.</param>
		/// <param name="suggestionType">The suggestion type for the new <see cref="INPerson" /> instance.</param>
		/// <param name="personType">Whether the <paramref name="isMe" /> parameter determines whether the <see cref="INPerson" /> is me (or not), or whether it's a contact suggestion (or not).</param>
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public INPerson (INPersonHandle personHandle, NSPersonNameComponents? nameComponents, string? displayName, INImage? image, string? contactIdentifier, string? customIdentifier, bool isMe, INPersonSuggestionType suggestionType, INPersonType personType)
			: base (NSObjectFlag.Empty)
		{
			switch (personType) {
			case INPersonType.Me:
				InitializeHandle (_InitWithMe (personHandle, nameComponents, displayName, image, contactIdentifier, customIdentifier, isMe, suggestionType),
					"initWithPersonHandle:nameComponents:displayName:image:contactIdentifier:customIdentifier:isMe:suggestionType:");
				break;
			case INPersonType.ContactSuggestion:
				InitializeHandle (_InitWithContactSuggestion (personHandle, nameComponents, displayName, image, contactIdentifier, customIdentifier, isMe, suggestionType),
					"initWithPersonHandle:nameComponents:displayName:image:contactIdentifier:customIdentifier:isContactSuggestion:suggestionType:");
				break;
			default:
				throw new ArgumentException (nameof (personType));
			}
		}
	}
#endif // !TVOS
}
