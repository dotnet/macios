#if BGENERATOR
using ProductException = BindingException;
#endif

using Xamarin.Utils;

namespace Xamarin.Bundler;

public interface IToolLog {
	int Verbosity { get; }
	ApplePlatform Platform { get; }
	void Log (string message);
	void LogError (string message);
	// Log an error we raise ourselves (through an exception)
	void LogError (ProductException exception);
	void LogWarning (ProductException exception);
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
#elif BGENERATOR
	int verbosity = Driver.GetDefaultVerbosity ();
#else
	int verbosity = Driver.GetDefaultVerbosity (Driver.NAME);
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

	public void LogError (ProductException exception)
	{
		Console.Error.WriteLine (exception);
	}

	public void LogWarning (ProductException exception)
	{
		Console.WriteLine (exception);
	}

	public void LogException (Exception exception)
	{
		Console.Error.WriteLine (exception);
	}
}
#endif // !MSBUILD_TASKS
