//
// INInteraction.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#if IOS
using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace Intents {
	public partial class INInteraction {

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="parameter">To be added.</param>
		///         <summary>Returns the specified <paramref name="parameter" /> as an instance of <typeparamref name="T" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public T GetParameterValue<T> (INParameter parameter) where T : NSObject
		{
			return Runtime.GetNSObject<T> (_GetParameterValue (parameter))!;
		}
	}
}
#endif
