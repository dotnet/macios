﻿namespace monotouchtestWatchKitExtension

open System
open System.Collections
open System.Linq
open System.Threading

open WatchKit
open Foundation

open NUnit.Framework.Internal.Filters
open MonoTouch.NUnit.UI

[<Register ("InterfaceController")>]
type InterfaceController (handle: IntPtr) =
    inherit WKInterfaceController (handle)

    let mutable runner = Unchecked.defaultof<WatchOSRunner>

    [<Outlet ("lblStatus")>]
    member val lblStatus = Unchecked.defaultof<WKInterfaceLabel> with get, set

    [<Outlet ("lblSuccess")>]
    member val lblSuccess = Unchecked.defaultof<WKInterfaceLabel> with get, set

    [<Outlet ("lblFailed")>]
    member val lblFailed = Unchecked.defaultof<WKInterfaceLabel> with get, set

    [<Outlet ("lblIgnored")>]
    member val lblIgnored = Unchecked.defaultof<WKInterfaceLabel> with get, set

    [<Outlet ("lblInconclusive")>]
    member val lblInconclusive = Unchecked.defaultof<WKInterfaceLabel> with get, set

    [<Outlet ("cmdRun")>]
    member val cmdRun = Unchecked.defaultof<WKInterfaceButton> with get, set

    member this.LoadTests () =
        runner <- new WatchOSRunner ()
        let ce = new CategoryExpression ("MobileNotWorking,NotOnMac,NotWorking,ValueAdd,CAS,InetAccess,NotWorkingInterpreter")
        runner.Filter <- new NotFilter (ce.Filter)
        let tp = this.GetType ()
        runner.Add (tp.Assembly)
        ThreadPool.QueueUserWorkItem (fun v ->
            runner.LoadSync ()
            this.BeginInvokeOnMainThread (fun x ->
                this.lblStatus.SetText (String.Format ("{0} tests", runner.TestCount))
                this.RenderResults ()
                this.cmdRun.SetEnabled (true)
                this.cmdRun.SetHidden (false)
            )
        )
        |> ignore

    override this.Awake (context: NSObject) =
        base.Awake (context)
        this.BeginInvokeOnMainThread (fun x ->
            this.LoadTests ()
            ()
        )

    member this.RenderResults () =
        this.lblSuccess.SetText (String.Format ("Passed: {0}/{1} {2}%", runner.PassedCount, runner.TestCount, 100 * runner.PassedCount / runner.TestCount))
        this.lblFailed.SetText (String.Format ("Failed: {0}/{1} {2}%", runner.FailedCount, runner.TestCount, 100 * runner.FailedCount / runner.TestCount))
        this.lblIgnored.SetText (String.Format ("Ignored: {0}/{1} {2}%", runner.IgnoredCount, runner.TestCount, 100 * runner.IgnoredCount / runner.TestCount))
        this.lblInconclusive.SetText (String.Format ("Inconclusive: {0}/{1} {2}%", runner.InconclusiveCount, runner.TestCount, 100 * runner.InconclusiveCount / runner.TestCount))

    member this.RunTests () = 
        this.cmdRun.SetEnabled (false)
        this.lblStatus.SetText ("Running")
        this.BeginInvokeOnMainThread (fun v ->
            runner.Run ()
            this.BeginInvokeOnMainThread (fun x ->
                this.RenderResults ()
            )
        )

    member this.RunTests (obj: NSObject) =
        this.RunTests ()

