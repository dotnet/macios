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
				Assert.That (fr.AffectedStores, Is.Null, "AffectedStores");
				Assert.That (fr.Entity, Is.Null, "Entity");
				Assert.That (fr.EntityName, Is.Null, "EntityName");
				Assert.That (fr.FetchBatchSize, Is.EqualTo ((nint) 0), "FetchBatchSize");
				Assert.That (fr.FetchLimit, Is.EqualTo ((nuint) 0), "FetchLimit");
				Assert.That (fr.FetchOffset, Is.EqualTo ((nuint) 0), "FetchOffset");
				Assert.That (fr.HavingPredicate, Is.Null, "HavingPredicate");
				Assert.That (fr.IncludesPendingChanges, Is.True, "IncludesPendingChanges");
				Assert.That (fr.IncludesPropertyValues, Is.True, "IncludesPropertyValues");
				Assert.That (fr.IncludesSubentities, Is.True, "IncludesSubentities");
				Assert.That (fr.Predicate, Is.Null, "Predicate");
				Assert.That (fr.PropertiesToFetch, Is.Null, "PropertiesToFetch");
				Assert.That (fr.PropertiesToGroupBy, Is.Null, "PropertiesToGroupBy");
				Assert.That (fr.RelationshipKeyPathsForPrefetching, Is.Null, "RelationshipKeyPathsForPrefetching");
				Assert.That (fr.ResultType, Is.EqualTo (NSFetchRequestResultType.ManagedObject), "ResultType");
				Assert.That (fr.ReturnsDistinctResults, Is.False, "ReturnsDistinctResults");
				Assert.That (fr.ReturnsObjectsAsFaults, Is.True, "ReturnsObjectsAsFaults");
				Assert.That (fr.ShouldRefreshRefetchedObjects, Is.False, "ShouldRefreshRefetchedObjects");
				Assert.That (fr.SortDescriptors, Is.Null, "SortDescriptors");
			}
		}

		[Test]
		public void CtorString ()
		{
			using (var fr = new NSFetchRequest ("entityName")) {
				Assert.That (fr.EntityName, Is.EqualTo ("entityName"), "EntityName");
				// Entity is invalid (and throws) so we do not check it - except to see if we can set it to null
				fr.Entity = null;
				Assert.That (fr.Entity, Is.Null, "Entity");
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
