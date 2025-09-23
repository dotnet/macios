using System;
using System.Runtime.InteropServices;

using Foundation;
using Metal;

using NUnit.Framework;

namespace MonoTouchFixtures.Metal {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTL4UpdateSparseTextureMappingOperationTest {

		[Test]
		public void Constructor_Default_InitializesWithDefaultValues ()
		{
			// Arrange & Act
			var operation = new MTL4UpdateSparseTextureMappingOperation ();

			// Assert
			Assert.AreEqual (default (MTLSparseTextureMappingMode), operation.Mode);
			Assert.AreEqual (default (MTLRegion), operation.TextureRegion);
			Assert.AreEqual (default (nuint), operation.TextureLevel);
			Assert.AreEqual (default (nuint), operation.TextureSlice);
			Assert.AreEqual (default (nuint), operation.HeapOffset);
		}

		[Test]
		public void Mode_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();
			var expectedMode = MTLSparseTextureMappingMode.Map;

			// Act
			operation.Mode = expectedMode;

			// Assert
			Assert.AreEqual (expectedMode, operation.Mode);
		}

		[Test]
		public void TextureRegion_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();
			var expectedRegion = new MTLRegion (new MTLOrigin (1, 2, 3), new MTLSize (10, 20, 30));

			// Act
			operation.TextureRegion = expectedRegion;

			// Assert
			Assert.AreEqual (expectedRegion, operation.TextureRegion);
		}

		[Test]
		public void TextureLevel_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();
			nuint expectedLevel = 5;

			// Act
			operation.TextureLevel = expectedLevel;

			// Assert
			Assert.AreEqual (expectedLevel, operation.TextureLevel);
		}

		[Test]
		public void TextureSlice_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();
			nuint expectedSlice = 3;

			// Act
			operation.TextureSlice = expectedSlice;

			// Assert
			Assert.AreEqual (expectedSlice, operation.TextureSlice);
		}

		[Test]
		public void HeapOffset_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();
			nuint expectedOffset = 1024;

			// Act
			operation.HeapOffset = expectedOffset;

			// Assert
			Assert.AreEqual (expectedOffset, operation.HeapOffset);
		}

		[Test]
		public void Properties_SetAllProperties_RetainsAllValues ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();
			var expectedMode = MTLSparseTextureMappingMode.Unmap;
			var expectedRegion = new MTLRegion (new MTLOrigin (1, 2, 3), new MTLSize (10, 20, 30));
			nuint expectedLevel = 5;
			nuint expectedSlice = 3;
			nuint expectedOffset = 2048;

			// Act
			operation.Mode = expectedMode;
			operation.TextureRegion = expectedRegion;
			operation.TextureLevel = expectedLevel;
			operation.TextureSlice = expectedSlice;
			operation.HeapOffset = expectedOffset;

			// Assert
			Assert.AreEqual (expectedMode, operation.Mode);
			Assert.AreEqual (expectedRegion, operation.TextureRegion);
			Assert.AreEqual (expectedLevel, operation.TextureLevel);
			Assert.AreEqual (expectedSlice, operation.TextureSlice);
			Assert.AreEqual (expectedOffset, operation.HeapOffset);
		}

		[Test]
		public void TextureRegion_WithZeroSize_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();
			var zeroSizeRegion = new MTLRegion (new MTLOrigin (10, 20, 30), new MTLSize (0, 0, 0));

			// Act
			operation.TextureRegion = zeroSizeRegion;

			// Assert
			Assert.AreEqual (zeroSizeRegion, operation.TextureRegion);
		}

		[Test]
		public void LevelAndSlice_WithZeroValues_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();

			// Act
			operation.TextureLevel = 0;
			operation.TextureSlice = 0;

			// Assert
			Assert.AreEqual (0, (int) operation.TextureLevel);
			Assert.AreEqual (0, (int) operation.TextureSlice);
		}

		[Test]
		public void LevelAndSlice_WithMaxValues_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();

			// Act
			operation.TextureLevel = nuint.MaxValue;
			operation.TextureSlice = nuint.MaxValue;

			// Assert
			Assert.AreEqual (nuint.MaxValue, operation.TextureLevel);
			Assert.AreEqual (nuint.MaxValue, operation.TextureSlice);
		}

		[Test]
		public void HeapOffset_WithZeroValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();

			// Act
			operation.HeapOffset = 0;

			// Assert
			Assert.AreEqual (0, (int) operation.HeapOffset);
		}

		[Test]
		public void HeapOffset_WithMaxValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();

			// Act
			operation.HeapOffset = nuint.MaxValue;

			// Assert
			Assert.AreEqual (nuint.MaxValue, operation.HeapOffset);
		}

		[Test]
		public void Mode_WithAllValidValues_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();
			var validModes = new [] { MTLSparseTextureMappingMode.Map, MTLSparseTextureMappingMode.Unmap };

			// Act & Assert
			foreach (var mode in validModes) {
				operation.Mode = mode;
				Assert.AreEqual (mode, operation.Mode);
			}
		}

		[Test]
		public void Struct_MultipleInstances_AreIndependent ()
		{
			// Arrange
			var operation1 = new MTL4UpdateSparseTextureMappingOperation ();
			var operation2 = new MTL4UpdateSparseTextureMappingOperation ();

			// Act
			operation1.Mode = MTLSparseTextureMappingMode.Map;
			operation1.TextureRegion = new MTLRegion (new MTLOrigin (1, 2, 3), new MTLSize (10, 20, 30));
			operation1.TextureLevel = 5;
			operation1.TextureSlice = 3;
			operation1.HeapOffset = 1024;

			operation2.Mode = MTLSparseTextureMappingMode.Unmap;
			operation2.TextureRegion = new MTLRegion (new MTLOrigin (4, 5, 6), new MTLSize (40, 50, 60));
			operation2.TextureLevel = 2;
			operation2.TextureSlice = 7;
			operation2.HeapOffset = 2048;

			// Assert
			Assert.AreEqual (MTLSparseTextureMappingMode.Map, operation1.Mode);
			Assert.AreEqual (MTLSparseTextureMappingMode.Unmap, operation2.Mode);
			Assert.AreNotEqual (operation1.TextureRegion, operation2.TextureRegion);
			Assert.AreNotEqual (operation1.TextureLevel, operation2.TextureLevel);
			Assert.AreNotEqual (operation1.TextureSlice, operation2.TextureSlice);
			Assert.AreNotEqual (operation1.HeapOffset, operation2.HeapOffset);
		}

		[Test]
		public void TextureRegion_WithLargeValues_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();
			var largeRegion = new MTLRegion (new MTLOrigin (1000, 2000, 3000), new MTLSize (1024, 2048, 4096));

			// Act
			operation.TextureRegion = largeRegion;

			// Assert
			Assert.AreEqual (largeRegion, operation.TextureRegion);
		}
	}
}
