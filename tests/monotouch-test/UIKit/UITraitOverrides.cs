//
// Unit tests for UITraitOverrides
//
// Authors:
//	Rolf Bjarne Kvinge  <rolf@xamarin.com>
//
// Copyright 2023 Microsoft Corp. All rights reserved.
//

#if !__MACOS__

using System.IO;
using System.Reflection;
using System.Threading;

using UIKit;

using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UITraitOverridesTest {

		[Test]
		public void InlinedInUIViewController ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			IUITraitChangeRegistration token;
			using var obj = new NSObject ();
			using var observable = new UIViewController ();

			// RegisterForTraitChanges (Type [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges (new [] { typeof (UITraitVerticalSizeClass) }, (a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Action<IUITraitEnvironment, UITraitCollection> handler, params Type [] traits)
			token = observable.RegisterForTraitChanges ((a, b) => { }, typeof (UITraitVerticalSizeClass), typeof (UITraitHorizontalSizeClass));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T1, T2> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass, UITraitHorizontalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T1, T2, T3> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass, UITraitHorizontalSizeClass, UITraitHorizontalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T1, T2, T3, T4> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass, UITraitHorizontalSizeClass, UITraitHorizontalSizeClass, UITraitVerticalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Type [] traits, NSObject target, Selector action)
			token = observable.RegisterForTraitChanges (new [] { typeof (UITraitVerticalSizeClass), typeof (UITraitHorizontalSizeClass) }, obj, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Type [] traits, Selector action)
			token = observable.RegisterForTraitChanges (new [] { typeof (UITraitVerticalSizeClass), typeof (UITraitHorizontalSizeClass) }, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Class [] traits, [BlockProxy (typeof (ObjCRuntime.Trampolines.IUITraitChangeObservable_RegisterForTraitChanges_NIDAction))] global::System.Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges (new [] { new Class (typeof (UITraitVerticalSizeClass)) }, (a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Class [] traits, NSObject target, Selector action)
			token = observable.RegisterForTraitChanges (new [] { new Class (typeof (UITraitVerticalSizeClass)) }, obj, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Class [] traits, Selector action)
			token = observable.RegisterForTraitChanges (new [] { new Class (typeof (UITraitVerticalSizeClass)) }, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);
		}

		[Test]
		public void InlinedInUIView ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			IUITraitChangeRegistration token;
			using var obj = new NSObject ();
			using var observable = new UIView ();

			// RegisterForTraitChanges (Type [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges (new [] { typeof (UITraitVerticalSizeClass) }, (a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Action<IUITraitEnvironment, UITraitCollection> handler, params Type [] traits)
			token = observable.RegisterForTraitChanges ((a, b) => { }, typeof (UITraitVerticalSizeClass), typeof (UITraitHorizontalSizeClass));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T1, T2> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass, UITraitHorizontalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T1, T2, T3> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass, UITraitHorizontalSizeClass, UITraitHorizontalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T1, T2, T3, T4> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass, UITraitHorizontalSizeClass, UITraitHorizontalSizeClass, UITraitVerticalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Type [] traits, NSObject target, Selector action)
			token = observable.RegisterForTraitChanges (new [] { typeof (UITraitVerticalSizeClass), typeof (UITraitHorizontalSizeClass) }, obj, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Type [] traits, Selector action)
			token = observable.RegisterForTraitChanges (new [] { typeof (UITraitVerticalSizeClass), typeof (UITraitHorizontalSizeClass) }, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Class [] traits, [BlockProxy (typeof (ObjCRuntime.Trampolines.IUITraitChangeObservable_RegisterForTraitChanges_NIDAction))] global::System.Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges (new [] { new Class (typeof (UITraitVerticalSizeClass)) }, (a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Class [] traits, NSObject target, Selector action)
			token = observable.RegisterForTraitChanges (new [] { new Class (typeof (UITraitVerticalSizeClass)) }, obj, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Class [] traits, Selector action)
			token = observable.RegisterForTraitChanges (new [] { new Class (typeof (UITraitVerticalSizeClass)) }, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);
		}

		[Test]
		public void InlinedInUIWindowScene ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			IUITraitChangeRegistration token;
			using var obj = new NSObject ();
			UIWindowScene observable = UIApplication.SharedApplication.Windows [0].WindowScene;

			// RegisterForTraitChanges (Type [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges (new [] { typeof (UITraitVerticalSizeClass) }, (a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Action<IUITraitEnvironment, UITraitCollection> handler, params Type [] traits)
			token = observable.RegisterForTraitChanges ((a, b) => { }, typeof (UITraitVerticalSizeClass), typeof (UITraitHorizontalSizeClass));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T1, T2> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass, UITraitHorizontalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T1, T2, T3> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass, UITraitHorizontalSizeClass, UITraitHorizontalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T1, T2, T3, T4> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass, UITraitHorizontalSizeClass, UITraitHorizontalSizeClass, UITraitVerticalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Type [] traits, NSObject target, Selector action)
			token = observable.RegisterForTraitChanges (new [] { typeof (UITraitVerticalSizeClass), typeof (UITraitHorizontalSizeClass) }, obj, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Type [] traits, Selector action)
			token = observable.RegisterForTraitChanges (new [] { typeof (UITraitVerticalSizeClass), typeof (UITraitHorizontalSizeClass) }, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Class [] traits, [BlockProxy (typeof (ObjCRuntime.Trampolines.IUITraitChangeObservable_RegisterForTraitChanges_NIDAction))] global::System.Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges (new [] { new Class (typeof (UITraitVerticalSizeClass)) }, (a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Class [] traits, NSObject target, Selector action)
			token = observable.RegisterForTraitChanges (new [] { new Class (typeof (UITraitVerticalSizeClass)) }, obj, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Class [] traits, Selector action)
			token = observable.RegisterForTraitChanges (new [] { new Class (typeof (UITraitVerticalSizeClass)) }, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);
		}

		[Test]
		public void InlinedInUIPresentationController ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			IUITraitChangeRegistration token;
			using var obj = new NSObject ();
			using var vc = new UIViewController ();
			using var observable = new UIPresentationController (vc, null);

			// RegisterForTraitChanges (Type [] traits, Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges (new [] { typeof (UITraitVerticalSizeClass) }, (a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Action<IUITraitEnvironment, UITraitCollection> handler, params Type [] traits)
			token = observable.RegisterForTraitChanges ((a, b) => { }, typeof (UITraitVerticalSizeClass), typeof (UITraitHorizontalSizeClass));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T1, T2> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass, UITraitHorizontalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T1, T2, T3> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass, UITraitHorizontalSizeClass, UITraitHorizontalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges<T1, T2, T3, T4> (Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges<UITraitVerticalSizeClass, UITraitHorizontalSizeClass, UITraitHorizontalSizeClass, UITraitVerticalSizeClass> ((a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Type [] traits, NSObject target, Selector action)
			token = observable.RegisterForTraitChanges (new [] { typeof (UITraitVerticalSizeClass), typeof (UITraitHorizontalSizeClass) }, obj, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Type [] traits, Selector action)
			token = observable.RegisterForTraitChanges (new [] { typeof (UITraitVerticalSizeClass), typeof (UITraitHorizontalSizeClass) }, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Class [] traits, [BlockProxy (typeof (ObjCRuntime.Trampolines.IUITraitChangeObservable_RegisterForTraitChanges_NIDAction))] global::System.Action<IUITraitEnvironment, UITraitCollection> handler)
			token = observable.RegisterForTraitChanges (new [] { new Class (typeof (UITraitVerticalSizeClass)) }, (a, b) => { });
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Class [] traits, NSObject target, Selector action)
			token = observable.RegisterForTraitChanges (new [] { new Class (typeof (UITraitVerticalSizeClass)) }, obj, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);

			// RegisterForTraitChanges (Class [] traits, Selector action)
			token = observable.RegisterForTraitChanges (new [] { new Class (typeof (UITraitVerticalSizeClass)) }, new Selector ("notifyTraitChange:collection:"));
			observable.UnregisterForTraitChanges (token);
		}

		[Test]
		public void RegisterForTraitChanges_ClassArray ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			CallbackTest ((vc, callback) => {
				return vc.RegisterForTraitChanges (new [] {
						new Class (typeof (UITraitVerticalSizeClass)),
						new Class (typeof (UITraitHorizontalSizeClass)),
						}, callback);
			}, "ClassArray ");
		}

		[Test]
		public void RegisterForTraitChanges_TypeArray ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			CallbackTest ((vc, callback) => {
				return vc.RegisterForTraitChanges (new [] {
						typeof (UITraitVerticalSizeClass),
						typeof (UITraitHorizontalSizeClass),
						}, callback);
			}, "TypeArray ");
		}

		[Test]
		public void RegisterForTraitChanges_ParamsTypeArray ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			CallbackTest ((vc, callback) => {
				return vc.RegisterForTraitChanges (callback,
						typeof (UITraitVerticalSizeClass),
						typeof (UITraitHorizontalSizeClass));
			}, "ParamsTypeArray ");
		}

		[Test]
		public void RegisterForTraitChanges_Generic ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			CallbackTest ((vc, callback) => {
				// return vc.RegisterForTraitChanges<NSObject> (callback);
				return vc.RegisterForTraitChanges<UITraitHorizontalSizeClass, UITraitVerticalSizeClass> (callback);
			}, "Generic ");
		}

		void CallbackTest (Func<IUITraitChangeObservable, Action<IUITraitEnvironment, UITraitCollection>, IUITraitChangeRegistration> registerFunc, string prefix)
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			using var vc = new UIViewController ();
			using var label = new UILabel () {
				TextAlignment = UITextAlignment.Center,
			};
			vc.View!.AddSubview (label);

			UIUserInterfaceSizeClass? horizontal = null;
			UIUserInterfaceSizeClass? vertical = null;
			var msgPrefix = "?";
			var callbackCounter = 0;
			Exception? ex = null;

			Action<IUITraitEnvironment, UITraitCollection> callback = (env, coll) => {
				try {
					Assert.That (vc.TraitOverrides.ContainsTrait<UITraitHorizontalSizeClass> (), Is.EqualTo (horizontal is not null), $"{prefix}{msgPrefix} Horizontal A");
					Assert.That (vc.TraitOverrides.ContainsTrait (typeof (UITraitHorizontalSizeClass)), Is.EqualTo (horizontal is not null), $"{prefix}{msgPrefix} Horizontal B");
					Assert.That (vc.TraitOverrides.ContainsTrait (new Class (typeof (UITraitHorizontalSizeClass))), Is.EqualTo (horizontal is not null), $"{prefix}{msgPrefix} Horizontal C");
					Assert.That (vc.TraitOverrides.ContainsTrait<UITraitVerticalSizeClass> (), Is.EqualTo (vertical is not null), $"{prefix}{msgPrefix} Vertical A");
					Assert.That (vc.TraitOverrides.ContainsTrait (typeof (UITraitVerticalSizeClass)), Is.EqualTo (vertical is not null), $"{prefix}{msgPrefix} Vertical B");
					Assert.That (vc.TraitOverrides.ContainsTrait (new Class (typeof (UITraitVerticalSizeClass))), Is.EqualTo (vertical is not null), $"{prefix}{msgPrefix} Vertical C");
					if (horizontal is not null) {
						Assert.That (vc.TraitOverrides.HorizontalSizeClass, Is.EqualTo (horizontal.Value), $"{prefix}{msgPrefix} Horizontal Value");
					}
					if (vertical is not null) {
						Assert.That (vc.TraitOverrides.VerticalSizeClass, Is.EqualTo (vertical.Value), $"{prefix}{msgPrefix} Vertical Value");
					}
					callbackCounter++;
				} catch (Exception e) {
					ex = e;
				}
			};

			// Figure out the initial values so that we can choose a different value when setting them.
			var tc = UITraitCollection.CurrentTraitCollection;
			UIUserInterfaceSizeClass? initialHorizonal = null;
			UIUserInterfaceSizeClass? initialVertical = null;
			tc.PerformAsCurrentTraitCollection (() => {
				initialHorizonal = tc.HorizontalSizeClass;
				initialVertical = tc.VerticalSizeClass;
			});

			var firstHorizontal = (initialHorizonal == UIUserInterfaceSizeClass.Regular) ? UIUserInterfaceSizeClass.Compact : UIUserInterfaceSizeClass.Regular;
			var firstVertical = (initialVertical == UIUserInterfaceSizeClass.Regular) ? UIUserInterfaceSizeClass.Compact : UIUserInterfaceSizeClass.Regular;
			var secondHorizontal = (initialHorizonal == UIUserInterfaceSizeClass.Regular) ? UIUserInterfaceSizeClass.Regular : UIUserInterfaceSizeClass.Compact;
			var secondVertical = (initialVertical == UIUserInterfaceSizeClass.Regular) ? UIUserInterfaceSizeClass.Regular : UIUserInterfaceSizeClass.Compact;

			var token = registerFunc (vc, callback);

			horizontal = firstHorizontal;
			vc.TraitOverrides.HorizontalSizeClass = horizontal.Value;
			Assert.That (callbackCounter, Is.EqualTo (1), $"{prefix}CallbackCounter 1");
			Assert.That (ex, Is.Null, $"{prefix}Exception 1");

			horizontal = secondHorizontal;
			vc.TraitOverrides.HorizontalSizeClass = horizontal.Value;
			Assert.That (callbackCounter, Is.EqualTo (2), $"{prefix}CallbackCounter 2");
			Assert.That (ex, Is.Null, $"{prefix}Exception 2");

			vertical = firstVertical;
			vc.TraitOverrides.VerticalSizeClass = vertical.Value;
			Assert.That (callbackCounter, Is.EqualTo (3), $"{prefix}CallbackCounter 3");
			Assert.That (ex, Is.Null, $"{prefix}Exception 3");

			vertical = secondVertical;
			vc.TraitOverrides.VerticalSizeClass = vertical.Value;
			Assert.That (callbackCounter, Is.EqualTo (4), $"{prefix}CallbackCounter 4");
			Assert.That (ex, Is.Null, $"{prefix}Exception 4");

			vertical = null;
			vc.TraitOverrides.RemoveTrait (typeof (UITraitVerticalSizeClass));
			Assert.That (callbackCounter, Is.EqualTo (4), $"{prefix}CallbackCounter 5");
			Assert.That (ex, Is.Null, $"{prefix}Exception 5");

			horizontal = null;
			vc.TraitOverrides.RemoveTrait<UITraitHorizontalSizeClass> ();
			Assert.That (callbackCounter, Is.EqualTo (4), $"{prefix}CallbackCounter 6");
			Assert.That (ex, Is.Null, $"{prefix}Exception 6");

			horizontal = firstHorizontal;
			vc.TraitOverrides.HorizontalSizeClass = horizontal.Value;
			Assert.That (callbackCounter, Is.EqualTo (5), $"{prefix}CallbackCounter 7");
			Assert.That (ex, Is.Null, $"{prefix}Exception 7");

			horizontal = null;
			vc.TraitOverrides.RemoveTrait (new Class (typeof (UITraitHorizontalSizeClass)));
			Assert.That (callbackCounter, Is.EqualTo (5), $"{prefix}CallbackCounter 8");
			Assert.That (ex, Is.Null, $"{prefix}Exception 8");

			vc.UnregisterForTraitChanges (token);
		}
	}
}

#endif // !__MACOS__
