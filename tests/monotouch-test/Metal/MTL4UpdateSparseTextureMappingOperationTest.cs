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
			Assert.That (operation.Mode, Is.EqualTo (default (MTLSparseTextureMappingMode)));
			Assert.That (operation.TextureRegion, Is.EqualTo (default (MTLRegion)));
			Assert.That (operation.TextureLevel, Is.EqualTo (default (nuint)));
			Assert.That (operation.TextureSlice, Is.EqualTo (default (nuint)));
			Assert.That (operation.HeapOffset, Is.EqualTo (default (nuint)));
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
			Assert.That (operation.Mode, Is.EqualTo (expectedMode));
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
			Assert.That (operation.TextureRegion, Is.EqualTo (expectedRegion));
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
			Assert.That (operation.TextureLevel, Is.EqualTo (expectedLevel));
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
			Assert.That (operation.TextureSlice, Is.EqualTo (expectedSlice));
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
			Assert.That (operation.HeapOffset, Is.EqualTo (expectedOffset));
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
			Assert.That (operation.Mode, Is.EqualTo (expectedMode));
			Assert.That (operation.TextureRegion, Is.EqualTo (expectedRegion));
			Assert.That (operation.TextureLevel, Is.EqualTo (expectedLevel));
			Assert.That (operation.TextureSlice, Is.EqualTo (expectedSlice));
			Assert.That (operation.HeapOffset, Is.EqualTo (expectedOffset));
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
			Assert.That (operation.TextureRegion, Is.EqualTo (zeroSizeRegion));
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
			Assert.That ((int) operation.TextureLevel, Is.EqualTo (0));
			Assert.That ((int) operation.TextureSlice, Is.EqualTo (0));
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
			Assert.That (operation.TextureLevel, Is.EqualTo (nuint.MaxValue));
			Assert.That (operation.TextureSlice, Is.EqualTo (nuint.MaxValue));
		}

		[Test]
		public void HeapOffset_WithZeroValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();

			// Act
			operation.HeapOffset = 0;

			// Assert
			Assert.That ((int) operation.HeapOffset, Is.EqualTo (0));
		}

		[Test]
		public void HeapOffset_WithMaxValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseTextureMappingOperation ();

			// Act
			operation.HeapOffset = nuint.MaxValue;

			// Assert
			Assert.That (operation.HeapOffset, Is.EqualTo (nuint.MaxValue));
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
				Assert.That (operation.Mode, Is.EqualTo (mode));
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
			Assert.That (operation1.Mode, Is.EqualTo (MTLSparseTextureMappingMode.Map));
			Assert.That (operation2.Mode, Is.EqualTo (MTLSparseTextureMappingMode.Unmap));
			Assert.That (operation2.TextureRegion, Is.Not.EqualTo (operation1.TextureRegion));
			Assert.That (operation2.TextureLevel, Is.Not.EqualTo (operation1.TextureLevel));
			Assert.That (operation2.TextureSlice, Is.Not.EqualTo (operation1.TextureSlice));
			Assert.That (operation2.HeapOffset, Is.Not.EqualTo (operation1.HeapOffset));
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
			Assert.That (operation.TextureRegion, Is.EqualTo (largeRegion));
		}
	}
}
