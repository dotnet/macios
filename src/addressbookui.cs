//
// This file describes the API that the generator will produce
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2014-2015, Xamarin Inc.
//
using ObjCRuntime;
using Foundation;
using CoreGraphics;
using CoreLocation;
using UIKit;
using AddressBook;
using System;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace AddressBookUI {

	/// <summary>A view controller used to create a new contact.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AddressBookUI/Reference/ABNewPersonViewController_Class/index.html">Apple documentation for <c>ABNewPersonViewController</c></related>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	[BaseType (typeof (UIViewController))]
	interface ABNewPersonViewController {
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[Export ("displayedPerson"), Internal]
		IntPtr _DisplayedPerson { get; set; }

		[Export ("addressBook"), Internal]
		IntPtr _AddressBook { get; set; }

		[Export ("parentGroup"), Internal]
		IntPtr _ParentGroup { get; set; }

		/// <summary>An instance of the AddressBookUI.IABNewPersonViewControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the AddressBookUI.IABNewPersonViewControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IABNewPersonViewControllerDelegate Delegate { get; set; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("newPersonViewDelegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }
	}

	/// <summary>The delegate object for <see cref="AddressBookUI.ABNewPersonViewController" />. Provides an event when data entry is complete.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AddressBookUI/Reference/ABNewPersonViewControllerDelegate_Protocol/index.html">Apple documentation for <c>ABNewPersonViewControllerDelegate</c></related>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface ABNewPersonViewControllerDelegate {

		/// <param name="controller">To be added.</param>
		///         <param name="person">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("newPersonViewController:didCompleteWithNewPerson:")]
		[Abstract]
		void DidCompleteWithNewPerson (ABNewPersonViewController controller, [NullAllowed] ABPerson person);
	}

	/// <include file="../docs/api/AddressBookUI/IABNewPersonViewControllerDelegate.xml" path="/Documentation/Docs[@DocId='T:AddressBookUI.IABNewPersonViewControllerDelegate']/*" />
	interface IABNewPersonViewControllerDelegate { }

	/// <include file="../docs/api/AddressBookUI/ABPeoplePickerNavigationController.xml" path="/Documentation/Docs[@DocId='T:AddressBookUI.ABPeoplePickerNavigationController']/*" />
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	[BaseType (typeof (UINavigationController))]
	interface ABPeoplePickerNavigationController : UIAppearance {
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		/// <param name="rootViewController">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithRootViewController:")]
		[PostGet ("ViewControllers")] // that will PostGet TopViewController and VisibleViewController too
		NativeHandle Constructor (UIViewController rootViewController);

		[NullAllowed]
		[Export ("displayedProperties", ArgumentSemantic.Copy), Internal]
		NSNumber [] _DisplayedProperties { get; set; }

		[Export ("addressBook"), Internal]
		IntPtr _AddressBook { get; set; }

		/// <summary>An instance of the AddressBookUI.IABPeoplePickerNavigationControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the AddressBookUI.IABPeoplePickerNavigationControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IABPeoplePickerNavigationControllerDelegate Delegate { get; set; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[NullAllowed] // by default this property is null
		[Export ("peoplePickerDelegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set; }

		/// <summary>Use this property to set a predicate that determines whether the person can be selected or not.</summary>
		///         <value>
		///           <para>If the value is null, all persons are selectable;  Otherwise only those persons that match the predicate will be.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Export ("predicateForEnablingPerson", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSPredicate PredicateForEnablingPerson { get; set; }

		/// <summary>Use this property to set a predicate that determines whether the person should be returned to the app, or displayed to the user.</summary>
		///         <value>
		///           <para>If set, the predicate that determines whether to return the person to the app (the predicate evaluates to true) or displayed (the predicate evaluates to false).</para>
		///           <para>If the value is not set, the decision on whether the person is returned or displayed rests on the methods from the <see cref="AddressBookUI.ABPeoplePickerNavigationControllerDelegate" /> delegate.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Export ("predicateForSelectionOfPerson", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSPredicate PredicateForSelectionOfPerson { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("predicateForSelectionOfProperty", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSPredicate PredicateForSelectionOfProperty { get; set; }
	}

	/// <summary>A delegate object that allows the application developer to have fine-grained control of events in the life-cycle of a <see cref="AddressBookUI.ABPeoplePickerNavigationController" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AddressBookUI/Reference/ABPeoplePickerNavigationControllerDelegate_Protocol/index.html">Apple documentation for <c>ABPeoplePickerNavigationControllerDelegate</c></related>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface ABPeoplePickerNavigationControllerDelegate {
		/// <param name="peoplePicker">To be added.</param>
		///         <param name="selectedPerson">To be added.</param>
		///         <summary>Developers should not use this deprecated method. Developers should use 'DidSelectPerson' instead (or 'ABPeoplePickerNavigationController.PredicateForSelectionOfPerson').</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 8, 0, message: "Use 'DidSelectPerson' instead (or 'ABPeoplePickerNavigationController.PredicateForSelectionOfPerson').")]
		[Export ("peoplePickerNavigationController:shouldContinueAfterSelectingPerson:")]
		bool ShouldContinue (ABPeoplePickerNavigationController peoplePicker, ABPerson selectedPerson);

		/// <param name="peoplePicker">To be added.</param>
		///         <param name="selectedPerson">To be added.</param>
		///         <param name="propertyId">To be added.</param>
		///         <param name="identifier">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 8, 0, message: "Use 'DidSelectPerson' instead (or 'ABPeoplePickerNavigationController.PredicateForSelectionOfProperty').")]
		[Export ("peoplePickerNavigationController:shouldContinueAfterSelectingPerson:property:identifier:")]
		bool ShouldContinue (ABPeoplePickerNavigationController peoplePicker, ABPerson selectedPerson, int /* ABPropertyId = int32 */ propertyId, int /* ABMultiValueIdentifier = int32 */ identifier);

		/// <param name="peoplePicker">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peoplePickerNavigationControllerDidCancel:")]
		void Cancelled (ABPeoplePickerNavigationController peoplePicker);

		/// <param name="peoplePicker">To be added.</param>
		///         <param name="selectedPerson">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peoplePickerNavigationController:didSelectPerson:")]
		void DidSelectPerson (ABPeoplePickerNavigationController peoplePicker, ABPerson selectedPerson);

		/// <param name="peoplePicker">To be added.</param>
		///         <param name="selectedPerson">To be added.</param>
		///         <param name="propertyId">To be added.</param>
		///         <param name="identifier">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("peoplePickerNavigationController:didSelectPerson:property:identifier:")]
		void DidSelectPerson (ABPeoplePickerNavigationController peoplePicker, ABPerson selectedPerson, int /* ABPropertyId = int32 */ propertyId, int /* ABMultiValueIdentifier = int32 */ identifier);
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="AddressBookUI.ABPeoplePickerNavigationControllerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="AddressBookUI.ABPeoplePickerNavigationControllerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="AddressBookUI.ABPeoplePickerNavigationControllerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="AddressBookUI.ABPeoplePickerNavigationControllerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IABPeoplePickerNavigationControllerDelegate { }

	/// <summary>A <see cref="UIKit.UIViewController" /> that displays the data of an <see cref="AddressBook.ABPerson" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AddressBookUI/Reference/ABPersonViewController_Class/index.html">Apple documentation for <c>ABPersonViewController</c></related>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	[BaseType (typeof (UIViewController))]
	interface ABPersonViewController : UIViewControllerRestoration {
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[Export ("displayedPerson"), Internal]
		IntPtr _DisplayedPerson { get; set; }

		[NullAllowed]
		[Export ("displayedProperties", ArgumentSemantic.Copy), Internal]
		NSNumber [] _DisplayedProperties { get; set; }

		[Export ("addressBook"), Internal]
		IntPtr _AddressBook { get; set; }

		/// <summary>Gets or sets whether the buttons for predefined actions (send text message, etc.) are displayed.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsActions")]
		bool AllowsActions { get; set; }

		/// <summary>Gets or sets whether the user is allowed to edit the person's data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsEditing")]
		bool AllowsEditing { get; set; }

		/// <summary>Gets or sets whether the view controller should show linked people.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shouldShowLinkedPeople")]
		bool ShouldShowLinkedPeople { get; set; }

		/// <summary>An instance of the AddressBookUI.IABPersonViewControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the AddressBookUI.IABPersonViewControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IABPersonViewControllerDelegate Delegate { get; set; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[NullAllowed] // by default this property is null
		[Export ("personViewDelegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set; }

		// Obsolete for public use; we should "remove" this member by making
		// it [Internal] in some future release, as it's needed internally.
		[Internal]
		[Export ("setHighlightedItemForProperty:withIdentifier:")]
		void SetHighlightedItemForProperty (int /* ABPropertyId = int32 */ property, int /* ABMultiValueIdentifier = int32 */ identifier);
	}

	/// <summary>Constants for use with <see cref="AddressBookUI.ABPeoplePickerNavigationController" /> predicate methods (<see cref="AddressBookUI.ABPeoplePickerNavigationController.PredicateForEnablingPerson" />,
	/// 	<see cref="AddressBookUI.ABPeoplePickerNavigationController.PredicateForSelectionOfPerson" />
	/// 	and <see cref="AddressBookUI.ABPeoplePickerNavigationController.PredicateForSelectionOfProperty" />).</summary>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	[Static]
	interface ABPersonPredicateKey {
		/// <summary>Represents the value associated with the constant ABPersonBirthdayProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonBirthdayProperty")]
		NSString Birthday { get; }

		/// <summary>Represents the value associated with the constant ABPersonDatesProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonDatesProperty")]
		NSString Dates { get; }

		/// <summary>Represents the value associated with the constant ABPersonDepartmentNameProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonDepartmentNameProperty")]
		NSString DepartmentName { get; }

		/// <summary>Represents the value associated with the constant ABPersonEmailAddressesProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonEmailAddressesProperty")]
		NSString EmailAddresses { get; }

		/// <summary>Represents the value associated with the constant ABPersonFamilyNameProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonFamilyNameProperty")]
		NSString FamilyName { get; }

		/// <summary>Represents the value associated with the constant ABPersonGivenNameProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonGivenNameProperty")]
		NSString GivenName { get; }

		/// <summary>Represents the value associated with the constant ABPersonInstantMessageAddressesProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonInstantMessageAddressesProperty")]
		NSString InstantMessageAddresses { get; }

		/// <summary>Represents the value associated with the constant ABPersonJobTitleProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonJobTitleProperty")]
		NSString JobTitle { get; }

		/// <summary>Represents the value associated with the constant ABPersonMiddleNameProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonMiddleNameProperty")]
		NSString MiddleName { get; }

		/// <summary>Represents the value associated with the constant ABPersonNamePrefixProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonNamePrefixProperty")]
		NSString NamePrefix { get; }

		/// <summary>Represents the value associated with the constant ABPersonNameSuffixProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonNameSuffixProperty")]
		NSString NameSuffix { get; }

		/// <summary>Represents the value associated with the constant ABPersonNicknameProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonNicknameProperty")]
		NSString Nickname { get; }

		/// <summary>Represents the value associated with the constant ABPersonNoteProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonNoteProperty")]
		NSString Note { get; }

		/// <summary>Represents the value associated with the constant ABPersonOrganizationNameProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonOrganizationNameProperty")]
		NSString OrganizationName { get; }

		/// <summary>Represents the value associated with the constant ABPersonPhoneNumbersProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonPhoneNumbersProperty")]
		NSString PhoneNumbers { get; }

		/// <summary>Represents the value associated with the constant ABPersonPhoneticFamilyNameProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonPhoneticFamilyNameProperty")]
		NSString PhoneticFamilyName { get; }

		/// <summary>Represents the value associated with the constant ABPersonPhoneticGivenNameProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonPhoneticGivenNameProperty")]
		NSString PhoneticGivenName { get; }

		/// <summary>Represents the value associated with the constant ABPersonPhoneticMiddleNameProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonPhoneticMiddleNameProperty")]
		NSString PhoneticMiddleName { get; }

		/// <summary>Represents the value associated with the constant ABPersonPostalAddressesProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonPostalAddressesProperty")]
		NSString PostalAddresses { get; }

		/// <summary>Represents the value associated with the constant ABPersonPreviousFamilyNameProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonPreviousFamilyNameProperty")]
		NSString PreviousFamilyName { get; }

		/// <summary>Represents the value associated with the constant ABPersonRelatedNamesProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonRelatedNamesProperty")]
		NSString RelatedNames { get; }

		/// <summary>Represents the value associated with the constant ABPersonSocialProfilesProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonSocialProfilesProperty")]
		NSString SocialProfiles { get; }

		/// <summary>Represents the value associated with the constant ABPersonUrlAddressesProperty</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("ABPersonUrlAddressesProperty")]
		NSString UrlAddresses { get; }
	}

	/// <summary>A delegate object that allows the application developer have fine-grained control of events in the life-cycle of a <see cref="AddressBookUI.ABPersonViewController" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AddressBookUI/Reference/ABPersonViewControllerDelegate_Protocol/index.html">Apple documentation for <c>ABPersonViewControllerDelegate</c></related>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface ABPersonViewControllerDelegate {

		/// <param name="personViewController">To be added.</param>
		///         <param name="person">To be added.</param>
		///         <param name="propertyId">To be added.</param>
		///         <param name="identifier">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("personViewController:shouldPerformDefaultActionForPerson:property:identifier:")]
		[Abstract]
		bool ShouldPerformDefaultActionForPerson (ABPersonViewController personViewController, ABPerson person, int /* ABPropertyID = int32 */ propertyId, int /* ABMultiValueIdentifier = int32 */ identifier);
	}

	/// <include file="../docs/api/AddressBookUI/IABPersonViewControllerDelegate.xml" path="/Documentation/Docs[@DocId='T:AddressBookUI.IABPersonViewControllerDelegate']/*" />
	interface IABPersonViewControllerDelegate { }

	/// <summary>A <see cref="UIKit.UIViewController" /> that allows the application user to enter data and create a new person record.</summary>
	///     
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AddressBookUI/Reference/ABUnknownPersonViewController_Class/index.html">Apple documentation for <c>ABUnknownPersonViewController</c></related>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	[BaseType (typeof (UIViewController))]
	interface ABUnknownPersonViewController {
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		/// <summary>Gets or sets an alternative name other than first and last name for the person. Nullable. (see <see cref="AddressBookUI.ABUnknownPersonViewController.Message" />)</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("alternateName", ArgumentSemantic.Copy)]
		string AlternateName { get; set; }

		/// <summary>Gets or sets the text shown beneath the <see cref="AddressBookUI.ABUnknownPersonViewController.AlternateName" /> field.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("message", ArgumentSemantic.Copy)]
		string Message { get; set; }

		[Export ("displayedPerson"), Internal]
		IntPtr _DisplayedPerson { get; set; }

		[Export ("addressBook"), Internal]
		IntPtr _AddressBook { get; set; }

		/// <summary>Gets or sets whether the buttons for predefined actions (send text message, etc.) are shown by the controller.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsActions")]
		bool AllowsActions { get; set; }

		/// <summary>Gets or sets whether the user's changes to the displayed data should be saved to the <see cref="AddressBookUI.ABUnknownPersonViewController.AddressBook" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsAddingToAddressBook")]
		bool AllowsAddingToAddressBook { get; set; }

		/// <summary>An instance of the AddressBookUI.IABUnknownPersonViewControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the AddressBookUI.IABUnknownPersonViewControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IABUnknownPersonViewControllerDelegate Delegate { get; set; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[NullAllowed] // by default this property is null
		[Export ("unknownPersonViewDelegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set; }
	}

	/// <summary>A delegate object that allows the application developer have fine-grained control of events in the life-cycle of a <see cref="AddressBookUI.ABUnknownPersonViewController" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AddressBookUI/Reference/ABUnknownPersonViewControllerDelegate_Protocol/index.html">Apple documentation for <c>ABUnknownPersonViewControllerDelegate</c></related>
	[Deprecated (PlatformName.iOS, 9, 0, message: "Use the 'Contacts' API instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'Contacts' API instead.")]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface ABUnknownPersonViewControllerDelegate {
		/// <param name="unknownPersonView">To be added.</param>
		///         <param name="person">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("unknownPersonViewController:didResolveToPerson:")]
		[Abstract]
		void DidResolveToPerson (ABUnknownPersonViewController unknownPersonView, [NullAllowed] ABPerson person);

		/// <param name="personViewController">To be added.</param>
		///         <param name="person">To be added.</param>
		///         <param name="propertyId">To be added.</param>
		///         <param name="identifier">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("unknownPersonViewController:shouldPerformDefaultActionForPerson:property:identifier:")]
		bool ShouldPerformDefaultActionForPerson (ABUnknownPersonViewController personViewController, ABPerson person, int /* ABPropertyID = int32 */ propertyId, int /* ABMultiValueIdentifier = int32 */ identifier);
	}
	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="AddressBookUI.ABUnknownPersonViewControllerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="AddressBookUI.ABUnknownPersonViewControllerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="AddressBookUI.ABUnknownPersonViewControllerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="AddressBookUI.ABUnknownPersonViewControllerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IABUnknownPersonViewControllerDelegate { }
}
