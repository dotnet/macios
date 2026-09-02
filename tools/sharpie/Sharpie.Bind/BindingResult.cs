// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.CSharp;

namespace Sharpie.Bind;

public class BindingResult {
	public int Verbosity { get; set; }
	public int ExitCode { get; set; }
	public string BindingCode { get; set; } = "";
	public Dictionary<string, string> AdditionalFiles { get; set; } = new ();
	public IList<BindingMessage> Errors { get => Messages.Where (v => v.IsError).ToList (); }
	public IList<BindingMessage> Warnings { get => Messages.Where (v => !v.IsError).ToList (); }

	public List<BindingMessage> Messages { get; set; } = new ();

	public void ReportError (int code, params IEnumerable<object> arguments)
	{
		ReportError (code, (CXSourceLocation?) null, arguments);
	}

	public void ReportError (int code, CXSourceLocation? location, params IEnumerable<object> arguments)
	{
		var message = new BindingMessage (code, arguments, isError: true, location);
		ReportError (message);
	}

	public void ReportError (BindingMessage message)
	{
		Messages.Add (message);
		ExitCode = 1;
	}

	public void ReportWarning (int code, params IEnumerable<object> arguments)
	{
		ReportWarning (code, (CXSourceLocation?) null, arguments);
	}

	public void ReportWarning (int code, CXSourceLocation? location, params IEnumerable<object> args)
	{
		var message = new BindingMessage (code, args, isError: false, location);
		ReportWarning (message);
	}

	public void ReportWarning (BindingMessage message)
	{
		Messages.Add (message);
	}

	public void Log (int verbosity, string message, params IEnumerable<object> args)
	{
		if (verbosity > Verbosity)
			return;

		var msg = message;
		if (args.Any ())
			msg = string.Format (message, args.ToArray ());
		Console.WriteLine (msg);
	}

	public void ReportUnsupportedConstruct (CXSourceLocation? location, string message)
	{
		ReportWarning (1 /* Unsupported construct: {0}. Generated bindings may be incomplete. Please file an issue at https://github.com/dotnet/macios/issues. */, location, message.TrimEnd ('.'));
	}

	public void ReportUnexpectedError (Exception exception)
	{
		ReportError (0 /* An unexpected error occurred: {0}. Please fill a bug report at https://github.com/dotnet/macios/issues. */, exception.Message.TrimEnd ('.'));
	}

	public void ReportInternalError (CXSourceLocation? location, string message)
	{
		ReportError (99 /* Internal error: {0}. Please file an issue at https://github.com/dotnet/macios/issues. */, location, message);
	}

	public BindingException CreateInternalError (string message, CXSourceLocation? location = null)
	{
		return new BindingException (new BindingMessage (99 /* Internal error: {0}. Please file an issue at https://github.com/dotnet/macios/issues. */, [message], true, location));
	}

	public void ReportException (Exception exception)
	{
		if (exception is BindingException be) {
			if (be.BindingMessage.IsError) {
				ReportError (be.BindingMessage);
			} else {
				ReportWarning (be.BindingMessage);
			}
		} else if (exception is AggregateException ae) {
			foreach (var inner in ae.InnerExceptions)
				ReportException (inner);
		} else {
			ReportUnexpectedError (exception);
		}
	}

	public int PrintMessages ()
	{
		Messages.PrintMessages ();
		return ExitCode;
	}
}

public static class BindingResultExtensions {
	public static void PrintMessages (this IEnumerable<BindingMessage> messages)
	{
		foreach (var msg in messages) {
			msg.PrintMessage ();
		}
	}
}
