using System;

using CoreFoundation;
using Foundation;

namespace NaturalLanguage {

	// nicer replacement for `NSDictionary<NSString, NSArray<NSNumber>>`
	public class NLVectorDictionary : DictionaryContainer {

#if !COREBUILD
		public NLVectorDictionary ()
		{
		}

		public NLVectorDictionary (NSDictionary dictionary) : base (dictionary)
		{
		}

		public float[] this [NSString key] {
			get {
				if (key == null)
					throw new ArgumentNullException (nameof (key));

				var a = CFDictionary.GetValue (Dictionary.Handle, key.Handle);
                return NSArray.ArrayFromHandle<float> (a, input => {
					return new NSNumber (input).FloatValue;
				});
			}
			set {
				if (key == null)
					throw new ArgumentNullException (nameof (key));

				if (value == null)
					RemoveValue (key);
				else
					Dictionary [key] = NSArray.From (value);
			}
		}

		public float[] this [string key] {
			get {
				return this [(NSString) key];
			}
			set {
				this [(NSString) key] = value;
			}
		}
#endif
	}
}
