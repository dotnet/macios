//
// Link All [Regression] Tests
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012-2016 Xamarin Inc. All rights reserved.
//

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using MonoTouch;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using StoreKit;
#if __MACOS__ || __IOS__
using PdfKit;
#endif
#if !__MACOS__
using UIKit;
#endif
using NUnit.Framework;
using MonoTests.System.Net.Http;

namespace LinkAll {

	// we DO NOT want the code to be "fully" available
	public class NotPreserved {

		public byte One {
			get; set;
		}

		[DefaultValue (2)]
		public int Two {
			get; set;
		}
	}

	// nothing directly uses Parent
	public class Parent {
		// but the nested type is a subclass of NSObject and gets preserved (as it's not part of monotouch.dll)
		public class Derived : NSObject {
			[Export ("foo")]
			public void Foo () { }
		}

		public void UnusedMethod () { }
	}

	[TestFixture]
	// we want the tests to be available because we use the linker
	[Preserve (AllMembers = true)]
	public class LinkAllRegressionTest {
#if __MACCATALYST__
		public const string NamespacePrefix = "";
		public const string AssemblyName = "Microsoft.MacCatalyst";
#elif __IOS__
		public const string NamespacePrefix = "";
		public const string AssemblyName = "Microsoft.iOS";
#elif __TVOS__
		public const string NamespacePrefix = "";
		public const string AssemblyName = "Microsoft.tvOS";
#elif __MACOS__
		public const string NamespacePrefix = "";
		public const string AssemblyName = "Microsoft.macOS";
#else
#error Unknown platform
#endif

		Type not_preserved_type = typeof (NotPreserved);


		class TypeAttribute : Attribute {
			public TypeAttribute (Type type) { }
		}

		[Type (null)]
		public void NullTypeInAttribute ()
		{
			// there's no need to execute this test.
			// desk #68380.
		}

		[Test]
		public void GetterOnly ()
		{
			// that ensure the getter is not linked away, 
			// which means the property will be available for MEF_3862
			NotPreserved np = new NotPreserved ();
			Assert.That (np.Two, Is.EqualTo (0), "Two==0");

			PropertyInfo pi = not_preserved_type.GetProperty ("Two");
			// check the *unused* setter absence from the application
			Assert.NotNull (pi.GetGetMethod (), "getter");
			Assert.Null (pi.GetSetMethod (), "setter");
		}

		[Test]
		public void SetterOnly ()
		{
			// that ensure the setter is not linked away, 
			NotPreserved np = new NotPreserved ();
			np.One = 1;

			PropertyInfo pi = not_preserved_type.GetProperty ("One");
			// check the *unused* setter absence from the application
			Assert.Null (pi.GetGetMethod (), "getter");
			Assert.NotNull (pi.GetSetMethod (), "setter");
		}

		[Test]
		public void MEF_3862 ()
		{
			// note: avoiding using "typeof(DefaultValueAttribute)" in the code
			// so the linker does not keep it just because of it
			PropertyInfo pi = not_preserved_type.GetProperty ("Two");
			object [] attrs = pi.GetCustomAttributes (false);
			bool default_value = false;
			foreach (var ca in attrs) {
				if (ca.GetType ().Name == "DefaultValueAttribute") {
					default_value = true;
					break;
				}
			}
			Assert.True (default_value, "DefaultValue");
		}

		static void Check (string calendarName, bool present)
		{
			var type = Type.GetType ("System.Globalization." + calendarName);
			bool success = present == (type is not null);
			Assert.AreEqual (present, type is not null, calendarName);
		}

		[Test]
		public void Calendars ()
		{
			Check ("GregorianCalendar", true);
			Check ("UmAlQuraCalendar", true);
			Check ("HijriCalendar", true);
			Check ("ThaiBuddhistCalendar", true);
		}

		public enum CertificateProblem : long {
			CertEXPIRED = 0x800B0101,
			CertVALIDITYPERIODNESTING = 0x800B0102,
			CertROLE = 0x800B0103,
			CertPATHLENCONST = 0x800B0104,
			CertCRITICAL = 0x800B0105,
			CertPURPOSE = 0x800B0106,
			CertISSUERCHAINING = 0x800B0107,
			CertMALFORMED = 0x800B0108,
			CertUNTRUSTEDROOT = 0x800B0109,
			CertCHAINING = 0x800B010A,
			CertREVOKED = 0x800B010C,
			CertUNTRUSTEDTESTROOT = 0x800B010D,
			CertREVOCATION_FAILURE = 0x800B010E,
			CertCN_NO_MATCH = 0x800B010F,
			CertWRONG_USAGE = 0x800B0110,
			CertUNTRUSTEDCA = 0x800B0112,
			CertTRUSTEFAIL = 0x800B010B,
		}

#if !__MACOS__
		[Test]
		public void DetectPlatform ()
		{
			// for (future) nunit[lite] platform detection - if this test fails then platform detection won't work
			var typename = NamespacePrefix + "UIKit.UIApplicationDelegate, " + AssemblyName;
			Assert.NotNull (Helper.GetType (typename), typename);
			Assert.Null (Helper.GetType ("Mono.Runtime"), "Mono.Runtime");
		}
#endif // !__MACOS__

