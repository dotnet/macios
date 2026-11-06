// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Threading.Tasks;

#nullable enable

namespace SensorKit {
	public partial class SRSensorReader {
		/// <summary>Requests authorization to read data from the specified sensors.</summary>
		/// <param name="sensors">The sensors for which to request authorization.</param>
		/// <param name="completion">The handler invoked when the request completes, with an error if the request failed, or <see langword="null" /> otherwise.</param>
		/// <exception cref="ArgumentNullException"><paramref name="sensors" /> or <paramref name="completion" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentOutOfRangeException">A sensor does not have a native constant.</exception>
		public static void RequestAuthorization (SRSensor [] sensors, Action<NSError?> completion)
		{
			ArgumentNullException.ThrowIfNull (completion);

			using var sensorSet = CreateSensorSet (sensors);
			RequestAuthorization (sensorSet, completion);
		}

		/// <summary>Asynchronously requests authorization to read data from the specified sensors.</summary>
		/// <param name="sensors">The sensors for which to request authorization.</param>
		/// <returns>A task that completes when the authorization request completes.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="sensors" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentOutOfRangeException">A sensor does not have a native constant.</exception>
		public static Task RequestAuthorizationAsync (params SRSensor [] sensors)
		{
			using var sensorSet = CreateSensorSet (sensors);
			return RequestAuthorizationAsync (sensorSet);
		}

		static NSSet<NSString> CreateSensorSet (SRSensor [] sensors)
		{
			ArgumentNullException.ThrowIfNull (sensors);

			return new NSSet<NSString> (Array.ConvertAll (sensors, sensor => sensor.GetConstant () ?? throw new ArgumentOutOfRangeException (nameof (sensors))));
		}
	}
}
