#if __MACOS__
#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Security;

namespace MonoTouchFixtures.SecurityInterface {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AuthorizationRightsTest {

		[Test]
		public void AuthorizationRight_IsClass ()
		{
			Assert.That (typeof (AuthorizationRight).IsValueType, Is.False, "IsValueType");
		}

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
			var right = new AuthorizationRight ("com.example.test", new byte [] { 1, 2, 3 });
			using var rights = new AuthorizationRights (right);
			Assert.That (rights.Count, Is.EqualTo (1), "Count");
			Assert.That (rights [0].Name, Is.EqualTo ("com.example.test"), "Name");
			Assert.That (rights [0].Value, Is.EqualTo (new byte [] { 1, 2, 3 }), "Value");
		}

		[Test]
		public void Create_FromEnumerable ()
		{
			var values = new List<AuthorizationRight> {
				new AuthorizationRight ("a"),
				new AuthorizationRight ("b"),
			};
			using var rights = new AuthorizationRights (values);
			Assert.That (rights.Count, Is.EqualTo (2), "Count");
		}

		[Test]
		public void Create_Empty ()
		{
			using var rights = new AuthorizationRights ();
			Assert.That (rights.Count, Is.EqualTo (0), "Count");
			Assert.That (rights.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle should be valid even when empty");
		}

		[Test]
		public void AuthorizationRight_NullValue ()
		{
			var right = new AuthorizationRight ("com.example.novalue");
			Assert.That (right.Name, Is.EqualTo ("com.example.novalue"), "Name");
			Assert.That (right.Value, Is.Null, "Value should be null");
		}

		[Test]
		public void AuthorizationRight_ValueIsCopied ()
		{
			var original = new byte [] { 10, 20, 30 };
			var right = new AuthorizationRight ("test", original);
			original [0] = 99;
			var value = right.Value;
			Assert.That (value, Is.Not.Null, "Value");
			if (value is null)
				return;
			Assert.That (value [0], Is.EqualTo (10), "Value should be a copy, not a reference");
			var copy = right.Value;
			Assert.That (copy, Is.Not.Null, "Copy");
			if (copy is null)
				return;
			copy [0] = 88;
			Assert.That (right.Value, Is.EqualTo (new byte [] { 10, 20, 30 }), "Each Value access should return a copy");
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
		public void EmptyValue_NormalizesToNull ()
		{
			var right = new AuthorizationRight ("empty", []);
			Assert.That (right.Value, Is.Null, "Value");
		}

		[Test]
		public void NativeLayouts ()
		{
			Assert.That (Marshal.SizeOf<AuthorizationItemLayout> (), Is.EqualTo (32), "AuthorizationItem size");
			Assert.That (Marshal.OffsetOf<AuthorizationItemLayout> (nameof (AuthorizationItemLayout.Name)).ToInt32 (), Is.EqualTo (0), "AuthorizationItem.Name");
			Assert.That (Marshal.OffsetOf<AuthorizationItemLayout> (nameof (AuthorizationItemLayout.ValueLength)).ToInt32 (), Is.EqualTo (8), "AuthorizationItem.ValueLength");
			Assert.That (Marshal.OffsetOf<AuthorizationItemLayout> (nameof (AuthorizationItemLayout.Value)).ToInt32 (), Is.EqualTo (16), "AuthorizationItem.Value");
			Assert.That (Marshal.OffsetOf<AuthorizationItemLayout> (nameof (AuthorizationItemLayout.Flags)).ToInt32 (), Is.EqualTo (24), "AuthorizationItem.Flags");
			Assert.That (Marshal.SizeOf<AuthorizationRightsLayout> (), Is.EqualTo (16), "AuthorizationRights size");
		}

		[StructLayout (LayoutKind.Sequential)]
		struct AuthorizationItemLayout {
			public IntPtr Name;
			public nuint ValueLength;
			public IntPtr Value;
			public uint Flags;
		}

		[StructLayout (LayoutKind.Sequential)]
		struct AuthorizationRightsLayout {
			public uint Count;
			public IntPtr Items;
		}
	}
}
#endif // __MACOS__
