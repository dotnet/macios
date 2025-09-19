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
			Assert.AreEqual (default (MTLRegion), operation.SourceRegion);
			Assert.AreEqual (default (nuint), operation.SourceLevel);
			Assert.AreEqual (default (nuint), operation.SourceSlice);
			Assert.AreEqual (default (MTLOrigin), operation.DestinationOrigin);
			Assert.AreEqual (default (nuint), operation.DestinationLevel);
			Assert.AreEqual (default (nuint), operation.DestinationSlice);
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
			Assert.AreEqual (expectedRegion, operation.SourceRegion);
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
			Assert.AreEqual (expectedLevel, operation.SourceLevel);
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
			Assert.AreEqual (expectedSlice, operation.SourceSlice);
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
			Assert.AreEqual (expectedOrigin, operation.DestinationOrigin);
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
			Assert.AreEqual (expectedLevel, operation.DestinationLevel);
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
			Assert.AreEqual (expectedSlice, operation.DestinationSlice);
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
			Assert.AreEqual (expectedSourceRegion, operation.SourceRegion);
			Assert.AreEqual (expectedSourceLevel, operation.SourceLevel);
			Assert.AreEqual (expectedSourceSlice, operation.SourceSlice);
			Assert.AreEqual (expectedDestinationOrigin, operation.DestinationOrigin);
			Assert.AreEqual (expectedDestinationLevel, operation.DestinationLevel);
			Assert.AreEqual (expectedDestinationSlice, operation.DestinationSlice);
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
			Assert.AreEqual (0, (int) operation.SourceLevel);
			Assert.AreEqual (0, (int) operation.SourceSlice);
			Assert.AreEqual (0, (int) operation.DestinationLevel);
			Assert.AreEqual (0, (int) operation.DestinationSlice);
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
			Assert.AreEqual (nuint.MaxValue, operation.SourceLevel);
			Assert.AreEqual (nuint.MaxValue, operation.SourceSlice);
			Assert.AreEqual (nuint.MaxValue, operation.DestinationLevel);
			Assert.AreEqual (nuint.MaxValue, operation.DestinationSlice);
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
			Assert.AreNotEqual (operation1.SourceRegion, operation2.SourceRegion);
			Assert.AreNotEqual (operation1.SourceLevel, operation2.SourceLevel);
			Assert.AreNotEqual (operation1.SourceSlice, operation2.SourceSlice);
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
			Assert.AreEqual (zeroSizeRegion, operation.SourceRegion);
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
			Assert.AreEqual (zeroOrigin, operation.DestinationOrigin);
		}
	}
}
