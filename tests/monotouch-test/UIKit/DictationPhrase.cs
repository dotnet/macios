// Copyright 2012 Xamarin Inc. All rights reserved

#if !MONOMAC

using System.Drawing;
using UIKit;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DictationPhraseTest {

		[Test]
		public void Defaults ()
		{
			using (UIDictationPhrase dp = new UIDictationPhrase ()) {
				ClassicAssert.Null (dp.AlternativeInterpretations, "AlternativeInterpretations");
				ClassicAssert.Null (dp.Text, "Text");
			}
		}
	}
}

#endif // !MONOMAC
