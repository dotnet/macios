// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Sharpie.Bind.Tests;

using System.Diagnostics;
using System.Drawing;
using ICSharpCode.NRefactory.CSharp;
using Sharpie.Bind;
using Xamarin.Tests;
using Xamarin.Utils;

public static class Extensions {

	static Dictionary<ApplePlatform, string> platformAssemblyPaths = new ();
	public static string GetPlatformAssemblyPath (ApplePlatform platform)
	{
		if (!platformAssemblyPaths.TryGetValue (platform, out var path))
			platformAssemblyPaths [platform] = path = Configuration.GetBaseLibrary (platform);
		return path;
	}

	public static string GetClangResourceDirectory ()
	{
		return Path.Combine (Configuration.SourceRoot, "tools", "sharpie", "clang");
	}

	// Running out-of-process is useful when there's something causing clang to crash, in which case
	// running out-of-process will only fail the single test that's running, and not the entire test run.
	public static bool UseOutOfProcessBinding = true;

	public static BindingResult BindInOrOut (this ObjectiveCBinder binder)
	{
		BindingResult rv;
		if (UseOutOfProcessBinding && !Debugger.IsAttached) {
			rv = binder.ExecuteOutOfProcess ();
		} else {
			rv = binder.Bind ();
		}

		return rv;
	}

	/// <summary>
	/// Asserts that the binding was successful and that the generated binding code matches the expected binding code.
	/// </summary>
	/// <param name="bindings"></param>
	/// <param name="expectedBindingCode"></param>
	/// <param name="additionalInfo"></param>
	public static void AssertSuccess (this BindingResult bindings, string? expectedBindingCode, params string [] additionalInfo)
	{
		bindings.Warnings.ForEach (Console.WriteLine);
		bindings.Errors.ForEach (Console.Error.WriteLine);

		if (bindings.ExitCode == 139) {
			Console.Error.WriteLine ("Binding process crashed with exit code 139 (SIGSEGV).");
			Assert.Fail ("Binding process crashed with exit code 139 (SIGSEGV).");
		}

		Assert.That (bindings.ExitCode, Is.EqualTo (0), "Expected success");
		Assert.That (bindings.Errors.Count, Is.EqualTo (0), $"Unexpected number of errors\n\t{string.Join ("\n\t", bindings.Errors)}");

		if (expectedBindingCode is null)
			return;

		var expected = expectedBindingCode.Replace ("\r\n", "\n").Trim ();
		var actual = bindings.BindingCode?.Replace ("\r\n", "\n").Trim () ?? "";
		if (expected != actual) {
			const string splitter = "===========================";
			Console.WriteLine ("Expected Binding Code:");
			Console.WriteLine (splitter);
			Console.WriteLine (expected);
			Console.WriteLine (splitter);
			Console.WriteLine ("Actual Binding Code:");
			Console.WriteLine (splitter);
			Console.WriteLine (actual);
			Console.WriteLine (splitter);
			var diff = ComputeDiff (expected, actual).Split ('\n');
			Console.WriteLine ("Diff:");
			Console.WriteLine (splitter);
			foreach (var line in diff) {
				var colorized = false;
				if (line.Length >= 2) {
					if (line [0] == '+' && line [1] != '+') {
						Console.Write (Color.Green);
						colorized = true;
					} else if (line [0] == '-' && line [1] != '-') {
						Console.Write (Color.Red);
						colorized = true;
					}
				}
				Console.Write (line);
				if (colorized)
					Console.Write (Color.Reset);
				Console.WriteLine ();
			}
			Console.WriteLine (splitter);
			foreach (var line in additionalInfo)
				Console.WriteLine (line);
			Assert.That (actual, Is.EqualTo (expected), "Expected binding code");
		}
	}

