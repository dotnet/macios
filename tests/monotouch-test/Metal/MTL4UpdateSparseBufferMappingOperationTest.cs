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
			ClassicAssert.AreEqual (default (MTLSparseTextureMappingMode), operation.Mode);
			ClassicAssert.AreEqual (default (NSRange), operation.BufferRange);
			ClassicAssert.AreEqual (default (nuint), operation.HeapOffset);
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
			ClassicAssert.AreEqual (expectedMode, operation.Mode);
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
			ClassicAssert.AreEqual (expectedRange, operation.BufferRange);
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
			ClassicAssert.AreEqual (expectedOffset, operation.HeapOffset);
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
			ClassicAssert.AreEqual (expectedMode, operation.Mode);
			ClassicAssert.AreEqual (expectedRange, operation.BufferRange);
			ClassicAssert.AreEqual (expectedOffset, operation.HeapOffset);
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
			ClassicAssert.AreEqual (zeroLengthRange, operation.BufferRange);
			ClassicAssert.AreEqual (10, (int) operation.BufferRange.Location);
			ClassicAssert.AreEqual (0, (int) operation.BufferRange.Length);
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
			ClassicAssert.AreEqual (maxRange, operation.BufferRange);
		}

		[Test]
		public void HeapOffset_WithZeroValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseBufferMappingOperation ();

			// Act
			operation.HeapOffset = 0;

			// Assert
			ClassicAssert.AreEqual (0, (int) operation.HeapOffset);
		}

		[Test]
		public void HeapOffset_WithMaxValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4UpdateSparseBufferMappingOperation ();

			// Act
			operation.HeapOffset = nuint.MaxValue;

			// Assert
			ClassicAssert.AreEqual (nuint.MaxValue, operation.HeapOffset);
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
				ClassicAssert.AreEqual (mode, operation.Mode);
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
			ClassicAssert.AreEqual (MTLSparseTextureMappingMode.Map, operation1.Mode);
			ClassicAssert.AreEqual (MTLSparseTextureMappingMode.Unmap, operation2.Mode);
			ClassicAssert.AreNotEqual (operation1.BufferRange, operation2.BufferRange);
			ClassicAssert.AreNotEqual (operation1.HeapOffset, operation2.HeapOffset);
		}
	}
}
