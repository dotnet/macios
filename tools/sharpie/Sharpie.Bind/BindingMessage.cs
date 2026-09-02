// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text;
using Xamarin.Utils;

namespace Sharpie.Bind;

public class BindingMessage {
	public int Code;
	public string Message;
	public string? FileName;
	public long LineNumber;
	public bool IsError;

	static string PREFIX = "SHARPIE";

	BindingMessage (string message)
	{
		Message = message;
	}

	public BindingMessage (int code, params IEnumerable<object> arguments)
		: this (code, arguments, true)
	{
	}

	public BindingMessage (int code, IEnumerable<object> arguments, bool isError, CXSourceLocation? location = null)
	{
		Code = code;
		Message = GetMessage (code);
		if (arguments.Any ())
			Message = string.Format (Message, arguments.ToArray ());
		IsError = isError;

		if (location.HasValue && location.Value.TryGetPresumedLocation (out var presumedLoc)) {
			var loc = presumedLoc.Value;
			FileName = loc.FileName;
			LineNumber = loc.Line;
		}
	}

	static string GetMessage (int code)
	{
		var property = typeof (Resources).GetProperty ($"SHARPIE{code:D4}");
		if (property is not null) {
			var value = property.GetValue (null);
			if (value is string str && !string.IsNullOrEmpty (str))
				return str;
		}
		return Resources.ErrorMessageNotFound;
	}

	// http://blogs.msdn.com/b/msbuild/archive/2006/11/03/msbuild-visual-studio-aware-error-messages-and-message-formats.aspx
	public override string ToString ()
	{
		return StringUtils.FormatMessage (FileName, LineNumber, IsError, PREFIX, Code, Message);
	}

	public static BindingMessage? Parse (string line)
	{
		if (!StringUtils.TryParseFormattedMessage (line, out var fileName, out var lineNumber, out var isError, out var _, out var code, out var message))
			return null;

		return new BindingMessage (message) {
			Code = code,
			IsError = isError,
			FileName = fileName,
			LineNumber = lineNumber ?? 0,
		};
	}

	public void PrintMessage ()
	{
		if (IsError) {
			Console.Error.WriteLine (this);
		} else {
			Console.WriteLine (this);
		}
	}
}
