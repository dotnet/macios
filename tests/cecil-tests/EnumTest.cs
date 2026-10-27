using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Mono.Cecil;

using Xamarin.Utils;

#nullable enable

namespace Cecil.Tests {

	[TestFixture]
	public class EnumTest {

		[TestCaseSource (typeof (Helper), nameof (Helper.NetPlatformAssemblyDefinitions))]
		public void AVErrorParsingCompatibility (AssemblyInfo info)
		{
			var type = info.Assembly.MainModule.GetType ("AVFoundation.AVError");
			var fileError = type.Fields.Single (v => v.Name == "FileFailedToParse");
			Assert.That (fileError.Constant, Is.EqualTo (-11829L), "FileFailedToParse");
			var dataError = type.Fields.Single (v => v.Name == "FailedToParse");
#if XAMCORE_5_0
			Assert.That (dataError.Constant, Is.EqualTo (-11853L), "FailedToParse");
			Assert.That (type.Fields.Any (v => v.Name == "FailedToParse2"), Is.False, "Legacy alias removed");
#else
			Assert.That (dataError.Constant, Is.EqualTo (-11829L), "Legacy FailedToParse");
			Assert.That (dataError.CustomAttributes.Any (v => v.AttributeType.Is ("System", "ObsoleteAttribute")), Is.True, "Legacy FailedToParse is obsolete");
			var browsable = dataError.CustomAttributes.Single (v => v.AttributeType.Is ("System.ComponentModel", "EditorBrowsableAttribute"));
			Assert.That (browsable.ConstructorArguments [0].Value, Is.EqualTo ((int) System.ComponentModel.EditorBrowsableState.Never), "Legacy FailedToParse is hidden");
			Assert.That (type.Fields.Single (v => v.Name == "FailedToParse2").Constant, Is.EqualTo (-11853L), "FailedToParse2");
#endif
		}

		[TestCaseSource (typeof (Helper), nameof (Helper.NetPlatformAssemblyDefinitions))]
		// https://github.com/dotnet/macios/issues/9724
		public void NoAvailabilityOnError (AssemblyInfo info)
		{
			var assembly = info.Assembly;
			HashSet<string> found = new HashSet<string> ();
			foreach (var type in assembly.MainModule.Types)
				NoAvailabilityOnError (type, found);

			Assert.That (found, Is.Empty, string.Join (", ", found));
		}

		bool IsErrorEnum (TypeDefinition type)
		{
			if (!type.IsEnum)
				return false;

			if (type.Name.EndsWith ("Error", StringComparison.Ordinal))
				return true;
			if (type.Name.EndsWith ("ErrorCode", StringComparison.Ordinal))
				return true;

			if (!type.HasCustomAttributes)
				return false;
			foreach (var ca in type.CustomAttributes) {
				if (ca.AttributeType.Name == "ErrorDomainAttribute")
					return true;
			}
			return false;
		}

		void NoAvailabilityOnError (TypeDefinition type, ISet<string> found)
		{
			if (type.HasNestedTypes) {
				foreach (var nt in type.NestedTypes)
					NoAvailabilityOnError (nt, found);
			}

			if (!IsErrorEnum (type))
				return;

			foreach (var f in type.Fields) {
				if (!f.HasCustomAttributes)
					continue;

				// If there are any ObsoletedOSPlatform attributes, then we must add other availability attributes for supported platforms,
				// because otherwise they're implicitly not supported. This means that we can't assert that there aren't any other
				// availability attributes if there's any ObsoletedOSPlatform attributes.
				var hasAnyObsoletedOSPlatformAttributes = f.CustomAttributes.Any (v => v.AttributeType.Name == "ObsoletedOSPlatformAttribute");
				if (hasAnyObsoletedOSPlatformAttributes)
					continue;

				foreach (var ca in f.CustomAttributes) {
					switch (ca.AttributeType.Name) {
					case "ObsoleteAttribute":
						// this is fine
						break;
					case "IntroducedAttribute":
					case "UnavailableAttribute":
					case "iOSAttribute":
					case "MacAttribute":
					case "TVAttribute":
					case "NoiOSAttribute":
					case "NoMacAttribute":
					case "NoTVAttribute":
					case "SupportedOSPlatformAttribute":
					case "UnsupportedOSPlatformAttribute":
						found.Add ($"{type.FullName}.{f.Name}: {ca.AttributeType.Name}");
						break;
					default:
						break;
					}
				}
			}
		}
	}
}
