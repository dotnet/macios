//
// Unit tests for NSFetchRequest
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using CoreData;

namespace MonoTouchFixtures.CoreData {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class FetchRequestTest {

		[Test]
		public void DefaultValues ()
		{
			using (var fr = new NSFetchRequest ()) {
				ClassicAssert.Null (fr.AffectedStores, "AffectedStores");
				ClassicAssert.Null (fr.Entity, "Entity");
				ClassicAssert.Null (fr.EntityName, "EntityName");
				Assert.That (fr.FetchBatchSize, Is.EqualTo ((nint) 0), "FetchBatchSize");
				Assert.That (fr.FetchLimit, Is.EqualTo ((nuint) 0), "FetchLimit");
				Assert.That (fr.FetchOffset, Is.EqualTo ((nuint) 0), "FetchOffset");
				ClassicAssert.Null (fr.HavingPredicate, "HavingPredicate");
				ClassicAssert.True (fr.IncludesPendingChanges, "IncludesPendingChanges");
				ClassicAssert.True (fr.IncludesPropertyValues, "IncludesPropertyValues");
				ClassicAssert.True (fr.IncludesSubentities, "IncludesSubentities");
				ClassicAssert.Null (fr.Predicate, "Predicate");
				ClassicAssert.Null (fr.PropertiesToFetch, "PropertiesToFetch");
				ClassicAssert.Null (fr.PropertiesToGroupBy, "PropertiesToGroupBy");
				ClassicAssert.Null (fr.RelationshipKeyPathsForPrefetching, "RelationshipKeyPathsForPrefetching");
				Assert.That (fr.ResultType, Is.EqualTo (NSFetchRequestResultType.ManagedObject), "ResultType");
				ClassicAssert.False (fr.ReturnsDistinctResults, "ReturnsDistinctResults");
				ClassicAssert.True (fr.ReturnsObjectsAsFaults, "ReturnsObjectsAsFaults");
				ClassicAssert.False (fr.ShouldRefreshRefetchedObjects, "ShouldRefreshRefetchedObjects");
				ClassicAssert.Null (fr.SortDescriptors, "SortDescriptors");
			}
		}

		[Test]
		public void CtorString ()
		{
			using (var fr = new NSFetchRequest ("entityName")) {
				Assert.That (fr.EntityName, Is.EqualTo ("entityName"), "EntityName");
				// Entity is invalid (and throws) so we do not check it - except to see if we can set it to null
				fr.Entity = null;
				ClassicAssert.Null (fr.Entity, "Entity");
			}
		}

		[Test]
		public void SettersNull ()
		{
			using (var fr = new NSFetchRequest ()) {
				// bug #18153
				fr.Predicate = null;
				// bug #18152
				fr.SortDescriptors = null;
				// other properties that are null (by default) are likely accepting being set to null
				fr.AffectedStores = fr.AffectedStores;
				fr.HavingPredicate = fr.HavingPredicate;
				fr.PropertiesToGroupBy = fr.PropertiesToGroupBy;
				fr.RelationshipKeyPathsForPrefetching = fr.RelationshipKeyPathsForPrefetching;
			}
		}
	}
}