		[Test]
#pragma warning disable CA1418 // The platform '*' is not a known platform name
		[SupportedOSPlatform ("none")]
		[UnsupportedOSPlatform ("none)")]
#pragma warning restore CA1418
		[ThreadSafe]
		public void RemovedAttributes ()
		{
			// Don't use constants here, because the linker can see what we're trying to do and keeps what we're verifying has been removed.
			string prefix = NamespacePrefix;
			string suffix = AssemblyName;

			// since we're linking the attributes will NOT be available - even if they are used
			Assert.Null (Helper.GetType (prefix + "ObjCRuntime.IntroducedAttribute, " + suffix), "IntroducedAttribute");
			Assert.Null (Helper.GetType (prefix + "ObjCRuntime.DeprecatedAttribute, " + suffix), "DeprecatedAttribute");
			Assert.Null (Helper.GetType (prefix + "ObjCRuntime.ObsoletedAttribute, " + suffix), "ObsoletedAttribute");
			Assert.Null (Helper.GetType (prefix + "ObjCRuntime.UnavailableAttribute, " + suffix), "UnavailableAttribute");
			Assert.Null (Helper.GetType (prefix + "ObjCRuntime.ThreadSafeAttribute, " + suffix), "ThreadSafeAttribute");
			Assert.Null (Helper.GetType ("System.Runtime.Versioning.SupportedOSPlatformAttribute, " + suffix), "SupportedOSPlatformAttribute");
			Assert.Null (Helper.GetType ("System.Runtime.Versioning.UnsupportedOSPlatformAttribute, " + suffix), "UnsupportedOSPlatformAttribute");
		}

		[Test]
		public void Assembly_Load ()
		{
			Assembly mscorlib = Assembly.Load ("System.Private.CoreLib.dll");
			Assert.NotNull (mscorlib, "System.Private.CoreLib.dll");
		}

		string FindAssemblyPath ()
		{
			var filename = Path.GetFileName (GetType ().Assembly.Location);
			var bundlePath = NSBundle.MainBundle.BundlePath;
			var isExtension = bundlePath.EndsWith (".appex", StringComparison.Ordinal);
			var mainBundlePath = bundlePath;
			if (isExtension)
				mainBundlePath = Path.GetDirectoryName (Path.GetDirectoryName (bundlePath));
			foreach (var filepath in Directory.EnumerateFiles (mainBundlePath, filename, SearchOption.AllDirectories)) {
				var fname = Path.GetFileName (filepath);
				if (filepath.EndsWith ($"{fname}.framework/{fname}", StringComparison.Ordinal)) {
					// This isn't the assembly, but the native AOT'ed executable for the assembly.
					continue;
				}

				if (isExtension) {
					return "../../" + filepath.Substring (mainBundlePath.Length + 1);
				} else {
					return filepath.Substring (mainBundlePath.Length + 1);
				}
			}
			throw new FileNotFoundException ($"Could not find the assembly ${filename} in the bundle {bundlePath}.");
		}

		[Test]
		public void Assembly_LoadFile ()
		{
			string filename = FindAssemblyPath ();
			Assert.NotNull (Assembly.LoadFile (Path.GetFullPath (filename)), "1");
		}

		[Test]
		public void Assembly_LoadFrom ()
		{
			string filename = FindAssemblyPath ();
			Assert.NotNull (Assembly.LoadFrom (filename), "1");
		}

