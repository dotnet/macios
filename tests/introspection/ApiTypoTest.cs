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
		const ApplePlatform All = ApplePlatform.MacOSX | ApplePlatform.iOS | ApplePlatform.TVOS | ApplePlatform.MacCatalyst;

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
			{ "Addr", All },
			{ "Adessive", All },
			{ "Agc", All },
			{ "Aifc", All },
			{ "Aiff", All },
			{ "Alg", All }, // short for Algorithm
			{ "Aliasable", All },
			{ "Allative", All },
			{ "Amete", All },
			{ "Amr", All },
			{ "Ane", All },
			{ "Anglet", All },
			{ "Apac", All },
			{ "Apdu", All },
			{ "Apng", All }, // Animated Portable Network Graphics
			{ "Applei", All },
			{ "Arcball", All },
			{ "Argb", All },
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
			{ "Attrib", All },
			{ "Attributevalue", All },
			{ "Attrs", All }, // Attributes (used by Apple for keys)
			{ "Audiofile", All },
			{ "Automapping", All },
			{ "Automatch", All },
			{ "Automounted", All },
			{ "Avb", All }, // acronym: Audio Video Bridging
			{ "Avci", All }, // file type
			{ "Avg", All },
			{ "Bary", All },
			{ "Ber", All },
			{ "Bggr", All }, // acronym for Blue, Green, Green, Red
			{ "Bgra", All }, // acrnym for Blue, Green, Red, Alpha
			{ "Bgrx", All },
			{ "Bim", All },
			{ "Bitangent", All },
			{ "Blinn", All },
			{ "Blit", All },
			{ "Brotli", All },
			{ "Bsln", All },
			{ "Cabac", All },
			{ "Caf", All }, // acronym: Core Audio Format
			{ "Callables", All },
			{ "Catmull", All },
			{ "Cavlc", All },
			{ "Cct", All },
			{ "Ccw", All },
			{ "Cdma", All },
			{ "Cea", All },
			{ "Celp", All }, // MPEG4ObjectID
			{ "Celu", All }, // Continuously Differentiable Exponential Linear Unit (ML)
			{ "Cfa", All }, // acronym: Color Filter Array
			{ "Chacha", All },
			{ "Characterteristic", All },
			{ "Cholesky", All },
			{ "Chromaticities", All },
			{ "Chw", All },
			{ "Ciff", All },
			{ "Cinepak", All },
			{ "Cla", All },
			{ "Clearcoat", All },
			{ "Clockstamp", All },
			{ "Cmaf", All }, // Common Media Application Format (mpeg4)
			{ "Cmyk", All }, // acronym: Cyan, magenta, yellow and key
			{ "Cnn", All }, // Convolutional Neural Network
			{ "Codabar", All },
			{ "Conv", All },
			{ "Copyback", All },
			{ "Csr", All },
			{ "Ctor", All },
			{ "Cubemap", All },
			{ "Daap", All },
			{ "Dangi", All },
			{ "Dcip", All }, // acronym: Digital Cinema Implementation Partners
			{ "Decomposables", All },
			{ "Deinterlace", All },
			{ "Denimonator", All },
			{ "Denoise", All },
			{ "Denoised", All },
			{ "Depthwise", All },
			{ "Dequantize", All },
			{ "Descendents", All },
			{ "Dhe", All }, // Diffie–Hellman key exchange
			{ "Dhwio", All },
			{ "Dicom", All },
			{ "Diconnection", All },
			{ "Diffable", All }, // that you can diff it.. made up word from apple
			{ "Differental", All },
			{ "Diffie", All },
			{ "Dist", All },
			{ "dlclose", All },
			{ "dlerror", All },
			{ "Dlfcn", All },
			{ "Dng", All },
			{ "Dnssec", All },
			{ "Dont", All },
			{ "Dopesheet", All },
			{ "Downmix", All }, // Sound terminology that means making a stereo mix from a 5.1 surround mix.
			{ "Dpa", All },
			{ "Dpad", All }, // Directional pad (D-pad)
			{ "Dpads", All }, // plural of above
			{ "Droste", All },
			{ "Dtls", All },
			{ "dy", All },
			{ "Eap", All },
			{ "Ebu", All },
			{ "Ecc", All }, // Elliptic Curve Cryptography
			{ "Ecdh", All }, // Elliptic Curve Diffie–Hellman
			{ "Ecdhe", All }, // Elliptic Curve Diffie-Hellman Ephemeral
			{ "Ecdsa", All }, // Elliptic Curve Digital Signature Algorithm
			{ "Ecies", All }, // Elliptic Curve Integrated Encryption Scheme
			{ "Ecn", All }, // Explicit Congestion Notification
			{ "Ect", All }, // ECN Capable Transport
			{ "Edr", All },
			{ "Elative", All },
			{ "Elu", All },
			{ "Emagic", All },
			{ "Embd", All },
			{ "Emebedding", All },
			{ "Enc", All },
			{ "Endc", All },
			{ "Eof", All }, // acronym End-Of-File
			{ "Eppc", All },
			{ "Epub", All },
			{ "Erf", All },
			{ "Essive", All },
			{ "Evdo", All },
			{ "Exabits", All },
			{ "Exbibits", All },
			{ "Exhange", All },
			{ "Expr", All },
			{ "Exr", All },
			{ "Extrinsics", All },
			{ "Femtowatts", All },
			{ "Fft", All },
			{ "Formati", All },
			{ "Fov", All },
			{ "Fqdns", All },
			{ "Framebuffer", All },
			{ "Framesetter", All },
			{ "Freq", All },
			{ "Ftps", All },
			{ "Gainmap", All },
			{ "Gbrg", All }, // acronym for Green-Blue-Reg-Green
			{ "Gcm", All },
			{ "Gelu", All }, // Gaussian Error Linear Unit (ML)
			{ "Gibibits", All },
			{ "Gigapascals", All },
			{ "Glorot", All }, // NN
			{ "Gop", All }, // acronym for Group Of Pictures
			{ "Gpp", All },
			{ "Grbg", All }, // acronym for Green-Red-Blue-Green
			{ "Gru", All },
			{ "Gtin", All },
			{ "Gui", All },
			{ "Hdr", All },
			{ "Heic", All }, // file type
			{ "Heics", All }, // High Efficiency Image File Format (Sequence)
			{ "Heif", All }, // High Efficiency Image File Format
			{ "Hermitean", All },
			{ "Hevc", All }, // CMVideoCodecType / High Efficiency Video Coding
			{ "Hhr", All },
			{ "Hindlegs", All },
			{ "Hipass", All },
			{ "Hlg", All }, // Hybrid Log-Gamma
			{ "Hls", All },
			{ "Hoa", All },
			{ "Hrtf", All }, // acronym used in AUSpatializationAlgorithm
			{ "Hvxc", All }, // MPEG4ObjectID
			{ "Hwc", All },
			{ "Hwio", All },
			{ "Icns", All },
			{ "Ico", All },
			{ "Identd", All },
			{ "Iec", All },
			{ "Ies", All },
			{ "Imageblock", All },
			{ "Imap", All },
			{ "Imaps", All },
			{ "Img", All },
			{ "Impl", All }, // BindingImplAttribute
			{ "Inessive", All },
			{ "Inklist", All },
			{ "Inot", All },
			{ "Inser", All },
			{ "Interframe", All },
			{ "Interitem", All },
			{ "Inv", All },
			{ "Invitable", All },
			{ "Iou", All },
			{ "Ipa", All },
			{ "Ipp", All },
			{ "Iptc", All },
			{ "Ircs", All },
			{ "Isrc", All },
			{ "Itf", All },
			{ "Itu", All },
			{ "Itur", All }, // Itur_2020_Hlg
			{ "Jfif", All },
			{ "Keepalive", All },
			{ "Keyerror", All },
			{ "Keyi", All },
			{ "Keypoint", All },
			{ "Keypoints", All },
			{ "Kibibits", All },
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
			{ "Matchingcoalesce", All },
			{ "Mcp", All }, // metacarpophalangeal (hand)
			{ "Mebibits", All },
			{ "Mebx", All },
			{ "Megaampere", All },
			{ "Megaamperes", All },
			{ "Megaliters", All },
			{ "Megameters", All },
			{ "Megaohms", All },
			{ "Megapascals", All },
			{ "Metacharacters", All },
			{ "Metadatas", All },
			{ "Metalness", All },
			{ "Mgmt", All },
			{ "Microampere", All },
			{ "Microamperes", All },
			{ "Microohms", All },
			{ "Microwatts", All },
			{ "Millimoles", All },
			{ "Milliohms", All },
			{ "Minification", All },
			{ "Mmw", All },
			{ "Mobike", All }, // acronym
			{ "Morpher", All },
			{ "Mpe", All }, // acronym
			{ "Mps", All },
			{ "Msaa", All }, // multisample anti-aliasing
			{ "Msi", All },
			{ "Mtc", All }, // acronym
			{ "Mtgp", All },
			{ "Mtl", All },
			{ "Mtu", All }, // acronym
			{ "Mul", All },
			{ "Mult", All },
			{ "Multiary", All },
			{ "Multipath", All },
			{ "Multipeer", All },
			{ "Multiscript", All },
			{ "Multivariant", All },
			{ "Multiview", All },
			{ "Muxed", All },
			{ "Nand", All },
			{ "Nanograms", All },
			{ "Nanowatts", All },
			{ "Ncdhw", All },
			{ "Nchw", All },
			{ "nd", All },
			{ "Ndhwc", All },
			{ "Nesterov", All },
			{ "Nestrov", All },
			{ "Nfnt", All },
			{ "Nhwc", All },
			{ "Nntps", All },
			{ "Noop", All },
			{ "Ntlm", All },
			{ "Ntsc", All },
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
			{ "Organisation", All }, // kCGImagePropertyIPTCExtRegistryOrganisationID in Xcode9.3-b1
			{ "Orth", All },
			{ "Osa", All }, // Open Scripting Architecture
			{ "Otsu", All }, // threshold for image binarization
			{ "ove", All },
			{ "Paeth", All }, // PNG filter
			{ "Palettize", All },
			{ "Parms", All },
			{ "Pausable", All },
			{ "Pcl", All },
			{ "Pcm", All },
			{ "Pdu", All },
			{ "Pebibits", All },
			{ "Perlin", All },
			{ "Persistable", All },
			{ "Persistance", All },
			{ "Petabits", All },
			{ "Pfs", All }, // acronym
			{ "Philox", All },
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
			{ "Ppk", All },
			{ "Preds", All },
			{ "Prefilter", All },
			{ "Prereleased", All },
			{ "Prerolls", All },
			{ "Preseti", All },
			{ "Propogate", All },
			{ "Psec", All },
			{ "Psk", All },
			{ "Psm", All }, // Protocol/Service Multiplexer
			{ "Pvr", All },
			{ "Pvrtc", All }, // MTLBlitOption - PowerVR Texture Compression
			{ "Qos", All },
			{ "Quadding", All },
			{ "Quaterniond", All },
			{ "Quic", All },
			{ "Qura", All },
			{ "Qwac", All },
			{ "Reacquirer", All },
			{ "Reinvitation", All },
			{ "Reinvite", All },
			{ "Rel", All },
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
			{ "Rtsp", All },
			{ "Scc", All },
			{ "Scn", All },
			{ "Sdr", All },
			{ "Seekable", All },
			{ "Selu", All }, // Scaled Exponential Linear unit (ML)
			{ "Sensel", All },
			{ "Shadable", All },
			{ "Signbit", All },
			{ "Sint", All }, // as in "Signed Integer"
			{ "Slerp", All },
			{ "Slomo", All },
			{ "Smpte", All },
			{ "Snapshotter", All },
			{ "Snn", All },
			{ "Snorm", All },
			{ "Sobel", All },
			{ "Softmax", All }, // get_SoftmaxNormalization
			{ "Spacei", All },
			{ "Spl", All },
			{ "Sqrt", All },
			{ "Srgb", All },
			{ "Ssid", All },
			{ "Ssml", All },
			{ "st", All },
			{ "Standarize", All },
			{ "Strided", All },
			{ "Subbeat", All },
			{ "Subentities", All },
			{ "Subheadline", All },
			{ "Sublocality", All },
			{ "Sublocation", All },
			{ "Submesh", All },
			{ "Submeshes", All },
			{ "Subpixel", All },
			{ "Subresources", All },
			{ "Subsec", All },
			{ "Superentity", All },
			{ "Supertype", All },
			{ "Supertypes", All },
			{ "Svfg", All },
			{ "Svg", All }, // Scalable Vector Graphics
			{ "Svgf", All },
			{ "Sysex", All },
			{ "Tbgr", All },
			{ "Tebibits", All },
			{ "Tensorflow", All },
			{ "Tessellator", All },
			{ "Texcoord", All },
			{ "Texel", All },
			{ "Tga", All },
			{ "th", All },
			{ "Threadgroup", All },
			{ "Threadgroups", All },
			{ "Thumbstick", All },
			{ "Tls", All },
			{ "Tlv", All },
			{ "Toc", All },
			{ "Toci", All },
			{ "Tonemap", All },
			{ "Trc", All },
			{ "Tri", All },
			{ "Tweening", All },
			{ "tx", All },
			{ "ty", All },
			{ "Udp", All },
			{ "Undecodable", All },
			{ "Underrun", All },
			{ "Unfetched", All },
			{ "Unioning", All },
			{ "Unmap", All },
			{ "Unorm", All },
			{ "Unpremultiplied", All },
			{ "Unpremultiplying", All },
			{ "Unprepare", All },
			{ "Unproject", All },
			{ "Unpublish", All },
			{ "Unsolo", All },
			{ "Upce", All },
			{ "Usac", All }, // Unified Speech and Audio Coding
			{ "Usd", All }, // Universal Scene Description
			{ "Usdz", All }, // USD zip
			{ "Uterance", All },
			{ "Utf", All },
			{ "Varispeed", All },
			{ "Vbr", All },
			{ "Vbv", All },
			{ "Vergence", All },
			{ "Vnode", All },
			{ "Voronoi", All },
			{ "Vpn", All },
			{ "Vtt", All },
			{ "Warichu", All },
			{ "Warpable", All },
			{ "Wcdma", All },
			{ "Writeability", All },
			{ "Xbgr", All },
			{ "Xmp", All },
			{ "Xnor", All },
			{ "Xrgb", All },
			{ "xy", All },
			{ "Xyz", All },
			{ "Xzy", All },
			{ "Yobibits", All },
			{ "Yottabits", All },
			{ "yuvs", All },
			{ "yx", All },
			{ "Yxz", All },
			{ "yy", All },
			{ "Yyy", All },
			{ "Yzx", All },
			{ "Zebibits", All },
			{ "Zettabits", All },
			{ "Zlib", All },
			{ "Zxy", All },
			{ "Zyx", All },
			{ "Fieldset", All & ~ApplePlatform.MacCatalyst },
			{ "Saml", All & ~ApplePlatform.MacCatalyst }, // acronym
			{ "Adjustmentfor", All & ~ApplePlatform.MacOSX },
			{ "Ancs", All & ~ApplePlatform.MacOSX },
			{ "Arraycollation", All & ~ApplePlatform.MacOSX },
			{ "Directionfor", All & ~ApplePlatform.MacOSX },
			{ "Editability", All & ~ApplePlatform.MacOSX },
			{ "Hdmi", All & ~ApplePlatform.MacOSX },
			{ "Hfp", All & ~ApplePlatform.MacOSX },
			{ "Imagefor", All & ~ApplePlatform.MacOSX },
			{ "Imei", All & ~ApplePlatform.MacOSX },
			{ "Intoi", All & ~ApplePlatform.MacOSX },
			{ "Langauges", All & ~ApplePlatform.MacOSX },
			{ "Multiselect", All & ~ApplePlatform.MacOSX },
			{ "Nyquist", All & ~ApplePlatform.MacOSX },
			{ "Oper", All & ~ApplePlatform.MacOSX },
			{ "Pci", All & ~ApplePlatform.MacOSX },
			{ "Rtp", All & ~ApplePlatform.MacOSX },
			{ "Sel", All & ~ApplePlatform.MacOSX },
			{ "Subcardioid", All & ~ApplePlatform.MacOSX },
			{ "Unconfigured", All & ~ApplePlatform.MacOSX },
			{ "Zenkaku", All & ~ApplePlatform.MacOSX },
			{ "Apl", All & ~ApplePlatform.TVOS },
			{ "Apns", All & ~ApplePlatform.TVOS },
			{ "Axept", All & ~ApplePlatform.TVOS },
			{ "Bancomat", All & ~ApplePlatform.TVOS },
			{ "Bssid", All & ~ApplePlatform.TVOS },
			{ "Cartes", All & ~ApplePlatform.TVOS }, // french
			{ "Cda", All & ~ApplePlatform.TVOS }, // acronym: Clinical Document Architecture
			{ "Conecs", All & ~ApplePlatform.TVOS },
			{ "Cose", All & ~ApplePlatform.TVOS },
			{ "Dankort", All & ~ApplePlatform.TVOS },
			{ "Dav", All & ~ApplePlatform.TVOS },
			{ "Deca", All & ~ApplePlatform.TVOS },
			{ "Dirbursement", All & ~ApplePlatform.TVOS },
			{ "Dstu", All & ~ApplePlatform.TVOS },
			{ "Ecg", All & ~ApplePlatform.TVOS },
			{ "Eftpos", All & ~ApplePlatform.TVOS }, // Electronic funds transfer at point of sale
			{ "Emaili", All & ~ApplePlatform.TVOS },
			{ "Felica", All & ~ApplePlatform.TVOS }, // Japanese contactless RFID smart card system
			{ "Fhir", All & ~ApplePlatform.TVOS },
			{ "Gadu", All & ~ApplePlatform.TVOS },
			{ "Girocard", All & ~ApplePlatform.TVOS },
			{ "Groupless", All & ~ApplePlatform.TVOS },
			{ "Himyan", All & ~ApplePlatform.TVOS },
			{ "Histogrammed", All & ~ApplePlatform.TVOS },
			{ "Icq", All & ~ApplePlatform.TVOS },
			{ "Interac", All & ~ApplePlatform.TVOS },
			{ "Intermenstrual", All & ~ApplePlatform.TVOS },
			{ "Intravaginal", All & ~ApplePlatform.TVOS },
			{ "Itt", All & ~ApplePlatform.TVOS },
			{ "Jaywan", All & ~ApplePlatform.TVOS },
			{ "Jcb", All & ~ApplePlatform.TVOS }, // Japanese credit card company
			{ "Jrts", All & ~ApplePlatform.TVOS },
			{ "Jws", All & ~ApplePlatform.TVOS },
			{ "Kickboard", All & ~ApplePlatform.TVOS },
			{ "Mada", All & ~ApplePlatform.TVOS }, // payment system
			{ "Meeza", All & ~ApplePlatform.TVOS },
			{ "Monoline", All & ~ApplePlatform.TVOS },
			{ "Muid", All & ~ApplePlatform.TVOS },
			{ "Nanaco", All & ~ApplePlatform.TVOS },
			{ "Noninteractive", All & ~ApplePlatform.TVOS },
			{ "Overline", All & ~ApplePlatform.TVOS },
			{ "Phq", All & ~ApplePlatform.TVOS },
			{ "Prf", All & ~ApplePlatform.TVOS },
			{ "Pskc", All & ~ApplePlatform.TVOS },
			{ "Sdnn", All & ~ApplePlatform.TVOS },
			{ "Siemen", All & ~ApplePlatform.TVOS },
			{ "Subband", All & ~ApplePlatform.TVOS },
			{ "Subfilter", All & ~ApplePlatform.TVOS },
			{ "Subfilters", All & ~ApplePlatform.TVOS },
			{ "Suica", All & ~ApplePlatform.TVOS }, // Japanese contactless smart card type
			{ "Swolf", All & ~ApplePlatform.TVOS },
			{ "Thumbnailing", All & ~ApplePlatform.TVOS },
			{ "Timecodes", All & ~ApplePlatform.TVOS },
			{ "Tmoney", All & ~ApplePlatform.TVOS },
			{ "Udi", All & ~ApplePlatform.TVOS },
			{ "Uid", All & ~ApplePlatform.TVOS },
			{ "Uti", All & ~ApplePlatform.TVOS },
			{ "Waon", All & ~ApplePlatform.TVOS },
			{ "Wifes", All & ~ApplePlatform.TVOS },
			{ "Willl", All & ~ApplePlatform.TVOS },
			{ "Wpa", All & ~ApplePlatform.TVOS },
			{ "Voip", ApplePlatform.MacCatalyst },
			{ "Autoredirect", ApplePlatform.MacCatalyst | ApplePlatform.TVOS },
			{ "Addons", ApplePlatform.MacOSX },
			{ "Aime", ApplePlatform.MacOSX },
			{ "Aio", ApplePlatform.MacOSX },
			{ "Appactive", ApplePlatform.MacOSX },
			{ "Aps", ApplePlatform.MacOSX },
			{ "Apv", ApplePlatform.MacOSX },
			{ "Arae", ApplePlatform.MacOSX },
			{ "Aswas", ApplePlatform.MacOSX },
			{ "Attr", ApplePlatform.MacOSX },
			{ "Attributesfor", ApplePlatform.MacOSX },
			{ "Audiograph", ApplePlatform.MacOSX },
			{ "Authenticatable", ApplePlatform.MacOSX },
			{ "Autospace", ApplePlatform.MacOSX },
			{ "Autostarts", ApplePlatform.MacOSX },
			{ "Blockmap", ApplePlatform.MacOSX },
			{ "Blockquote", ApplePlatform.MacOSX },
			{ "Bsd", ApplePlatform.MacOSX },
			{ "Btle", ApplePlatform.MacOSX }, // Bluetooth Low Energy
			{ "Ccitt", ApplePlatform.MacOSX },
			{ "Ciexyz", ApplePlatform.MacOSX },
			{ "Cmy", ApplePlatform.MacOSX }, // acronym: Cyan, magenta, yellow
			{ "Cmyka", ApplePlatform.MacOSX },
			{ "Cns", ApplePlatform.MacOSX },
			{ "Commited", ApplePlatform.MacOSX },
			{ "Constrainted", ApplePlatform.MacOSX },
			{ "Ctm", ApplePlatform.MacOSX },
			{ "Cymk", ApplePlatform.MacOSX },
			{ "Cymka", ApplePlatform.MacOSX },
			{ "Dirs", ApplePlatform.MacOSX },
			{ "Dismissable", ApplePlatform.MacOSX },
			{ "Dissapearing", ApplePlatform.MacOSX },
			{ "Distinguised", ApplePlatform.MacOSX }, // ITLibPlaylistPropertyDistinguisedKind
			{ "Dls", ApplePlatform.MacOSX },
			{ "Drm", ApplePlatform.MacOSX }, // MediaItemProperty.IsDrmProtected
			{ "Dtss", ApplePlatform.MacOSX },
			{ "Eisu", ApplePlatform.MacOSX },
			{ "Fourty", ApplePlatform.MacOSX },
			{ "Froms", ApplePlatform.MacOSX }, // NSMetadataItemWhereFromsKey
			{ "Gid", ApplePlatform.MacOSX },
			{ "Grammarl", ApplePlatform.MacOSX },
			{ "Greeking", ApplePlatform.MacOSX },
			{ "Hardlink", ApplePlatform.MacOSX },
			{ "Hpke", ApplePlatform.MacOSX },
			{ "Hsb", ApplePlatform.MacOSX },
			{ "Hsba", ApplePlatform.MacOSX },
			{ "Ibss", ApplePlatform.MacOSX },
			{ "Iconfor", ApplePlatform.MacOSX },
			{ "Incrementor", ApplePlatform.MacOSX },
			{ "Inode", ApplePlatform.MacOSX },
			{ "Instamatic", ApplePlatform.MacOSX },
			{ "Interactable", ApplePlatform.MacOSX },
			{ "Itemto", ApplePlatform.MacOSX },
			{ "Jis", ApplePlatform.MacOSX },
			{ "Jwks", ApplePlatform.MacOSX },
			{ "Jwt", ApplePlatform.MacOSX },
			{ "Keypath", ApplePlatform.MacOSX },
			{ "Lzw", ApplePlatform.MacOSX },
			{ "Nonenumerated", ApplePlatform.MacOSX },
			{ "Nop", ApplePlatform.MacOSX },
			{ "Nsevent", ApplePlatform.MacOSX },
			{ "Numberof", ApplePlatform.MacOSX },
			{ "Pbm", ApplePlatform.MacOSX },
			{ "Pde", ApplePlatform.MacOSX },
			{ "Performwith", ApplePlatform.MacOSX },
			{ "Phy", ApplePlatform.MacOSX },
			{ "Preauthentication", ApplePlatform.MacOSX },
			{ "Previewable", ApplePlatform.MacOSX },
			{ "Ptp", ApplePlatform.MacOSX },
			{ "Reassociation", ApplePlatform.MacOSX },
			{ "Reauthentication", ApplePlatform.MacOSX },
			{ "Rectfrom", ApplePlatform.MacOSX },
			{ "Registeration", ApplePlatform.MacOSX },
			{ "Sdof", ApplePlatform.MacOSX },
			{ "Semitransient", ApplePlatform.MacOSX },
			{ "Sixtyfour", ApplePlatform.MacOSX },
			{ "Sopen", ApplePlatform.MacOSX },
			{ "Sso", ApplePlatform.MacOSX },
			{ "Sta", ApplePlatform.MacOSX },
			{ "Targetand", ApplePlatform.MacOSX },
			{ "Twentyfour", ApplePlatform.MacOSX },
			{ "Twips", ApplePlatform.MacOSX },
			{ "Unemphasized", ApplePlatform.MacOSX },
			{ "Xattr", ApplePlatform.MacOSX },
			{ "Xattrs", ApplePlatform.MacOSX },
			{ "Yuv", ApplePlatform.MacOSX },
			{ "Yuvk", ApplePlatform.MacOSX },
			{ "Ahap", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Callpout", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Cdrom", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Descriptorat", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Descriptorfor", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Emsg", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst }, // 4cc
			{ "Gps", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Keycode", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Nfc", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Nsl", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst }, // InternetLocationNslNeighborhoodIcon
			{ "Sdk", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Supression", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Uri", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Usec", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Wlan", ApplePlatform.MacOSX | ApplePlatform.MacCatalyst },
			{ "Evictable", ApplePlatform.MacOSX | ApplePlatform.iOS },
			{ "Unsynced", ApplePlatform.MacOSX | ApplePlatform.iOS },
			{ "Cinemagraph", ApplePlatform.TVOS },
			{ "Sdh", ApplePlatform.TVOS },
			{ "Sdtv", ApplePlatform.TVOS }, // acronym: Standard Definition Tele Vision
			{ "Dop", ApplePlatform.iOS },
			{ "Raycast", ApplePlatform.iOS },
			{ "Raycasts", ApplePlatform.iOS },
			{ "Relocalization", ApplePlatform.iOS },
			{ "Securit", ApplePlatform.iOS },
			{ "Tdoa", ApplePlatform.iOS },
			{ "Thumbsticks", ApplePlatform.iOS },
			{ "Untrash", ApplePlatform.iOS },
			{ "Upi", ApplePlatform.iOS },
			{ "Afi", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Chapv", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Crosstraining", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Dfsi", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Dhs", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Dsf", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Dsfi", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Dtmf", ApplePlatform.iOS | ApplePlatform.MacCatalyst }, // DTMF
			{ "Feli", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Flipside", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Gbtac", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Gbtdc", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Iap", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Indoorcycle", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Indoorrun", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Indoorwalk", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Mennekes", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Mifare", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Mncs", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Nacs", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Nai", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Ndef", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Nsa", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Peap", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Photoplethysmogram", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Postback", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Ssids", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Subcaption", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Transceive", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Ttls", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Unentitled", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Unmatch", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Ussd", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Wep", ApplePlatform.iOS | ApplePlatform.MacCatalyst },
			{ "Gles", ApplePlatform.iOS | ApplePlatform.TVOS },
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
