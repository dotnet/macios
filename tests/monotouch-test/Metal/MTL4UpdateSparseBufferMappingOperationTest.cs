using System;
using System.Runtime.InteropServices;

using Foundation;
using Metal;

using NUnit.Framework;

namespace MonoTouchFixtures.Metal {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTL4UpdateSparseBufferMappingOperationTest {

		[Test]
		public void Constructor_Default_InitializesWithDefaultValues ()
		{
			// Arrange & Act
			var operation = new MTL4UpdateSparseBufferMappingOperation ();

			// Assert
			Assert.That (operation.Mode, Is.EqualTo (default (MTLSparseTextureMappingMode)));
			Assert.That (operation.BufferRange, Is.EqualTo (default (NSRange)));
			Assert.That (operation.HeapOffset, Is.EqualTo (default (nuint)));
		}

		[Test]
		public void Mode_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseBufferMappingOperation ();
			var expectedMode = MTLSparseTextureMappingMode.Map;

			// Act
			operation.Mode = expectedMode;

			// Assert
			Assert.That (operation.Mode, Is.EqualTo (expectedMode));
		}

		[Test]
		public void BufferRange_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseBufferMappingOperation ();
			var expectedRange = new NSRange (10, 20);

			// Act
			operation.BufferRange = expectedRange;

			// Assert
			Assert.That (operation.BufferRange, Is.EqualTo (expectedRange));
		}

		[Test]
		public void HeapOffset_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseBufferMappingOperation ();
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
			var operation = new MTL4UpdateSparseBufferMappingOperation ();
			var expectedMode = MTLSparseTextureMappingMode.Unmap;
			var expectedRange = new NSRange (5, 15);
			nuint expectedOffset = 2048;

			// Act
			operation.Mode = expectedMode;
			operation.BufferRange = expectedRange;
			operation.HeapOffset = expectedOffset;

			// Assert
			Assert.That (operation.Mode, Is.EqualTo (expectedMode));
			Assert.That (operation.BufferRange, Is.EqualTo (expectedRange));
			Assert.That (operation.HeapOffset, Is.EqualTo (expectedOffset));
		}

		[Test]
		public void BufferRange_WithZeroLength_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseBufferMappingOperation ();
			var zeroLengthRange = new NSRange (10, 0);

			// Act
			operation.BufferRange = zeroLengthRange;

			// Assert
			Assert.That (operation.BufferRange, Is.EqualTo (zeroLengthRange));
			Assert.That ((int) operation.BufferRange.Location, Is.EqualTo (10));
			Assert.That ((int) operation.BufferRange.Length, Is.EqualTo (0));
		}

		[Test]
		public void BufferRange_WithMaxValues_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseBufferMappingOperation ();
			var maxRange = new NSRange (nint.MaxValue - 1, 1);

			// Act
			operation.BufferRange = maxRange;

			// Assert
			Assert.That (operation.BufferRange, Is.EqualTo (maxRange));
		}

		[Test]
		public void HeapOffset_WithZeroValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseBufferMappingOperation ();

			// Act
			operation.HeapOffset = 0;

			// Assert
			Assert.That ((int) operation.HeapOffset, Is.EqualTo (0));
		}

		[Test]
		public void HeapOffset_WithMaxValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseBufferMappingOperation ();

			// Act
			operation.HeapOffset = nuint.MaxValue;

			// Assert
			Assert.That (operation.HeapOffset, Is.EqualTo (nuint.MaxValue));
		}

		[Test]
		public void Mode_WithAllValidValues_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseBufferMappingOperation ();
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
			var operation1 = new MTL4UpdateSparseBufferMappingOperation ();
			var operation2 = new MTL4UpdateSparseBufferMappingOperation ();

			// Act
			operation1.Mode = MTLSparseTextureMappingMode.Map;
			operation1.BufferRange = new NSRange (10, 20);
			operation1.HeapOffset = 1024;

			operation2.Mode = MTLSparseTextureMappingMode.Unmap;
			operation2.BufferRange = new NSRange (30, 40);
			operation2.HeapOffset = 2048;

			// Assert
			Assert.That (operation1.Mode, Is.EqualTo (MTLSparseTextureMappingMode.Map));
			Assert.That (operation2.Mode, Is.EqualTo (MTLSparseTextureMappingMode.Unmap));
			Assert.That (operation2.BufferRange, Is.Not.EqualTo (operation1.BufferRange));
			Assert.That (operation2.HeapOffset, Is.Not.EqualTo (operation1.HeapOffset));
		}
	}
}
