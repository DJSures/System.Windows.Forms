// =============================================================================
// Synthiam.Web.Forms - ToolTip for Blazor
// =============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // ToolTipIcon enum
  // ---------------------------------------------------------------------------
  public enum ToolTipIcon {
    None = 0,
    Info = 1,
    Warning = 2,
    Error = 3
  }

  // ---------------------------------------------------------------------------
  // ToolTip
  // ---------------------------------------------------------------------------
  public class ToolTip : IComponent, IExtenderProvider, IDisposable {

    private readonly Dictionary<Control, string> _toolTips = new Dictionary<Control, string>();
    private bool _active = true;
    private int _automaticDelay = 500;
    private int _autoPopDelay = 5000;
    private int _initialDelay = 500;
    private int _reshowDelay = 100;
    private bool _showAlways;
    private bool _isBalloon;
    private ToolTipIcon _toolTipIcon = ToolTipIcon.None;
    private string _toolTipTitle = string.Empty;
    private Color _backColor = Color.Empty;
    private Color _foreColor = Color.Empty;
    private bool _ownerDraw;
    private bool _useAnimation = true;
    private bool _useFading = true;
    private bool _stripAmpersands;
    private object _tag;
    private bool _isDisposed;

    public ToolTip() { }
    public ToolTip(IContainer container) { container?.Add(this); }

    // IComponent
    public event EventHandler Disposed;
    public ISite Site { get; set; }

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public bool Active {
      get => _active;
      set => _active = value;
    }

    public int AutomaticDelay {
      get => _automaticDelay;
      set {
        _automaticDelay = value;
        _autoPopDelay = value * 10;
        _initialDelay = value;
        _reshowDelay = value / 5;
      }
    }

    public int AutoPopDelay {
      get => _autoPopDelay;
      set => _autoPopDelay = value;
    }

    public int InitialDelay {
      get => _initialDelay;
      set => _initialDelay = value;
    }

    public int ReshowDelay {
      get => _reshowDelay;
      set => _reshowDelay = value;
    }

    public bool ShowAlways {
      get => _showAlways;
      set => _showAlways = value;
    }

    public bool IsBalloon {
      get => _isBalloon;
      set => _isBalloon = value;
    }

    public ToolTipIcon ToolTipIcon {
      get => _toolTipIcon;
      set => _toolTipIcon = value;
    }

    public string ToolTipTitle {
      get => _toolTipTitle;
      set => _toolTipTitle = value ?? string.Empty;
    }

    public Color BackColor {
      get => _backColor;
      set => _backColor = value;
    }

    public Color ForeColor {
      get => _foreColor;
      set => _foreColor = value;
    }

    public bool OwnerDraw {
      get => _ownerDraw;
      set => _ownerDraw = value;
    }

    public bool UseAnimation {
      get => _useAnimation;
      set => _useAnimation = value;
    }

    public bool UseFading {
      get => _useFading;
      set => _useFading = value;
    }

    public bool StripAmpersands {
      get => _stripAmpersands;
      set => _stripAmpersands = value;
    }

    public object Tag {
      get => _tag;
      set => _tag = value;
    }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event DrawToolTipEventHandler Draw;
    public event PopupEventHandler Popup;

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public void SetToolTip(Control control, string caption) {
      if (control == null) return;
      if (string.IsNullOrEmpty(caption)) {
        _toolTips.Remove(control);
        control._toolTipText = null;
      } else {
        _toolTips[control] = caption;
        control._toolTipText = _stripAmpersands ? caption.Replace("&", "") : caption;
      }
      control.NotifyStateChanged();
    }

    public string GetToolTip(Control control) {
      if (control != null && _toolTips.TryGetValue(control, out var tip))
        return tip;
      return string.Empty;
    }

    public void RemoveAll() {
      foreach (var kvp in _toolTips) {
        kvp.Key._toolTipText = null;
        kvp.Key.NotifyStateChanged();
      }
      _toolTips.Clear();
    }

    public void Show(string text, IWin32Window window) {
      // Stub: in web, the browser handles tooltip display via title attribute
    }

    public void Show(string text, IWin32Window window, int duration) {
      // Stub
    }

    public void Show(string text, IWin32Window window, Point point) {
      // Stub
    }

    public void Hide(IWin32Window window) {
      // Stub
    }

    // IExtenderProvider
    public bool CanExtend(object extendee) {
      return extendee is Control;
    }

    public void Dispose() {
      Dispose(true);
    }

    protected virtual void Dispose(bool disposing) {
      if (!_isDisposed) {
        _isDisposed = true;
        if (disposing) {
          RemoveAll();
        }
        Disposed?.Invoke(this, EventArgs.Empty);
      }
    }
  }

  // ToolTip event args and delegates
  public class DrawToolTipEventArgs : EventArgs {
    public Graphics Graphics { get; }
    public IWin32Window AssociatedWindow { get; }
    public Control AssociatedControl { get; }
    public Rectangle Bounds { get; }
    public string ToolTipText { get; }
    public Font Font { get; }

    public DrawToolTipEventArgs(Graphics graphics, IWin32Window associatedWindow, Control associatedControl,
      Rectangle bounds, string toolTipText, Color backColor, Color foreColor, Font font) {
      Graphics = graphics;
      AssociatedWindow = associatedWindow;
      AssociatedControl = associatedControl;
      Bounds = bounds;
      ToolTipText = toolTipText;
      Font = font;
    }

    public void DrawBackground() { }
    public void DrawText() { }
    public void DrawBorder() { }
  }

  public class PopupEventArgs : System.ComponentModel.CancelEventArgs {
    public Control AssociatedControl { get; }
    public IWin32Window AssociatedWindow { get; }
    public bool IsBalloon { get; }
    public Size ToolTipSize { get; set; }

    public PopupEventArgs(IWin32Window associatedWindow, Control associatedControl, bool isBalloon, Size size) {
      AssociatedWindow = associatedWindow;
      AssociatedControl = associatedControl;
      IsBalloon = isBalloon;
      ToolTipSize = size;
    }
  }

  public delegate void DrawToolTipEventHandler(object sender, DrawToolTipEventArgs e);
  public delegate void PopupEventHandler(object sender, PopupEventArgs e);
}
