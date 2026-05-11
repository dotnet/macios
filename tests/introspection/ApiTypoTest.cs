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

using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
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

		public virtual bool Skip (Type baseType, string typo)
		{
			return SkipAllowed (baseType.Name, null, typo);
		}

		public virtual bool Skip (MemberInfo methodName, string typo)
		{
			return SkipAllowed (methodName.DeclaringType?.Name, methodName.Name, typo);
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
			{ "Accurracy", All },
			{ "Achivements", All },
			{ "Acos", All },
			{ "Acosh", All },
			{ "Activatable", All },
			{ "Addin", All },
			{ "Addl", All },
			{ "Addons", ApplePlatform.MacOSX },
			{ "Addr", All },
			{ "Adessive", All },
			{ "Adjustmentfor", All & ~ApplePlatform.MacOSX },
			{ "Afi", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Agc", All },
			{ "Ahap", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Aifc", All },
			{ "Aiff", All },
			{ "Aime", ApplePlatform.MacOSX },
			{ "Aio", ApplePlatform.MacOSX },
			{ "Alg", All }, // short for Algorithm
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
			{ "Appactive", ApplePlatform.MacOSX },
			{ "Applei", All },
			{ "Aps", ApplePlatform.MacOSX },
			{ "Apv", ApplePlatform.MacOSX },
			{ "Arae", ApplePlatform.MacOSX },
			{ "Arcball", All },
			{ "Argb", All },
			{ "Arraycollation", All & ~ApplePlatform.MacOSX },
			{ "Asin", All },
			{ "Asinh", All },
			{ "Astc", All },
			{ "Aswas", ApplePlatform.MacOSX },
			{ "Atan", All },
			{ "Atanh", All },
			{ "Atm", All },
			{ "Atmos", All }, // Dolby Atmos
			{ "Atr", All },
			{ "Ats", All }, // App Transport Security
			{ "Atsc", All },
			{ "Attr", ApplePlatform.MacOSX },
			{ "Attrib", All },
			{ "Attributesfor", ApplePlatform.MacOSX },
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
			{ "Callpout", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Cartes", All & ~ApplePlatform.TVOS }, // french
			{ "Catmull", All },
			{ "Cavlc", All },
			{ "Ccitt", ApplePlatform.MacOSX },
			{ "Cct", All },
			{ "Ccw", All },
			{ "Cda", All & ~ApplePlatform.TVOS }, // acronym: Clinical Document Architecture
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
			{ "Ciff", All },
			{ "Cinemagraph", ApplePlatform.TVOS },
			{ "Cinepak", All },
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
			{ "Commited", ApplePlatform.MacOSX },
			{ "Conecs", All & ~ApplePlatform.TVOS },
			{ "Constrainted", ApplePlatform.MacOSX },
			{ "Conv", All },
			{ "Copyback", All },
			{ "Cose", All & ~ApplePlatform.TVOS },
			{ "Crosstraining", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Csr", All },
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
			{ "Denimonator", All },
			{ "Denoise", All },
			{ "Denoised", All },
			{ "Depthwise", All },
			{ "Dequantize", All },
			{ "Descendents", All },
			{ "Descriptorat", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Descriptorfor", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Dfsi", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Dhe", All }, // Diffie–Hellman key exchange
			{ "Dhs", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Dhwio", All },
			{ "Dicom", All },
			{ "Diconnection", All },
			{ "Diffable", All }, // that you can diff it.. made up word from apple
			{ "Differental", All },
			{ "Diffie", All },
			{ "Dirbursement", All & ~ApplePlatform.TVOS },
			{ "Directionfor", All & ~ApplePlatform.MacOSX },
			{ "Dirs", ApplePlatform.MacOSX },
			{ "Dismissable", ApplePlatform.MacOSX },
			{ "Dissapearing", ApplePlatform.MacOSX },
			{ "Dist", All },
			{ "Distinguised", ApplePlatform.MacOSX }, // ITLibPlaylistPropertyDistinguisedKind
			{ "dlclose", All },
			{ "dlerror", All },
			{ "Dlfcn", All },
			{ "Dls", ApplePlatform.MacOSX },
			{ "Dng", All },
			{ "Dnssec", All },
			{ "Dont", All },
			{ "Dop", ApplePlatform.iOS },
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
			{ "Ebu", All },
			{ "Ecc", All }, // Elliptic Curve Cryptography
			{ "Ecdh", All }, // Elliptic Curve Diffie–Hellman
			{ "Ecdhe", All }, // Elliptic Curve Diffie-Hellman Ephemeral
			{ "Ecdsa", All }, // Elliptic Curve Digital Signature Algorithm
			{ "Ecg", All & ~ApplePlatform.TVOS },
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
			{ "Emaili", All & ~ApplePlatform.TVOS },
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
			{ "Exhange", All },
			{ "Expr", All },
			{ "Exr", All },
			{ "Extrinsics", All },
			{ "Feli", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Felica", All & ~ApplePlatform.TVOS }, // Japanese contactless RFID smart card system
			{ "Femtowatts", All },
			{ "Fft", All },
			{ "Fhir", All & ~ApplePlatform.TVOS },
			{ "Fieldset", All & ~ApplePlatform.MacCatalyst },
			{ "Flipside", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Formati", All },
			{ "Fourty", ApplePlatform.MacOSX },
			{ "Fov", All },
			{ "Fqdns", All },
			{ "Framebuffer", All },
			{ "Framesetter", All },
			{ "Freq", All },
			{ "Froms", ApplePlatform.MacOSX }, // NSMetadataItemWhereFromsKey
			{ "Ftps", All },
			{ "Gadu", All & ~ApplePlatform.TVOS },
			{ "Gainmap", All },
			{ "Gbrg", All }, // acronym for Green-Blue-Reg-Green
			{ "Gbtac", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Gbtdc", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Gcm", All },
			{ "Gelu", All }, // Gaussian Error Linear Unit (ML)
			{ "Gibibits", All },
			{ "Gid", ApplePlatform.MacOSX },
			{ "Gigapascals", All },
			{ "Girocard", All & ~ApplePlatform.TVOS },
			{ "Gles", ApplePlatform.iOS | ApplePlatform.TVOS },
			{ "Glorot", All }, // NN
			{ "Gop", All }, // acronym for Group Of Pictures
			{ "Gpp", All },
			{ "Gps", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Grammarl", ApplePlatform.MacOSX },
			{ "Grbg", All }, // acronym for Green-Red-Blue-Green
			{ "Greeking", ApplePlatform.MacOSX },
			{ "Groupless", All & ~ApplePlatform.TVOS },
			{ "Gru", All },
			{ "Gtin", All },
			{ "Gui", All },
			{ "Hardlink", ApplePlatform.MacOSX },
			{ "Hdmi", All & ~ApplePlatform.MacOSX },
			{ "Hdr", All },
			{ "Heic", All }, // file type
			{ "Heics", All }, // High Efficiency Image File Format (Sequence)
			{ "Heif", All }, // High Efficiency Image File Format
			{ "Hermitean", All },
			{ "Hevc", All }, // CMVideoCodecType / High Efficiency Video Coding
			{ "Hfp", All & ~ApplePlatform.MacOSX },
			{ "Hhr", All },
			{ "Himyan", All & ~ApplePlatform.TVOS },
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
			{ "Iconfor", ApplePlatform.MacOSX },
			{ "Icq", All & ~ApplePlatform.TVOS },
			{ "Identd", All },
			{ "Iec", All },
			{ "Ies", All },
			{ "Imageblock", All },
			{ "Imagefor", All & ~ApplePlatform.MacOSX },
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
			{ "Inklist", All },
			{ "Inode", ApplePlatform.MacOSX },
			{ "Inot", All },
			{ "Inser", All },
			{ "Instamatic", ApplePlatform.MacOSX },
			{ "Interac", All & ~ApplePlatform.TVOS },
			{ "Interactable", ApplePlatform.MacOSX },
			{ "Interframe", All },
			{ "Interitem", All },
			{ "Intermenstrual", All & ~ApplePlatform.TVOS },
			{ "Intoi", All & ~ApplePlatform.MacOSX },
			{ "Intravaginal", All & ~ApplePlatform.TVOS },
			{ "Inv", All },
			{ "Invitable", All },
			{ "Iou", All },
			{ "Ipa", All },
			{ "Ipp", All },
			{ "Iptc", All },
			{ "Ircs", All },
			{ "Isrc", All },
			{ "Itemto", ApplePlatform.MacOSX },
			{ "Itf", All },
			{ "Itt", All & ~ApplePlatform.TVOS },
			{ "Itu", All },
			{ "Itur", All }, // Itur_2020_Hlg
			{ "Jaywan", All & ~ApplePlatform.TVOS },
			{ "Jcb", All & ~ApplePlatform.TVOS }, // Japanese credit card company
			{ "Jfif", All },
			{ "Jis", ApplePlatform.MacOSX },
			{ "Jrts", All & ~ApplePlatform.TVOS },
			{ "Jwks", ApplePlatform.MacOSX },
			{ "Jws", All & ~ApplePlatform.TVOS },
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
			{ "Langauges", All & ~ApplePlatform.MacOSX },
			{ "Latm", All }, //  Low Overhead Audio Transport Multiplex
			{ "Lbc", All },
			{ "Ldaps", All },
			{ "Lerp", All },
			{ "libcompression", All },
			{ "libdispatch", All },
			{ "Lingustic", All },
			{ "Lod", All },
			{ "Lopass", All },
			{ "Lowlevel", All },
			{ "Lpcm", All },
			{ "Lstm", All },
			{ "Lte", All },
			{ "Ltr", All },
			{ "Lun", All },
			{ "Lut", All },
			{ "Lzfse", All }, // acronym
			{ "Lzma", All }, // acronym
			{ "Lzw", ApplePlatform.MacOSX },
			{ "Mada", All & ~ApplePlatform.TVOS }, // payment system
			{ "Matchingcoalesce", All },
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
			{ "Nanaco", All & ~ApplePlatform.TVOS },
			{ "Nand", All },
			{ "Nanograms", All },
			{ "Nanowatts", All },
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
			{ "Numberof", ApplePlatform.MacOSX },
			{ "Nyquist", All & ~ApplePlatform.MacOSX },
			{ "Objectness", All },
			{ "Occlussion", All },
			{ "Ocr", All },
			{ "Ocsp", All }, // Online Certificate Status Protocol
			{ "Octree", All },
			{ "Ocurrences", All },
			{ "Odia", All },
			{ "Ohwi", All },
			{ "Oid", All },
			{ "Oidhw", All },
			{ "Oihw", All },
			{ "Onnx", All },
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
			{ "Performwith", ApplePlatform.MacOSX },
			{ "Perlin", All },
			{ "Persistable", All },
			{ "Persistance", All },
			{ "Petabits", All },
			{ "Pfs", All }, // acronym
			{ "Philox", All },
			{ "Photoplethysmogram", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Phq", All & ~ApplePlatform.TVOS },
			{ "Phy", ApplePlatform.MacOSX },
			{ "Picometers", All },
			{ "Picowatts", All },
			{ "Pkcs", All },
			{ "Placemark", All },
			{ "Playout", All },
			{ "Pnc", All }, // MIDI
			{ "Pnorm", All },
			{ "Polyline", All },
			{ "Polylines", All },
			{ "Popularimeter", All },
			{ "Postback", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Ppk", All },
			{ "Preauthentication", ApplePlatform.MacOSX },
			{ "Preds", All },
			{ "Prefilter", All },
			{ "Prereleased", All },
			{ "Prerolls", All },
			{ "Preseti", All },
			{ "Previewable", ApplePlatform.MacOSX },
			{ "Prf", All & ~ApplePlatform.TVOS },
			{ "Propogate", All },
			{ "Psec", All },
			{ "Psk", All },
			{ "Pskc", All & ~ApplePlatform.TVOS },
			{ "Psm", All }, // Protocol/Service Multiplexer
			{ "Ptp", ApplePlatform.MacOSX },
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
			{ "Rectfrom", ApplePlatform.MacOSX },
			{ "Registeration", ApplePlatform.MacOSX },
			{ "Reinvitation", All },
			{ "Reinvite", All },
			{ "Rel", All },
			{ "Relocalization", ApplePlatform.iOS },
			{ "Relu", All }, // Rectified Linear Unit (ML)
			{ "Remmote", All },
			{ "Replayable", All },
			{ "Reprojection", All },
			{ "Rgb", All },
			{ "Rgba", All },
			{ "Rgbaf", All },
			{ "Rgbah", All },
			{ "Rgbx", All },
			{ "Rggb", All }, // acronym for Red, Green, Green, Blue
			{ "Rint", All },
			{ "Rle", All },
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
			{ "st", All },
			{ "Sta", ApplePlatform.MacOSX },
			{ "Standarize", All },
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
			{ "Supression", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Svfg", All },
			{ "Svg", All }, // Scalable Vector Graphics
			{ "Svgf", All },
			{ "Swolf", All & ~ApplePlatform.TVOS },
			{ "Sysex", All },
			{ "Targetand", ApplePlatform.MacOSX },
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
			{ "Tls", All },
			{ "Tlv", All },
			{ "Tmoney", All & ~ApplePlatform.TVOS },
			{ "Toc", All },
			{ "Toci", All },
			{ "Tonemap", All },
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
			{ "Unioning", All },
			{ "Unmap", All },
			{ "Unmatch", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Unorm", All },
			{ "Unpremultiplied", All },
			{ "Unpremultiplying", All },
			{ "Unprepare", All },
			{ "Unproject", All },
			{ "Unpublish", All },
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
			{ "Wifes", All & ~ApplePlatform.TVOS },
			{ "Willl", All & ~ApplePlatform.TVOS },
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
			{ "Zenkaku", All & ~ApplePlatform.MacOSX },
			{ "Zettabits", All },
			{ "Zlib", All },
			{ "Zxy", All },
			{ "Zyx", All },
		};

		// tracks which allowed words were actually seen during TypoTest
		HashSet<string> used = new HashSet<string> ();

		bool SkipAllowed (string? typeName, string? methodName, string typo)
		{
			if (allowed.TryGetValue (typo, out var platforms) && platforms.HasFlag (TestRuntime.CurrentPlatform)) {
				used.Add (typo);
				return true;
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
			return IsObsolete (mi.DeclaringType);
		}

		[Test]
		public virtual void AttributeTypoTest ()
		{
			var types = Assembly.GetTypes ();
			int totalErrors = 0;
			foreach (Type t in types)
				AttributeTypo (t, ref totalErrors);

			Assert.AreEqual (0, totalErrors, "Attributes have typos!");
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

			// that's slow and there's no value to run it on devices as the API names
			// being verified won't change from the simulator
			TestRuntime.AssertSimulatorOrDesktop ("Typos only detected on simulator/desktop");

			using var checker = new SpellChecker ();

			var types = Assembly.GetTypes ();
			int totalErrors = 0;
			foreach (Type t in types) {
				if (t.IsPublic) {
					if (IsObsolete (t))
						continue;

					string txt = NameCleaner (t.Name);
					var typo = GetCachedTypo (checker, txt);
					if (typo.Length > 0) {
						if (!Skip (t, typo)) {
							ReportError ("Typo in TYPE: {0} - {1} ", t.Name, typo);
							totalErrors++;
						}
					}

					var fields = t.GetFields ();
					foreach (FieldInfo f in fields) {
						if (!f.IsPublic && !f.IsFamily)
							continue;

						if (IsObsolete (f))
							continue;

						txt = NameCleaner (f.Name);
						typo = GetCachedTypo (checker, txt);
						if (typo.Length > 0) {
							if (!Skip (f, typo)) {
								ReportError ("Typo in FIELD name: {0} - {1}, Type: {2}", f.Name, typo, t.Name);
								totalErrors++;
							}
						}
					}

					var methods = t.GetMethods ();
					foreach (MethodInfo m in methods) {
						if (!m.IsPublic && !m.IsFamily)
							continue;

						if (IsObsolete (m))
							continue;

						txt = NameCleaner (m.Name);
						typo = GetCachedTypo (checker, txt);
						if (typo.Length > 0) {
							if (!Skip (m, typo)) {
								ReportError ("Typo in METHOD name: {0} - {1}, Type: {2}", m.Name, typo, t.Name);
								totalErrors++;
							}
						}
#if false
						var parameters = m.GetParameters ();
						foreach (ParameterInfo p in parameters) {
							txt = NameCleaner (p.Name);
							typo = GetCachedTypo (checker, txt);
							if (typo.Length > 0) {
								ReportError ("Typo in PARAMETER Name: {0} - {1}, Method: {2}, Type: {3}", p.Name, typo, m.Name, t.Name);
								totalErrors++;
							}
						}
#endif
					}
				}
			}
			// verify that all allowed words for the current platform are still needed
			var currentPlatform = TestRuntime.CurrentPlatform;
			var unused = allowed.Keys
				.Where (w => allowed [w].HasFlag (currentPlatform))
				.Except (used);
			foreach (var typo in unused) {
				ReportError ($"Unnecessary allowed typo \"{typo}\" is not present in any API name");
				totalErrors++;
			}
			Assert.AreEqual (0, totalErrors, "Typos!");
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

		Dictionary<string, string> cached_typoes = new Dictionary<string, string> ();
		string GetCachedTypo (SpellChecker checker, string txt)
		{
			if (!cached_typoes.TryGetValue (txt, out var rv))
				cached_typoes [txt] = rv = GetTypo (checker, txt);
			return rv;
		}

		string GetTypo (SpellChecker checker, string txt)
		{
			var checkRange = new NSRange (0, txt.Length);
#if MONOMAC
			var typoRange = checker.CheckSpelling (txt, 0, "en_US", false, 0, out var _);
#else
			var typoRange = checker.RangeOfMisspelledWordInString (txt, checkRange, checkRange.Location, false, "en_US");
#endif
			if (typoRange.Length == 0)
				return String.Empty;
			return txt.Substring ((int) typoRange.Location, (int) typoRange.Length);
		}

		static StringBuilder clean = new StringBuilder ();

		static string NameCleaner (string name)
		{
			clean.Clear ();
			foreach (char c in name) {
				if (Char.IsUpper (c)) {
					clean.Append (' ').Append (c);
					continue;
				}
				if (Char.IsDigit (c)) {
					clean.Append (' ');
					continue;
				}
				switch (c) {
				case '<':
				case '>':
				case '_':
					clean.Append (' ');
					break;
				default:
					clean.Append (c);
					break;
				}
			}
			return clean.ToString ();
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
					Assert.True (Version.TryParse (s, out _), fi.Name);
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
						Assert.True (CheckLibrary (s), fi.Name);
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
						Assert.True (CheckLibrary (s), fi.Name);
					} else {
						Assert.Fail ($"Unknown '{fi.Name}' field cannot be verified - please fix me!");
					}
					break;
				}
			}
		}
	}
}
