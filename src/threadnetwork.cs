using CoreFoundation;

namespace ThreadNetwork {

	delegate void THClientEnableCredentialSharingModeCompletionHandler ([NullAllowed] NSError error);

	/// <summary>Handler invoked with active credentials for nearby Thread networks.</summary>
	/// <param name="credentials">The active credentials, or <see langword="null" /> if they could not be retrieved.</param>
	/// <param name="error">The error, or <see langword="null" /> if the operation succeeded.</param>
	delegate void THClientRetrieveActiveCredentialsForNearbyNetworksCompletionHandler ([NullAllowed] NSSet<THCredentials> credentials, [NullAllowed] NSError error);

	[iOS (15, 0), MacCatalyst (16, 1), NoTV]
	[BaseType (typeof (NSObject))]
	interface THClient {
		[Async]
		[Export ("retrieveAllCredentials:")]
		void RetrieveAllCredentials (Action<NSSet<THCredentials>, NSError> completion);

		[iOS (16, 4), MacCatalyst (16, 4)]
		[Async]
		[Export ("retrieveAllActiveCredentials:")]
		void RetrieveAllActiveCredentials (Action<NSSet<THCredentials>, NSError> completion);

		/// <summary>Retrieves active credentials for nearby Thread networks.</summary>
		/// <param name="completion">The handler to invoke when the credentials are available.</param>
		[Mac (27, 0), iOS (27, 0), MacCatalyst (27, 0)]
		[Async (XmlDocs = """
			<summary>Asynchronously retrieves active credentials for nearby Thread networks.</summary>
			<returns>A task that represents the asynchronous retrieval operation.</returns>
			""")]
		[Export ("retrieveActiveCredentialsForNearbyNetworksWithCompletion:")]
		void RetrieveActiveCredentialsForNearbyNetworks (THClientRetrieveActiveCredentialsForNearbyNetworksCompletionHandler completion);

		[Async]
		[Export ("deleteCredentialsForBorderAgent:completion:")]
		void DeleteCredentialsForBorderAgent (NSData borderAgentId, Action<NSError> completion);

		[Async]
		[Export ("retrieveCredentialsForBorderAgent:completion:")]
		void RetrieveCredentialsForBorderAgent (NSData borderAgentId, Action<THCredentials, NSError> completion);

		[Async]
		[Export ("storeCredentialsForBorderAgent:activeOperationalDataSet:completion:")]
		void StoreCredentialsForBorderAgent (NSData borderAgentId, NSData activeOperationalDataSet, Action<NSError> completion);

		[Async]
		[Export ("retrievePreferredCredentials:")]
		void RetrievePreferredCredentials (Action<THCredentials, NSError> completion);

		[Async]
		[Export ("retrieveCredentialsForExtendedPANID:completion:")]
		void RetrieveCredentialsForExtendedPanId (NSData extendedPanId, Action<THCredentials, NSError> completion);

		[iOS (16, 0)] // was added in xcode14 targeting iOS 15, intro says otherthings.
		[MacCatalyst (16, 1)]
		[Async]
		[Export ("checkPreferredNetworkForActiveOperationalDataset:completion:")]
		void CheckPreferredNetwork (NSData activeOperationalDataSet, Action<bool> completion);

		[iOS (16, 4), MacCatalyst (16, 4)]
		[Async]
		[Export ("isPreferredNetworkAvailableWithCompletion:")]
		void IsPreferredNetworkAvailable (Action<bool> completion);

		[Mac (27, 0), iOS (27, 0), MacCatalyst (27, 0)]
		[Async]
		[Export ("enableCredentialSharingModeForExtendedPANID:completion:")]
		void EnableCredentialSharingMode (NSData extendedPanId, THClientEnableCredentialSharingModeCompletionHandler completion);
	}

	[iOS (15, 0), MacCatalyst (16, 1), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface THCredentials : NSSecureCoding {
		[NullAllowed, Export ("networkName")]
		string NetworkName { get; }

		[NullAllowed, Export ("extendedPANID")]
		NSData ExtendedPanId { get; }

		[NullAllowed, Export ("borderAgentID")]
		NSData BorderAgentId { get; }

		[NullAllowed, Export ("activeOperationalDataSet")]
		NSData ActiveOperationalDataSet { get; }

		[NullAllowed, Export ("networkKey")]
		NSData NetworkKey { get; }

		[NullAllowed, Export ("PSKC")]
		NSData Pskc { get; }

		[Export ("channel")]
		byte Channel { get; set; }

		[NullAllowed, Export ("panID")]
		NSData PanId { get; }

		[NullAllowed, Export ("creationDate")]
		NSDate CreationDate { get; }

		[NullAllowed, Export ("lastModificationDate")]
		NSDate LastModificationDate { get; }
	}

}
