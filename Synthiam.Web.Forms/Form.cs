// =============================================================================
// Synthiam.Web.Forms - Form class for Blazor
// =============================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Synthiam.Web.Forms;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // IWin32Window
  // ---------------------------------------------------------------------------
  public interface IWin32Window {
    IntPtr Handle { get; }
  }

  // ---------------------------------------------------------------------------
  // Form
  // ---------------------------------------------------------------------------
  public partial class Form : ContainerControl, IWin32Window {

    // ═══════════════════════════════════════════════
    // Fields
    // ═══════════════════════════════════════════════
    private FormBorderStyle _formBorderStyle = FormBorderStyle.Sizable;
    private FormStartPosition _startPosition = FormStartPosition.WindowsDefaultLocation;
    private FormWindowState _windowState = FormWindowState.Normal;
    private bool _topMost;
    private bool _showInTaskbar = true;
    private bool _showIcon = true;
    private bool _maximizeBox = true;
    private bool _minimizeBox = true;
    private bool _controlBox = true;
    private bool _keyPreview;
    private double _opacity = 1.0;
    private Color _transparencyKey = Color.Empty;
    private MenuStrip _mainMenuStrip;
    private Icon _icon;
    private Size _formMaximumSize;
    private Size _formMinimumSize;
    private DialogResult _dialogResult = DialogResult.None;
    private Button _acceptButton;
    private Button _cancelButton;
    private Form _owner;
    private Form _mdiParent;
    private bool _isMdiContainer;
    private readonly List<Form> _ownedForms = new List<Form>();

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public FormBorderStyle FormBorderStyle {
      get => _formBorderStyle;
      set { _formBorderStyle = value; NotifyStateChanged(); }
    }

    public FormStartPosition StartPosition {
      get => _startPosition;
      set => _startPosition = value;
    }

    public FormWindowState WindowState {
      get => _windowState;
      set { _windowState = value; NotifyStateChanged(); }
    }

    public bool TopMost {
      get => _topMost;
      set => _topMost = value;
    }

    public bool ShowInTaskbar {
      get => _showInTaskbar;
      set => _showInTaskbar = value;
    }

    public bool ShowIcon {
      get => _showIcon;
      set => _showIcon = value;
    }

    public bool MaximizeBox {
      get => _maximizeBox;
      set => _maximizeBox = value;
    }

    public bool MinimizeBox {
      get => _minimizeBox;
      set => _minimizeBox = value;
    }

    public bool ControlBox {
      get => _controlBox;
      set => _controlBox = value;
    }

    public bool KeyPreview {
      get => _keyPreview;
      set => _keyPreview = value;
    }

    public double Opacity {
      get => _opacity;
      set {
        _opacity = Math.Max(0.0, Math.Min(1.0, value));
        NotifyStateChanged();
      }
    }

    public Color TransparencyKey {
      get => _transparencyKey;
      set => _transparencyKey = value;
    }

    public MenuStrip MainMenuStrip {
      get => _mainMenuStrip;
      set => _mainMenuStrip = value;
    }

    public Icon Icon {
      get => _icon;
      set => _icon = value;
    }

    public override Size MaximumSize {
      get => _formMaximumSize;
      set => _formMaximumSize = value;
    }

    public override Size MinimumSize {
      get => _formMinimumSize;
      set => _formMinimumSize = value;
    }

    public DialogResult DialogResult {
      get => _dialogResult;
      set {
        _dialogResult = value;
        // In WinForms, setting DialogResult on a modal form automatically closes it
        if (value != DialogResult.None && _modalStack.Contains(this)) {
          Close();
        }
      }
    }

    public Button AcceptButton {
      get => _acceptButton;
      set => _acceptButton = value;
    }

    public Button CancelButton {
      get => _cancelButton;
      set => _cancelButton = value;
    }

    public Form Owner {
      get => _owner;
      set {
        if (value == this)
          throw new ArgumentException("A form cannot own itself.");
        if (_owner != value) {
          _owner?._ownedForms.Remove(this);
          _owner = value;
          _owner?._ownedForms.Add(this);
        }
      }
    }

    public Form MdiParent {
      get => _mdiParent;
      set => _mdiParent = value;
    }

    public bool IsMdiContainer {
      get => _isMdiContainer;
      set => _isMdiContainer = value;
    }

    public Form[] OwnedForms => _ownedForms.ToArray();

    public Form ActiveMdiChild => null;

    public static Form ActiveForm { get; set; }

    public static Keys ModifierKeys => Application.ModifierKeys;

    public bool TopLevel { get; set; } = true;

    public SizeF AutoScaleBaseSize { get; set; }

    public bool HelpButton { get; set; }

    protected override CreateParams CreateParams => new CreateParams();

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event FormClosingEventHandler FormClosing;
    public event FormClosedEventHandler FormClosed;
    public event EventHandler Load;
    public event EventHandler Shown;
    public event EventHandler Activated;
    public event EventHandler Deactivate;
    public event System.ComponentModel.CancelEventHandler Closing;
    public event EventHandler ResizeEnd;
    public event System.ComponentModel.CancelEventHandler HelpButtonClicked;

    // ═══════════════════════════════════════════════
    // Virtual event raisers
    // ═══════════════════════════════════════════════

    protected virtual void OnFormClosing(FormClosingEventArgs e) {
      FormClosing?.Invoke(this, e);
    }

    protected virtual void OnFormClosed(FormClosedEventArgs e) {
      FormClosed?.Invoke(this, e);
    }

    protected virtual void OnLoad(EventArgs e) {
      Load?.Invoke(this, e);
    }

    /// <summary>Called by FormRenderer after first render to fire Load/Shown events.</summary>
    internal void InvokeOnLoad() {
      OnLoad(EventArgs.Empty);
      OnShown(EventArgs.Empty);
    }

    protected virtual void OnShown(EventArgs e) {
      Shown?.Invoke(this, e);
    }

    protected virtual void OnActivated(EventArgs e) {
      Activated?.Invoke(this, e);
    }

    protected virtual void OnDeactivate(EventArgs e) {
      Deactivate?.Invoke(this, e);
    }

    protected virtual void OnClosing(System.ComponentModel.CancelEventArgs e) {
      Closing?.Invoke(this, e);
    }

    protected new virtual void OnResizeEnd(EventArgs e) {
      base.OnResizeEnd(e);
    }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public void Close() {
      // Fire legacy Closing event first
      var legacyArgs = new System.ComponentModel.CancelEventArgs(false);
      OnClosing(legacyArgs);
      if (legacyArgs.Cancel) return;

      var closingArgs = new FormClosingEventArgs(CloseReason.UserClosing, false);
      OnFormClosing(closingArgs);
      if (!closingArgs.Cancel) {
        Visible = false;
        _modalStack.Remove(this);
        OnFormClosed(new FormClosedEventArgs(CloseReason.UserClosing));
        DialogCompleted?.Invoke(_dialogResult);
        NotifyStateChanged();
      }
    }

    /// <summary>
    /// Global stack of modal dialogs currently being displayed.
    /// The FormRenderer checks this to render modal overlays.
    /// </summary>
    internal static readonly List<Form> _modalStack = new List<Form>();

    /// <summary>
    /// Static reference to the root form's Blazor context.
    /// Set by FormRenderer so that all dialogs can inherit it.
    /// </summary>
    internal static Microsoft.AspNetCore.Components.IHandleEvent _globalBlazorReceiver;
    internal static Func<Action, System.Threading.Tasks.Task> _globalBlazorInvokeAsync;
    internal static Action _globalStateChanged;

    /// <summary>
    /// Event raised when the dialog is closed with a result (for async dialog pattern).
    /// </summary>
    public event Action<DialogResult> DialogCompleted;

    private void InheritBlazorContext() {
      if (_blazorReceiver == null)
        _blazorReceiver = _globalBlazorReceiver;
      if (_blazorInvokeAsync == null)
        _blazorInvokeAsync = _globalBlazorInvokeAsync;
      if (_stateChanged == null)
        _stateChanged = _globalStateChanged;
    }

    public DialogResult ShowDialog() {
      _modalStack.Add(this);
      InheritBlazorContext();
      Visible = true;
      OnLoad(EventArgs.Empty);
      OnShown(EventArgs.Empty);
      // Trigger re-render so the modal overlay appears
      NotifyStateChanged();
      // In Blazor we can't block - return None; actual result comes via DialogCompleted or Close()
      return _dialogResult;
    }

    public DialogResult ShowDialog(IWin32Window owner) {
      if (owner is Control ownerControl) {
        // Inherit Blazor context from the owner
        _blazorReceiver = ownerControl._blazorReceiver ?? (ownerControl.GetBlazorReceiver() as Microsoft.AspNetCore.Components.IHandleEvent);
        _blazorInvokeAsync = ownerControl._blazorInvokeAsync ?? ownerControl.GetBlazorInvokeAsync();
        _stateChanged = ownerControl._stateChanged;
        // Walk up if not found directly
        var p = ownerControl;
        while (p != null && _stateChanged == null) {
          _stateChanged = p._stateChanged;
          _blazorReceiver = _blazorReceiver ?? p._blazorReceiver;
          _blazorInvokeAsync = _blazorInvokeAsync ?? p._blazorInvokeAsync;
          p = p._parent;
        }
      }
      return ShowDialog();
    }

    public void Show() {
      InheritBlazorContext();
      Visible = true;
      OnLoad(EventArgs.Empty);
      OnShown(EventArgs.Empty);
      NotifyStateChanged();
    }

    public void Show(IWin32Window owner) {
      Show();
    }

    public void Activate() {
      OnActivated(EventArgs.Empty);
    }

    public void CenterToScreen() {
      // In a browser, positioning is handled differently.
      // Set start position for CSS handling.
      _startPosition = FormStartPosition.CenterScreen;
    }

    public void CenterToParent() {
      _startPosition = FormStartPosition.CenterParent;
    }

    // ═══════════════════════════════════════════════
    // HTML rendering
    // ═══════════════════════════════════════════════

    protected override string GetCssClasses() {
      var css = base.GetCssClasses() + " swf-form";
      if (_formBorderStyle == FormBorderStyle.None) css += " swf-form-borderless";
      if (_windowState == FormWindowState.Maximized) css += " swf-form-maximized";
      return css;
    }

    protected override string BuildCssStyle() {
      // For root forms (no parent), fill the container instead of using fixed pixel size
      if (Parent == null) {
        var sb = new System.Text.StringBuilder(256);
        sb.Append("position:relative;width:100%;height:100vh;overflow:hidden;");

        if (!BackColor.IsEmpty)
          sb.Append("background-color:").Append(BackColor.ToCss()).Append(';');
        if (!ForeColor.IsEmpty)
          sb.Append("color:").Append(ForeColor.ToCss()).Append(';');

        if (Font != null) {
          sb.Append("font:");
          if (Font.Italic) sb.Append("italic ");
          if (Font.Bold) sb.Append("bold ");
          sb.Append(Font.SizeInPoints.ToString("0.##", CultureInfo.InvariantCulture)).Append("pt");
          sb.Append(" '").Append(Font.Name).Append("';");
        }

        if (_opacity < 1.0)
          sb.Append("opacity:").Append(_opacity.ToString("0.##", CultureInfo.InvariantCulture)).Append(';');
        if (_topMost)
          sb.Append("z-index:9999;");
        return sb.ToString();
      }

      var style = base.BuildCssStyle();
      if (_opacity < 1.0)
        style += "opacity:" + _opacity.ToString("0.##", CultureInfo.InvariantCulture) + ";";
      if (_topMost)
        style += "z-index:9999;";
      return style;
    }
  }

  // ---------------------------------------------------------------------------
  // Forward declarations for types referenced by Form
  // (these will be fully implemented in their own files)
  // ---------------------------------------------------------------------------
  public partial class MenuStrip { }
  public partial class ContextMenuStrip { }
  public partial class Button { }
}
