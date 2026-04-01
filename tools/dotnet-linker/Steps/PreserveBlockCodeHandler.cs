using System;
using System.Linq;

using Mono.Cecil;

using Mono.Linker;
using Mono.Linker.Steps;
using Mono.Tuner;

using Xamarin.Bundler;

#nullable enable

namespace Xamarin.Linker.Steps {

	public class PreserveBlockCodeHandler : ConfigurationAwareMarkHandler {

		protected override string Name { get; } = "Preserve Block Code";
		protected override int ErrorCode { get; } = 2240;

		public override void Initialize (LinkContext context, MarkContext markContext)
		{
			base.Initialize (context);
			markContext.RegisterMarkTypeAction (ProcessType);
		}

		protected override void Process (TypeDefinition type)
		{
			if (!PreserveBlockCodeStep.GetMembersToPreserve (type, out var field, out var method))
				return;

			Context.Annotations.Mark (field);
			Context.Annotations.Mark (method);
		}
	}
}
