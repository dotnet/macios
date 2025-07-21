#pragma warning disable APL0003
using System;
using Foundation;
using ObjCBindings;

namespace TestNamespace;

[BindingType<Class> (Name = "AVAudioPCMBuffer", Flags = Class.DisableDefaultCtor)]
public partial class AVAudioPcmBuffer {
}
