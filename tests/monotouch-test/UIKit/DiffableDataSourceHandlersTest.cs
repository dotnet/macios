// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if !MONOMAC
using System;
using System.Runtime.InteropServices;
using UIKit;

namespace MonoTouchFixtures.UIKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DiffableDataSourceHandlersTest {
		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate byte PredicateInvoker (IntPtr block, NativeHandle item);

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate void ActionInvoker (IntPtr block, NativeHandle item);

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate NativeHandle SnapshotInvoker (IntPtr block, NativeHandle item, NativeHandle snapshot);

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (12, 0);
		}

		[Test]
		public unsafe void SectionSnapshotHandlers ()
		{
			using var handlers = new UICollectionViewDiffableDataSourceSectionSnapshotHandlers<NSString> ();
			using var item = new NSString ("item");
			using var snapshot = new NSDiffableDataSourceSectionSnapshot<NSString> ();
			Func<NSString, bool> predicate = value => value == item;
			NSString? expanded = null;
			NSString? collapsed = null;
			Action<NSString> willExpand = value => expanded = value;
			Action<NSString> willCollapse = value => collapsed = value;
			Func<NSString, NSDiffableDataSourceSectionSnapshot<NSString>, NSDiffableDataSourceSectionSnapshot<NSString>> getSnapshot = (value, current) => {
				Assert.That (value, Is.SameAs (item), "Snapshot item");
				Assert.That (current, Is.SameAs (snapshot), "Current snapshot");
				return snapshot;
			};

			handlers.ShouldExpandItemHandler = predicate;
			handlers.ShouldCollapseItemHandler = predicate;
			handlers.WillExpandItemHandler = willExpand;
			handlers.WillCollapseItemHandler = willCollapse;
			handlers.SnapshotForExpandingParentItemHandler = getSnapshot;
			Assert.That (handlers.ShouldExpandItemHandler, Is.SameAs (predicate), "Expand round trip");
			Assert.That (handlers.ShouldCollapseItemHandler, Is.SameAs (predicate), "Collapse round trip");
			Assert.That (handlers.WillExpandItemHandler, Is.SameAs (willExpand), "Will expand round trip");
			Assert.That (handlers.WillCollapseItemHandler, Is.SameAs (willCollapse), "Will collapse round trip");
			Assert.That (handlers.SnapshotForExpandingParentItemHandler, Is.SameAs (getSnapshot), "Snapshot round trip");

			foreach (var selector in new [] { "shouldExpandItemHandler", "shouldCollapseItemHandler" }) {
				var block = Messaging.IntPtr_objc_msgSend (handlers.Handle, Selector.GetHandle (selector));
				Assert.That (((BlockLiteral*) block)->GetDelegateForBlock<PredicateInvoker> () (block, item.Handle), Is.EqualTo ((byte) 1), selector);
			}
			foreach (var selector in new [] { "willExpandItemHandler", "willCollapseItemHandler" }) {
				var block = Messaging.IntPtr_objc_msgSend (handlers.Handle, Selector.GetHandle (selector));
				((BlockLiteral*) block)->GetDelegateForBlock<ActionInvoker> () (block, item.Handle);
			}
			Assert.That (expanded, Is.SameAs (item), "Expanded item");
			Assert.That (collapsed, Is.SameAs (item), "Collapsed item");
			var snapshotBlock = Messaging.IntPtr_objc_msgSend (handlers.Handle, Selector.GetHandle ("snapshotForExpandingParentItemHandler"));
			Assert.That (((BlockLiteral*) snapshotBlock)->GetDelegateForBlock<SnapshotInvoker> () (snapshotBlock, item.Handle, snapshot.Handle), Is.EqualTo (snapshot.Handle), "Returned snapshot");

			handlers.ShouldExpandItemHandler = null;
			handlers.ShouldCollapseItemHandler = null;
			handlers.WillExpandItemHandler = null;
			handlers.WillCollapseItemHandler = null;
			handlers.SnapshotForExpandingParentItemHandler = null;
			Assert.That (handlers.ShouldExpandItemHandler, Is.Null, "Clear expand");
			Assert.That (handlers.ShouldCollapseItemHandler, Is.Null, "Clear collapse");
			Assert.That (handlers.WillExpandItemHandler, Is.Null, "Clear will expand");
			Assert.That (handlers.WillCollapseItemHandler, Is.Null, "Clear will collapse");
			Assert.That (handlers.SnapshotForExpandingParentItemHandler, Is.Null, "Clear snapshot");
		}

		[Test]
		public unsafe void ReorderingHandlers ()
		{
			using var handlers = new UICollectionViewDiffableDataSourceReorderingHandlers<NSNumber, NSString> ();
			using var item = new NSString ("item");
			Func<NSString, bool> predicate = value => value == item;
			Action<NSDiffableDataSourceTransaction<NSNumber, NSString>> willReorder = _ => { };
			Action<NSDiffableDataSourceTransaction<NSNumber, NSString>> didReorder = _ => { };
			handlers.CanReorderItemHandler = predicate;
			handlers.WillReorderHandler = willReorder;
			handlers.DidReorderHandler = didReorder;
			Assert.That (handlers.CanReorderItemHandler, Is.SameAs (predicate), "Predicate round trip");
			Assert.That (handlers.WillReorderHandler, Is.SameAs (willReorder), "Will reorder round trip");
			Assert.That (handlers.DidReorderHandler, Is.SameAs (didReorder), "Did reorder round trip");
			var block = Messaging.IntPtr_objc_msgSend (handlers.Handle, Selector.GetHandle ("canReorderItemHandler"));
			Assert.That (((BlockLiteral*) block)->GetDelegateForBlock<PredicateInvoker> () (block, item.Handle), Is.EqualTo ((byte) 1), "Can reorder");
			handlers.CanReorderItemHandler = null;
			handlers.WillReorderHandler = null;
			handlers.DidReorderHandler = null;
			Assert.That (handlers.CanReorderItemHandler, Is.Null, "Clear predicate");
			Assert.That (handlers.WillReorderHandler, Is.Null, "Clear will reorder");
			Assert.That (handlers.DidReorderHandler, Is.Null, "Clear did reorder");
		}

		[Preserve (AllMembers = true)]
		class ManagedSnapshotHandlers : UICollectionViewDiffableDataSourceSectionSnapshotHandlers<NSString> {
			public override Func<NSString, bool>? ShouldExpandItemHandler { get; set; }
			public override Func<NSString, NSDiffableDataSourceSectionSnapshot<NSString>, NSDiffableDataSourceSectionSnapshot<NSString>>? SnapshotForExpandingParentItemHandler { get; set; }
		}

		[Preserve (AllMembers = true)]
		class ManagedReorderingHandlers : UICollectionViewDiffableDataSourceReorderingHandlers<NSNumber, NSString> {
			public override Func<NSString, bool>? CanReorderItemHandler { get; set; }
			public override Action<NSDiffableDataSourceTransaction<NSNumber, NSString>>? WillReorderHandler { get; set; }
		}

		[Test]
		public unsafe void ManagedOverrides ()
		{
			using var source = new UICollectionViewDiffableDataSourceSectionSnapshotHandlers<NSString> ();
			using var handlers = new ManagedSnapshotHandlers ();
			using var reorderingHandlers = new ManagedReorderingHandlers ();
			using var sourceReordering = new UICollectionViewDiffableDataSourceReorderingHandlers<NSNumber, NSString> ();
			using var item = new NSString ("item");
			using var snapshot = new NSDiffableDataSourceSectionSnapshot<NSString> ();
			Func<NSString, bool> predicate = value => value == item;
			Func<NSString, NSDiffableDataSourceSectionSnapshot<NSString>, NSDiffableDataSourceSectionSnapshot<NSString>> getSnapshot = (value, current) => {
				Assert.That (value, Is.SameAs (item), "Override item");
				Assert.That (current, Is.SameAs (snapshot), "Override snapshot");
				return snapshot;
			};
			source.ShouldExpandItemHandler = predicate;
			source.SnapshotForExpandingParentItemHandler = getSnapshot;
			Action<NSDiffableDataSourceTransaction<NSNumber, NSString>> willReorder = _ => { };
			sourceReordering.WillReorderHandler = willReorder;
			var reorderingBlock = Messaging.IntPtr_objc_msgSend (sourceReordering.Handle, Selector.GetHandle ("willReorderHandler"));
			Messaging.void_objc_msgSend_IntPtr (reorderingHandlers.Handle, Selector.GetHandle ("setWillReorderHandler:"), reorderingBlock);
			Assert.That (reorderingHandlers.WillReorderHandler, Is.SameAs (willReorder), "Nested generic setter");
			reorderingBlock = Messaging.IntPtr_objc_msgSend (reorderingHandlers.Handle, Selector.GetHandle ("willReorderHandler"));
			Messaging.void_objc_msgSend_IntPtr (sourceReordering.Handle, Selector.GetHandle ("setWillReorderHandler:"), reorderingBlock);
			Assert.That (sourceReordering.WillReorderHandler, Is.SameAs (willReorder), "Nested generic getter");
			var predicateBlock = Messaging.IntPtr_objc_msgSend (source.Handle, Selector.GetHandle ("shouldExpandItemHandler"));
			Messaging.void_objc_msgSend_IntPtr (handlers.Handle, Selector.GetHandle ("setShouldExpandItemHandler:"), predicateBlock);
			Messaging.void_objc_msgSend_IntPtr (reorderingHandlers.Handle, Selector.GetHandle ("setCanReorderItemHandler:"), predicateBlock);
			var snapshotBlock = Messaging.IntPtr_objc_msgSend (source.Handle, Selector.GetHandle ("snapshotForExpandingParentItemHandler"));
			Messaging.void_objc_msgSend_IntPtr (handlers.Handle, Selector.GetHandle ("setSnapshotForExpandingParentItemHandler:"), snapshotBlock);
			Assert.That (handlers.ShouldExpandItemHandler, Is.SameAs (predicate), "Override predicate setter");
			Assert.That (reorderingHandlers.CanReorderItemHandler, Is.SameAs (predicate), "Subset of generic arguments");
			Assert.That (handlers.SnapshotForExpandingParentItemHandler, Is.SameAs (getSnapshot), "Override snapshot setter");

			predicateBlock = Messaging.IntPtr_objc_msgSend (handlers.Handle, Selector.GetHandle ("shouldExpandItemHandler"));
			Assert.That (((BlockLiteral*) predicateBlock)->GetDelegateForBlock<PredicateInvoker> () (predicateBlock, item.Handle), Is.EqualTo ((byte) 1), "Override predicate getter");
			predicateBlock = Messaging.IntPtr_objc_msgSend (reorderingHandlers.Handle, Selector.GetHandle ("canReorderItemHandler"));
			Assert.That (((BlockLiteral*) predicateBlock)->GetDelegateForBlock<PredicateInvoker> () (predicateBlock, item.Handle), Is.EqualTo ((byte) 1), "Override subset getter");
			snapshotBlock = Messaging.IntPtr_objc_msgSend (handlers.Handle, Selector.GetHandle ("snapshotForExpandingParentItemHandler"));
			Assert.That (((BlockLiteral*) snapshotBlock)->GetDelegateForBlock<SnapshotInvoker> () (snapshotBlock, item.Handle, snapshot.Handle), Is.EqualTo (snapshot.Handle), "Override snapshot getter");
		}
	}
}
#endif