		[Test]
		public void Assembly_ReflectionOnlyLoadFrom ()
		{
			string filename = FindAssemblyPath ();
			// new behavior across all platforms, see https://github.com/dotnet/runtime/issues/50529
#pragma warning disable SYSLIB0018 // 'Assembly.ReflectionOnlyLoadFrom(string)' is obsolete: 'ReflectionOnly loading is not supported and throws PlatformNotSupportedException.'
			Assert.Throws<PlatformNotSupportedException> (() => Assembly.ReflectionOnlyLoadFrom (filename));
#pragma warning restore SYSLIB0018
		}

#if !__TVOS__ && !__MACOS__
		[Test]
		public void Pasteboard_ImagesTest ()
		{
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			using (var dp = new CGDataProvider (file)) {
				using (var cgimg = CGImage.FromPNG (dp, null, false, CGColorRenderingIntent.Default)) {
					using (var img = new UIImage (cgimg)) {
						UIPasteboard.General.Images = new UIImage [] { img };
						if (TestRuntime.CheckXcodeVersion (8, 0))
							Assert.True (UIPasteboard.General.HasImages, "HasImages");

						Assert.AreEqual (1, UIPasteboard.General.Images.Length, "a - length");

						UIPasteboard.General.Images = new UIImage [] { img, img };
						Assert.AreEqual (2, UIPasteboard.General.Images.Length, "b - length");
						Assert.IsNotNull (UIPasteboard.General.Images [0], "b - nonnull[0]");
						Assert.IsNotNull (UIPasteboard.General.Images [1], "b - nonnull[0]");
					}
				}
			}
		}
#endif // !__TVOS__

		[Test]
		public void UltimateBindings ()
		{
			Assert.IsNotNull (Bindings.Test.UltimateMachine.SharedInstance, "SharedInstance");
		}

		#region bug 14456

		[TypeConverter (typeof (TestEnumTypeConverter))]
		public enum TestEnum {
			One, Two
		}

		[Preserve (AllMembers = true)]
		public class TestEnumTypeConverter : System.ComponentModel.TypeConverter {

			public override object ConvertTo (ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				return "hello";
			}
		}

		[Test]
		public void Bug14456 ()
		{
			var tc = TypeDescriptor.GetConverter (typeof (TestEnum));
			// not using Is.TypeOf since it would give additional clue to the linker
			Assert.That (tc.GetType ().Name, Is.EqualTo ("TestEnumTypeConverter"), "TestEnumTypeConverter");
			// notes:
			// * without [Preserve (AllMembers = true)] -> MissingMethodException : Default constructor 
			// * without [TypeConverter (typeof (TestEnumTypeConverter))] -> EnumConverter
		}

		#endregion

		[Test]
		public void SingleEpsilon_Compare ()
		{
			TestRuntime.AssertNotDevice ("Known to fail on devices, see bug #15802");
			// works on some ARM CPU (e.g. iPhone5S) but not others (iPad4 or iPodTouch5)
			Assert.That (Single.Epsilon, Is.Not.EqualTo (0f), "Epsilon");
			Assert.That (-Single.Epsilon, Is.Not.EqualTo (0f), "-Epsilon");
		}

		[Test]
		public void SingleEpsilon_ToString ()
		{
			TestRuntime.AssertNotDevice ("Known to fail on devices, see bug #15802");
			var ci = CultureInfo.InvariantCulture;
			Assert.That (Single.Epsilon.ToString (ci), Is.EqualTo ("1E-45"), "Epsilon.ToString()");
			Assert.That ((-Single.Epsilon).ToString (ci), Is.EqualTo ("-1E-45"), "-Epsilon.ToString()");
		}

		[Test]
		public void DoubleEpsilon_Compare ()
		{
			TestRuntime.AssertNotDevice ("Known to fail on devices, see bug #15802");
			// works on some ARM CPU (e.g. iPhone5S) but not others (iPad4 or iPodTouch5)
			Assert.That (Double.Epsilon, Is.Not.EqualTo (0f), "Epsilon");
			Assert.That (-Double.Epsilon, Is.Not.EqualTo (0f), "-Epsilon");
		}

		[Test]
		public void DoubleEpsilon_ToString ()
		{
			TestRuntime.AssertNotDevice ("Known to fail on devices, see bug #15802");
			var ci = CultureInfo.InvariantCulture;
			// note: unlike Single this works on both my iPhone5S and iPodTouch5
			Assert.That (Double.Epsilon.ToString (ci), Is.EqualTo ("5E-324"), "Epsilon.ToString()");
			Assert.That ((-Double.Epsilon).ToString (ci), Is.EqualTo ("-5E-324"), "-Epsilon.ToString()");
		}

