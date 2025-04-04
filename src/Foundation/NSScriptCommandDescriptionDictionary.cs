// Copyright 2015 Xamarin, Inc.
using System;
using Foundation;
using ObjCRuntime;

namespace Foundation {

#if MONOMAC || __MACCATALYST__

	public static class NSScriptCommandDescriptionDictionaryKeys {
		private static NSString cmdClass = new NSString ("CommandClass");
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static NSString CommandClassKey {
			get { return cmdClass; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static NSString AppleEventCodeKey {
			get { return NSScriptCommonKeys.AppleEventCodeKey; }
		}

		private static NSString codeClass = new NSString ("AppleEventClassCode");
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static NSString AppleEventClassCodeKey {
			get { return codeClass; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static NSString TypeKey {
			get { return NSScriptCommonKeys.TypeKey; }
		}

		private static NSString resultAppEventCode = new NSString ("ResultAppleEventCode");
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static NSString ResultAppleEventCodeKey {
			get { return resultAppEventCode; }
		}

		private static NSString args = new NSString ("Arguments");
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static NSString ArgumentsKey {
			get { return args; }
		}
	}

	public partial class NSScriptCommandDescriptionDictionary {

		/// <param name="arg">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Add (NSScriptCommandArgumentDescription arg)
		{
			if (arg is null)
				throw new ArgumentNullException ("arg");
			if (Arguments is null)
				Arguments = new NSMutableDictionary ();
			using (var nsName = new NSString (arg.Name)) {
				Arguments.Add (nsName, arg.Dictionary);
			}
		}

		/// <param name="arg">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Remove (NSScriptCommandArgumentDescription arg)
		{
			if (arg is null)
				throw new ArgumentNullException ("arg");
			using (var nsName = new NSString (arg.Name)) {
				return Arguments?.Remove (nsName) ?? false;
			}
		}
	}

#endif

}
