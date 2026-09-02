// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Sharpie.Bind.Attributes;

public abstract class GroupedAttribute : ICSharpCode.NRefactory.CSharp.Attribute {
	public int GroupNumber { get; set; }

	protected GroupedAttribute (int groupNumber)
	{
		GroupNumber = groupNumber;
	}
}
