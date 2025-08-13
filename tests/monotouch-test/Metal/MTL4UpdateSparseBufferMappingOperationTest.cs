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
			Assert.AreEqual (default (MTLSparseTextureMappingMode), operation.Mode);
			Assert.AreEqual (default (NSRange), operation.BufferRange);
			Assert.AreEqual (default (nuint), operation.HeapOffset);
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
			Assert.AreEqual (expectedMode, operation.Mode);
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
			Assert.AreEqual (expectedRange, operation.BufferRange);
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
			Assert.AreEqual (expectedOffset, operation.HeapOffset);
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
			Assert.AreEqual (expectedMode, operation.Mode);
			Assert.AreEqual (expectedRange, operation.BufferRange);
			Assert.AreEqual (expectedOffset, operation.HeapOffset);
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
			Assert.AreEqual (zeroLengthRange, operation.BufferRange);
			Assert.AreEqual (10, (int) operation.BufferRange.Location);
			Assert.AreEqual (0, (int) operation.BufferRange.Length);
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
			Assert.AreEqual (maxRange, operation.BufferRange);
		}

		[Test]
		public void HeapOffset_WithZeroValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseBufferMappingOperation ();

			// Act
			operation.HeapOffset = 0;

			// Assert
			Assert.AreEqual (0, (int) operation.HeapOffset);
		}

		[Test]
		public void HeapOffset_WithMaxValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseBufferMappingOperation ();

			// Act
			operation.HeapOffset = nuint.MaxValue;

			// Assert
			Assert.AreEqual (nuint.MaxValue, operation.HeapOffset);
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
				Assert.AreEqual (mode, operation.Mode);
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
			Assert.AreEqual (MTLSparseTextureMappingMode.Map, operation1.Mode);
			Assert.AreEqual (MTLSparseTextureMappingMode.Unmap, operation2.Mode);
			Assert.AreNotEqual (operation1.BufferRange, operation2.BufferRange);
			Assert.AreNotEqual (operation1.HeapOffset, operation2.HeapOffset);
		}
	}
}
