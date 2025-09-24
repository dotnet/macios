using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Xamarin.Utils;
using Xunit;
using Xunit.Internal;
using Xunit.Sdk;
using Xunit.v3;

namespace Xamarin.Tests {

	[AttributeUsage (AttributeTargets.Method, AllowMultiple = true)]
	public sealed class PlatformInlineDataAttribute : DataAttribute {
		readonly object [] data;
		public PlatformInlineDataAttribute (ApplePlatform platform, params object [] parameters)
		{
			// data values are the join of the platform and all other values passed to the attr
			data = parameters.Prepend (platform).ToArray ();
			// based on the passed platform and the configuration, decide if we skip the test
			switch (platform) {
			case ApplePlatform.iOS:
				if (!Configuration.include_ios)
					Skip = "iOS is not included in this build";
				break;
			case ApplePlatform.TVOS:
				if (!Configuration.include_tvos)
					Skip = "tvOS is not included in this build";
				break;
			case ApplePlatform.MacOSX:
				if (!Configuration.include_mac)
					Skip = "macOS is not included in this build";
				break;
			case ApplePlatform.MacCatalyst:
				if (!Configuration.include_maccatalyst)
					Skip = "Mac Catalyst is not included in this build";
				break;
			default:
				throw new ArgumentOutOfRangeException ($"Unknown platform: {platform}");
			}
		}

		public override bool SupportsDiscoveryEnumeration () => true;

		public object [] Data {
			get { return data; }
		}

		public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData (MethodInfo testMethod, DisposalTracker disposalTracker)
		{
			var traits = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
			TestIntrospectionHelper.MergeTraitsInto(traits, Traits);
			return new([
				new TheoryDataRow (Data)
				{
					Explicit = ExplicitAsNullable,
					Label = Label,
					Skip = Skip,
					TestDisplayName = TestDisplayName,
					Timeout = TimeoutAsNullable,
					Traits = traits,
				}
			]);
			
		}

	}

	[AttributeUsage (AttributeTargets.Method, AllowMultiple = true)]
	public sealed class AllSupportedPlatformsAttribute : DataAttribute {

		readonly object [] dataValues;

		public AllSupportedPlatformsAttribute (params object [] parameters)
		{
			dataValues = parameters;
		}
		
		public override bool SupportsDiscoveryEnumeration () => true;
		
		public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
			MethodInfo testMethod,
			DisposalTracker disposalTracker)
		{
			var result = new List<ITheoryDataRow>();

			foreach (var platform in Configuration.GetIncludedPlatforms ()) {
				var row = dataValues.Prepend (platform).ToArray ();
				result.Add (ConvertDataRow (row));
			}

			return ValueTask.FromResult(result.CastOrToReadOnlyCollection());
		}
	}

	[AttributeUsage (AttributeTargets.Method, AllowMultiple = true)]
	public sealed class AllSupportedPlatformsClassDataAttribute<T> : DataAttribute where T : IEnumerable<object []> {
		readonly Type dataAttributeType;

		public AllSupportedPlatformsClassDataAttribute ()
		{
			dataAttributeType = typeof (T);
		}
		
		public override bool SupportsDiscoveryEnumeration() =>
			!typeof(IDisposable).IsAssignableFrom(dataAttributeType) && !typeof(IAsyncDisposable).IsAssignableFrom(dataAttributeType);
		
		/// <inheritdoc/>
		protected override ITheoryDataRow ConvertDataRow(object dataRow)
		{
			Guard.ArgumentNotNull(dataRow);

			try
			{
				return base.ConvertDataRow(dataRow);
			}
			catch (ArgumentException)
			{
				throw new ArgumentException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Class '{0}' yielded an item of type '{1}' which is not an 'object?[]', 'Xunit.ITheoryDataRow' or 'System.Runtime.CompilerServices.ITuple'",
						dataAttributeType.FullName,
						dataRow?.GetType().SafeName()
					),
					nameof(dataRow)
				);
			}
		}
		
		public override async ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
			MethodInfo testMethod,
			DisposalTracker disposalTracker)
		{
			var classInstance = Activator.CreateInstance (dataAttributeType);
			disposalTracker.Add(classInstance);

			if (classInstance is IAsyncLifetime classLifetime)
				await classLifetime.InitializeAsync();

			if (classInstance is IEnumerable<object[]> dataItems)
			{
				var result = new List<ITheoryDataRow>();

				foreach (var platform in Configuration.GetIncludedPlatforms ()) {
					foreach (var row in dataItems) {
						
						var platformRow = row.Prepend (platform).ToArray ();
						result.Add (ConvertDataRow (platformRow));
					}
				}

				return result.CastOrToReadOnlyCollection();
			}

			throw new ArgumentException(
				string.Format(
					CultureInfo.CurrentCulture,
					"'{0}' must implement one of the following interfaces to be used as ClassData:{1}- IEnumerable<object[]>{1}",
					dataAttributeType.FullName,
					Environment.NewLine
				)
			);
		}
	}

	public partial class Configuration {
		static string TestAssemblyDirectory {
			get {
				return Assembly.GetExecutingAssembly ().Location;
			}
		}

		public static bool IsEnabled (ApplePlatform platform)
			=> platform switch {
				ApplePlatform.iOS => include_ios,
				ApplePlatform.TVOS => include_tvos,
				ApplePlatform.MacCatalyst => include_maccatalyst,
				ApplePlatform.MacOSX => include_mac,
				_ => false
			};
	}
}
