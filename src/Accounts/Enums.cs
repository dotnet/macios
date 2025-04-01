using System;
using ObjCRuntime;

#nullable enable

namespace Accounts {

	// untyped enum -> ACError.h
	/// <summary>An enumeration whose values indicate various errors relating to accessing accounts.</summary>
	///     
	///     <!-- TODO: Confirm that this value is used somewhere -->
	[ErrorDomain ("ACErrorDomain")]
	public enum ACErrorCode {
		/// <summary>Indicates that an unknown error occurred.</summary>
		Unknown = 1,
		/// <summary>Indicates the the account was not saved because it was missing a required property.</summary>
		AccountMissingRequiredProperty,
		/// <summary>Indicates that the account was not saved because authentication of its credentials failed.</summary>
		AccountAuthenticationFailed,
		/// <summary>Indicates that the account was not saved because it was of an invalid type.</summary>
		AccountTypeInvalid,
		/// <summary>Indicates that an attempt was made to add an account that already exists.</summary>
		AccountAlreadyExits,
		/// <summary>Indicates that the account was not found, and therefore could not be deleted.</summary>
		AccountNotFound,
		/// <summary>Indicates that the application did not have permission to complete the operation.</summary>
		PermissionDenied,
		/// <summary>Indicates that the client access information dictionary is missing values or contains incorrect values.</summary>
		AccessInfoInvalid,
		/// <summary>Indicates that the client was denied permission.</summary>
		ClientPermissionDenied,
		/// <summary>Indicates that the current protection policy stopped the credentials from being fetched.</summary>
		AccessDeniedByProtectionPolicy,
		/// <summary>Indicates that the credential was not found.</summary>
		CredentialNotFound,
		/// <summary>Indicates that the credentials could not be fetched.</summary>
		FetchCredentialFailed,
		/// <summary>Indicates that the credentials were not stored in the Keychain.</summary>
		StoreCredentialFailed,
		/// <summary>Indicates that the credentials could not be removed from the Keychain.</summary>
		RemoveCredentialFailed,
		/// <summary>Indicates that the target account of an updated did not exist.</summary>
		UpdatingNonexistentAccount,
		/// <summary>Indicates that the client did not have a valid bundle identifier.</summary>
		InvalidClientBundleID,      // in the header file, but not in the API diff
		/// <summary>Indicates that the operation was denied by a plug-in.</summary>
		DeniedByPlugin,
		/// <summary>Indicates that Core Data failed to save the account.</summary>
		CoreDataSaveFailed,
		/// <summary>Indicates that the account information could not be serialized.</summary>
		FailedSerializingAccountInfo,
		/// <summary>Indicates that the command was invalid.</summary>
		InvalidCommand,
		/// <summary>Indicates that the message identifier was missing.</summary>
		MissingTransportMessageId,
		/// <summary>Indicates that the credential item was not found.</summary>
		CredentialItemNotFound,
		/// <summary>Indicates that the credential item wasn't expired.</summary>
		CredentialItemNotExpired,
	}

	// NSInteger -> ACAccountStore.h
	/// <summary>An enumeration whose values indicate the result of a credential renewal request (see <see cref="M:Accounts.ACAccountStore.RenewCredentials(Accounts.ACAccount,System.Action{Accounts.ACAccountCredentialRenewResult,Foundation.NSError})" />).</summary>
	[Native]
	public enum ACAccountCredentialRenewResult : long {
		/// <summary>The renewal succeeded.</summary>
		Renewed,
		/// <summary>The user has disallowed access.</summary>
		Rejected,
		/// <summary>There was an error in processing.</summary>
		Failed,
	}
}
