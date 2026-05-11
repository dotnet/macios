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
#else
using UIKit;
#endif
using Xamarin.Tests;
using Xamarin.Utils;

#nullable enable

namespace Introspection {
	public abstract class ApiTypoTest : ApiBaseTest {
		protected ApiTypoTest ()
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

		HashSet<string> allowed = new HashSet<string> () {
			"Aac",
			"Abgr",
			"Accurracy",
			"Achivements",
			"Acos",
			"Acosh",
			"Acn",
			"Actionname",
			"Activatable",
			"Activitiy",
			"Addin",
			"Addl",
			"Addr",
			"Adessive",
			"Adjustmentfor",
			"Aes", // Advanced Encryption Standard
			"Afi",
			"Ahap",
			"Aifc",
			"Aiff",
			"Agc",
			"Aio",
			"Alg", // short for Algorithm
			"Allative",
			"Alpn", // Application-Layer Protocol Negotiation RFC7301
			"Amete",
			"Amr",
			"Ancs",
			"Ane",
			"Anglet",
			"Apac",
			"Apdu",
			"Apl",
			"Apng", // Animated Portable Network Graphics
			"Apns",
			"Applei",
			"Aps",
			"Arraycollation",
			"Argb",
			"Asin",
			"Asinh",
			"Astc",
			"Atan",
			"Atanh",
			"Atmos", // Dolby Atmos
			"Atr",
			"Ats",	// App Transport Security
			"Atsc",
			"Attrib",
			"Attributevalue",
			"Attrs", // Attributes (used by Apple for keys)
			"Audiofile",
			"Automapping",
			"Automatch",
			"Automounted",
			"Autoredirect",
			"Autospace",
			"Autostarts",
			"Avci", // file type
			"Avb", // acronym: Audio Video Bridging
			"Aliasable",
			"Arcball",
			"Atm",
			"Avg",
			"Axept",
			"Backface",
			"Bancaire", // french
			"Bancaires", // french
			"Bancomat",
			"Bary",
			"Batc",
			"Ber",
			"Bgra", // acrnym for Blue, Green, Red, Alpha
			"Bgrx",
			"Bim",
			"Biquad",
			"Bitangent",
			"Blinn",
			"Blit",
			"Bokeh",
			"Bggr", // acronym for Blue, Green, Green, Red 
			"Brotli",
			"Bsln",
			"Bssid",
			"Btle", // Bluetooth Low Energy
			"Bzip",
			"Cabac",
			"Caf", // acronym: Core Audio Format
			"Callables",
			"Callpout",
			"Cancellable",
			"Cartes", // french
			"Catmull",
			"Cavlc",
			"Cct",
			"Ccw",
			"Cda", // acronym: Clinical Document Architecture
			"Cdma",
			"Cdrom",
			"Cea",
			"Celu", // Continuously Differentiable Exponential Linear Unit (ML)
			"Cfa", // acronym: Color Filter Array
			"Celp", // MPEG4ObjectID
			"Characterteristic",
			"Chapv",
			"Cholesky",
			"Chacha",
			"Chromaticities",
			"Chw",
			"Ciexyz",
			"Ciff",
			"Cinepak",
			"Cla",
			"Clearcoat",
			"Clockstamp",
			"Cnn", // Convolutional Neural Network
			"Cns",
			"Codabar",
			"Colos",
			"Commerical",
			"Composable",
			"Conecs",
			"Conflictserror",
			"Connnect",
			"Conv",
			"Cose",
			"Counterclock",
			"Copyback",
			"Craete",
			"Crosstraining",
			"Csr",
			"Ctor",
			"Cubemap",
			"Cmaf", // Common Media Application Format (mpeg4)
			"Cmy", // acronym: Cyan, magenta, yellow
			"Cmyk", // acronym: Cyan, magenta, yellow and key
			"Daap",
			"Dangi",
			"Dankort",
			"Dav",
			"Dcip", // acronym: Digital Cinema Implementation Partners
			"Deca",
			"Decomposables",
			"Deinterlace",
			"Denimonator",
			"Denoise",
			"Denoised",
			"Depthwise",
			"Dequantize",
			"Descendents",
			"Descrete",
			"Descriptorat",
			"Descriptorfor",
			"Dfsi",
			"Dhe", // Diffie–Hellman key exchange
			"Dhs",
			"Dhwio",
			"Dicom",
			"Diconnection",
			"Diffable", // that you can diff it.. made up word from apple
			"Differental",
			"Diffie",
			"Dirbursement",
			"Directionfor",
			"Dist",
			"dlclose",
			"dlerror",
			"Dlfcn",
			"dlopen",
			"Dls",
			"Dlsym",
			"dlsym",
			"Dng",
			"Dns",
			"Dnssec",
			"Dont",
			"Dop",
			"Dopesheet",
			"Downsample",
			"Downmix", // Sound terminology that means making a stereo mix from a 5.1 surround mix.
			"Dpa",
			"Dpad", // Directional pad (D-pad)
			"Dpads", // plural of above
			"Droste",
			"Dsf",
			"Dsfi",
			"Dstu",
			"Dtls",
			"Dtmf", // DTMF
			"dy",
			"Eap",
			"Ebu",
			"Ecc",   // Elliptic Curve Cryptography
			"Ecdh",  // Elliptic Curve Diffie–Hellman
			"Ecdhe", // Elliptic Curve Diffie-Hellman Ephemeral
			"Ecdsa", // Elliptic Curve Digital Signature Algorithm
			"Ecg",
			"Ecies", // Elliptic Curve Integrated Encryption Scheme
			"Ecn",   // Explicit Congestion Notification
			"Ect",   // ECN Capable Transport
			"Editability",
			"Edr",
			"Elative",
			"Emebedding",
			"Endc",
			"Eof", // acronym End-Of-File
			"Elu",
			"Emagic",
			"Emaili",
			"Embd",
			"Emsg",	// 4cc
			"Enc",
			"Eppc",
			"Epub",
			"Eftpos", // Electronic funds transfer at point of sale
			"Eotf", // DisplayP3_PQ_Eotf
			"Erf",
			"Essive",
			"Evdo",
			"Exabits",
			"Exbibits",
			"Exbibytes",
			"Exhange",
			"Exp",
			"Expr",
			"Exr",
			"Extrinsics",
			"Feli",
			"Felica", // Japanese contactless RFID smart card system
			"Femtowatts",
			"Fft",
			"Fhir",
			"Flipside",
			"Formati",
			"Fov",
			"Fqdns",
			"Framebuffer",
			"Framesetter",
			"Froms", // NSMetadataItemWhereFromsKey
			"Freq",
			"Ftps",
			"Func",
			"Gadu",
			"Gainmap",
			"Gbrg",	// acronym for Green-Blue-Reg-Green
			"Gbtac",
			"Gbtdc",
			"Gcm",
			"Gelu", // Gaussian Error Linear Unit (ML)
			"Geocoder",
			"Gigapascals",
			"Gibibits",
			"Gibibytes",
			"Girocard",
			"Glorot", // NN
			"Gop", // acronym for Group Of Pictures
			"Gpp",
			"Gps",
			"Gpu",	// acronym for Graphics Processing Unit
			"Grbg", // acronym for Green-Red-Blue-Green
			"Groupless",
			"Gru",
			"Greeking",
			"Gtin",
			"Gui",
			"Hardlink",
			"Heics", // High Efficiency Image File Format (Sequence)
			"Hdmi",
			"Hdr",
			"Hectopascals",
			"Heic", // file type
			"Heif", // file type
			"Hermitean",
			"Hevc", // CMVideoCodecType / High Efficiency Video Coding
			"Heif", // High Efficiency Image File Format
			"Hfp",
			"Hhr",
			"Himyan",
			"Hindlegs",
			"Hipass",
			"Histogrammed",
			"Hlg", // Hybrid Log-Gamma
			"Hls",
			"Hoa",
			"Hrtf", // acronym used in AUSpatializationAlgorithm
			"Hvxc", // MPEG4ObjectID
			"Hwc",
			"Hwio",
			"Iap",
			"Icns",
			"Ico",
			"Iec",
			"Ies",
			"Icq",
			"Ident",
			"Identd",
			"Imageblock",
			"Imagefor",
			"Imap",
			"Imaps",
			"Imei",
			"Img",
			"Impl", // BindingImplAttribute
			"Inessive",
			"Inot",
			"Intravaginal",
			"Inv",
			"Indoorrun",
			"Indoorcycle",
			"Inklist",
			"Indeterm",
			"Indoorwalk",
			"Inode",
			"Inser",
			"Instamatic",
			"Interac",
			"Interframe",
			"Interitem",
			"Intermenstrual",
			"Intersector",
			"Intoi",
			"Invitable",
			"Ios",
			"Iou",
			"Ipa",
			"Ipp",
			"Iptc",
			"Ircs",
			"Iso",
			"Isrc",
			"Itf",
			"Itt",
			"Itu",
			"Itur", // Itur_2020_Hlg
			"Jaywan",
			"Jcb", // Japanese credit card company
			"Jfif",
			"Jis",
			"Jrts",
			"Json",
			"Jws",
			"Keepalive",
			"Keycode",
			"Keyerror",
			"Keyi",
			"Keypoint",
			"Keypoints",
			"Keyspace",
			"Kickboard",
			"ks",
			"Kibibits",
			"Kibibytes",
			"Kiloampere",
			"Kiloamperes",
			"Kiloohms",
			"Kilopascals",
			"Kullback", // Kullback-Leibler Divergence
			"Langauges",
			"Lacunarity",
			"Latm", //  Low Overhead Audio Transport Multiplex
			"Lbc",
			"Ldaps",
			"Lerp",
			"Linecap",
			"Lingustic",
			"libcompression",
			"libdispatch",
			"Loas", // Low Overhead Audio Stream 
			"Lod",
			"Lopass",
			"Lowlevel",
			"Lpcm",
			"Lstm",
			"Lte",
			"Ltr",
			"Lun",
			"Luma",
			"Lut",
			"Lzfse", // acronym
			"Lzma", // acronym
			"Mada", // payment system
			"Mapbuffer",
			"Matchingcoalesce",
			"Mcp", // metacarpophalangeal (hand)
			"Mebibits",
			"Mebibytes",
			"Mebx",
			"Meeza",
			"Megaampere",
			"Megaamperes",
			"Megaliters",
			"Megameters",
			"Megaohms",
			"Megapascals",
			"Mennekes",
			"Metacharacters",
			"Metalness",
			"Metadatas",
			"Microampere",
			"Microamperes",
			"Microohms",
			"Microwatts",
			"Mifare",
			"Millimoles",
			"Milliohms",
			"Mimap",
			"Minification",
			"Mmw",
			"Mncs",
			"Mgmt",
			"Mobike", // acronym
			"Monoline",
			"Morpher",
			"Msi",
			"Mtl",
			"mtouch",
			"Mpe", // acronym
			"Mps",
			"Msaa", // multisample anti-aliasing 
			"Mtu", // acronym
			"Mtc", // acronym
			"Mtgp",
			"Muid",
			"Mul",
			"Mult",
			"Multiary",
			"Multihead",
			"Multipath",
			"Multipeer",
			"Multiscript",
			"Multiselect",
			"Multivariant",
			"Multiview",
			"Muxed",
			"Nacs",
			"Nai",
			"Nanaco",
			"Nand",
			"Nanograms",
			"Nanowatts",
			"Ncdhw",
			"Nchw",
			"nd",
			"Ndhwc",
			"Nestrov",
			"Nesterov",
			"Nfc",
			"nfloat",
			"Nfnt",
			"Nhwc",
			"nint",
			"Nntps",
			"Noninteractive",
			"Noop",
			"Nsa",
			"Ntlm",
			"Nsl", // InternetLocationNslNeighborhoodIcon
			"Ntsc",
			"nuint",
			"Ndef",
			"Noi", // From NoiOSAttribute
			"Nop",
			"Numbernumber",
			"Nyquist",
			"Oaep", // Optimal asymmetric encryption padding
			"Objectfor",
			"Objectness",
			"Occlussion",
			"Ocr",
			"Ocurrences",
			"Ocsp", // Online Certificate Status Protocol
			"Octree",
			"Odia",
			"Ohwi",
			"Oid",
			"Oidhw",
			"Oihw",
			"Oneup", // TVElementKeyOneupTemplate
			"Onnx",
			"Oper",
			"Organisation", // kCGImagePropertyIPTCExtRegistryOrganisationID in Xcode9.3-b1
			"Orthographyrange",
			"Orth",
			"Osa", // Open Scripting Architecture
			"Otsu", // threshold for image binarization
			"ove",
			"Overline",
			"Paeth", // PNG filter
			"Palettize",
			"Parms", // short for Parameters
			"Pci",
			"Peap",
			"Pebibits",
			"Pebibytes",
			"Petabits",
			"Perlin",
			"Persistable",
			"Pausable",
			"Pcl",
			"Pcm",
			"Pdu",
			"Persistance",
			"Pesented",
			"Pfs", // acronym
			"Philox",
			"Photoplethysmogram",
			"Phq",
			"Picometers",
			"Picowatts",
			"Pkcs",
			"Placemark",
			"Playout",
			"Playthrough",
			"Pnc", // MIDI
			"Pnorm",
			"Pointillize",
			"Polyline",
			"Polylines",
			"Popularimeter",
			"Postback",
			"Ppk",
			"Preds", // short for Predicates
			"Prefilter",
			"Prereleased",
			"Prerolls",
			"Preseti",
			"Prev",
			"Prf",
			"Privs", // SharingPrivsNotApplicableIcon
			"Propogate",
			"Psec",
			"Pskc",
			"Psm", // Protocol/Service Multiplexer
			"Psk",
			"Ptp",
			"Pvr",
			"Pvrtc", // MTLBlitOption - PowerVR Texture Compression
			"Qos",
			"Quaterniond",
			"Quadding",
			"Qura",
			"Quic",
			"Qwac",
			"Reacquirer",
			"Reinvitation",
			"Reinvite",
			"Rel",
			"Relocalization",
			"Relu", // Rectified Linear Unit (ML)
			"Relun", // ReLUn - degree n Hermite coefficients
			"Remmote",
			"Reprandial",
			"Replayable",
			"Reprojection",
			"Requestwith",
			"Rgbaf",
			"Rgbah",
			"Rgbx",
			"Ridesharing",
			"Rgb",
			"Rgba",
			"Rggb", // acronym for Red, Green, Green, Blue
			"Rint",
			"Rle",
			"Rnn",
			"Roi",
			"Romm", // acronym: Reference Output Medium Metric
			"Rpa",
			"Rpn", // acronym
			"Rsa", // Rivest, Shamir and Adleman
			"Rsapss",
			"Rsqrt", // reciprocal square root
			"Rssi",
			"Rtp",
			"Rtl",
			"Rtsp",
			"Saml", // acronym
			"Scc",
			"Sdof",
			"Scn",
			"Sdk",
			"Sdr",
			"Sdtv", // acronym: Standard Definition Tele Vision
			"Sdnn",
			"Seekable",
			"Sel",
			"Selu", // Scaled Exponential Linear unit (ML)
			"Sensel",
			"Sgd", // Stochastic Gradient Descent (ML)
			"Shadable",
			"Sharegroup",
			"Sha", //  Secure Hash Algorithm
			"Siemen",
			"Signbit",
			"simd",
			"Sinh",
			"Sint", // as in "Signed Integer"
			"Simd",
			"Slerp",
			"Slomo",
			"Smpte",
			"Snapshotter",
			"Snn",
			"Snorm",
			"Sobel",
			"Softmax", // get_SoftmaxNormalization
			"Spacei",
			"Spl",
			"Sqrt",
			"Srgb",
			"Ssid",
			"Ssids",
			"Ssml",
			"st",
			"Standarize",
			"Stateful",
			"Stateright",
			"Strided",
			"Subband",
			"Subbeat",
			"Subcaption",
			"Subcardioid",
			"Subentities",
			"Subfilter",
			"Subfilters",
			"Subheadline",
			"Sublocality",
			"Sublocation",
			"Submesh",
			"Submeshes",
			"Subpixel",
			"Subresource",
			"Subresources",
			"Subsec",
			"Suica", // Japanese contactless smart card type
			"Superentity",
			"Supertype",
			"Supertypes",
			"Supression",
			"Svfg",
			"Svg", // Scalable Vector Graphics
			"Svgf",
			"Swolf",
			"Sym",
			"Synchronizable",
			"Symbologies",
			"Sysex",
			"Tanh",
			"Tbgr",
			"Tebibits",
			"Tebibytes",
			"Tensorflow",
			"Tessellator",
			"Texcoord",
			"Texel",
			"Tga",
			"th",
			"Threadgroup",
			"Threadgroups",
			"Thumbnailing",
			"Thumbstick",
			"Thumbsticks",
			"Timecodes",
			"Timelapse",
			"Timelapses",
			"Tls",
			"Tmoney",
			"Tonemap",
			"Tri",
			"Ttls",
			"Tlv",
			"Toc",
			"Toci",
			"Toi",
			"Transceive",
			"Trc",
			"Truncantion",
			"Tweening",
			"Twips",
			"tx",
			"ty",
			"Udi",
			"Udp",
			"Unconfigured",
			"Undecodable",
			"Unemphasized",
			"Underrun",
			"Unfetched",
			"Unflagged",
			"Unfocusing",
			"Uid",
			"Unioning",
			"Unmap",
			"Unmatch",
			"Unorm",
			"Unpremultiplied",
			"Unpremultiplying",
			"Unprepare",
			"Unproject",
			"Unpublish",
			"Unsolo",
			"Usec",
			"Ussd",
			"Uterance",
			"Unentitled",
			"Untrash",
			"Utf",
			"Upce",
			"Uri",
			"Usac", // Unified Speech and Audio Coding
			"Usd", // Universal Scene Description
			"Usdz", // USD zip
			"Uti",
			"Varispeed",
			"Vbr",
			"Vbv",
			"Vergence",
			"Voip",
			"Voronoi",
			"Vnode",
			"Vpn",
			"Vtt",
			"Waon",
			"Warichu",
			"Wcdma",
			"Wep",
			"Willl",
			"Wlan",
			"Wpa",
			"Warpable",
			"Whitespaces",
			"Wifes",
			"Writeability",
			"Xbgr",
			"Xmp",
			"Xnor",
			"Xpc",
			"Xrgb",
			"xy",
			"Xyz",
			"Xzy",
			"Yobibits",
			"Yobibytes",
			"Yottabits",
			"Yxz",
			"Yzx",
			"Zenkaku",
			"Zxy",
			"Zyx",
			"Yuv",
			"Yuvk",
			"yuvs",
			"yx",
			"yy",
			"Yyy",
			"Zebibits",
			"Zebibytes",
			"Zettabits",
			"Zlib",
#if MONOMAC
			"Abbr",
			"Accum",
			"Ack", // TcpSetDisableAckStretching
			"Addin",
			"Addons",
			"Appactive",
			"Approx",
			"Arae",
			"Attr",
			"Attributesfor",
			"Autoresizin",
			"Avc",
			"Callpout",
			"Ccitt",
			"Commited",
			"Constrainted",
			"Ctm",
			"Cymk",
			"Cymka",
			"Cmyka",
			"Compat",
			"Credendtials",
			"Descriptorat",
			"Descriptorfor",
			"Dimensionsfor",
			"Dissapearing",
			"Distinguised", // ITLibPlaylistPropertyDistinguisedKind
			"Dirs",
			"Drm", // MediaItemProperty.IsDrmProtected 
			"Editability",
			"Eisu",
			"Entryat",
			"Equiv",
			"Fourty",
			"Grammarl",
			"Greeking",
			"Hsb",
			"Hsba",
			"Ibss",
			"Iconfor",
			"Incrementor",
			"Indexeffective",
			"Indexestable",
			"Itemto",
			"Lowsrc",
			"Lpcm",
			"Lzw",
			"Mihret",
			"Mps",
			"Nonenumerated",
			"Nsevent",
			"Numberof",
			"Orginal",
			"Parms",
			"Pbm",
			"Pde",
			"Performwith",
			"Phy",
			"Pmgt",
			"Preceeding",
			"Preds",
			"Previewable",
			"Qtvr",
			"Rangewith",
			"Rangeswith",
			"Reassociation",
			"Rectfrom",
			"Registeration",
			"Segmentnew",
			"Semitransient",
			"Sixtyfour",
			"Sourcei",
			"Steppable",
			"Stringto",
			"Succesfully",
			"Supression",
			"Targetand",
			"Tkip",
			"Tsn",
			"Tunesi",
			"Twentyfour",
			"Uneditable",
			"Unfocus",
			"Unpublish",
			"Usec",
			"Usedby",
			"Viewwrite",
			"Wep",
			"Wlan",
			"Wme",
			"Writeln",
			"Xattr",
#endif
		};

