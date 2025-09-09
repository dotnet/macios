//
// ClassKit bindings
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2018 Xamarin Inc. All rights reserved.
//

using System;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using System.Reflection;

namespace ClassKit {

	/// <summary>Enumerates activity outcome types.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[Native]
	enum CLSBinaryValueType : long {
		/// <summary>Indicates an activity that results in a true or false value</summary>
		TrueFalse = 0,
		/// <summary>Indicates an activity that results in a pass or fail value.</summary>
		PassFail,
		/// <summary>Indicates an activity that results in a yes or no value</summary>
		YesNo,
		/// <summary>To be added.</summary>
		[MacCatalyst (14, 0)]
		CorrectIncorrect,
	}

	/// <summary>Enumerates curriculum units.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[Native]
	enum CLSContextType : long {
		/// <summary>Indicates no context type.</summary>
		None = 0,
		/// <summary>Indicates an app context.</summary>
		App,
		/// <summary>Indicates a context for chapter.</summary>
		Chapter,
		/// <summary>Indicates a context for a section.</summary>
		Section,
		/// <summary>Indicates a context for level.</summary>
		Level,
		/// <summary>Indicates a context for a page.</summary>
		Page,
		/// <summary>Indicates a context for a task.</summary>
		Task,
		/// <summary>Indicates a context for a challenge.</summary>
		Challenge,
		/// <summary>Indicates a context for a quiz.</summary>
		Quiz,
		/// <summary>Indicates a context for an exercise</summary>
		Exercise,
		/// <summary>Indicates a context for a lesson.</summary>
		Lesson,
		/// <summary>Indicates a context for a book.</summary>
		Book,
		/// <summary>Indicates a context for a game.</summary>
		Game,
		/// <summary>Indicates a context for a document.</summary>
		Document,
		/// <summary>Indicates a context for audio material.</summary>
		Audio,
		/// <summary>Indicates a context for a video.</summary>
		Video,
		[iOS (13, 4)]
		[MacCatalyst (14, 0)]
		Course,
		[iOS (13, 4)]
		[MacCatalyst (14, 0)]
		Custom,
	}

	/// <summary>Enumerates ClassKit error codes.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[Native]
	[ErrorDomain ("CLSErrorCodeDomain")]
	public enum CLSErrorCode : long {
		/// <summary>No error occurred.</summary>
		None = 0,
		/// <summary>ClassKit was not available</summary>
		ClassKitUnavailable,
		/// <summary>An argument was invalid.</summary>
		InvalidArgument,
		/// <summary>An attempt was made to modify a read-only value.</summary>
		InvalidModification,
		/// <summary>Authorization for the operation was denied.</summary>
		AuthorizationDenied,
		/// <summary>A database could not be accessed.</summary>
		DatabaseInaccessible,
		/// <summary>An unspecified limit was exceeded.</summary>
		Limits,
		/// <summary>An attempt was made to save a created object that is identical to an existing one.</summary>
		InvalidCreate,
		/// <summary>An update failed.</summary>
		InvalidUpdate,
		/// <summary>More than one error occurred, so the topmost error has an underlying error or errors.</summary>
		PartialFailure,
		InvalidAccountCredentials,
	}


