using System.Collections.Generic;
using System.Linq;

using Foundation;

using NUnit.Framework;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSCollectionLinqTest {

		// ----- helpers -----

		static NSString S (string s) => (NSString) s;

		static NSSet<NSString> MakeSet (params string [] items)
		{
			return new NSSet<NSString> (items.Select (S).ToArray ());
		}

		static NSMutableSet<NSString> MakeMutableSet (params string [] items)
		{
			return new NSMutableSet<NSString> (items.Select (S).ToArray ());
		}

		static NSArray<NSString> MakeArray (params string [] items)
		{
			return NSArray<NSString>.FromNSObjects (items.Select (S).ToArray ());
		}

		static NSMutableArray<NSString> MakeMutableArray (params string [] items)
		{
			var arr = new NSMutableArray<NSString> ();
			foreach (var s in items)
				arr.Add (S (s));
			return arr;
		}

		static NSOrderedSet<NSString> MakeOrderedSet (params string [] items)
		{
			return new NSOrderedSet<NSString> (items.Select (S).ToArray ());
		}

		static NSMutableOrderedSet<NSString> MakeMutableOrderedSet (params string [] items)
		{
			var set = new NSMutableOrderedSet<NSString> ();
			foreach (var s in items)
				set.Add (S (s));
			return set;
		}

		// ===== NSSet<T> =====

		[Test]
		public void NSSet_First ()
		{
			using var set = MakeSet ("a");
			var first = set.First ();
			Assert.That (first.ToString (), Is.EqualTo ("a"), "First");
		}

		[Test]
		public void NSSet_First_Predicate ()
		{
			using var set = MakeSet ("a", "b", "c");
			var found = set.First (s => s.ToString () == "b");
			Assert.That (found.ToString (), Is.EqualTo ("b"), "First predicate");
		}

		[Test]
		public void NSSet_First_Empty_Throws ()
		{
			using var set = MakeSet ();
			Assert.Throws<InvalidOperationException> (() => set.First ());
		}

		[Test]
		public void NSSet_FirstOrDefault_Empty ()
		{
			using var set = MakeSet ();
			Assert.That (set.FirstOrDefault (), Is.Null, "FirstOrDefault empty");
		}

		[Test]
		public void NSSet_FirstOrDefault ()
		{
			using var set = MakeSet ("x");
			Assert.That (set.FirstOrDefault (), Is.Not.Null, "FirstOrDefault");
		}

		[Test]
		public void NSSet_FirstOrDefault_Predicate_NoMatch ()
		{
			using var set = MakeSet ("a", "b");
			Assert.That (set.FirstOrDefault (s => s.ToString () == "z"), Is.Null, "no match");
		}

		[Test]
		public void NSSet_Last ()
		{
			using var set = MakeSet ("a");
			var last = set.Last ();
			Assert.That (last.ToString (), Is.EqualTo ("a"), "Last");
		}

		[Test]
		public void NSSet_Last_Predicate ()
		{
			using var set = MakeSet ("a", "b", "c");
			var found = set.Last (s => s.ToString () != "c");
			Assert.That (found, Is.Not.Null, "Last predicate not null");
		}

		[Test]
		public void NSSet_Last_Empty_Throws ()
		{
			using var set = MakeSet ();
			Assert.Throws<InvalidOperationException> (() => set.Last ());
		}

		[Test]
		public void NSSet_LastOrDefault_Empty ()
		{
			using var set = MakeSet ();
			Assert.That (set.LastOrDefault (), Is.Null, "LastOrDefault empty");
		}

		[Test]
		public void NSSet_LastOrDefault_Predicate_NoMatch ()
		{
			using var set = MakeSet ("a");
			Assert.That (set.LastOrDefault (s => s.ToString () == "z"), Is.Null, "LastOrDefault no match");
		}

		[Test]
		public void NSSet_Single ()
		{
			using var set = MakeSet ("only");
			Assert.That (set.Single ().ToString (), Is.EqualTo ("only"), "Single");
		}

		[Test]
		public void NSSet_Single_Throws_OnMultiple ()
		{
			using var set = MakeSet ("a", "b");
			Assert.Throws<InvalidOperationException> (() => set.Single ());
		}

		[Test]
		public void NSSet_SingleOrDefault_Empty ()
		{
			using var set = MakeSet ();
			Assert.That (set.SingleOrDefault (), Is.Null, "SingleOrDefault empty");
		}

		[Test]
		public void NSSet_SingleOrDefault_Predicate ()
		{
			using var set = MakeSet ("a", "b");
			Assert.That (set.SingleOrDefault (s => s.ToString () == "a")?.ToString (), Is.EqualTo ("a"), "SingleOrDefault predicate");
		}

		[Test]
		public void NSSet_Any_Empty ()
		{
			using var set = MakeSet ();
			Assert.That (set.Any (), Is.False, "Any empty");
		}

		[Test]
		public void NSSet_Any_NonEmpty ()
		{
			using var set = MakeSet ("a");
			Assert.That (set.Any (), Is.True, "Any non-empty");
		}

		[Test]
		public void NSSet_Any_Predicate ()
		{
			using var set = MakeSet ("a", "b");
			Assert.That (set.Any (s => s.ToString () == "b"), Is.True, "Any predicate true");
			Assert.That (set.Any (s => s.ToString () == "z"), Is.False, "Any predicate false");
		}

		[Test]
		public void NSSet_All ()
		{
			using var set = MakeSet ("ab", "abc");
			Assert.That (set.All (s => s.Length > 1), Is.True, "All true");
			Assert.That (set.All (s => s.Length > 2), Is.False, "All false");
		}

		[Test]
		public void NSSet_Count ()
		{
			using var set = MakeSet ("a", "b", "c");
			Assert.That (set.Count (), Is.EqualTo (3), "Count");
			Assert.That (set.Count (s => s.ToString () == "a"), Is.EqualTo (1), "Count predicate");
		}

		[Test]
		public void NSSet_LongCount ()
		{
			using var set = MakeSet ("a", "b");
			Assert.That (set.LongCount (), Is.EqualTo (2L), "LongCount");
			Assert.That (set.LongCount (s => s.ToString () == "a"), Is.EqualTo (1L), "LongCount predicate");
		}

		[Test]
		public void NSSet_Where ()
		{
			using var set = MakeSet ("a", "bb", "ccc");
			var result = set.Where (s => s.Length > 1).ToList ();
			Assert.That (result.Count, Is.EqualTo (2), "Where count");
		}

		[Test]
		public void NSSet_Where_Index ()
		{
			using var arr = MakeOrderedSet ("a", "b", "c");
			var result = arr.Where ((s, i) => i % 2 == 0).ToList ();
			Assert.That (result.Count, Is.EqualTo (2), "Where index count");
		}

		[Test]
		public void NSSet_Select ()
		{
			using var set = MakeSet ("hello");
			var lengths = set.Select (s => s.Length).ToList ();
			Assert.That (lengths, Contains.Item (5), "Select length");
		}

		[Test]
		public void NSSet_Select_Index ()
		{
			using var arr = MakeOrderedSet ("a", "b", "c");
			var indexed = arr.Select ((s, i) => $"{i}:{s}").ToList ();
			Assert.That (indexed [0], Is.EqualTo ("0:a"), "Select index 0");
			Assert.That (indexed [1], Is.EqualTo ("1:b"), "Select index 1");
		}

		[Test]
		public void NSSet_OrderBy ()
		{
			using var set = MakeSet ("banana", "apple", "cherry");
			var sorted = set.OrderBy (s => s.ToString ()).ToList ();
			Assert.That (sorted [0].ToString (), Is.EqualTo ("apple"), "OrderBy first");
			Assert.That (sorted [2].ToString (), Is.EqualTo ("cherry"), "OrderBy last");
		}

		[Test]
		public void NSSet_OrderByDescending ()
		{
			using var set = MakeSet ("banana", "apple", "cherry");
			var sorted = set.OrderByDescending (s => s.ToString ()).ToList ();
			Assert.That (sorted [0].ToString (), Is.EqualTo ("cherry"), "OrderByDesc first");
		}

		[Test]
		public void NSSet_Distinct ()
		{
			using var set = MakeSet ("a", "b", "b");
			// NSSet already ensures uniqueness, but verify Distinct works
			var distinct = set.Distinct ().ToList ();
			Assert.That (distinct.Count, Is.EqualTo (2), "Distinct count");
		}

		[Test]
		public void NSSet_Reverse ()
		{
			using var arr = MakeOrderedSet ("a", "b", "c");
			var reversed = arr.Reverse ().ToList ();
			Assert.That (reversed [0].ToString (), Is.EqualTo ("c"), "Reverse first");
			Assert.That (reversed [2].ToString (), Is.EqualTo ("a"), "Reverse last");
		}

		[Test]
		public void NSSet_Concat ()
		{
			using var set = MakeSet ("a", "b");
			var extra = new [] { S ("c"), S ("d") };
			var all = set.Concat (extra).ToList ();
			Assert.That (all.Count, Is.EqualTo (4), "Concat count");
		}

		[Test]
		public void NSSet_Skip_Take ()
		{
			using var arr = MakeOrderedSet ("a", "b", "c", "d");
			var sliced = arr.Skip (1).Take (2).ToList ();
			Assert.That (sliced.Count, Is.EqualTo (2), "Skip/Take count");
			Assert.That (sliced [0].ToString (), Is.EqualTo ("b"), "Skip/Take first");
		}

		[Test]
		public void NSSet_SkipWhile_TakeWhile ()
		{
			using var arr = MakeOrderedSet ("a", "b", "c");
			var skipped = arr.SkipWhile (s => s.ToString () == "a").ToList ();
			Assert.That (skipped.Count, Is.EqualTo (2), "SkipWhile count");

			var taken = arr.TakeWhile (s => s.ToString () != "c").ToList ();
			Assert.That (taken.Count, Is.EqualTo (2), "TakeWhile count");
		}

		[Test]
		public void NSSet_ElementAt ()
		{
			using var arr = MakeOrderedSet ("a", "b", "c");
			Assert.That (arr.ElementAt (1).ToString (), Is.EqualTo ("b"), "ElementAt");
		}

		[Test]
		public void NSSet_ElementAtOrDefault ()
		{
			using var arr = MakeOrderedSet ("a", "b");
			Assert.That (arr.ElementAtOrDefault (0), Is.Not.Null, "ElementAtOrDefault in range");
			Assert.That (arr.ElementAtOrDefault (99), Is.Null, "ElementAtOrDefault out of range");
		}

		[Test]
		public void NSSet_ToList ()
		{
			using var set = MakeSet ("a", "b");
			var list = set.ToList ();
			Assert.That (list, Is.TypeOf<List<NSString>> (), "ToList type");
			Assert.That (list.Count, Is.EqualTo (2), "ToList count");
		}

		[Test]
		public void NSSet_ToArray ()
		{
			using var set = MakeSet ("a", "b");
			var arr = set.ToArray ();
			Assert.That (arr, Is.TypeOf<NSString []> (), "ToArray type");
			Assert.That (arr.Length, Is.EqualTo (2), "ToArray length");
		}

		[Test]
		public void NSSet_Aggregate ()
		{
			using var arr = MakeOrderedSet ("a", "b", "c");
			var result = arr.Aggregate ((acc, s) => (NSString) (acc.ToString () + s.ToString ()));
			Assert.That (result.ToString (), Is.EqualTo ("abc"), "Aggregate");
		}

		[Test]
		public void NSSet_Aggregate_Seed ()
		{
			using var arr = MakeOrderedSet ("a", "b", "c");
			var result = arr.Aggregate ("", (acc, s) => acc + s.ToString ());
			Assert.That (result, Is.EqualTo ("abc"), "Aggregate seed");
		}

		// ===== NSMutableSet<T> =====

		[Test]
		public void NSMutableSet_First ()
		{
			using var set = MakeMutableSet ("a");
			Assert.That (set.First ().ToString (), Is.EqualTo ("a"), "First");
		}

		[Test]
		public void NSMutableSet_FirstOrDefault_Empty ()
		{
			using var set = MakeMutableSet ();
			Assert.That (set.FirstOrDefault (), Is.Null, "FirstOrDefault empty");
		}

		[Test]
		public void NSMutableSet_Last ()
		{
			using var set = MakeMutableSet ("a");
			Assert.That (set.Last ().ToString (), Is.EqualTo ("a"), "Last");
		}

		[Test]
		public void NSMutableSet_LastOrDefault_Empty ()
		{
			using var set = MakeMutableSet ();
			Assert.That (set.LastOrDefault (), Is.Null, "LastOrDefault empty");
		}

		[Test]
		public void NSMutableSet_Single ()
		{
			using var set = MakeMutableSet ("only");
			Assert.That (set.Single ().ToString (), Is.EqualTo ("only"), "Single");
		}

		[Test]
		public void NSMutableSet_SingleOrDefault_Empty ()
		{
			using var set = MakeMutableSet ();
			Assert.That (set.SingleOrDefault (), Is.Null, "SingleOrDefault empty");
		}

		[Test]
		public void NSMutableSet_Any ()
		{
			using var set = MakeMutableSet ();
			Assert.That (set.Any (), Is.False, "Any empty");
			using var set2 = MakeMutableSet ("a");
			Assert.That (set2.Any (), Is.True, "Any non-empty");
		}

		[Test]
		public void NSMutableSet_All ()
		{
			using var set = MakeMutableSet ("ab", "abc");
			Assert.That (set.All (s => s.Length > 1), Is.True, "All true");
		}

		[Test]
		public void NSMutableSet_Count ()
		{
			using var set = MakeMutableSet ("a", "b", "c");
			Assert.That (set.Count (), Is.EqualTo (3), "Count");
		}

		[Test]
		public void NSMutableSet_LongCount ()
		{
			using var set = MakeMutableSet ("a", "b");
			Assert.That (set.LongCount (), Is.EqualTo (2L), "LongCount");
		}

		[Test]
		public void NSMutableSet_Where ()
		{
			using var set = MakeMutableSet ("a", "bb", "ccc");
			var result = set.Where (s => s.Length > 1).ToList ();
			Assert.That (result.Count, Is.EqualTo (2), "Where count");
		}

		[Test]
		public void NSMutableSet_Select ()
		{
			using var set = MakeMutableSet ("hello");
			var lengths = set.Select (s => s.Length).ToList ();
			Assert.That (lengths, Contains.Item (5), "Select length");
		}

		[Test]
		public void NSMutableSet_OrderBy ()
		{
			using var set = MakeMutableSet ("banana", "apple", "cherry");
			var sorted = set.OrderBy (s => s.ToString ()).ToList ();
			Assert.That (sorted [0].ToString (), Is.EqualTo ("apple"), "OrderBy first");
		}

		[Test]
		public void NSMutableSet_Skip_Take ()
		{
			using var arr = MakeOrderedSet ("a", "b", "c", "d");
			var sliced = arr.Skip (1).Take (2).ToList ();
			Assert.That (sliced.Count, Is.EqualTo (2), "Skip/Take count");
		}

		[Test]
		public void NSMutableSet_ToList ()
		{
			using var set = MakeMutableSet ("a", "b");
			var list = set.ToList ();
			Assert.That (list.Count, Is.EqualTo (2), "ToList count");
		}

		[Test]
		public void NSMutableSet_ToArray ()
		{
			using var set = MakeMutableSet ("a", "b");
			var arr = set.ToArray ();
			Assert.That (arr.Length, Is.EqualTo (2), "ToArray length");
		}

		[Test]
		public void NSMutableSet_Aggregate ()
		{
			using var set = MakeMutableSet ("a");
			var result = set.Aggregate ((acc, s) => (NSString) (acc.ToString () + s.ToString ()));
			Assert.That (result.ToString (), Is.EqualTo ("a"), "Aggregate");
		}

		// ===== NSArray<T> =====

		[Test]
		public void NSArray_First ()
		{
			using var arr = MakeArray ("a", "b", "c");
			Assert.That (arr.First ().ToString (), Is.EqualTo ("a"), "First");
		}

		[Test]
		public void NSArray_First_Predicate ()
		{
			using var arr = MakeArray ("a", "b", "c");
			Assert.That (arr.First (s => s.ToString () == "b").ToString (), Is.EqualTo ("b"), "First predicate");
		}

		[Test]
		public void NSArray_First_Empty_Throws ()
		{
			using var arr = MakeArray ();
			Assert.Throws<InvalidOperationException> (() => arr.First ());
		}

		[Test]
		public void NSArray_FirstOrDefault_Empty ()
		{
			using var arr = MakeArray ();
			Assert.That (arr.FirstOrDefault (), Is.Null, "FirstOrDefault empty");
		}

		[Test]
		public void NSArray_FirstOrDefault ()
		{
			using var arr = MakeArray ("x", "y");
			Assert.That (arr.FirstOrDefault ()?.ToString (), Is.EqualTo ("x"), "FirstOrDefault");
		}

		[Test]
		public void NSArray_FirstOrDefault_Predicate ()
		{
			using var arr = MakeArray ("a", "b", "c");
			Assert.That (arr.FirstOrDefault (s => s.ToString () == "b")?.ToString (), Is.EqualTo ("b"), "FirstOrDefault predicate match");
			Assert.That (arr.FirstOrDefault (s => s.ToString () == "z"), Is.Null, "FirstOrDefault predicate no match");
		}

		[Test]
		public void NSArray_Last ()
		{
			using var arr = MakeArray ("a", "b", "c");
			Assert.That (arr.Last ().ToString (), Is.EqualTo ("c"), "Last");
		}

		[Test]
		public void NSArray_Last_Predicate ()
		{
			using var arr = MakeArray ("a", "b", "c");
			Assert.That (arr.Last (s => s.ToString () != "c").ToString (), Is.EqualTo ("b"), "Last predicate");
		}

		[Test]
		public void NSArray_Last_Empty_Throws ()
		{
			using var arr = MakeArray ();
			Assert.Throws<InvalidOperationException> (() => arr.Last ());
		}

		[Test]
		public void NSArray_LastOrDefault_Empty ()
		{
			using var arr = MakeArray ();
			Assert.That (arr.LastOrDefault (), Is.Null, "LastOrDefault empty");
		}

		[Test]
		public void NSArray_LastOrDefault_Predicate_NoMatch ()
		{
			using var arr = MakeArray ("a", "b");
			Assert.That (arr.LastOrDefault (s => s.ToString () == "z"), Is.Null, "LastOrDefault no match");
		}

		[Test]
		public void NSArray_Single ()
		{
			using var arr = MakeArray ("only");
			Assert.That (arr.Single ().ToString (), Is.EqualTo ("only"), "Single");
		}

		[Test]
		public void NSArray_Single_Throws_OnEmpty ()
		{
			using var arr = MakeArray ();
			Assert.Throws<InvalidOperationException> (() => arr.Single ());
		}

		[Test]
		public void NSArray_Single_Throws_OnMultiple ()
		{
			using var arr = MakeArray ("a", "b");
			Assert.Throws<InvalidOperationException> (() => arr.Single ());
		}

		[Test]
		public void NSArray_SingleOrDefault_Empty ()
		{
			using var arr = MakeArray ();
			Assert.That (arr.SingleOrDefault (), Is.Null, "SingleOrDefault empty");
		}

		[Test]
		public void NSArray_SingleOrDefault_Predicate ()
		{
			using var arr = MakeArray ("a", "b");
			Assert.That (arr.SingleOrDefault (s => s.ToString () == "a")?.ToString (), Is.EqualTo ("a"), "SingleOrDefault predicate match");
			Assert.That (arr.SingleOrDefault (s => s.ToString () == "z"), Is.Null, "SingleOrDefault predicate no match");
		}

		[Test]
		public void NSArray_ElementAt ()
		{
			using var arr = MakeArray ("a", "b", "c");
			Assert.That (arr.ElementAt (0).ToString (), Is.EqualTo ("a"), "ElementAt 0");
			Assert.That (arr.ElementAt (2).ToString (), Is.EqualTo ("c"), "ElementAt 2");
		}

		[Test]
		public void NSArray_ElementAt_OutOfRange_Throws ()
		{
			using var arr = MakeArray ("a");
			Assert.Throws<ArgumentOutOfRangeException> (() => arr.ElementAt (5));
		}

		[Test]
		public void NSArray_ElementAtOrDefault ()
		{
			using var arr = MakeArray ("a", "b");
			Assert.That (arr.ElementAtOrDefault (1)?.ToString (), Is.EqualTo ("b"), "ElementAtOrDefault in range");
			Assert.That (arr.ElementAtOrDefault (99), Is.Null, "ElementAtOrDefault out of range");
		}

		[Test]
		public void NSArray_Any ()
		{
			using var empty = MakeArray ();
			Assert.That (empty.Any (), Is.False, "Any empty");
			using var arr = MakeArray ("a");
			Assert.That (arr.Any (), Is.True, "Any non-empty");
			Assert.That (arr.Any (s => s.ToString () == "a"), Is.True, "Any predicate true");
			Assert.That (arr.Any (s => s.ToString () == "z"), Is.False, "Any predicate false");
		}

		[Test]
		public void NSArray_All ()
		{
			using var arr = MakeArray ("ab", "abc");
			Assert.That (arr.All (s => s.Length > 1), Is.True, "All true");
			Assert.That (arr.All (s => s.Length > 2), Is.False, "All false");
		}

		[Test]
		public void NSArray_Count ()
		{
			using var arr = MakeArray ("a", "b", "c");
			Assert.That (arr.Count (), Is.EqualTo (3), "Count");
			Assert.That (arr.Count (s => s.ToString () == "a"), Is.EqualTo (1), "Count predicate");
		}

		[Test]
		public void NSArray_LongCount ()
		{
			using var arr = MakeArray ("a", "b");
			Assert.That (arr.LongCount (), Is.EqualTo (2L), "LongCount");
			Assert.That (arr.LongCount (s => s.ToString () == "a"), Is.EqualTo (1L), "LongCount predicate");
		}

		[Test]
		public void NSArray_Where ()
		{
			using var arr = MakeArray ("a", "bb", "ccc");
			var result = arr.Where (s => s.Length > 1).ToList ();
			Assert.That (result.Count, Is.EqualTo (2), "Where count");
		}

		[Test]
		public void NSArray_Where_Index ()
		{
			using var arr = MakeArray ("a", "b", "c");
			var result = arr.Where ((s, i) => i % 2 == 0).ToList ();
			Assert.That (result.Count, Is.EqualTo (2), "Where index count");
			Assert.That (result [0].ToString (), Is.EqualTo ("a"), "Where index [0]");
			Assert.That (result [1].ToString (), Is.EqualTo ("c"), "Where index [1]");
		}

		[Test]
		public void NSArray_Select ()
		{
			using var arr = MakeArray ("hello", "world");
			var lengths = arr.Select (s => s.Length).ToList ();
			Assert.That (lengths [0], Is.EqualTo (5), "Select length [0]");
			Assert.That (lengths [1], Is.EqualTo (5), "Select length [1]");
		}

		[Test]
		public void NSArray_Select_Index ()
		{
			using var arr = MakeArray ("a", "b", "c");
			var indexed = arr.Select ((s, i) => $"{i}:{s}").ToList ();
			Assert.That (indexed [0], Is.EqualTo ("0:a"), "Select index 0");
			Assert.That (indexed [2], Is.EqualTo ("2:c"), "Select index 2");
		}

		[Test]
		public void NSArray_OrderBy ()
		{
			using var arr = MakeArray ("banana", "apple", "cherry");
			var sorted = arr.OrderBy (s => s.ToString ()).ToList ();
			Assert.That (sorted [0].ToString (), Is.EqualTo ("apple"), "OrderBy first");
			Assert.That (sorted [1].ToString (), Is.EqualTo ("banana"), "OrderBy second");
			Assert.That (sorted [2].ToString (), Is.EqualTo ("cherry"), "OrderBy third");
		}

		[Test]
		public void NSArray_OrderByDescending ()
		{
			using var arr = MakeArray ("banana", "apple", "cherry");
			var sorted = arr.OrderByDescending (s => s.ToString ()).ToList ();
			Assert.That (sorted [0].ToString (), Is.EqualTo ("cherry"), "OrderByDesc first");
			Assert.That (sorted [2].ToString (), Is.EqualTo ("apple"), "OrderByDesc last");
		}

		[Test]
		public void NSArray_Skip ()
		{
			using var arr = MakeArray ("a", "b", "c", "d");
			var skipped = arr.Skip (2).ToList ();
			Assert.That (skipped.Count, Is.EqualTo (2), "Skip count");
			Assert.That (skipped [0].ToString (), Is.EqualTo ("c"), "Skip [0]");
		}

		[Test]
		public void NSArray_SkipWhile ()
		{
			using var arr = MakeArray ("a", "b", "c");
			var result = arr.SkipWhile (s => s.ToString () != "b").ToList ();
			Assert.That (result.Count, Is.EqualTo (2), "SkipWhile count");
			Assert.That (result [0].ToString (), Is.EqualTo ("b"), "SkipWhile first");
		}

		[Test]
		public void NSArray_Take ()
		{
			using var arr = MakeArray ("a", "b", "c");
			var taken = arr.Take (2).ToList ();
			Assert.That (taken.Count, Is.EqualTo (2), "Take count");
			Assert.That (taken [1].ToString (), Is.EqualTo ("b"), "Take [1]");
		}

		[Test]
		public void NSArray_TakeWhile ()
		{
			using var arr = MakeArray ("a", "b", "c");
			var taken = arr.TakeWhile (s => s.ToString () != "c").ToList ();
			Assert.That (taken.Count, Is.EqualTo (2), "TakeWhile count");
		}

		[Test]
		public void NSArray_Distinct ()
		{
			using var arr = MakeArray ("a", "a", "b");
			var distinct = arr.Distinct ().ToList ();
			Assert.That (distinct.Count, Is.EqualTo (2), "Distinct count");
		}

		[Test]
		public void NSArray_Reverse ()
		{
			using var arr = MakeArray ("a", "b", "c");
			var reversed = arr.Reverse ().ToList ();
			Assert.That (reversed [0].ToString (), Is.EqualTo ("c"), "Reverse [0]");
			Assert.That (reversed [2].ToString (), Is.EqualTo ("a"), "Reverse [2]");
		}

		[Test]
		public void NSArray_Concat ()
		{
			using var arr = MakeArray ("a", "b");
			var extra = new [] { S ("c") };
			var all = arr.Concat (extra).ToList ();
			Assert.That (all.Count, Is.EqualTo (3), "Concat count");
			Assert.That (all [2].ToString (), Is.EqualTo ("c"), "Concat last");
		}

		[Test]
		public void NSArray_ToList ()
		{
			using var arr = MakeArray ("a", "b", "c");
			var list = arr.ToList ();
			Assert.That (list, Is.TypeOf<List<NSString>> (), "ToList type");
			Assert.That (list.Count, Is.EqualTo (3), "ToList count");
		}

		[Test]
		public void NSArray_ToArray ()
		{
			using var arr = MakeArray ("a", "b");
			var result = arr.ToArray ();
			Assert.That (result, Is.TypeOf<NSString []> (), "ToArray type");
			Assert.That (result.Length, Is.EqualTo (2), "ToArray length");
		}

		[Test]
		public void NSArray_Aggregate ()
		{
			using var arr = MakeArray ("a", "b", "c");
			var result = arr.Aggregate ((acc, s) => (NSString) (acc.ToString () + s.ToString ()));
			Assert.That (result.ToString (), Is.EqualTo ("abc"), "Aggregate");
		}

		[Test]
		public void NSArray_Aggregate_Seed ()
		{
			using var arr = MakeArray ("a", "b", "c");
			var result = arr.Aggregate ("", (acc, s) => acc + s.ToString ());
			Assert.That (result, Is.EqualTo ("abc"), "Aggregate seed");
		}

		// ===== NSMutableArray<T> =====

		[Test]
		public void NSMutableArray_First ()
		{
			using var arr = MakeMutableArray ("a", "b");
			Assert.That (arr.First ().ToString (), Is.EqualTo ("a"), "First");
		}

		[Test]
		public void NSMutableArray_First_Empty_Throws ()
		{
			using var arr = MakeMutableArray ();
			Assert.Throws<InvalidOperationException> (() => arr.First ());
		}

		[Test]
		public void NSMutableArray_FirstOrDefault_Empty ()
		{
			using var arr = MakeMutableArray ();
			Assert.That (arr.FirstOrDefault (), Is.Null, "FirstOrDefault empty");
		}

		[Test]
		public void NSMutableArray_FirstOrDefault ()
		{
			using var arr = MakeMutableArray ("x", "y");
			Assert.That (arr.FirstOrDefault ()?.ToString (), Is.EqualTo ("x"), "FirstOrDefault");
		}

		[Test]
		public void NSMutableArray_Last ()
		{
			using var arr = MakeMutableArray ("a", "b", "c");
			Assert.That (arr.Last ().ToString (), Is.EqualTo ("c"), "Last");
		}

		[Test]
		public void NSMutableArray_LastOrDefault_Empty ()
		{
			using var arr = MakeMutableArray ();
			Assert.That (arr.LastOrDefault (), Is.Null, "LastOrDefault empty");
		}

		[Test]
		public void NSMutableArray_Single ()
		{
			using var arr = MakeMutableArray ("only");
			Assert.That (arr.Single ().ToString (), Is.EqualTo ("only"), "Single");
		}

		[Test]
		public void NSMutableArray_Single_Throws_OnMultiple ()
		{
			using var arr = MakeMutableArray ("a", "b");
			Assert.Throws<InvalidOperationException> (() => arr.Single ());
		}

		[Test]
		public void NSMutableArray_SingleOrDefault_Empty ()
		{
			using var arr = MakeMutableArray ();
			Assert.That (arr.SingleOrDefault (), Is.Null, "SingleOrDefault empty");
		}

		[Test]
		public void NSMutableArray_ElementAt ()
		{
			using var arr = MakeMutableArray ("a", "b", "c");
			Assert.That (arr.ElementAt (1).ToString (), Is.EqualTo ("b"), "ElementAt 1");
		}

		[Test]
		public void NSMutableArray_ElementAtOrDefault ()
		{
			using var arr = MakeMutableArray ("a");
			Assert.That (arr.ElementAtOrDefault (0)?.ToString (), Is.EqualTo ("a"), "ElementAtOrDefault in range");
			Assert.That (arr.ElementAtOrDefault (10), Is.Null, "ElementAtOrDefault out of range");
		}

		[Test]
		public void NSMutableArray_Any ()
		{
			using var empty = MakeMutableArray ();
			Assert.That (empty.Any (), Is.False, "Any empty");
			using var arr = MakeMutableArray ("a");
			Assert.That (arr.Any (), Is.True, "Any non-empty");
		}

		[Test]
		public void NSMutableArray_All ()
		{
			using var arr = MakeMutableArray ("ab", "abc");
			Assert.That (arr.All (s => s.Length > 1), Is.True, "All true");
		}

		[Test]
		public void NSMutableArray_Count ()
		{
			using var arr = MakeMutableArray ("a", "b", "c");
			Assert.That (arr.Count (), Is.EqualTo (3), "Count");
		}

		[Test]
		public void NSMutableArray_LongCount ()
		{
			using var arr = MakeMutableArray ("a", "b");
			Assert.That (arr.LongCount (), Is.EqualTo (2L), "LongCount");
		}

		[Test]
		public void NSMutableArray_Where ()
		{
			using var arr = MakeMutableArray ("a", "bb", "ccc");
			var result = arr.Where (s => s.Length > 1).ToList ();
			Assert.That (result.Count, Is.EqualTo (2), "Where count");
		}

		[Test]
		public void NSMutableArray_Select ()
		{
			using var arr = MakeMutableArray ("hello", "world");
			var lengths = arr.Select (s => s.Length).ToList ();
			Assert.That (lengths [0], Is.EqualTo (5), "Select [0]");
		}

		[Test]
		public void NSMutableArray_OrderBy ()
		{
			using var arr = MakeMutableArray ("banana", "apple", "cherry");
			var sorted = arr.OrderBy (s => s.ToString ()).ToList ();
			Assert.That (sorted [0].ToString (), Is.EqualTo ("apple"), "OrderBy first");
		}

		[Test]
		public void NSMutableArray_OrderByDescending ()
		{
			using var arr = MakeMutableArray ("banana", "apple", "cherry");
			var sorted = arr.OrderByDescending (s => s.ToString ()).ToList ();
			Assert.That (sorted [0].ToString (), Is.EqualTo ("cherry"), "OrderByDesc first");
		}

		[Test]
		public void NSMutableArray_Skip_Take ()
		{
			using var arr = MakeMutableArray ("a", "b", "c", "d");
			var sliced = arr.Skip (1).Take (2).ToList ();
			Assert.That (sliced.Count, Is.EqualTo (2), "Skip/Take count");
		}

		[Test]
		public void NSMutableArray_SkipWhile_TakeWhile ()
		{
			using var arr = MakeMutableArray ("a", "b", "c");
			var skipped = arr.SkipWhile (s => s.ToString () == "a").ToList ();
			Assert.That (skipped.Count, Is.EqualTo (2), "SkipWhile count");
			var taken = arr.TakeWhile (s => s.ToString () != "c").ToList ();
			Assert.That (taken.Count, Is.EqualTo (2), "TakeWhile count");
		}

		[Test]
		public void NSMutableArray_Distinct ()
		{
			using var arr = MakeMutableArray ("a", "a", "b");
			var distinct = arr.Distinct ().ToList ();
			Assert.That (distinct.Count, Is.EqualTo (2), "Distinct count");
		}

		[Test]
		public void NSMutableArray_Reverse ()
		{
			using var arr = MakeMutableArray ("a", "b", "c");
			var reversed = arr.Reverse ().ToList ();
			Assert.That (reversed [0].ToString (), Is.EqualTo ("c"), "Reverse [0]");
		}

		[Test]
		public void NSMutableArray_Concat ()
		{
			using var arr = MakeMutableArray ("a");
			var all = arr.Concat (new [] { S ("b") }).ToList ();
			Assert.That (all.Count, Is.EqualTo (2), "Concat count");
		}

		[Test]
		public void NSMutableArray_ToList ()
		{
			using var arr = MakeMutableArray ("a", "b");
			var list = arr.ToList ();
			Assert.That (list.Count, Is.EqualTo (2), "ToList count");
		}

		[Test]
		public void NSMutableArray_ToArray ()
		{
			using var arr = MakeMutableArray ("a", "b");
			var result = arr.ToArray ();
			Assert.That (result.Length, Is.EqualTo (2), "ToArray length");
		}

		[Test]
		public void NSMutableArray_Aggregate ()
		{
			using var arr = MakeMutableArray ("a", "b", "c");
			var result = arr.Aggregate ((acc, s) => (NSString) (acc.ToString () + s.ToString ()));
			Assert.That (result.ToString (), Is.EqualTo ("abc"), "Aggregate");
		}

		[Test]
		public void NSMutableArray_Aggregate_Seed ()
		{
			using var arr = MakeMutableArray ("a", "b", "c");
			var result = arr.Aggregate (0, (acc, s) => acc + s.Length);
			Assert.That (result, Is.EqualTo (3), "Aggregate seed");
		}

		// ===== NSOrderedSet<T> =====

		[Test]
		public void NSOrderedSet_First ()
		{
			using var set = MakeOrderedSet ("a", "b", "c");
			Assert.That (set.First ().ToString (), Is.EqualTo ("a"), "First");
		}

		[Test]
		public void NSOrderedSet_First_Predicate ()
		{
			using var set = MakeOrderedSet ("a", "b", "c");
			Assert.That (set.First (s => s.ToString () == "c").ToString (), Is.EqualTo ("c"), "First predicate");
		}

		[Test]
		public void NSOrderedSet_First_Empty_Throws ()
		{
			using var set = MakeOrderedSet ();
			Assert.Throws<InvalidOperationException> (() => set.First ());
		}

		[Test]
		public void NSOrderedSet_FirstOrDefault_Empty ()
		{
			using var set = MakeOrderedSet ();
			Assert.That (set.FirstOrDefault (), Is.Null, "FirstOrDefault empty");
		}

		[Test]
		public void NSOrderedSet_FirstOrDefault ()
		{
			using var set = MakeOrderedSet ("x", "y");
			Assert.That (set.FirstOrDefault ()?.ToString (), Is.EqualTo ("x"), "FirstOrDefault");
		}

		[Test]
		public void NSOrderedSet_Last ()
		{
			using var set = MakeOrderedSet ("a", "b", "c");
			Assert.That (set.Last ().ToString (), Is.EqualTo ("c"), "Last");
		}

		[Test]
		public void NSOrderedSet_Last_Predicate ()
		{
			using var set = MakeOrderedSet ("a", "b", "c");
			Assert.That (set.Last (s => s.ToString () != "c").ToString (), Is.EqualTo ("b"), "Last predicate");
		}

		[Test]
		public void NSOrderedSet_Last_Empty_Throws ()
		{
			using var set = MakeOrderedSet ();
			Assert.Throws<InvalidOperationException> (() => set.Last ());
		}

		[Test]
		public void NSOrderedSet_LastOrDefault_Empty ()
		{
			using var set = MakeOrderedSet ();
			Assert.That (set.LastOrDefault (), Is.Null, "LastOrDefault empty");
		}

		[Test]
		public void NSOrderedSet_Single ()
		{
			using var set = MakeOrderedSet ("only");
			Assert.That (set.Single ().ToString (), Is.EqualTo ("only"), "Single");
		}

		[Test]
		public void NSOrderedSet_Single_Throws_OnMultiple ()
		{
			using var set = MakeOrderedSet ("a", "b");
			Assert.Throws<InvalidOperationException> (() => set.Single ());
		}

		[Test]
		public void NSOrderedSet_SingleOrDefault_Empty ()
		{
			using var set = MakeOrderedSet ();
			Assert.That (set.SingleOrDefault (), Is.Null, "SingleOrDefault empty");
		}

		[Test]
		public void NSOrderedSet_SingleOrDefault_Predicate ()
		{
			using var set = MakeOrderedSet ("a", "b");
			Assert.That (set.SingleOrDefault (s => s.ToString () == "a")?.ToString (), Is.EqualTo ("a"), "SingleOrDefault predicate match");
		}

		[Test]
		public void NSOrderedSet_ElementAt ()
		{
			using var set = MakeOrderedSet ("a", "b", "c");
			Assert.That (set.ElementAt (1).ToString (), Is.EqualTo ("b"), "ElementAt 1");
		}

		[Test]
		public void NSOrderedSet_ElementAtOrDefault ()
		{
			using var set = MakeOrderedSet ("a");
			Assert.That (set.ElementAtOrDefault (0)?.ToString (), Is.EqualTo ("a"), "in range");
			Assert.That (set.ElementAtOrDefault (5), Is.Null, "out of range");
		}

		[Test]
		public void NSOrderedSet_Any ()
		{
			using var empty = MakeOrderedSet ();
			Assert.That (empty.Any (), Is.False, "Any empty");
			using var set = MakeOrderedSet ("a");
			Assert.That (set.Any (), Is.True, "Any non-empty");
			Assert.That (set.Any (s => s.ToString () == "a"), Is.True, "Any predicate true");
			Assert.That (set.Any (s => s.ToString () == "z"), Is.False, "Any predicate false");
		}

		[Test]
		public void NSOrderedSet_All ()
		{
			using var set = MakeOrderedSet ("ab", "abc");
			Assert.That (set.All (s => s.Length > 1), Is.True, "All true");
			Assert.That (set.All (s => s.Length > 2), Is.False, "All false");
		}

		[Test]
		public void NSOrderedSet_Count ()
		{
			using var set = MakeOrderedSet ("a", "b", "c");
			Assert.That (set.Count (), Is.EqualTo (3), "Count");
			Assert.That (set.Count (s => s.ToString () == "a"), Is.EqualTo (1), "Count predicate");
		}

		[Test]
		public void NSOrderedSet_LongCount ()
		{
			using var set = MakeOrderedSet ("a", "b");
			Assert.That (set.LongCount (), Is.EqualTo (2L), "LongCount");
		}

		[Test]
		public void NSOrderedSet_Where ()
		{
			using var set = MakeOrderedSet ("a", "bb", "ccc");
			var result = set.Where (s => s.Length > 1).ToList ();
			Assert.That (result.Count, Is.EqualTo (2), "Where count");
		}

		[Test]
		public void NSOrderedSet_Where_Index ()
		{
			using var set = MakeOrderedSet ("a", "b", "c");
			var result = set.Where ((s, i) => i % 2 == 0).ToList ();
			Assert.That (result.Count, Is.EqualTo (2), "Where index count");
		}

		[Test]
		public void NSOrderedSet_Select ()
		{
			using var set = MakeOrderedSet ("hello", "world");
			var lengths = set.Select (s => s.Length).ToList ();
			Assert.That (lengths [0], Is.EqualTo (5), "Select [0]");
		}

		[Test]
		public void NSOrderedSet_Select_Index ()
		{
			using var set = MakeOrderedSet ("a", "b");
			var indexed = set.Select ((s, i) => $"{i}:{s}").ToList ();
			Assert.That (indexed [0], Is.EqualTo ("0:a"), "Select index 0");
		}

		[Test]
		public void NSOrderedSet_OrderBy ()
		{
			using var set = MakeOrderedSet ("banana", "apple", "cherry");
			var sorted = set.OrderBy (s => s.ToString ()).ToList ();
			Assert.That (sorted [0].ToString (), Is.EqualTo ("apple"), "OrderBy first");
		}

		[Test]
		public void NSOrderedSet_OrderByDescending ()
		{
			using var set = MakeOrderedSet ("banana", "apple", "cherry");
			var sorted = set.OrderByDescending (s => s.ToString ()).ToList ();
			Assert.That (sorted [0].ToString (), Is.EqualTo ("cherry"), "OrderByDesc first");
		}

		[Test]
		public void NSOrderedSet_Skip_Take ()
		{
			using var set = MakeOrderedSet ("a", "b", "c", "d");
			var sliced = set.Skip (1).Take (2).ToList ();
			Assert.That (sliced.Count, Is.EqualTo (2), "Skip/Take count");
			Assert.That (sliced [0].ToString (), Is.EqualTo ("b"), "Skip/Take [0]");
		}

		[Test]
		public void NSOrderedSet_SkipWhile_TakeWhile ()
		{
			using var set = MakeOrderedSet ("a", "b", "c");
			var skipped = set.SkipWhile (s => s.ToString () == "a").ToList ();
			Assert.That (skipped.Count, Is.EqualTo (2), "SkipWhile count");
			var taken = set.TakeWhile (s => s.ToString () != "c").ToList ();
			Assert.That (taken.Count, Is.EqualTo (2), "TakeWhile count");
		}

		[Test]
		public void NSOrderedSet_Distinct ()
		{
			using var set = MakeOrderedSet ("a", "b");
			var distinct = set.Distinct ().ToList ();
			Assert.That (distinct.Count, Is.EqualTo (2), "Distinct count");
		}

		[Test]
		public void NSOrderedSet_Reverse ()
		{
			using var set = MakeOrderedSet ("a", "b", "c");
			var reversed = set.Reverse ().ToList ();
			Assert.That (reversed [0].ToString (), Is.EqualTo ("c"), "Reverse [0]");
			Assert.That (reversed [2].ToString (), Is.EqualTo ("a"), "Reverse [2]");
		}

		[Test]
		public void NSOrderedSet_Concat ()
		{
			using var set = MakeOrderedSet ("a", "b");
			var all = set.Concat (new [] { S ("c") }).ToList ();
			Assert.That (all.Count, Is.EqualTo (3), "Concat count");
		}

		[Test]
		public void NSOrderedSet_ToList ()
		{
			using var set = MakeOrderedSet ("a", "b");
			var list = set.ToList ();
			Assert.That (list.Count, Is.EqualTo (2), "ToList count");
		}

		[Test]
		public void NSOrderedSet_ToArray ()
		{
			using var set = MakeOrderedSet ("a", "b");
			var arr = set.ToArray ();
			Assert.That (arr.Length, Is.EqualTo (2), "ToArray length");
		}

		[Test]
		public void NSOrderedSet_Aggregate ()
		{
			using var set = MakeOrderedSet ("a", "b", "c");
			var result = set.Aggregate ((acc, s) => (NSString) (acc.ToString () + s.ToString ()));
			Assert.That (result.ToString (), Is.EqualTo ("abc"), "Aggregate");
		}

		[Test]
		public void NSOrderedSet_Aggregate_Seed ()
		{
			using var set = MakeOrderedSet ("a", "b", "c");
			var result = set.Aggregate ("", (acc, s) => acc + s.ToString ());
			Assert.That (result, Is.EqualTo ("abc"), "Aggregate seed");
		}

		// ===== NSMutableOrderedSet<T> =====

		[Test]
		public void NSMutableOrderedSet_First ()
		{
			using var set = MakeMutableOrderedSet ("a", "b", "c");
			Assert.That (set.First ().ToString (), Is.EqualTo ("a"), "First");
		}

		[Test]
		public void NSMutableOrderedSet_First_Empty_Throws ()
		{
			using var set = MakeMutableOrderedSet ();
			Assert.Throws<InvalidOperationException> (() => set.First ());
		}

		[Test]
		public void NSMutableOrderedSet_FirstOrDefault_Empty ()
		{
			using var set = MakeMutableOrderedSet ();
			Assert.That (set.FirstOrDefault (), Is.Null, "FirstOrDefault empty");
		}

		[Test]
		public void NSMutableOrderedSet_FirstOrDefault ()
		{
			using var set = MakeMutableOrderedSet ("x", "y");
			Assert.That (set.FirstOrDefault ()?.ToString (), Is.EqualTo ("x"), "FirstOrDefault");
		}

		[Test]
		public void NSMutableOrderedSet_FirstOrDefault_Predicate ()
		{
			using var set = MakeMutableOrderedSet ("a", "b", "c");
			Assert.That (set.FirstOrDefault (s => s.ToString () == "b")?.ToString (), Is.EqualTo ("b"), "FirstOrDefault predicate match");
			Assert.That (set.FirstOrDefault (s => s.ToString () == "z"), Is.Null, "FirstOrDefault predicate no match");
		}

		[Test]
		public void NSMutableOrderedSet_Last ()
		{
			using var set = MakeMutableOrderedSet ("a", "b", "c");
			Assert.That (set.Last ().ToString (), Is.EqualTo ("c"), "Last");
		}

		[Test]
		public void NSMutableOrderedSet_LastOrDefault_Empty ()
		{
			using var set = MakeMutableOrderedSet ();
			Assert.That (set.LastOrDefault (), Is.Null, "LastOrDefault empty");
		}

		[Test]
		public void NSMutableOrderedSet_Single ()
		{
			using var set = MakeMutableOrderedSet ("only");
			Assert.That (set.Single ().ToString (), Is.EqualTo ("only"), "Single");
		}

		[Test]
		public void NSMutableOrderedSet_Single_Throws_OnMultiple ()
		{
			using var set = MakeMutableOrderedSet ("a", "b");
			Assert.Throws<InvalidOperationException> (() => set.Single ());
		}

		[Test]
		public void NSMutableOrderedSet_SingleOrDefault_Empty ()
		{
			using var set = MakeMutableOrderedSet ();
			Assert.That (set.SingleOrDefault (), Is.Null, "SingleOrDefault empty");
		}

		[Test]
		public void NSMutableOrderedSet_ElementAt ()
		{
			using var set = MakeMutableOrderedSet ("a", "b", "c");
			Assert.That (set.ElementAt (2).ToString (), Is.EqualTo ("c"), "ElementAt 2");
		}

		[Test]
		public void NSMutableOrderedSet_ElementAtOrDefault ()
		{
			using var set = MakeMutableOrderedSet ("a");
			Assert.That (set.ElementAtOrDefault (0)?.ToString (), Is.EqualTo ("a"), "in range");
			Assert.That (set.ElementAtOrDefault (5), Is.Null, "out of range");
		}

		[Test]
		public void NSMutableOrderedSet_Any ()
		{
			using var empty = MakeMutableOrderedSet ();
			Assert.That (empty.Any (), Is.False, "Any empty");
			using var set = MakeMutableOrderedSet ("a");
			Assert.That (set.Any (), Is.True, "Any non-empty");
		}

		[Test]
		public void NSMutableOrderedSet_All ()
		{
			using var set = MakeMutableOrderedSet ("ab", "abc");
			Assert.That (set.All (s => s.Length > 1), Is.True, "All true");
		}

		[Test]
		public void NSMutableOrderedSet_Count ()
		{
			using var set = MakeMutableOrderedSet ("a", "b", "c");
			Assert.That (set.Count (), Is.EqualTo (3), "Count");
			Assert.That (set.Count (s => s.ToString () == "a"), Is.EqualTo (1), "Count predicate");
		}

		[Test]
		public void NSMutableOrderedSet_LongCount ()
		{
			using var set = MakeMutableOrderedSet ("a", "b");
			Assert.That (set.LongCount (), Is.EqualTo (2L), "LongCount");
		}

		[Test]
		public void NSMutableOrderedSet_Where ()
		{
			using var set = MakeMutableOrderedSet ("a", "bb", "ccc");
			var result = set.Where (s => s.Length > 1).ToList ();
			Assert.That (result.Count, Is.EqualTo (2), "Where count");
		}

		[Test]
		public void NSMutableOrderedSet_Select ()
		{
			using var set = MakeMutableOrderedSet ("hello", "world");
			var lengths = set.Select (s => s.Length).ToList ();
			Assert.That (lengths [0], Is.EqualTo (5), "Select [0]");
		}

		[Test]
		public void NSMutableOrderedSet_OrderBy ()
		{
			using var set = MakeMutableOrderedSet ("banana", "apple", "cherry");
			var sorted = set.OrderBy (s => s.ToString ()).ToList ();
			Assert.That (sorted [0].ToString (), Is.EqualTo ("apple"), "OrderBy first");
		}

		[Test]
		public void NSMutableOrderedSet_OrderByDescending ()
		{
			using var set = MakeMutableOrderedSet ("banana", "apple", "cherry");
			var sorted = set.OrderByDescending (s => s.ToString ()).ToList ();
			Assert.That (sorted [0].ToString (), Is.EqualTo ("cherry"), "OrderByDesc first");
		}

		[Test]
		public void NSMutableOrderedSet_Skip_Take ()
		{
			using var set = MakeMutableOrderedSet ("a", "b", "c", "d");
			var sliced = set.Skip (1).Take (2).ToList ();
			Assert.That (sliced.Count, Is.EqualTo (2), "Skip/Take count");
			Assert.That (sliced [0].ToString (), Is.EqualTo ("b"), "Skip/Take [0]");
		}

		[Test]
		public void NSMutableOrderedSet_SkipWhile_TakeWhile ()
		{
			using var set = MakeMutableOrderedSet ("a", "b", "c");
			var skipped = set.SkipWhile (s => s.ToString () == "a").ToList ();
			Assert.That (skipped.Count, Is.EqualTo (2), "SkipWhile count");
			var taken = set.TakeWhile (s => s.ToString () != "c").ToList ();
			Assert.That (taken.Count, Is.EqualTo (2), "TakeWhile count");
		}

		[Test]
		public void NSMutableOrderedSet_Distinct ()
		{
			using var set = MakeMutableOrderedSet ("a", "b");
			var distinct = set.Distinct ().ToList ();
			Assert.That (distinct.Count, Is.EqualTo (2), "Distinct count");
		}

		[Test]
		public void NSMutableOrderedSet_Reverse ()
		{
			using var set = MakeMutableOrderedSet ("a", "b", "c");
			var reversed = set.Reverse ().ToList ();
			Assert.That (reversed [0].ToString (), Is.EqualTo ("c"), "Reverse [0]");
			Assert.That (reversed [2].ToString (), Is.EqualTo ("a"), "Reverse [2]");
		}

		[Test]
		public void NSMutableOrderedSet_Concat ()
		{
			using var set = MakeMutableOrderedSet ("a", "b");
			var all = set.Concat (new [] { S ("c") }).ToList ();
			Assert.That (all.Count, Is.EqualTo (3), "Concat count");
		}

		[Test]
		public void NSMutableOrderedSet_ToList ()
		{
			using var set = MakeMutableOrderedSet ("a", "b");
			var list = set.ToList ();
			Assert.That (list.Count, Is.EqualTo (2), "ToList count");
		}

		[Test]
		public void NSMutableOrderedSet_ToArray ()
		{
			using var set = MakeMutableOrderedSet ("a", "b");
			var arr = set.ToArray ();
			Assert.That (arr.Length, Is.EqualTo (2), "ToArray length");
		}

		[Test]
		public void NSMutableOrderedSet_Aggregate ()
		{
			using var set = MakeMutableOrderedSet ("a", "b", "c");
			var result = set.Aggregate ((acc, s) => (NSString) (acc.ToString () + s.ToString ()));
			Assert.That (result.ToString (), Is.EqualTo ("abc"), "Aggregate");
		}

		[Test]
		public void NSMutableOrderedSet_Aggregate_Seed ()
		{
			using var set = MakeMutableOrderedSet ("a", "b", "c");
			var result = set.Aggregate ("", (acc, s) => acc + s.ToString ());
			Assert.That (result, Is.EqualTo ("abc"), "Aggregate seed");
		}
	}
}
