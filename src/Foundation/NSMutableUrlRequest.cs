namespace Foundation {

	public partial class NSUrlRequest {
		/// <param name="key">HTTP Header Name.</param>
		/// <summary>Gets the value of the specified HTTP header.</summary>
		/// <value>To be added.</value>
		/// <remarks>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// void DumpLenght (NSUrlRequest request)
		/// {
		///     Console.WriteLine ("The content lenght is: {0}", request ["Content-Length"]);
		/// }
		/// 	    ]]></code>
		///           </example>
		///         </remarks>
		public string this [string key] {
			get {
				return Header (key);
			}
		}
	}

	public partial class NSMutableUrlRequest {
		/// <param name="key">HTTP Header Name.</param>
		/// <summary>Gets or sets the value of the specified HTTP header.</summary>
		/// <value>
		///         </value>
		/// <remarks>
		///           <para>
		/// 	    Use this indexer value to set or get the value of a specific HTTP header.
		/// 	  </para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// void SetHeader (NSMutableUrlRequest request)
		/// {
		///     request ["User-Agent"] = "XamarinMac Spider Program";
		/// }
		/// 	    ]]></code>
		///           </example>
		///         </remarks>
		public new string this [string key] {
			get {
				return Header (key);
			}

			set {
				_SetValue (value, key);
			}
		}
	}
}