	/// <summary>Enumerates topics for contexts.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	enum CLSContextTopic {
		/// <summary>Indicates mathematics.</summary>
		[Field ("CLSContextTopicMath")]
		Math,
		/// <summary>Indicates science.</summary>
		[Field ("CLSContextTopicScience")]
		Science,
		/// <summary>Indicates literacy and/or writing.</summary>
		[Field ("CLSContextTopicLiteracyAndWriting")]
		LiteracyAndWriting,
		/// <summary>Indicates world languages.</summary>
		[Field ("CLSContextTopicWorldLanguage")]
		WorldLanguage,
		/// <summary>Indicates social sciences.</summary>
		[Field ("CLSContextTopicSocialScience")]
		SocialScience,
		/// <summary>Indicates computer science and/or engineering.</summary>
		[Field ("CLSContextTopicComputerScienceAndEngineering")]
		ComputerScienceAndEngineering,
		/// <summary>Indicates arts and/or music.</summary>
		[Field ("CLSContextTopicArtsAndMusic")]
		ArtsAndMusic,
		/// <summary>Indicates health and fitness.</summary>
		[Field ("CLSContextTopicHealthAndFitness")]
		HealthAndFitness,
		// CLSContext.Topic should be nullable, thus we should add a null option
		[Field (null)]
		None = 1000,
	}


	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV, iOS (14, 0)]
	[Native]
	public enum CLSProgressReportingCapabilityKind : long {
		Duration = 0,
		Percent,
		Binary,
		Quantity,
		Score,
	}

	/// <summary>Contains keys for accessing error data.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[Static]
	interface CLSErrorUserInfoKeys {

		/// <summary>Gets a key that can be used for finding the object that caused the error.</summary>
		///         <value>The key that can be used for finding the object that caused the error.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CLSErrorObjectKey")]
		NSString ObjectKey { get; }

		/// <summary>Gets a key that can be used for finding the underlying error that caused the current error.</summary>
		///         <value>The key that can be used for finding the underlying error that caused the current error.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CLSErrorUnderlyingErrorsKey")]
		NSString UnderlyingErrorsKey { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("CLSErrorSuccessfulObjectsKey")]
		NSString SuccessfulObjectsKey { get; }
	}

	/// <summary>Enumerates key paths for retrieving ClassKit contexts.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[Static]
	interface CLSPredicateKeyPath {
		/// <summary>Gets the key path for retrieving contexts by their creation date.</summary>
		///         <value>The key path for retrieving contexts by their creation date.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CLSPredicateKeyPathDateCreated")]
		NSString DateCreated { get; }

		/// <summary>Gets the key path for retrieving contexts by their identifier.</summary>
		///         <value>The key path for retrieving contexts by their identifier.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CLSPredicateKeyPathIdentifier")]
		NSString Identifier { get; }

		/// <summary>Gets the key path for retrieving contexts by their title.</summary>
		///         <value>The key path for retrieving contexts by their title.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CLSPredicateKeyPathTitle")]
		NSString Title { get; }

		/// <summary>Gets the key path for retrieving contexts by universal link URL.</summary>
		///         <value>The key path for retrieving contexts by universal link URL.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CLSPredicateKeyPathUniversalLinkURL")]
		NSString UniversalLinkUrl { get; }

		/// <summary>Gets the key path for retrieving contexts by topic.</summary>
		///         <value>The key path for retrieving contexts by topic.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CLSPredicateKeyPathTopic")]
		NSString Topic { get; }

		/// <summary>Gets the key path for retrieving contexts by their parent.</summary>
		///         <value>The key path for retrieving contexts by their parent.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CLSPredicateKeyPathParent")]
		NSString Parent { get; }
	}

	/// <summary>Base class for ClassKit objects.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CLSObject : NSSecureCoding {

		[Export ("dateCreated", ArgumentSemantic.Strong)]
		NSDate DateCreated { get; }

		[Export ("dateLastModified", ArgumentSemantic.Strong)]
		NSDate DateLastModified { get; }
	}

	/// <summary>Encapsulates and interaction between the student and a task for a context.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[BaseType (typeof (CLSObject))]
	[DisableDefaultCtor]
	interface CLSActivity {

		[Export ("progress")]
		double Progress { get; set; }

		[Export ("duration")]
		double Duration { get; }

		[NullAllowed]
		[Export ("primaryActivityItem", ArgumentSemantic.Strong)]
		CLSActivityItem PrimaryActivityItem { get; set; }

		[Export ("addProgressRangeFromStart:toEnd:")]
		void AddProgressRange (double start, double end);

		[Export ("addAdditionalActivityItem:")]
		void AddAdditionalActivityItem (CLSActivityItem activityItem);

		[Export ("additionalActivityItems", ArgumentSemantic.Strong)]
		CLSActivityItem [] AdditionalActivityItems { get; }

		// From CLSActivity (Activation) Category

		/// <summary>Gets a Boolean value that tells whether the activity is active.</summary>
		///         <value>A Boolean value that tells whether the activity is active.</value>
		///         <remarks>To be added.</remarks>
		[Export ("started")]
		bool Started { [Bind ("isStarted")] get; }

		[Export ("start")]
		void Start ();

		[Export ("stop")]
		void Stop ();

		[Introduced (PlatformName.MacCatalyst, 14, 5)]
		[iOS (14, 5)]
		[Export ("removeAllActivityItems")]
		void RemoveAllActivityItems ();
	}

	/// <summary>Base class for activity items.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[BaseType (typeof (CLSObject))]
	[DisableDefaultCtor]
	interface CLSActivityItem {

		[Export ("title")]
		string Title { get; set; }

		[Export ("identifier")]
		string Identifier { get; }
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[BaseType (typeof (CLSActivityItem))]
	[DisableDefaultCtor]
	interface CLSBinaryItem {

		[Export ("value")]
		bool Value { get; set; }

		[Export ("valueType", ArgumentSemantic.Assign)]
		CLSBinaryValueType ValueType { get; }

		[Export ("initWithIdentifier:title:type:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string identifier, string title, CLSBinaryValueType valueType);
	}

	/// <summary>A node in a ClassKit context hierarchy.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[BaseType (typeof (CLSObject))]
	[DisableDefaultCtor]
	interface CLSContext {

		[iOS (13, 4)]
		[MacCatalyst (14, 0)]
		[Export ("identifierPath", ArgumentSemantic.Copy)]
		string [] IdentifierPath { get; }

		[Export ("identifier")]
		string Identifier { get; }

		[NullAllowed, Export ("universalLinkURL", ArgumentSemantic.Strong)]
		NSUrl UniversalLinkUrl { get; set; }

		[Export ("type", ArgumentSemantic.Assign)]
		CLSContextType Type { get; }

		[iOS (13, 4)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("customTypeName")]
		string CustomTypeName { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("displayOrder")]
		nint DisplayOrder { get; set; }

		[iOS (13, 4)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("summary")]
		string Summary { get; set; }

		[iOS (13, 4)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("thumbnail", ArgumentSemantic.Assign)]
		CGImage Thumbnail { get; set; }

		[Protected]
		[NullAllowed, Export ("topic", ArgumentSemantic.Copy)]
		NSString WeakTopic { get; set; }

		[Export ("initWithType:identifier:title:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CLSContextType type, string identifier, string title);

		/// <summary>Gets a Boolean value that tells whether the context is currently active.</summary>
		///         <value>
		///           <see langword="true" /> if the context is active. Otherwise, <see langword="false" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("active")]
		bool Active { [Bind ("isActive")] get; }

		[Export ("becomeActive")]
		void BecomeActive ();

		[Export ("resignActive")]
		void ResignActive ();

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("assignable")]
		bool Assignable { [Bind ("isAssignable")] get; set; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("suggestedAge", ArgumentSemantic.Assign)]
		NSRange SuggestedAge { get; set; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("suggestedCompletionTime", ArgumentSemantic.Assign)]
		NSRange SuggestedCompletionTime { get; set; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("progressReportingCapabilities", ArgumentSemantic.Copy)]
		NSSet<CLSProgressReportingCapability> ProgressReportingCapabilities { get; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("setType:")]
		void SetType (CLSContextType type);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("addProgressReportingCapabilities:")]
		void AddProgressReportingCapabilities (NSSet<CLSProgressReportingCapability> capabilities);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("resetProgressReportingCapabilities")]
		void ResetProgressReportingCapabilities ();

		// From CLSContext (Hierarchy) Category

		[NullAllowed, Export ("parent")]
		CLSContext Parent { get; }

		[Export ("removeFromParent")]
		void RemoveFromParent ();

		[Export ("addChildContext:")]
		void AddChild (CLSContext childContext);

		[Async (XmlDocs = """
			<param name="identifierPath">The identifier path for the context to retrieve.</param>
			<summary>Returns a task that contains the context that is represented by the provided identifier path.</summary>
			<returns>A task that contains the context that is represented by the provided identifier path.</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("descendantMatchingIdentifierPath:completion:")]
		void FindDescendantMatching (string [] identifierPath, Action<CLSContext, NSError> completion);

		[Introduced (PlatformName.MacCatalyst, 14, 5)]
		[iOS (14, 5)]
		[Export ("navigationChildContexts", ArgumentSemantic.Copy)]
		CLSContext [] NavigationChildContexts { get; }

		[Introduced (PlatformName.MacCatalyst, 14, 5)]
		[iOS (14, 5)]
		[Export ("addNavigationChildContext:")]
		void AddNavigationChild (CLSContext childContext);

		[Introduced (PlatformName.MacCatalyst, 14, 5)]
		[iOS (14, 5)]
		[Export ("removeNavigationChildContext:")]
		void RemoveNavigationChild (CLSContext childContext);

		// From CLSContext (Activity) Category

		[NullAllowed, Export ("currentActivity", ArgumentSemantic.Strong)]
		CLSActivity CurrentActivity { get; }

		[Export ("createNewActivity")]
		CLSActivity CreateNewActivity ();
	}

	interface ICLSDataStoreDelegate { }

	/// <summary>Delegate for requesting data store contexts.</summary>
	///     <remarks>
	///       <para>ClassKit contexts are used to arrange nested content, such as chapters and sections of a lesson plan, in order to organize and track student progress and tests. ClassKit supports a maximum of 8 layers of content nesting.</para>
	///     </remarks>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CLSDataStoreDelegate {

		/// <param name="identifier">The identifier for the context to create.</param>
		/// <param name="parentContext">The parent context for the context to create.</param>
		/// <param name="parentIdentifierPath">The identifier path for the parent of the context to create.</param>
		/// <summary>Requests a context for the provided parameters.</summary>
		/// <returns>A new ClassKit store context.</returns>
		/// <remarks>
		///           <para>ClassKit contexts are used to arrange nested content, such as chapters and sections of a lesson plan, in order to organize and track student progress and tests. ClassKit supports a maximum of 8 layers of content nesting.</para>
		///         </remarks>
		[Abstract]
		[Export ("createContextForIdentifier:parentContext:parentIdentifierPath:")]
		[return: NullAllowed]
		CLSContext CreateContext (string identifier, CLSContext parentContext, string [] parentIdentifierPath);
	}

	/// <summary>Manages ClassKit data by operating on hierarchical contexts, such as acts, chapters, sections, and so on.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CLSDataStore {

		[Static]
		[Export ("shared", ArgumentSemantic.Strong)]
		CLSDataStore Shared { get; }

		[Export ("mainAppContext", ArgumentSemantic.Strong)]
		CLSContext MainAppContext { get; }

		[NullAllowed, Export ("activeContext", ArgumentSemantic.Strong)]
		CLSContext ActiveContext { get; }

		[NullAllowed, Export ("runningActivity", ArgumentSemantic.Strong)]
		CLSActivity RunningActivity { get; }

		/// <summary>An instance of the ClassKit.ICLSDataStoreDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the ClassKit.ICLSDataStoreDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICLSDataStoreDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Async (XmlDocs = """
			<summary>Asynchronously saves the data store and returns a task that represents the operation.</summary>
			<returns>A task that represents the operation.</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("saveWithCompletion:")]
		void Save ([NullAllowed] Action<NSError> completion);

		[MacCatalyst (14, 0)]
		[Export ("completeAllAssignedActivitiesMatching:")]
		void CompleteAllAssignedActivitiesMatching (string [] contextPath);

		// From CLSDataStore (Contexts) Category

		[Async (XmlDocs = """
			<param name="predicate">The search predicate.</param>
			<summary>Searches for a context that matches the supplied <paramref name="predicate" /> and returns a task that contains the result.</summary>
			<returns>A task that contains the search results</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("contextsMatchingPredicate:completion:")]
		void FindContextsMatching (NSPredicate predicate, Action<CLSContext [], NSError> completion);

		[Async (XmlDocs = """
			<param name="identifierPath">The identifier paths for the contexts to find.</param>
			<summary>Finds the contexts identifed by a set of identifier paths and returns a task that contains the reults.</summary>
			<returns>A task that contains the search results</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("contextsMatchingIdentifierPath:completion:")]
		void FindContextsMatching (string [] identifierPath, Action<CLSContext [], NSError> completion);

		[Export ("removeContext:")]
		void Remove (CLSContext context);

		[Introduced (PlatformName.MacCatalyst, 14, 5)]
		[iOS (14, 5)]
		[Async]
		[Export ("fetchActivityForURL:completion:")]
		void FetchActivity (NSUrl url, Action<CLSActivity, NSError> completion);
	}

	/// <summary>Represents a quantitative data item.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[BaseType (typeof (CLSActivityItem))]
	[DisableDefaultCtor]
	interface CLSQuantityItem {

		[Export ("quantity")]
		double Quantity { get; set; }

		[Export ("initWithIdentifier:title:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string identifier, string title);
	}

	/// <summary>Represents a score for a test or quiz.</summary>
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[BaseType (typeof (CLSActivityItem))]
	[DisableDefaultCtor]
	interface CLSScoreItem {

		[Export ("score")]
		double Score { get; set; }

		[Export ("maxScore")]
		double MaxScore { get; set; }

		[Export ("initWithIdentifier:title:score:maxScore:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string identifier, string title, double score, double maxScore);
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV]
	[Protocol]
	interface CLSContextProvider {
		/// <param name="context">To be added.</param>
		/// <param name="completion">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("updateDescendantsOfContext:completion:")]
		void UpdateDescendants (CLSContext context, Action<NSError> completion);
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[NoTV, iOS (14, 0)]
	[BaseType (typeof (CLSObject))]
	[DisableDefaultCtor]
	interface CLSProgressReportingCapability {
		[Export ("kind", ArgumentSemantic.Assign)]
		CLSProgressReportingCapabilityKind Kind { get; }

		[NullAllowed]
		[Export ("details")]
		string Details { get; }

		[Export ("initWithKind:details:")]
		NativeHandle Constructor (CLSProgressReportingCapabilityKind kind, [NullAllowed] string details);
	}

}
