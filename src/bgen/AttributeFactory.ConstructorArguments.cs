using System.Diagnostics.CodeAnalysis;

#nullable enable

public static partial class AttributeFactory {
	public static readonly Type PlatformEnum = typeof (PlatformName);

	static readonly Type [] ctorTypes_platform_message = new [] { PlatformEnum, typeof (string) };
	static readonly Type [] ctorTypes_platform_major_minor_message = new [] { PlatformEnum, typeof (int), typeof (int), typeof (string) };
	static readonly Type [] ctorTypes_platform_major_minor_build_message = new [] { PlatformEnum, typeof (int), typeof (int), typeof (int), typeof (string) };

	public readonly struct ConstructorArguments {
		readonly PlatformName platform;
		readonly int? major;
		readonly int? minor;
		readonly int? build;
		readonly string? message;

		public ConstructorArguments (PlatformName platformIn, int majorIn, int minorIn, int buildIn, string? messageIn)
		{
			platform = platformIn;
			major = majorIn;
			minor = minorIn;
			build = buildIn;
			message = messageIn;
		}

		public ConstructorArguments (PlatformName platformIn, int majorIn, int minorIn, string? messageIn)
		{
			platform = platformIn;
			major = majorIn;
			minor = minorIn;
			build = null;
			message = messageIn;
		}

		public ConstructorArguments (PlatformName platformIn, string? messageIn)
		{
			platform = platformIn;
			major = null;
			minor = null;
			build = null;
			message = messageIn;
		}

		public object? [] GetCtorValues ()
		{
			if (major is null || minor is null) {
				return new object? [] { (byte) platform, message };
			}

			if (build is null)
				return new object? [] { (byte) platform, major, minor, message };
			return new object? [] { (byte) platform, major, minor, build, message };
		}

		public Type [] GetCtorTypes ()
		{
			if (major is null || minor is null)
				return ctorTypes_platform_message;

			if (build is null)
				return ctorTypes_platform_major_minor_message;
			return ctorTypes_platform_major_minor_build_message;
		}

		public static bool TryGetCtorArguments (object [] constructorArguments, PlatformName platform, [NotNullWhen (true)] out object? []? ctorValues, [NotNullWhen (true)] out Type []? ctorTypes)
		{
			ctorValues = null;
			ctorTypes = null;

			switch (constructorArguments.Length) {
			case 2:
				if (constructorArguments [0] is byte &&
					constructorArguments [1] is byte) {
					ctorValues = new object? [] { (byte) platform, (int) (byte) constructorArguments [0], (int) (byte) constructorArguments [1], null };
					ctorTypes = ctorTypes_platform_major_minor_message;
					return true;
				}

				return false;
			case 3:
				if (constructorArguments [0] is byte &&
					constructorArguments [1] is byte &&
					constructorArguments [2] is byte) {
					ctorValues = new object? [] { (byte) platform, (int) (byte) constructorArguments [0], (int) (byte) constructorArguments [1], (int) (byte) constructorArguments [2], null };
					ctorTypes = ctorTypes_platform_major_minor_build_message;
					return true;
				}
				return false;
			default:
				return false;
			}
		}
	}
}
