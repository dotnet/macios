// Copyright 2016 Xamarin Inc. All rights reserved.

namespace Linker.Sealer {

	[Preserve (AllMembers = true)]
	public class Unsealable {

		public virtual bool A () { return true; }
		public virtual bool B () { return false; }
	}

	[Preserve (AllMembers = true)]
	public class Sealable : Unsealable {
		public override bool B () { return true; }
		public virtual bool C () { return false; }
	}

	interface Interface {
		bool A ();
	}

	[Preserve (AllMembers = true)]
	public class Base {
		public bool A () { return true; }
	}

	[Preserve (AllMembers = true)]
	public class Subclass : Base, Interface {
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SealerTest {

		[SetUp]
		public void SetUp ()
		{
			// XML serialization mechanism is controlled by RuntimeFeature.IsDynamicCodeSupported
			// which will be true for simulator / JIT builds
			// so the optimization is disabled unless AOT is used
			TestRuntime.AssertDevice ();
		}

		[Test]
		public void Sealed ()
		{
			// this can not be optimized into a sealed type
			Assert.That (typeof (Unsealable).IsSealed, Is.False, "Unsealed");
#if DEBUG || __MACOS__
			// this is not a sealed type (in the source)
			Assert.That (typeof (Sealable).IsSealed, Is.False, "Sealable");
			Assert.That (typeof (Base).IsSealed, Is.False, "Base");
			Assert.That (typeof (Subclass).IsSealed, Is.False, "Subclass");
			Assert.That (typeof (Interface).IsSealed, Is.False, "Interface");
#else
			// Sealable can be optimized / sealed as nothing else is (or can) subclass it
			Assert.That (typeof (Sealable).IsSealed, Is.True, "Sealable");
			// Base is subclassed so it can't be sealed
			Assert.That (typeof (Base).IsSealed, Is.False, "Base");
			// Subclass is not subclassed anymore and can be sealed
			Assert.That (typeof (Subclass).IsSealed, Is.True, "Subclass");
			// interface can not be sealed
			Assert.That (typeof (Interface).IsSealed, Is.False, "Interface");
#endif
		}

		[Test]
		public void Final ()
		{
			var t = typeof (Sealable);
			var a = t.GetMethod ("A")!;
			var b = t.GetMethod ("B")!;
			var c = t.GetMethod ("C")!;
#if DEBUG || __MACOS__
			// this is not a sealed (C#) method (in the source)
			Assert.That (a.IsFinal, Is.False, "A");
			Assert.That (b.IsFinal, Is.False, "B");
			Assert.That (c.IsFinal, Is.False, "C");
#else
			// but it can be optimized / sealed as nothing else is (or can) overrides it
			Assert.That (a.IsFinal, Is.True, "A");
			Assert.That (b.IsFinal, Is.True, "B");
			Assert.That (c.IsFinal, Is.False, "C"); // devirtualized
#endif
		}

		[Test]
		public void Virtual ()
		{
			var t = typeof (Sealable);
			var a = t.GetMethod ("A")!;
			var b = t.GetMethod ("B")!;
			var c = t.GetMethod ("C")!;
#if DEBUG || __MACOS__
			// both methods are virtual (both in C# and IL)
			Assert.That (a.IsVirtual, Is.True, "A");
			Assert.That (b.IsVirtual, Is.True, "B");
			Assert.That (c.IsVirtual, Is.True, "C");
#else
			// calling A needs dispatch to base type Unsealable
			Assert.That (a.IsVirtual, Is.True, "A");
			// B is an override and must remain virtual
			Assert.That (b.IsVirtual, Is.True, "B");
			// C has no special requirement and can be de-virtualized
			Assert.That (c.IsVirtual, Is.False, "C");
#endif
		}

		[Test]
		public void Interface ()
		{
			var t = typeof (Subclass);
			var a = t.GetMethod ("A")!;
			// A cannot be de-virtualized since Concrete must satisfy Interface thru Base
			Assert.That (a.IsVirtual, Is.True, "A");
		}
	}
}
