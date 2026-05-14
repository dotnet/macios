#!/bin/bash
echo "error: sharpie is not supported with an x64 runtime." >&2
echo "" >&2
echo "sharpie requires Apple's libclang, which is only available for arm64." >&2
echo "Please install the arm64 version of .NET and run sharpie with the arm64 .NET runtime." >&2
exit 1
