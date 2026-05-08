//
// Resource Bundling Tests
//
// Authors:
//	Rolf Bjarne Kvinge (rolf@xamarin.com)
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

using System.IO;
using System.Resources;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace EmbeddedResources {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ResourcesTest {

		[Test]
		public void Embedded ()
		{
			var manager = new ResourceManager ("EmbeddedResources.Welcome", typeof (ResourcesTest).Assembly);
			ClassicAssert.AreEqual ("Welcome", manager.GetString ("String1", new CultureInfo ("en")), "en");
			ClassicAssert.AreEqual ("G'day", manager.GetString ("String1", new CultureInfo ("en-AU")), "en-AU");
			ClassicAssert.AreEqual ("Willkommen", manager.GetString ("String1", new CultureInfo ("de")), "de");
			ClassicAssert.AreEqual ("Willkommen", manager.GetString ("String1", new CultureInfo ("de-DE")), "de-DE");
			ClassicAssert.AreEqual ("Bienvenido", manager.GetString ("String1", new CultureInfo ("es")), "es");
			ClassicAssert.AreEqual ("Bienvenido", manager.GetString ("String1", new CultureInfo ("es-AR")), "es-AR");
			ClassicAssert.AreEqual ("Bienvenido", manager.GetString ("String1", new CultureInfo ("es-ES")), "es-ES");
		}
	}
}
