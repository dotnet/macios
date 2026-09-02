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
			Assert.That (operation.SourceRange, Is.EqualTo (default (NSRange)));
			Assert.That (operation.DestinationOffset, Is.EqualTo (default (nuint)));
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
			Assert.That (operation.SourceRange, Is.EqualTo (expectedRange));
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
			Assert.That (operation.DestinationOffset, Is.EqualTo (expectedOffset));
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
			Assert.That (operation.SourceRange, Is.EqualTo (expectedRange));
			Assert.That (operation.DestinationOffset, Is.EqualTo (expectedOffset));
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
			Assert.That (operation.SourceRange, Is.EqualTo (zeroLengthRange));
			Assert.That ((int) operation.SourceRange.Location, Is.EqualTo (10));
			Assert.That ((int) operation.SourceRange.Length, Is.EqualTo (0));
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
			Assert.That (operation.SourceRange, Is.EqualTo (maxRange));
		}

		[Test]
		public void DestinationOffset_WithZeroValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4CopySparseBufferMappingOperation ();

			// Act
			operation.DestinationOffset = 0;

			// Assert
			Assert.That ((int) operation.DestinationOffset, Is.EqualTo (0));
		}

		[Test]
		public void DestinationOffset_WithMaxValue_HandlesCorrectly ()
		{
			// Arrange
			var operation = new MTL4CopySparseBufferMappingOperation ();

			// Act
			operation.DestinationOffset = nuint.MaxValue;

			// Assert
			Assert.That (operation.DestinationOffset, Is.EqualTo (nuint.MaxValue));
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
			Assert.That (operation2.SourceRange, Is.Not.EqualTo (operation1.SourceRange));
			Assert.That (operation2.DestinationOffset, Is.Not.EqualTo (operation1.DestinationOffset));
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
			Assert.That (operation.SourceRange, Is.EqualTo (largeRange));
			Assert.That ((int) operation.SourceRange.Location, Is.EqualTo (1000000));
			Assert.That ((int) operation.SourceRange.Length, Is.EqualTo (500000));
		}
	}
}
