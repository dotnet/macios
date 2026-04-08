//
// Resource Bundling Tests
//
// Authors:
//	Rolf Bjarne Kvinge (rolf@xamarin.com)
//
// Copyright 2022 Microsoft Corp. All rights reserved.
//

using System.IO;
using System.Resources;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace MonoTouchFixtures.ObjCRuntime {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ResourcesTest {

		[Test]
		public void Embedded ()
		{
			var manager = new ResourceManager ("monotouchtest.Welcome", typeof (ResourcesTest).Assembly);
			var englishAustralian = manager.GetString ("String1", new CultureInfo ("en-AU"));

			Assert.AreEqual ("Welcome!", manager.GetString ("String1", new CultureInfo ("en")), "en");
			if (TestRuntime.IsCoreCLR) {
				Assert.That (englishAustralian, Is.EqualTo ("G'day!").Or.EqualTo ("Welcome!"), "en-AU");
			} else {
				Assert.AreEqual ("G'day!", englishAustralian, "en-AU");
			}
			Assert.AreEqual ("Willkommen!", manager.GetString ("String1", new CultureInfo ("de")), "de");
			Assert.AreEqual ("Willkommen!", manager.GetString ("String1", new CultureInfo ("de-DE")), "de-DE");
			Assert.AreEqual ("Bienvenido!", manager.GetString ("String1", new CultureInfo ("es")), "es");
			Assert.AreEqual ("Bienvenido!", manager.GetString ("String1", new CultureInfo ("es-AR")), "es-AR");
			Assert.AreEqual ("Bienvenido!", manager.GetString ("String1", new CultureInfo ("es-ES")), "es-ES");
		}
	}
}
