
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
		/// <summary>Gets the uniform type identifier for an item.</summary>
		[Field ("kUTTypeItem")]
		NSString Item { get; }

		/// <summary>Gets the uniform type identifier for content.</summary>
		[Field ("kUTTypeContent")]
		NSString Content { get; }

		/// <summary>Gets the uniform type identifier for content with embedded parts.</summary>
		[Field ("kUTTypeCompositeContent")]
		NSString CompositeContent { get; }

		/// <summary>Gets the uniform type identifier for a message.</summary>
		[Field ("kUTTypeMessage")]
		NSString Message { get; }

		/// <summary>Gets the uniform type identifier for a contact.</summary>
		[Field ("kUTTypeContact")]
		NSString Contact { get; }

		/// <summary>Gets the uniform type identifier for an archive.</summary>
		[Field ("kUTTypeArchive")]
		NSString Archive { get; }

		/// <summary>Gets the uniform type identifier for a disk image.</summary>
		[Field ("kUTTypeDiskImage")]
		NSString DiskImage { get; }

		/// <summary>Gets the uniform type identifier for data.</summary>
		[Field ("kUTTypeData")]
		NSString Data { get; }

		/// <summary>Gets the uniform type identifier for a directory.</summary>
		[Field ("kUTTypeDirectory")]
		NSString Directory { get; }

		/// <summary>Gets the uniform type identifier for a resolvable item, such as a symbolic link or alias.</summary>
		[Field ("kUTTypeResolvable")]
		NSString Resolvable { get; }

		/// <summary>Gets the uniform type identifier for a symbolic link.</summary>
		[Field ("kUTTypeSymLink")]
		NSString SymLink { get; }

		/// <summary>Gets the uniform type identifier for executable content.</summary>
		[Field ("kUTTypeExecutable")]
		[MacCatalyst (13, 1)]
		NSString Executable { get; }

		/// <summary>Gets the uniform type identifier for a file-system mount point.</summary>
		[Field ("kUTTypeMountPoint")]
		NSString MountPoint { get; }

		/// <summary>Gets the uniform type identifier for an alias file.</summary>
		[Field ("kUTTypeAliasFile")]
		NSString AliasFile { get; }

		/// <summary>Gets the uniform type identifier for an alias record.</summary>
		[Field ("kUTTypeAliasRecord")]
		NSString AliasRecord { get; }

		/// <summary>Gets the uniform type identifier for URL bookmark data.</summary>
		[Field ("kUTTypeURLBookmarkData")]
		[MacCatalyst (13, 1)]
		NSString URLBookmarkData { get; }

		/// <summary>Gets the uniform type identifier for a URL.</summary>
		[Field ("kUTTypeURL")]
		NSString URL { get; }

		/// <summary>Gets the uniform type identifier for a file URL.</summary>
		[Field ("kUTTypeFileURL")]
		NSString FileURL { get; }

		/// <summary>Gets the uniform type identifier for text.</summary>
		[Field ("kUTTypeText")]
		NSString Text { get; }

		/// <summary>Gets the uniform type identifier for plain text.</summary>
		[Field ("kUTTypePlainText")]
		NSString PlainText { get; }

		/// <summary>Gets the uniform type identifier for UTF-8 plain text.</summary>
		[Field ("kUTTypeUTF8PlainText")]
		NSString UTF8PlainText { get; }

		/// <summary>Gets the uniform type identifier for externally encoded UTF-16 plain text.</summary>
		[Field ("kUTTypeUTF16ExternalPlainText")]
		NSString UTF16ExternalPlainText { get; }

		/// <summary>Gets the uniform type identifier for UTF-16 plain text.</summary>
		[Field ("kUTTypeUTF16PlainText")]
		NSString UTF16PlainText { get; }

		/// <summary>Gets the uniform type identifier for delimited text.</summary>
		[Field ("kUTTypeDelimitedText")]
		[MacCatalyst (13, 1)]
		NSString DelimitedText { get; }

		/// <summary>Gets the uniform type identifier for comma-separated text.</summary>
		[Field ("kUTTypeCommaSeparatedText")]
		[MacCatalyst (13, 1)]
		NSString CommaSeparatedText { get; }

		/// <summary>Gets the uniform type identifier for tab-separated text.</summary>
		[Field ("kUTTypeTabSeparatedText")]
		[MacCatalyst (13, 1)]
		NSString TabSeparatedText { get; }

		/// <summary>Gets the uniform type identifier for UTF-8 tab-separated text.</summary>
		[Field ("kUTTypeUTF8TabSeparatedText")]
		[MacCatalyst (13, 1)]
		NSString UTF8TabSeparatedText { get; }

		/// <summary>Gets the uniform type identifier for rich text in RTF format.</summary>
		[Field ("kUTTypeRTF")]
		NSString RTF { get; }

		/// <summary>Gets the uniform type identifier for HTML content.</summary>
		[Field ("kUTTypeHTML")]
		NSString HTML { get; }

		/// <summary>Gets the uniform type identifier for XML content.</summary>
		[Field ("kUTTypeXML")]
		NSString XML { get; }

		/// <summary>Gets the uniform type identifier for source code.</summary>
		[Field ("kUTTypeSourceCode")]
		NSString SourceCode { get; }

		/// <summary>Gets the uniform type identifier for assembly language source code.</summary>
		[Field ("kUTTypeAssemblyLanguageSource")]
		[MacCatalyst (13, 1)]
		NSString AssemblyLanguageSource { get; }

		/// <summary>Gets the uniform type identifier for C source code.</summary>
		[Field ("kUTTypeCSource")]
		NSString CSource { get; }

		/// <summary>Gets the uniform type identifier for Objective-C source code.</summary>
		[Field ("kUTTypeObjectiveCSource")]
		NSString ObjectiveCSource { get; }

		/// <summary>Gets the uniform type identifier for C++ source code.</summary>
		[Field ("kUTTypeCPlusPlusSource")]
		NSString CPlusPlusSource { get; }

		/// <summary>Gets the uniform type identifier for Objective-C++ source code.</summary>
		[Field ("kUTTypeObjectiveCPlusPlusSource")]
		NSString ObjectiveCPlusPlusSource { get; }

		/// <summary>Gets the uniform type identifier for a C header.</summary>
		[Field ("kUTTypeCHeader")]
		NSString CHeader { get; }

		/// <summary>Gets the uniform type identifier for a C++ header.</summary>
		[Field ("kUTTypeCPlusPlusHeader")]
		NSString CPlusPlusHeader { get; }

		/// <summary>Gets the uniform type identifier for Java source code.</summary>
		[Field ("kUTTypeJavaSource")]
		NSString JavaSource { get; }

		/// <summary>Gets the uniform type identifier for a script.</summary>
		[Field ("kUTTypeScript")]
		[MacCatalyst (13, 1)]
		NSString Script { get; }

		/// <summary>Gets the uniform type identifier for an AppleScript script.</summary>
		[Field ("kUTTypeAppleScript")]
		[MacCatalyst (13, 1)]
		NSString AppleScript { get; }

		/// <summary>Gets the uniform type identifier for an Open Scripting Architecture script.</summary>
		[Field ("kUTTypeOSAScript")]
		[MacCatalyst (13, 1)]
		NSString OSAScript { get; }

		/// <summary>Gets the uniform type identifier for an Open Scripting Architecture script bundle.</summary>
		[Field ("kUTTypeOSAScriptBundle")]
		[MacCatalyst (13, 1)]
		NSString OSAScriptBundle { get; }

		/// <summary>Gets the uniform type identifier for JavaScript source code.</summary>
		[Field ("kUTTypeJavaScript")]
		[MacCatalyst (13, 1)]
		NSString JavaScript { get; }

		/// <summary>Gets the uniform type identifier for a shell script.</summary>
		[Field ("kUTTypeShellScript")]
		[MacCatalyst (13, 1)]
		NSString ShellScript { get; }

		/// <summary>Gets the uniform type identifier for a Perl script.</summary>
		[Field ("kUTTypePerlScript")]
		[MacCatalyst (13, 1)]
		NSString PerlScript { get; }

		/// <summary>Gets the uniform type identifier for a Python script.</summary>
		[Field ("kUTTypePythonScript")]
		[MacCatalyst (13, 1)]
		NSString PythonScript { get; }

		/// <summary>Gets the uniform type identifier for a Ruby script.</summary>
		[Field ("kUTTypeRubyScript")]
		[MacCatalyst (13, 1)]
		NSString RubyScript { get; }

		/// <summary>Gets the uniform type identifier for a PHP script.</summary>
		[Field ("kUTTypePHPScript")]
		[MacCatalyst (13, 1)]
		NSString PHPScript { get; }

		/// <summary>Gets the uniform type identifier for JSON content.</summary>
		[Field ("kUTTypeJSON")]
		[MacCatalyst (13, 1)]
		NSString JSON { get; }

		/// <summary>Gets the uniform type identifier for a property list.</summary>
		[Field ("kUTTypePropertyList")]
		[MacCatalyst (13, 1)]
		NSString PropertyList { get; }

		/// <summary>Gets the uniform type identifier for an XML property list.</summary>
		[Field ("kUTTypeXMLPropertyList")]
		[MacCatalyst (13, 1)]
		NSString XMLPropertyList { get; }

		/// <summary>Gets the uniform type identifier for a binary property list.</summary>
		[Field ("kUTTypeBinaryPropertyList")]
		[MacCatalyst (13, 1)]
		NSString BinaryPropertyList { get; }

		/// <summary>Gets the uniform type identifier for a PDF document.</summary>
		[Field ("kUTTypePDF")]
		NSString PDF { get; }

		/// <summary>Gets the uniform type identifier for a rich text document with attachments in RTFD format.</summary>
		[Field ("kUTTypeRTFD")]
		NSString RTFD { get; }

		/// <summary>Gets the uniform type identifier for a flattened RTFD document.</summary>
		[Field ("kUTTypeFlatRTFD")]
		NSString FlatRTFD { get; }

		/// <summary>Gets the uniform type identifier for Textension text and multimedia data.</summary>
		[Field ("kUTTypeTXNTextAndMultimediaData")]
		NSString TXNTextAndMultimediaData { get; }

		/// <summary>Gets the uniform type identifier for a web archive.</summary>
		[Field ("kUTTypeWebArchive")]
		NSString WebArchive { get; }

		/// <summary>Gets the uniform type identifier for an image.</summary>
		[Field ("kUTTypeImage")]
		NSString Image { get; }

		/// <summary>Gets the uniform type identifier for a JPEG image.</summary>
		[Field ("kUTTypeJPEG")]
		NSString JPEG { get; }

		/// <summary>Gets the uniform type identifier for a JPEG 2000 image.</summary>
		[Field ("kUTTypeJPEG2000")]
		NSString JPEG2000 { get; }

		/// <summary>Gets the uniform type identifier for a TIFF image.</summary>
		[Field ("kUTTypeTIFF")]
		NSString TIFF { get; }

		/// <summary>Gets the uniform type identifier for a PICT image.</summary>
		[Field ("kUTTypePICT")]
		NSString PICT { get; }

		/// <summary>Gets the uniform type identifier for a GIF image.</summary>
		[Field ("kUTTypeGIF")]
		NSString GIF { get; }

		/// <summary>Gets the uniform type identifier for a PNG image.</summary>
		[Field ("kUTTypePNG")]
		NSString PNG { get; }

		/// <summary>Gets the uniform type identifier for a QuickTime image.</summary>
		[Field ("kUTTypeQuickTimeImage")]
		NSString QuickTimeImage { get; }

		/// <summary>Gets the uniform type identifier for an Apple icon image in ICNS format.</summary>
		[Field ("kUTTypeAppleICNS")]
		NSString AppleICNS { get; }

		/// <summary>Gets the uniform type identifier for a BMP image.</summary>
		[Field ("kUTTypeBMP")]
		NSString BMP { get; }

		/// <summary>Gets the uniform type identifier for an ICO image.</summary>
		[Field ("kUTTypeICO")]
		NSString ICO { get; }

		/// <summary>Gets the uniform type identifier for a RAW image.</summary>
		[Field ("kUTTypeRawImage")]
		[MacCatalyst (13, 1)]
		NSString RawImage { get; }

		/// <summary>Gets the uniform type identifier for a scalable vector graphics image.</summary>
		[Field ("kUTTypeScalableVectorGraphics")]
		[MacCatalyst (13, 1)]
		NSString ScalableVectorGraphics { get; }

		/// <summary>Gets the uniform type identifier for audiovisual content.</summary>
		[Field ("kUTTypeAudiovisualContent")]
		NSString AudiovisualContent { get; }

		/// <summary>Gets the uniform type identifier for a movie.</summary>
		[Field ("kUTTypeMovie")]
		NSString Movie { get; }

		/// <summary>Gets the uniform type identifier for video.</summary>
		[Field ("kUTTypeVideo")]
		NSString Video { get; }

		/// <summary>Gets the uniform type identifier for audio.</summary>
		[Field ("kUTTypeAudio")]
		NSString Audio { get; }

		/// <summary>Gets the uniform type identifier for a QuickTime movie.</summary>
		[Field ("kUTTypeQuickTimeMovie")]
		NSString QuickTimeMovie { get; }

		/// <summary>Gets the uniform type identifier for an MPEG movie.</summary>
		[Field ("kUTTypeMPEG")]
		NSString MPEG { get; }

		/// <summary>Gets the uniform type identifier for MPEG-2 video.</summary>
		[Field ("kUTTypeMPEG2Video")]
		[MacCatalyst (13, 1)]
		NSString MPEG2Video { get; }

		/// <summary>Gets the uniform type identifier for an MPEG-2 transport stream.</summary>
		[Field ("kUTTypeMPEG2TransportStream")]
		[MacCatalyst (13, 1)]
		NSString MPEG2TransportStream { get; }

		/// <summary>Gets the uniform type identifier for MP3 audio.</summary>
		[Field ("kUTTypeMP3")]
		NSString MP3 { get; }

		/// <summary>Gets the uniform type identifier for an MPEG-4 movie.</summary>
		[Field ("kUTTypeMPEG4")]
		NSString MPEG4 { get; }

		/// <summary>Gets the uniform type identifier for MPEG-4 audio.</summary>
		[Field ("kUTTypeMPEG4Audio")]
		NSString MPEG4Audio { get; }

		/// <summary>Gets the uniform type identifier for Apple-protected MPEG-4 audio.</summary>
		[Field ("kUTTypeAppleProtectedMPEG4Audio")]
		NSString AppleProtectedMPEG4Audio { get; }

		/// <summary>Gets the uniform type identifier for Apple-protected MPEG-4 video.</summary>
		[Field ("kUTTypeAppleProtectedMPEG4Video")]
		[MacCatalyst (13, 1)]
		NSString AppleProtectedMPEG4Video { get; }

		/// <summary>Gets the uniform type identifier for an AVI movie.</summary>
		[Field ("kUTTypeAVIMovie")]
		[MacCatalyst (13, 1)]
		NSString AVIMovie { get; }

		/// <summary>Gets the uniform type identifier for audio in Audio Interchange File Format.</summary>
		[Field ("kUTTypeAudioInterchangeFileFormat")]
		[MacCatalyst (13, 1)]
		NSString AudioInterchangeFileFormat { get; }

		/// <summary>Gets the uniform type identifier for waveform audio.</summary>
		[Field ("kUTTypeWaveformAudio")]
		[MacCatalyst (13, 1)]
		NSString WaveformAudio { get; }

		/// <summary>Gets the uniform type identifier for MIDI data.</summary>
		[Field ("kUTTypeMIDIAudio")]
		[MacCatalyst (13, 1)]
		NSString MIDIAudio { get; }

		/// <summary>Gets the uniform type identifier for a playlist.</summary>
		[Field ("kUTTypePlaylist")]
		[MacCatalyst (13, 1)]
		NSString Playlist { get; }

		/// <summary>Gets the uniform type identifier for an M3U playlist.</summary>
		[Field ("kUTTypeM3UPlaylist")]
		[MacCatalyst (13, 1)]
		NSString M3UPlaylist { get; }

		/// <summary>Gets the uniform type identifier for a folder.</summary>
		[Field ("kUTTypeFolder")]
		NSString Folder { get; }

		/// <summary>Gets the uniform type identifier for a volume.</summary>
		[Field ("kUTTypeVolume")]
		NSString Volume { get; }

		/// <summary>Gets the uniform type identifier for a package.</summary>
		[Field ("kUTTypePackage")]
		NSString Package { get; }

		/// <summary>Gets the uniform type identifier for a bundle.</summary>
		[Field ("kUTTypeBundle")]
		NSString Bundle { get; }

		/// <summary>Gets the uniform type identifier for a plug-in bundle.</summary>
		[Field ("kUTTypePluginBundle")]
		[MacCatalyst (13, 1)]
		NSString PluginBundle { get; }

		/// <summary>Gets the uniform type identifier for a Spotlight importer.</summary>
		[Field ("kUTTypeSpotlightImporter")]
		[MacCatalyst (13, 1)]
		NSString SpotlightImporter { get; }

		/// <summary>Gets the uniform type identifier for a Quick Look generator.</summary>
		[Field ("kUTTypeQuickLookGenerator")]
		[MacCatalyst (13, 1)]
		NSString QuickLookGenerator { get; }

		/// <summary>Gets the uniform type identifier for an XPC service.</summary>
		[Field ("kUTTypeXPCService")]
		[MacCatalyst (13, 1)]
		NSString XPCService { get; }

		/// <summary>Gets the uniform type identifier for a framework.</summary>
		[Field ("kUTTypeFramework")]
		NSString Framework { get; }

		/// <summary>Gets the uniform type identifier for an application.</summary>
		[Field ("kUTTypeApplication")]
		NSString Application { get; }

		/// <summary>Gets the uniform type identifier for an application bundle.</summary>
		[Field ("kUTTypeApplicationBundle")]
		NSString ApplicationBundle { get; }

		/// <summary>Gets the uniform type identifier for a single-file application.</summary>
		[Field ("kUTTypeApplicationFile")]
		NSString ApplicationFile { get; }

		/// <summary>Gets the uniform type identifier for a UNIX executable.</summary>
		[Field ("kUTTypeUnixExecutable")]
		[MacCatalyst (13, 1)]
		NSString UnixExecutable { get; }

		/// <summary>Gets the uniform type identifier for a Windows executable.</summary>
		[Field ("kUTTypeWindowsExecutable")]
		[MacCatalyst (13, 1)]
		NSString WindowsExecutable { get; }

		/// <summary>Gets the uniform type identifier for a Java class file.</summary>
		[Field ("kUTTypeJavaClass")]
		[MacCatalyst (13, 1)]
		NSString JavaClass { get; }

		/// <summary>Gets the uniform type identifier for a Java archive.</summary>
		[Field ("kUTTypeJavaArchive")]
		[MacCatalyst (13, 1)]
		NSString JavaArchive { get; }

		/// <summary>Gets the uniform type identifier for a system preferences pane.</summary>
		[Field ("kUTTypeSystemPreferencesPane")]
		[MacCatalyst (13, 1)]
		NSString SystemPreferencesPane { get; }

		/// <summary>Gets the uniform type identifier for a gzip archive.</summary>
		[Field ("kUTTypeGNUZipArchive")]
		[MacCatalyst (13, 1)]
		NSString GNUZipArchive { get; }

		/// <summary>Gets the uniform type identifier for a bzip2 archive.</summary>
		[Field ("kUTTypeBzip2Archive")]
		[MacCatalyst (13, 1)]
		NSString Bzip2Archive { get; }

		/// <summary>Gets the uniform type identifier for a ZIP archive.</summary>
		[Field ("kUTTypeZipArchive")]
		[MacCatalyst (13, 1)]
		NSString ZipArchive { get; }

		/// <summary>Gets the uniform type identifier for a spreadsheet.</summary>
		[Field ("kUTTypeSpreadsheet")]
		[MacCatalyst (13, 1)]
		NSString Spreadsheet { get; }

		/// <summary>Gets the uniform type identifier for a presentation.</summary>
		[Field ("kUTTypePresentation")]
		[MacCatalyst (13, 1)]
		NSString Presentation { get; }

		/// <summary>Gets the uniform type identifier for a database.</summary>
		[Field ("kUTTypeDatabase")]
		[MacCatalyst (13, 1)]
		NSString Database { get; }

		/// <summary>Gets the uniform type identifier for a vCard.</summary>
		[Field ("kUTTypeVCard")]
		NSString VCard { get; }

		/// <summary>Gets the uniform type identifier for a to-do item.</summary>
		[Field ("kUTTypeToDoItem")]
		[MacCatalyst (13, 1)]
		NSString ToDoItem { get; }

		/// <summary>Gets the uniform type identifier for a calendar event.</summary>
		[Field ("kUTTypeCalendarEvent")]
		[MacCatalyst (13, 1)]
		NSString CalendarEvent { get; }

		/// <summary>Gets the uniform type identifier for an email message.</summary>
		[Field ("kUTTypeEmailMessage")]
		[MacCatalyst (13, 1)]
		NSString EmailMessage { get; }

		/// <summary>Gets the uniform type identifier for an internet location.</summary>
		[Field ("kUTTypeInternetLocation")]
		[MacCatalyst (13, 1)]
		NSString InternetLocation { get; }

		/// <summary>Gets the uniform type identifier for Ink text data.</summary>
		[Field ("kUTTypeInkText")]
		NSString InkText { get; }

		/// <summary>Gets the uniform type identifier for a font.</summary>
		[Field ("kUTTypeFont")]
		[MacCatalyst (13, 1)]
		NSString Font { get; }

		/// <summary>Gets the uniform type identifier for a bookmark.</summary>
		[Field ("kUTTypeBookmark")]
		[MacCatalyst (13, 1)]
		NSString Bookmark { get; }

		/// <summary>Gets the uniform type identifier for 3D content.</summary>
		[Field ("kUTType3DContent")]
		[MacCatalyst (13, 1)]
		NSString ThreeDContent { get; }

		/// <summary>Gets the uniform type identifier for a PKCS #12 file.</summary>
		[Field ("kUTTypePKCS12")]
		[MacCatalyst (13, 1)]
		NSString PKCS12 { get; }

		/// <summary>Gets the uniform type identifier for an X.509 certificate.</summary>
		[Field ("kUTTypeX509Certificate")]
		[MacCatalyst (13, 1)]
		NSString X509Certificate { get; }

		/// <summary>Gets the uniform type identifier for an electronic publication.</summary>
		[Field ("kUTTypeElectronicPublication")]
		[MacCatalyst (13, 1)]
		NSString ElectronicPublication { get; }

		/// <summary>Gets the uniform type identifier for a log.</summary>
		[Field ("kUTTypeLog")]
		[MacCatalyst (13, 1)]
		NSString Log { get; }

		/// <summary>Gets the property-list key for exported type declarations.</summary>
		[Field ("kUTExportedTypeDeclarationsKey")]
		NSString ExportedTypeDeclarationsKey { get; }

		/// <summary>Gets the property-list key for imported type declarations.</summary>
		[Field ("kUTImportedTypeDeclarationsKey")]
		NSString ImportedTypeDeclarationsKey { get; }

		/// <summary>Gets the type-declaration key for the type identifier.</summary>
		[Field ("kUTTypeIdentifierKey")]
		NSString IdentifierKey { get; }

		/// <summary>Gets the type-declaration key for tag specifications.</summary>
		[Field ("kUTTypeTagSpecificationKey")]
		NSString TagSpecificationKey { get; }

		/// <summary>Gets the type-declaration key for the identifiers of types to which the declared type conforms.</summary>
		[Field ("kUTTypeConformsToKey")]
		NSString ConformsToKey { get; }

		/// <summary>Gets the type-declaration key for the type description.</summary>
		[Field ("kUTTypeDescriptionKey")]
		NSString DescriptionKey { get; }

		/// <summary>Gets the type-declaration key for an icon file.</summary>
		[Field ("kUTTypeIconFileKey")]
		NSString IconFileKey { get; }

		/// <summary>Gets the type-declaration key for a reference URL.</summary>
		[Field ("kUTTypeReferenceURLKey")]
		NSString ReferenceURLKey { get; }

		/// <summary>Gets the type-declaration key for the declaration version.</summary>
		[Field ("kUTTypeVersionKey")]
		NSString VersionKey { get; }

		/// <summary>Gets the tag class for filename extensions.</summary>
		[Field ("kUTTagClassFilenameExtension")]
		NSString TagClassFilenameExtension { get; }

		/// <summary>Gets the tag class for MIME types.</summary>
		[Field ("kUTTagClassMIMEType")]
		NSString TagClassMIMEType { get; }

		/// <summary>Gets the tag class for pasteboard types.</summary>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Field ("kUTTagClassNSPboardType")]
		NSString TagClassNSPboardType { get; }

		/// <summary>Gets the tag class for HFS type codes.</summary>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Field ("kUTTagClassOSType")]
		NSString TagClassOSType { get; }

		/// <summary>Gets the uniform type identifier for Swift source code.</summary>
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

		/// <summary>Gets the uniform type identifier for a Live Photo.</summary>
		[NoMac]
		[MacCatalyst (13, 1)]
		[Field ("kUTTypeLivePhoto")]
		NSString LivePhoto { get; }
	}
}
