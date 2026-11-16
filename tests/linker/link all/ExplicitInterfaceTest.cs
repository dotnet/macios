// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LinkAll.ExplicitInterfaces {

	public interface IMarker {
		string Mark ();
	}

	[Preserve (AllMembers = true)]
	public class BaseMarker {
		public virtual string Identify () => "base";
	}

	[Preserve (AllMembers = true)]
	public class MarkerA : BaseMarker, IMarker, IComparable<MarkerA> {
		public string Mark () => "a";
		public int CompareTo (MarkerA? other) => 0;
		public override string Identify () => "markera";
	}

	public interface IContract<TLeft, TRight> {
		string Echo (string value);
		string Overload (int value);
		string Overload (string value);
		void ByRef (ref int value, out string text);
		string Value { get; set; }
		event Action Changed;
		string Plain<T> ();
		string Reference<T> () where T : class;
		string Construct<T> () where T : new();
		string Marker<T> () where T : IMarker;
		string Multiple<T> () where T : BaseMarker, IMarker, new();
		string Recursive<T> () where T : IComparable<T>;
		string Pair<T, U> ();
		string Overloaded<T> (T value);
		string Overloaded<T> (T value, int number);
		string Dependent<T, U> () where T : U;
	}

	[Preserve (AllMembers = true)]
	public class Target : IContract<Dictionary<string, List<int>>, long> {
		int eventCount;
		string IContract<Dictionary<string, List<int>>, long>.Echo (string value) => "echo:" + value;
		string IContract<Dictionary<string, List<int>>, long>.Overload (int value) => "int:" + value;
		string IContract<Dictionary<string, List<int>>, long>.Overload (string value) => "string:" + value;
		void IContract<Dictionary<string, List<int>>, long>.ByRef (ref int value, out string text)
		{
			value++;
			text = "ref:" + value;
		}
		string IContract<Dictionary<string, List<int>>, long>.Value { get; set; } = "initial";
		event Action IContract<Dictionary<string, List<int>>, long>.Changed {
			add => eventCount++;
			remove => eventCount--;
		}
		string IContract<Dictionary<string, List<int>>, long>.Plain<T> () => "plain:" + typeof (T).Name;
		string IContract<Dictionary<string, List<int>>, long>.Reference<T> () => "reference:" + typeof (T).Name;
		string IContract<Dictionary<string, List<int>>, long>.Construct<T> () => "construct:" + new T ()!.GetType ().Name;
		string IContract<Dictionary<string, List<int>>, long>.Marker<T> () => "marker:" + typeof (T).Name;
		string IContract<Dictionary<string, List<int>>, long>.Multiple<T> () => "multiple:" + new T ().Identify ();
		string IContract<Dictionary<string, List<int>>, long>.Recursive<T> () => "recursive:" + typeof (T).Name;
		string IContract<Dictionary<string, List<int>>, long>.Pair<T, U> () => "pair:" + typeof (T).Name + ":" + typeof (U).Name;
		string IContract<Dictionary<string, List<int>>, long>.Overloaded<T> (T value) => "one:" + value;
		string IContract<Dictionary<string, List<int>>, long>.Overloaded<T> (T value, int number) => "two:" + value + ":" + number;
		string IContract<Dictionary<string, List<int>>, long>.Dependent<T, U> () => "dependent:" + typeof (T).Name + ":" + typeof (U).Name;
	}

	public interface IGeneric<T> {
		T Echo (T value);
		U Convert<U> (U value);
		T Value { get; set; }
		event Action Changed;
	}

	[Preserve (AllMembers = true)]
	public class GenericTarget<T> : IGeneric<T> {
		T IGeneric<T>.Echo (T value) => value;
		U IGeneric<T>.Convert<U> (U value) => value;
		T IGeneric<T>.Value { get; set; } = default!;
		event Action IGeneric<T>.Changed { add { } remove { } }
	}

	[Preserve (AllMembers = true)]
	public struct ValueTarget : IGeneric<string> {
		string IGeneric<string>.Echo (string value) => value;
		U IGeneric<string>.Convert<U> (U value) => value;
		string IGeneric<string>.Value { get; set; }
		event Action IGeneric<string>.Changed { add { } remove { } }
	}

	public interface IStatic<T> {
		static abstract string Echo<U> ();
	}

	[Preserve (AllMembers = true)]
	public class StaticTarget : IStatic<string> {
		static string IStatic<string>.Echo<U> () => "static:" + typeof (U).Name;
	}

	public interface ISelected<T> {
		string Chosen<U> ();
		string Conditional ();
		string Discarded ();
	}

	public class MemberTarget : ISelected<string> {
		[Preserve]
		string ISelected<string>.Chosen<U> () => "chosen:" + typeof (U).Name;
		[Preserve (Conditional = true)]
		string ISelected<string>.Conditional () => "conditional";
		string ISelected<string>.Discarded () => "discarded";
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	[UnconditionalSuppressMessage ("Trimming", "IL2060", Justification = "This test verifies that preserved generic methods can be instantiated.")]
	[UnconditionalSuppressMessage ("AOT", "IL3050", Justification = "This test verifies that preserved generic methods can be instantiated.")]
	public class ExplicitInterfaceTest {
		const BindingFlags Declared = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

		// Only access the explicit interface implementations using reflection, so that they're only kept if they're preserved.
		static MethodInfo GetMethod (object target, string name, int parameterCount = -1)
		{
			return GetMethod (target.GetType (), name, parameterCount);
		}

		static MethodInfo GetMethod (Type type, string name, int parameterCount = -1)
		{
			var methods = type.GetMethods (Declared).Where (v => v.Name.EndsWith ("." + name, StringComparison.Ordinal));
			if (parameterCount >= 0)
				methods = methods.Where (v => v.GetParameters ().Length == parameterCount);
			var rv = methods.ToArray ();
			Assert.That (rv.Length, Is.EqualTo (1), $"Method {name} in {type}");
			return rv [0];
		}

		[Test]
		public void Methods ()
		{
			var target = new Target ();
			Assert.That (GetMethod (target, "Echo").Invoke (target, new object [] { "test" }), Is.EqualTo ("echo:test"), "Echo");

			var overloads = target.GetType ().GetMethods (Declared).Where (v => v.Name.EndsWith (".Overload", StringComparison.Ordinal)).ToArray ();
			Assert.That (overloads.Length, Is.EqualTo (2), "Overloads");
			Assert.That (overloads.Single (v => v.GetParameters () [0].ParameterType == typeof (int)).Invoke (target, new object [] { 4 }), Is.EqualTo ("int:4"), "Overload (int)");
			Assert.That (overloads.Single (v => v.GetParameters () [0].ParameterType == typeof (string)).Invoke (target, new object [] { "x" }), Is.EqualTo ("string:x"), "Overload (string)");

			var arguments = new object? [] { 9, null };
			GetMethod (target, "ByRef").Invoke (target, arguments);
			Assert.That (arguments, Is.EqualTo (new object [] { 10, "ref:10" }), "ByRef");
		}

		[Test]
		public void PropertiesAndEvents ()
		{
			var target = new Target ();
			var type = target.GetType ();
			var property = type.GetProperties (Declared).SingleOrDefault (v => v.Name.EndsWith (".Value", StringComparison.Ordinal));
			Assert.That (property, Is.Not.Null, "Property");
			property!.SetValue (target, "updated");
			Assert.That (property.GetValue (target), Is.EqualTo ("updated"), "Property value");

			var @event = type.GetEvents (Declared).SingleOrDefault (v => v.Name.EndsWith (".Changed", StringComparison.Ordinal));
			Assert.That (@event, Is.Not.Null, "Event");
			Assert.That (@event!.AddMethod, Is.Not.Null, "Event add method");
			Assert.That (@event.RemoveMethod, Is.Not.Null, "Event remove method");
			@event.AddMethod!.Invoke (target, new object [] { (Action) (() => { }) });
		}

		[Test]
		[TestCase ("Plain", typeof (string), "plain:String")]
		[TestCase ("Plain", typeof (MarkerA), "plain:MarkerA")]
		[TestCase ("Reference", typeof (string), "reference:String")]
		[TestCase ("Construct", typeof (MarkerA), "construct:MarkerA")]
		[TestCase ("Marker", typeof (MarkerA), "marker:MarkerA")]
		[TestCase ("Multiple", typeof (MarkerA), "multiple:markera")]
		[TestCase ("Recursive", typeof (MarkerA), "recursive:MarkerA")]
		[TestCase ("Pair", typeof (string), typeof (MarkerA), "pair:String:MarkerA")]
		[TestCase ("Dependent", typeof (MarkerA), typeof (IMarker), "dependent:MarkerA:IMarker")]
		[TestCase ("Dependent", typeof (string), typeof (object), "dependent:String:Object")]
		public void GenericMethods (string name, Type typeArgument1, object typeArgument2OrExpected, string? expected = null)
		{
			var typeArguments = expected is null ? new [] { typeArgument1 } : new [] { typeArgument1, (Type) typeArgument2OrExpected };
			expected ??= (string) typeArgument2OrExpected;
			var target = new Target ();
			var method = GetMethod (target, name).MakeGenericMethod (typeArguments);
			if (name == "Dependent" && !RuntimeFeature.IsDynamicCodeSupported)
				Assert.Ignore ("NativeAOT doesn't generate code for generic methods with a 'T : U' constraint (not even without assembly preparation).");
			Assert.That (method.Invoke (target, null), Is.EqualTo (expected), name);
		}

		[Test]
		public void GenericMethodOverloads ()
		{
			var target = new Target ();
			Assert.That (GetMethod (target, "Overloaded", 1).MakeGenericMethod (typeof (string)).Invoke (target, new object [] { "x" }), Is.EqualTo ("one:x"), "One parameter");
			Assert.That (GetMethod (target, "Overloaded", 2).MakeGenericMethod (typeof (string)).Invoke (target, new object [] { "x", 3 }), Is.EqualTo ("two:x:3"), "Two parameters");
		}

		[Test]
		public void GenericOwner ()
		{
			object target = new GenericTarget<string> ();
			Assert.That (GetMethod (target, "Echo").Invoke (target, new object [] { "value" }), Is.EqualTo ("value"), "Echo");
			Assert.That (GetMethod (target, "Convert").MakeGenericMethod (typeof (string)).Invoke (target, new object [] { "converted" }), Is.EqualTo ("converted"), "Convert");
			Assert.That (target.GetType ().GetProperties (Declared).Count (v => v.Name.EndsWith (".Value", StringComparison.Ordinal)), Is.EqualTo (1), "Property");
			Assert.That (target.GetType ().GetEvents (Declared).Count (v => v.Name.EndsWith (".Changed", StringComparison.Ordinal)), Is.EqualTo (1), "Event");
		}

		[Test]
		public void ValueTypeOwner ()
		{
			object target = new ValueTarget ();
			Assert.That (GetMethod (target, "Echo").Invoke (target, new object [] { "value" }), Is.EqualTo ("value"), "Echo");
			Assert.That (GetMethod (target, "Convert").MakeGenericMethod (typeof (string)).Invoke (target, new object [] { "converted" }), Is.EqualTo ("converted"), "Convert");
		}

		[Test]
		public void StaticMethod ()
		{
			Assert.That (GetMethod (typeof (StaticTarget), "Echo").MakeGenericMethod (typeof (MarkerA)).Invoke (null, null), Is.EqualTo ("static:MarkerA"), "Echo");
		}

		[Test]
		public void IndividuallyPreservedMethods ()
		{
			var target = new MemberTarget ();
			Assert.That (GetMethod (target, "Chosen").MakeGenericMethod (typeof (string)).Invoke (target, null), Is.EqualTo ("chosen:String"), "Chosen");
			Assert.That (GetMethod (target, "Conditional").Invoke (target, null), Is.EqualTo ("conditional"), "Conditional");
			// ILLink removes unpreserved explicit interface implementations, but NativeAOT may keep them.
			if (RuntimeFeature.IsDynamicCodeSupported)
				Assert.That (target.GetType ().GetMethods (Declared).Where (v => v.Name.EndsWith (".Discarded", StringComparison.Ordinal)), Is.Empty, "Discarded");
		}
	}
}
