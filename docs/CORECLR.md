# CoreCLR

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
