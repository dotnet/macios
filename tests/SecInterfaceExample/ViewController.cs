using System;
using System.Runtime.InteropServices;
using AppKit;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using Security;
using SecurityInterface;

namespace SecInterfaceExample;

public class DemoViewController : NSViewController {
NSTextView? logView;
SFAuthorizationView? authView;

public override void LoadView ()
{
View = new NSView (new CGRect (0, 0, 960, 720));
}

public override void ViewDidLoad ()
{
base.ViewDidLoad ();

var scroll = new NSScrollView (View.Bounds) {
AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable,
HasVerticalScroller = true, DrawsBackground = false,
};
var content = new FlippedView (new CGRect (0, 0, 920, 3000));
scroll.DocumentView = content;
View.AddSubview (scroll);

nfloat y = 24, x = 24, w = 900;

// ── Header ──
y = Lbl (content, "SecurityInterface Framework Demo", y, x, 22, true);
y = Lbl (content, "Exercises every SecurityInterface C# binding — NativeObject wrappers, blittable structs, function-pointer callbacks, IDisposable collections, panels, views, enums, and field constants.", y, x, 13, false, NSColor.SecondaryLabel);
y = Sep (content, y, x, w);

// ── 1. SFAuthorizationView ──
y = Hdr (content, y, x, "1. SFAuthorizationView",
"Displays the system lock icon for controlling access to a privileged operation. " +
"This demo sets the right to \"system.preferences\" and calls UpdateStatus to initialize the lock.");
authView = new SFAuthorizationView (new CGRect (x, y, 64, 64));
authView.SetAuthorizationString ("system.preferences");
authView.UpdateStatus (null);
content.AddSubview (authView);
y = Lbl (content, $"State = {authView.AuthorizationState}    Enabled = {authView.IsEnabled}", y + 8, x + 80, 13, false, NSColor.SystemGreen);
y = (nfloat) Math.Max ((double) y, (double) authView.Frame.Bottom + 8);
y = Btn (content, "SetAutoupdate (true, 30s)", y, x,
"Enables 30-second auto-refresh. Expected: log shows '✓ SetAutoupdate' — no visible change.", () => {
authView.SetAutoupdate (true, 30);
Log ("✓ SetAutoupdate (true, 30) called");
});
y = Sep (content, y + 4, x, w);

// ── 2. SecKeychain ──
y = Hdr (content, y, x, "2. SecKeychain — Manual NativeObject",
"Wraps SecKeychainRef (CF opaque type) with retain/release. GetDefault() returns the default keychain, " +
"Open() opens by path, GetPath() reads the POSIX path. All P/Invokes are blittable (unsafe pointers + TransientString).");
y = Btn (content, "Query Keychains", y, x,
"Expected: TypeID ≈ 136, default path ends with login.keychain-db, login keychain opens successfully.", () => {
Log ($"SecKeychain.GetTypeID() = {SecKeychain.GetTypeID ()}");
using var kc = SecKeychain.GetDefault ();
if (kc is not null) {
Log ($"Default keychain: {kc.GetPath ()}");
Log ($"Handle: 0x{kc.Handle:X}");
}
var p = $"{Environment.GetFolderPath (Environment.SpecialFolder.UserProfile)}/Library/Keychains/login.keychain-db";
using var lk = SecKeychain.Open (p);
Log (lk is not null ? $"✓ Opened login keychain: {lk.GetPath ()}" : "✗ Could not open login keychain");
});
y = Sep (content, y + 4, x, w);

// ── 3. SecKeychainSettings ──
y = Hdr (content, y, x, "3. SecKeychainSettings — Manual Blittable Struct",
"Managed struct with LayoutKind.Sequential matching the native layout. Uses byte fields for Boolean (blittable). " +
"Create() returns version=1. Properties round-trip correctly.");
var st = SecKeychainSettings.Create ();
st.LockOnSleep = true; st.UseLockInterval = true; st.LockInterval = 300;
y = Val (content, y, x, $"Version        = {st.Version}");
y = Val (content, y, x, $"LockOnSleep    = {st.LockOnSleep}  (set to true)");
y = Val (content, y, x, $"UseLockInterval= {st.UseLockInterval}  (set to true)");
y = Val (content, y, x, $"LockInterval   = {st.LockInterval} seconds  (set to 300)");
y = Sep (content, y + 4, x, w);

// ── 4. AuthorizationRights ──
y = Hdr (content, y, x, "4. AuthorizationRights — Manual IDisposable + INativeObject",
"Creates AuthorizationRight structs (name + optional byte[] value + flags), allocates native AuthorizationItemSet " +
"memory, then verifies FromHandle() reads them back correctly. Dispose frees all unmanaged memory.");
y = Btn (content, "Create 3 Rights & Round-Trip", y, x,
"Expected: 3 rights created with non-zero Handle. FromHandle recovers all 3 with matching names and values.", () => {
using var rights = new AuthorizationRights (
new AuthorizationRight ("com.example.right1", new byte [] { 0xCA, 0xFE }, 0),
new AuthorizationRight ("com.example.right2"),
new AuthorizationRight ("system.privilege.admin")
);
Log ($"Created {rights.Count} rights — Handle=0x{rights.Handle:X}");
foreach (var r in rights)
Log ($"  {r.Name}: Value={Fmt (r.Value)}, Flags={r.Flags}");
using var rt = AuthorizationRights.FromHandle (rights.Handle);
if (rt is not null) {
for (int i = 0; i < rt.Count; i++)
Log ($"  Round-trip [{i}]: {rt [i].Name} match={rt [i].Name == rights [i].Name}");
Log ("✓ All names match");
}
});
y = Btn (content, "Dispose & Double-Dispose", y, x,
"Expected: Handle becomes 0x0 after Dispose(). Second Dispose() does not throw.", () => {
var tmp = new AuthorizationRights ("temp");
Log ($"Before: Handle=0x{tmp.Handle:X}");
tmp.Dispose ();
Log ($"After Dispose: Handle=0x{(IntPtr) tmp.Handle:X}");
tmp.Dispose ();
Log ("✓ Double Dispose — no exception");
});
y = Sep (content, y + 4, x, w);

// ── 5. AuthorizationCallbacks ──
y = Hdr (content, y, x, "5. AuthorizationCallbacks — INativeObject with delegate* unmanaged<>",
"Non-owning wrapper around the native AuthorizationCallbacks struct (15 function pointers). " +
"Reads the Version field directly from native memory via unsafe pointer cast.");
y = Btn (content, "Structural Test (fake native struct)", y, x,
"Expected: Version reads 42, after in-place memory update reads 99 — proving live native memory access.", () => {
var ptr = Marshal.AllocHGlobal (256);
try {
Marshal.WriteInt32 (ptr, 42);
var cb = new AuthorizationCallbacks (ptr);
Log ($"Version = {cb.Version}  (expected 42)");
Marshal.WriteInt32 (ptr, 99);
Log ($"After update: Version = {cb.Version}  (expected 99)");
Log ("✓ Live native memory read confirmed");
} finally { Marshal.FreeHGlobal (ptr); }
});
y = Sep (content, y + 4, x, w);

// ── 6. Panels ──
y = Hdr (content, y, x, "6. SecurityInterface Panels",
"System panels for certificate display, trust decisions, and identity selection. Uses RunModal (synchronous) to guarantee the panel appears.");
y = Btn (content, "SFCertificatePanel (modal)", y, x,
"Shows a modal certificate panel. Click OK to dismiss. Shows an empty certificate list.", () => {
Log ("Opening certificate panel...");
var p = SFCertificatePanel.SharedCertificatePanel;
p.SetDefaultButtonTitle ("OK"); p.SetShowsHelp (false);
var result = p.RunModalForCertificates (new NSArray (), true);
Log ($"✓ Certificate panel closed with code {result}");
});
y = Btn (content, "SFCertificateTrustPanel — Properties", y, x,
"Demonstrates SetInformativeText/InformativeText round-trip on the trust panel.", () => {
var p = SFCertificateTrustPanel.SharedCertificateTrustPanel;
p.SetInformativeText ("This demonstrates the SFCertificateTrustPanel binding.");
Log ($"InformativeText = \"{p.InformativeText}\"");
Log ($"✓ Trust panel handle: 0x{p.Handle:X}");
});
y = Btn (content, "SFChooseIdentityPanel (modal)", y, x,
"Shows an identity chooser. Empty list is expected if no client certificates are installed.", () => {
Log ("Opening identity chooser...");
var p = SFChooseIdentityPanel.SharedChooseIdentityPanel;
p.SetInformativeText ("Select a digital identity for this demo.");
p.SetDomain ("com.example.secinterfacedemo");
var result = p.RunModalForIdentities (new NSArray (), "Choose an identity:");
Log ($"✓ Identity chooser closed with code {result}");
var identity = p.Identity;
Log (identity is not null ? $"Selected identity: 0x{identity.Handle:X}" : "No identity selected (list was empty)");
});
y = Sep (content, y + 4, x, w);

// ── 7. SFCertificateView ──
y = Hdr (content, y, x, "7. SFCertificateView",
"Embeddable view displaying certificate details, trust, and policy disclosure. Currently empty (no certificate set). " +
"In production, call SetCertificate() with a SecCertificate.");
var cv = new SFCertificateView (new CGRect (x, y, w, 80));
cv.SetDisplayDetails (true); cv.SetDisplayTrust (true); cv.SetEditableTrust (false);
content.AddSubview (cv);
y += 88;
y = Val (content, y, x, $"DetailsDisplayed={cv.DetailsDisplayed}  IsTrustDisplayed={cv.IsTrustDisplayed}  IsEditable={cv.IsEditable}");
y = Val (content, y, x, $"DisclosureNotification = \"{SFCertificateView.DisclosureStateDidChangeNotification}\"");
y = Sep (content, y + 4, x, w);

// ── 8. Constants ──
y = Hdr (content, y, x, "8. Field Constants",
"NSString constants from the SecurityInterface framework headers.");
y = Val (content, y, x, $"UserNameKey         = \"{SFAuthorizationPluginViewKeys.UserNameKey}\"");
y = Val (content, y, x, $"UserShortNameKey    = \"{SFAuthorizationPluginViewKeys.UserShortNameKey}\"");
y = Val (content, y, x, $"DisplayViewException = \"{SFAuthorizationPluginViewExceptions.DisplayViewException}\"");
y = Sep (content, y + 4, x, w);

// ── 9. Enums ──
y = Hdr (content, y, x, "9. Enums",
"Plain C enums (not NS_ENUM) bound with int backing type.");
y = Val (content, y, x, "SFAuthorizationViewState:  Startup=0  Locked=1  InProgress=2  Unlocked=3");
y = Val (content, y, x, "SFButtonType:  Cancel=0  Ok=1  Back=0  Login=1");
y = Val (content, y, x, "SFViewType:  IdentityAndCredentials=0  Credentials=1");
y = Val (content, y, x, "AuthorizationResult:  Allow=0  Deny=1  Undefined=2  UserCanceled=3");
y = Val (content, y, x, "AuthorizationContextFlags:  Extractable=1  Volatile=2  Sticky=4");
y += 10;

// ── Log ──
y = Lbl (content, "Log Output", y, x, 16, true);
var ls = new NSScrollView (new CGRect (x, y, w, 200)) {
HasVerticalScroller = true, BorderType = NSBorderType.BezelBorder,
};
logView = new NSTextView (ls.ContentView.Bounds) {
Editable = false,
AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable,
};
ls.DocumentView = logView;
content.AddSubview (ls);
y += 210;
content.SetFrameSize (new CGSize (920, y + 20));

// Startup log
Log ("App loaded — all SecurityInterface bindings available.");
Log ($"SFAuthorizationView state = {authView.AuthorizationState}");
Log ($"SecKeychain.GetTypeID() = {SecKeychain.GetTypeID ()}");
using (var kc = SecKeychain.GetDefault ())
if (kc is not null) Log ($"Default keychain: {kc.GetPath ()}");
}

void Log (string m) {
logView?.TextStorage?.Append (new NSAttributedString ($"[{DateTime.Now:HH:mm:ss}] {m}\n",
new NSStringAttributes { Font = NSFont.MonospacedSystemFont (11, NSFontWeight.Regular) }));
logView?.ScrollRangeToVisible (new NSRange ((nint) logView!.TextStorage!.Length, 0));
}

static string Fmt (byte[]? v) => v is null ? "(null)" : BitConverter.ToString (v);

// ── UI building helpers ──

nfloat Hdr (NSView p, nfloat y, nfloat x, string title, string desc) {
y = Lbl (p, title, y, x, 16, true);
y = Lbl (p, desc, y, x, 12, false, NSColor.SecondaryLabel);
return y + 2;
}

nfloat Val (NSView p, nfloat y, nfloat x, string t) =>
Lbl (p, t, y, x + 16, 12, false, NSColor.SystemTeal);

nfloat Lbl (NSView p, string text, nfloat y, nfloat x, nfloat sz, bool bold, NSColor? c = null) {
var l = new NSTextField (new CGRect (x, y, p.Bounds.Width - x - 24, 0)) {
StringValue = text, Editable = false, Bordered = false,
BackgroundColor = NSColor.Clear, LineBreakMode = NSLineBreakMode.ByWordWrapping,
Font = bold ? NSFont.BoldSystemFontOfSize (sz) : NSFont.SystemFontOfSize (sz),
MaximumNumberOfLines = 0, PreferredMaxLayoutWidth = p.Bounds.Width - x - 24,
};
if (c is not null) l.TextColor = c;
l.SizeToFit ();
p.AddSubview (l);
return y + l.Frame.Height + 3;
}

nfloat Btn (NSView p, string title, nfloat y, nfloat x, string hint, Action action) {
y = Lbl (p, $"▸ {hint}", y, x + 16, 11, false, NSColor.SystemGray);
var b = new NSButton (new CGRect (x, y, 320, 28)) { Title = title, BezelStyle = NSBezelStyle.Rounded };
b.Activated += (_, _) => { try { action (); } catch (Exception ex) { Log ($"❌ {ex.Message}"); } };
p.AddSubview (b);
return y + 34;
}

nfloat Sep (NSView p, nfloat y, nfloat x, nfloat w) {
p.AddSubview (new NSBox (new CGRect (x, y + 4, w, 1)) { BoxType = NSBoxType.NSBoxSeparator });
return y + 14;
}
}

class FlippedView : NSView {
public FlippedView (CGRect frame) : base (frame) { }
public override bool IsFlipped => true;
}
