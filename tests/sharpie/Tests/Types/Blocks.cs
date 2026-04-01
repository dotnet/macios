using System;
using Foundation;
using ObjCRuntime;

// typedef void (^Action)();
delegate void Action ();

// typedef int (^Anon_Func_Long_Int)(long long);
delegate int Anon_Func_Long_Int (long arg0);

// typedef int (^Named_Func_Long_Int)(long long);
delegate int Named_Func_Long_Int (long arg0);

// typedef void (^Variadic)(int, ...);
delegate void Variadic (int arg0, IntPtr varArgs);

// @interface TypedefBlockTests
interface TypedefBlockTests {
	// -(Action)get_Action;
	[Export ("get_Action")]
	[Verify (MethodToProperty)]
	Action Get_Action { get; }

	// -(Anon_Func_Long_Int)get_Anon_Func_Long_Int;
	[Export ("get_Anon_Func_Long_Int")]
	[Verify (MethodToProperty)]
	Anon_Func_Long_Int Get_Anon_Func_Long_Int { get; }

	// -(Named_Func_Long_Int)get_Named_Func_Long_Int;
	[Export ("get_Named_Func_Long_Int")]
	[Verify (MethodToProperty)]
	Named_Func_Long_Int Get_Named_Func_Long_Int { get; }
}

// @interface PropertyBlockTests
interface PropertyBlockTests {
	// @property (readonly, copy) void (^action)();
	[Export ("action", ArgumentSemantic.Copy)]
	Action Action { get; }

	// @property (readonly, copy) void (^action_int)(int);
	[Export ("action_int", ArgumentSemantic.Copy)]
	Action<int> Action_int { get; }

	// @property (readonly, copy) void (^action_action)(void (^)());
	[Export ("action_action", ArgumentSemantic.Copy)]
	Action<Action> Action_action { get; }

	// @property (readonly, copy) void (^action_action_int)(void (^)(int));
	[Export ("action_action_int", ArgumentSemantic.Copy)]
	Action<Action<int>> Action_action_int { get; }

	// @property (readonly, copy) void (^action_action_action)(void (^)(void (^)()));
	[Export ("action_action_action", ArgumentSemantic.Copy)]
	Action<Action<Action>> Action_action_action { get; }

	// @property (readonly, copy) int (^func_int_int)(int);
	[Export ("func_int_int", ArgumentSemantic.Copy)]
	Func<int, int> Func_int_int { get; }

	// @property (readonly, copy) long (^func_short_nint)(short);
	[Export ("func_short_nint", ArgumentSemantic.Copy)]
	Func<short, nint> Func_short_nint { get; }
}

// @interface AnonymousBlockTests
interface AnonymousBlockTests {
	// -(void)set_Action:(void (^)())handler;
	[Export ("set_Action:")]
	void Set_Action (Action handler);

	// -(void)set_Action_int:(void (^)(int))handler;
	[Export ("set_Action_int:")]
	void Set_Action_int (Action<int> handler);

	// -(void)set_Action_short_int_long:(void (^)(short, int, long long))handler;
	[Export ("set_Action_short_int_long:")]
	void Set_Action_short_int_long (Action<short, int, long> handler);

	// -(void)set_Func_int:(int (^)())handler;
	[Export ("set_Func_int:")]
	void Set_Func_int (Func<int> handler);

	// -(void)set_Func_int_int:(int (^)(int))handler;
	[Export ("set_Func_int_int:")]
	void Set_Func_int_int (Func<int, int> handler);

	// -(void)set_Func_short_int_long_bool:(_Bool (^)(short, int, long long))handler;
	[Export ("set_Func_short_int_long_bool:")]
	void Set_Func_short_int_long_bool (Func<short, int, long, bool> handler);

	// -(void)set_Func_Func_short_Action_long_short_bool:(_Bool (^)(long long (^)(short, void (^)()), short))handler;
	[Export ("set_Func_Func_short_Action_long_short_bool:")]
	void Set_Func_Func_short_Action_long_short_bool (Func<Func<short, Action, long>, short, bool> handler);

	// -(void)set_Func_Func_short_Action_Action_int_nint_long_short_bool:(_Bool (^)(long long (^)(short, void (^)(void (^)(int, long))), short))handler;
	[Export ("set_Func_Func_short_Action_Action_int_nint_long_short_bool:")]
	void Set_Func_Func_short_Action_Action_int_nint_long_short_bool (Func<Func<short, Action<Action<int, nint>>, long>, short, bool> handler);
}
