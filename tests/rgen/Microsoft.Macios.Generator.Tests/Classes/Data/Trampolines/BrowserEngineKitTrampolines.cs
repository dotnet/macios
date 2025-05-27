// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using BrowserEngineKit;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class BrowserEngineKitTrampolines {

	[Export<Property> ("beDownloadMonitorBeginMonitoringCallback", ArgumentSemantic.Copy)]
	public partial BrowserEngineKit.BEDownloadMonitorBeginMonitoringCallback BEDownloadMonitorBeginMonitoringCallback { get; set; }

	[Export<Property> ("beDownloadMonitorResumeMonitoringCallback", ArgumentSemantic.Copy)]
	public partial BrowserEngineKit.BEDownloadMonitorResumeMonitoringCallback BEDownloadMonitorResumeMonitoringCallback { get; set; }

	[Export<Property> ("beDownloadMonitorUseDownloadsFolderCallback", ArgumentSemantic.Copy)]
	public partial BrowserEngineKit.BEDownloadMonitorUseDownloadsFolderCallback BEDownloadMonitorUseDownloadsFolderCallback { get; set; }

	[Export<Property> ("beDragInteractionDelegateGetDragItemsCallback", ArgumentSemantic.Copy)]
	public partial BrowserEngineKit.BEDragInteractionDelegateGetDragItemsCallback BEDragInteractionDelegateGetDragItemsCallback { get; set; }

	[Export<Property> ("beNetworkingProcessCreateCallback", ArgumentSemantic.Copy)]
	public partial BrowserEngineKit.BENetworkingProcessCreateCallback BENetworkingProcessCreateCallback { get; set; }

	[Export<Property> ("beRenderingProcessCreateCallback", ArgumentSemantic.Copy)]
	public partial BrowserEngineKit.BERenderingProcessCreateCallback BERenderingProcessCreateCallback { get; set; }

	[Export<Property> ("beTextInputHandleKeyEntryCallback", ArgumentSemantic.Copy)]
	public partial BrowserEngineKit.BETextInputHandleKeyEntryCallback BETextInputHandleKeyEntryCallback { get; set; }

	[Export<Property> ("beTextInputReplaceTextCallback", ArgumentSemantic.Copy)]
	public partial BrowserEngineKit.BETextInputReplaceTextCallback BETextInputReplaceTextCallback { get; set; }

	[Export<Property> ("beTextInputRequestTextContextForAutocorrectionCallback", ArgumentSemantic.Copy)]
	public partial BrowserEngineKit.BETextInputRequestTextContextForAutocorrectionCallback BETextInputRequestTextContextForAutocorrectionCallback { get; set; }

	[Export<Property> ("beTextInputRequestTextRectsCallback", ArgumentSemantic.Copy)]
	public partial BrowserEngineKit.BETextInputRequestTextRectsCallback BETextInputRequestTextRectsCallback { get; set; }

	[Export<Property> ("beWebContentProcessCreateCallback", ArgumentSemantic.Copy)]
	public partial BrowserEngineKit.BEWebContentProcessCreateCallback BEWebContentProcessCreateCallback { get; set; }
}
