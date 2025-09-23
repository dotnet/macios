using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.Macios.Generator.IO;

static class TabbedStreamWriterExtensions {

	/// <summary>
	/// Appends the comments and doc comments trivia to the writer.
	/// </summary>
	/// <param name="self">The current writer.</param>
	/// <param name="leadingTrivia">The leading trivia of a syntax node to append.</param>
	/// <returns>The writer itself.</returns>
	public static async Task<TabbedWriter<StreamWriter>> AppendCommentsTrivia (
		this TabbedWriter<StreamWriter> self, SyntaxTriviaList leadingTrivia)
	{
		// loop over the trivia and append only the doc comments or the comments
		foreach (var trivia in leadingTrivia) {
			// leading a single line
			if (trivia.IsKind (SyntaxKind.SingleLineCommentTrivia)
				|| trivia.IsKind (SyntaxKind.SingleLineDocumentationCommentTrivia)) {
				// This is a regular comment
				await self.WriteLineAsync (trivia.ToFullString ().Trim ());
			}

			if (trivia.IsKind (SyntaxKind.MultiLineCommentTrivia)
				|| trivia.IsKind (SyntaxKind.MultiLineDocumentationCommentTrivia)) {
				// multilines, we want to split them and write line by line so that we
				// can keep the correct indentation
				var triviaText = trivia.ToString ();
				var lines = triviaText.Split (['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

				for (var index = 0; index < lines.Length - 1; index++) {
					await self.WriteLineAsync (lines [index].Trim ());
				}
				// add the last line without trimming, since it might be important and we do not want to
				// mess with the formatting
				await self.WriteAsync (lines [^1]);
			}
		}
		return self;
	}

	/// <summary>
	/// Appends the using directives to the writer.
	/// </summary>
	/// <param name="self">The current writer.</param>
	/// <param name="usingDirectives">The collection of using directives to append.</param>
	/// <returns>The writer itself.</returns>
	public static async Task<TabbedWriter<StreamWriter>> AppendUsingDirectives (
		this TabbedWriter<StreamWriter> self, IReadOnlySet<string> usingDirectives)
	{
		// we want to create a hashset with the directives to avoid duplicates and we want to make sure
		// that the ObCBindings and the System.Versioning are always present
		HashSet<string> directives = [
			.. usingDirectives,
			"ObjCRuntime",
			"ObjCBindings",
			"System.Runtime.Versioning"
		];
		foreach (var directive in directives.OrderBy (d => d)) {
			await self.WriteLineAsync ($"using {directive};");
		}
		return self;
	}
}
