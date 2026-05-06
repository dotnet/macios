// Copyright 2015 Xamarin, Inc.

#nullable enable

namespace Foundation {

#if MONOMAC || __MACCATALYST__

	// The keys are not found in any of the public headers from Apple. That is the reason
	// to use this technique.
	static class NSScriptCommonKeys {
		static readonly NSString appEventCode = new NSString ("AppleEventCode");
		public static NSString AppleEventCodeKey {
			get { return appEventCode; }
		}

		static readonly NSString typeKey = new NSString ("Type");
		public static NSString TypeKey {
			get { return typeKey; }
		}
	}

	public partial class NSScriptCommandDescription {

		NSScriptCommandDescriptionDictionary? description;

		static int ToIntValue (string fourCC)
		{
			if (fourCC.Length != 4)
				throw new FormatException (string.Format ("{0} must have a length of 4", nameof (fourCC)));
			int ret = 0;
			for (int i = 0; i < 4; i++) {
				ret <<= 8;
				ret |= fourCC [i];
			}
			return ret;
		}

		/// <summary>Creates a new <see cref="NSScriptCommandDescription" /> with the specified suite name, command name, and command declaration.</summary>
		/// <param name="suiteName">The name of the suite to which the command belongs.</param>
		/// <param name="commandName">The name of the command.</param>
		/// <param name="commandDeclaration">A dictionary containing the command declaration.</param>
		/// <returns>A new <see cref="NSScriptCommandDescription" /> instance.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="suiteName" /> or <paramref name="commandName" /> is null or empty, or when the command declaration is malformed.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="commandDeclaration" /> is <see langword="null" />.</exception>
		public static NSScriptCommandDescription Create (string suiteName, string commandName, NSScriptCommandDescriptionDictionary commandDeclaration)
		{
			if (String.IsNullOrEmpty (suiteName))
				throw new ArgumentException ("suiteName cannot be null or empty");
			if (String.IsNullOrEmpty (commandName))
				throw new ArgumentException ("commandName cannot be null or empty");
			ArgumentNullException.ThrowIfNull (commandDeclaration);

			// ensure that the passed description is well formed
			if (String.IsNullOrEmpty (commandDeclaration.CommandClass))
				throw new ArgumentException ("cmdClass");
			if (String.IsNullOrEmpty (commandDeclaration.AppleEventCode))
				throw new ArgumentException ("eventCode");
			if (commandDeclaration.AppleEventCode.Length != 4)
				throw new ArgumentException ("eventCode must be a four characters string.");
			if (String.IsNullOrEmpty (commandDeclaration.AppleEventClassCode))
				throw new ArgumentException ("eventClass");
			if (commandDeclaration.AppleEventClassCode.Length != 4)
				throw new ArgumentException ("eventClass must be a four characters string.");
			if (commandDeclaration.ResultAppleEventCode is not null && commandDeclaration.ResultAppleEventCode.Length != 4)
				throw new ArgumentException ("resultAppleEvent must be a four characters string.");

			using (var nsSuitName = new NSString (suiteName))
			using (var nsCommandName = new NSString (commandName)) {
				try {
					var cmd = new NSScriptCommandDescription (nsSuitName, nsCommandName, commandDeclaration.Dictionary);
					cmd.description = commandDeclaration;
					return cmd;
				} catch (Exception e) {
					// this exception is raised by the platform because the internal constructor returns a nil
					// from the docs we know:
					// 
					// Returns nil if the event constant or class name for the command description is missing; also returns nil
					// if the return type or argument values are of the wrong type.
					// 
					// the conclusion is that the user created a wrong description dict, we let him know
					throw new ArgumentException ("commandDeclaration",
						"Wrong description dictionary: Check that the event constant, class name and argument definitions are well formed as per apple documentation.", e);
				}
			}
		}

		/// <summary>Gets the Apple event class code for the command.</summary>
		/// <value>A four-character string representing the Apple event class code.</value>
		public string AppleEventClassCode {
			get { return Runtime.ToFourCCString (FCCAppleEventClassCode); }
		}

		/// <summary>Gets the Apple event code for the command.</summary>
		/// <value>A four-character string representing the Apple event code.</value>
		public string AppleEventCode {
			get { return Runtime.ToFourCCString (FCCAppleEventCode); }
		}

		/// <summary>Gets the type for the specified argument.</summary>
		/// <param name="name">The name of the argument.</param>
		/// <returns>The type of the argument, or <see langword="null" /> if the argument is not found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="name" /> is <see langword="null" />.</exception>
		public string? GetTypeForArgument (string name)
		{
			ArgumentNullException.ThrowIfNull (name);

			using (var nsName = new NSString (name))
			using (var nsType = GetNSTypeForArgument (nsName)) {
				return nsType?.ToString ();
			}
		}

		/// <summary>Gets the Apple event code for the specified argument.</summary>
		/// <param name="name">The name of the argument.</param>
		/// <returns>A four-character string representing the Apple event code for the argument.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="name" /> is <see langword="null" />.</exception>
		public string GetAppleEventCodeForArgument (string name)
		{
			ArgumentNullException.ThrowIfNull (name);

			using (var nsName = new NSString (name)) {
				return Runtime.ToFourCCString (FCCAppleEventCodeForArgument (nsName));
			}
		}

		/// <summary>Determines whether the specified argument is optional.</summary>
		/// <param name="name">The name of the argument.</param>
		/// <returns><see langword="true" /> if the argument is optional; otherwise, <see langword="false" />.</returns>
		public bool IsOptionalArgument (string name)
		{
			using (var nsName = new NSString (name)) {
				return NSIsOptionalArgument (nsName);
			}
		}

		/// <summary>Gets the Apple event code for the return type.</summary>
		/// <value>A four-character string representing the Apple event code for the return type.</value>
		public string AppleEventCodeForReturnType {
			get { return Runtime.ToFourCCString (FCCAppleEventCodeForReturnType); }
		}

		/// <summary>Creates a new <see cref="NSScriptCommand" /> instance for this command description.</summary>
		/// <returns>A new <see cref="NSScriptCommand" /> instance.</returns>
		public NSScriptCommand CreateCommand ()
		{
			return new NSScriptCommand (CreateCommandInstancePtr ());
		}

		/// <summary>Gets the dictionary containing the command description.</summary>
		/// <value>An <see cref="NSDictionary" /> containing the command description.</value>
		public NSDictionary? Dictionary {
			get { return description?.Dictionary; }
		}
	}
#endif

}
