#nullable enable

#if __IOS__

namespace StoreKit {

	partial class SKCloudServiceSetupOptions {

		/// <summary>Gets or sets the setup action.</summary>
		/// <value>The cloud service setup action, or <see langword="null" /> if not set.</value>
		public virtual SKCloudServiceSetupAction? Action {
			get {
				if (_Action is null)
					return null;
				return (SKCloudServiceSetupAction?) (SKCloudServiceSetupActionExtensions.GetValue (_Action));
			}
			set {
				_Action = value is not null ? SKCloudServiceSetupActionExtensions.GetConstant (value.Value) : null;
			}
		}
	}
}

#endif // __IOS__
