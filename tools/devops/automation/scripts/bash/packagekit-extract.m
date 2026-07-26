// Extracts Apple-signed XIP containers using the same PackageKit API as Archive Utility.

@import Foundation;

@interface PKSignedContainerCopyCancelHandler
- (void) cancel;
- (BOOL) isCancelled;
@end

@interface PKSignedContainer : NSObject
- (instancetype) initForReadingFromContainerAtURL:(NSURL *) url error:(NSError **) error;
- (PKSignedContainerCopyCancelHandler *) startUnarchivingAtPath:(NSString *) path
	notifyOnQueue:(dispatch_queue_t) queue
	progress:(void (^)(double, NSString *)) progressBlock
	finish:(void (^)(BOOL)) finishBlock;
@end

int main (int argc, char **argv)
{
	if (argc != 3) {
		fprintf (stderr, "usage: %s ARCHIVE_FILE TARGET_DIRECTORY\n", argv [0]);
		return 1;
	}

	NSURL *containerUrl = [NSURL fileURLWithPath:[NSString stringWithUTF8String:argv [1]]];
	NSURL *destinationUrl = [NSURL fileURLWithPath:[NSString stringWithUTF8String:argv [2]]];
	NSError *error = nil;

	if (![NSFileManager.defaultManager
			createDirectoryAtURL:destinationUrl
			withIntermediateDirectories:YES
			attributes:nil
			error:&error]) {
		fprintf (stderr, "%s\n", error == nil ? "Unable to create the target directory." : error.description.UTF8String);
		return 2;
	}

	PKSignedContainer *container = [[PKSignedContainer alloc]
		initForReadingFromContainerAtURL:containerUrl
		error:&error];
	if (container == nil) {
		fprintf (stderr, "%s\n", error == nil ? "Unable to open the signed container." : error.description.UTF8String);
		return 3;
	}
	if (![NSFileManager.defaultManager changeCurrentDirectoryPath:destinationUrl.path]) {
		fprintf (stderr, "Unable to change the working directory to %s.\n", destinationUrl.path.UTF8String);
		return 4;
	}

	[container
		startUnarchivingAtPath:destinationUrl.path
		notifyOnQueue:dispatch_get_main_queue ()
		progress:^void (double progress, NSString *status) {
			static NSInteger lastPercent = -1;
			static NSString *lastStatus = nil;
			NSInteger currentPercent = (NSInteger) progress;

			if (currentPercent != lastPercent || ![status isEqualToString:lastStatus]) {
				printf ("%g: %s\n", progress / 100.0, status.UTF8String);
				fflush (stdout);
				lastPercent = currentPercent;
				lastStatus = [status copy];
			}
		}
		finish:^void (BOOL failed) {
			exit (failed ? 1 : 0);
		}];

	dispatch_main ();
}
