// =============================================================================
// Synthiam.Web.Forms - Application class for Blazor
// =============================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // HighDpiMode enum
  // ---------------------------------------------------------------------------
  public enum HighDpiMode {
    DpiUnaware = 0,
    SystemAware = 1,
    PerMonitor = 2,
    PerMonitorV2 = 3,
    DpiUnawareGdiScaled = 4
  }

  // ---------------------------------------------------------------------------
  // ThreadExceptionEventHandler delegate
  // ---------------------------------------------------------------------------
  public delegate void ThreadExceptionEventHandler(object sender, ThreadExceptionEventArgs e);

  // ---------------------------------------------------------------------------
  // Application
  // ---------------------------------------------------------------------------
  public static class Application {

    private static readonly FormCollection _openForms = new FormCollection();

    public static string StartupPath => AppDomain.CurrentDomain.BaseDirectory;

    public static string ExecutablePath =>
      System.Reflection.Assembly.GetEntryAssembly()?.Location ?? string.Empty;

    public static string ProductName { get; set; } = string.Empty;
    public static string ProductVersion { get; set; } = string.Empty;
    public static string CompanyName { get; set; } = string.Empty;

    public static string UserAppDataPath =>
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    public static string CommonAppDataPath =>
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

    public static string LocalUserAppDataPath =>
      Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    public static FormCollection OpenForms => _openForms;

    public static bool MessageLoop => true;

    public static void AddMessageFilter(IMessageFilter filter) { }
    public static void RemoveMessageFilter(IMessageFilter filter) { }

    public static string CurrentCulture { get; set; } =
      System.Globalization.CultureInfo.CurrentCulture.Name;

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public static void Run(Form mainForm) {
      // For Blazor, this is a no-op: the Blazor host takes care of running the app.
      // The form would be rendered as a Blazor component.
    }

    public static void Run() {
      // No-op for Blazor
    }

    public static void Exit() {
      ApplicationExit?.Invoke(null, EventArgs.Empty);
    }

    public static void ExitThread() {
      Exit();
    }

    public static void DoEvents() {
      // No-op: Blazor handles the message loop
    }

    public static void EnableVisualStyles() {
      // No-op: Blazor uses CSS styling
    }

    public static void SetCompatibleTextRenderingDefault(bool defaultValue) {
      // No-op for Blazor
    }

    public static void SetHighDpiMode(HighDpiMode highDpiMode) {
      // No-op: browser handles DPI
    }

    public static bool SetSuspendState(PowerState state, bool force, bool disableWakeEvent) {
      // No-op: browser cannot suspend the system
      return false;
    }

    public static Keys ModifierKeys => Keys.None;

    public static bool RenderWithVisualStyles => true;

    public static bool UseSystemColors { get; set; } = true;

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public static event EventHandler ApplicationExit;
    public static event EventHandler Idle;
    public static event ThreadExceptionEventHandler ThreadException;

    // ═══════════════════════════════════════════════
    // FormCollection
    // ═══════════════════════════════════════════════

    public class FormCollection : ReadOnlyCollection<Form> {

      private readonly List<Form> _forms;

      internal FormCollection() : base(new List<Form>()) {
        _forms = (List<Form>)Items;
      }

      internal void Add(Form form) {
        if (!_forms.Contains(form))
          _forms.Add(form);
      }

      internal void Remove(Form form) {
        _forms.Remove(form);
      }
    }
  }
}
