// 
// DictionaryContainer.cs: Foundation implementation for NSDictionary based setting classes
//
// Authors: Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012, 2014 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#if !COREBUILD
using CoreFoundation;
using ObjCRuntime;
using Foundation;
using CoreGraphics;
using CoreMedia;
#endif

#if HAS_UIKIT
using UIKit;
#endif

#nullable enable

namespace Foundation {

	/// <include file="../../docs/api/Foundation/DictionaryContainer.xml" path="/Documentation/Docs[@DocId='T:Foundation.DictionaryContainer']/*" />
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public abstract class DictionaryContainer {
#if !COREBUILD
		/// <include file="../../docs/api/Foundation/DictionaryContainer.xml" path="/Documentation/Docs[@DocId='M:Foundation.DictionaryContainer.#ctor']/*" />
		protected DictionaryContainer ()
		{
			Dictionary = new NSMutableDictionary ();
		}

		/// <include file="../../docs/api/Foundation/DictionaryContainer.xml" path="/Documentation/Docs[@DocId='M:Foundation.DictionaryContainer.#ctor(Foundation.NSDictionary)']/*" />
		protected DictionaryContainer (NSDictionary? dictionary)
		{
			Dictionary = dictionary ?? new NSMutableDictionary ();
		}

		/// <summary>Gets the wrapped <see cref="T:Foundation.NSDictionary" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSDictionary Dictionary { get; private set; }

		/// <typeparam name="T">The type of values stored in the array identified by <paramref name="key" />.</typeparam>
		///         <param name="key">The identifier of the array.</param>
		///         <summary>Retrieves the array of type <c>T</c> associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected T []? GetArray<T> (NSString key) where T : NSObject
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			var value = CFDictionary.GetValue (Dictionary.Handle, key.Handle);
			GC.KeepAlive (key);
			return NSArray.ArrayFromHandle<T> (value);
		}

