using Foundation;

// @interface MethodToProperty
interface MethodToProperty {
	// -(bool)isEnabled;
	[Export ("isEnabled")]
	[Verify (MethodToProperty)]
	bool IsEnabled { get; }

	// -(void)enable;
	[Export ("enable")]
	void Enable ();

	// -(bool)visible;
	// -(void)setVisible:(bool)visible;
	[Export ("visible")]
	[Verify (MethodToProperty)]
	bool Visible { get; set; }

	// -(bool)getState:(int)foo;
	[Export ("getState:")]
	bool GetState (int foo);
}
