// Copyright 2017 Xamarin Inc.

using Mono.Cecil;
using Mono.Linker;
using Mono.Linker.Steps;

#nullable enable

namespace Xamarin.Linker.Steps {
	public class PreserveSmartEnumConversionsHandler : ExceptionalMarkHandler {
		protected override string Name { get; } = "Smart Enum Conversion Preserver";
		protected override int ErrorCode { get; } = 2200;

		PreserveSmartEnumConversion? preserver;
		PreserveSmartEnumConversion Preserver {
			get {
				if (preserver is null)
					preserver = new PreserveSmartEnumConversion (LinkContext, Mark);
				return preserver;
			}
		}

		public override void Initialize (LinkContext context, MarkContext markContext)
		{
			base.Initialize (context);
			markContext.RegisterMarkMethodAction (ProcessMethod);
		}

		bool Mark (Tuple<MethodDefinition, MethodDefinition> pair, bool alreadyProcessed, params MethodDefinition? [] conditions)
		{
			if (alreadyProcessed)
				return false;
			Context.Annotations.Mark (pair.Item1);
			Context.Annotations.Mark (pair.Item2);
			return true;
		}

		protected override void Process (MethodDefinition method)
		{
			static bool IsPropertyMethod (MethodDefinition method)
			{
				return method.IsGetter || method.IsSetter;
			}

			Preserver.ProcessAttributeProvider (method);
			Preserver.ProcessAttributeProvider (method.MethodReturnType);

			if (method.HasParameters) {
				foreach (var p in method.Parameters)
					Preserver.ProcessAttributeProvider (p);
			}
			if (IsPropertyMethod (method)) {
				foreach (PropertyDefinition property in method.DeclaringType.Properties)
					if (property.GetMethod == method || property.SetMethod == method) {
						Preserver.ProcessAttributeProvider (property);
						break;
					}
			}
		}
	}
}
