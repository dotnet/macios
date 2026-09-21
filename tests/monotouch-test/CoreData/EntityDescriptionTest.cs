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
			Assert.That (defaultValue, Is.Not.Null, "default not null");
			Assert.That (defaultValue!.Length, Is.EqualTo (0), "default empty");

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
			Assert.That (result, Is.Not.Null, "result");
			Assert.That (result!.Length, Is.EqualTo (2), "outer length");
			Assert.That (result [0].Length, Is.EqualTo (2), "constraint0 length");
			Assert.That (result [1].Length, Is.EqualTo (1), "constraint1 length");
		}
	}
}
