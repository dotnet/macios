
namespace MonoTouchFixtures.Foundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSKeyedUnarchiverTest {
		[Test]
		public void GetUnarchivedObject_TypeWrappers ()
		{
			TestRuntime.AssertXcodeVersion (10, 0);

			NSDictionary<NSString, NSString> testValues = new NSDictionary<NSString, NSString> ((NSString) "1", (NSString) "a");
			NSData data = NSKeyedArchiver.GetArchivedData (testValues, true, out NSError error);
			ClassicAssert.IsNull (error);

			Type dictionaryType = typeof (NSDictionary<NSString, NSString>);
			Class dictionaryClass = new Class (dictionaryType);
			NSObject o = NSKeyedUnarchiver.GetUnarchivedObject (dictionaryClass, data, out error);
			ClassicAssert.IsNotNull (o);
			ClassicAssert.IsNull (error, "GetUnarchivedObject - Class");

			o = NSKeyedUnarchiver.GetUnarchivedObject (new NSSet<Class> (new Class [] { dictionaryClass }), data, out error);
			ClassicAssert.IsNotNull (o);
			ClassicAssert.IsNull (error, "GetUnarchivedObject - NSSet<Class>");

			o = NSKeyedUnarchiver.GetUnarchivedObject (dictionaryType, data, out error);
			ClassicAssert.IsNotNull (o);
			ClassicAssert.IsNull (error, "GetUnarchivedObject - Type");

			o = NSKeyedUnarchiver.GetUnarchivedObject (new Type [] { dictionaryType }, data, out error);
			ClassicAssert.IsNotNull (o);
			ClassicAssert.IsNull (error, "GetUnarchivedObject - Type []");
		}

		[Test]
		public void DataTransformer_AllowedTopLevelTypes_WrapperTests ()
		{
			TestRuntime.AssertXcodeVersion (10, 0);

			Class [] classes = NSSecureUnarchiveFromDataTransformer.AllowedTopLevelClasses;
			Type [] types = NSSecureUnarchiveFromDataTransformer.AllowedTopLevelTypes;

			ClassicAssert.AreEqual (classes.Length, types.Length, "Lengths not equal");
		}
	}
}
