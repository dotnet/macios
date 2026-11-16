using FSKit;

namespace macOSFSKitExtension1;

// The build finds this class (the only subclass of FSUnaryFileSystem in the project)
// and uses it to start the file system extension.
public class FileSystemClass : FSUnaryFileSystem, IFSUnaryFileSystemOperations {
	public void ProbeResource (FSResource resource, FSUnaryFileSystemOperationsProbeResourceResult replyHandler)
	{
		// Check if the resource contains a file system this extension supports.
		replyHandler (null, null);
	}

	public void LoadResource (FSResource resource, FSTaskOptions options, FSUnaryFileSystemOperationsLoadResourceResult replyHandler)
	{
		// Create and return an FSVolume for the resource.
		replyHandler (null, null);
	}

	public void UnloadResource (FSResource resource, FSTaskOptions options, FSUnaryFileSystemOperationsUnloadResourceResult reply)
	{
		reply (null);
	}
}