		// ease maintenance of the list
		HashSet<string> used = new HashSet<string> ();

		bool SkipAllowed (string? typeName, string? methodName, string typo)
		{
			if (allowed.Contains (typo)) {
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
			var types = Assembly.GetTypes ();
			int totalErrors = 0;
			foreach (Type t in types) {
				if (t.IsPublic) {
					if (IsObsolete (t))
						continue;

					string txt = NameCleaner (t.Name);
					var typo = GetCachedTypo (txt);
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
						typo = GetCachedTypo (txt);
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
						typo = GetCachedTypo (txt);
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
							typo = GetCachedTypo (txt);
							if (typo.Length > 0) {
								ReportError ("Typo in PARAMETER Name: {0} - {1}, Method: {2}, Type: {3}", p.Name, typo, m.Name, t.Name);
								totalErrors++;
							}
						}
#endif
					}
				}
			}
#if false
			// ease removal of unrequired values (but needs to be checked for every profile)
			var unused = allowed.Except (used);
			foreach (var typo in unused)
				Console.WriteLine ("Unused entry \"{0}\"", typo);
#endif
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
		string GetCachedTypo (string txt)
		{
			if (!cached_typoes.TryGetValue (txt, out var rv))
				cached_typoes [txt] = rv = GetTypo (txt);
			return rv;
		}
		public abstract string GetTypo (string txt);

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
