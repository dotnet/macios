//
#if IOS
using System;
using System.Drawing;
using System.Threading.Tasks;
using CoreMedia;
using CoreMotion;
using Foundation;
using ObjCRuntime;
using CoreAnimation;
using CoreLocation;

#nullable enable

namespace AVFoundation {

	public unsafe partial class AVOutputSettingsAssistant : NSObject {
		/// <summary>Preset for 640x480 output.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public AVOutputSettingsAssistant? Preset640x480 {
			get {
				return FromPreset (_Preset640x480);
			}
		}

		/// <summary>Preset for 960x540 output.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public AVOutputSettingsAssistant? Preset960x540 {
			get {
				return FromPreset (_Preset960x540);
			}
		}

		/// <summary>Preset for 1280x720 output.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public AVOutputSettingsAssistant? Preset1280x720 {
			get {
				return FromPreset (_Preset1280x720);
			}
		}

		/// <summary>Preset for 1920x1080 output.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public AVOutputSettingsAssistant? Preset1920x1080 {
			get {
				return FromPreset (_Preset1920x1080);
			}
		}

		/// <summary>Preset for 3840x2160 output.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AVOutputSettingsAssistant? Preset3840x2160 {
			get {
				return FromPreset (_Preset3840x2160);
			}
		}

		/// <summary>Preset for HVEC 1920x1080 output.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AVOutputSettingsAssistant? PresetHevc1920x1080 {
			get {
				return FromPreset (_PresetHevc1920x1080);
			}
		}

		/// <summary>Preset for HVEC 3840x2160 output</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AVOutputSettingsAssistant? PresetHevc3840x2160 {
			get {
				return FromPreset (_PresetHevc3840x2160);
			}
		}

	}
}

#endif // IOS
