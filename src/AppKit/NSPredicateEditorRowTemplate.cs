//
// NSPredicateEditorRowTemplate.cs
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013 Xamarin Inc
//

#if !__MACCATALYST__

using System;
using System.Linq;
using System.Collections.Generic;

using Foundation;
using CoreData;

#nullable enable

namespace AppKit {
	public partial class NSPredicateEditorRowTemplate {
		/// <param name="compoundTypes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSPredicateEditorRowTemplate (params NSCompoundPredicateType [] compoundTypes)
			: this (Array.ConvertAll (compoundTypes, t => NSNumber.FromUInt32 ((uint) t)))
		{
		}

		/// <param name="leftExpressions">To be added.</param>
		///         <param name="rightExpressions">To be added.</param>
		///         <param name="operators">To be added.</param>
		///         <param name="modifier">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSPredicateEditorRowTemplate (
			IEnumerable<NSExpression> leftExpressions,
			IEnumerable<NSExpression> rightExpressions,
			IEnumerable<NSPredicateOperatorType> operators,
			NSComparisonPredicateModifier modifier = NSComparisonPredicateModifier.Direct,
			NSComparisonPredicateOptions options = NSComparisonPredicateOptions.None)
			: this (
				leftExpressions.ToArray (),
				rightExpressions.ToArray (),
				modifier,
				operators.Select (o => NSNumber.FromUInt32 ((uint) o)).ToArray (),
				options)
		{
		}

		/// <param name="leftExpressionsFromKeyPaths">To be added.</param>
		///         <param name="rightExpressionsFromConstants">To be added.</param>
		///         <param name="operators">To be added.</param>
		///         <param name="modifier">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSPredicateEditorRowTemplate (
			IEnumerable<string> leftExpressionsFromKeyPaths,
			IEnumerable<string> rightExpressionsFromConstants,
			IEnumerable<NSPredicateOperatorType> operators,
			NSComparisonPredicateModifier modifier = NSComparisonPredicateModifier.Direct,
			NSComparisonPredicateOptions options = NSComparisonPredicateOptions.None)
			: this (
				leftExpressionsFromKeyPaths.Select (k => NSExpression.FromKeyPath (k)),
				rightExpressionsFromConstants.Select (k => NSExpression.FromConstant (new NSString (k))),
				operators,
				modifier,
				options)
		{
		}

		/// <param name="leftExpressionFromKeyPath">To be added.</param>
		///         <param name="rightExpressionFromConstant">To be added.</param>
		///         <param name="operators">To be added.</param>
		///         <param name="modifier">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSPredicateEditorRowTemplate (
			string leftExpressionFromKeyPath,
			string rightExpressionFromConstant,
			IEnumerable<NSPredicateOperatorType> operators,
			NSComparisonPredicateModifier modifier = NSComparisonPredicateModifier.Direct,
			NSComparisonPredicateOptions options = NSComparisonPredicateOptions.None)
			: this (
				new [] { leftExpressionFromKeyPath },
				new [] { rightExpressionFromConstant },
				operators,
				modifier,
				options)
		{
		}

		/// <param name="leftExpressionFromKeyPath">To be added.</param>
		///         <param name="rightExpressionsFromConstants">To be added.</param>
		///         <param name="operators">To be added.</param>
		///         <param name="modifier">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSPredicateEditorRowTemplate (
			string leftExpressionFromKeyPath,
			IEnumerable<string> rightExpressionsFromConstants,
			IEnumerable<NSPredicateOperatorType> operators,
			NSComparisonPredicateModifier modifier = NSComparisonPredicateModifier.Direct,
			NSComparisonPredicateOptions options = NSComparisonPredicateOptions.None)
			: this (
				new [] { leftExpressionFromKeyPath },
				rightExpressionsFromConstants,
				operators,
				modifier,
				options)
		{
		}

		/// <param name="leftExpressions">To be added.</param>
		///         <param name="attributeType">To be added.</param>
		///         <param name="operators">To be added.</param>
		///         <param name="modifier">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSPredicateEditorRowTemplate (
			IEnumerable<NSExpression> leftExpressions,
			NSAttributeType attributeType,
			IEnumerable<NSPredicateOperatorType> operators,
			NSComparisonPredicateModifier modifier = NSComparisonPredicateModifier.Direct,
			NSComparisonPredicateOptions options = NSComparisonPredicateOptions.None)
			: this (
				leftExpressions.ToArray (),
				attributeType,
				modifier,
				operators.Select (o => NSNumber.FromUInt32 ((uint) o)).ToArray (),
				options)
		{
		}

		/// <param name="leftExpressionsFromKeyPaths">To be added.</param>
		///         <param name="attributeType">To be added.</param>
		///         <param name="operators">To be added.</param>
		///         <param name="modifier">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSPredicateEditorRowTemplate (
			IEnumerable<string> leftExpressionsFromKeyPaths,
			NSAttributeType attributeType,
			IEnumerable<NSPredicateOperatorType> operators,
			NSComparisonPredicateModifier modifier = NSComparisonPredicateModifier.Direct,
			NSComparisonPredicateOptions options = NSComparisonPredicateOptions.None)
			: this (
				leftExpressionsFromKeyPaths.Select (k => NSExpression.FromKeyPath (k)),
				attributeType,
				operators,
				modifier,
				options)
		{
		}

		/// <param name="leftExpressionFromKeyPath">To be added.</param>
		///         <param name="attributeType">To be added.</param>
		///         <param name="operators">To be added.</param>
		///         <param name="modifier">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSPredicateEditorRowTemplate (
			string leftExpressionFromKeyPath,
			NSAttributeType attributeType,
			IEnumerable<NSPredicateOperatorType> operators,
			NSComparisonPredicateModifier modifier = NSComparisonPredicateModifier.Direct,
			NSComparisonPredicateOptions options = NSComparisonPredicateOptions.None)
			: this (
				new [] { leftExpressionFromKeyPath },
				attributeType,
				operators,
				modifier,
				options)
		{
		}
	}
}
#endif // !__MACCATALYST__
