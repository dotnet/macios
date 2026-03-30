using System;
using Foundation;
using ObjCRuntime;

// @interface WebFetcher : NSObject <NSURLConnectionDelegate>
[BaseType (typeof (NSObject))]
interface WebFetcher : INSUrlConnectionDelegate {
	// @property (readonly, copy, nonatomic) NSURL * url;
	[Export ("url", ArgumentSemantic.Copy)]
	NSUrl Url { get; }

	// -(NSURLResponse *)getResponseForUrl:(NSURL *)url withCredential:(NSURLCredential *)credential;
	[Export ("getResponseForUrl:withCredential:")]
	NSUrlResponse GetResponseForUrl (NSUrl url, NSUrlCredential credential);

	// -(void)loadDataWithUrl:(NSURL *)url completionHandler:(void (^)(NSData *, NSURLResponse *))handler;
	[Export ("loadDataWithUrl:completionHandler:")]
	void LoadDataWithUrl (NSUrl url, Action<NSData, NSUrlResponse> handler);
}
