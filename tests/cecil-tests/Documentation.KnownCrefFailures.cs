
using System.Collections.Generic;

namespace Cecil.Tests {
	public partial class Documentation {
		static HashSet<string> KnownCrefFailures = new HashSet<string> {
			"M:Foundation.NSDictionary`2.ToDictionary``2(System.Func{`0,`1,System.ValueTuple{``0,``1}}): Found element 'cref', should be element 'see' with attribute 'cref' (i.e. instead of <cref ... /> do <see cref=... />).",
			"M:Foundation.NSSet`1.Create``1(System.Collections.Generic.IEnumerable{``0},System.Func{``0,`0}): Found element 'cref', should be element 'see' with attribute 'cref' (i.e. instead of <cref ... /> do <see cref=... />).",
			"M:Foundation.NSSet`1.ToHashSet``1(System.Func{`0,``0}): Found element 'cref', should be element 'see' with attribute 'cref' (i.e. instead of <cref ... /> do <see cref=... />).",
		};
	}
}
