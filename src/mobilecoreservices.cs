using Foundation;
using ObjCRuntime;

namespace MobileCoreServices {

	/// <summary>Defines constants for Uniform Type Identifiers (UTIs) which are strings that identify a given class of type or item.</summary>
	///     <remarks>
	///       <para>Uniform Type Identifiers (UTIs) are Apple's method for identifying data types.</para>
	///       <para>UTIs use a reverse-DNS naming structure.  UTIs support multiple inheritance, allowing files to be identified with any number of relevant types, as appropriate to the contained data.</para>
	///     </remarks>
	[Deprecated (PlatformName.iOS, 14, 0, message: "Use the 'UniformTypeIdentifiers.UTType' API instead.")]
	[Deprecated (PlatformName.TvOS, 14, 0, message: "Use the 'UniformTypeIdentifiers.UTType' API instead.")]
	[Deprecated (PlatformName.MacOSX, 11, 0, message: "Use the 'UniformTypeIdentifiers.UTType' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use the 'UniformTypeIdentifiers.UTType' API instead.")]
	[Partial]
	interface UTType {
		/// <summary>Represents the value associated with the constant kUTTypeItem</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeItem", "+CoreServices")]
		NSString Item { get; }

		/// <summary>Represents the value associated with the constant kUTTypeContent</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeContent", "+CoreServices")]
		NSString Content { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCompositeContent</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCompositeContent", "+CoreServices")]
		NSString CompositeContent { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMessage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMessage", "+CoreServices")]
		NSString Message { get; }

		/// <summary>Represents the value associated with the constant kUTTypeContact</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeContact", "+CoreServices")]
		NSString Contact { get; }

		/// <summary>Represents the value associated with the constant kUTTypeArchive</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeArchive", "+CoreServices")]
		NSString Archive { get; }

		/// <summary>Represents the value associated with the constant kUTTypeDiskImage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeDiskImage", "+CoreServices")]
		NSString DiskImage { get; }

		/// <summary>Represents the value associated with the constant kUTTypeData</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeData", "+CoreServices")]
		NSString Data { get; }

		/// <summary>Represents the value associated with the constant kUTTypeDirectory</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeDirectory", "+CoreServices")]
		NSString Directory { get; }

		/// <summary>Represents the value associated with the constant kUTTypeResolvable</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeResolvable", "+CoreServices")]
		NSString Resolvable { get; }

		/// <summary>Represents the value associated with the constant kUTTypeSymLink</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeSymLink", "+CoreServices")]
		NSString SymLink { get; }

		/// <summary>Represents the value associated with the constant kUTTypeExecutable</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeExecutable", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString Executable { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMountPoint</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMountPoint", "+CoreServices")]
		NSString MountPoint { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAliasFile</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAliasFile", "+CoreServices")]
		NSString AliasFile { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAliasRecord</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAliasRecord", "+CoreServices")]
		NSString AliasRecord { get; }

		/// <summary>Represents the value associated with the constant kUTTypeURLBookmarkData</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeURLBookmarkData", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString URLBookmarkData { get; }

		/// <summary>Represents the value associated with the constant kUTTypeURL</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeURL", "+CoreServices")]
		NSString URL { get; }

		/// <summary>Represents the value associated with the constant kUTTypeFileURL</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeFileURL", "+CoreServices")]
		NSString FileURL { get; }

		/// <summary>Represents the value associated with the constant kUTTypeText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeText", "+CoreServices")]
		NSString Text { get; }

		/// <summary>Represents the value associated with the constant kUTTypePlainText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePlainText", "+CoreServices")]
		NSString PlainText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeUTF8PlainText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeUTF8PlainText", "+CoreServices")]
		NSString UTF8PlainText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeUTF16ExternalPlainText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeUTF16ExternalPlainText", "+CoreServices")]
		NSString UTF16ExternalPlainText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeUTF16PlainText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeUTF16PlainText", "+CoreServices")]
		NSString UTF16PlainText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeDelimitedText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeDelimitedText", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString DelimitedText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCommaSeparatedText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCommaSeparatedText", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString CommaSeparatedText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeTabSeparatedText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeTabSeparatedText", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString TabSeparatedText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeUTF8TabSeparatedText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeUTF8TabSeparatedText", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString UTF8TabSeparatedText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeRTF</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeRTF", "+CoreServices")]
		NSString RTF { get; }

		/// <summary>Represents the value associated with the constant kUTTypeHTML</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeHTML", "+CoreServices")]
		NSString HTML { get; }

		/// <summary>Represents the value associated with the constant kUTTypeXML</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeXML", "+CoreServices")]
		NSString XML { get; }

		/// <summary>Represents the value associated with the constant kUTTypeSourceCode</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeSourceCode", "+CoreServices")]
		NSString SourceCode { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAssemblyLanguageSource</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAssemblyLanguageSource", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString AssemblyLanguageSource { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCSource</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCSource", "+CoreServices")]
		NSString CSource { get; }

		/// <summary>Represents the value associated with the constant kUTTypeObjectiveCSource</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeObjectiveCSource", "+CoreServices")]
		NSString ObjectiveCSource { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCPlusPlusSource</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCPlusPlusSource", "+CoreServices")]
		NSString CPlusPlusSource { get; }

		/// <summary>Represents the value associated with the constant kUTTypeObjectiveCPlusPlusSource</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeObjectiveCPlusPlusSource", "+CoreServices")]
		NSString ObjectiveCPlusPlusSource { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCHeader</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCHeader", "+CoreServices")]
		NSString CHeader { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCPlusPlusHeader</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCPlusPlusHeader", "+CoreServices")]
		NSString CPlusPlusHeader { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJavaSource</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJavaSource", "+CoreServices")]
		NSString JavaSource { get; }

		/// <summary>Represents the value associated with the constant kUTTypeScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeScript", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString Script { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAppleScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAppleScript", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString AppleScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypeOSAScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeOSAScript", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString OSAScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypeOSAScriptBundle</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeOSAScriptBundle", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString OSAScriptBundle { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJavaScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJavaScript", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString JavaScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypeShellScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeShellScript", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString ShellScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypePerlScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePerlScript", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString PerlScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypePythonScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePythonScript", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString PythonScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypeRubyScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeRubyScript", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString RubyScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypePHPScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePHPScript", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString PHPScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJSON</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJSON", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString JSON { get; }

		/// <summary>Represents the value associated with the constant kUTTypePropertyList</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePropertyList", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString PropertyList { get; }

		/// <summary>Represents the value associated with the constant kUTTypeXMLPropertyList</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeXMLPropertyList", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString XMLPropertyList { get; }

		/// <summary>Represents the value associated with the constant kUTTypeBinaryPropertyList</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeBinaryPropertyList", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString BinaryPropertyList { get; }

		/// <summary>Represents the value associated with the constant kUTTypePDF</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePDF", "+CoreServices")]
		NSString PDF { get; }

		/// <summary>Represents the value associated with the constant kUTTypeRTFD</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeRTFD", "+CoreServices")]
		NSString RTFD { get; }

		/// <summary>Represents the value associated with the constant kUTTypeFlatRTFD</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeFlatRTFD", "+CoreServices")]
		NSString FlatRTFD { get; }

		/// <summary>Represents the value associated with the constant kUTTypeTXNTextAndMultimediaData</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeTXNTextAndMultimediaData", "+CoreServices")]
		NSString TXNTextAndMultimediaData { get; }

		/// <summary>Represents the value associated with the constant kUTTypeWebArchive</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeWebArchive", "+CoreServices")]
		NSString WebArchive { get; }

		/// <summary>Represents the value associated with the constant kUTTypeImage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeImage", "+CoreServices")]
		NSString Image { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJPEG</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJPEG", "+CoreServices")]
		NSString JPEG { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJPEG2000</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJPEG2000", "+CoreServices")]
		NSString JPEG2000 { get; }

		/// <summary>Represents the value associated with the constant kUTTypeTIFF</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeTIFF", "+CoreServices")]
		NSString TIFF { get; }

		/// <summary>Represents the value associated with the constant kUTTypePICT</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePICT", "+CoreServices")]
		NSString PICT { get; }

		/// <summary>Represents the value associated with the constant kUTTypeGIF</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeGIF", "+CoreServices")]
		NSString GIF { get; }

		/// <summary>Represents the value associated with the constant kUTTypePNG</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePNG", "+CoreServices")]
		NSString PNG { get; }

		/// <summary>Represents the value associated with the constant kUTTypeQuickTimeImage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeQuickTimeImage", "+CoreServices")]
		NSString QuickTimeImage { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAppleICNS</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAppleICNS", "+CoreServices")]
		NSString AppleICNS { get; }

		/// <summary>Represents the value associated with the constant kUTTypeBMP</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeBMP", "+CoreServices")]
		NSString BMP { get; }

		/// <summary>Represents the value associated with the constant kUTTypeICO</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeICO", "+CoreServices")]
		NSString ICO { get; }

		/// <summary>Represents the value associated with the constant kUTTypeRawImage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeRawImage", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString RawImage { get; }

		/// <summary>Represents the value associated with the constant kUTTypeScalableVectorGraphics</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeScalableVectorGraphics", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString ScalableVectorGraphics { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAudiovisualContent</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAudiovisualContent", "+CoreServices")]
		NSString AudiovisualContent { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMovie</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMovie", "+CoreServices")]
		NSString Movie { get; }

		/// <summary>Represents the value associated with the constant kUTTypeVideo</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeVideo", "+CoreServices")]
		NSString Video { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAudio</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAudio", "+CoreServices")]
		NSString Audio { get; }

		/// <summary>Represents the value associated with the constant kUTTypeQuickTimeMovie</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeQuickTimeMovie", "+CoreServices")]
		NSString QuickTimeMovie { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMPEG</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMPEG", "+CoreServices")]
		NSString MPEG { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMPEG2Video</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMPEG2Video", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString MPEG2Video { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMPEG2TransportStream</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMPEG2TransportStream", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString MPEG2TransportStream { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMP3</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMP3", "+CoreServices")]
		NSString MP3 { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMPEG4</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMPEG4", "+CoreServices")]
		NSString MPEG4 { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMPEG4Audio</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMPEG4Audio", "+CoreServices")]
		NSString MPEG4Audio { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAppleProtectedMPEG4Audio</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAppleProtectedMPEG4Audio", "+CoreServices")]
		NSString AppleProtectedMPEG4Audio { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAppleProtectedMPEG4Video</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAppleProtectedMPEG4Video", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString AppleProtectedMPEG4Video { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAVIMovie</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAVIMovie", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString AVIMovie { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAudioInterchangeFileFormat</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAudioInterchangeFileFormat", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString AudioInterchangeFileFormat { get; }

		/// <summary>Represents the value associated with the constant kUTTypeWaveformAudio</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeWaveformAudio", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString WaveformAudio { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMIDIAudio</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMIDIAudio", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString MIDIAudio { get; }

		/// <summary>Represents the value associated with the constant kUTTypePlaylist</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePlaylist", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString Playlist { get; }

		/// <summary>Represents the value associated with the constant kUTTypeM3UPlaylist</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeM3UPlaylist", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString M3UPlaylist { get; }

		/// <summary>Represents the value associated with the constant kUTTypeFolder</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeFolder", "+CoreServices")]
		NSString Folder { get; }

		/// <summary>Represents the value associated with the constant kUTTypeVolume</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeVolume", "+CoreServices")]
		NSString Volume { get; }

		/// <summary>Represents the value associated with the constant kUTTypePackage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePackage", "+CoreServices")]
		NSString Package { get; }

		/// <summary>Represents the value associated with the constant kUTTypeBundle</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeBundle", "+CoreServices")]
		NSString Bundle { get; }

		/// <summary>Represents the value associated with the constant kUTTypePluginBundle</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePluginBundle", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString PluginBundle { get; }

		/// <summary>Represents the value associated with the constant kUTTypeSpotlightImporter</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeSpotlightImporter", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString SpotlightImporter { get; }

		/// <summary>Represents the value associated with the constant kUTTypeQuickLookGenerator</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeQuickLookGenerator", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString QuickLookGenerator { get; }

		/// <summary>Represents the value associated with the constant kUTTypeXPCService</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeXPCService", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString XPCService { get; }

		/// <summary>Represents the value associated with the constant kUTTypeFramework</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeFramework", "+CoreServices")]
		NSString Framework { get; }

		/// <summary>Represents the value associated with the constant kUTTypeApplication</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeApplication", "+CoreServices")]
		NSString Application { get; }

		/// <summary>Represents the value associated with the constant kUTTypeApplicationBundle</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeApplicationBundle", "+CoreServices")]
		NSString ApplicationBundle { get; }

		/// <summary>Represents the value associated with the constant kUTTypeApplicationFile</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeApplicationFile", "+CoreServices")]
		NSString ApplicationFile { get; }

		/// <summary>Represents the value associated with the constant kUTTypeUnixExecutable</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeUnixExecutable", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString UnixExecutable { get; }

		/// <summary>Represents the value associated with the constant kUTTypeWindowsExecutable</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeWindowsExecutable", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString WindowsExecutable { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJavaClass</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJavaClass", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString JavaClass { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJavaArchive</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJavaArchive", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString JavaArchive { get; }

		/// <summary>Represents the value associated with the constant kUTTypeSystemPreferencesPane</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeSystemPreferencesPane", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString SystemPreferencesPane { get; }

		/// <summary>Represents the value associated with the constant kUTTypeGNUZipArchive</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeGNUZipArchive", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString GNUZipArchive { get; }

		/// <summary>Represents the value associated with the constant kUTTypeBzip2Archive</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeBzip2Archive", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString Bzip2Archive { get; }

		/// <summary>Represents the value associated with the constant kUTTypeZipArchive</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeZipArchive", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString ZipArchive { get; }

		/// <summary>Represents the value associated with the constant kUTTypeSpreadsheet</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeSpreadsheet", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString Spreadsheet { get; }

		/// <summary>Represents the value associated with the constant kUTTypePresentation</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePresentation", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString Presentation { get; }

		/// <summary>Represents the value associated with the constant kUTTypeDatabase</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeDatabase", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString Database { get; }

		/// <summary>Represents the value associated with the constant kUTTypeVCard</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeVCard", "+CoreServices")]
		NSString VCard { get; }

		/// <summary>Represents the value associated with the constant kUTTypeToDoItem</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeToDoItem", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString ToDoItem { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCalendarEvent</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCalendarEvent", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString CalendarEvent { get; }

		/// <summary>Represents the value associated with the constant kUTTypeEmailMessage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeEmailMessage", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString EmailMessage { get; }

		/// <summary>Represents the value associated with the constant kUTTypeInternetLocation</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeInternetLocation", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString InternetLocation { get; }

		/// <summary>Represents the value associated with the constant kUTTypeInkText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeInkText", "+CoreServices")]
		NSString InkText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeFont</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeFont", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString Font { get; }

		/// <summary>Represents the value associated with the constant kUTTypeBookmark</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeBookmark", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString Bookmark { get; }

		/// <summary>Represents the value associated with the constant kUTType3DContent</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTType3DContent", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString ThreeDContent { get; }

		/// <summary>Represents the value associated with the constant kUTTypePKCS12</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePKCS12", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString PKCS12 { get; }

		/// <summary>Represents the value associated with the constant kUTTypeX509Certificate</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeX509Certificate", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString X509Certificate { get; }

		/// <summary>Represents the value associated with the constant kUTTypeElectronicPublication</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeElectronicPublication", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString ElectronicPublication { get; }

		/// <summary>Represents the value associated with the constant kUTTypeLog</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeLog", "+CoreServices")]
		[MacCatalyst (13, 1)]
		NSString Log { get; }

		/// <summary>Represents the value associated with the constant kUTExportedTypeDeclarationsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTExportedTypeDeclarationsKey", "+CoreServices")]
		NSString ExportedTypeDeclarationsKey { get; }

		/// <summary>Represents the value associated with the constant kUTImportedTypeDeclarationsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTImportedTypeDeclarationsKey", "+CoreServices")]
		NSString ImportedTypeDeclarationsKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeIdentifierKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeIdentifierKey", "+CoreServices")]
		NSString IdentifierKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeTagSpecificationKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeTagSpecificationKey", "+CoreServices")]
		NSString TagSpecificationKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeConformsToKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeConformsToKey", "+CoreServices")]
		NSString ConformsToKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeDescriptionKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeDescriptionKey", "+CoreServices")]
		NSString DescriptionKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeIconFileKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeIconFileKey", "+CoreServices")]
		NSString IconFileKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeReferenceURLKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeReferenceURLKey", "+CoreServices")]
		NSString ReferenceURLKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeVersionKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeVersionKey", "+CoreServices")]
		NSString VersionKey { get; }

		/// <summary>Represents the value associated with the constant kUTTagClassFilenameExtension</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTagClassFilenameExtension", "+CoreServices")]
		NSString TagClassFilenameExtension { get; }

		/// <summary>Represents the value associated with the constant kUTTagClassMIMEType</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTagClassMIMEType", "+CoreServices")]
		NSString TagClassMIMEType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Field ("kUTTagClassNSPboardType", "+CoreServices")]
		NSString TagClassNSPboardType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Field ("kUTTagClassOSType", "+CoreServices")]
		NSString TagClassOSType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kUTTypeSwiftSource", "+CoreServices")]
		NSString SwiftSource { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kUTTypeAlembic", "ModelIO")]
		NSString Alembic { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kUTType3dObject", "ModelIO")]
		NSString k3dObject { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kUTTypePolygon", "ModelIO")]
		NSString Polygon { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kUTTypeStereolithography", "ModelIO")]
		NSString Stereolithography { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kUTTypeUniversalSceneDescription", "ModelIO")]
		NSString UniversalSceneDescription { get; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Field ("kUTTypeUniversalSceneDescriptionMobile", "ModelIO")]
		NSString UniversalSceneDescriptionMobile { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[MacCatalyst (13, 1)]
		[Field ("kUTTypeLivePhoto", "+CoreServices")]
		NSString LivePhoto { get; }
	}
}
