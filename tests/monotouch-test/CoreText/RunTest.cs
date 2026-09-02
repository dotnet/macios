
using CoreGraphics;
using CoreText;

using MonoTouchFixtures.CoreGraphics;

namespace MonoTouchFixtures.CoreText {

	class MyOps : CTRunDelegateOperations {

		static public bool Ascent;
		static public bool Descent;
		static public bool Width;

		public MyOps ()
		{
			// to re-run the test
			Ascent = false;
			Descent = false;
			Width = false;
		}

		public override nfloat GetAscent ()
		{
			Ascent = true;
			return base.GetAscent ();
		}

		public override nfloat GetDescent ()
		{
			Descent = true;
			return base.GetDescent ();
		}

		public override nfloat GetWidth ()
		{
			Width = true;
			return base.GetWidth ();
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class RunTest {

		[Test]
		public void CustomOps ()
		{
			using (var o = new MyOps ())
			using (var d = new CTRunDelegate (o)) {
				Assert.That (d.Operations, Is.SameAs (o), "same");
			}
		}

		[Test]
		public void Runs ()
		{
			using (var mas = new NSMutableAttributedString ("Bonjour"))
			using (var rd = new CTRunDelegate (new MyOps ())) {
				var sa = new CTStringAttributes () {
					RunDelegate = rd,
				};
				mas.SetAttributes (sa, new NSRange (3, 3));
				using (var fs = new CTFramesetter (mas)) {
					Assert.That (MyOps.Ascent, Is.True, "Ascent called");
					Assert.That (MyOps.Descent, Is.True, "Descent called");
					Assert.That (MyOps.Width, Is.True, "Width called");
				}
			}
		}

		[Test]
		public void GetBaseAdvancesAndOrigins ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			using (var attributedString = new NSAttributedString ("Hello world."))
			using (var line = new CTLine (attributedString)) {
				var runs = line.GetGlyphRuns ();
				Assert.That (runs.Length, Is.EqualTo (1), "runs");
				runs [0].GetBaseAdvancesAndOrigins (new NSRange (0, 10), out var advances, out var origins);
				Assert.That (advances, Is.Not.Null, "advances");
				Assert.That (origins, Is.Not.Null, "origins");
			}
		}
	}
}