		[Test]
		[Ignore ("Assumption broken with mono 2017-12")]
		public void AssemblyReferences_16213 ()
		{
			foreach (var assembly in typeof (System.Data.AcceptRejectRule).Assembly.GetReferencedAssemblies ()) {
				// Unlike the original bug report the unit tests uses Mono.Data.Tds so it will be part of the .app
				// so we check for System.Transactions which is not used (but referenced by the original System.Data)
				if (assembly.Name == "System.Transactions")
					Assert.Fail ("System.Transactions reference should have removed by the linker");
			}
		}

		[Test]
		public void NestedNSObject ()
		{
			// Parent type is not used - but it's not linked out
			var p = Helper.GetType ("LinkAll.Parent");
			Assert.NotNull (p, "Parent");
			// because a nested type is a subclass of NSObject (and not part of monotouch.dll)
			var n = p.GetNestedType ("Derived");
			Assert.NotNull (n, "Derived");
			// however other stuff in Parent, like unused methods, will be removed
			Assert.Null (p.GetMethod ("UnusedMethod"), "unused method");
			// while exported stuff will be present
			Assert.NotNull (n.GetMethod ("Foo"), "unused Export method");
		}

		[Test]
		public void Bug20363 ()
		{
			// testing compile time error
			CancelEventArgs cea = new CancelEventArgs ();
			Assert.NotNull (cea, "CancelEventArgs");
		}

		string GetField (object o, string s)
		{
			var type = o.GetType ();
			var f1 = type.GetField (s, BindingFlags.Instance | BindingFlags.NonPublic);
			var f2 = type.GetField (s + "i__Field", BindingFlags.Instance | BindingFlags.NonPublic);
			if (f1 is null && f2 is null)
				return s;

			//Console.WriteLine (f.GetValue (o));
			return null;
		}

		string FromPattern (string pattern, object o)
		{
			var s = GetField (o, "<action>");
			if (s is not null)
				return s;
			s = GetField (o, "<id>");
			if (s is not null)
				return s;
			return GetField (o, "<contentType>");
		}

		[Test]
		public void AnonymousType ()
		{
			var result = FromPattern ("/{action}/{id}.{contentType}", new {
				action = "foo",
				id = 1234,
				contentType = "xml"
			});
			Assert.Null (result, result);
		}

		[Test]
		public void Events ()
		{
			using (var pr = new SKProductsRequest ()) {
				Assert.Null (pr.WeakDelegate, "none");
				// event on SKProductsRequest itself
				pr.ReceivedResponse += (object sender, SKProductsRequestResponseEventArgs e) => { };

				var t = pr.WeakDelegate.GetType ();
				Assert.That (t.Name, Is.EqualTo ("_SKProductsRequestDelegate"), "delegate");

				var fi = t.GetField ("receivedResponse", BindingFlags.NonPublic | BindingFlags.Instance);
				Assert.NotNull (fi, "receivedResponse");
				var value = fi.GetValue (pr.WeakDelegate);
				Assert.NotNull (value, "value");

				// and on the SKRequest defined one
				pr.RequestFailed += (object sender, SKRequestErrorEventArgs e) => { };
				// and the existing (initial field) is still set
				fi = t.GetField ("receivedResponse", BindingFlags.NonPublic | BindingFlags.Instance);
				Assert.NotNull (fi, "receivedResponse/SKRequest");
			}
		}

		[Test]
		public void Aot_27116 ()
		{
			var nix = (from nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces ()
					   where nic.Id.StartsWith ("en") || nic.Id.StartsWith ("pdp_ip")
					   select nic);
			Assert.NotNull (nix);
		}

		[Test]
		public void AppleTls ()
		{
			// make test work for classic (monotouch) and unified (iOS, tvOS)
			var fqn = typeof (NSObject).AssemblyQualifiedName.Replace ("Foundation.NSObject", "Security.Tls.AppleTlsProvider");
			Assert.Null (Helper.GetType (fqn), "Should NOT be included (no SslStream or Socket support)");
		}

		[Test]
		// https://bugzilla.xamarin.com/show_bug.cgi?id=59247
		public void WebKit_NSProxy ()
		{
			// this test works only because "Link all" does not use WebKit
			var fqn = typeof (NSObject).AssemblyQualifiedName.Replace ("Foundation.NSObject", "Foundation.NSProxy");
			Assert.Null (Helper.GetType (fqn), fqn);
		}

		static Type type_Task = typeof (Task);

