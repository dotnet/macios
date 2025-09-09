//
// Enums.cs: enums for MultipeerConnectivity
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013-2014, 2016 Xamarin, Inc.

using ObjCRuntime;

namespace MultipeerConnectivity {

	// NSInteger -> MCSession.h
	/// <summary>An enumeration whose values specify whether a message's delivery is guaranteed. Used with <see cref="MultipeerConnectivity.MCSession.SendData(Foundation.NSData,MultipeerConnectivity.MCPeerID[],MultipeerConnectivity.MCSessionSendDataMode,out Foundation.NSError)" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MCSessionSendDataMode : long {
		/// <summary>Indicates guaranteed delivery.</summary>
		Reliable,
		/// <summary>Indicates non-guaranteed delivery.</summary>
		Unreliable,
	}

	// NSInteger -> MCSession.h
	/// <summary>An enumeration whose values indicate the state of a <see cref="MultipeerConnectivity.MCSession" />. Used with <see cref="MultipeerConnectivity.MCSessionDelegate.DidChangeState(MultipeerConnectivity.MCSession,MultipeerConnectivity.MCPeerID,MultipeerConnectivity.MCSessionState)" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MCSessionState : long {
		/// <summary>Indicates that the <see cref="MultipeerConnectivity.MCSession" /> is not connected.</summary>
		NotConnected,
		/// <summary>Indicates that the <see cref="MultipeerConnectivity.MCSession" /> is in the process of connecting.</summary>
		Connecting,
		/// <summary>Indicates that the <see cref="MultipeerConnectivity.MCSession" /> is connected.</summary>
		Connected,
	}

	// NSInteger -> MCSession.h
	/// <summary>An enumeration whose values specify whether an <see cref="MultipeerConnectivity.MCSession" /> should encrypt its connection. Used with <see cref="MultipeerConnectivity.MCSession(MultipeerConnectivity.MCPeerID,Security.SecIdentity, MultipeerConnectivity.MCEncryptionPreference)" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MCEncryptionPreference : long {
		/// <summary>No preference.</summary>
		Optional = 0,
		/// <summary>Connections should be encrypted.</summary>
		Required = 1,
		/// <summary>A preference for unencrypted connections.</summary>
		None = 2,
	}

	// NSInteger -> MCError.h
	/// <summary>An enumeration whose values specify various errors relating to multipeer connectivity.</summary>
	[MacCatalyst (13, 1)]
	[Native ("MCErrorCode")]
	[ErrorDomain ("MCErrorDomain")]
	public enum MCError : long {
		/// <summary>The type of the error could not be determined.</summary>
		Unknown,
		/// <summary>Data was sent to a peer that is not connected.</summary>
		NotConnected,
		/// <summary>The relevant operation was called with an invalid parameter.</summary>
		InvalidParameter,
		/// <summary>The relevant operation is not supported (for instance, an attempt to send a non-local or non-Web-based resource).</summary>
		Unsupported,
		/// <summary>Indicates a connection or data-transmission time-out.</summary>
		TimedOut,
		/// <summary>The relevant operation was cancelled.</summary>
		Cancelled,
		/// <summary>Indicates that Multipeer Connectivity is not available.</summary>
		Unavailable,
	}
}
