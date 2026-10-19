// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text;

namespace MonoTouchFixtures.ObjCRuntime {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SelectorTest {
		[Test]
		public void GetHandleUtf8Literal ()
		{
			Assert.That (Selector.GetHandle ("mySelector:"u8), Is.EqualTo (Selector.GetHandle ("mySelector:")), "Literal");
			Assert.That (Selector.GetHandle ("s\u00e9lector:"u8), Is.EqualTo (Selector.GetHandle ("s\u00e9lector:")), "UTF-8");
		}

		[Test]
		public void GetHandleEmptySpan ()
		{
			var expected = Selector.GetHandle ("");
			Assert.That (Selector.GetHandle (ReadOnlySpan<byte>.Empty), Is.EqualTo (expected), "Empty");
			Assert.That (Selector.GetHandle (default (ReadOnlySpan<byte>)), Is.EqualTo (expected), "Default");
			Assert.That (Selector.GetHandle ("\0"u8), Is.EqualTo (expected), "NUL");
		}

		[Test]
		public void GetHandleSpanSlice ()
		{
			var bytes = Encoding.UTF8.GetBytes ("prefix:mySelector:suffix");
			Assert.That (Selector.GetHandle (bytes.AsSpan (7, 11)), Is.EqualTo (Selector.GetHandle ("mySelector:")), "Slice");
		}

		[Test]
		public void GetHandleNullTerminatedSpan ()
		{
			var bytes = Encoding.UTF8.GetBytes ("mySelector:\0suffix");
			var expected = Selector.GetHandle ("mySelector:");
			Assert.That (Selector.GetHandle (bytes.AsSpan (0, 12)), Is.EqualTo (expected), "Terminated");
			Assert.That (Selector.GetHandle (bytes), Is.EqualTo (expected), "Embedded NUL");
		}

		[TestCase (254)]
		[TestCase (255)]
		[TestCase (256)]
		[TestCase (8192)]
		public void GetHandleSpanLength (int length)
		{
			var name = new string ('a', length);
			var bytes = Encoding.UTF8.GetBytes (name);
			Assert.That (Selector.GetHandle (bytes), Is.EqualTo (Selector.GetHandle (name)), "Handle");
		}
	}
}
