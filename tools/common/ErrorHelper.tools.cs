// Copyright 2020, Microsoft Corp. All rights reserved,

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Mono.Cecil;
using Mono.Cecil.Cil;

using Xamarin.Utils;

#nullable enable

namespace Xamarin.Bundler {
	public static partial class ErrorHelper {
		internal static string GetPrefix (IToolLog? log)
		{
			switch (log?.Platform) {
			case ApplePlatform.iOS:
			case ApplePlatform.TVOS:
			case ApplePlatform.MacCatalyst:
			case ApplePlatform.None: // Return "MT" by default instead of throwing an exception, because any exception here will most likely hide whatever other error we're trying to show.
			case null:
				return "MT";
			case ApplePlatform.MacOSX:
				return "MM";
			default:
				// Do not use the ErrorHandler machinery, because it will probably end up recursing and eventually throwing a StackOverflowException.
				throw new InvalidOperationException ($"Unknown platform: {log.Platform}");
			}
		}

		public enum WarningLevel {
			Error = -1,
			Warning = 0,
			Disable = 1,
		}

		static ConditionalWeakTable<IToolLog, Dictionary<int, WarningLevel>> warning_levels = new ();

		public static Dictionary<int, WarningLevel>? GetWarningLevels (IToolLog log)
		{
			warning_levels.TryGetValue (log, out var log_warning_levels);
			return log_warning_levels;
		}

		public static WarningLevel GetWarningLevel (IToolLog log, int code)
		{
			if (warning_levels.TryGetValue (log, out var log_warning_levels)) {
				// code -1: all codes
				if (log_warning_levels.TryGetValue (-1, out var level))
					return level;

				if (log_warning_levels.TryGetValue (code, out level))
					return level;
			}

			return WarningLevel.Warning;
		}

		public static void SetWarningLevel (IToolLog log, WarningLevel level, int? code = null /* if null, apply to all warnings */)
		{
			if (!warning_levels.TryGetValue (log, out var log_warning_levels)) {
				log_warning_levels = new Dictionary<int, WarningLevel> ();
				warning_levels.Add (log, log_warning_levels);
			}
			if (code.HasValue) {
				log_warning_levels [code.Value] = level;
			} else {
				log_warning_levels [-1] = level; // code -1: all codes.
			}
		}

		public static void ParseWarningLevel (IToolLog log, WarningLevel level, string value)
		{
			if (string.IsNullOrEmpty (value)) {
				SetWarningLevel (log, level);
			} else {
				foreach (var code in value.Split (new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries))
					SetWarningLevel (log, level, int.Parse (code));
			}
		}

		public static void SetLocation (Application app, ProductException ex, MethodDefinition method, Instruction? instruction = null)
		{
			if (!method.HasBody)
				return;

			if (instruction is null && method.Body.Instructions.Count == 0)
				return;

			if (instruction is null)
				instruction = method.Body.Instructions [0];

			app.LoadSymbols ();

			if (!method.DebugInformation.HasSequencePoints)
				return;

			// Find the sequence point with the highest offset that is less than or equal to the instruction's offset
			SequencePoint? seq = null;
			foreach (var pnt in method.DebugInformation.SequencePoints) {
				if (pnt.Offset > instruction.Offset)
					continue;

				if (seq is not null && seq.Offset >= pnt.Offset)
					continue;

				seq = pnt;
			}
			if (seq is null)
				return;

			ex.FileName = seq.Document.Url;
			ex.LineNumber = seq.StartLine;
		}

		public static ProductException CreateError (Application app, int code, MemberReference? member, string message, params object? [] args)
		{
			return Create (app, code, true, null, member, null, message, args);
		}

		public static ProductException CreateError (Application app, int code, MethodDefinition? location, string message, params object? [] args)
		{
			return Create (app, code, true, null, location, null, message, args);
		}

		public static ProductException CreateError (Application app, int code, MethodDefinition? location, Instruction? instruction, string message, params object? [] args)
		{
			return Create (app, code, true, null, location, instruction, message, args);
		}

		public static ProductException CreateError (Application app, int code, ICustomAttributeProvider? provider, string message, params object? [] args)
		{
			return Create (app, code, true, null, provider, null, message, args);
		}

		public static ProductException CreateError (Application app, int code, Exception? innerException, MethodDefinition? location, string message, params object? [] args)
		{
			return Create (app, code, true, innerException, location, message, args);
		}

		public static ProductException CreateError (Application app, int code, Exception? innerException, TypeReference? location, string message, params object? [] args)
		{
			return Create (app, code, true, innerException, location, message, args);
		}

		public static ProductException CreateError (Application app, int code, Exception? innerException, ICustomAttributeProvider? provider, string message, params object? [] args)
		{
			return Create (app, code, true, innerException, provider, message, args);
		}

		public static ProductException CreateWarning (Application app, int code, MemberReference? member, string message, params object? [] args)
		{
			return Create (app, code, false, null, member, null, message, args);
		}

		public static ProductException CreateWarning (Application app, int code, MemberReference? member, Instruction? instruction, string message, params object? [] args)
		{
			return Create (app, code, false, null, member, instruction, message, args);
		}

		public static ProductException CreateWarning (Application app, int code, MethodDefinition? location, string message, params object? [] args)
		{
			return Create (app, code, false, null, location, message, args);
		}

		public static ProductException CreateWarning (Application app, int code, ICustomAttributeProvider? provider, string message, params object? [] args)
		{
			return Create (app, code, false, null, provider, message, args);
		}

