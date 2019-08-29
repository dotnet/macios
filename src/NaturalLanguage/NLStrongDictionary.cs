using System;

using CoreFoundation;
using Foundation;

namespace NaturalLanguage {

	// nicer replacement for `NSDictionary<NSString, NSArray<NSString>>`
	public class NLStrongDictionary : DictionaryContainer {

#if !COREBUILD
		public NLStrongDictionary ()
		{
		}

		public NLStrongDictionary (NSDictionary dictionary) : base (dictionary)
		{
		}

		public string[] this [NSString key] {
			get {
				if (key == null)
					throw new ArgumentNullException (nameof (key));

				var value = CFDictionary.GetValue (Dictionary.Handle, key.Handle);
				return NSArray.StringArrayFromHandle (value);
			}
			set {
				SetArrayValue (key, value);
			}
		}

		public string[] this [string key] {
			get {
				return this [(NSString) key];
			}
			set {
				SetArrayValue ((NSString) key, value);
			}
		}
#endif
	}
}
