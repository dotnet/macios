// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.Macios.Generator.DataModel;

enum ReferenceKind {
	None,
	Ref,
	Out,
	In,
	RefReadOnlyParameter,
}

static class RefKindExtensions {

	/// <summary>
	/// Convert from the roslyn enum to our own enum to decouple the type and use t for sharpie.
	/// </summary>
	/// <param name="self">The refkind of a parameter.</param>
	/// <returns>The matching enum value.</returns>
	public static ReferenceKind ToReferenceKind (this RefKind self) => self switch {
		RefKind.Ref => ReferenceKind.Ref,
		RefKind.Out => ReferenceKind.Out,
		RefKind.In => ReferenceKind.In,
		RefKind.RefReadOnlyParameter => ReferenceKind.RefReadOnlyParameter,
		_ => ReferenceKind.None,
	};

	/// <summary>
	/// Returns the appropriate SyntaxTokenList for the given ReferenceKind.
	/// </summary>
	/// <param name="self">The ReferenceKind value.</param>
	/// <returns>A SyntaxTokenList representing the modifier(s) for the parameter.</returns>
	public static SyntaxTokenList ToTokens (this ReferenceKind self) => self switch {
		ReferenceKind.Ref => new (Token (SyntaxKind.RefKeyword)),
		ReferenceKind.Out => new (Token (SyntaxKind.OutKeyword)),
		ReferenceKind.In => new (Token (SyntaxKind.InKeyword)),
		ReferenceKind.RefReadOnlyParameter => new (Token (SyntaxKind.RefKeyword), Token (SyntaxKind.ReadOnlyKeyword)),
		_ => []
	};

	/// <summary>
	/// Converts the ReferenceKind to the corresponding SyntaxKind keyword.
	/// </summary>
	/// <param name="self">The ReferenceKind value.</param>
	/// <returns>The SyntaxKind keyword for the modifier, or SyntaxKind.None if not applicable.</returns>
	public static SyntaxKind ToSyntaxKind (this ReferenceKind self) => self switch {
		ReferenceKind.Ref => SyntaxKind.RefKeyword,
		ReferenceKind.Out => SyntaxKind.OutKeyword,
		ReferenceKind.In => SyntaxKind.InKeyword,
		ReferenceKind.RefReadOnlyParameter => SyntaxKind.RefKeyword, // 'ref readonly' is a combination, return 'ref'
		_ => SyntaxKind.None
	};
}