		public static ProductException CreateWarning (Application app, int code, Exception? innerException, MethodDefinition? location, string message, params object? [] args)
		{
			return Create (app, code, false, innerException, location, message, args);
		}

		public static ProductException CreateWarning (Application app, int code, Exception? innerException, MethodDefinition? location, Instruction? instruction, string message, params object? [] args)
		{
			return Create (app, code, false, innerException, location, instruction, message, args);
		}

		public static ProductException CreateWarning (Application app, int code, Exception? innerException, TypeReference? location, string message, params object? [] args)
		{
			return Create (app, code, false, innerException, location, message, args);
		}

		public static ProductException CreateWarning (Application app, int code, Exception? innerException, ICustomAttributeProvider? provider, string message, params object? [] args)
		{
			return Create (app, code, false, innerException, provider, message, args);
		}

		public static ProductException Create (Application app, int code, bool error, Exception? innerException, ICustomAttributeProvider? provider, string message, params object? [] args)
		{
			return Create (app, code, error, innerException, provider, null, message, args);
		}

		public static ProductException Create (Application app, int code, bool error, Exception? innerException, ICustomAttributeProvider? provider, Instruction? instruction, string message, params object? [] args)
		{
			if (provider is MemberReference member) {
				if (instruction is not null)
					return Create (app, code, error, innerException, member, instruction, message, args);
				return Create (app, code, error, innerException, member, null, message, args);
			}

			if (provider is TypeReference type)
				return Create (app, code, error, innerException, type, message, args);

			return new ProductException (code, error, innerException, message, args);
		}

		public static ProductException Create (Application app, int code, bool error, Exception? innerException, MemberReference? member, Instruction? instruction, string message, params object? [] args)
		{
			var method = member as MethodReference;
			if (method is null) {
				var property = member as PropertyDefinition;
				if (property is not null) {
					method = property.GetMethod;
					if (method is null)
						method = property.SetMethod;
				}
			}
			return Create (app, code, error, innerException, method is null ? null : method.Resolve (), instruction, message, args);
		}

		public static ProductException Create (Application app, int code, bool error, Exception? innerException, MethodDefinition? location, Instruction? instruction, string message, params object? [] args)
		{
			var e = new ProductException (code, error, innerException, message, args);
			if (location is not null)
				SetLocation (app, e, location, instruction);
			return e;
		}

		public static ProductException Create (Application app, int code, bool error, Exception? innerException, TypeReference? location, string message, params object? [] args)
		{
			var e = new ProductException (code, error, innerException, message, args);
			if (location is not null) {
				var td = location.Resolve ();

				if (td.HasMethods) {
					foreach (var method in td.Methods) {
						if (!method.IsConstructor)
							continue;
						SetLocation (app, e, method);
						if (e.FileName is not null)
							break;
					}
				}
			}
			return e;
		}

		public static void Warning (IToolLog log, int code, string message, params object [] args)
		{
			Show (log, new ProductException (code, false, null, message, args));
		}

		public static void Warning (IToolLog log, int code, Exception innerException, string message, params object [] args)
		{
			Show (log, new ProductException (code, false, innerException, message, args));
		}

		// Shows any warnings, and if there are any errors, throws an AggregateException.
		public static void ThrowIfErrors (IToolLog log, IList<Exception> exceptions)
		{
			if (exceptions?.Any () != true)
				return;

			// Separate warnings from errors
			var grouped = exceptions.GroupBy ((v) => (v as ProductException)?.IsError (log) == false);

			var warnings = grouped.SingleOrDefault ((v) => v.Key);
			if (warnings?.Any () == true)
				Show (log, warnings);

			var errors = grouped.SingleOrDefault ((v) => !v.Key);
			if (errors?.Any () == true)
				throw new AggregateException (errors);
		}

		public static void Show (IToolLog log, IEnumerable<Exception> list)
		{
			var exceptions = CollectExceptions (list);
			bool error = false;

			foreach (var ex in exceptions)
				error |= ShowInternal (log, ex);

			if (error)
				Exit (1);
		}

		public static void Show (IToolLog log, Exception e)
		{
			Show (log, new Exception [] { e });
		}

		static void Exit (int exitCode)
		{
			Environment.Exit (exitCode);
		}

		static bool ShowInternal (IToolLog log, Exception e)
		{
			var mte = e as ProductException;
			bool error = true;

			if (mte is not null) {
				error = mte.IsError (log);

				if (!error && GetWarningLevel (log, mte.Code) == WarningLevel.Disable)
					return false; // This is an ignored warning.

				log.LogError (mte.ToString ());

				ShowInner (log, e);

				if (log.Verbosity > 2 && !string.IsNullOrEmpty (e.StackTrace))
					log.LogError (e.StackTrace);
			} else {
				log.LogError ("error " + GetPrefix (log) + "0000: Unexpected error - Please file a bug report at https://github.com/dotnet/macios/issues/new");
				log.LogError (e.ToString ());
			}

			return error;
		}

		static void ShowInner (IToolLog log, Exception e)
		{
			var ie = e.InnerException;
			if (ie is null)
				return;

			if (log.Verbosity > 3) {
				log.LogError ("--- inner exception");
				log.LogError (ie.ToString ());
				log.LogError ("---");
			} else if (log.Verbosity > 0 || ie is ProductException) {
				log.LogError ($"\t{ie.Message}");
			}
			ShowInner (log, ie);
		}
	}
}
