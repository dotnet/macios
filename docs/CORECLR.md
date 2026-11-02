# CoreCLR

## Native object finalizer draining

Finalized `NSObject` instances release their native references on the main thread.
By default, each drain callback processes at most 256 objects or 5 ms of work,
then defers remaining work by 1 ms so the run loop can process other work.
A single native release cannot be interrupted and may exceed the time budget.

To restore the previous behavior, which processes the entire batch present at
the start of each callback without a time budget or continuation delay, set the
`Foundation.NSObject.UseUnboundedFinalizerDrain` AppContext switch to `true`:

```xml
<ItemGroup>
  <RuntimeHostConfigurationOption Include="Foundation.NSObject.UseUnboundedFinalizerDrain" Value="true" />
</ItemGroup>
```

Alternatively, call
`AppContext.SetSwitch ("Foundation.NSObject.UseUnboundedFinalizerDrain", true)`
at application startup. The default is `false`. Enabling this switch can cause
long main-thread stalls when many objects are awaiting release; it does not
change when garbage collections occur.

## How to build and use dotnet/runtime from source

1. Clone dotnet/runtime somewhere:

    ```shell
    cd ~/work/dotnet
    git clone git@github.com:dotnet/runtime
    # Checkout any specific branch you want to test
    ```

2. Configure dotnet/macios to use the cloned dotnet/runtime:

    ```shell
    cd ~/work/dotnet/macios
    make git-clean-all # Must start from a clean state when switching between downloaded and custom built dotnet/runtime.
    ./configure --custom-dotnet=$HOME/work/dotnet/runtime
    ```

3. Build dotnet/runtime using our script that builds everything we need:

    ```shell
    cd ~/work/dotnet/macios/dotnet
    ./build-custom-runtime.sh
    ```

4. Finally build dotnet/macios:

    ```shell
    cd ~/work/dotnet/macios
    make all -j8
    make install -j8
    ```

### Caveats

You must clean your dotnet/macios working copy every time you switch between
using a locally built dotnet/runtime and a downloaded version. Not all parts
of the build will pick up any configure changes for an incremental rebuild.
