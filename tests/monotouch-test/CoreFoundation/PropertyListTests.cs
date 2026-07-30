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
			Assert.That (rv.Error, Is.Null, "Error 1");
			Assert.That (rv.PropertyList, Is.Not.Null, "PropertyList 1");
			Assert.That (rv.Format, Is.EqualTo (CFPropertyListFormat.XmlFormat1), "Format 1");
			Assert.That (rv.PropertyList.IsValid (CFPropertyListFormat.BinaryFormat1), Is.True, "IsValid Binary 1");
			Assert.That (rv.PropertyList.IsValid (CFPropertyListFormat.OpenStep), Is.True, "IsValid OpenStep 1");
			Assert.That (rv.PropertyList.IsValid (CFPropertyListFormat.XmlFormat1), Is.True, "IsValid Xml 1");
		}

		[Test]
		public void Constructors ()
		{
			using (var dummy = CreateDummy ()) {
				var rc = CFGetRetainCount (dummy.Handle);
				using (var clone = Runtime.GetINativeObject<CFPropertyList> (dummy.Handle, false)) {
					Assert.That (dummy.Handle, Is.EqualTo (clone.Handle), "Handle 1");
					Assert.That (CFGetRetainCount (clone.Handle), Is.EqualTo (rc + 1), "RC 1");
				}
			}

			using (var dummy = CreateDummy ()) {
				var rc = CFGetRetainCount (dummy.Handle);
				using (var clone = Runtime.GetINativeObject<CFPropertyList> (dummy.Handle, false)) {
					Assert.That (dummy.Handle, Is.EqualTo (clone.Handle), "Handle 2");
					Assert.That (CFGetRetainCount (clone.Handle), Is.EqualTo (rc + 1), "RC 2");
				}
			}

			using (var dummy = CreateDummy ()) {
				CFRetain (dummy.Handle);
				var rc = CFGetRetainCount (dummy.Handle);
				using (var clone = Runtime.GetINativeObject<CFPropertyList> (dummy.Handle, true)) {
					Assert.That (dummy.Handle, Is.EqualTo (clone.Handle), "Handle 3");
					Assert.That (CFGetRetainCount (clone.Handle), Is.EqualTo (rc), "RC 3");
				}
			}
		}

		[Test]
		public void DeepCopy ()
		{
			using (var dummy = CreateDummy ()) {
				using (var clone = dummy.DeepCopy ()) {
					Assert.That (clone.Handle, Is.Not.EqualTo (dummy.Handle), "Handle");
					Assert.That (clone.Value.ToString (), Is.EqualTo (dummy.Value.ToString ()), "Value comparison");
				}
			}
		}

		[Test]
		public void AsData ()
		{
			using (var dummy = CreateDummy ()) {
				var data = dummy.AsData (CFPropertyListFormat.XmlFormat1);
				Assert.That (data.Error, Is.Null, "Error");
				Assert.That (data.Data, Is.Not.Null, "Data");
				Assert.That (new StreamReader (data.Data.AsStream ()).ReadToEnd (), Does.StartWith ("<?xml"), "String Value");
			}
		}

		[Test]
		public void IsValid ()
		{
			using (var dummy = CreateDummy ()) {
				Assert.That (dummy.IsValid (CFPropertyListFormat.BinaryFormat1), Is.True, "IsValid Binary 1");
				Assert.That (dummy.IsValid (CFPropertyListFormat.OpenStep), Is.True, "IsValid OpenStep 1");
				Assert.That (dummy.IsValid (CFPropertyListFormat.XmlFormat1), Is.True, "IsValid Xml 1");
			}
		}

		[Test]
		public void Value ()
		{

			using (var dummy = CreateDummy ("<array><string>SomeStringArrayValue</string></array>")) {
				var value = dummy.Value;
				Assert.That (value.GetType (), Is.EqualTo (typeof (NSMutableArray)), "Array Value Type");
				var arr = (NSArray) value;
				Assert.That (arr.Count, Is.EqualTo ((nuint) 1), "Array Count");
				Assert.That (arr.GetItem<NSString> (0).ToString (), Is.EqualTo ("SomeStringArrayValue"), "Array First Value");
			}

			using (var dummy = CreateDummy ("<data>U29tZURhdGFWYWx1ZQ==</data>")) {
				var value = dummy.Value;
				Assert.That (value.GetType (), Is.EqualTo (typeof (NSMutableData)), "Data Value Type");
				Assert.That (new StreamReader (((NSData) value).AsStream ()).ReadToEnd (), Is.EqualTo ("SomeDataValue"), "Data Value");
			}

			using (var dummy = CreateDummy ("<dict><key>SomeKey</key><string>SomeStringValue</string></dict>")) {
				var value = dummy.Value;
				Assert.That (value.GetType (), Is.EqualTo (typeof (NSMutableDictionary)), "Dictionary Value Type");
				var dict = (NSDictionary) value;
				Assert.That (dict.Count, Is.EqualTo ((nuint) 1), "Dictionary Count");
				Assert.That (dict.Keys [0].ToString (), Is.EqualTo ("SomeKey"), "Dictionary Key Value");
				Assert.That (dict ["SomeKey"].ToString (), Is.EqualTo ("SomeStringValue"), "Dictionary Entry Value");
			}

			using (var dummy = CreateDummy ("<string>SomeStringValue</string>")) {
				var value = dummy.Value;
				Assert.That (value.GetType (), Is.EqualTo (typeof (NSMutableString)), "String Value Type");
				Assert.That (((NSString) value).ToString (), Is.EqualTo ("SomeStringValue"), "String Value");
			}

			using (var dummy = CreateDummy ("<date>2018-08-01T01:00:00Z</date>")) {
				var value = dummy.Value;
				Assert.That (value.GetType (), Is.EqualTo (typeof (NSDate)), "Date Value Type");
				var date = (NSDate) value;
				Assert.That (date.SecondsSinceReferenceDate, Is.EqualTo (554778000.0), "Date Value");
			}

			using (var dummy = CreateDummy ("<integer>42</integer>")) {
				var value = dummy.Value;
				Assert.That (value.GetType (), Is.EqualTo (typeof (NSNumber)), "Int Value Type");
				Assert.That (((NSNumber) value).Int32Value, Is.EqualTo (42), "Int Value");
			}

			using (var dummy = CreateDummy ($"<integer>{long.MaxValue}</integer>")) {
				var value = dummy.Value;
				Assert.That (value.GetType (), Is.EqualTo (typeof (NSNumber)), "Long Value Type");
				Assert.That (((NSNumber) value).Int64Value, Is.EqualTo (long.MaxValue), "Long Value");
			}

			using (var dummy = CreateDummy ($"<real>3.1415926</real>")) {
				var value = dummy.Value;
				Assert.That (value.GetType (), Is.EqualTo (typeof (NSNumber)), "Real Value Type");
				Assert.That (((NSNumber) value).FloatValue, Is.EqualTo (3.1415926).Within (0.001), "Real PI Value");
			}

			using (var dummy = CreateDummy ($"<true/>")) {
				var value = dummy.Value;
				Assert.That (value.GetType (), Is.EqualTo (typeof (bool)), "Bool True Value Type");
				Assert.That ((bool) value, Is.EqualTo (true), "Bool True Value");
			}

			using (var dummy = CreateDummy ($"<false/>")) {
				var value = dummy.Value;
				Assert.That (value.GetType (), Is.EqualTo (typeof (bool)), "Bool True Value Type");
				Assert.That ((bool) value, Is.EqualTo (false), "Bool True Value");
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
			Assert.That (dummy.Error, Is.Null, "Dummy Error");
			return dummy.PropertyList;
		}
	}
}
