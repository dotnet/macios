#if __MACOS__
using System;
using NUnit.Framework;
using SecurityInterface;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AuthorizationRightsTest {

		[Test]
		public void Create_FromStrings ()
		{
			using var rights = new AuthorizationRights ("com.example.right1", "com.example.right2");
			Assert.That (rights.Count, Is.EqualTo (2), "Count");
			Assert.That (rights [0].Name, Is.EqualTo ("com.example.right1"), "Name[0]");
			Assert.That (rights [1].Name, Is.EqualTo ("com.example.right2"), "Name[1]");
			Assert.That (rights.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
		}

		[Test]
		public void Create_FromAuthorizationRight ()
		{
			var right = new AuthorizationRight ("com.example.test", new byte [] { 1, 2, 3 }, 42);
			using var rights = new AuthorizationRights (right);
			Assert.That (rights.Count, Is.EqualTo (1), "Count");
			Assert.That (rights [0].Name, Is.EqualTo ("com.example.test"), "Name");
			Assert.That (rights [0].Value, Is.EqualTo (new byte [] { 1, 2, 3 }), "Value");
			Assert.That (rights [0].Flags, Is.EqualTo (42u), "Flags");
		}

		[Test]
		public void Create_Empty ()
		{
			using var rights = new AuthorizationRights (Array.Empty<string> ());
			Assert.That (rights.Count, Is.EqualTo (0), "Count");
			Assert.That (rights.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle should be valid even when empty");
		}

		[Test]
		public void AuthorizationRight_NullValue ()
		{
			var right = new AuthorizationRight ("com.example.novalue");
			Assert.That (right.Name, Is.EqualTo ("com.example.novalue"), "Name");
			Assert.That (right.Value, Is.Null, "Value should be null");
			Assert.That (right.Flags, Is.EqualTo (0u), "Flags should default to 0");
		}

		[Test]
		public void AuthorizationRight_ValueIsCopied ()
		{
			var original = new byte [] { 10, 20, 30 };
			var right = new AuthorizationRight ("test", original);
			original [0] = 99;
			Assert.That (right.Value! [0], Is.EqualTo (10), "Value should be a copy, not a reference");
		}

		[Test]
		public void AuthorizationRight_NullName_Throws ()
		{
			Assert.Throws<ArgumentNullException> (() => new AuthorizationRight (null!));
		}

		[Test]
		public void Dispose_ClearsHandle ()
		{
			var rights = new AuthorizationRights ("test");
			Assert.That (rights.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle before dispose");
			rights.Dispose ();
			Assert.That ((IntPtr) rights.Handle, Is.EqualTo (IntPtr.Zero), "Handle after dispose");
		}

		[Test]
		public void Dispose_CanBeCalledMultipleTimes ()
		{
			var rights = new AuthorizationRights ("test");
			rights.Dispose ();
			Assert.DoesNotThrow (() => rights.Dispose (), "Double dispose should not throw");
		}

		[Test]
		public void Enumeration ()
		{
			using var rights = new AuthorizationRights ("a", "b", "c");
			var names = new global::System.Collections.Generic.List<string> ();
			foreach (var right in rights)
				names.Add (right.Name);
			Assert.That (names, Is.EqualTo (new [] { "a", "b", "c" }), "Enumeration");
		}

		[Test]
		public void NullStrings_Throws ()
		{
			Assert.Throws<ArgumentNullException> (() => new AuthorizationRights ((string []) null!));
		}
	}
}
#endif // __MACOS__
