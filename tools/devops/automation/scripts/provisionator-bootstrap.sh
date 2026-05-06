#!/bin/bash -e
# shellcheck disable=SC2220,SC2181
#
# Remove the following GUID if you do not wish for this script self-update:
# D6F71FB5-F2A7-4A62-86D3-10DFE08301CC
# https://github.com/xamarin/provisionator

while getopts ":t:" opt; do
  case "${opt}" in
    t)
      TOKEN=${OPTARG}
      shift 2
      ;;
  esac
done

function selfdir { (cd "$(dirname "$1")"; echo "$PWD"; ) }

selfdir=$(selfdir "$0")

channel="${PROVISIONATOR_CHANNEL:-latest}"

provisionator_sas_token=$AUTH_TOKEN_PROVISIONATOR
if [[ -n $provisionator_sas_token ]]; then
    echo "Downloading Provisionator directly from BosStorageMirror Azure blob storage"
    base_url="https://bosstoragemirror.blob.core.windows.net/provisionator/664bd334021e3102cdef1af66c4fc9f1b2ecd2a21b47419e80d08da1f6c61c2a/${channel}"
else
  # Grab auth token from Environment first
  # Otherwise set to provided option, if available
  base_url="https://dl.internalx.com/provisionator/664bd334021e3102cdef1af66c4fc9f1b2ecd2a21b47419e80d08da1f6c61c2a/${channel}"
  auth_token=$AUTH_TOKEN_GITHUB_COM
  if [[ -z $auth_token ]]; then
    echo "Setting auth_token to input token and not AUTH_TOKEN_GITHUB_COM"
    auth_token=$TOKEN
  fi
  if [[ -z $auth_token ]]; then
    echo "WARNING: Please set AUTH_TOKEN_GITHUB_COM to a GitHub PAT before running provisionator or run provisionator.sh -t <TOKEN> ..."
    echo "At a future point, Provisionator will not be able to bootstrap without the PAT set properly"
    base_url="https://bosstoragemirror.blob.core.windows.net/provisionator/664bd334021e3102cdef1af66c4fc9f1b2ecd2a21b47419e80d08da1f6c61c2a/${channel}"
  fi
fi

latest_version_url="${base_url}/version"

if [[ -n $provisionator_sas_token ]]; then
    echo "latest_version_url: Appending SAS token"
    latest_version_url="${latest_version_url}?${provisionator_sas_token}"
fi

archive_name="provisionator.osx.10.11-x64.zip"
archive_path="${selfdir}/${archive_name}"
archive_extract_path="${selfdir}/_provisionator"
archive_url="${base_url}/${archive_name}"
if [[ -n $provisionator_sas_token ]]; then
    echo "archive_url: Appending SAS token"
    archive_url="${archive_url}?${provisionator_sas_token}"
fi

binary_path="${archive_extract_path}/provisionator"
pk_path="${archive_extract_path}/pk"

set +e

# Double brackets are required for the following condition to evaulate as expected
if [[ -z $auth_token ]] || [[ -n $provisionator_sas_token ]]; then
  if [[ -n $provisionator_sas_token ]]; then
    echo "Fetching latest version with SAS token"
  else
    echo "Fetching latest version without auth token"
  fi
  latest_version="$(curl -fsL "${latest_version_url}")"
else
  echo "Fetching latest version with auth token"
  latest_version="$(curl -fsL -H "Authorization: token ${auth_token}" "${latest_version_url}")"
fi
if [[ $? != 0 ]]; then
  echo "Unable to determine latest version from ${latest_version_url}"
  echo "Please check to make sure a valid GitHub PAT was provided"
  exit 1
fi
set -e

function update_in_place {
  echo "Downloading Provisionator $latest_version..."
  local progress_type="-s"
  tty -s && progress_type="-#"
  if [[ -z $auth_token ]] || [[ -n $provisionator_sas_token ]]; then
    curl -fL $progress_type -o "$archive_path" "$archive_url"
  else
    curl -fLH "Authorization: token ${auth_token}"  $progress_type -o "$archive_path" "$archive_url"
  fi
  rm -rf "$archive_extract_path"
  unzip -q -o -d "$archive_extract_path" "$archive_path"
  rm -f "$archive_path"
}

if [[ -f "$binary_path" ]]; then
  chmod +x "$binary_path"
  current_version="$("$binary_path" -version 2>&1 || true)"
  if [[ "$latest_version" != "$current_version" ]]; then
    update_in_place
  fi
else
  update_in_place
fi

# Check for the pk (PackageKitTool) binary, which should be included in the provisionator macOS ZIP package
if [[ "$OSTYPE" == "darwin"* ]]; then
  echo "Checking for PackageKitTool (pk) binary..."
  if [[ -f "$pk_path" ]]; then
    echo "PackageKitTool (pk) binary found: ${pk_path}"
    echo "chmod +x ${pk_path}"
    chmod +x "$pk_path"
  else
    echo "ERROR: pk file not found: ${pk_path}"
    exit 5
  fi
fi

# Opt out of macOS backward compat so correct Big Sur OS versions can be obtained.
# More info https://github.com/ziglang/zig/issues/7569
export SYSTEM_VERSION_COMPAT=0

# TODO: The executable permission should be applied here. However, since this script is reflective of production scripts used by other clients/teams avoid modifying the executable permission here.
#       A reason to avoid this is that the executable permission is expected to be preserved in the ZIP file produced by the ClientTools.Provisionator build pipeline
# CONSIDER: A possible follow up item would be to go update all the production scripts to apply the executable permission here to ensure the provisionator binary has the executable permission set
#       This may also involve updating the Provisionator Azure DevOps provisionator task to ensure it applies the executable attribute after ZIP extraction
#       https://devdiv.visualstudio.com/Engineering/_git/xamarin-azdev-extension?path=/provisionator-task
# echo "Applying executable attribute: ${binary_path}"
# chmod +x "$binary_path"
exec caffeinate "$binary_path" "$@"
