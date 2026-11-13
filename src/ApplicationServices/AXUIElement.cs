// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__
using CoreFoundation;

namespace ApplicationServices {
	/// <summary>Provides access to an application's accessibility hierarchy, attributes, and actions.</summary>
	/// <remarks>
	///   <para>Most operations require the user to grant accessibility access to the calling application in System Settings. Native failures are returned as <see cref="AXError" /> values.</para>
	///   <para>Attribute and action names can be obtained from AppKit's accessibility constants or from the element itself.</para>
	/// </remarks>
	[SupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("maccatalyst")]
	public unsafe class AXUIElement : NativeObject {
		[Preserve (Conditional = true)]
		internal AXUIElement (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static nuint AXUIElementGetTypeID ();

		/// <summary>Gets the Core Foundation type identifier of accessibility elements.</summary>
		/// <returns>The native type identifier.</returns>
		public static nuint GetTypeID () => AXUIElementGetTypeID ();

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static IntPtr AXUIElementCreateApplication (int pid);

		/// <summary>Creates an accessibility element for an application.</summary>
		/// <param name="pid">The application's process identifier.</param>
		/// <returns>The application's accessibility element.</returns>
		public static AXUIElement CreateApplication (int pid) => new AXUIElement (AXUIElementCreateApplication (pid), true);

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static IntPtr AXUIElementCreateSystemWide ();

		/// <summary>Creates the system-wide accessibility element.</summary>
		/// <returns>An element for querying system-wide attributes and finding elements on screen.</returns>
		public static AXUIElement CreateSystemWide () => new AXUIElement (AXUIElementCreateSystemWide (), true);

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static byte AXIsProcessTrusted ();

		/// <summary>Gets whether the current process is trusted to use the accessibility API.</summary>
		public static bool IsProcessTrusted => AXIsProcessTrusted () != 0;

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static byte AXIsProcessTrustedWithOptions (IntPtr options);

		/// <summary>Checks whether the current process is trusted, optionally prompting for access.</summary>
		/// <param name="options">Options containing <see cref="AXTrustedCheckOptions.Prompt" />, or <see langword="null" /> for the default behavior.</param>
		/// <returns>Whether the process is currently trusted. A prompt does not change the result synchronously.</returns>
		public static bool IsProcessTrustedWithOptions (NSDictionary? options)
		{
			var result = AXIsProcessTrustedWithOptions (options.GetHandle ()) != 0;
			GC.KeepAlive (options);
			return result;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementGetPid (IntPtr element, int* pid);

		/// <summary>Gets the process identifier of the element's application.</summary>
		/// <param name="pid">The process identifier, valid only on success.</param>
		/// <returns>The native result code.</returns>
		public AXError GetPid (out int pid)
		{
			pid = 0;
			AXError error;
			fixed (int* ptr = &pid)
				error = AXUIElementGetPid (GetCheckedHandle (), ptr);
			GC.KeepAlive (this);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementCopyAttributeNames (IntPtr element, IntPtr* names);

		/// <summary>Gets the names of the element's attributes.</summary>
		/// <param name="names">The attribute names, or <see langword="null" /> on failure.</param>
		/// <returns>The native result code.</returns>
		public AXError GetAttributeNames (out string? []? names)
		{
			var handle = IntPtr.Zero;
			var error = AXUIElementCopyAttributeNames (GetCheckedHandle (), &handle);
			GC.KeepAlive (this);
			names = CFArray.StringArrayFromHandle (handle, true);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementCopyAttributeValue (IntPtr element, IntPtr attribute, IntPtr* value);

		/// <summary>Gets an attribute value using its native wrapper type.</summary>
		/// <typeparam name="T">The wrapper matching the attribute's native type, such as <see cref="AXUIElement" />, <see cref="AXValue" />, <see cref="CFArray" />, or <see cref="NSString" />.</typeparam>
		/// <param name="attribute">The attribute name.</param>
		/// <param name="value">The owned value, or <see langword="null" /> on failure.</param>
		/// <returns>The native result code.</returns>
		public AXError GetAttributeValue<T> (string attribute, out T? value) where T : class, INativeObject
		{
			using var name = new CFString (attribute);
			var handle = IntPtr.Zero;
			var error = AXUIElementCopyAttributeValue (GetCheckedHandle (), name.Handle, &handle);
			GC.KeepAlive (this);
			value = Runtime.GetINativeObject<T> (handle, true);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementGetAttributeValueCount (IntPtr element, IntPtr attribute, nint* count);

		/// <summary>Gets the number of values of an array-valued attribute.</summary>
		/// <param name="attribute">The attribute name.</param>
		/// <param name="count">The number of values, valid only on success.</param>
		/// <returns>The native result code.</returns>
		public AXError GetAttributeValueCount (string attribute, out nint count)
		{
			using var name = new CFString (attribute);
			count = 0;
			AXError error;
			fixed (nint* ptr = &count)
				error = AXUIElementGetAttributeValueCount (GetCheckedHandle (), name.Handle, ptr);
			GC.KeepAlive (this);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementCopyAttributeValues (IntPtr element, IntPtr attribute, nint index, nint maxValues, IntPtr* values);

		/// <summary>Gets a slice of an array-valued attribute.</summary>
		/// <param name="attribute">The attribute name.</param>
		/// <param name="index">The first value's zero-based index.</param>
		/// <param name="maxValues">The maximum number of values to return.</param>
		/// <param name="values">The owned array, or <see langword="null" /> on failure.</param>
		/// <returns>The native result code.</returns>
		public AXError GetAttributeValues (string attribute, nint index, nint maxValues, out CFArray? values)
		{
			using var name = new CFString (attribute);
			var handle = IntPtr.Zero;
			var error = AXUIElementCopyAttributeValues (GetCheckedHandle (), name.Handle, index, maxValues, &handle);
			GC.KeepAlive (this);
			values = Runtime.GetINativeObject<CFArray> (handle, true);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementIsAttributeSettable (IntPtr element, IntPtr attribute, byte* settable);

		/// <summary>Checks whether an attribute can be changed.</summary>
		/// <param name="attribute">The attribute name.</param>
		/// <param name="settable">Whether the attribute is settable, valid only on success.</param>
		/// <returns>The native result code.</returns>
		public AXError IsAttributeSettable (string attribute, out bool settable)
		{
			using var name = new CFString (attribute);
			byte result = 0;
			var error = AXUIElementIsAttributeSettable (GetCheckedHandle (), name.Handle, &result);
			GC.KeepAlive (this);
			settable = result != 0;
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementSetAttributeValue (IntPtr element, IntPtr attribute, IntPtr value);

		/// <summary>Sets an attribute to a native value.</summary>
		/// <param name="attribute">The attribute name.</param>
		/// <param name="value">The value, using the attribute's expected native type.</param>
		/// <returns>The native result code.</returns>
		public AXError SetAttributeValue (string attribute, INativeObject value)
		{
			using var name = new CFString (attribute);
			var error = AXUIElementSetAttributeValue (GetCheckedHandle (), name.Handle, value.GetNonNullHandle (nameof (value)));
			GC.KeepAlive (value);
			GC.KeepAlive (this);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementCopyMultipleAttributeValues (IntPtr element, IntPtr attributes, AXCopyMultipleAttributeOptions options, IntPtr* values);

		/// <summary>Gets multiple attribute values in a single request.</summary>
		/// <param name="attributes">The attribute names.</param>
		/// <param name="options">How to handle errors for individual attributes.</param>
		/// <param name="values">The owned array, or <see langword="null" /> on failure. Entries may be values, <see cref="NSNull" />, or <see cref="AXValue" /> objects containing errors.</param>
		/// <returns>The native result code.</returns>
		public AXError GetMultipleAttributeValues (string [] attributes, AXCopyMultipleAttributeOptions options, out CFArray? values)
		{
			using var names = CFArray.FromStrings (attributes);
			var handle = IntPtr.Zero;
			var error = AXUIElementCopyMultipleAttributeValues (GetCheckedHandle (), names.Handle, options, &handle);
			GC.KeepAlive (this);
			values = Runtime.GetINativeObject<CFArray> (handle, true);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementCopyParameterizedAttributeNames (IntPtr element, IntPtr* names);

		/// <summary>Gets the names of the element's parameterized attributes.</summary>
		/// <param name="names">The names, or <see langword="null" /> on failure.</param>
		/// <returns>The native result code.</returns>
		public AXError GetParameterizedAttributeNames (out string? []? names)
		{
			var handle = IntPtr.Zero;
			var error = AXUIElementCopyParameterizedAttributeNames (GetCheckedHandle (), &handle);
			GC.KeepAlive (this);
			names = CFArray.StringArrayFromHandle (handle, true);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementCopyParameterizedAttributeValue (IntPtr element, IntPtr attribute, IntPtr parameter, IntPtr* result);

		/// <summary>Gets a parameterized attribute value using its native wrapper type.</summary>
		/// <typeparam name="T">The wrapper matching the attribute's native return type.</typeparam>
		/// <param name="attribute">The parameterized attribute name.</param>
		/// <param name="parameter">The parameter, using the attribute's expected native type.</param>
		/// <param name="value">The owned result, or <see langword="null" /> on failure.</param>
		/// <returns>The native result code.</returns>
		public AXError GetParameterizedAttributeValue<T> (string attribute, INativeObject parameter, out T? value) where T : class, INativeObject
		{
			using var name = new CFString (attribute);
			var handle = IntPtr.Zero;
			var error = AXUIElementCopyParameterizedAttributeValue (GetCheckedHandle (), name.Handle, parameter.GetNonNullHandle (nameof (parameter)), &handle);
			GC.KeepAlive (parameter);
			GC.KeepAlive (this);
			value = Runtime.GetINativeObject<T> (handle, true);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementCopyActionNames (IntPtr element, IntPtr* names);

		/// <summary>Gets the names of the element's supported actions.</summary>
		/// <param name="names">The action names, or <see langword="null" /> on failure.</param>
		/// <returns>The native result code.</returns>
		public AXError GetActionNames (out string? []? names)
		{
			var handle = IntPtr.Zero;
			var error = AXUIElementCopyActionNames (GetCheckedHandle (), &handle);
			GC.KeepAlive (this);
			names = CFArray.StringArrayFromHandle (handle, true);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementCopyActionDescription (IntPtr element, IntPtr action, IntPtr* description);

		/// <summary>Gets a localized description of an action.</summary>
		/// <param name="action">The action name.</param>
		/// <param name="description">The description, or <see langword="null" /> on failure.</param>
		/// <returns>The native result code.</returns>
		public AXError GetActionDescription (string action, out string? description)
		{
			using var name = new CFString (action);
			var handle = IntPtr.Zero;
			var error = AXUIElementCopyActionDescription (GetCheckedHandle (), name.Handle, &handle);
			GC.KeepAlive (this);
			description = CFString.FromHandle (handle, true);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementPerformAction (IntPtr element, IntPtr action);

		/// <summary>Performs an accessibility action.</summary>
		/// <param name="action">The action name.</param>
		/// <returns>The native result code.</returns>
		public AXError PerformAction (string action)
		{
			using var name = new CFString (action);
			var error = AXUIElementPerformAction (GetCheckedHandle (), name.Handle);
			GC.KeepAlive (this);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementCopyElementAtPosition (IntPtr application, float x, float y, IntPtr* element);

		/// <summary>Finds the accessibility element at a screen position.</summary>
		/// <param name="x">The horizontal screen coordinate.</param>
		/// <param name="y">The vertical screen coordinate, measured from the top of the main display.</param>
		/// <param name="element">The owned element, or <see langword="null" /> on failure.</param>
		/// <returns>The native result code.</returns>
		public AXError GetElementAtPosition (float x, float y, out AXUIElement? element)
		{
			var handle = IntPtr.Zero;
			var error = AXUIElementCopyElementAtPosition (GetCheckedHandle (), x, y, &handle);
			GC.KeepAlive (this);
			element = Runtime.GetINativeObject<AXUIElement> (handle, true);
			return error;
		}

		[DllImport (Constants.ApplicationServicesLibrary)]
		extern static AXError AXUIElementSetMessagingTimeout (IntPtr element, float timeoutInSeconds);

		/// <summary>Sets the timeout for accessibility messages sent to the element's application.</summary>
		/// <param name="timeoutInSeconds">The timeout in seconds, or zero to restore the default. On the system-wide element, sets the calling process's default timeout.</param>
		/// <returns>The native result code.</returns>
		public AXError SetMessagingTimeout (float timeoutInSeconds)
		{
			var error = AXUIElementSetMessagingTimeout (GetCheckedHandle (), timeoutInSeconds);
			GC.KeepAlive (this);
			return error;
		}
	}
}
#endif
