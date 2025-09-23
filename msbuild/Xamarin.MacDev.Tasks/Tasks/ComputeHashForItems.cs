#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	// This task will iterate over each input item, compute a hash value for all the specified metadata in the input items, and then set the specified output metadata to the hashed value
	public class ComputeHashForItems : XamarinTask {
		[Required]
		public ITaskItem [] Input { get; set; } = Array.Empty<ITaskItem> ();

		// The metadata in each input item to use as input for the hash algorithm.
		[Required]
		public ITaskItem [] InputMetadata { get; set; } = Array.Empty<ITaskItem> ();

		// The name of the metadata where to store the computed hashed value
		[Required]
		public string OutputMetadata { get; set; } = string.Empty;

		// The output items. This will be Input, where each item will also have 'OutputMetadata' set to the computed hash value.
		[Output]
		public ITaskItem [] Output { get; set; } = Array.Empty<ITaskItem> ();

		public override bool Execute ()
		{
			if (Input.Length == 0)
				return true;

			using var sha = CreateHashAlgorithm ();

			var buffer = new List<byte> ();
			for (var i = 0; i < Input.Length; i++) {
				var input = Input [i];
				buffer.Clear ();
				foreach (var im in InputMetadata) {
					buffer.AddRange (Encoding.UTF8.GetBytes (input.GetMetadata (im.ItemSpec)));
				}
				var hashBytes = sha.ComputeHash (buffer.ToArray ());
				var hash = string.Join ("", hashBytes.Select (b => $"{b:x2}"));
				input.SetMetadata (OutputMetadata, hash);
			}

			Output = Input;

			return !Log.HasLoggedErrors;
		}

		HashAlgorithm CreateHashAlgorithm ()
		{
			return SHA256.Create ();
		}
	}
}
