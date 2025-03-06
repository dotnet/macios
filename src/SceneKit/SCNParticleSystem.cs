//
// SCNParticleSystem.cs: extensions to SCNParticleSystem
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright Xamarin Inc.
//

using System;
using System.Collections;
using System.Collections.Generic;

using Foundation;
using System.Runtime.Versioning;

#nullable enable

namespace SceneKit {
#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	public class SCNPropertyControllers {
		NSMutableDictionary? mutDict;
		internal NSDictionary dict;

		internal SCNPropertyControllers (NSDictionary dict)
		{
			this.dict = dict;
			if (dict is NSMutableDictionary)
				mutDict = (NSMutableDictionary) dict;
		}

		public SCNPropertyControllers ()
		{
			mutDict = new NSMutableDictionary ();
			dict = mutDict;
		}

		internal void Set (NSString key, SCNParticlePropertyController? value)
		{
			if (mutDict is null) {
				mutDict = new NSMutableDictionary (dict);
				dict = mutDict;
			}
			mutDict [key] = value;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? Position {
			get {
				return dict [SCNParticleProperty.Position] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.Position, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? Angle {
			get {
				return dict [SCNParticleProperty.Angle] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.Angle, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? RotationAxis {
			get {
				return dict [SCNParticleProperty.RotationAxis] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.RotationAxis, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? Velocity {
			get {
				return dict [SCNParticleProperty.Velocity] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.Velocity, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? AngularVelocity {
			get {
				return dict [SCNParticleProperty.AngularVelocity] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.AngularVelocity, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? Life {
			get {
				return dict [SCNParticleProperty.Life] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.Life, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? Color {
			get {
				return dict [SCNParticleProperty.Color] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.Color, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? Opacity {
			get {
				return dict [SCNParticleProperty.Opacity] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.Opacity, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? Size {
			get {
				return dict [SCNParticleProperty.Size] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.Size, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? Frame {
			get {
				return dict [SCNParticleProperty.Frame] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.Frame, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? FrameRate {
			get {
				return dict [SCNParticleProperty.FrameRate] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.FrameRate, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? Bounce {
			get {
				return dict [SCNParticleProperty.Bounce] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.Bounce, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? Charge {
			get {
				return dict [SCNParticleProperty.Charge] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.Charge, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNParticlePropertyController? Friction {
			get {
				return dict [SCNParticleProperty.Friction] as SCNParticlePropertyController;
			}
			set {
				Set (SCNParticleProperty.Friction, value);
			}
		}

	}

	public partial class SCNParticleSystem {
		/// <summary>Gets or sets a dictionary of controllers that animate different properties of particles in the system.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNPropertyControllers? PropertyControllers {
			get {
				var weak = WeakPropertyControllers;
				if (weak is null)
					return null;
				return new SCNPropertyControllers (weak);
			}
			set {
				WeakPropertyControllers = value?.dict;
			}
		}
	}
}
