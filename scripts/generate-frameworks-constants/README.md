# generate-frameworks-constants

This script generates a C# file that contains the path to the frameworks a particular platform binds/needs.

```csharp
namespace ObjCRuntime {
	public static partial class Constants {
		// iOS 3.0
		public const string AddressBookLibrary = "/System/Library/Frameworks/AddressBook.framework/AddressBook";
		public const string AddressBookUILibrary = "/System/Library/Frameworks/AddressBookUI.framework/AddressBookUI";
		public const string AudioUnitLibrary = "/System/Library/Frameworks/AudioToolbox.framework/AudioToolbox";
		// ...
	}
```

