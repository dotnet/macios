﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace xharness
{
	public class DeviceLogCapturer
	{
		public Harness Harness;
		public Log Log;
		public string DeviceName;

		Process process;
		CountdownEvent streamEnds;

		public void StartCapture ()
		{
			streamEnds = new CountdownEvent (2);

			process = new Process ();
			process.StartInfo.FileName = Harness.MlaunchPath;
			var sb = new StringBuilder ();
			sb.Append ("--logdev ");
			sb.Append ("--sdkroot ").Append (Harness.Quote (Harness.XcodeRoot)).Append (' ');
			AppRunner.AddDeviceName (sb, DeviceName);
			process.StartInfo.Arguments = sb.ToString ();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => {
				if (e.Data == null) {
					streamEnds.Signal ();
				} else {
					lock (Log) {
						Log.WriteLine (e.Data);
					}
				}
			};
			process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => {
				if (e.Data == null) {
					streamEnds.Signal ();
				} else {
					lock (Log) {
						Log.WriteLine (e.Data);
					}
				}
			};
			Log.WriteLine ("{0} {1}", process.StartInfo.FileName, process.StartInfo.Arguments);
			process.Start ();
			process.BeginOutputReadLine ();
			process.BeginErrorReadLine ();
		}

		public void StopCapture ()
		{
			if (process.HasExited)
				return;
			
			process.Kill ();
			if (!streamEnds.Wait (TimeSpan.FromSeconds (5))) {
				Harness.Log ("Could not kill 'mtouch --logdev' process in 5 seconds.");
			}
			process.Dispose ();
		}
	}
}

