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
	[TestFixture]
	public class ApiTypoTest : ApiBaseTest {
#if MONOMAC
		NSSpellChecker? checker;
#else
		UITextChecker checker = new UITextChecker ();
#endif

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

		HashSet<string> allowed = new HashSet<string> () {
			"Aac",
			"Abgr",
			"Accurracy",
			"Achivements",
			"Acos",
			"Acosh",
			"Activatable",
			"Addin",
			"Addl",
			"Addr",
			"Adessive",
			"Agc",
			"Aifc",
			"Aiff",
			"Alg", // short for Algorithm
			"Aliasable",
			"Allative",
			"Amete",
			"Amr",
			"Ane",
			"Anglet",
			"Apac",
			"Apdu",
			"Apng", // Animated Portable Network Graphics
			"Applei",
			"Arcball",
			"Argb",
			"Asin",
			"Asinh",
			"Astc",
			"Atan",
			"Atanh",
			"Atm",
			"Atmos", // Dolby Atmos
			"Atr",
			"Ats", // App Transport Security
			"Atsc",
			"Attrib",
			"Attributevalue",
			"Attrs", // Attributes (used by Apple for keys)
			"Audiofile",
			"Automapping",
			"Automatch",
			"Automounted",
			"Avb", // acronym: Audio Video Bridging
			"Avci", // file type
			"Avg",
			"Bary",
			"Ber",
			"Bggr", // acronym for Blue, Green, Green, Red
			"Bgra", // acrnym for Blue, Green, Red, Alpha
			"Bgrx",
			"Bim",
			"Bitangent",
			"Blinn",
			"Blit",
			"Brotli",
			"Bsln",
			"Cabac",
			"Caf", // acronym: Core Audio Format
			"Callables",
			"Catmull",
			"Cavlc",
			"Cct",
			"Ccw",
			"Cdma",
			"Cea",
			"Celp", // MPEG4ObjectID
			"Celu", // Continuously Differentiable Exponential Linear Unit (ML)
			"Cfa", // acronym: Color Filter Array
			"Chacha",
			"Characterteristic",
			"Cholesky",
			"Chromaticities",
			"Chw",
			"Ciff",
			"Cinepak",
			"Cla",
			"Clearcoat",
			"Clockstamp",
			"Cmaf", // Common Media Application Format (mpeg4)
			"Cmyk", // acronym: Cyan, magenta, yellow and key
			"Cnn", // Convolutional Neural Network
			"Codabar",
			"Conv",
			"Copyback",
			"Csr",
			"Ctor",
			"Cubemap",
			"Daap",
			"Dangi",
			"Dcip", // acronym: Digital Cinema Implementation Partners
			"Decomposables",
			"Deinterlace",
			"Denimonator",
			"Denoise",
			"Denoised",
			"Depthwise",
			"Dequantize",
			"Descendents",
			"Dhe", // Diffie–Hellman key exchange
			"Dhwio",
			"Dicom",
			"Diconnection",
			"Diffable", // that you can diff it.. made up word from apple
			"Differental",
			"Diffie",
			"Dist",
			"dlclose",
			"dlerror",
			"Dlfcn",
			"Dng",
			"Dnssec",
			"Dont",
			"Dopesheet",
			"Downmix", // Sound terminology that means making a stereo mix from a 5.1 surround mix.
			"Dpa",
			"Dpad", // Directional pad (D-pad)
			"Dpads", // plural of above
			"Droste",
			"Dtls",
			"dy",
			"Eap",
			"Ebu",
			"Ecc", // Elliptic Curve Cryptography
			"Ecdh", // Elliptic Curve Diffie–Hellman
			"Ecdhe", // Elliptic Curve Diffie-Hellman Ephemeral
			"Ecdsa", // Elliptic Curve Digital Signature Algorithm
			"Ecies", // Elliptic Curve Integrated Encryption Scheme
			"Ecn", // Explicit Congestion Notification
			"Ect", // ECN Capable Transport
			"Edr",
			"Elative",
			"Elu",
			"Emagic",
			"Embd",
			"Emebedding",
			"Enc",
			"Endc",
			"Eof", // acronym End-Of-File
			"Eppc",
			"Epub",
			"Erf",
			"Essive",
			"Evdo",
			"Exabits",
			"Exbibits",
			"Exhange",
			"Expr",
			"Exr",
			"Extrinsics",
			"Femtowatts",
			"Fft",
			"Formati",
			"Fov",
			"Fqdns",
			"Framebuffer",
			"Framesetter",
			"Freq",
			"Ftps",
			"Gainmap",
			"Gbrg", // acronym for Green-Blue-Reg-Green
			"Gcm",
			"Gelu", // Gaussian Error Linear Unit (ML)
			"Gibibits",
			"Gigapascals",
			"Glorot", // NN
			"Gop", // acronym for Group Of Pictures
			"Gpp",
			"Grbg", // acronym for Green-Red-Blue-Green
			"Gru",
			"Gtin",
			"Gui",
			"Hdr",
			"Heic", // file type
			"Heics", // High Efficiency Image File Format (Sequence)
			"Heif", // High Efficiency Image File Format
			"Hermitean",
			"Hevc", // CMVideoCodecType / High Efficiency Video Coding
			"Hhr",
			"Hindlegs",
			"Hipass",
			"Hlg", // Hybrid Log-Gamma
			"Hls",
			"Hoa",
			"Hrtf", // acronym used in AUSpatializationAlgorithm
			"Hvxc", // MPEG4ObjectID
			"Hwc",
			"Hwio",
			"Icns",
			"Ico",
			"Identd",
			"Iec",
			"Ies",
			"Imageblock",
			"Imap",
			"Imaps",
			"Img",
			"Impl", // BindingImplAttribute
			"Inessive",
			"Inklist",
			"Inot",
			"Inser",
			"Interframe",
			"Interitem",
			"Inv",
			"Invitable",
			"Iou",
			"Ipa",
			"Ipp",
			"Iptc",
			"Ircs",
			"Isrc",
			"Itf",
			"Itu",
			"Itur", // Itur_2020_Hlg
			"Jfif",
			"Keepalive",
			"Keyerror",
			"Keyi",
			"Keypoint",
			"Keypoints",
			"Kibibits",
			"Kiloampere",
			"Kiloamperes",
			"Kiloohms",
			"Kilopascals",
			"ks",
			"Kullback", // Kullback-Leibler Divergence
			"Lacunarity",
			"Latm", //  Low Overhead Audio Transport Multiplex
			"Lbc",
			"Ldaps",
			"Lerp",
			"libcompression",
			"libdispatch",
			"Lingustic",
			"Lod",
			"Lopass",
			"Lowlevel",
			"Lpcm",
			"Lstm",
			"Lte",
			"Ltr",
			"Lun",
			"Lut",
			"Lzfse", // acronym
			"Lzma", // acronym
			"Matchingcoalesce",
			"Mcp", // metacarpophalangeal (hand)
			"Mebibits",
			"Mebx",
			"Megaampere",
			"Megaamperes",
			"Megaliters",
			"Megameters",
			"Megaohms",
			"Megapascals",
			"Metacharacters",
			"Metadatas",
			"Metalness",
			"Mgmt",
			"Microampere",
			"Microamperes",
			"Microohms",
			"Microwatts",
			"Millimoles",
			"Milliohms",
			"Minification",
			"Mmw",
			"Mobike", // acronym
			"Morpher",
			"Mpe", // acronym
			"Mps",
			"Msaa", // multisample anti-aliasing
			"Msi",
			"Mtc", // acronym
			"Mtgp",
			"Mtl",
			"Mtu", // acronym
			"Mul",
			"Mult",
			"Multiary",
			"Multipath",
			"Multipeer",
			"Multiscript",
			"Multivariant",
			"Multiview",
			"Muxed",
			"Nand",
			"Nanograms",
			"Nanowatts",
			"Ncdhw",
			"Nchw",
			"nd",
			"Ndhwc",
			"Nesterov",
			"Nestrov",
			"Nfnt",
			"Nhwc",
			"Nntps",
			"Noop",
			"Ntlm",
			"Ntsc",
			"Objectness",
			"Occlussion",
			"Ocr",
			"Ocsp", // Online Certificate Status Protocol
			"Octree",
			"Ocurrences",
			"Odia",
			"Ohwi",
			"Oid",
			"Oidhw",
			"Oihw",
			"Onnx",
			"Organisation", // kCGImagePropertyIPTCExtRegistryOrganisationID in Xcode9.3-b1
			"Orth",
			"Osa", // Open Scripting Architecture
			"Otsu", // threshold for image binarization
			"ove",
			"Paeth", // PNG filter
			"Palettize",
			"Parms",
			"Pausable",
			"Pcl",
			"Pcm",
			"Pdu",
			"Pebibits",
			"Perlin",
			"Persistable",
			"Persistance",
			"Petabits",
			"Pfs", // acronym
			"Philox",
			"Picometers",
			"Picowatts",
			"Pkcs",
			"Placemark",
			"Playout",
			"Pnc", // MIDI
			"Pnorm",
			"Polyline",
			"Polylines",
			"Popularimeter",
			"Ppk",
			"Preds",
			"Prefilter",
			"Prereleased",
			"Prerolls",
			"Preseti",
			"Propogate",
			"Psec",
			"Psk",
			"Psm", // Protocol/Service Multiplexer
			"Pvr",
			"Pvrtc", // MTLBlitOption - PowerVR Texture Compression
			"Qos",
			"Quadding",
			"Quaterniond",
			"Quic",
			"Qura",
			"Qwac",
			"Reacquirer",
			"Reinvitation",
			"Reinvite",
			"Rel",
			"Relu", // Rectified Linear Unit (ML)
			"Remmote",
			"Replayable",
			"Reprojection",
			"Rgb",
			"Rgba",
			"Rgbaf",
			"Rgbah",
			"Rgbx",
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
			"Rtl",
			"Rtsp",
			"Scc",
			"Scn",
			"Sdr",
			"Seekable",
			"Selu", // Scaled Exponential Linear unit (ML)
			"Sensel",
			"Shadable",
			"Signbit",
			"Sint", // as in "Signed Integer"
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
			"Ssml",
			"st",
			"Standarize",
			"Strided",
			"Subbeat",
			"Subentities",
			"Subheadline",
			"Sublocality",
			"Sublocation",
			"Submesh",
			"Submeshes",
			"Subpixel",
			"Subresources",
			"Subsec",
			"Superentity",
			"Supertype",
			"Supertypes",
			"Svfg",
			"Svg", // Scalable Vector Graphics
			"Svgf",
			"Sysex",
			"Tbgr",
			"Tebibits",
			"Tensorflow",
			"Tessellator",
			"Texcoord",
			"Texel",
			"Tga",
			"th",
			"Threadgroup",
			"Threadgroups",
			"Thumbstick",
			"Tls",
			"Tlv",
			"Toc",
			"Toci",
			"Tonemap",
			"Trc",
			"Tri",
			"Tweening",
			"tx",
			"ty",
			"Udp",
			"Undecodable",
			"Underrun",
			"Unfetched",
			"Unioning",
			"Unmap",
			"Unorm",
			"Unpremultiplied",
			"Unpremultiplying",
			"Unprepare",
			"Unproject",
			"Unpublish",
			"Unsolo",
			"Upce",
			"Usac", // Unified Speech and Audio Coding
			"Usd", // Universal Scene Description
			"Usdz", // USD zip
			"Uterance",
			"Utf",
			"Varispeed",
			"Vbr",
			"Vbv",
			"Vergence",
			"Vnode",
			"Voronoi",
			"Vpn",
			"Vtt",
			"Warichu",
			"Warpable",
			"Wcdma",
			"Writeability",
			"Xbgr",
			"Xmp",
			"Xnor",
			"Xrgb",
			"xy",
			"Xyz",
			"Xzy",
			"Yobibits",
			"Yottabits",
			"yuvs",
			"yx",
			"Yxz",
			"yy",
			"Yyy",
			"Yzx",
			"Zebibits",
			"Zettabits",
			"Zlib",
			"Zxy",
			"Zyx",
#if !__TVOS__
			"Apl",
			"Apns",
			"Axept",
			"Bancomat",
			"Bssid",
			"Cartes", // french
			"Cda", // acronym: Clinical Document Architecture
			"Conecs",
			"Cose",
			"Dankort",
			"Dav",
			"Deca",
			"Dirbursement",
			"Dstu",
			"Ecg",
			"Eftpos", // Electronic funds transfer at point of sale
			"Emaili",
			"Felica", // Japanese contactless RFID smart card system
			"Fhir",
			"Gadu",
			"Girocard",
			"Groupless",
			"Himyan",
			"Histogrammed",
			"Icq",
			"Interac",
			"Intermenstrual",
			"Intravaginal",
			"Itt",
			"Jaywan",
			"Jcb", // Japanese credit card company
			"Jrts",
			"Jws",
			"Kickboard",
			"Mada", // payment system
			"Meeza",
			"Monoline",
			"Muid",
			"Nanaco",
			"Noninteractive",
			"Overline",
			"Phq",
			"Prf",
			"Pskc",
			"Sdnn",
			"Siemen",
			"Subband",
			"Subfilter",
			"Subfilters",
			"Suica", // Japanese contactless smart card type
			"Swolf",
			"Thumbnailing",
			"Timecodes",
			"Tmoney",
			"Udi",
			"Uid",
			"Uti",
			"Waon",
			"Wifes",
			"Willl",
			"Wpa",
#endif
#if !MONOMAC
			"Adjustmentfor",
			"Ancs",
			"Arraycollation",
			"Directionfor",
			"Editability",
			"Hdmi",
			"Hfp",
			"Imagefor",
			"Imei",
			"Intoi",
			"Langauges",
			"Multiselect",
			"Nyquist",
			"Oper",
			"Pci",
			"Rtp",
			"Sel",
			"Subcardioid",
			"Unconfigured",
			"Zenkaku",
#endif
#if !__MACCATALYST__
			"Fieldset",
			"Saml", // acronym
#endif
#if __IOS__
			"Afi",
			"Chapv",
			"Crosstraining",
			"Dfsi",
			"Dhs",
			"Dsf",
			"Dsfi",
			"Dtmf", // DTMF
			"Feli",
			"Flipside",
			"Gbtac",
			"Gbtdc",
			"Iap",
			"Indoorcycle",
			"Indoorrun",
			"Indoorwalk",
			"Mennekes",
			"Mifare",
			"Mncs",
			"Nacs",
			"Nai",
			"Ndef",
			"Nsa",
			"Peap",
			"Photoplethysmogram",
			"Postback",
			"Ssids",
			"Subcaption",
			"Transceive",
			"Ttls",
			"Unentitled",
			"Unmatch",
			"Ussd",
			"Wep",
#endif
#if MONOMAC || (__IOS__ && !__MACCATALYST__)
			"Evictable",
			"Unsynced",
#endif
#if !MONOMAC && !__MACCATALYST__
			"Gles",
#endif
#if MONOMAC || __MACCATALYST__
			"Ahap",
			"Callpout",
			"Cdrom",
			"Descriptorat",
			"Descriptorfor",
			"Emsg", // 4cc
			"Gps",
			"Keycode",
			"Nfc",
			"Nsl", // InternetLocationNslNeighborhoodIcon
			"Sdk",
			"Supression",
			"Uri",
			"Usec",
			"Wlan",
#endif
#if __MACCATALYST__ || __TVOS__
			"Autoredirect",
#endif
#if __IOS__ && !__MACCATALYST__
			"Dop",
			"Raycast",
			"Raycasts",
			"Relocalization",
			"Securit",
			"Tdoa",
			"Thumbsticks",
			"Untrash",
			"Upi",
#endif
#if __MACCATALYST__
			"Voip",
#endif
#if MONOMAC
			"Addons",
			"Aime",
			"Aio",
			"Appactive",
			"Aps",
			"Apv",
			"Arae",
			"Aswas",
			"Attr",
			"Attributesfor",
			"Audiograph",
			"Authenticatable",
			"Autospace",
			"Autostarts",
			"Blockmap",
			"Blockquote",
			"Bsd",
			"Btle", // Bluetooth Low Energy
			"Ccitt",
			"Ciexyz",
			"Cmy", // acronym: Cyan, magenta, yellow
			"Cmyka",
			"Cns",
			"Commited",
			"Constrainted",
			"Ctm",
			"Cymk",
			"Cymka",
			"Dirs",
			"Dismissable",
			"Dissapearing",
			"Distinguised", // ITLibPlaylistPropertyDistinguisedKind
			"Dls",
			"Drm", // MediaItemProperty.IsDrmProtected
			"Dtss",
			"Eisu",
			"Fourty",
			"Froms", // NSMetadataItemWhereFromsKey
			"Gid",
			"Grammarl",
			"Greeking",
			"Hardlink",
			"Hpke",
			"Hsb",
			"Hsba",
			"Ibss",
			"Iconfor",
			"Incrementor",
			"Inode",
			"Instamatic",
			"Interactable",
			"Itemto",
			"Jis",
			"Jwks",
			"Jwt",
			"Keypath",
			"Lzw",
			"Nonenumerated",
			"Nop",
			"Nsevent",
			"Numberof",
			"Pbm",
			"Pde",
			"Performwith",
			"Phy",
			"Preauthentication",
			"Previewable",
			"Ptp",
			"Reassociation",
			"Reauthentication",
			"Rectfrom",
			"Registeration",
			"Sdof",
			"Semitransient",
			"Sixtyfour",
			"Sopen",
			"Sso",
			"Sta",
			"Targetand",
			"Twentyfour",
			"Twips",
			"Unemphasized",
			"Xattr",
			"Xattrs",
			"Yuv",
			"Yuvk",
#endif
#if __TVOS__
			"Cinemagraph",
			"Sdh",
			"Sdtv", // acronym: Standard Definition Tele Vision
#endif
		};

		// tracks which allowed words were actually seen during TypoTest
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
#if MONOMAC
			AssertMatchingOSVersionAndSdkVersion ();
			checker = new NSSpellChecker ();
#else
			// the dictionary used by iOS varies with versions and
			// we don't want to maintain special cases for each version
			var sdk = new Version (Constants.SdkVersion);
			if (!UIDevice.CurrentDevice.CheckSystemVersion (sdk.Major, sdk.Minor))
				Assert.Ignore ("Typos only verified using the latest SDK");

			// that's slow and there's no value to run it on devices as the API names
			// being verified won't change from the simulator
			TestRuntime.AssertSimulatorOrDesktop ("Typos only detected on simulator");
#endif

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
			// verify that all allowed words are still needed
			var unused = allowed.Except (used);
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
		string GetCachedTypo (string txt)
		{
			if (!cached_typoes.TryGetValue (txt, out var rv))
				cached_typoes [txt] = rv = GetTypo (txt);
			return rv;
		}
		public string GetTypo (string txt)
		{
#if MONOMAC
			var checkRange = new NSRange (0, txt.Length);
			nint wordCount;
			var typoRange = checker!.CheckSpelling (txt, 0, "en_US", false, 0, out wordCount);
#else
			var checkRange = new NSRange (0, txt.Length);
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
