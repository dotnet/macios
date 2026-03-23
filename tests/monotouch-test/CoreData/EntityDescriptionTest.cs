// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using CoreData;

namespace MonoTouchFixtures.CoreData {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class EntityDescriptionTest {

		[Test]
		public void UniquenessConstraints ()
		{
			using var entity = new NSEntityDescription ();

			// Default is an empty array, not null
			var defaultValue = entity.UniquenessConstraints;
			Assert.IsNotNull (defaultValue, "default not null");
			Assert.AreEqual (0, defaultValue!.Length, "default empty");

			// Add attributes so the entity knows about these property names
			using var nameAttr = new NSAttributeDescription { Name = "name", AttributeType = NSAttributeType.String };
			using var emailAttr = new NSAttributeDescription { Name = "email", AttributeType = NSAttributeType.String };
			using var idAttr = new NSAttributeDescription { Name = "id", AttributeType = NSAttributeType.String };
			entity.Properties = new NSPropertyDescription [] { nameAttr, emailAttr, idAttr };

			var constraints = new NSObject [] [] {
				new NSObject [] { (NSString) "name", (NSString) "email" },
				new NSObject [] { (NSString) "id" },
			};
			entity.UniquenessConstraints = constraints;

			var result = entity.UniquenessConstraints;
			Assert.IsNotNull (result, "result");
			Assert.AreEqual (2, result!.Length, "outer length");
			Assert.AreEqual (2, result [0].Length, "constraint0 length");
			Assert.AreEqual (1, result [1].Length, "constraint1 length");
		}
	}
}
