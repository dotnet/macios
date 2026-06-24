// Copyright 2012-2013, 2016 Xamarin Inc. All rights reserved.

using Mono.Cecil;
using Mono.Linker;
using Mono.Linker.Steps;

#nullable enable

namespace Xamarin.Linker {
	public class OptimizeGeneratedCodeHandler : ExceptionalMarkHandler {
		protected override string Name { get; } = "Binding Optimizer";
		protected override int ErrorCode { get; } = 2020;

		OptimizeGeneratedCodeData? data;

		public override void Initialize (LinkContext context, MarkContext markContext)
		{
			base.Initialize (context);
			markContext.RegisterMarkMethodAction (ProcessMethod);
		}

		bool IsActiveFor (AssemblyDefinition assembly)
		{
			return OptimizeGeneratedCode.IsActiveFor (assembly, Profile, Annotations);
		}

		protected override void Process (MethodDefinition method)
		{
			if (!IsActiveFor (method.DeclaringType.Module.Assembly))
				return;

			if (data is null) {
				data = new OptimizeGeneratedCodeData {
					LinkContext = LinkContext,
					InlineIsArm64CallingConvention = LinkContext.App.InlineIsArm64CallingConventionForCurrentAbi,
					Optimizations = LinkContext.App.Optimizations,
					Device = LinkContext.App.IsDeviceBuild,
				};
			}
			OptimizeGeneratedCode.OptimizeMethod (data, method);
		}
	}
}
