// =============================================================================
// Synthiam.Web.Forms - CommonDialog base class for Blazor
// =============================================================================

using System;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // CommonDialog
  // ---------------------------------------------------------------------------
  public abstract class CommonDialog : System.ComponentModel.IComponent, IDisposable {

    private bool _isDisposed;

    public event EventHandler HelpRequest;
    public event EventHandler Disposed;
    public System.ComponentModel.ISite Site { get; set; }
    public object Tag { get; set; }

    public DialogResult ShowDialog() {
      return RunDialog(IntPtr.Zero) ? DialogResult.OK : DialogResult.Cancel;
    }

    public DialogResult ShowDialog(IWin32Window owner) {
      return RunDialog(owner?.Handle ?? IntPtr.Zero) ? DialogResult.OK : DialogResult.Cancel;
    }

    protected abstract bool RunDialog(IntPtr hwndOwner);

    public abstract void Reset();

    protected virtual void OnHelpRequest(EventArgs e) {
      HelpRequest?.Invoke(this, e);
    }

    public void Dispose() {
      if (!_isDisposed) {
        _isDisposed = true;
        Disposed?.Invoke(this, EventArgs.Empty);
      }
    }
  }
}
