//
// GuidWrapper.cs: Support for treating C# Guids as UUids
//
// Author:
//   Miguel de Icaza (miguel@xamarin.com
//
// Copyright 2011-2014 Xamarin Inc
//
using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;
using CoreFoundation;

#nullable enable

namespace CoreBluetooth {

	internal static class CFUUID {

		// CFUUID.h
		[DllImport (Constants.CoreFoundationLibrary)]
		public extern static /* CFUUIDRef */ IntPtr CFUUIDCreateFromString ( /* CFAllocatorRef */ IntPtr alloc, /* CFStringRef */ IntPtr uuidStr);
	}

	public partial class CBCentralManager {

		/// <include file="../../docs/api/CoreBluetooth/CBCentralManager.xml" path="/Documentation/Docs[@DocId='M:CoreBluetooth.CBCentralManager.ConnectPeripheral(CoreBluetooth.CBPeripheral,CoreBluetooth.PeripheralConnectionOptions)']/*" />
		public void ConnectPeripheral (CBPeripheral peripheral, PeripheralConnectionOptions? options = null)
		{
			ConnectPeripheral (peripheral, options?.Dictionary);
		}

		/// <summary>Scans for peripherals that are advertising any of the specified <paramref name="peripheralUuids" /> with the specified <paramref name="options" />.</summary>
		/// <param name="peripheralUuids">An array of advertised UUIDs to scan for.</param>
		/// <param name="options">Any custom options for the scan.</param>
		public void ScanForPeripherals (CBUUID []? peripheralUuids, NSDictionary? options)
		{
			if (peripheralUuids is null)
				ScanForPeripherals ((NSArray?) null, options);
			else
				ScanForPeripherals (NSArray.FromObjects (peripheralUuids), options);
		}

		/// <summary>Scans for peripherals that are advertising any of the specified <paramref name="peripheralUuids" /> with the specified <paramref name="options" />.</summary>
		/// <param name="peripheralUuids">An array of advertised UUIDs to scan for.</param>
		/// <param name="options">Any custom options for the scan.</param>
		public void ScanForPeripherals (CBUUID []? peripheralUuids, PeripheralScanningOptions? options = null)
		{
			ScanForPeripherals (peripheralUuids, options?.Dictionary);
		}

		/// <summary>Scans for peripherals that are advertising any of the specified <paramref name="peripheralUuids" />.</summary>
		/// <param name="peripheralUuids">An array of advertised UUIDs to scan for.</param>
		public void ScanForPeripherals (CBUUID []? peripheralUuids)
		{
			ScanForPeripherals (peripheralUuids, null as NSDictionary);
		}

		/// <summary>Scans for peripherals that are advertising the specified <paramref name="serviceUuid" /> with the specified <paramref name="options" />.</summary>
		/// <param name="serviceUuid">The advertised UUID to scan for.</param>
		/// <param name="options">Any custom options for the scan.</param>
		public void ScanForPeripherals (CBUUID serviceUuid, NSDictionary? options)
		{
			ScanForPeripherals (new [] { serviceUuid }, options);
		}

		/// <summary>Scans for peripherals that are advertising the specified <paramref name="serviceUuid" />.</summary>
		/// <param name="serviceUuid">The advertised UUID to scan for.</param>
		public void ScanForPeripherals (CBUUID serviceUuid)
		{
			ScanForPeripherals (new [] { serviceUuid }, null as NSDictionary);
		}
	}

	public partial class CBPeripheral {

		/// <summary>Discovers all available services.</summary>
		///         <remarks>To be added.</remarks>
		public void DiscoverServices ()
		{
			DiscoverServices ((NSArray?) null);
		}

		/// <param name="services">To be added.</param>
		///         <summary>Discovers the specified <paramref name="services" />.</summary>
		///         <remarks>To be added.</remarks>
		public void DiscoverServices (CBUUID []? services)
		{
			if (services is null)
				DiscoverServices ((NSArray?) null);
			else
				DiscoverServices (NSArray.FromObjects (services));
		}

		/// <param name="includedServiceUUIDs">To be added.</param>
		///         <param name="forService">To be added.</param>
		///         <summary>Discovers the included services in <paramref name="forService" /> that are of the service types that are identified by the UUIDs in <paramref name="includedServiceUUIDs" />.</summary>
		///         <remarks>To be added.</remarks>
		public void DiscoverIncludedServices (CBUUID []? includedServiceUUIDs, CBService forService)
		{
			if (includedServiceUUIDs is null)
				DiscoverIncludedServices ((NSArray?) null, forService);
			else
				DiscoverIncludedServices (NSArray.FromObjects (includedServiceUUIDs), forService);
		}

		/// <param name="forService">Service that you want to discover all the characteristics for.</param>
		///         <summary>Discover all characteristics in a service (slow).</summary>
		///         <remarks>
		///           <para>
		/// 	    When the characteristics are discovered, the event
		/// 	    DiscoverCharacteristic is raised (or alternatively, if you
		/// 	    set a Delegate, the method DiscoverCharacteristic on the
		/// 	    delegate is invoked with the results).
		/// 	  </para>
		///           <para>
		/// 	    Once the characterstics have been discovered, they are
		/// 	    available on the <see cref="P:CoreBluetooth.CBService.Characteristics" />
		/// 	    property.
		/// 	  </para>
		///           <para>
		/// 	    This method is potentially slow and will return all the
		/// 	    characteristics supported by the service.  Ideally, you
		/// 	    should use the overload that allows you to specifify an
		/// 	    array of CBUUIDs as that will be faster.
		/// 	  </para>
		///         </remarks>
		public void DiscoverCharacteristics (CBService forService)
		{
			DiscoverCharacteristics ((NSArray?) null, forService);
		}

		/// <param name="charactersticUUIDs">Array of CBUUIDs containing the characteristics that you are probing for.</param>
		///         <param name="forService">Service that you want to discover the characteristics for.</param>
		///         <summary>Discovers the list of characteristics in the specified service.</summary>
		///         <remarks>
		///           <para>
		/// 	    When the characteristics are discovered, the event
		/// 	    DiscoverCharacteristic is raised (or alternatively, if you
		/// 	    set a Delegate, the method DiscoverCharacteristic on the
		/// 	    delegate is invoked with the results).
		/// 	  </para>
		///           <para>
		/// 	    Once the characterstics have been discovered, they are
		/// 	    available on the <see cref="P:CoreBluetooth.CBService.Characteristics" />
		/// 	    property.
		/// 	  </para>
		///         </remarks>
		public void DiscoverCharacteristics (CBUUID []? charactersticUUIDs, CBService forService)
		{
			if (charactersticUUIDs is null)
				DiscoverCharacteristics ((NSArray?) null, forService);
			else
				DiscoverCharacteristics (NSArray.FromObjects (charactersticUUIDs), forService);
		}
	}
}
