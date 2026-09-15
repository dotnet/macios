# How to add BindAs support for new types

Currently the BindAs code only supports converting between a certain set of
types and NSValue, NSNumber and smart enums.

If your BindAs support does not involve NSValue or NSNumber, then the required
changes are a bit more extensive than I explain here (but this is still a good
starting point).

[Sample code][8]

The sample code is to support a new type for NSValue, the exact code locations will differ slightly for NSNumber (different switches, etc).

1. Add a test (or three)

    * Add an entry to [tests/test-libraries/testgenerator.cs][1] for the new type.
      testgenerator.cs will generate the code required to test your new BindAs
      support for all known scenarios.

    * Any other manual tests should go in monotouch-test.

2. Add native conversions functions to runtime/trampolines.m|h. In the sample
   code this is the two functions to convert between NSValue and
   NSDirectionalEdgeInsets:

   `xamarin_nsdirectionaledgeinsets_to_nsvalue`: [trampolines.h#197][2], [trampolines.m#1252][3]
   `xamarin_nsvalue_to_nsdirectionaledgeinsets`: [trampolines.h#161][4], [trampolines.m#1192][5]

3. Add a switch entry to [trampolines.m#1601][6] to use the two new conversion functions.

4. The registrar also needs to know ([Registrar.cs#794][7]).

5. And the static registrar needs to know too, so that it can call the right native conversion function ([StaticRegistrar.cs#4896][9], [StaticRegistrar.cs#4925][10]).

6. Now there's just the generator support left ([Generator.cs#390][11], [Generator.cs#470][12]).

7. Finally run the following tests (at least)

* All variations of monotouch-test (iOS/tvOS) on both simulator and device.
* link all on both simulator and device.


[1]: https://github.com/dotnet/macios/blob/18a22fd0812016cfdc8397d9b70981a1fc365fed/tests/test-libraries/testgenerator.cs#L125
[2]: https://github.com/dotnet/macios/blob/18a22fd0812016cfdc8397d9b70981a1fc365fed/runtime/xamarin/trampolines.h#L197
[3]: https://github.com/dotnet/macios/blob/18a22fd0812016cfdc8397d9b70981a1fc365fed/runtime/trampolines.m#L1252
[4]: https://github.com/dotnet/macios/blob/18a22fd0812016cfdc8397d9b70981a1fc365fed/runtime/xamarin/trampolines.h#L161
[5]: https://github.com/dotnet/macios/blob/18a22fd0812016cfdc8397d9b70981a1fc365fed/runtime/trampolines.m#L1192
[6]: https://github.com/dotnet/macios/blob/18a22fd0812016cfdc8397d9b70981a1fc365fed/runtime/trampolines.m#L1601-L1602
[7]: https://github.com/dotnet/macios/blob/18a22fd0812016cfdc8397d9b70981a1fc365fed/src/ObjCRuntime/Registrar.cs#L794
[8]: https://github.com/dotnet/macios/pull/2288/commits/b38c114fbe8c9d229ec41a312dc36802cb4f027e
[9]: https://github.com/dotnet/macios/blob/18a22fd0812016cfdc8397d9b70981a1fc365fed/tools/common/StaticRegistrar.cs#L4896
[10]: https://github.com/dotnet/macios/blob/18a22fd0812016cfdc8397d9b70981a1fc365fed/tools/common/StaticRegistrar.cs#L4925
[11]: https://github.com/dotnet/macios/blob/18a22fd0812016cfdc8397d9b70981a1fc365fed/src/bgen/Generator.cs#L390
[12]: https://github.com/dotnet/macios/blob/18a22fd0812016cfdc8397d9b70981a1fc365fed/src/bgen/Generator.cs#L470
