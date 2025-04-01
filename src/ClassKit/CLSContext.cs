//
// CLSContext.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2018 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;
using ObjCRuntime;

namespace ClassKit {
	public partial class CLSContext {

		/// <summary>Gets or sets the topic that the content of the context covers.</summary>
		///         <value>The topic that the content of the context covers.</value>
		///         <remarks>To be added.</remarks>
		public CLSContextTopic Topic {
			get => CLSContextTopicExtensions.GetValue (WeakTopic);
			set => WeakTopic = value.GetConstant ();
		}
	}
}
