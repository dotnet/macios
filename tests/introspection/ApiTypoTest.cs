//
// Test the generated API selectors against typos or non-existing cases
//
// Authors:
//	Paola Villarreal  <paola.villarreal@xamarin.com>
//
// Copyright 2015 Xamarin Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
#if MONOMAC
using AppKit;
using SpellChecker = AppKit.NSSpellChecker;
#else
using UIKit;
using SpellChecker = UIKit.UITextChecker;
#endif
using Xamarin.Tests;
using Xamarin.Utils;

#nullable enable

namespace Introspection {
	[TestFixture]
	public class ApiTypoTest : ApiBaseTest {
		const ApplePlatform All = ApplePlatform.MacOSX | ApplePlatform.iOS | ApplePlatform.TVOS | ApplePlatform.MacCatalyst;

		public ApiTypoTest ()
		{
			ContinueOnFailure = true;
		}

		readonly HashSet<string> allowedRule3 = new HashSet<string> {
			"IARAnchorCopying", // We're showing a code snippet in the 'Advice' message and that shouldn't end with a dot.
		};

		HashSet<string> allowedMemberRule4 = new HashSet<string> {
			"Platform",
			"PlatformHelper",
			"AvailabilityAttribute",
			"iOSAttribute",
			"MacAttribute",
		};

