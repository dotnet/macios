// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.Macios.Generator.Availability;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.Macios.Generator.Tests;

/// <summary>
/// Provides a set of custom assertion methods that can be used with xUnit, including support for multiple assertion scopes.
/// </summary>
public class AssertEx {
	[ThreadStatic]
	static List<Exception>? _exceptions;

	/// <summary>
	/// Enters a scope where multiple assertions can be made without failing fast.
	/// All assertion failures are collected and reported at the end of the scope.
	/// </summary>
	/// <returns>An <see cref="IDisposable"/> that ends the multiple assertion scope when disposed.</returns>
	public static IDisposable EnterMultipleScope ()
	{
		// We could probably support nesting, but for now let's keep it simple.
		if (_exceptions is not null)
			throw new InvalidOperationException ("A multiple assertion scope is already active.");

		_exceptions = new ();
		return new MultipleAssertionScope ();
	}

	class MultipleAssertionScope : IDisposable {
		public void Dispose ()
		{
			var exceptions = _exceptions;
			_exceptions = null;

			if (exceptions is not null && exceptions.Count > 0) {
				if (exceptions.Count == 1) {
					// We're not using ExceptionDispatchInfo.Capture(exceptions[0]).Throw()
					// because we want to preserve the original stack trace, and we're not
					// crossing any thread boundaries.
					throw exceptions [0];
				}

				throw new AggregateException (exceptions);
			}
		}
	}

	/// <summary>
	/// Executes a series of assertions and collects all failures, throwing an <see cref="AggregateException"/> if one or more assertions fail.
	/// </summary>
	/// <param name="asserts">The action containing the assertions to execute.</param>
	public static void Multiple (Action asserts)
	{
		ArgumentNullException.ThrowIfNull (asserts);

		var exceptions = new List<Exception> ();
		try {
			asserts ();
		} catch (Exception ex) {
			exceptions.Add (ex);
		}

		if (exceptions.Count > 0) {
			if (exceptions.Count == 1)
				throw exceptions [0];
			throw new AggregateException (exceptions);
		}
	}

	/// <summary>
	/// Verifies that two objects are equal. If inside a multiple assertion scope, it collects failures instead of throwing immediately.
	/// </summary>
	/// <typeparam name="T">The type of the objects to be compared.</typeparam>
	/// <param name="expected">The expected value.</param>
	/// <param name="actual">The value to be compared against.</param>
	/// <param name="comparer">Optional comparer to use for the equality.</param>
	public static void Equal<T> (T expected, T actual, IEqualityComparer<T>? comparer = null)
	{
		if (_exceptions is null) {
			Assert.Equal (expected, actual, comparer ??  EqualityComparer<T>.Default);
			return;
		}

		try {
			Assert.Equal (expected, actual, comparer ??  EqualityComparer<T>.Default);
		} catch (EqualException ex) {
			_exceptions.Add (ex);
		}
	}
	
	/// <summary>
	/// Verifies that a condition is true. If inside a multiple assertion scope, it collects failures instead of throwing immediately.
	/// </summary>
	/// <param name="condition">The condition to be evaluated.</param>
	public static void True (bool condition)
	{
		if (_exceptions is null) {
			Assert.True (condition);
			return;
		}

		try {
			Assert.True (condition);
		} catch (TrueException ex) {
			_exceptions.Add (ex);
		}
	}

	/// <summary>
	/// Verifies that two <see cref="PlatformAvailability"/> instances are equal by comparing their properties within a multiple assertion scope.
	/// </summary>
	/// <param name="expected">The expected platform availability.</param>
	/// <param name="actual">The actual platform availability.</param>
	internal static void Equal (PlatformAvailability expected, PlatformAvailability actual)
	{
		var obsoleteComparer = new DictionaryComparer<Version, (string?, string?)> ();
		var unsupportedComparer = new DictionaryComparer<Version, string?> ();
		
		// use a MultipleAssertionScope to test all the diff fields of the struct
		using (EnterMultipleScope ()) {
			Equal (expected.Platform, actual.Platform);
			Equal (expected.SupportedVersion, actual.SupportedVersion);
			True (unsupportedComparer.Equals (expected.UnsupportedVersions, actual.UnsupportedVersions));
			True (obsoleteComparer.Equals (expected.ObsoletedVersions, actual.ObsoletedVersions));
		}
	}

	/// <summary>
	/// Verifies that two <see cref="SymbolAvailability"/> instances are equal by comparing the availability for each supported platform within a multiple assertion scope.
	/// </summary>
	/// <param name="expected">The expected symbol availability.</param>
	/// <param name="actual">The actual symbol availability.</param>
	internal static void Equal (SymbolAvailability expected, SymbolAvailability actual)
	{
		using (EnterMultipleScope ()) {
			// compare each of the platforms individually
			foreach (var platform in SymbolAvailability.SupportedPlatforms) {
				Equal (expected [platform], actual [platform]);
			}
		}
	}
}
