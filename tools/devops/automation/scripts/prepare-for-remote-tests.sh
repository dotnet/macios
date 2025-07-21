#!/bin/bash -eux

cd "$(dirname "${BASH_SOURCE[0]}")/../../../.."

# Install the local .NET we're using into XMA's directory
# (we can't point XMA to our local directory)
mkdir -p ~/Library/Caches/maui/PairToMac/Sdks
cp -cRH ./builds/downloads/dotnet ~/Library/Caches/maui/PairToMac/Sdks
sed '/local-tests-feed/d' ./NuGet.config > ~/Library/Caches/maui/PairToMac/Sdks/dotnet/NuGet.config

mkdir -p ~/Library/Caches/maui/PairToMac/Sdks/.home/.nuget/NuGet/
cp ~/Library/Caches/maui/PairToMac/Sdks/dotnet/NuGet.config ~/Library/Caches/maui/PairToMac/Sdks/.home/.nuget/NuGet/NuGet.Config

# some diagnostics
cat ~/Library/Caches/maui/PairToMac/Sdks/dotnet/NuGet.config
cd ~/Library/Caches/maui/PairToMac/Sdks/dotnet/
./dotnet --info || true
