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
			ClassicAssert.False (typeof (Unsealable).IsSealed, "Unsealed");
#if DEBUG || __MACOS__
			// this is not a sealed type (in the source)
			ClassicAssert.False (typeof (Sealable).IsSealed, "Sealable");
			ClassicAssert.False (typeof (Base).IsSealed, "Base");
			ClassicAssert.False (typeof (Subclass).IsSealed, "Subclass");
			ClassicAssert.False (typeof (Interface).IsSealed, "Interface");
#else
			// Sealable can be optimized / sealed as nothing else is (or can) subclass it
			ClassicAssert.True (typeof (Sealable).IsSealed, "Sealable");
			// Base is subclassed so it can't be sealed
			ClassicAssert.False (typeof (Base).IsSealed, "Base");
			// Subclass is not subclassed anymore and can be sealed
			ClassicAssert.True (typeof (Subclass).IsSealed, "Subclass");
			// interface can not be sealed
			ClassicAssert.False (typeof (Interface).IsSealed, "Interface");
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
			ClassicAssert.False (a.IsFinal, "A");
			ClassicAssert.False (b.IsFinal, "B");
			ClassicAssert.False (c.IsFinal, "C");
#else
			// but it can be optimized / sealed as nothing else is (or can) overrides it
			ClassicAssert.True (a.IsFinal, "A");
			ClassicAssert.True (b.IsFinal, "B");
			ClassicAssert.False (c.IsFinal, "C"); // devirtualized
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
			ClassicAssert.True (a.IsVirtual, "A");
			ClassicAssert.True (b.IsVirtual, "B");
			ClassicAssert.True (c.IsVirtual, "C");
#else
			// calling A needs dispatch to base type Unsealable
			ClassicAssert.True (a.IsVirtual, "A");
			// B is an override and must remain virtual
			ClassicAssert.True (b.IsVirtual, "B");
			// C has no special requirement and can be de-virtualized
			ClassicAssert.False (c.IsVirtual, "C");
#endif
		}

		[Test]
		public void Interface ()
		{
			var t = typeof (Subclass);
			var a = t.GetMethod ("A")!;
			// A cannot be de-virtualized since Concrete must satisfy Interface thru Base
			ClassicAssert.True (a.IsVirtual, "A");
		}
	}
}
