using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Mono.Cecil;
using Mono.Cecil.Cil;

using ObjCRuntime;

using Xamarin.Tests;
using Xamarin.Utils;

#nullable enable

namespace Cecil.Tests {
	public partial class ApiAvailabilityTest {
		static HashSet<string> knownFailuresAvailabilityWarnings = new HashSet<string> {
		};
	}
}
