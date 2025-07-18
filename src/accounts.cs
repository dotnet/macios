//
// Copyright 2011, 2013 Xamarin, Inc.
//

#nullable enable

using System;
using ObjCRuntime;
using Foundation;

namespace Accounts {

	/// <include file="../docs/api/Accounts/ACAccount.xml" path="/Documentation/Docs[@DocId='T:Accounts.ACAccount']/*" />
	[Deprecated (PlatformName.iOS, 15, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
	[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
	[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
	[BaseType (typeof (NSObject))]
	interface ACAccount : NSSecureCoding {
		[Export ("identifier", ArgumentSemantic.Weak)]
		string Identifier { get; }

		[NullAllowed] // by default this property is null
		[Export ("accountType", ArgumentSemantic.Retain)]
		ACAccountType AccountType { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("accountDescription", ArgumentSemantic.Copy)]
		string AccountDescription { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("username", ArgumentSemantic.Copy)]
		string Username { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("credential", ArgumentSemantic.Retain)]
		ACAccountCredential Credential { get; set; }

		[DesignatedInitializer]
		[Export ("initWithAccountType:")]
		NativeHandle Constructor (ACAccountType type);

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("userFullName")]
		string UserFullName { get; }
	}

	/// <summary>Encapsulates information needed to authenticate a user.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Accounts/Reference/ACAccountCredentialClassRef/index.html">Apple documentation for <c>ACAccountCredential</c></related>
	[Deprecated (PlatformName.iOS, 15, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
	[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
	[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
	[BaseType (typeof (NSObject))]
	interface ACAccountCredential : NSSecureCoding {
		[Export ("initWithOAuthToken:tokenSecret:")]
		NativeHandle Constructor (string oauthToken, string tokenSecret);

		[Export ("initWithOAuth2Token:refreshToken:expiryDate:")]
		NativeHandle Constructor (string oauth2Token, string refreshToken, NSDate expiryDate);

		[NullAllowed] // by default this property is null
		[Export ("oauthToken", ArgumentSemantic.Copy)]
		string OAuthToken { get; set; }
	}

	/// <summary>A delegate that specifies the completion handler in calls to the <see cref="Accounts.ACAccountStore.SaveAccount(Accounts.ACAccount,Accounts.ACAccountStoreSaveCompletionHandler)" /> method.</summary>
	delegate void ACAccountStoreSaveCompletionHandler (bool success, [NullAllowed] NSError error);
	/// <param name="success">
	///       <see langword="true" /> if the account was removed. Otherwise, <see langword="false" />.</param>
	///     <param name="error">The error that was encountered, or <see langword="null" /> if no error was encountered.</param>
	///     <summary>A handler to be run when an attempt is made to remove an account from the store.</summary>
	delegate void ACAccountStoreRemoveCompletionHandler (bool success, [NullAllowed] NSError error);
	/// <summary>A delegate that specifies the handler executed at the completion of calls to <see cref="Accounts.ACAccountStore.RequestAccessAsync(Accounts.ACAccountType,Foundation.NSDictionary)" />s.</summary>
	delegate void ACRequestCompletionHandler (bool granted, [NullAllowed] NSError error);

	/// <include file="../docs/api/Accounts/ACAccountStore.xml" path="/Documentation/Docs[@DocId='T:Accounts.ACAccountStore']/*" />
	[Deprecated (PlatformName.iOS, 15, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
	[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
	[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
	[BaseType (typeof (NSObject))]
	interface ACAccountStore {
		[Export ("accounts", ArgumentSemantic.Weak)]
		ACAccount [] Accounts { get; }

		[Export ("accountWithIdentifier:")]
		ACAccount FindAccount (string identifier);

		[Export ("accountTypeWithAccountTypeIdentifier:")]
		ACAccountType FindAccountType (string typeIdentifier);

		[Export ("accountsWithAccountType:")]
		ACAccount [] FindAccounts (ACAccountType accountType);

		[Export ("saveAccount:withCompletionHandler:")]
		[Async (XmlDocs = """
			<param name="account">The account to be saved.</param>
			<summary>Attempts to save an <see cref="Accounts.ACAccount" /> to the Accounts database.</summary>
			<returns>
			          <para>A task that represents the asynchronous SaveAccount operation.   The value of the TResult parameter is a Accounts.ACAccountStoreSaveCompletionHandler.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void SaveAccount (ACAccount account, ACAccountStoreSaveCompletionHandler completionHandler);

		[NoMac] // marked as unavailable in xcode10 beta 2
		[Export ("requestAccessToAccountsWithType:withCompletionHandler:")]
		[Deprecated (PlatformName.iOS, 6, 0, message: "Use 'RequestAccess (ACAccountType, AccountStoreOptions, ACRequestCompletionHandler)' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'RequestAccess (ACAccountType, AccountStoreOptions, ACRequestCompletionHandler)' instead.")]
		[Async (XmlDocs = """
			<param name="accountType">The type of account for which access is being requested.</param>
			<summary>Requests access to a type of social account.</summary>
			<returns>
			          <para>A task that represents the asynchronous RequestAccess operation.   The value of the TResult parameter is a Accounts.ACRequestCompletionHandler.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void RequestAccess (ACAccountType accountType, ACRequestCompletionHandler completionHandler);

		/// <include file="../docs/api/Accounts/ACAccountStore.xml" path="/Documentation/Docs[@DocId='P:Accounts.ACAccountStore.ChangeNotification']/*" />
		[Deprecated (PlatformName.iOS, 14, 0)]
		[Deprecated (PlatformName.MacOSX, 11, 0)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0)]
		[Field ("ACAccountStoreDidChangeNotification")]
		[Notification]
		NSString ChangeNotification { get; }

		[Export ("renewCredentialsForAccount:completion:")]
		[Async (XmlDocs = """
			<param name="account">The account whose credentials require renewing.</param>
			<summary>Attempts to renew credentials if they have become invalid.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous RenewCredentials operation.  The value of the TResult parameter is of type System.Action&lt;Accounts.ACAccountCredentialRenewResult,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void RenewCredentials (ACAccount account, Action<ACAccountCredentialRenewResult, NSError?> completionHandler);

		[Protected]
		[Export ("requestAccessToAccountsWithType:options:completion:")]
		[Async (XmlDocs = """
			<param name="accountType">To be added.</param>
			<param name="options">To be added.</param>
			<summary>To be added.</summary>
			<returns>To be added.</returns>
			<remarks>To be added.</remarks>
			""")]
		void RequestAccess (ACAccountType accountType, [NullAllowed] NSDictionary options, ACRequestCompletionHandler completion);

		/// <param name="accountType">The type of account for which access is being requested.</param>
		///         <param name="options">Options for accessing Facebook accounts or <see langword="null" />.</param>
		///         <param name="completion">The handler to be called when the method completes.</param>
		///         <summary>Requests access to a type of social account.</summary>
		///         <remarks>
		///           <para>Application developers can retrieve the <paramref name="accountType" /> object with the <see cref="Accounts.ACAccountStore.FindAccountType(System.String)" /> method.
		///           </para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// var objStore = new ACAccountStore();
		/// var options = new AccountStoreOptions();
		/// objStore.RequestAccess(objStore.FindAccountType(ACAccountType.Facebook), options, (granted, error) => { });
		/// ]]></code>
		///           </example>
		///         </remarks>
		///         <altmember cref="Accounts.AccountStoreOptions" />
		[Wrap ("RequestAccess (accountType, options.GetDictionary (), completion)")]
		[Async (XmlDocs = """
			<param name="accountType">To be added.</param>
			<param name="options">To be added.</param>
			<summary>To be added.</summary>
			<returns>To be added.</returns>
			<remarks>To be added.</remarks>
			""")]
		void RequestAccess (ACAccountType accountType, [NullAllowed] AccountStoreOptions options, ACRequestCompletionHandler completion);

		[Export ("removeAccount:withCompletionHandler:")]
		[Async (XmlDocs = """
			<param name="account">The account to remove.</param>
			<summary>Removes the specified <paramref name="account" /> from the account store, and runs a completion handler after the operation is complete.</summary>
			<returns>
			          <para>A task that represents the asynchronous RemoveAccount operation.   The value of the TResult parameter is a Accounts.ACAccountStoreRemoveCompletionHandler.</para>
			        </returns>
			<remarks>
			          <para copied="true">The RemoveAccountAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		void RemoveAccount (ACAccount account, ACAccountStoreRemoveCompletionHandler completionHandler);
	}

	/// <summary>A class that contains information about <see cref="Accounts.ACAccount" />s of a particular type.</summary>
	///     <remarks>
	///       <para>Application developers do not instantiate <see cref="Accounts.ACAccountType" /> directly. Rather, they can retrieve an appropriate object with the <see cref="Accounts.ACAccountStore.FindAccountType(System.String)" /> method.
	/// 	</para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// 		ACAccountStore objStore = new ACAccountStore();
	/// 		objStore.RequestAccess(objStore.FindAccountType(ACAccountType.Twitter), new AccountStoreOptions(), (granted, error) => { });
	/// 	]]></code>
	///       </example>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Accounts/Reference/ACAccountTypeClassRef/index.html">Apple documentation for <c>ACAccountType</c></related>
	[Deprecated (PlatformName.iOS, 15, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
	[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
	[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use the non-Apple SDK relating to your account type instead.")]
	[BaseType (typeof (NSObject))]
	interface ACAccountType : NSSecureCoding {
		[Export ("accountTypeDescription")]
		string Description { get; }

		[Export ("identifier")]
		string Identifier { get; }

		[Export ("accessGranted")]
		bool AccessGranted { get; }

		/// <summary>Represents the value associated with the constant ACAccountTypeIdentifierTwitter</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use Twitter SDK instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Twitter SDK instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Twitter SDK instead.")]
		[Field ("ACAccountTypeIdentifierTwitter")]
		NSString Twitter { get; }

		/// <summary>Represents the value associated with the constant ACAccountTypeIdentifierSinaWeibo</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use Sina Weibo SDK instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Sina Weibo SDK instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Sina Weibo SDK instead.")]
		[Field ("ACAccountTypeIdentifierSinaWeibo")]
		NSString SinaWeibo { get; }

		/// <summary>Developers should not use this deprecated property. Developers should use Facebook SDK instead.</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use Facebook SDK instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Facebook SDK instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Facebook SDK instead.")]
		[Field ("ACAccountTypeIdentifierFacebook")]
		NSString Facebook { get; }

		/// <summary>Represents the value associated with the constant ACAccountTypeIdentifierTencentWeibo</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use Tencent Weibo SDK instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Tencent Weibo SDK instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Tencent Weibo SDK instead.")]
		[Field ("ACAccountTypeIdentifierTencentWeibo")]
		NSString TencentWeibo { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoTV]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use LinkedIn SDK instead.")]
		[NoMacCatalyst]
		[Field ("ACAccountTypeIdentifierLinkedIn")]
		NSString LinkedIn { get; }
	}

	/// <summary>A class that encapsulates keys necessary for Facebook requests. Used with <see cref="Accounts.ACAccountStore.RequestAccess (Accounts.ACAccountType,Accounts.AccountStoreOptions,Accounts.ACRequestCompletionHandler)" />.</summary>
	[Deprecated (PlatformName.iOS, 11, 0, message: "Use Facebook SDK instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Facebook SDK instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Facebook SDK instead.")]
	[Static]
	interface ACFacebookKey {
		/// <summary>Represents the value associated with the constant ACFacebookAppIdKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ACFacebookAppIdKey")]
		NSString AppId { get; }

		/// <summary>Represents the value associated with the constant ACFacebookPermissionsKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ACFacebookPermissionsKey")]
		NSString Permissions { get; }

		// FIXME: does not exists in OSX 10.8 - which breaks our custom, higher level API for permissions
		/// <summary>Represents the value associated with the constant ACFacebookAudienceKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ACFacebookAudienceKey")]
		NSString Audience { get; }
	}

	/// <summary>An enumeration whose values specify the visibility of a post to Facebook.</summary>
	[Deprecated (PlatformName.iOS, 11, 0, message: "Use Facebook SDK instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Facebook SDK instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Facebook SDK instead.")]
	[Static]
	interface ACFacebookAudienceValue {
		/// <summary>Represents the value associated with the constant ACFacebookAudienceEveryone</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ACFacebookAudienceEveryone")]
		NSString Everyone { get; }

		/// <summary>Represents the value associated with the constant ACFacebookAudienceFriends</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ACFacebookAudienceFriends")]
		NSString Friends { get; }

		/// <summary>Represents the value associated with the constant ACFacebookAudienceOnlyMe</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ACFacebookAudienceOnlyMe")]
		NSString OnlyMe { get; }
	}

	/// <summary>Key to use when accessing Tencent Weibo accounts. Used with <see cref="Accounts.ACAccountStore.RequestAccess (Accounts.ACAccountType,Accounts.AccountStoreOptions,Accounts.ACRequestCompletionHandler)" />.</summary>
	[Deprecated (PlatformName.iOS, 11, 0, message: "Use Tencent Weibo SDK instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use Tencent Weibo SDK instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use Tencent Weibo SDK instead.")]
	[Static]
	interface ACTencentWeiboKey {
		/// <summary>Represents the value associated with the constant ACTencentWeiboAppIdKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ACTencentWeiboAppIdKey")]
		NSString AppId { get; }
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use LinkedIn SDK instead.")]
	[NoMacCatalyst]
	[Static]
	interface ACLinkedInKey {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("ACLinkedInAppIdKey")]
		NSString AppId { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("ACLinkedInPermissionsKey")]
		NSString Permissions { get; }
	}
}
