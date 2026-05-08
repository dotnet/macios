//
// Unit tests for CFNotificationCenter
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System.IO;

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CFPropertyListTests {
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static nint CFGetRetainCount (IntPtr handle);

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFRetain (IntPtr handle);

		[Test]
		public void CreateFromData ()
		{
			var plist = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
	<dict>
		<key>SomeKey</key>
		<string>SomeStringValue</string>
	</dict>
</plist>";
			var rv = CFPropertyList.FromData (NSData.FromString (plist));
			ClassicAssert.IsNull (rv.Error, "Error 1");
			ClassicAssert.IsNotNull (rv.PropertyList, "PropertyList 1");
			ClassicAssert.AreEqual (CFPropertyListFormat.XmlFormat1, rv.Format, "Format 1");
			ClassicAssert.IsTrue (rv.PropertyList.IsValid (CFPropertyListFormat.BinaryFormat1), "IsValid Binary 1");
			ClassicAssert.IsTrue (rv.PropertyList.IsValid (CFPropertyListFormat.OpenStep), "IsValid OpenStep 1");
			ClassicAssert.IsTrue (rv.PropertyList.IsValid (CFPropertyListFormat.XmlFormat1), "IsValid Xml 1");
		}

		[Test]
		public void Constructors ()
		{
			using (var dummy = CreateDummy ()) {
				var rc = CFGetRetainCount (dummy.Handle);
				using (var clone = Runtime.GetINativeObject<CFPropertyList> (dummy.Handle, false)) {
					ClassicAssert.AreEqual (clone.Handle, dummy.Handle, "Handle 1");
					ClassicAssert.AreEqual (rc + 1, CFGetRetainCount (clone.Handle), "RC 1");
				}
			}

			using (var dummy = CreateDummy ()) {
				var rc = CFGetRetainCount (dummy.Handle);
				using (var clone = Runtime.GetINativeObject<CFPropertyList> (dummy.Handle, false)) {
					ClassicAssert.AreEqual (clone.Handle, dummy.Handle, "Handle 2");
					ClassicAssert.AreEqual (rc + 1, CFGetRetainCount (clone.Handle), "RC 2");
				}
			}

			using (var dummy = CreateDummy ()) {
				CFRetain (dummy.Handle);
				var rc = CFGetRetainCount (dummy.Handle);
				using (var clone = Runtime.GetINativeObject<CFPropertyList> (dummy.Handle, true)) {
					ClassicAssert.AreEqual (clone.Handle, dummy.Handle, "Handle 3");
					ClassicAssert.AreEqual (rc, CFGetRetainCount (clone.Handle), "RC 3");
				}
			}
		}

		[Test]
		public void DeepCopy ()
		{
			using (var dummy = CreateDummy ()) {
				using (var clone = dummy.DeepCopy ()) {
					ClassicAssert.AreNotEqual (dummy.Handle, clone.Handle, "Handle");
					ClassicAssert.AreEqual (dummy.Value.ToString (), clone.Value.ToString (), "Value comparison");
				}
			}
		}

		[Test]
		public void AsData ()
		{
			using (var dummy = CreateDummy ()) {
				var data = dummy.AsData (CFPropertyListFormat.XmlFormat1);
				ClassicAssert.IsNull (data.Error, "Error");
				ClassicAssert.IsNotNull (data.Data, "Data");
				Assert.That (new StreamReader (data.Data.AsStream ()).ReadToEnd (), Does.StartWith ("<?xml"), "String Value");
			}
		}

		[Test]
		public void IsValid ()
		{
			using (var dummy = CreateDummy ()) {
				ClassicAssert.IsTrue (dummy.IsValid (CFPropertyListFormat.BinaryFormat1), "IsValid Binary 1");
				ClassicAssert.IsTrue (dummy.IsValid (CFPropertyListFormat.OpenStep), "IsValid OpenStep 1");
				ClassicAssert.IsTrue (dummy.IsValid (CFPropertyListFormat.XmlFormat1), "IsValid Xml 1");
			}
		}

		[Test]
		public void Value ()
		{

			using (var dummy = CreateDummy ("<array><string>SomeStringArrayValue</string></array>")) {
				var value = dummy.Value;
				ClassicAssert.AreEqual (typeof (NSMutableArray), value.GetType (), "Array Value Type");
				var arr = (NSArray) value;
				ClassicAssert.AreEqual ((nuint) 1, arr.Count, "Array Count");
				ClassicAssert.AreEqual ("SomeStringArrayValue", arr.GetItem<NSString> (0).ToString (), "Array First Value");
			}

			using (var dummy = CreateDummy ("<data>U29tZURhdGFWYWx1ZQ==</data>")) {
				var value = dummy.Value;
				ClassicAssert.AreEqual (typeof (NSMutableData), value.GetType (), "Data Value Type");
				ClassicAssert.AreEqual ("SomeDataValue", new StreamReader (((NSData) value).AsStream ()).ReadToEnd (), "Data Value");
			}

			using (var dummy = CreateDummy ("<dict><key>SomeKey</key><string>SomeStringValue</string></dict>")) {
				var value = dummy.Value;
				ClassicAssert.AreEqual (typeof (NSMutableDictionary), value.GetType (), "Dictionary Value Type");
				var dict = (NSDictionary) value;
				ClassicAssert.AreEqual ((nuint) 1, dict.Count, "Dictionary Count");
				ClassicAssert.AreEqual ("SomeKey", dict.Keys [0].ToString (), "Dictionary Key Value");
				ClassicAssert.AreEqual ("SomeStringValue", dict ["SomeKey"].ToString (), "Dictionary Entry Value");
			}

			using (var dummy = CreateDummy ("<string>SomeStringValue</string>")) {
				var value = dummy.Value;
				ClassicAssert.AreEqual (typeof (NSMutableString), value.GetType (), "String Value Type");
				ClassicAssert.AreEqual ("SomeStringValue", ((NSString) value).ToString (), "String Value");
			}

			using (var dummy = CreateDummy ("<date>2018-08-01T01:00:00Z</date>")) {
				var value = dummy.Value;
				ClassicAssert.AreEqual (typeof (NSDate), value.GetType (), "Date Value Type");
				var date = (NSDate) value;
				ClassicAssert.AreEqual (554778000.0, date.SecondsSinceReferenceDate, "Date Value");
			}

			using (var dummy = CreateDummy ("<integer>42</integer>")) {
				var value = dummy.Value;
				ClassicAssert.AreEqual (typeof (NSNumber), value.GetType (), "Int Value Type");
				ClassicAssert.AreEqual (42, ((NSNumber) value).Int32Value, "Int Value");
			}

			using (var dummy = CreateDummy ($"<integer>{long.MaxValue}</integer>")) {
				var value = dummy.Value;
				ClassicAssert.AreEqual (typeof (NSNumber), value.GetType (), "Long Value Type");
				ClassicAssert.AreEqual (long.MaxValue, ((NSNumber) value).Int64Value, "Long Value");
			}

			using (var dummy = CreateDummy ($"<real>3.1415926</real>")) {
				var value = dummy.Value;
				ClassicAssert.AreEqual (typeof (NSNumber), value.GetType (), "Real Value Type");
				ClassicAssert.AreEqual (3.1415926, ((NSNumber) value).FloatValue, 0.001, "Real PI Value");
			}

			using (var dummy = CreateDummy ($"<true/>")) {
				var value = dummy.Value;
				ClassicAssert.AreEqual (typeof (bool), value.GetType (), "Bool True Value Type");
				ClassicAssert.AreEqual (true, (bool) value, "Bool True Value");
			}

			using (var dummy = CreateDummy ($"<false/>")) {
				var value = dummy.Value;
				ClassicAssert.AreEqual (typeof (bool), value.GetType (), "Bool True Value Type");
				ClassicAssert.AreEqual (false, (bool) value, "Bool True Value");
			}
		}

		CFPropertyList CreateDummy (string data = "<dict><key>SomeKey</key><string>SomeStringValue</string></dict>")
		{
			var plist = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
	{data}
</plist>";
			var dummy = CFPropertyList.FromData (NSData.FromString (plist));
			ClassicAssert.IsNull (dummy.Error, "Dummy Error");
			return dummy.PropertyList;
		}
	}
}
