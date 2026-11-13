
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
		[Field ("kUTTypeItem")]
		NSString Item { get; }

		/// <summary>Represents the value associated with the constant kUTTypeContent</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeContent")]
		NSString Content { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCompositeContent</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCompositeContent")]
		NSString CompositeContent { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMessage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMessage")]
		NSString Message { get; }

		/// <summary>Represents the value associated with the constant kUTTypeContact</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeContact")]
		NSString Contact { get; }

		/// <summary>Represents the value associated with the constant kUTTypeArchive</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeArchive")]
		NSString Archive { get; }

		/// <summary>Represents the value associated with the constant kUTTypeDiskImage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeDiskImage")]
		NSString DiskImage { get; }

		/// <summary>Represents the value associated with the constant kUTTypeData</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeData")]
		NSString Data { get; }

		/// <summary>Represents the value associated with the constant kUTTypeDirectory</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeDirectory")]
		NSString Directory { get; }

		/// <summary>Represents the value associated with the constant kUTTypeResolvable</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeResolvable")]
		NSString Resolvable { get; }

		/// <summary>Represents the value associated with the constant kUTTypeSymLink</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeSymLink")]
		NSString SymLink { get; }

		/// <summary>Represents the value associated with the constant kUTTypeExecutable</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeExecutable")]
		[MacCatalyst (13, 1)]
		NSString Executable { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMountPoint</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMountPoint")]
		NSString MountPoint { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAliasFile</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAliasFile")]
		NSString AliasFile { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAliasRecord</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAliasRecord")]
		NSString AliasRecord { get; }

		/// <summary>Represents the value associated with the constant kUTTypeURLBookmarkData</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeURLBookmarkData")]
		[MacCatalyst (13, 1)]
		NSString URLBookmarkData { get; }

		/// <summary>Represents the value associated with the constant kUTTypeURL</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeURL")]
		NSString URL { get; }

		/// <summary>Represents the value associated with the constant kUTTypeFileURL</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeFileURL")]
		NSString FileURL { get; }

		/// <summary>Represents the value associated with the constant kUTTypeText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeText")]
		NSString Text { get; }

		/// <summary>Represents the value associated with the constant kUTTypePlainText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePlainText")]
		NSString PlainText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeUTF8PlainText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeUTF8PlainText")]
		NSString UTF8PlainText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeUTF16ExternalPlainText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeUTF16ExternalPlainText")]
		NSString UTF16ExternalPlainText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeUTF16PlainText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeUTF16PlainText")]
		NSString UTF16PlainText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeDelimitedText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeDelimitedText")]
		[MacCatalyst (13, 1)]
		NSString DelimitedText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCommaSeparatedText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCommaSeparatedText")]
		[MacCatalyst (13, 1)]
		NSString CommaSeparatedText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeTabSeparatedText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeTabSeparatedText")]
		[MacCatalyst (13, 1)]
		NSString TabSeparatedText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeUTF8TabSeparatedText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeUTF8TabSeparatedText")]
		[MacCatalyst (13, 1)]
		NSString UTF8TabSeparatedText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeRTF</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeRTF")]
		NSString RTF { get; }

		/// <summary>Represents the value associated with the constant kUTTypeHTML</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeHTML")]
		NSString HTML { get; }

		/// <summary>Represents the value associated with the constant kUTTypeXML</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeXML")]
		NSString XML { get; }

		/// <summary>Represents the value associated with the constant kUTTypeSourceCode</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeSourceCode")]
		NSString SourceCode { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAssemblyLanguageSource</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAssemblyLanguageSource")]
		[MacCatalyst (13, 1)]
		NSString AssemblyLanguageSource { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCSource</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCSource")]
		NSString CSource { get; }

		/// <summary>Represents the value associated with the constant kUTTypeObjectiveCSource</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeObjectiveCSource")]
		NSString ObjectiveCSource { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCPlusPlusSource</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCPlusPlusSource")]
		NSString CPlusPlusSource { get; }

		/// <summary>Represents the value associated with the constant kUTTypeObjectiveCPlusPlusSource</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeObjectiveCPlusPlusSource")]
		NSString ObjectiveCPlusPlusSource { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCHeader</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCHeader")]
		NSString CHeader { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCPlusPlusHeader</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCPlusPlusHeader")]
		NSString CPlusPlusHeader { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJavaSource</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJavaSource")]
		NSString JavaSource { get; }

		/// <summary>Represents the value associated with the constant kUTTypeScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeScript")]
		[MacCatalyst (13, 1)]
		NSString Script { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAppleScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAppleScript")]
		[MacCatalyst (13, 1)]
		NSString AppleScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypeOSAScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeOSAScript")]
		[MacCatalyst (13, 1)]
		NSString OSAScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypeOSAScriptBundle</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeOSAScriptBundle")]
		[MacCatalyst (13, 1)]
		NSString OSAScriptBundle { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJavaScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJavaScript")]
		[MacCatalyst (13, 1)]
		NSString JavaScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypeShellScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeShellScript")]
		[MacCatalyst (13, 1)]
		NSString ShellScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypePerlScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePerlScript")]
		[MacCatalyst (13, 1)]
		NSString PerlScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypePythonScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePythonScript")]
		[MacCatalyst (13, 1)]
		NSString PythonScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypeRubyScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeRubyScript")]
		[MacCatalyst (13, 1)]
		NSString RubyScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypePHPScript</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePHPScript")]
		[MacCatalyst (13, 1)]
		NSString PHPScript { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJSON</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJSON")]
		[MacCatalyst (13, 1)]
		NSString JSON { get; }

		/// <summary>Represents the value associated with the constant kUTTypePropertyList</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePropertyList")]
		[MacCatalyst (13, 1)]
		NSString PropertyList { get; }

		/// <summary>Represents the value associated with the constant kUTTypeXMLPropertyList</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeXMLPropertyList")]
		[MacCatalyst (13, 1)]
		NSString XMLPropertyList { get; }

		/// <summary>Represents the value associated with the constant kUTTypeBinaryPropertyList</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeBinaryPropertyList")]
		[MacCatalyst (13, 1)]
		NSString BinaryPropertyList { get; }

		/// <summary>Represents the value associated with the constant kUTTypePDF</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePDF")]
		NSString PDF { get; }

		/// <summary>Represents the value associated with the constant kUTTypeRTFD</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeRTFD")]
		NSString RTFD { get; }

		/// <summary>Represents the value associated with the constant kUTTypeFlatRTFD</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeFlatRTFD")]
		NSString FlatRTFD { get; }

		/// <summary>Represents the value associated with the constant kUTTypeTXNTextAndMultimediaData</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeTXNTextAndMultimediaData")]
		NSString TXNTextAndMultimediaData { get; }

		/// <summary>Represents the value associated with the constant kUTTypeWebArchive</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeWebArchive")]
		NSString WebArchive { get; }

		/// <summary>Represents the value associated with the constant kUTTypeImage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeImage")]
		NSString Image { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJPEG</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJPEG")]
		NSString JPEG { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJPEG2000</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJPEG2000")]
		NSString JPEG2000 { get; }

		/// <summary>Represents the value associated with the constant kUTTypeTIFF</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeTIFF")]
		NSString TIFF { get; }

		/// <summary>Represents the value associated with the constant kUTTypePICT</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePICT")]
		NSString PICT { get; }

		/// <summary>Represents the value associated with the constant kUTTypeGIF</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeGIF")]
		NSString GIF { get; }

		/// <summary>Represents the value associated with the constant kUTTypePNG</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePNG")]
		NSString PNG { get; }

		/// <summary>Represents the value associated with the constant kUTTypeQuickTimeImage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeQuickTimeImage")]
		NSString QuickTimeImage { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAppleICNS</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAppleICNS")]
		NSString AppleICNS { get; }

		/// <summary>Represents the value associated with the constant kUTTypeBMP</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeBMP")]
		NSString BMP { get; }

		/// <summary>Represents the value associated with the constant kUTTypeICO</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeICO")]
		NSString ICO { get; }

		/// <summary>Represents the value associated with the constant kUTTypeRawImage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeRawImage")]
		[MacCatalyst (13, 1)]
		NSString RawImage { get; }

		/// <summary>Represents the value associated with the constant kUTTypeScalableVectorGraphics</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeScalableVectorGraphics")]
		[MacCatalyst (13, 1)]
		NSString ScalableVectorGraphics { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAudiovisualContent</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAudiovisualContent")]
		NSString AudiovisualContent { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMovie</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMovie")]
		NSString Movie { get; }

		/// <summary>Represents the value associated with the constant kUTTypeVideo</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeVideo")]
		NSString Video { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAudio</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAudio")]
		NSString Audio { get; }

		/// <summary>Represents the value associated with the constant kUTTypeQuickTimeMovie</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeQuickTimeMovie")]
		NSString QuickTimeMovie { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMPEG</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMPEG")]
		NSString MPEG { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMPEG2Video</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMPEG2Video")]
		[MacCatalyst (13, 1)]
		NSString MPEG2Video { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMPEG2TransportStream</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMPEG2TransportStream")]
		[MacCatalyst (13, 1)]
		NSString MPEG2TransportStream { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMP3</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMP3")]
		NSString MP3 { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMPEG4</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMPEG4")]
		NSString MPEG4 { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMPEG4Audio</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMPEG4Audio")]
		NSString MPEG4Audio { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAppleProtectedMPEG4Audio</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAppleProtectedMPEG4Audio")]
		NSString AppleProtectedMPEG4Audio { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAppleProtectedMPEG4Video</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAppleProtectedMPEG4Video")]
		[MacCatalyst (13, 1)]
		NSString AppleProtectedMPEG4Video { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAVIMovie</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAVIMovie")]
		[MacCatalyst (13, 1)]
		NSString AVIMovie { get; }

		/// <summary>Represents the value associated with the constant kUTTypeAudioInterchangeFileFormat</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeAudioInterchangeFileFormat")]
		[MacCatalyst (13, 1)]
		NSString AudioInterchangeFileFormat { get; }

		/// <summary>Represents the value associated with the constant kUTTypeWaveformAudio</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeWaveformAudio")]
		[MacCatalyst (13, 1)]
		NSString WaveformAudio { get; }

		/// <summary>Represents the value associated with the constant kUTTypeMIDIAudio</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeMIDIAudio")]
		[MacCatalyst (13, 1)]
		NSString MIDIAudio { get; }

		/// <summary>Represents the value associated with the constant kUTTypePlaylist</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePlaylist")]
		[MacCatalyst (13, 1)]
		NSString Playlist { get; }

		/// <summary>Represents the value associated with the constant kUTTypeM3UPlaylist</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeM3UPlaylist")]
		[MacCatalyst (13, 1)]
		NSString M3UPlaylist { get; }

		/// <summary>Represents the value associated with the constant kUTTypeFolder</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeFolder")]
		NSString Folder { get; }

		/// <summary>Represents the value associated with the constant kUTTypeVolume</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeVolume")]
		NSString Volume { get; }

		/// <summary>Represents the value associated with the constant kUTTypePackage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePackage")]
		NSString Package { get; }

		/// <summary>Represents the value associated with the constant kUTTypeBundle</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeBundle")]
		NSString Bundle { get; }

		/// <summary>Represents the value associated with the constant kUTTypePluginBundle</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePluginBundle")]
		[MacCatalyst (13, 1)]
		NSString PluginBundle { get; }

		/// <summary>Represents the value associated with the constant kUTTypeSpotlightImporter</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeSpotlightImporter")]
		[MacCatalyst (13, 1)]
		NSString SpotlightImporter { get; }

		/// <summary>Represents the value associated with the constant kUTTypeQuickLookGenerator</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeQuickLookGenerator")]
		[MacCatalyst (13, 1)]
		NSString QuickLookGenerator { get; }

		/// <summary>Represents the value associated with the constant kUTTypeXPCService</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeXPCService")]
		[MacCatalyst (13, 1)]
		NSString XPCService { get; }

		/// <summary>Represents the value associated with the constant kUTTypeFramework</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeFramework")]
		NSString Framework { get; }

		/// <summary>Represents the value associated with the constant kUTTypeApplication</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeApplication")]
		NSString Application { get; }

		/// <summary>Represents the value associated with the constant kUTTypeApplicationBundle</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeApplicationBundle")]
		NSString ApplicationBundle { get; }

		/// <summary>Represents the value associated with the constant kUTTypeApplicationFile</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeApplicationFile")]
		NSString ApplicationFile { get; }

		/// <summary>Represents the value associated with the constant kUTTypeUnixExecutable</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeUnixExecutable")]
		[MacCatalyst (13, 1)]
		NSString UnixExecutable { get; }

		/// <summary>Represents the value associated with the constant kUTTypeWindowsExecutable</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeWindowsExecutable")]
		[MacCatalyst (13, 1)]
		NSString WindowsExecutable { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJavaClass</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJavaClass")]
		[MacCatalyst (13, 1)]
		NSString JavaClass { get; }

		/// <summary>Represents the value associated with the constant kUTTypeJavaArchive</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeJavaArchive")]
		[MacCatalyst (13, 1)]
		NSString JavaArchive { get; }

		/// <summary>Represents the value associated with the constant kUTTypeSystemPreferencesPane</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeSystemPreferencesPane")]
		[MacCatalyst (13, 1)]
		NSString SystemPreferencesPane { get; }

		/// <summary>Represents the value associated with the constant kUTTypeGNUZipArchive</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeGNUZipArchive")]
		[MacCatalyst (13, 1)]
		NSString GNUZipArchive { get; }

		/// <summary>Represents the value associated with the constant kUTTypeBzip2Archive</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeBzip2Archive")]
		[MacCatalyst (13, 1)]
		NSString Bzip2Archive { get; }

		/// <summary>Represents the value associated with the constant kUTTypeZipArchive</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeZipArchive")]
		[MacCatalyst (13, 1)]
		NSString ZipArchive { get; }

		/// <summary>Represents the value associated with the constant kUTTypeSpreadsheet</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeSpreadsheet")]
		[MacCatalyst (13, 1)]
		NSString Spreadsheet { get; }

		/// <summary>Represents the value associated with the constant kUTTypePresentation</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePresentation")]
		[MacCatalyst (13, 1)]
		NSString Presentation { get; }

		/// <summary>Represents the value associated with the constant kUTTypeDatabase</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeDatabase")]
		[MacCatalyst (13, 1)]
		NSString Database { get; }

		/// <summary>Represents the value associated with the constant kUTTypeVCard</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeVCard")]
		NSString VCard { get; }

		/// <summary>Represents the value associated with the constant kUTTypeToDoItem</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeToDoItem")]
		[MacCatalyst (13, 1)]
		NSString ToDoItem { get; }

		/// <summary>Represents the value associated with the constant kUTTypeCalendarEvent</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeCalendarEvent")]
		[MacCatalyst (13, 1)]
		NSString CalendarEvent { get; }

		/// <summary>Represents the value associated with the constant kUTTypeEmailMessage</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeEmailMessage")]
		[MacCatalyst (13, 1)]
		NSString EmailMessage { get; }

		/// <summary>Represents the value associated with the constant kUTTypeInternetLocation</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeInternetLocation")]
		[MacCatalyst (13, 1)]
		NSString InternetLocation { get; }

		/// <summary>Represents the value associated with the constant kUTTypeInkText</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeInkText")]
		NSString InkText { get; }

		/// <summary>Represents the value associated with the constant kUTTypeFont</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeFont")]
		[MacCatalyst (13, 1)]
		NSString Font { get; }

		/// <summary>Represents the value associated with the constant kUTTypeBookmark</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeBookmark")]
		[MacCatalyst (13, 1)]
		NSString Bookmark { get; }

		/// <summary>Represents the value associated with the constant kUTType3DContent</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTType3DContent")]
		[MacCatalyst (13, 1)]
		NSString ThreeDContent { get; }

		/// <summary>Represents the value associated with the constant kUTTypePKCS12</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypePKCS12")]
		[MacCatalyst (13, 1)]
		NSString PKCS12 { get; }

		/// <summary>Represents the value associated with the constant kUTTypeX509Certificate</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeX509Certificate")]
		[MacCatalyst (13, 1)]
		NSString X509Certificate { get; }

		/// <summary>Represents the value associated with the constant kUTTypeElectronicPublication</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeElectronicPublication")]
		[MacCatalyst (13, 1)]
		NSString ElectronicPublication { get; }

		/// <summary>Represents the value associated with the constant kUTTypeLog</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeLog")]
		[MacCatalyst (13, 1)]
		NSString Log { get; }

		/// <summary>Represents the value associated with the constant kUTExportedTypeDeclarationsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTExportedTypeDeclarationsKey")]
		NSString ExportedTypeDeclarationsKey { get; }

		/// <summary>Represents the value associated with the constant kUTImportedTypeDeclarationsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTImportedTypeDeclarationsKey")]
		NSString ImportedTypeDeclarationsKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeIdentifierKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeIdentifierKey")]
		NSString IdentifierKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeTagSpecificationKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeTagSpecificationKey")]
		NSString TagSpecificationKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeConformsToKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeConformsToKey")]
		NSString ConformsToKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeDescriptionKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeDescriptionKey")]
		NSString DescriptionKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeIconFileKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeIconFileKey")]
		NSString IconFileKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeReferenceURLKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeReferenceURLKey")]
		NSString ReferenceURLKey { get; }

		/// <summary>Represents the value associated with the constant kUTTypeVersionKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTypeVersionKey")]
		NSString VersionKey { get; }

		/// <summary>Represents the value associated with the constant kUTTagClassFilenameExtension</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTagClassFilenameExtension")]
		NSString TagClassFilenameExtension { get; }

		/// <summary>Represents the value associated with the constant kUTTagClassMIMEType</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("kUTTagClassMIMEType")]
		NSString TagClassMIMEType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Field ("kUTTagClassNSPboardType")]
		NSString TagClassNSPboardType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Field ("kUTTagClassOSType")]
		NSString TagClassOSType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kUTTypeSwiftSource")]
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
		[Field ("kUTTypeLivePhoto")]
		NSString LivePhoto { get; }
	}
}
