// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;
using Mono.Cecil;
using Xamarin.Tests;

#nullable enable

namespace GeneratorTests {
	[TestFixture]
	[Parallelizable (ParallelScope.All)]
	public class IndexerTests : BGenBase {
		[TestCase (Profile.iOS)]
		[TestCase (Profile.tvOS)]
		[TestCase (Profile.macOSMobile)]
		[TestCase (Profile.MacCatalyst)]
		public void WrappedIndexers (Profile profile)
		{
			var bgen = BuildFile (profile, "indexers.cs");
			var type = bgen.ApiAssembly.MainModule.GetType ("IndexerTests.WrappedIndexers");
			var indexers = type.Properties.Where (p => p.Name == "Item").ToArray ();
			Assert.That (indexers.Length, Is.EqualTo (7), "Indexer overloads");
			Assert.That (type.CustomAttributes.Single (a => a.AttributeType.Name == "DefaultMemberAttribute").ConstructorArguments [0].Value, Is.EqualTo ("Item"), "Default member");
			Assert.That (type.Fields.Any (f => f.Name.Contains ("Item")), Is.False, "No indexer backing fields");

			var attributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName;
			bgen.AssertMethod (type.FullName, "get_Item", attributes, "System.Int32", "System.Int32");
			bgen.AssertMethod (type.FullName, "set_Item", attributes, "System.Void", "System.Int32", "System.Int32");
			bgen.AssertMethod (type.FullName, "get_Item", attributes, "Foundation.NSObject", "System.String");
			bgen.AssertMethod (type.FullName, "set_Item", attributes, "System.Void", "System.String", "Foundation.NSObject");
			bgen.AssertMethod (type.FullName, "get_Item", attributes, "System.String", "System.Int32", "System.Int32");
			bgen.AssertMethod (type.FullName, "set_Item", attributes, "System.Void", "System.Double", "System.Int32");
			bgen.AssertNoMethod (type.FullName, "get_Item", "System.Int32", "System.Double");

			var integerIndexer = indexers.Single (p => p.Parameters.Count == 1 && p.Parameters [0].ParameterType.FullName == "System.Int32");
			bgen.AssertApiCallsMethod (integerIndexer.GetMethod, "GetValue", "Getter forwarding");
			bgen.AssertApiCallsMethod (integerIndexer.SetMethod, "SetValue", "Setter forwarding");
			var paramsIndexer = indexers.Single (p => p.Parameters.Count == 1 && p.Parameters [0].ParameterType.IsArray);
			Assert.That (paramsIndexer.GetMethod.Parameters [0].CustomAttributes.Any (a => a.AttributeType.Name == "ParamArrayAttribute"), Is.True, "params getter");
			Assert.That (paramsIndexer.SetMethod.Parameters [0].CustomAttributes.Any (a => a.AttributeType.Name == "ParamArrayAttribute"), Is.True, "params setter");

			var derived = bgen.ApiAssembly.MainModule.GetType ("IndexerTests.DerivedIndexers");
			Assert.That (derived.Properties.Count (p => p.Name == "Item"), Is.EqualTo (2), "Derived indexers");
			bgen.AssertMethod (derived.FullName, "get_Item", attributes, "System.Int32", "System.Int32");
			bgen.AssertNoMethod (derived.FullName, "set_Item", "System.Void", "System.Int32", "System.Int32");

			var protocolClass = bgen.ApiAssembly.MainModule.GetType ("IndexerTests.ProtocolIndexers");
			Assert.That (protocolClass.Properties.Count (p => p.Name == "Item"), Is.EqualTo (3), "Injected protocol indexers");
			bgen.AssertMethod (protocolClass.FullName, "get_Item", attributes, "System.Int32", "System.Int32");
			bgen.AssertMethod (protocolClass.FullName, "get_Item", attributes, "System.Int32", "System.UIntPtr");
			bgen.AssertMethod (protocolClass.FullName, "get_Item", attributes, "System.Int32", "System.Char");

			Assert.That (bgen.TmpDirectory, Is.Not.Null, "Generated source directory");
			var source = File.ReadAllText (Path.Combine (bgen.TmpDirectory ?? throw new InvalidOperationException (), "IndexerTests", "WrappedIndexers.g.cs"));
			Assert.That (source, Does.Contain ("this [string? @event]"), "Nullable escaped key");
			Assert.That (source, Does.Contain ("NSObject? this"), "Nullable value");
			Assert.That (source, Does.Contain ("throw new NotImplementedException (\"Read-only indexer\")"), "NotImplemented setter");
			Assert.That (source, Does.Contain ("Gets or sets the value at the specified index."), "Indexer documentation");
		}

