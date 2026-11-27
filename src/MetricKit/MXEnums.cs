//
// MetricKit enumerations
//
// Copyright 2022 Microsoft Inc.
//

using System.ComponentModel;
using CoreFoundation;

namespace MetricKit {
	/// <summary>Enumerates the errors that MetricKit reports for launch tasks.</summary>
	[NoMac, iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[ErrorDomain ("MXErrorDomain")]
	[Native]
	public enum MXErrorCode : long {
		/// <summary>The task identifier is null or longer than 128 characters.</summary>
		InvalidId,
		/// <summary>The app exceeded the maximum number of tasks.</summary>
		MaxCount,
		/// <summary>The app started the task too late.</summary>
		PastDeadline,
		/// <summary>A task with the same identifier has already been started.</summary>
		Duplicated,
		/// <summary>The task hasn't been started, or it has already finished.</summary>
		Unknown,
		/// <summary>MetricKit failed internally.</summary>
		InternalFailure,
	}
}
