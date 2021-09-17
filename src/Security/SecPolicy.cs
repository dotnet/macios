// 
// SecPolicy.cs: Implements the managed SecPolicy wrapper.
//
// Authors: 
//  Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013-2014 Xamarin Inc.
//

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using ObjCRuntime;
using CoreFoundation;
using Foundation;

namespace Security {

	public partial class SecPolicy {

#if !NET
		[iOS (7,0)]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* __nullable CFDictionaryRef */ SecPolicyCopyProperties (IntPtr /* SecPolicyRef */ policyRef);

#if !NET
		[iOS (7,0)]
#endif
		public NSDictionary GetProperties ()
		{
			var dict = SecPolicyCopyProperties (Handle);
			return Runtime.GetNSObject<NSDictionary> (dict, true);
		}

#if !NET
		[Mac (10,9)]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* __nullable SecPolicyRef */ SecPolicyCreateRevocation (/* CFOptionFlags */ nuint revocationFlags);

#if !NET
		[Mac (10,9)][iOS (7,0)]
#endif
		static public SecPolicy CreateRevocationPolicy (SecRevocation revocationFlags)
		{
			var policy = SecPolicyCreateRevocation ((nuint)(ulong) revocationFlags);
			return policy == IntPtr.Zero ? null : new SecPolicy (policy, true);
		}

#if !NET
		[Mac (10,9)][iOS (7,0)]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* __nullable SecPolicyRef */ SecPolicyCreateWithProperties (IntPtr /* CFTypeRef */ policyIdentifier, IntPtr /* CFDictionaryRef */ properties);

#if !NET
		[Mac (10,9)][iOS (7,0)]
#endif
		static public SecPolicy CreatePolicy (NSString policyIdentifier, NSDictionary properties)
		{
			if (policyIdentifier == null)
				throw new ArgumentNullException ("policyIdentifier");
			IntPtr dh = properties == null ? IntPtr.Zero : properties.Handle;

			// note: only accept known OIDs or return null (unit test will alert us if that change, FIXME in Apple code)
			// see: https://github.com/Apple-FOSS-Mirror/libsecurity_keychain/blob/master/lib/SecPolicy.cpp#L245
			IntPtr ph = SecPolicyCreateWithProperties (policyIdentifier.Handle, dh);
			if (ph == IntPtr.Zero)
				throw new ArgumentException ("Unknown policyIdentifier");
			return new SecPolicy (ph, true);
		}
	}
}
