using Xamarin.Utils;

namespace Xamarin.Bundler;

public interface IToolLog {
	int Verbosity { get; }
	ApplePlatform Platform { get; }
	void Log (string message);
	void LogError (string message);
	// Log an error we raise ourselves (through an exception)
	void LogError (Exception exception);
	// Log an unexpected exception
	void LogException (Exception exception);
}

public static class IToolLogExtensions {
	public static void Log (this IToolLog log, string format, params object? [] args)
	{
		log.Log (string.Format (format, args));
	}

	public static void Log (this IToolLog log, int min_verbosity, string message)
	{
		if (min_verbosity > log.Verbosity)
			return;

		log.Log (message);
	}

	public static void Log (this IToolLog log, int min_verbosity, string format, params object? [] args)
	{
		if (min_verbosity > log.Verbosity)
			return;

		Log (log, format, args);
	}
}

#if !MSBUILD_TASKS
public class ConsoleLog : IToolLog {
	public readonly static IToolLog Instance = new ConsoleLog ();

#if TESTS
	int verbosity = 0;
#else
	int verbosity = Driver.GetDefaultVerbosity ();
#endif

	public int Verbosity { get => verbosity; }

	public ApplePlatform Platform => ApplePlatform.None;

	public void Log (string message)
	{
		Console.WriteLine (message);
	}

	public void LogError (string message)
	{
		Console.Error.WriteLine (message);
	}

	public void LogError (Exception exception)
	{
		Console.Error.WriteLine (exception);
	}

	public void LogException (Exception exception)
	{
		Console.Error.WriteLine (exception);
	}
}
#endif // !MSBUILD_TASKS
