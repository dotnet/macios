// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if !MONOMAC

using UIKit;

namespace MonoTouchFixtures.UIKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UIContentConfigurationTest {
		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (12, 0);
		}

		[Test]
		public void NativeContentView ()
		{
			using var configuration = UIListContentConfiguration.ValueCellConfiguration;
			using var contentView = configuration.MakeContentView (out var view);
			using (view) {
				Assert.That (view.Handle, Is.EqualTo (contentView.Handle), "Handle");
				using var container = new UIView ();
				container.AddSubview (view);
				Assert.That (view.Superview, Is.SameAs (container), "Superview");

				using var updatedConfiguration = UIListContentConfiguration.ValueCellConfiguration;
				updatedConfiguration.Text = "Updated content";
				contentView.Configuration = updatedConfiguration;
				using var protocolConfiguration = contentView.Configuration;
				using var actualConfiguration = Runtime.GetNSObject<UIListContentConfiguration> (protocolConfiguration.Handle, false);
				Assert.That (actualConfiguration?.Text, Is.EqualTo ("Updated content"), "Configuration text");
			}
		}

		[Test]
		public void DisposeProtocolWrapper ()
		{
			using var configuration = UIListContentConfiguration.ValueCellConfiguration;
			using var contentView = configuration.MakeContentView (out var view);
			using (view) {
				if (ReferenceEquals (contentView, view))
					Assert.Ignore ("The content view already has a shared managed wrapper.");
				contentView.Dispose ();
				view.Alpha = 0.5f;
				Assert.That (view.Alpha, Is.EqualTo ((nfloat) 0.5f), "View remains usable");
			}
		}

		[Test]
		public void DisposeViewWrapper ()
		{
			using var configuration = UIListContentConfiguration.ValueCellConfiguration;
			using var contentView = configuration.MakeContentView (out var view);
			using (view) {
				if (ReferenceEquals (contentView, view))
					Assert.Ignore ("The content view already has a shared managed wrapper.");
				view.Dispose ();
				using var actualConfiguration = contentView.Configuration;
				Assert.That (actualConfiguration.Handle, Is.Not.EqualTo (NativeHandle.Zero), "Protocol remains usable");
			}
		}

		[Test]
		public void ManagedContentViewIdentity ()
		{
			using var configuration = new ManagedConfiguration ();
			using var expectedView = configuration.ContentView;
			using var contentView = configuration.MakeContentView (out var view);
			Assert.That (contentView, Is.SameAs (expectedView), "Protocol wrapper");
			Assert.That (view, Is.SameAs (expectedView), "View wrapper");
			Assert.That (configuration.MakeContentViewCalls, Is.EqualTo (1), "Factory calls");
		}

		[Test]
		public void NullConfiguration ()
		{
#pragma warning disable CS8625 // Verify the runtime null check.
			Assert.Throws<ArgumentNullException> (() => UIContentConfiguration_Extensions.MakeContentView (null, out _));
#pragma warning restore CS8625
		}

		[Test]
		public void DisposedConfiguration ()
		{
			using var configuration = new ManagedConfiguration ();
			using var contentView = configuration.ContentView;
			configuration.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => configuration.MakeContentView (out _));
			Assert.That (configuration.MakeContentViewCalls, Is.Zero, "Factory calls");
		}

		[Test]
		public void DisposedContentView ()
		{
			using var configuration = new ManagedConfiguration ();
			using var contentView = configuration.ContentView;
			contentView.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => configuration.MakeContentView (out _));
			Assert.That (configuration.MakeContentViewCalls, Is.EqualTo (1), "Factory calls");
		}

		class ManagedConfiguration : NSObject, IUIContentConfiguration {
			public ManagedConfiguration ()
			{
				ContentView = new ManagedContentView (this);
			}

			public ManagedContentView ContentView { get; }
			public int MakeContentViewCalls { get; private set; }

			public IUIContentView MakeContentView ()
			{
				MakeContentViewCalls++;
				return ContentView;
			}

			public IUIContentConfiguration GetUpdatedConfiguration (IUIConfigurationState state)
			{
				return this;
			}

			public NSObject Copy (NSZone? zone)
			{
				DangerousRetain ();
				return this;
			}
		}

		class ManagedContentView : UIView, IUIContentView {
			public ManagedContentView (IUIContentConfiguration configuration)
			{
				Configuration = configuration;
			}

			public IUIContentConfiguration Configuration { get; set; }
		}
	}
}

#endif
