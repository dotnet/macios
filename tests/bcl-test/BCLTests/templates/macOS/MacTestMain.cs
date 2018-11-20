using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
#if XAMCORE_2_0 || __UNIFIED__
using AppKit;
using Foundation;
#else
using MonoMac.AppKit;
using MonoMac.Foundation;
#endif
using BCLTests;
using Xamarin.iOS.UnitTests;
using Xamarin.iOS.UnitTests.NUnit;
using BCLTests.TestRunner.Core;
using Xamarin.iOS.UnitTests.XUnit;



namespace Xamarin.Mac.Tests
{
	static class MainClass
	{
		static void Main (string[] args)
		{
			NSApplication.Init();
			RunTests (args);
		}

		internal static IEnumerable<TestAssemblyInfo> GetTestAssemblies ()
 		{
			// var t = Path.GetFileName (typeof (ActivatorCas).Assembly.Location);
			foreach (var name in RegisterType.TypesToRegister.Keys) {
				var a = RegisterType.TypesToRegister [name].Assembly;
				yield return new TestAssemblyInfo (a, name);
			}
 		}
 		
		static void RunTests (string [] original_args)
		{
			Console.WriteLine ("Running tests");
			var options = ApplicationOptions.Current;
			TcpTextWriter writer = null;
			if (!string.IsNullOrEmpty (options.HostName))
				writer = new TcpTextWriter (options.HostName, options.HostPort);

			// we generate the logs in two different ways depending if the generate xml flag was
			// provided. If it was, we will write the xml file to the tcp writer if present, else
			// we will write the normal console output using the LogWriter
			var logger = new LogWriter (Console.Out); 
			logger.MinimumLogLevel = MinimumLogLevel.Info;
			var testAssemblies = GetTestAssemblies ();
			TestRunner runner;
			if (RegisterType.IsXUnit)
				runner = new XUnitTestRunner (logger);
			else
				runner = new NUnitTestRunner (logger);
			
			runner.Run (testAssemblies.ToList ());
			if (options.EnableXml) {
				runner.WriteResultsToFile (writer);
				logger.Info ("Xml file was written to the tcp listener.");
			} else {
				string resultsFilePath = runner.WriteResultsToFile ();
				logger.Info ($"Xml result can be found {resultsFilePath}");
			}
			
			logger.Info ($"Tests run: {runner.TotalTests} Passed: {runner.PassedTests} Inconclusive: {runner.InconclusiveTests} Failed: {runner.FailedTests} Ignored: {runner.SkippedTests}");
			if (options.TerminateAfterExecution)
				_exit (0);			
		}

		[DllImport ("/usr/lib/libSystem.dylib")]
		static extern void _exit (int exit_code);
	}
}
