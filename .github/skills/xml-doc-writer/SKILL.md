---
name: xml-doc-writer
description: >-
  Write, validate, and improve XML documentation comments in C# source code.
  Use when asked to add, fix, review, or complete XML docs for C# APIs.
---

# XML Doc Writer

Write accurate, consistent XML documentation for the exact C# APIs in scope. This
skill may be invoked directly or loaded by another agent working on those APIs.

## Workflow

### 1. Gather context

Identify the exact types and members whose documentation is in scope. Read their
signatures, nearby documentation, inherited APIs, and relevant implementation
details before editing. Do not change API behavior while updating documentation.

### 2. Write and improve the documentation

- Add XML docs for undocumented public and protected types and members in scope.
- Remove documentation whose complete content is `To be added.`, then replace it
  with useful documentation when the API is in scope.
- Verify existing documentation for correctness, grammar, consistency, and clear
  descriptions of parameters, return values, exceptions, and side effects.
- Use `<see cref="..."/>` and `<paramref name="..."/>` when they make references
  clearer and can be resolved from the documented API.
- Prefer `/// <inheritdoc />` for overrides and interface implementations when the
  inherited documentation is accurate.
- Replace XML documentation `include` attributes with the referenced content.
  Search for all references before removing inlined content from the include file,
  and delete the include file only when it has no remaining content or consumers.
- Rewrite mentions of Xamarin.iOS or Xamarin.Mac using the current product names.
- Remove empty `<para>` and `<remarks>` elements.

### 3. Normalize formatting

- Keep the existing code indentation and remove whitespace between `///` and the
  XML element.
- Indent nested XML elements by two spaces per level.
- Order top-level elements as: `<summary>`, `<value>`, `<typeparam>`, `<param>`,
  `<returns>`, `<exception>`, `<remarks>`, and `<seealso>`.
- Preserve a final newline in every modified file.

### 4. Validate

Review the final diff for valid XML, resolvable `cref` values, complete parameter
and type-parameter coverage, and documentation that matches the API signatures and
behavior.
