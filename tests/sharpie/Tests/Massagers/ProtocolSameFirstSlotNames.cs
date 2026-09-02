using Foundation;

// @protocol ProtocolSameFirstSlotNames
/*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/
[Protocol]
interface ProtocolSameFirstSlotNames {
	// @required -(void)firstSlot;
	[Abstract]
	[Export ("firstSlot")]
	void FirstSlot ();

	// @required -(void)firstSlot:(int)s1;
	[Abstract]
	[Export ("firstSlot:")]
	void FirstSlot (int s1);

	// @required -(void)firstSlot:(int)s1 a:(int)s2;
	[Abstract]
	[Export ("firstSlot:a:")]
	void A (int s1, int s2);

	// @required -(void)firstSlot:(int)s1 b:(int)s2;
	[Abstract]
	[Export ("firstSlot:b:")]
	void B (int s1, int s2);
}
