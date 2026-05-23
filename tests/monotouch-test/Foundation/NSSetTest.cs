
namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSSetTest {
		[Test]
		public void SetCtors ()
		{
			// The NSSet (params object [] args)
			var s = new NSSet (1);
			Assert.That (s.Count, Is.EqualTo ((nuint) 1));
			s = new NSSet (1, 2, 3);
			Assert.That (s.Count, Is.EqualTo ((nuint) 3));

			// The NSSet (params [] NSObject args)
			var objs = new NSObject [5];
			for (int i = 0; i < objs.Length; i++)
				objs [i] = new NSNumber (i);

			s = new NSSet (objs [0], objs [1], objs [2], objs [3], objs [4]);
			Assert.That (s.Count, Is.EqualTo ((nuint) 5));

			// Repeat the values
			s = new NSSet (objs [0], objs [1], objs [2], objs [0], objs [1]);
			Assert.That (s.Count, Is.EqualTo ((nuint) 3));
		}

		[Test]
		public void OperatorPlus ()
		{
			var one = new NSSet (1, 2, 3);
			var two = new NSSet (4, 5, 6);
			var sum = one + two;
			Assert.That (sum.Count, Is.EqualTo ((nuint) 6));

			var objs = new NSObject [5];
			for (int i = 0; i < objs.Length; i++)
				objs [i] = new NSNumber (i * 100);

			sum = new NSSet (objs) + one + two;
			Assert.That (sum.Count, Is.EqualTo ((nuint) 11));
			sum = new NSSet (objs) + new NSSet (objs);
			Assert.That (sum.Count, Is.EqualTo ((nuint) 5));

			Assert.That ((one + one).Count, Is.EqualTo ((nuint) 3));
			var sub = one - one;
			Assert.That (sub.Count, Is.EqualTo ((nuint) 0));

			var three = new NSSet (1, 2, 3, 4, 5, 6);
			var subt = three - two;
			Assert.That (subt.Count, Is.EqualTo ((nuint) 3));
			Assert.That (three.Contains (1), Is.True);
			Assert.That (three.Contains (2), Is.True);
			Assert.That (three.Contains (3), Is.True);
			subt = three - one;
			Assert.That (subt.Count, Is.EqualTo ((nuint) 3));
			Assert.That (three.Contains (4), Is.True);
			Assert.That (three.Contains (5), Is.True);
			Assert.That (three.Contains (6), Is.True);

		}

		[Test]
		public void OperatorPlusReferenceTest ()
		{
			var one = new NSSet (1, 2, 3);
			var two = new NSSet (4, 5, 6);
			NSSet nil = null;
			using (var sum = one + nil)
			using (var sum2 = two + one)
			using (var sum3 = one + two) {

			}
			Assert.That (one.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
			Assert.That (two.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle must be != IntPtr.Zero");
		}

		[Test]
		public void OperatorAddTest ()
		{
			var str1 = "1";
			var str2 = "2";
			var str3 = "3";

			using (var set1 = new NSSet (str1))
			using (var set2 = new NSOrderedSet (str2, str3))
			using (var result = set1 + set2) {
				Assert.That (result.Count, Is.EqualTo ((nuint) 3), "AddTest Count");
				Assert.That (result.Contains (str1), Is.True, "AddTest Contains 1");
				Assert.That (result.Contains (str2), Is.True, "AddTest Contains 2");
				Assert.That (result.Contains (str3), Is.True, "AddTest Contains 3");
			}
		}

		[Test]
		public void OperatorAddTest2 ()
		{
			var str1 = "1";
			var str2 = "2";
			var str3 = "3";

			using (var set1 = new NSSet (str1))
			using (var set2 = new NSMutableOrderedSet (str2, str3))
			using (var result = set1 + set2) {
				Assert.That (result.Count, Is.EqualTo ((nuint) 3), "AddTest Count");
				Assert.That (result.Contains (str1), Is.True, "AddTest Contains 1");
				Assert.That (result.Contains (str2), Is.True, "AddTest Contains 2");
				Assert.That (result.Contains (str3), Is.True, "AddTest Contains 3");
			}
		}

		[Test]
		public void OperatorSubtractTest ()
		{
			var str1 = "1";
			var str2 = "2";
			var str3 = "3";
			var str4 = "4";

			using (var first = new NSSet (str1, str2, str3, str4))
			using (var second = new NSOrderedSet (str3, str4))
			using (var third = first - second) {

				Assert.That (third.Count, Is.EqualTo ((nuint) 2), "OperatorSubtract Count");
				Assert.That (third.Contains (str1), Is.True, "OperatorSubtract 1");
				Assert.That (third.Contains (str2), Is.True, "OperatorSubtract 2");
				Assert.That (third.Contains (str3), Is.False, "OperatorSubtract 3");
				Assert.That (third.Contains (str4), Is.False, "OperatorSubtract 4");
			}
		}

		[Test]
		public void OperatorSubtractTest2 ()
		{
			var str1 = "1";
			var str2 = "2";
			var str3 = "3";
			var str4 = "4";

			using (var first = new NSSet (str1, str2, str3, str4))
			using (var second = new NSMutableOrderedSet (str3, str4))
			using (var third = first - second) {

				Assert.That (third.Count, Is.EqualTo ((nuint) 2), "OperatorSubtract Count");
				Assert.That (third.Contains (str1), Is.True, "OperatorSubtract 1");
				Assert.That (third.Contains (str2), Is.True, "OperatorSubtract 2");
				Assert.That (third.Contains (str3), Is.False, "OperatorSubtract 3");
				Assert.That (third.Contains (str4), Is.False, "OperatorSubtract 4");
			}
		}
	}
}
