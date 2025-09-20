using System;
using System.Runtime.InteropServices;

using Foundation;
using Metal;

using NUnit.Framework;

namespace MonoTouchFixtures.Metal {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTL4CopySparseBufferMappingOperationTest {

		[Test]
		public void Constructor_Default_InitializesWithDefaultValues ()
		{
			// Arrange & Act
			var operation = new MTL4CopySparseBufferMappingOperation ();

			// Assert
			Assert.AreEqual (default (NSRange), operation.SourceRange);
			Assert.AreEqual (default (nuint), operation.DestinationOffset);
		}

		[Test]
		public void SourceRange_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4CopySparseBufferMappingOperation ();
			var expectedRange = new NSRange (10, 20);

			// Act
			operation.SourceRange = expectedRange;

			// Assert
			Assert.AreEqual (expectedRange, operation.SourceRange);
		}

		[Test]
		public void DestinationOffset_SetAndGet_ReturnsCorrectValue ()
		{
			// Arrange
			var operation = new MTL4CopySparseBufferMappingOperation ();
			nuint expectedOffset = 1024;

			// Act
			operation.DestinationOffset = expectedOffset;

			// Assert
			Assert.AreEqual (expectedOffset, operation.DestinationOffset);
		}

		[Test]
		public void Properties_SetAllProperties_RetainsAllValues ()
		{
			// Arrange
			var operation = new MTL4CopySparseBufferMappingOperation ();
			var expectedRange = new NSRange (5, 15);
			nuint expectedOffset = 2048;

			// Act
			operation.SourceRange = expectedRange;
			operation.DestinationOffset = expectedOffset;

			// Assert
			Assert.AreEqual (expectedRange, operation.SourceRange);
			Assert.AreEqual (expectedOffset, operation.DestinationOffset);
		}

		[Test]
		public void SourceRange_WithZeroLength_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4CopySparseBufferMappingOperation ();
			var zeroLengthRange = new NSRange (10, 0);

			// Act
			operation.SourceRange = zeroLengthRange;

			// Assert
			Assert.AreEqual (zeroLengthRange, operation.SourceRange);
			Assert.AreEqual (10, (int) operation.SourceRange.Location);
			Assert.AreEqual (0, (int) operation.SourceRange.Length);
		}

		[Test]
		public void SourceRange_WithMaxValues_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4CopySparseBufferMappingOperation ();
			var maxRange = new NSRange (nint.MaxValue - 1, 1);

			// Act
			operation.SourceRange = maxRange;

			// Assert
			Assert.AreEqual (maxRange, operation.SourceRange);
		}

		[Test]
		public void DestinationOffset_WithZeroValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4CopySparseBufferMappingOperation ();

			// Act
			operation.DestinationOffset = 0;

			// Assert
			Assert.AreEqual (0, (int) operation.DestinationOffset);
		}

		[Test]
		public void DestinationOffset_WithMaxValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4CopySparseBufferMappingOperation ();

			// Act
			operation.DestinationOffset = nuint.MaxValue;

			// Assert
			Assert.AreEqual (nuint.MaxValue, operation.DestinationOffset);
		}

		[Test]
		public void Struct_MultipleInstances_AreIndependent ()
		{
			// Arrange
			var operation1 = new MTL4CopySparseBufferMappingOperation ();
			var operation2 = new MTL4CopySparseBufferMappingOperation ();

			// Act
			operation1.SourceRange = new NSRange (10, 20);
			operation1.DestinationOffset = 1024;

			operation2.SourceRange = new NSRange (30, 40);
			operation2.DestinationOffset = 2048;

			// Assert
			Assert.AreNotEqual (operation1.SourceRange, operation2.SourceRange);
			Assert.AreNotEqual (operation1.DestinationOffset, operation2.DestinationOffset);
		}

		[Test]
		public void SourceRange_WithLargeValues_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4CopySparseBufferMappingOperation ();
			var largeRange = new NSRange (1000000, 500000);

			// Act
			operation.SourceRange = largeRange;

			// Assert
			Assert.AreEqual (largeRange, operation.SourceRange);
			Assert.AreEqual (1000000, (int) operation.SourceRange.Location);
			Assert.AreEqual (500000, (int) operation.SourceRange.Length);
		}
	}
}