		[Test]
		public void Bug59015 ()
		{
			CheckAsyncTaskMethodBuilder (typeof (AsyncTaskMethodBuilder));
			CheckAsyncTaskMethodBuilder (typeof (AsyncTaskMethodBuilder<int>));
			var snfwc = type_Task.GetMethod ("NotifyDebuggerOfWaitCompletion", BindingFlags.Instance | BindingFlags.NonPublic);
#if DEBUG
			Assert.NotNull (snfwc, "Task.NotifyDebuggerOfWaitCompletion");
#else
			// something keeps it from being removed
			// Assert.Null (snfwc, "Task.NotifyDebuggerOfWaitCompletion");
#endif
		}

		void CheckAsyncTaskMethodBuilder (Type atmb)
		{
			Assert.NotNull (atmb, "AsyncTaskMethodBuilder");
			var snfwc = atmb.GetMethod ("SetNotificationForWaitCompletion", BindingFlags.Instance | BindingFlags.NonPublic);
			var oifd = atmb.GetProperty ("ObjectIdForDebugger", BindingFlags.Instance | BindingFlags.NonPublic);
#if DEBUG
			Assert.NotNull (snfwc, atmb.FullName + ".SetNotificationForWaitCompletion");
			Assert.NotNull (oifd,  atmb.FullName + ".ObjectIdForDebugger");
#else
			Assert.Null (snfwc, atmb.FullName + ".SetNotificationForWaitCompletion");
			Assert.Null (oifd, atmb.FullName + ".ObjectIdForDebugger");
#endif
		}

		[Test]
		[Ignore ("BUG https://github.com/dotnet/macios/issues/11280")]
		public void LinkedAwayGenericTypeAsOptionalMemberInProtocol ()
		{
			// https://github.com/dotnet/macios/issues/3523
			// This test will fail at build time if it regresses (usually these types of build tests go into monotouch-test, but monotouch-test uses NSSet<T> elsewhere, which this test requires to be linked away).
			Assert.IsNull (typeof (NSObject).Assembly.GetType (NamespacePrefix + "Foundation.NSSet`1"), "NSSet<T> must be linked away, otherwise this test is useless");
		}

		[Protocol (Name = "ProtocolWithGenericsInOptionalMember", WrapperType = typeof (ProtocolWithGenericsInOptionalMemberWrapper))]
		[ProtocolMember (IsRequired = false, IsProperty = false, IsStatic = false, Name = "ConfigureView", Selector = "configureViewForParameters:", ParameterType = new Type [] { typeof (global::Foundation.NSSet<global::Foundation.NSString>) }, ParameterByRef = new bool [] { false })]
		public interface IProtocolWithGenericsInOptionalMember : INativeObject, IDisposable { }

		internal sealed class ProtocolWithGenericsInOptionalMemberWrapper : BaseWrapper, IProtocolWithGenericsInOptionalMember {
			public ProtocolWithGenericsInOptionalMemberWrapper (IntPtr handle, bool owns) : base (handle, owns) { }
		}

		[Test]
		public void NoFatCorlib ()
		{
			var corlib = typeof (int).Assembly.Location;
			// special location when we build a shared (app and extensions) framework for mono
			if (corlib.EndsWith ("/Frameworks/Xamarin.Sdk.framework/MonoBundle/mscorlib.dll", StringComparison.Ordinal))
				Assert.Pass (corlib);

#if __MACCATALYST__ || __MACOS__
			var bundleLocation = Path.Combine ("Contents", "MonoBundle");
#else
			var bundleLocation = string.Empty;
#endif
			var bundlePath = Path.Combine (NSBundle.MainBundle.BundlePath, bundleLocation);
			var isExtension = bundlePath.EndsWith (".appex", StringComparison.Ordinal);
			var bundleName = isExtension ? "link all.appex" : "link all.app";
			const string corelib = "System.Private.CoreLib.dll";
			var suffix = Path.Combine (bundleName, bundleLocation, corelib);
			Assert.That (corlib, Does.EndWith (suffix), corlib);
		}

#if __MACOS__ || __IOS__
		[Test]
		public void CGPdfPage ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
			var pdfPath = NSBundle.MainBundle.PathForResource ("Tamarin", "pdf");
			using var view = new PdfView ();
			view.Document = new PdfDocument (NSUrl.FromFilename (pdfPath));
			using var page = view.CurrentPage;
			Assert.IsNotNull (page.Page, "Page");
		}
#endif
	}

	[SupportedOSPlatform ("macos1.0")]
	[SupportedOSPlatform ("ios1.0")]
	[SupportedOSPlatform ("tvos1.0")]
	[SupportedOSPlatform ("maccatalyst1.0")]
	[Preserve]
	public class ClassFromThePast : NSObject {
		[Export ("foo:")]
		public void Foo (ClassFromThePast obj)
		{
		}
	}
}