		[TestCase (Profile.iOS)]
		[TestCase (Profile.tvOS)]
		[TestCase (Profile.macOSMobile)]
		[TestCase (Profile.MacCatalyst)]
		public void MetalIndexers (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			using var assembly = AssemblyDefinition.ReadAssembly (Configuration.GetBaseLibrary (profile.AsPlatform ()));
			var arrays = new [] {
				("MTLRenderPipelineColorAttachmentDescriptorArray", "System.IntPtr"),
				("MTLRenderPassColorAttachmentDescriptorArray", "System.IntPtr"),
				("MTLVertexAttributeDescriptorArray", "System.IntPtr"),
				("MTLVertexBufferLayoutDescriptorArray", "System.IntPtr"),
				("MTLBufferLayoutDescriptorArray", "System.UIntPtr"),
				("MTLAttributeDescriptorArray", "System.UIntPtr"),
				("MTLPipelineBufferDescriptorArray", "System.UIntPtr"),
				("MTLTileRenderPipelineColorAttachmentDescriptorArray", "System.UIntPtr"),
				("MTLBlitPassSampleBufferAttachmentDescriptorArray", "System.UIntPtr"),
				("MTLComputePassSampleBufferAttachmentDescriptorArray", "System.UIntPtr"),
				("MTLRenderPassSampleBufferAttachmentDescriptorArray", "System.UIntPtr"),
				("MTLResourceStatePassSampleBufferAttachmentDescriptorArray", "System.UIntPtr"),
			};
			foreach (var (name, parameterType) in arrays) {
				var type = assembly.MainModule.GetType ("Metal." + name);
				var indexers = type.Properties.Where (p => p.Name == "Item").ToArray ();
				var excluded = (name == "MTLTileRenderPipelineColorAttachmentDescriptorArray" && profile != Profile.iOS && profile != Profile.MacCatalyst) ||
					(name == "MTLResourceStatePassSampleBufferAttachmentDescriptorArray" && profile == Profile.tvOS);
				Assert.That (indexers.Length, Is.EqualTo (excluded ? 0 : 1), $"{profile}: {name} indexer count");
				if (excluded)
					continue;
				var indexer = indexers.Single ();
				Assert.That (indexer.Parameters.Single ().ParameterType.FullName, Is.EqualTo (parameterType), $"{name} index parameter type");
				Assert.That (indexer.PropertyType.Name, Is.EqualTo (name.Substring (0, name.Length - "Array".Length)), $"{name} value type");
				Assert.That (indexer.GetMethod.IsPublic, Is.True, $"{name} public getter");
				Assert.That (indexer.SetMethod.IsPublic, Is.True, $"{name} public setter");
				Assert.That (indexer.GetMethod.IsVirtual, Is.False, $"{name} non-virtual getter");
				Assert.That (indexer.SetMethod.IsVirtual, Is.False, $"{name} non-virtual setter");
			}
		}

		[TestCase ("[Export (\"item:\")] int this [int index] { get; set; }")]
		[TestCase ("[Wrap (\"Values [index]\")] int this [int index] { get; set; }")]
		[TestCase ("[Field (\"items\", \"__Internal\")] int this [int index] { get; }")]
		[TestCase ("[Static] int this [int index] { [Wrap (\"index\")] get; }")]
		[TestCase ("int this [int index] { [Static] [Wrap (\"index\")] get; }")]
		[TestCase ("int this [int index] { [Wrap (\"index\")] get; set; }")]
		[TestCase ("int this [int index] { get; [Wrap (\"SetValue (value, index)\")] set; }")]
		[TestCase ("int this [int index] { [Wrap (\"index\")] get; [Export (\"setItem:\")] [Wrap (\"SetValue (value, index)\")] set; }")]
		public void InvalidIndexers (string declaration)
		{
			Configuration.IgnoreIfIgnoredPlatform (Profile.iOS.AsPlatform ());
			var bgen = new BGenTool {
				Profile = Profile.iOS,
			};
			bgen.CreateTemporaryBinding ($@"
using Foundation;
namespace IndexerTests {{
	[BaseType (typeof (NSObject))]
	interface InvalidIndexer {{
		{declaration}
	}}
}}");
			bgen.AssertExecuteError ("Invalid indexer");
			bgen.AssertError (1130, "The indexer 'IndexerTests.InvalidIndexer.Item' must be an instance member with [Wrap] on each accessor (or [NotImplemented] on the setter), and must not have [Export], [Field], or property-level [Wrap] attributes.");
		}
	}
}
