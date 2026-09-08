// Copyright 2020, Microsoft Corp. All rights reserved,

#nullable enable

using System.Linq;
using System.Runtime.CompilerServices;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Xamarin.Utils;

namespace Xamarin.Bundler {
	public static partial class ErrorHelper {
		public static ApplePlatform Platform;

		internal static string GetPrefix (IToolLog? log)
		{
			return "TESTS";
		}

		public enum WarningLevel {
			Error = -1,
			Warning = 0,
			Disable = 1,
		}

		static ConditionalWeakTable<IToolLog, Dictionary<int, WarningLevel>> warning_levels = new ();

		public static bool TryGetWarningLevel (IToolLog log, int code, out WarningLevel warningLevel)
		{
			warningLevel = default;

			if (warning_levels.TryGetValue (log, out var log_warning_levels)) {
				// code -1: all codes
				if (log_warning_levels.TryGetValue (-1, out warningLevel))
					return true;

				if (log_warning_levels.TryGetValue (code, out warningLevel))
					return true;
			}

			return false;
		}

		public static WarningLevel GetWarningLevel (IToolLog log, int code)
		{
			if (TryGetWarningLevel (log, code, out var warningLevel))
				return warningLevel;

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
	}
}