		protected T []? GetArray<T> (NSString key, Func<NativeHandle, T> creator)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			var value = CFDictionary.GetValue (Dictionary.Handle, key.Handle);
			GC.KeepAlive (key);
			return NSArray.ArrayFromHandleFunc<T> (value, creator);
		}

		/// <param name="key">The identifier of the int.</param>
		///         <summary>Returns the nullable int associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected int? GetInt32Value (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			return ((NSNumber) value).Int32Value;
		}

		/// <param name="key">The identifier of the <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20UInt%2032&amp;scope=Xamarin" title="T:System.UInt32">T:System.UInt32</a></format>.</param>
		///         <summary>Returns the nullable <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20UInt%2032&amp;scope=Xamarin" title="T:System.UInt32">T:System.UInt32</a></format> associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected uint? GetUInt32Value (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			return ((NSNumber) value).UInt32Value;
		}

		/// <param name="key">The identifier of the native integer.</param>
		///         <summary>Returns the nullable native integer associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected nint? GetNIntValue (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			return ((NSNumber) value).NIntValue;
		}

		/// <param name="key">The identifier of the nuint</param>
		///         <summary>Returns the nullable native unsigned int associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected nuint? GetNUIntValue (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			return ((NSNumber) value).NUIntValue;
		}

		/// <param name="key">The identifier of the long.</param>
		///         <summary>Returns the nullable long associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected long? GetLongValue (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			return ((NSNumber) value).Int64Value;
		}

		protected ulong? GetULongValue (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			if (!Dictionary.TryGetValue (key, out var value))
				return null;

			return ((NSNumber) value).UInt64Value;
		}

		/// <param name="key">The identifier of the <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20UInt%2032&amp;scope=Xamarin" title="T:System.UInt32">T:System.UInt32</a></format>.</param>
		///         <summary>Returns the nullable <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20UInt%2032&amp;scope=Xamarin" title="T:System.UInt32">T:System.UInt32</a></format> associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected uint? GetUIntValue (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			return ((NSNumber) value).UInt32Value;
		}

		/// <param name="key">The identifier of the float.</param>
		///         <summary>Returns the nullable float associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected float? GetFloatValue (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			return ((NSNumber) value).FloatValue;
		}

		/// <param name="key">The identifier of the double.</param>
		///         <summary>Returns the nullable double associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected double? GetDoubleValue (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			return ((NSNumber) value).DoubleValue;
		}

		/// <param name="key">The identifier of the bool.</param>
		///         <summary>Returns the nullable Boolean associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected bool? GetBoolValue (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			var value = CFDictionary.GetValue (Dictionary.Handle, key.Handle);
			GC.KeepAlive (key);
			if (value == IntPtr.Zero)
				return null;

			return CFBoolean.GetValue (value);
		}

		/// <typeparam name="T">The <see cref="T:ObjCRuntime.INativeObject" /> type associated with <paramref name="key" />.</typeparam>
		///         <param name="key">The identifier of the reference.</param>
		///         <summary>Returns the native object associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected T? GetNativeValue<T> (NSString key) where T : class, INativeObject
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			T? result = Runtime.GetINativeObject<T> (Dictionary.LowlevelObjectForKey (key.Handle), false);
			GC.KeepAlive (key);
			return result;
		}

		protected string []? GetStringArrayValue (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			var array = Dictionary.LowlevelObjectForKey (key.Handle);
			GC.KeepAlive (key);
			return CFArray.StringArrayFromHandle (array)!;
		}

		/// <param name="key">The identifier of the <see cref="T:Foundation.NSDictionary" />.</param>
		///         <summary>Returns the <see cref="T:Foundation.NSDictionary" /> associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected NSDictionary? GetNSDictionary (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			NSObject value;
			Dictionary.TryGetValue (key, out value);
			return value as NSDictionary;
		}

		/// <typeparam name="TKey">The type of keys in the stored dictionary.</typeparam>
		///         <typeparam name="TValue">The type of values in the stored dictionary.</typeparam>
		///         <param name="key">The identifier of the <see cref="T:Foundation.NSDictionary" />.</param>
		///         <summary>Returns the <see cref="T:Foundation.NSDictionary" /> associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected NSDictionary<TKey, TValue>? GetNSDictionary<TKey, TValue> (NSString key)
			where TKey : class, INativeObject
			where TValue : class, INativeObject
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			NSObject value;
			Dictionary.TryGetValue (key, out value);
			return value as NSDictionary<TKey, TValue>;
		}

		/// <typeparam name="T">The type of <see cref="T:Foundation.DictionaryContainer" /> associated with <paramref name="key" />.</typeparam>
		///         <param name="key">The identifier of the <see cref="T:Foundation.DictionaryContainer" />.</param>
		///         <summary>Returns the <see cref="T:Foundation.DictionaryContainer" /> associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected T? GetStrongDictionary<[DynamicallyAccessedMembers (DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T> (NSString key)
			where T : DictionaryContainer
		{
			return GetStrongDictionary (key, dict =>
				(T?) Activator.CreateInstance (typeof (T), new object [] { dict }));
		}

		protected T? GetStrongDictionary<T> (NSString? key, Func<NSDictionary, T?> createStrongDictionary)
			where T : DictionaryContainer
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			var dict = GetNSDictionary (key);
			if (dict is null)
				return null;

			return createStrongDictionary (dict);
		}

		/// <param name="key">The identifier of the <see cref="T:Foundation.NSString" />.</param>
		///         <summary>Returns the <see cref="T:Foundation.NSString" /> associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected NSString? GetNSStringValue (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			NSObject value;
			Dictionary.TryGetValue (key, out value);
			return value as NSString;
		}

		/// <param name="key">The identifier of the string.</param>
		///         <summary>Returns the string associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected string? GetStringValue (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			var result = CFString.FromHandle (value.Handle);
			GC.KeepAlive (value);
			return result;
		}

		/// <param name="key">The identifier of the string.</param>
		///         <summary>Returns the string associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected string? GetStringValue (string key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			var keyHandle = CFString.CreateNative (key);
			try {
				return CFString.FromHandle (CFDictionary.GetValue (Dictionary.Handle, keyHandle));
			} finally {
				CFString.ReleaseNative (keyHandle);
			}
		}

		/// <param name="key">The identifier of the <see cref="T:Foundation.NSString" />.</param>
		///         <summary>Returns the nullable <see cref="T:Foundation.NSString" /> associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected CGRect? GetCGRectValue (NSString key)
		{
			var dictValue = GetNSDictionary (key);
			CGRect value;
			if (!CGRect.TryParse (dictValue, out value))
				return null;

			return value;
		}

		/// <param name="key">The identifier of the <see cref="T:CoreGraphics.CGSize" />.</param>
		///         <summary>Returns the nullable <see cref="T:CoreGraphics.CGSize" /> associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected CGSize? GetCGSizeValue (NSString key)
		{
			var dictValue = GetNSDictionary (key);
			CGSize value;
			if (!CGSize.TryParse (dictValue, out value))
				return null;

			return value;
		}

		/// <param name="key">The identifier of the <see cref="T:CoreGraphics.CGPoint" />.</param>
		///         <summary>Returns the nullable <see cref="T:CoreGraphics.CGPoint" /> associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected CGPoint? GetCGPointValue (NSString key)
		{
			var dictValue = GetNSDictionary (key);
			CGPoint value;
			if (!CGPoint.TryParse (dictValue, out value))
				return null;

			return value;
		}

		/// <param name="key">The identifier of the <see cref="T:CoreMedia.CMTime" />.</param>
		///         <summary>Returns the nullable <see cref="T:CoreMedia.CMTime" /> associated with <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		protected CMTime? GetCMTimeValue (NSString key)
		{
			var dictValue = GetNSDictionary (key);
			if (dictValue is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dictValue));
			var value = CMTime.FromDictionary (dictValue);
			if (value.IsInvalid)
				return null;

			return value;
		}

