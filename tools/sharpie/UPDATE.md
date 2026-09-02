Updating CLANG/LLVM
===================

The steps to updating to a newer version of Clang/LLVM are:

1. Make sure there's an updated version of [ClangSharp](https://github.com/dotnet/clangsharp/) for the Clang/LLVM version we want to target.
2. Download the corresponding version of LLVM from their [releases page](https://github.com/llvm/llvm-project/releases/) (the macOS ARM64 / Apple Silicon archive). For instance for LLVM 21.1.8 this is the `LLVM-21.1.8-macOS-ARM64.tar.xz` file.
	* Note: we only support arm64, no need to worry about x64.
3. Extract the downloaded version of LLVM (say into `~/Downloads/LLVM-21.1.8-macOS-ARM64`).
4. Copy the `lib/clang/<major LLVM version>/include` directory into `clang/include` directory (remove the existing contents of the `clang/include` directory).
	* The `lib/clang/<major LLVM version>/lib` and `lib/clang/<major LLVM version>/share` directories are not needed. 
5. Bump the `ClangSharpPackageVersion` and `libclangPackageVersion` properties in the top level `Directory.Build.props` file.
6. Build and make sure everything works. New versions of Clang often come with breaking changes, so sometimes changes will be required in our source code.
