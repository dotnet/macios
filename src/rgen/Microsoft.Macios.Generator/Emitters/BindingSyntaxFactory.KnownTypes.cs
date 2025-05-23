// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Extensions;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {

	// CoreFoundation types

	/// <summary>
	/// TypeSyntax for CoreFoundation.CFArray.
	/// </summary>
	public static readonly TypeSyntax CFArray = StringExtensions.GetIdentifierName (
		@namespace: ["CoreFoundation"],
		@class: "CFArray");

	/// <summary>
	/// TypeSyntax for CoreFoundation.CFString.
	/// </summary>
	public static readonly TypeSyntax CFString = StringExtensions.GetIdentifierName (
		@namespace: ["CoreFoundation"],
		@class: "CFString");

	// ObjC runtime types

	/// <summary>
	/// TypeSyntax for ObjCRuntime.Selector.
	/// </summary>
	public readonly static TypeSyntax Selector = StringExtensions.GetIdentifierName (
		@namespace: ["ObjCRuntime"],
		@class: "Selector");

	/// <summary>
	/// TypeSyntax for ObjCRuntime.NativeHandle.
	/// </summary>
	public readonly static TypeSyntax NativeHandle = StringExtensions.GetIdentifierName (
		@namespace: ["ObjCRuntime"],
		@class: "NativeHandle");

	/// <summary>
	/// TypeSyntax for ObjCRuntime.Class.
	/// </summary>
	public readonly static TypeSyntax Class = StringExtensions.GetIdentifierName (
		@namespace: ["ObjCRuntime"],
		@class: "Class");

	/// <summary>
	/// TypeSyntax for ObjCRuntime.Dlfcn.
	/// </summary>
	internal readonly static TypeSyntax Dlfcn = StringExtensions.GetIdentifierName (
		@namespace: ["ObjCRuntime"],
		@class: "Dlfcn");

	/// <summary>
	/// TypeSyntax for ObjCRuntime.Libraries.
	/// </summary>
	public readonly static TypeSyntax Libraries = StringExtensions.GetIdentifierName (
		@namespace: ["ObjCRuntime"],
		@class: "Libraries");

	/// <summary>
	/// TypeSyntax for ObjCRuntime.Runtime.
	/// </summary>
	public static readonly TypeSyntax Runtime = StringExtensions.GetIdentifierName (
		@namespace: ["ObjCRuntime"],
		@class: "Runtime");

	// Foundation types

	/// <summary>
	/// TypeSyntax for Foundation.NSArray.
	/// </summary>
	public static readonly TypeSyntax NSArray = StringExtensions.GetIdentifierName (
		@namespace: ["Foundation"],
		@class: "NSArray");

	/// <summary>
	/// TypeSyntax for Foundation.NSValue.
	/// </summary>
	public static readonly TypeSyntax NSValue = StringExtensions.GetIdentifierName (
		@namespace: ["Foundation"],
		@class: "NSValue");

	/// <summary>
	/// TypeSyntax for Foundation.NSNumber.
	/// </summary>
	public static readonly TypeSyntax NSNumber = StringExtensions.GetIdentifierName (
		@namespace: ["Foundation"],
		@class: "NSNumber");

	/// <summary>
	/// TypeSyntax for Foundation.NSObject.
	/// </summary>
	public readonly static TypeSyntax NSObject = StringExtensions.GetIdentifierName (
		@namespace: ["Foundation"],
		@class: "NSObject");

	/// <summary>
	/// TypeSyntax for Foundation.NSObjectFlag.
	/// </summary>
	public readonly static TypeSyntax NSObjectFlag = StringExtensions.GetIdentifierName (
		@namespace: ["Foundation"],
		@class: "NSObjectFlag");

	/// <summary>
	/// TypeSyntax for Foundation.NSString.
	/// </summary>
	public readonly static TypeSyntax NSString = StringExtensions.GetIdentifierName (
		@namespace: ["Foundation"],
		@class: "NSString");

	/// <summary>
	/// TypeSyntax for Foundation.NotificationCenter.
	/// </summary>
	public readonly static TypeSyntax NotificationCenter = StringExtensions.GetIdentifierName (
		@namespace: ["Foundation"],
		@class: "NotificationCenter");

	/// <summary>
	/// TypeSyntax for Foundation.NSNotificationEventArgs.
	/// </summary>
	public readonly static TypeSyntax NSNotificationEventArgs = StringExtensions.GetIdentifierName (
		@namespace: ["Foundation"],
		@class: "NSNotificationEventArgs");

	// CoreMedia types

	/// <summary>
	/// TypeSyntax for CoreMedia.CMTag.
	/// </summary>
	public readonly static TypeSyntax CMTag = StringExtensions.GetIdentifierName (
		@namespace: ["CoreMedia"],
		@class: "CMTag");


	// System types

	/// <summary>
	/// TypeSyntax for System.IntPtr.
	/// </summary>
	public readonly static TypeSyntax IntPtr = StringExtensions.GetIdentifierName (
		@namespace: ["System"],
		@class: "IntPtr");

	/// <summary>
	/// TypeSyntax for System.EventHandler.
	/// </summary>
	public readonly static TypeSyntax EventHandler = StringExtensions.GetIdentifierName (
		@namespace: ["System"],
		@class: "EventHandler");

}
