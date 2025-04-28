//
// VNRequest.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;
using ObjCRuntime;

namespace Vision {
	public partial class VNRequest {

		/// <typeparam name="T">The subclass of <see cref="T:Vision.VNObservation" /> produced.</typeparam>
		///         <summary>Gets the detected objects, as an array of the specified subclass of <see cref="T:Vision.VNObservation" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>The following example shows how one might retrieve the results of a <see cref="T:Vision.VNDetectFaceRectanglesRequest" />:</para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// var rs = request.GetResults<VNFaceObservation>();
		/// foreach (var fo in rs)
		/// {
		///    // ... etc ...
		///     ]]></code>
		///           </example>
		///         </remarks>
		public virtual T [] GetResults<T> () where T : VNObservation
		{
			// From docs: If the request failed, this property will be nil;
			// otherwise, it will be an array of zero or more VNObservation
			// subclasses specific to the VNRequest subclass.
			// ArrayFromHandle<T> does the null checking for us.
			return NSArray.ArrayFromHandle<T> (_Results);
		}
	}
}
