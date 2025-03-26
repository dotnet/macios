using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace AVFoundation {
	/// <summary>This enum is used to select how to initialize a new <see cref="AVSpeechUtterance" /> instance.</summary>
	public enum AVSpeechUtteranceInitializationOption {
		/// <summary>The <c>string</c> parameter passed to the constructor is a plain text string.</summary>
		PlainText,
		/// <summary>The <c>string</c> parameter passed to the constructor is an SSML (Speech Synthesis Markup Language) string.</summary>
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("tvos16.0")]
		SsmlRepresentation,
	}

	public partial class AVSpeechUtterance {
		/// <summary>Create a new <see cref="AVSpeechUtterance" /> instance for the specified string.</summary>
		/// <param name="speechString">The text to speak.</param>
		public AVSpeechUtterance (string speechString)
			: this (speechString, AVSpeechUtteranceInitializationOption.PlainText)
		{
		}

		/// <summary>Create a new <see cref="AVSpeechUtterance" /> instance for the specified string.</summary>
		/// <param name="string">The text to speak.</param>
		/// <param name="option">Use this option to specify how to interpret the <paramref name="string" /> parameter.</param>
		public AVSpeechUtterance (string @string, AVSpeechUtteranceInitializationOption option)
			: base (NSObjectFlag.Empty)
		{
			switch (option) {
			case AVSpeechUtteranceInitializationOption.PlainText:
				InitializeHandle (_InitWithString (@string));
				break;
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later, 'macOS/OSX' 12.0 and later, 'tvos' 12.2 and later. 'AVSpeechUtterance._InitWithSsmlRepresentation(string)' is only supported on: 'ios' 16.0 and later, 'maccatalyst' 16.0 and later, 'macOS/OSX' 13.0 and later, 'tvos' 16.0 and later.
			case AVSpeechUtteranceInitializationOption.SsmlRepresentation:
				InitializeHandle (_InitWithSsmlRepresentation (@string));
#pragma warning restore CA1416
				break;
			default:
				throw new ArgumentOutOfRangeException (nameof (option), option, "Invalid enum value.");
			}
		}
	}
}
