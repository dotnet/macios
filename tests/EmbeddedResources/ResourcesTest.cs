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
			Assert.That (manager.GetString ("String1", new CultureInfo ("en")), Is.EqualTo ("Welcome"), "en");
			Assert.That (manager.GetString ("String1", new CultureInfo ("en-AU")), Is.EqualTo ("G'day"), "en-AU");
			Assert.That (manager.GetString ("String1", new CultureInfo ("de")), Is.EqualTo ("Willkommen"), "de");
			Assert.That (manager.GetString ("String1", new CultureInfo ("de-DE")), Is.EqualTo ("Willkommen"), "de-DE");
			Assert.That (manager.GetString ("String1", new CultureInfo ("es")), Is.EqualTo ("Bienvenido"), "es");
			Assert.That (manager.GetString ("String1", new CultureInfo ("es-AR")), Is.EqualTo ("Bienvenido"), "es-AR");
			Assert.That (manager.GetString ("String1", new CultureInfo ("es-ES")), Is.EqualTo ("Bienvenido"), "es-ES");
		}
	}
}