	static string ComputeDiff (string a, string b)
	{
		var fa = Path.Combine (Path.GetTempPath (), "sharpie", "tests", "a.cs");
		var fb = Path.Combine (Path.GetTempPath (), "sharpie", "tests", "b.cs");
		if (File.Exists (fa))
			File.Delete (fa);
		if (File.Exists (fb))
			File.Delete (fb);
		Directory.CreateDirectory (Path.GetDirectoryName (fa)!);
		File.WriteAllText (fa, a);
		File.WriteAllText (fb, b);

		var rv = Execution.RunAsync ("diff", new [] { "-u", fa, fb }, timeout: TimeSpan.FromSeconds (10)).Result;

		File.Delete (fa);
		File.Delete (fb);

		return rv.Output.MergedOutput;
	}

	public static void AssertErrors (this BindingResult bindings, params (int Code, string Message, string? FileName, long? LineNumber) [] expectedErrors)
	{
		Assert.That (bindings.ExitCode, Is.EqualTo (1), "Expected failure");
		Assert.That (bindings.BindingCode, Is.Empty.Or.Null, "Expected no binding code");
		AssertMessages ("error", bindings.Errors, expectedErrors);
	}

	public static void AssertWarnings (this BindingResult bindings, params (int Code, string Message, string? FileName, long? LineNumber) [] expectedWarnings)
	{
		AssertMessages ("warning", bindings.Warnings, expectedWarnings);
	}

	public static void AssertNoWarnings (this BindingResult bindings)
	{
		AssertWarnings (bindings);
	}

	static void AssertMessages (string type, IList<BindingMessage> messages, params (int Code, string Message, string? FileName, long? LineNumber) [] expectedMessages)
	{
		Assert.That (messages.Count, Is.EqualTo (expectedMessages.Length), $"Unexpected number of {type}s ({messages.Count})\n\t{string.Join ("\n\t", messages)}");
		for (int i = 0; i < expectedMessages.Length; i++) {
			Assert.That (messages [i].Code, Is.EqualTo (expectedMessages [i].Code), $"Code for {type} #{i}");
			Assert.That (messages [i].Message, Is.EqualTo (expectedMessages [i].Message), $"Message for {type} #{i}");
			if (expectedMessages [i].FileName is not null)
				Assert.That (messages [i].FileName, Is.EqualTo (expectedMessages [i].FileName), $"FileName for {type} #{i}");
			if (expectedMessages [i].LineNumber is not null)
				Assert.That (messages [i].LineNumber, Is.EqualTo (expectedMessages [i].LineNumber!.Value), $"LineNumber for {type} #{i}");
		}
	}
}

static class Color {
	internal const char ESC_Char = '\u001b';
	internal const string ESC = $"\u001b"; /* \033 - https://github.com/dotnet/csharplang/issues/7400 */
	internal const string ST = ESC + "\\";
	internal const string OSC = ESC + "]";

	public const string DarkBlack = ESC + "[30m";
	public const string DarkRed = ESC + "[31m";
	public const string DarkGreen = ESC + "[32m";
	public const string DarkYellow = ESC + "[33m";
	public const string DarkBlue = ESC + "[34m";
	public const string DarkMagenta = ESC + "[35m";
	public const string DarkCyan = ESC + "[36m";
	public const string DarkWhite = ESC + "[37m";

	public const string Black = ESC + "[90m";
	public const string Red = ESC + "[91m";
	public const string Green = ESC + "[92m";
	public const string Yellow = ESC + "[93m";
	public const string Blue = ESC + "[94m";
	public const string Magenta = ESC + "[95m";
	public const string Cyan = ESC + "[96m";
	public const string White = ESC + "[97m";
	public const string Reset = ESC + "[0m";

	public const string Bold = ESC + "[1m";
	public const string Italicize = ESC + "[3m";
	public const string Underline = ESC + "[4m";
	public const string Strikethrough = ESC + "[9m";

	public const string NotBold = ESC + "[22m";
	public const string NotItalic = ESC + "[23m";
	public const string NotUnderline = ESC + "[24m";
	public const string NotStrikethrough = ESC + "[29m";
}
