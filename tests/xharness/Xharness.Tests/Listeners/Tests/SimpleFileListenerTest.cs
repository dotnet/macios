﻿using System;
using System.IO;
using Moq;
using NUnit.Framework;
using Xharness.Listeners;
using Xharness.Logging;

namespace Xharness.Tests.Listeners.Tests {

	[TestFixture]
	public class SimpleFileListenerTest {

		string path;
		Mock<ILog> testLog;
		Mock<ILog> log;
		
		[SetUp]
		public void SetUp ()
		{
			path = Path.GetTempFileName ();
			testLog = new Mock<ILog> ();
			log = new Mock<ILog> ();
			File.Delete (path);
		}

		[TearDown]
		public void TearDown ()
		{
			if (File.Exists (path))
				File.Delete (path);
			path = null;
			testLog = null;
			log = null;
		}

		[Test]
		public void ConstructorTest ()
		{
			var listener = new SimpleFileListener (path);
			Assert.AreEqual (path, listener.Path);
		}

		[Test]
		public void ConstructorNullPathTest ()
		{
			Assert.Throws<ArgumentNullException> (() => new SimpleFileListener (null));
		}

		[TestCase ("Tests run: ", false)]
		[TestCase ("<!-- the end -->", true)]
		public void TestProcess (string endLine, bool isXml)
		{
			var lines = new string [] { "first line", "second line", "last line" };
			// set mock expectations
			testLog.Setup (l => l.WriteLine ("Tests have started executing"));
			testLog.Setup (l => l.WriteLine ("Tests have finished executing"));
			foreach (var line in lines) {
				testLog.Setup (l => l.WriteLine (line));
			}
			// create a listener, set the writer and ensure that what we write in the file is present in the final path
			using (var sourceWriter = new StreamWriter (path)) {
				var listener = new SimpleFileListener (path) {
					TestLog = testLog.Object,
					Log = log.Object,
					XmlOutput = isXml,
				};
				listener.Initialize ();
				listener.StartAsync ();
				// write a number of lines and ensure that those are called in the mock
				sourceWriter.WriteLine ("[Runner executing:");
				foreach (var line in lines) {
					sourceWriter.WriteLine (line);
					sourceWriter.Flush ();
				}
				sourceWriter.WriteLine (endLine);
				listener.Cancel ();
			}
			// verify that the expected lines were added
			foreach (var line in lines) {
				testLog.Verify (l => l.WriteLine (line), Times.Once);
			}
		}

	}
}