#if HAS_UIKIT
		protected UIEdgeInsets? GetUIEdgeInsets (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			if (!Dictionary.TryGetValue (key, out var value))
				return null;

			if (value is NSValue size)
				return size.UIEdgeInsetsValue;

			return null;
		}
#endif

		bool NullCheckAndRemoveKey ([NotNullWhen (true)] NSString key, bool removeEntry)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			if (removeEntry)
				RemoveValue (key);

			return !removeEntry;
		}

		/// <param name="key">The identifier to be associated with the array.</param>
		///         <param name="values">The <see cref="T:Foundation.NSNumber" /> array to be associated with <paramref name="key" />.</param>
		///         <summary>Associates the <see cref="T:Foundation.NSNumber" /> array <paramref name="values" /> with <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetArrayValue (NSString key, NSNumber []? values)
		{
			if (NullCheckAndRemoveKey (key, values is null))
				Dictionary [key] = NSArray.FromNSObjects (values);
		}

		/// <typeparam name="T">The type stored by the array.</typeparam>
		///         <param name="key">The identifier to be associated with the array.</param>
		///         <param name="values">The array of type <typeparamref name="T" /> to be associated with <paramref name="key" />.</param>
		///         <summary>Associates the array <paramref name="values" /> of type <typeparamref name="T" /> with <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetArrayValue<T> (NSString key, T []? values)
		{
			if (NullCheckAndRemoveKey (key, values is null)) {
				var nsValues = new NSObject [values!.Length];
				for (var i = 0; i < values.Length; i++)
					nsValues [i] = NSObject.FromObject (values [i]);
				Dictionary [key] = NSArray.FromNSObjects (nsValues);
			}
		}

		/// <param name="key">The identifier to be associated with the array.</param>
		///         <param name="values">The <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> array to be associated with <paramref name="key" />.</param>
		///         <summary>Associates the <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=System%20String&amp;scope=Xamarin" title="T:System.String">T:System.String</a></format> array <paramref name="values" /> with <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetArrayValue (NSString key, string []? values)
		{
			if (NullCheckAndRemoveKey (key, values is null))
				Dictionary [key] = NSArray.FromStrings (values);
		}

		/// <param name="key">The identifier to be associated with the array.</param>
		///         <param name="values">The <see cref="T:ObjCRuntime.INativeObject" /> array to be associated with <paramref name="key" />.</param>
		///         <summary>Associates the <see cref="T:ObjCRuntime.INativeObject" /> array <paramref name="values" /> with <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetArrayValue (NSString key, INativeObject []? values)
		{
			if (NullCheckAndRemoveKey (key, values is null)) {
				var array = CFArray.FromNativeObjects (values!);
				CFMutableDictionary.SetValue (Dictionary.Handle, key.Handle, array.Handle);
				GC.KeepAlive (key);
				GC.KeepAlive (array);
			}
		}

		#region Sets CFBoolean value

		/// <param name="key">The identifier associated with the Boolean.</param>
		///         <param name="value">The nullable Boolean to be associated with <paramref name="key" />.</param>
		///         <summary>Stores the Boolean <paramref name="value" /> and associates it with the <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetBooleanValue (NSString key, bool? value)
		{
			if (NullCheckAndRemoveKey (key, !value.HasValue)) {
				CFMutableDictionary.SetValue (Dictionary.Handle, key.Handle, value!.Value ? CFBoolean.TrueHandle : CFBoolean.FalseHandle);
				GC.KeepAlive (key);
			}
		}

		#endregion

		#region Sets NSNumber value

		/// <param name="key">The identifier associated with the int.</param>
		///         <param name="value">The nullable int to be associated with <paramref name="key" />.</param>
		///         <summary>Stores the int <paramref name="value" /> (or <see langword="null" />) and associates it with the <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetNumberValue (NSString key, int? value)
		{
			if (NullCheckAndRemoveKey (key, !value.HasValue))
				Dictionary [key] = new NSNumber (value!.Value);
		}

		/// <param name="key">The identifier associated with the uint.</param>
		///         <param name="value">The nullable unsigned int to be associated with <paramref name="key" />.</param>
		///         <summary>Stores the unsigned int <paramref name="value" /> (or <see langword="null" />) and associates it with the <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetNumberValue (NSString key, uint? value)
		{
			if (NullCheckAndRemoveKey (key, !value.HasValue))
				Dictionary [key] = new NSNumber (value!.Value);
		}

		protected void SetNumberValue (NSString key, nint? value)
		{
			if (NullCheckAndRemoveKey (key, !value.HasValue))
				Dictionary [key] = new NSNumber (value!.Value);
		}

		protected void SetNumberValue (NSString key, nuint? value)
		{
			if (NullCheckAndRemoveKey (key, !value.HasValue))
				Dictionary [key] = new NSNumber (value!.Value);
		}

		/// <param name="key">The identifier associated with the long.</param>
		///         <param name="value">The nullable long to be associated with <paramref name="key" />.</param>
		///         <summary>Stores the long <paramref name="value" /> (or <see langword="null" />) and associates it with the <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetNumberValue (NSString key, long? value)
		{
			if (NullCheckAndRemoveKey (key, !value.HasValue))
				Dictionary [key] = new NSNumber (value!.Value);
		}

		/// <param name="key">The identifier associated with the float.</param>
		///         <param name="value">The nullable float to be associated with <paramref name="key" />.</param>
		///         <summary>Stores the float <paramref name="value" /> (or <see langword="null" />) and associates it with the <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetNumberValue (NSString key, float? value)
		{
			if (NullCheckAndRemoveKey (key, !value.HasValue))
				Dictionary [key] = new NSNumber (value!.Value);
		}

		/// <param name="key">The identifier associated with the double.</param>
		///         <param name="value">The nullable double to be associated with <paramref name="key" />.</param>
		///         <summary>Stores the double <paramref name="value" /> (or <see langword="null" />) and associates it with the <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetNumberValue (NSString key, double? value)
		{
			if (NullCheckAndRemoveKey (key, !value.HasValue))
				Dictionary [key] = new NSNumber (value!.Value);
		}

		#endregion

		#region Sets NSString value

		/// <param name="key">The identifier associated with the string.</param>
		///         <param name="value">The string to be associated with <paramref name="key" />.</param>
		///         <summary>Stores the string <paramref name="value" /> and associates it with the <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetStringValue (NSString key, string? value)
		{
			SetStringValue (key, value is null ? (NSString) null! : new NSString (value));
		}

		/// <param name="key">The identifier associated with the string.</param>
		///         <param name="value">The string to be associated with <paramref name="key" />.</param>
		///         <summary>Stores the string <paramref name="value" /> and associates it with the <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetStringValue (NSString key, NSString? value)
		{
			if (NullCheckAndRemoveKey (key, value is null))
				Dictionary [key] = value;
		}

		#endregion

		#region Sets Native value

		/// <param name="key">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <param name="removeNullValue">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetNativeValue (NSString key, INativeObject? value, bool removeNullValue = true)
		{
			if (NullCheckAndRemoveKey (key, removeNullValue && value is null)) {
				CFMutableDictionary.SetValue (Dictionary.Handle, key.Handle, value.GetHandle ());
				GC.KeepAlive (key);
				GC.KeepAlive (value);
			}
		}

		#endregion

		/// <param name="key">The identifier of the value to be removed.</param>
		///         <summary>Removes from the dictionary the value associated with <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void RemoveValue (NSString key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			((NSMutableDictionary) Dictionary).Remove (key);
		}

		#region Sets structs values

		/// <param name="key">The identifier associated with the <see cref="T:CoreGraphics.CGRect" />.</param>
		///         <param name="value">The <see cref="T:CoreGraphics.CGRect" /> to be associated with <paramref name="key" />.</param>
		///         <summary>Stores the <see cref="T:CoreGraphics.CGRect" /><paramref name="value" /> and associates it with the <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetCGRectValue (NSString key, CGRect? value)
		{
			if (NullCheckAndRemoveKey (key, !value.HasValue))
				Dictionary [key] = value!.Value.ToDictionary ();
		}

		/// <param name="key">The identifier associated with the <see cref="T:CoreGraphics.CGSize" />.</param>
		///         <param name="value">The <see cref="T:CoreGraphics.CGSize" /> to be associated with <paramref name="key" />.</param>
		///         <summary>Stores the <see cref="T:CoreGraphics.CGSize" /><paramref name="value" /> and associates it with the <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetCGSizeValue (NSString key, CGSize? value)
		{
			if (NullCheckAndRemoveKey (key, !value.HasValue))
				Dictionary [key] = value!.Value.ToDictionary ();
		}

		/// <param name="key">The identifier associated with the <see cref="T:CoreGraphics.CGPoint" />.</param>
		///         <param name="value">The <see cref="T:CoreGraphics.CGPoint" /> to be associated with <paramref name="key" />.</param>
		///         <summary>Stores the <see cref="T:CoreGraphics.CGPoint" /><paramref name="value" /> and associates it with the <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetCGPointValue (NSString key, CGPoint? value)
		{
			if (NullCheckAndRemoveKey (key, !value.HasValue))
				Dictionary [key] = value!.Value.ToDictionary ();
		}

		/// <param name="key">The identifier associated with the <see cref="T:CoreMedia.CMTime" />.</param>
		///         <param name="value">The <see cref="T:CoreMedia.CMTime" /> to be associated with <paramref name="key" />.</param>
		///         <summary>Stores the <see cref="T:CoreMedia.CMTime" /><paramref name="value" /> and associates it with the <paramref name="key" />.</summary>
		///         <remarks>To be added.</remarks>
		protected void SetCMTimeValue (NSString key, CMTime? value)
		{
			if (NullCheckAndRemoveKey (key, !value.HasValue))
				Dictionary [key] = value!.Value.ToDictionary ();
		}

#if HAS_UIKIT
		protected void SetUIEdgeInsets (NSString key, UIEdgeInsets? value)
		{
			SetNativeValue (key, value is null ? null : NSValue.FromUIEdgeInsets (value.Value));
		}
#endif
		#endregion
#endif
	}

#if !COREBUILD
	static class DictionaryContainerHelper {

		// helper to avoid the (common pattern)
		// 	var p = x is null ? NativeHandle.Zero : h.Dictionary.Handle;
		static public NativeHandle GetHandle (this DictionaryContainer? self)
		{
			return self is null ? NativeHandle.Zero : self.Dictionary.Handle;
		}

		// helper to avoid the (common pattern)
		// 	var p = x is null ? null : x.Dictionary;
		[return: NotNullIfNotNull (nameof (self))]
		static public NSDictionary? GetDictionary (this DictionaryContainer? self)
		{
			return self is null ? null : self.Dictionary;
		}
	}
#endif
}
