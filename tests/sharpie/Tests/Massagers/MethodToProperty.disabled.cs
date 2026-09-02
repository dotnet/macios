using Foundation;

// @interface MethodToProperty
interface MethodToProperty {
	// -(bool)isEnabled;
	[Export ("isEnabled")]
	bool IsEnabled ();

	// -(void)enable;
	[Export ("enable")]
	void Enable ();

	// -(bool)visible;
	[Export ("visible")]
	bool Visible ();

	// -(void)setVisible:(bool)visible;
	[Export ("setVisible:")]
	void SetVisible (bool visible);

	// -(bool)getState:(int)foo;
	[Export ("getState:")]
	bool GetState (int foo);
}
