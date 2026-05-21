using System;
using System.Runtime.InteropServices;

using Foundation;
using Metal;

using NUnit.Framework;

namespace MonoTouchFixtures.Metal {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTL4CopySparseTextureMappingOperationTest {

		[Test]
		public void Constructor_Default_InitializesWithDefaultValues ()
		{
			// Arrange & Act
			var operation = new MTL4CopySparseTextureMappingOperation ();

			// Assert
			Assert.That (operation.SourceRegion, Is.EqualTo (default (MTLRegion)));
			Assert.That (operation.SourceLevel, Is.EqualTo (default (nuint)));
			Assert.That (operation.SourceSlice, Is.EqualTo (default (nuint)));
			Assert.That (operation.DestinationOrigin, Is.EqualTo (default (MTLOrigin)));
			Assert.That (operation.DestinationLevel, Is.EqualTo (default (nuint)));
			Assert.That (operation.DestinationSlice, Is.EqualTo (default (nuint)));
		}

		[Test]
		public void SourceRegion_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4CopySparseTextureMappingOperation ();
			var expectedRegion = new MTLRegion (new MTLOrigin (1, 2, 3), new MTLSize (10, 20, 30));

			// Act
			operation.SourceRegion = expectedRegion;

			// Assert
			Assert.That (operation.SourceRegion, Is.EqualTo (expectedRegion));
		}

		[Test]
		public void SourceLevel_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4CopySparseTextureMappingOperation ();
			nuint expectedLevel = 5;

			// Act
			operation.SourceLevel = expectedLevel;

			// Assert
			Assert.That (operation.SourceLevel, Is.EqualTo (expectedLevel));
		}

		[Test]
		public void SourceSlice_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4CopySparseTextureMappingOperation ();
			nuint expectedSlice = 3;

			// Act
			operation.SourceSlice = expectedSlice;

			// Assert
			Assert.That (operation.SourceSlice, Is.EqualTo (expectedSlice));
		}

		[Test]
		public void DestinationOrigin_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4CopySparseTextureMappingOperation ();
			var expectedOrigin = new MTLOrigin (10, 20, 30);

			// Act
			operation.DestinationOrigin = expectedOrigin;

			// Assert
			Assert.That (operation.DestinationOrigin, Is.EqualTo (expectedOrigin));
		}

		[Test]
		public void DestinationLevel_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4CopySparseTextureMappingOperation ();
			nuint expectedLevel = 2;

			// Act
			operation.DestinationLevel = expectedLevel;

			// Assert
			Assert.That (operation.DestinationLevel, Is.EqualTo (expectedLevel));
		}

		[Test]
		public void DestinationSlice_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4CopySparseTextureMappingOperation ();
			nuint expectedSlice = 7;

			// Act
			operation.DestinationSlice = expectedSlice;

			// Assert
			Assert.That (operation.DestinationSlice, Is.EqualTo (expectedSlice));
		}

		[Test]
		public void Properties_SetAllProperties_RetainsAllValues ()
		{
			// Arrange
			var operation = new MTL4CopySparseTextureMappingOperation ();
			var expectedSourceRegion = new MTLRegion (new MTLOrigin (1, 2, 3), new MTLSize (10, 20, 30));
			nuint expectedSourceLevel = 5;
			nuint expectedSourceSlice = 3;
			var expectedDestinationOrigin = new MTLOrigin (10, 20, 30);
			nuint expectedDestinationLevel = 2;
			nuint expectedDestinationSlice = 7;

			// Act
			operation.SourceRegion = expectedSourceRegion;
			operation.SourceLevel = expectedSourceLevel;
			operation.SourceSlice = expectedSourceSlice;
			operation.DestinationOrigin = expectedDestinationOrigin;
			operation.DestinationLevel = expectedDestinationLevel;
			operation.DestinationSlice = expectedDestinationSlice;

			// Assert
			Assert.That (operation.SourceRegion, Is.EqualTo (expectedSourceRegion));
			Assert.That (operation.SourceLevel, Is.EqualTo (expectedSourceLevel));
			Assert.That (operation.SourceSlice, Is.EqualTo (expectedSourceSlice));
			Assert.That (operation.DestinationOrigin, Is.EqualTo (expectedDestinationOrigin));
			Assert.That (operation.DestinationLevel, Is.EqualTo (expectedDestinationLevel));
			Assert.That (operation.DestinationSlice, Is.EqualTo (expectedDestinationSlice));
		}

		[Test]
		public void LevelAndSlice_WithZeroValues_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4CopySparseTextureMappingOperation ();

			// Act
			operation.SourceLevel = 0;
			operation.SourceSlice = 0;
			operation.DestinationLevel = 0;
			operation.DestinationSlice = 0;

			// Assert
			Assert.That ((int) operation.SourceLevel, Is.EqualTo (0));
			Assert.That ((int) operation.SourceSlice, Is.EqualTo (0));
			Assert.That ((int) operation.DestinationLevel, Is.EqualTo (0));
			Assert.That ((int) operation.DestinationSlice, Is.EqualTo (0));
		}

		[Test]
		public void LevelAndSlice_WithMaxValues_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4CopySparseTextureMappingOperation ();

			// Act
			operation.SourceLevel = nuint.MaxValue;
			operation.SourceSlice = nuint.MaxValue;
			operation.DestinationLevel = nuint.MaxValue;
			operation.DestinationSlice = nuint.MaxValue;

			// Assert
			Assert.That (operation.SourceLevel, Is.EqualTo (nuint.MaxValue));
			Assert.That (operation.SourceSlice, Is.EqualTo (nuint.MaxValue));
			Assert.That (operation.DestinationLevel, Is.EqualTo (nuint.MaxValue));
			Assert.That (operation.DestinationSlice, Is.EqualTo (nuint.MaxValue));
		}

		[Test]
		public void Struct_MultipleInstances_AreIndependent ()
		{
			// Arrange
			var operation1 = new MTL4CopySparseTextureMappingOperation ();
			var operation2 = new MTL4CopySparseTextureMappingOperation ();

			// Act
			operation1.SourceRegion = new MTLRegion (new MTLOrigin (1, 2, 3), new MTLSize (10, 20, 30));
			operation1.SourceLevel = 5;
			operation1.SourceSlice = 3;

			operation2.SourceRegion = new MTLRegion (new MTLOrigin (4, 5, 6), new MTLSize (40, 50, 60));
			operation2.SourceLevel = 2;
			operation2.SourceSlice = 7;

			// Assert
			Assert.That (operation2.SourceRegion, Is.Not.EqualTo (operation1.SourceRegion));
			Assert.That (operation2.SourceLevel, Is.Not.EqualTo (operation1.SourceLevel));
			Assert.That (operation2.SourceSlice, Is.Not.EqualTo (operation1.SourceSlice));
		}

		[Test]
		public void SourceRegion_WithZeroSize_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4CopySparseTextureMappingOperation ();
			var zeroSizeRegion = new MTLRegion (new MTLOrigin (10, 20, 30), new MTLSize (0, 0, 0));

			// Act
			operation.SourceRegion = zeroSizeRegion;

			// Assert
			Assert.That (operation.SourceRegion, Is.EqualTo (zeroSizeRegion));
		}

		[Test]
		public void DestinationOrigin_WithZeroValues_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4CopySparseTextureMappingOperation ();
			var zeroOrigin = new MTLOrigin (0, 0, 0);

			// Act
			operation.DestinationOrigin = zeroOrigin;

			// Assert
			Assert.That (operation.DestinationOrigin, Is.EqualTo (zeroOrigin));
		}
	}
}