		Dictionary<string, ApplePlatform> allowed = new Dictionary<string, ApplePlatform> () {
			{ "Aac", All },
			{ "Abgr", All },
			{ "Achivements", All },
			{ "Ack", All }, // acknowledgment
			{ "Acn", All }, // Ambisonic Channel Numbering
			{ "Acos", All },
			{ "Acosh", All },
			{ "Activatable", All },
			{ "Addin", All },
			{ "Addl", All },
			{ "Addons", ApplePlatform.MacOSX },
			{ "Addr", All },
			{ "Adessive", All },
			{ "Adposition", All }, // linguistic term
			{ "Aes", All }, // Advanced Encryption Standard
			{ "Afi", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Agc", All },
			{ "Ahap", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Aifc", All },
			{ "Aiff", All },
			{ "Aime", ApplePlatform.MacOSX },
			{ "Aio", ApplePlatform.MacOSX },
			{ "Alg", All }, // short for Algorithm
			{ "Alem", All }, // Ethiopic "Amete Alem" calendar
			{ "Aliasable", All },
			{ "Allative", All },
			{ "Amete", All },
			{ "Amr", All },
			{ "Ancs", All & ~ApplePlatform.MacOSX },
			{ "Ane", All },
			{ "Anglet", All },
			{ "Apac", All },
			{ "Apdu", All },
			{ "Apl", All & ~ApplePlatform.TVOS },
			{ "Apng", All }, // Animated Portable Network Graphics
			{ "Apns", All & ~ApplePlatform.TVOS },
			{ "Applei", All },
			{ "Aps", ApplePlatform.MacOSX },
			{ "Apv", ApplePlatform.MacOSX },
			{ "Arcball", All },
			{ "Argb", All },
			{ "Arraycollation", All & ~ApplePlatform.MacOSX },
			{ "Asin", All },
			{ "Asinh", All },
			{ "Astc", All },
			{ "Atan", All },
			{ "Atanh", All },
			{ "Atm", All },
			{ "Atmos", All }, // Dolby Atmos
			{ "Atr", All },
			{ "Ats", All }, // App Transport Security
			{ "Atsc", All },
			{ "Attr", ApplePlatform.MacOSX },
			{ "Attrib", All },
			{ "Attributevalue", All },
			{ "Attrs", All }, // Attributes (used by Apple for keys)
			{ "Audiofile", All },
			{ "Audiograph", ApplePlatform.MacOSX },
			{ "Authenticatable", ApplePlatform.MacOSX },
			{ "Automapping", All },
			{ "Automatch", All },
			{ "Automounted", All },
			{ "Autoredirect", ApplePlatform.MacCatalyst | ApplePlatform.TVOS },
			{ "Autospace", ApplePlatform.MacOSX },
			{ "Autostarts", ApplePlatform.MacOSX },
			{ "Avb", All }, // acronym: Audio Video Bridging
			{ "Avci", All }, // file type
			{ "Avg", All },
			{ "Axept", All & ~ApplePlatform.TVOS },
			{ "Bancomat", All & ~ApplePlatform.TVOS },
			{ "Bancaires", All & ~ApplePlatform.TVOS }, // Cartes Bancaires payment network
			{ "Bary", All },
			{ "Ber", All },
			{ "Bggr", All }, // acronym for Blue, Green, Green, Red
			{ "Bgra", All }, // acrnym for Blue, Green, Red, Alpha
			{ "Bgrx", All },
			{ "Bim", All },
			{ "Bitangent", All },
			{ "Blinn", All },
			{ "Blit", All },
			{ "Blockmap", ApplePlatform.MacOSX },
			{ "Blockquote", ApplePlatform.MacOSX },
			{ "Brotli", All },
			{ "Bsd", ApplePlatform.MacOSX },
			{ "Bsln", All },
			{ "Bssid", All & ~ApplePlatform.TVOS },
			{ "Btle", ApplePlatform.MacOSX }, // Bluetooth Low Energy
			{ "Cabac", All },
			{ "Caf", All }, // acronym: Core Audio Format
			{ "Callables", All },
			{ "Cartes", All & ~ApplePlatform.TVOS }, // french
			{ "Catmull", All },
			{ "Cavlc", All },
			{ "Ccitt", ApplePlatform.MacOSX },
			{ "Cbc", All }, // Cipher Block Chaining
			{ "Cct", All },
			{ "Ccw", All },
			{ "Cda", All & ~ApplePlatform.TVOS }, // acronym: Clinical Document Architecture
			{ "Cdf", All }, // Cumulative Distribution Function
			{ "Cdma", All },
			{ "Cdrom", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Cea", All },
			{ "Celp", All }, // MPEG4ObjectID
			{ "Celu", All }, // Continuously Differentiable Exponential Linear Unit (ML)
			{ "Cfa", All }, // acronym: Color Filter Array
			{ "Chacha", All },
			{ "Chapv", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Characterteristic", All },
			{ "Cholesky", All },
			{ "Chromaticities", All },
			{ "Chw", All },
			{ "Ciexyz", ApplePlatform.MacOSX },
			{ "Cif", All }, // Common Intermediate Format
			{ "Ciff", All },
			{ "Cinemagraph", ApplePlatform.TVOS },
			{ "Cinepak", All },
			{ "Ciphersuite", All }, // TLS cipher suite
			{ "Cla", All },
			{ "Clearcoat", All },
			{ "Clockstamp", All },
			{ "Cmaf", All }, // Common Media Application Format (mpeg4)
			{ "Cmy", ApplePlatform.MacOSX }, // acronym: Cyan, magenta, yellow
			{ "Cmyk", All }, // acronym: Cyan, magenta, yellow and key
			{ "Cmyka", ApplePlatform.MacOSX },
			{ "Cnn", All }, // Convolutional Neural Network
			{ "Cns", ApplePlatform.MacOSX },
			{ "Codabar", All },
			{ "Commited", ApplePlatform.MacOSX }, // CommitedLoad - will be renamed in XAMCORE_5_0
			{ "Conf", All }, // configuration abbreviation
			{ "Conecs", All & ~ApplePlatform.TVOS },
			{ "Conv", All },
			{ "Cooldown", All & ~ApplePlatform.TVOS },
			{ "Copyback", All },
			{ "Cose", All & ~ApplePlatform.TVOS },
			{ "Crosstraining", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Csr", All },
			{ "Csc", All }, // cosecant
			{ "Ctm", ApplePlatform.MacOSX },
			{ "Ctor", All },
			{ "Cubemap", All },
			{ "Cymk", ApplePlatform.MacOSX },
			{ "Cymka", ApplePlatform.MacOSX },
			{ "Daap", All },
			{ "Dangi", All },
			{ "Dankort", All & ~ApplePlatform.TVOS },
			{ "Dav", All & ~ApplePlatform.TVOS },
			{ "Dcip", All }, // acronym: Digital Cinema Implementation Partners
			{ "Deca", All & ~ApplePlatform.TVOS },
			{ "Decomposables", All },
			{ "Deinterlace", All },
			{ "Denoise", All },
			{ "Denoised", All },
			{ "Denoiser", All }, // noise reduction filter
			{ "Depthwise", All },
			{ "Dequantize", All },
			{ "Dfsi", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Dhe", All }, // Diffie–Hellman key exchange
			{ "Dhs", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Dhwio", All },
			{ "Dicom", All },
			{ "Diconnection", All },
			{ "Diffable", All }, // that you can diff it.. made up word from apple
			{ "Diffie", All },
			{ "Dirbursement", All & ~ApplePlatform.TVOS },
			{ "Dirs", ApplePlatform.MacOSX },
			{ "Dismissable", ApplePlatform.MacOSX },
			{ "Dist", All },
			{ "Distinguised", ApplePlatform.MacOSX }, // ITLibPlaylistPropertyDistinguisedKind
			{ "dlclose", All },
			{ "dlerror", All },
			{ "Directionfor", All & ~ApplePlatform.MacOSX }, // SetBaseWritingDirectionforRange - will be renamed in XAMCORE_5_0
			{ "Dlfcn", All },
			{ "Dls", ApplePlatform.MacOSX },
			{ "Dng", All },
			{ "Dnssec", All },
			{ "Dont", All },
			{ "Dop", All },
			{ "Dopesheet", All },
			{ "Downmix", All }, // Sound terminology that means making a stereo mix from a 5.1 surround mix.
			{ "Dpa", All },
			{ "Dpad", All }, // Directional pad (D-pad)
			{ "Dpads", All }, // plural of above
			{ "Drm", ApplePlatform.MacOSX }, // MediaItemProperty.IsDrmProtected
			{ "Droste", All },
			{ "Dsf", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Dsfi", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Dstu", All & ~ApplePlatform.TVOS },
			{ "Dtls", All },
			{ "Dtmf", ApplePlatform.iOS | ApplePlatform.MacCatalyst }, // DTMF
			{ "Dtss", ApplePlatform.MacOSX },
			{ "dy", All },
			{ "Eap", All },
			{ "Ean", All }, // European Article Number (barcode standard)
			{ "Ebu", All },
			{ "Ecc", All }, // Elliptic Curve Cryptography
			{ "Ecdh", All }, // Elliptic Curve Diffie–Hellman
			{ "Ecdhe", All }, // Elliptic Curve Diffie-Hellman Ephemeral
			{ "Ecdsa", All }, // Elliptic Curve Digital Signature Algorithm
			{ "Ecg", All & ~ApplePlatform.TVOS },
			{ "Echos", ApplePlatform.MacOSX }, // plural of echo
			{ "Ecies", All }, // Elliptic Curve Integrated Encryption Scheme
			{ "Ecn", All }, // Explicit Congestion Notification
			{ "Ect", All }, // ECN Capable Transport
			{ "Editability", All & ~ApplePlatform.MacOSX },
			{ "Edr", All },
			{ "Eftpos", All & ~ApplePlatform.TVOS }, // Electronic funds transfer at point of sale
			{ "Eisu", ApplePlatform.MacOSX },
			{ "Elative", All },
			{ "Elu", All },
			{ "Emagic", All },
			{ "Embd", All },
			{ "Emebedding", All },
			{ "Emsg", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst }, // 4cc
			{ "Enc", All },
			{ "Endc", All },
			{ "Eof", All }, // acronym End-Of-File
			{ "Eppc", All },
			{ "Epub", All },
			{ "Erf", All },
			{ "Essive", All },
			{ "Evdo", All },
			{ "Evictable", ApplePlatform.MacOSX | ApplePlatform.iOS },
			{ "Exabits", All },
			{ "Exbibits", All },
			{ "Exbibytes", All },
			{ "Exp", All }, // exponent/exponential
			{ "Expr", All },
			{ "Exr", All },
			{ "Extrinsics", All },
			{ "Fcp", All }, // Apple ATS Forward Compatibility Policy
			{ "Feli", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Felica", All & ~ApplePlatform.TVOS }, // Japanese contactless RFID smart card system
			{ "Femtowatts", All },
			{ "Fft", All },
			{ "Fhir", All & ~ApplePlatform.TVOS },
			{ "Fieldset", All & ~ApplePlatform.MacCatalyst },
			{ "Formati", All },
			{ "Fov", All },
			{ "Fqdns", All },
			{ "Framebuffer", All },
			{ "Framesetter", All },
			{ "Freq", All },
			{ "Froms", ApplePlatform.MacOSX }, // NSMetadataItemWhereFromsKey
			{ "Ftps", All },
			{ "Func", All }, // function abbreviation
			{ "Gadu", All & ~ApplePlatform.TVOS },
			{ "Gainmap", All },
			{ "Gbrg", All }, // acronym for Green-Blue-Reg-Green
			{ "Gbtac", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Gbtdc", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Gcm", All },
			{ "Gelu", All }, // Gaussian Error Linear Unit (ML)
			{ "Gen", All }, // generation (e.g. SiriRemote1stGen)
			{ "Gibibits", All },
			{ "Gid", ApplePlatform.MacOSX },
			{ "Gigapascals", All },
			{ "Girocard", All & ~ApplePlatform.TVOS },
			{ "Gles", ApplePlatform.iOS | ApplePlatform.TVOS },
			{ "Glorot", All }, // NN
			{ "Gop", All }, // acronym for Group Of Pictures
			{ "Gpp", All },
			{ "Gps", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Gsm", All }, // Global System for Mobile Communications
			{ "Grbg", All }, // acronym for Green-Red-Blue-Green
			{ "Groupless", All & ~ApplePlatform.TVOS },
			{ "Gru", All },
			{ "Gtin", All },
			{ "Gui", All },
			{ "Handwashing", All & ~ApplePlatform.TVOS },
			{ "Hankaku", All & ~ApplePlatform.MacOSX },
			{ "Hardlink", ApplePlatform.MacOSX },
			{ "Hdmi", All & ~ApplePlatform.MacOSX },
			{ "Hdr", All },
			{ "Heic", All }, // file type
			{ "Heics", All }, // High Efficiency Image File Format (Sequence)
			{ "Heif", All }, // High Efficiency Image File Format
			{ "Hectopascals", All },
			{ "Hevc", All }, // CMVideoCodecType / High Efficiency Video Coding
			{ "Hfp", All & ~ApplePlatform.MacOSX },
			{ "Hhr", All },
			{ "Himyan", All & ~ApplePlatform.TVOS },
			{ "Hermitean", All }, // Apple's spelling of Hermitian in MPSGraph FFT methods
			{ "Hindlegs", All },
			{ "Hipass", All },
			{ "Histogrammed", All & ~ApplePlatform.TVOS },
			{ "Hlg", All }, // Hybrid Log-Gamma
			{ "Hls", All },
			{ "Hoa", All },
			{ "Hpke", ApplePlatform.MacOSX },
			{ "Hrtf", All }, // acronym used in AUSpatializationAlgorithm
			{ "Hsb", ApplePlatform.MacOSX },
			{ "Hsba", ApplePlatform.MacOSX },
			{ "Hvxc", All }, // MPEG4ObjectID
			{ "Hwc", All },
			{ "Hwio", All },
			{ "Iap", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Ibss", ApplePlatform.MacOSX },
			{ "Icns", All },
			{ "Ico", All },
			{ "Icq", All & ~ApplePlatform.TVOS },
			{ "Identd", All },
			{ "Ident", All }, // identifier abbreviation
			{ "Iec", All },
			{ "Ies", All },
			{ "Ikev", All }, // Internet Key Exchange v2
			{ "Ima", All }, // Interactive Multimedia Association
			{ "Imageblock", All },
			{ "Imap", All },
			{ "Imaps", All },
			{ "Imei", All & ~ApplePlatform.MacOSX },
			{ "Img", All },
			{ "Impl", All }, // BindingImplAttribute
			{ "Incrementor", ApplePlatform.MacOSX },
			{ "Indoorcycle", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Indoorrun", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Indoorwalk", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Inessive", All },
			{ "Ingles", All }, // Inglés locale identifier
			{ "Inklist", All },
			{ "Inode", ApplePlatform.MacOSX },
			{ "Inser", All },
			{ "Instamatic", ApplePlatform.MacOSX },
			{ "Interac", All & ~ApplePlatform.TVOS },
			{ "Interactable", ApplePlatform.MacOSX },
			{ "Interframe", All },
			{ "Interitem", All },
			{ "Intermenstrual", All & ~ApplePlatform.TVOS },
			{ "Intravaginal", All & ~ApplePlatform.TVOS },
			{ "Inv", All },
			{ "Invitable", All },
			{ "Iou", All },
			{ "Ipa", All },
			{ "Ipp", All },
			{ "Iptc", All },
			{ "Ircs", All },
			{ "Isrc", All },
			{ "Itf", All },
			{ "Itt", All & ~ApplePlatform.TVOS },
			{ "Itu", All },
			{ "Itur", All }, // Itur_2020_Hlg
			{ "Jaywan", All & ~ApplePlatform.TVOS },
			{ "Jcb", All & ~ApplePlatform.TVOS }, // Japanese credit card company
			{ "Jfif", All },
			{ "Jis", ApplePlatform.MacOSX },
			{ "Jrts", All & ~ApplePlatform.TVOS },
			// "Jws" - HKVerifiableClinicalRecord is [ObsoletedOSPlatform] on iOS/MacCatalyst but not macOS
			{ "Jws", ApplePlatform.MacOSX }, // JSON Web Signature
			{ "Jwks", ApplePlatform.MacOSX },
			{ "Jwt", ApplePlatform.MacOSX },
			{ "Keepalive", All },
			{ "Keycode", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Keyerror", All },
			{ "Keyi", All },
			{ "Keypath", ApplePlatform.MacOSX },
			{ "Keypoint", All },
			{ "Keypoints", All },
			{ "Kibibits", All },
			{ "Kickboard", All & ~ApplePlatform.TVOS },
			{ "Kiloampere", All },
			{ "Kiloamperes", All },
			{ "Kiloohms", All },
			{ "Kilopascals", All },
			{ "ks", All },
			{ "Kullback", All }, // Kullback-Leibler Divergence
			{ "Lacunarity", All },
			{ "Latm", All }, //  Low Overhead Audio Transport Multiplex
			{ "Lbc", All },
			{ "Ldaps", All },
			{ "Leibler", All }, // Kullback-Leibler divergence
			{ "Lerp", All },
			{ "libcompression", All },
			{ "libdispatch", All },
			{ "Lingustic", All },
			{ "Lite", All }, // lightweight variant
			{ "Loas", All }, // Low Overhead Audio Stream
			{ "Lod", All },
			{ "Lopass", All },
			{ "Lowlevel", All },
			{ "Lpcm", All },
			{ "Lsb", All }, // Least Significant Bit
			{ "Lstm", All },
			{ "Lte", All },
			{ "Ltp", All }, // AAC Long Term Prediction
			{ "Ltr", All },
			{ "Luma", All }, // luminance component in video
			{ "Lun", All },
			{ "Lut", All },
			{ "Lzfse", All }, // acronym
			{ "Lzma", All }, // acronym
			{ "Lzw", ApplePlatform.MacOSX },
			{ "Mada", All & ~ApplePlatform.TVOS }, // payment system
			{ "Mcp", All }, // metacarpophalangeal (hand)
			{ "Mebibits", All },
			{ "Mebx", All },
			{ "Meeza", All & ~ApplePlatform.TVOS },
			{ "Megaampere", All },
			{ "Megaamperes", All },
			{ "Megaliters", All },
			{ "Megameters", All },
			{ "Megaohms", All },
			{ "Megapascals", All },
			{ "Mennekes", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Metacharacters", All },
			{ "Metadatas", All },
			{ "Metalness", All },
			{ "Mgmt", All },
			{ "Microampere", All },
			{ "Microamperes", All },
			{ "Microohms", All },
			{ "Microwatts", All },
			{ "Mifare", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Mihret", All }, // Ethiopic "Amete Mihret" calendar
			{ "Millimoles", All },
			{ "Milliohms", All },
			{ "Minification", All },
			{ "Mmw", All },
			{ "Mncs", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Mobike", All }, // acronym
			{ "Monoline", All & ~ApplePlatform.TVOS },
			{ "Morpher", All },
			{ "Mpe", All }, // acronym
			{ "Mps", All },
			{ "Msaa", All }, // multisample anti-aliasing
			{ "Msb", All }, // Most Significant Bit
			{ "Msi", All },
			{ "Mtc", All }, // acronym
			{ "Mtgp", All },
			{ "Mtl", All },
			{ "Mtu", All }, // acronym
			{ "Muid", All & ~ApplePlatform.TVOS },
			{ "Mul", All },
			{ "Mult", All },
			{ "Multiary", All },
			{ "Multipath", All },
			{ "Multipeer", All },
			{ "Multiscript", All },
			{ "Multiselect", All & ~ApplePlatform.MacOSX },
			{ "Multivariant", All },
			{ "Multiview", All },
			{ "Muxed", All },
			{ "Nacs", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Nai", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Nal", All }, // Network Abstraction Layer (video coding)
			{ "Nanaco", All & ~ApplePlatform.TVOS },
			{ "Nand", All },
			{ "Nanograms", All },
			{ "Nanowatts", All },
			{ "Napas", All & ~ApplePlatform.TVOS }, // Vietnamese payment network
			{ "Ncdhw", All },
			{ "Nchw", All },
			{ "nd", All },
			{ "Ndef", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Ndhwc", All },
			{ "Nesterov", All },
			{ "Nestrov", All },
			{ "Nfc", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Nfnt", All },
			{ "Nhwc", All },
			{ "Nntps", All },
			{ "Nonenumerated", ApplePlatform.MacOSX },
			{ "Noninteractive", All & ~ApplePlatform.TVOS },
			{ "Noop", All },
			{ "Nop", ApplePlatform.MacOSX },
			{ "Nsa", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Nsevent", ApplePlatform.MacOSX },
			{ "Nsl", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst }, // InternetLocationNslNeighborhoodIcon
			{ "Ntlm", All },
			{ "Ntsc", All },
			{ "Nyquist", All & ~ApplePlatform.MacOSX },
			{ "Oaep", All }, // Optimal Asymmetric Encryption Padding
			{ "Objectness", All },
			{ "Ocr", All },
			{ "Ocsp", All }, // Online Certificate Status Protocol
			{ "Octree", All },
			{ "Odia", All },
			{ "Ohwi", All },
			{ "Oid", All },
			{ "Oidhw", All },
			{ "Oihw", All },
			{ "Onnx", All },
			{ "Ootf", All }, // Opto-Optical Transfer Function (HDR)
			{ "Oper", All & ~ApplePlatform.MacOSX },
			{ "Organisation", All }, // kCGImagePropertyIPTCExtRegistryOrganisationID in Xcode9.3-b1
			{ "Orth", All },
			{ "Osa", All }, // Open Scripting Architecture
			{ "Otsu", All }, // threshold for image binarization
			{ "ove", All },
			{ "Overline", All & ~ApplePlatform.TVOS },
			{ "Paeth", All }, // PNG filter
			{ "Palettize", All },
			{ "Parms", All },
			{ "Pausable", All },
			{ "Pbm", ApplePlatform.MacOSX },
			{ "Pci", All & ~ApplePlatform.MacOSX },
			{ "Pcl", All },
			{ "Pcm", All },
			{ "Pde", ApplePlatform.MacOSX },
			{ "Pdu", All },
			{ "Peap", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Pebibits", All },
			{ "Pebibytes", All },
			{ "Perlin", All },
			{ "Persistable", All },
			{ "Petabits", All },
			{ "Pfs", All }, // acronym
			{ "Philox", All },
			{ "Phong", All }, // Phong shading/reflection model
			{ "Photoplethysmogram", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Phq", All & ~ApplePlatform.TVOS },
			{ "Phy", ApplePlatform.MacOSX },
			{ "Picometers", All },
			{ "Pickleball", All & ~ApplePlatform.TVOS },
			{ "Picowatts", All },
			{ "Pkcs", All },
			{ "Placemark", All },
			{ "Playout", All },
			{ "Plessey", All }, // MSI/Plessey barcode symbology
			{ "Pnc", All }, // MIDI
			{ "Pnorm", All },
			{ "Polyline", All },
			{ "Polylines", All },
			{ "Popularimeter", All },
			{ "Postback", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Ppd", ApplePlatform.MacOSX }, // PostScript Printer Description
			{ "Ppk", All },
			{ "Preauthentication", ApplePlatform.MacOSX },
			{ "Preds", All },
			{ "Prefilter", All },
			{ "Prereleased", All },
			{ "Prerolls", All },
			{ "Preseti", All },
			{ "Prev", All }, // previous abbreviation
			{ "Previewable", ApplePlatform.MacOSX },
			{ "Prf", All & ~ApplePlatform.TVOS },
			{ "Psec", All },
			{ "Psk", All },
			{ "Pskc", All & ~ApplePlatform.TVOS },
			{ "Psm", All }, // Protocol/Service Multiplexer
			{ "Privs", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst }, // privileges abbreviation
			{ "Pss", All }, // Probabilistic Signature Scheme (RSA-PSS)
			{ "Ptp", ApplePlatform.MacOSX },
			{ "Ptss", All & ~ApplePlatform.TVOS }, // Presentation Timestamps (plural)
			{ "Pvr", All },
			{ "Pvrtc", All }, // MTLBlitOption - PowerVR Texture Compression
			{ "Qos", All },
			{ "Quadding", All },
			{ "Quaterniond", All },
			{ "Quic", All },
			{ "Qura", All },
			{ "Qwac", All },
			{ "Raycast", ApplePlatform.iOS },
			{ "Raycasts", ApplePlatform.iOS },
			{ "Reacquirer", All },
			{ "Reassociation", ApplePlatform.MacOSX },
			{ "Reauthentication", ApplePlatform.MacOSX },
			{ "Reinvitation", All },
			{ "Reinvite", All },
			{ "Rel", All },
			{ "Relocalization", ApplePlatform.iOS },
			{ "Relu", All }, // Rectified Linear Unit (ML)
			{ "Replayable", All },
			{ "Reprojection", All },
			{ "Rfc", All }, // Request for Comments
			{ "Rgb", All },
			{ "Rgba", All },
			{ "Rgbaf", All },
			{ "Rgbah", All },
			{ "Rgbx", All },
			{ "Rggb", All }, // acronym for Red, Green, Green, Blue
			{ "Rint", All },
			{ "Rle", All },
			{ "Rms", All }, // root mean square
			{ "Rnn", All },
			{ "Roi", All },
			{ "Romm", All }, // acronym: Reference Output Medium Metric
			{ "Rpa", All },
			{ "Rpn", All }, // acronym
			{ "Rsa", All }, // Rivest, Shamir and Adleman
			{ "Rsapss", All },
			{ "Rsqrt", All }, // reciprocal square root
			{ "Rssi", All },
			{ "Rtl", All },
			{ "Rtp", All & ~ApplePlatform.MacOSX },
			{ "Rtsp", All },
			{ "Saml", All & ~ApplePlatform.MacCatalyst }, // acronym
			{ "Sbr", All }, // Spectral Band Replication (AAC)
			{ "Scc", All },
			{ "Scn", All },
			{ "Sdh", ApplePlatform.TVOS },
			{ "Sdk", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Sdnn", All & ~ApplePlatform.TVOS },
			{ "Sdof", ApplePlatform.MacOSX },
			{ "Sdr", All },
			{ "Sdtv", ApplePlatform.TVOS }, // acronym: Standard Definition Tele Vision
			{ "Securit", ApplePlatform.iOS },
			{ "Seekable", All },
			{ "Sel", All & ~ApplePlatform.MacOSX },
			{ "Selu", All }, // Scaled Exponential Linear unit (ML)
			{ "Semitransient", ApplePlatform.MacOSX },
			{ "Sensel", All },
			{ "Sha", All }, // Secure Hash Algorithm
			{ "Shadable", All },
			{ "Siemen", All & ~ApplePlatform.TVOS },
			{ "Signbit", All },
			{ "Sint", All }, // as in "Signed Integer"
			{ "Sixtyfour", ApplePlatform.MacOSX },
			{ "Slerp", All },
			{ "Slomo", All },
			{ "Smpte", All },
			{ "Snapshotter", All },
			{ "Snn", All },
			{ "Snorm", All },
			{ "Sobel", All },
			{ "Softmax", All }, // get_SoftmaxNormalization
			{ "Sopen", ApplePlatform.MacOSX },
			{ "Spacei", All },
			{ "Spl", All },
			{ "Sqrt", All },
			{ "Srgb", All },
			{ "Ssid", All },
			{ "Ssids", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Ssml", All },
			{ "Sso", ApplePlatform.MacOSX },
			{ "Ssr", All }, // Scalable Sample Rate (AAC)
			{ "st", All },
			{ "Sta", ApplePlatform.MacOSX },
			{ "Strided", All },
			{ "Subband", All & ~ApplePlatform.TVOS },
			{ "Subbeat", All },
			{ "Subcaption", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Subcardioid", All & ~ApplePlatform.MacOSX },
			{ "Subentities", All },
			{ "Subfilter", All & ~ApplePlatform.TVOS },
			{ "Subfilters", All & ~ApplePlatform.TVOS },
			{ "Subheadline", All },
			{ "Sublocality", All },
			{ "Sublocation", All },
			{ "Submesh", All },
			{ "Submeshes", All },
			{ "Subpixel", All },
			{ "Subresources", All },
			{ "Subsec", All },
			{ "Suica", All & ~ApplePlatform.TVOS }, // Japanese contactless smart card type
			{ "Superentity", All },
			{ "Supertype", All },
			{ "Supertypes", All },
			{ "Svfg", All },
			{ "Svg", All }, // Scalable Vector Graphics
			{ "Svgf", All },
			{ "Swolf", All & ~ApplePlatform.TVOS },
			{ "Symbologies", All }, // plural of symbology (barcode)
			{ "Synchronizable", All },
			{ "Sysex", All },
			{ "Tbgr", All },
			{ "Tdoa", ApplePlatform.iOS },
			{ "Tebibits", All },
			{ "Tensorflow", All },
			{ "Tessellator", All },
			{ "Texcoord", All },
			{ "Texel", All },
			{ "Tga", All },
			{ "th", All },
			{ "Threadgroup", All },
			{ "Threadgroups", All },
			{ "Thumbnailing", All & ~ApplePlatform.TVOS },
			{ "Thumbstick", All },
			{ "Thumbsticks", ApplePlatform.iOS },
			{ "Timecodes", All & ~ApplePlatform.TVOS },
			{ "Timelapse", All },
			{ "Timelapses", All },
			{ "Tls", All },
			{ "Tlv", All },
			{ "Tmoney", All & ~ApplePlatform.TVOS },
			{ "Toc", All },
			{ "Toci", All },
			{ "Tonemap", All },
			{ "Touchpads", All },
			{ "Transceive", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Trc", All },
			{ "Tri", All },
			{ "Ttls", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Tweening", All },
			{ "Twentyfour", ApplePlatform.MacOSX },
			{ "Twips", ApplePlatform.MacOSX },
			{ "tx", All },
			{ "ty", All },
			{ "Udi", All & ~ApplePlatform.TVOS },
			{ "Udp", All },
			{ "Uid", All & ~ApplePlatform.TVOS },
			{ "Unconfigured", All & ~ApplePlatform.MacOSX },
			{ "Undecodable", All },
			{ "Underrun", All },
			{ "Unemphasized", ApplePlatform.MacOSX },
			{ "Unentitled", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Unfetched", All },
			{ "Unfocus", All },
			{ "Unioning", All },
			{ "Unmap", All },
			{ "Unmatch", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Unorm", All },
			{ "Unpair", ApplePlatform.MacOSX },
			{ "Unpremultiplied", All },
			{ "Unpremultiplying", All },
			{ "Unprepare", All },
			{ "Unproject", All },
			{ "Unpublish", All },
			{ "Unsend", All & ~ApplePlatform.TVOS },
			{ "Unsolo", All },
			{ "Unsynced", ApplePlatform.MacOSX | ApplePlatform.iOS },
			{ "Untrash", ApplePlatform.iOS },
			{ "Upce", All },
			{ "Upi", ApplePlatform.iOS },
			{ "Uri", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Usac", All }, // Unified Speech and Audio Coding
			{ "Usd", All }, // Universal Scene Description
			{ "Usdz", All }, // USD zip
			{ "Usec", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Ussd", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Uterance", All },
			{ "Utf", All },
			{ "Uti", All & ~ApplePlatform.TVOS },
			{ "Varispeed", All },
			{ "Vbr", All },
			{ "Vbv", All },
			{ "Vergence", All },
			{ "Vnode", All },
			{ "Voip", ApplePlatform.MacCatalyst },
			{ "Voronoi", All },
			{ "Vpn", All },
			{ "Vtt", All },
			{ "Waon", All & ~ApplePlatform.TVOS },
			{ "Warichu", All },
			{ "Warpable", All },
			{ "Wcdma", All },
			{ "Wep", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Wlan", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Wpa", All & ~ApplePlatform.TVOS },
			{ "Writeability", All },
			{ "Xattr", ApplePlatform.MacOSX },
			{ "Xattrs", ApplePlatform.MacOSX },
			{ "Xbgr", All },
			{ "Xmp", All },
			{ "Xnor", All },
			{ "Xrgb", All },
			{ "xy", All },
			{ "Xyz", All },
			{ "Xzy", All },
			{ "Yobibits", All },
			{ "Yobibytes", All },
			{ "Yottabits", All },
			{ "Yuv", ApplePlatform.MacOSX },
			{ "Yuvk", ApplePlatform.MacOSX },
			{ "yuvs", All },
			{ "yx", All },
			{ "Yxz", All },
			{ "yy", All },
			{ "Yyy", All },
			{ "Yzx", All },
			{ "Zebibits", All },
			{ "Zebibytes", All },
			{ "Zenkaku", All & ~ApplePlatform.MacOSX },
			{ "Zettabits", All },
			{ "Zlib", All },
			{ "Zxy", All },
			{ "Zyx", All },
		};

		// Check if any API name in the assembly contains the given word.
		// This is used to avoid false "unnecessary allowed typo" reports caused
		// by the spell checker not flagging the word on some machines (the spell
		// checker is non-deterministic across machines/OS versions/locales).
		bool IsWordInAnyApiName (Type [] types, string word)
		{
			foreach (var t in types) {
				if (!t.IsPublic || IsObsolete (t))
					continue;
				if (t.Name.Contains (word, StringComparison.OrdinalIgnoreCase))
					return true;
				foreach (var f in t.GetFields ()) {
					if ((!f.IsPublic && !f.IsFamily) || IsObsolete (f))
						continue;
					if (f.Name.Contains (word, StringComparison.OrdinalIgnoreCase))
						return true;
				}
				foreach (var m in t.GetMethods ()) {
					if ((!m.IsPublic && !m.IsFamily) || IsObsolete (m))
						continue;
					if (m.Name.Contains (word, StringComparison.OrdinalIgnoreCase))
						return true;
				}
			}
			return false;
		}

		bool IsObsolete (MemberInfo? mi)
		{
			if (mi is null)
				return false;
			if (mi.GetCustomAttributes<ObsoleteAttribute> (true).Any ())
				return true;
			if (MemberHasObsolete (mi))
				return true;
			if (MemberHasEditorBrowsableNever (mi))
				return true;
			// Property accessors may not have [Obsolete] even if the property does
			if (mi is MethodInfo method && method.IsSpecialName && mi.DeclaringType is not null) {
				var name = mi.Name;
				if (name.StartsWith ("get_", StringComparison.Ordinal) || name.StartsWith ("set_", StringComparison.Ordinal)) {
					var propName = name.Substring (4);
					foreach (var prop in mi.DeclaringType.GetProperties (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)) {
						if (prop.Name != propName)
							continue;
						if (prop.GetCustomAttributes<ObsoleteAttribute> (true).Any () || MemberHasObsolete (prop))
							return true;
					}
				}
			}
			return IsObsolete (mi.DeclaringType);
		}

		[Test]
		public virtual void AttributeTypoTest ()
		{
			var types = Assembly.GetTypes ();
			int totalErrors = 0;
			foreach (Type t in types)
				AttributeTypo (t, ref totalErrors);

			Assert.That (totalErrors, Is.EqualTo (0), "Attributes have typos!");
		}

		void AttributeTypo (Type t, ref int totalErrors)
		{
			AttributesMessageTypoRules (t, t.Name, ref totalErrors);

			var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
			foreach (var f in t.GetFields (flags))
				AttributesMessageTypoRules (f, t.Name, ref totalErrors);

			foreach (var p in t.GetProperties (flags))
				AttributesMessageTypoRules (p, t.Name, ref totalErrors);

			foreach (var m in t.GetMethods (flags))
				AttributesMessageTypoRules (m, t.Name, ref totalErrors);

			foreach (var e in t.GetEvents (flags))
				AttributesMessageTypoRules (e, t.Name, ref totalErrors);

			foreach (var nt in t.GetNestedTypes ())
				AttributeTypo (nt, ref totalErrors);
		}

		[Test]
		public virtual void TypoTest ()
		{
			AssertMatchingOSVersionAndSdkVersion ();

			TestRuntime.AssertSimulatorOrDesktop ("Typos only detected on simulator/desktop");

			var sw = Stopwatch.StartNew ();

			using var checker = new SpellChecker ();

			// Collect all unique words from public API names (split on uppercase boundaries)
			var words = new HashSet<string> (StringComparer.Ordinal);
			var types = Assembly.GetTypes ();
			foreach (Type t in types) {
				if (!t.IsPublic || IsObsolete (t))
					continue;

				SplitIntoWords (words, t.Name);

				foreach (FieldInfo f in t.GetFields ()) {
					if ((!f.IsPublic && !f.IsFamily) || IsObsolete (f))
						continue;
					SplitIntoWords (words, f.Name);
				}

				foreach (MethodInfo m in t.GetMethods ()) {
					if ((!m.IsPublic && !m.IsFamily) || IsObsolete (m))
						continue;
					SplitIntoWords (words, m.Name);
				}
			}

			// Check each unique word individually with the spell checker
			var typos = new HashSet<string> (StringComparer.Ordinal);
			foreach (var word in words) {
				var checkRange = new NSRange (0, word.Length);
#if MONOMAC
				var typoRange = checker.CheckSpelling (word, 0, "en_US", false, 0, out var _);
#else
				var typoRange = checker.RangeOfMisspelledWordInString (word, checkRange, checkRange.Location, false, "en_US");
#endif
				if (typoRange.Length > 0)
					typos.Add (word.Substring ((int) typoRange.Location, (int) typoRange.Length));
			}

			// Check each typo against allowed list
			int totalErrors = 0;
			var currentPlatform = TestRuntime.CurrentPlatform;
			var usedAllowed = new HashSet<string> ();
			foreach (var typo in typos) {
				if (allowed.TryGetValue (typo, out var platforms) && platforms.HasFlag (currentPlatform)) {
					usedAllowed.Add (typo);
					continue;
				}
				ReportError ("Typo: {0}", typo);
				totalErrors++;
			}

			// Verify that all allowed words for the current platform are still needed
			var unusedAllowed = allowed.Keys
				.Where (w => allowed [w].HasFlag (currentPlatform))
				.Except (usedAllowed);
			foreach (var typo in unusedAllowed) {
				if (IsWordInAnyApiName (types, typo))
					continue;
				ReportError ($"Unnecessary allowed typo \"{typo}\" is not present in any API name");
				totalErrors++;
			}
			Console.WriteLine ($"TypoTest completed in {sw.Elapsed.TotalMilliseconds:F0}ms (unique words: {words.Count}, typos found: {typos.Count})");
			Assert.That (totalErrors, Is.EqualTo (0), "Typos!");
		}

		// Split an API name into words on uppercase/digit/symbol boundaries and add to the set
		static void SplitIntoWords (HashSet<string> words, string name)
		{
			int start = -1;
			for (int i = 0; i < name.Length; i++) {
				char c = name [i];
				if (Char.IsUpper (c)) {
					if (start >= 0 && i > start)
						words.Add (name.Substring (start, i - start));
					start = i;
				} else if (Char.IsDigit (c) || c == '<' || c == '>' || c == '_') {
					if (start >= 0 && i > start)
						words.Add (name.Substring (start, i - start));
					start = -1;
				} else if (start < 0) {
					// lowercase char with no word start — skip
				}
			}
			if (start >= 0 && name.Length > start)
				words.Add (name.Substring (start));
		}

		string? GetMessage (object attribute)
		{
			string? message = null;
			if (attribute is AdviceAttribute)
				message = ((AdviceAttribute) attribute).Message;
			if (attribute is ObsoleteAttribute)
				message = ((ObsoleteAttribute) attribute).Message;

			return message;
		}

		void AttributesMessageTypoRules (MemberInfo mi, string typeName, ref int totalErrors)
		{
			if (mi is null)
				return;

			foreach (object ca in mi.GetCustomAttributes ()) {
				string? message = GetMessage (ca);
				if (message is not null) {
					var memberAndTypeFormat = mi.Name == typeName ? "Type: {0}" : "Member name: {1}, Type: {0}";
					var memberAndType = string.Format (memberAndTypeFormat, typeName, mi.Name);

					// Rule 1: https://github.com/dotnet/macios/wiki/BINDINGS#rule-1
					// Note: we don't enforce that rule for the Obsolete (not Obsoleted) attribute since the attribute itself doesn't support versions.
					if (!(ca is ObsoleteAttribute)) {
						var forbiddenOSNames = new [] { "iOS", "watchOS", "tvOS", "macOS" };
						if (forbiddenOSNames.Any (s => Regex.IsMatch (message, $"({s} ?)[0-9]+"))) {
							ReportError ("[Rule 1] Don't put OS information in attribute's message: \"{0}\" - {1}", message, memberAndType);
							totalErrors++;
						}
					}

					// Rule 2: https://github.com/dotnet/macios/wiki/BINDINGS#rule-2
					if (message.Contains ('`')) {
						ReportError ("[Rule 2] Replace grave accent (`) by apostrophe (') in attribute's message: \"{0}\" - {1}", message, memberAndType);
						totalErrors++;
					}

					// Rule 3: https://github.com/dotnet/macios/wiki/BINDINGS#rule-3
					if (!message.EndsWith (".", StringComparison.Ordinal)) {
						if (!allowedRule3.Contains (typeName)) {
							ReportError ("[Rule 3] Missing '.' in attribute's message: \"{0}\" - {1}", message, memberAndType);
							totalErrors++;
						}
					}

					// Rule 4: https://github.com/dotnet/macios/wiki/BINDINGS#rule-4
					if (!allowedMemberRule4.Contains (mi.Name)) {
						var forbiddenAvailabilityKeywords = new [] { "introduced", "deprecated", "obsolete", "obsoleted" };
						if (forbiddenAvailabilityKeywords.Any (s => Regex.IsMatch (message, $"({s})", RegexOptions.IgnoreCase))) {
							ReportError ("[Rule 4] Don't use availability keywords in attribute's message: \"{0}\" - {1}", message, memberAndType);
							totalErrors++;
						}
					}

					var forbiddensWords = new [] { "OSX", "OS X" };
					for (int i = 0; i < forbiddensWords.Length; i++) {
						var word = forbiddensWords [i];
						if (Regex.IsMatch (message, $"({word})", RegexOptions.IgnoreCase)) {
							ReportError ("Don't use {0} in attribute's message: \"{1}\" - {2}", word, message, memberAndType);
							totalErrors++;
						}
					}
				}
			}
		}

		bool CheckLibrary (string? lib)
		{
#if MONOMAC
			// on macOS the file should exist on the specified path
			// for iOS the simulator paths do not match the strings
			switch (lib) {
			// location changed in 10.8 but it loads fine (and fixing it breaks on earlier macOS)
			case Constants.CFNetworkLibrary:
			// location changed in 10.10 but it loads fine (and fixing it breaks on earlier macOS)
			case Constants.CoreBluetoothLibrary:
			// location changed in 10.11 but it loads fine (and fixing it breaks on earlier macOS)
			case Constants.CoreImageLibrary:
				break;
			default:
				if (TestRuntime.CheckSystemVersion (ApplePlatform.MacOSX, 11, 0)) {
					// on macOS 11.0 the frameworks binary files are not present (cache) but can be loaded
					if (!Directory.Exists (Path.GetDirectoryName (lib)))
						return false;
				} else if (!File.Exists (lib))
					return false;
				break;
			}
#endif
			var h = IntPtr.Zero;
			try {
				h = Dlfcn.dlopen (lib, 0);
				if (h != IntPtr.Zero)
					return true;
#if MONOMAC
				// on macOS it might be wrong architecture
				// i.e. 64 bits only (thin) libraries running on 32 bits process
				if (IntPtr.Size == 4)
					return true;
#endif
			} finally {
				Dlfcn.dlclose (h);
			}
			return false;
		}

		protected void AssertMatchingOSVersionAndSdkVersion ()
		{
			var sdk = new Version (Constants.SdkVersion);
#if MONOMAC
			if (!NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion (new NSOperatingSystemVersion (sdk.Major, sdk.Minor, sdk.Build == -1 ? 0 : sdk.Build)))
#else
			if (!UIDevice.CurrentDevice.CheckSystemVersion (sdk.Major, sdk.Minor))
#endif
				Assert.Ignore ($"This test only executes using the latest OS version ({sdk.Major}.{sdk.Minor})");
		}

		[Test]
		public void ConstantsCheck ()
		{
			// The constants are file paths for frameworks / dylibs
			// unless the latest OS is used there's likely to be missing ones
			// so we run this test only on the latest supported (matching SDK) OS
			AssertMatchingOSVersionAndSdkVersion ();

			var c = typeof (Constants);
			foreach (var fi in c.GetFields ()) {
				if (!fi.IsPublic)
					continue;
				var s = fi.GetValue (null) as string;
				switch (fi.Name) {
				case "Version":
				case "SdkVersion":
					Assert.That (Version.TryParse (s, out _), Is.True, fi.Name);
					break;
#if !XAMCORE_5_0
				case "AssetsLibraryLibrary":
				case "NewsstandKitLibrary": // Removed from iOS, but we have to keep the constant around for binary compatibility.
					break;
#endif
#if !__MACOS__
				case "CinematicLibrary":
				case "ThreadNetworkLibrary":
				case "MediaSetupLibrary":
				case "MLComputeLibrary":
					// Xcode 12 beta 2 does not ship these framework/headers for the simulators
					if (TestRuntime.IsDevice)
						Assert.That (CheckLibrary (s), Is.True, fi.Name);
					break;
#endif
#if __TVOS__
				case "MetalPerformanceShadersGraphLibrary":
					// not supported in tvOS (12.1) simulator so load fails
					if (TestRuntime.IsSimulatorOrDesktop)
						break;
					goto default;
#endif
				case "MetalFXLibrary":
					if (TestRuntime.IsSimulatorOrDesktop)
						break;
					goto default;
				case "SensorKitLibrary": // SensorKit doesn't exist on iPads
					if (TestRuntime.IsDevice && TestRuntime.IsiPad)
						break;
					goto default;
#if __TVOS__
				// This framework is only available on device
				case "BrowserEngineKitLibrary":
					if (TestRuntime.CheckXcodeVersion (16, 2) && TestRuntime.IsSimulator)
						continue;
					goto default;
#endif // __TVOS__
				default:
					if (fi.Name.EndsWith ("Library", StringComparison.Ordinal)) {
#if __IOS__
						if (fi.Name == "CoreNFCLibrary") {
							// NFC is currently not available on iPad
							if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
								continue;
						}
#endif
#if __MACOS__
						// Only available in macOS 10.15.4+
						if (fi.Name == "AutomaticAssessmentConfigurationLibrary" && !TestRuntime.CheckXcodeVersion (11, 4))
							continue;
#endif
						Assert.That (CheckLibrary (s), Is.True, fi.Name);
					} else {
						Assert.Fail ($"Unknown '{fi.Name}' field cannot be verified - please fix me!");
					}
					break;
				}
			}
		}
	}
}
