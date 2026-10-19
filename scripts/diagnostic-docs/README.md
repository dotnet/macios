<!-- Copyright (c) Microsoft Corporation. Licensed under the MIT License. -->

# diagnostic-docs

Validates diagnostic Markdown, explicit English-resource/public-code mappings, current numeric allocations, historical reservations, includes and local links. Generates the combined reference and historical compatibility indexes without rewriting diagnostic bodies.

Run `bash docs/update-diagnostic-docs.sh` from the repository root to update indexes, or append `check` to validate without writing. `check-incoming <docs-mobile-ios-directory>` validates reverse-sync input, including its generated index, against the local resource and historical manifests.

See [the authoring guide](../../docs/messages/README.md) for the source format, allocation policy and publication boundaries.
