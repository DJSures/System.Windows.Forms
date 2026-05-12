// =============================================================================
// Synthiam.Web.Forms - Control base class for Blazor
// Translates WinForms controls to HTML via Blazor RenderTreeBuilder.
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Synthiam.Web.Forms;
using BlazorMouseEventArgs = Microsoft.AspNetCore.Components.Web.MouseEventArgs;
using BlazorKeyboardEventArgs = Microsoft.AspNetCore.Components.Web.KeyboardEventArgs;
using BlazorFocusEventArgs = Microsoft.AspNetCore.Components.Web.FocusEventArgs;
using BlazorWheelEventArgs = Microsoft.AspNetCore.Components.Web.WheelEventArgs;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // Message struct (for PreProcessMessage)
  // ---------------------------------------------------------------------------
  public struct Message {
    public IntPtr HWnd { get; set; }
    public int Msg { get; set; }
    public IntPtr WParam { get; set; }
    public IntPtr LParam { get; set; }
    public IntPtr Result { get; set; }
  }

  // ---------------------------------------------------------------------------
  // ControlEventArgs / delegates
  // ---------------------------------------------------------------------------
  public class ControlEventArgs : EventArgs {
    public Control Control { get; }
    public ControlEventArgs(Control control) { Control = control; }
  }

  public delegate void ControlEventHandler(object sender, ControlEventArgs e);
  public delegate void LayoutEventHandler(object sender, LayoutEventArgs e);
  public delegate void GiveFeedbackEventHandler(object sender, GiveFeedbackEventArgs e);
  public delegate void QueryContinueDragEventHandler(object sender, QueryContinueDragEventArgs e);

  // ---------------------------------------------------------------------------
  // AccessibleRole enum
  // ---------------------------------------------------------------------------
  public enum AccessibleRole {
    Default = -1, None = 0, TitleBar = 1, MenuBar = 2, ScrollBar = 3, Grip = 4,
    Sound = 5, Cursor = 6, Caret = 7, Alert = 8, Window = 9, Client = 10,
    MenuPopup = 11, MenuItem = 12, ToolTip = 13, Application = 14, Document = 15,
    Pane = 16, Chart = 17, Dialog = 18, Border = 19, Grouping = 20, Separator = 21,
    ToolBar = 22, StatusBar = 23, Table = 24, ColumnHeader = 25, RowHeader = 26,
    Column = 27, Row = 28, Cell = 29, Link = 30, HelpBalloon = 31, Character = 32,
    List = 33, ListItem = 34, Outline = 35, OutlineItem = 36, PageTab = 37,
    PropertyPage = 38, Indicator = 39, Graphic = 40, StaticText = 41, Text = 42,
    PushButton = 43, CheckButton = 44, RadioButton = 45, ComboBox = 46,
    DropList = 47, ProgressBar = 48, Dial = 49, HotkeyField = 50, Slider = 51,
    SpinButton = 52, Diagram = 53, Animation = 54, Equation = 55, ButtonDropDown = 56,
    ButtonMenu = 57, ButtonDropDownGrid = 58, WhiteSpace = 59, PageTabList = 60,
    Clock = 61
  }

  // ---------------------------------------------------------------------------
  // Control (partial class extending ForwardDeclarations.cs stub)
  // ---------------------------------------------------------------------------
  public partial class Control : System.ComponentModel.Component, IWin32Window, System.ComponentModel.ISupportInitialize {

    // ═══════════════════════════════════════════════
    // Fields
    // ═══════════════════════════════════════════════
    private string _text = string.Empty;
    private string _name = string.Empty;
    private object _tag;

    private Point _location;
    private Size _size = new Size(100, 23);
    private Size _minimumSize;
    private Size _maximumSize;

    private DockStyle _dock = DockStyle.None;
    private AnchorStyles _anchor = AnchorStyles.Top | AnchorStyles.Left;
    private Padding _padding;
    private Padding _margin;

    private Color _backColor = Color.Empty;
    private Color _foreColor = Color.Empty;
    private Font _font = new Font("Microsoft Sans Serif", 8.25f);

    private bool _enabled = true;
    private bool _visible = true;
    private int _tabIndex;
    private bool _tabStop = true;
    private Cursor _cursor;

    private bool _autoSize;
    private int _zIndex;
    private static int _zIndexCounter;
    private AutoScaleMode _autoScaleMode = AutoScaleMode.Font;
    private SizeF _autoScaleDimensions = new SizeF(6F, 13F);

    private Image _backgroundImage;
    private ImageLayout _backgroundImageLayout = ImageLayout.Tile;
    private RightToLeft _rightToLeft = RightToLeft.No;

    private bool _isDisposed;
    internal Control _parent;
    private ControlCollection _controls;
    private ControlStyles _controlStyles;

    /// <summary>
    /// When true, this container manages child layout via flex/grid.
    /// Children should not use absolute positioning; they fill their
    /// parent-provided container slot instead.
    /// </summary>
    internal virtual bool ManagesChildLayout => false;

    private bool _capture;
    private bool _allowDrop;
    private bool _useWaitCursor;
    private ContextMenuStrip _contextMenuStrip;

    private string _accessibleName;
    private string _accessibleDescription;
    private AccessibleRole _accessibleRole = AccessibleRole.Default;

    private bool _focused;

    // Blazor integration fields
    internal string _toolTipText;
    internal Action _stateChanged;

    /// <summary>
    /// Reference to the hosting Blazor component (FormRenderer).
    /// Used as the receiver for EventCallback to ensure proper Blazor event dispatching.
    /// </summary>
    internal Microsoft.AspNetCore.Components.IHandleEvent _blazorReceiver;

    /// <summary>
    /// Func to invoke an action on the Blazor render thread.
    /// Set by FormRenderer on the root form; child controls walk up the parent chain.
    /// Signature: Func&lt;Action, Task&gt; matching ComponentBase.InvokeAsync(Action).
    /// </summary>
    internal Func<Action, System.Threading.Tasks.Task> _blazorInvokeAsync;
    private bool _layoutSuspended;

    // ═══════════════════════════════════════════════
    // Cached render strings – avoids rebuilding on
    // every Blazor render and keeps the same object
    // reference so Blazor's diff sees "no change".
    // ═══════════════════════════════════════════════
    internal string _cachedCssStyle;
    internal string _cachedCssClasses;

    /// <summary>
    /// Marks the cached CSS style string as stale so it is rebuilt on the next render.
    /// Call this whenever a property that affects the inline style changes.
    /// </summary>
    internal void InvalidateCssStyle() {
      _cachedCssStyle = null;
    }

    /// <summary>
    /// Marks the cached CSS class string as stale so it is rebuilt on the next render.
    /// </summary>
    internal void InvalidateCssClasses() {
      _cachedCssClasses = null;
    }

    // ═══════════════════════════════════════════════
    // Constructor
    // ═══════════════════════════════════════════════
    public Control() {
      _controls = new ControlCollection(this);
    }

    public Control(string text) : this() {
      _text = text ?? string.Empty;
    }

    public Control(string text, int left, int top, int width, int height) : this(text) {
      _location = new Point(left, top);
      _size = new Size(width, height);
    }

    public Control(Control parent, string text) : this(text) {
      parent?.Controls.Add(this);
    }

    public Control(Control parent, string text, int left, int top, int width, int height) : this(text, left, top, width, height) {
      parent?.Controls.Add(this);
    }

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public virtual string Text {
      get => _text;
      set {
        if (_text != value) {
          _text = value ?? string.Empty;
          OnTextChanged(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public string Name {
      get => _name;
      set => _name = value ?? string.Empty;
    }

    public object Tag {
      get => _tag;
      set => _tag = value;
    }

    public Point Location {
      get => _location;
      set {
        if (_location != value) {
          _location = value;
          InvalidateCssStyle();
          OnLocationChanged(EventArgs.Empty);
          OnMove(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public Size Size {
      get => _size;
      set {
        if (_size != value) {
          _size = value;
          InvalidateCssStyle();
          OnSizeChanged(EventArgs.Empty);
          OnResize(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public virtual Size MinimumSize {
      get => _minimumSize;
      set => _minimumSize = value;
    }

    public virtual Size MaximumSize {
      get => _maximumSize;
      set => _maximumSize = value;
    }

    public int Width {
      get => _size.Width;
      set => Size = new Size(value, _size.Height);
    }

    public int Height {
      get => _size.Height;
      set => Size = new Size(_size.Width, value);
    }

    public int Left {
      get => _location.X;
      set => Location = new Point(value, _location.Y);
    }

    public int Top {
      get => _location.Y;
      set => Location = new Point(_location.X, value);
    }

    public int Right => Left + Width;

    public int Bottom => Top + Height;

    public Rectangle Bounds {
      get => new Rectangle(_location, _size);
      set {
        Location = value.Location;
        Size = value.Size;
      }
    }

    public Size ClientSize {
      get => _size;
      set => Size = value;
    }

    public Rectangle ClientRectangle => new Rectangle(0, 0, _size.Width, _size.Height);

    public virtual Rectangle DisplayRectangle => ClientRectangle;

    public virtual DockStyle Dock {
      get => _dock;
      set {
        if (_dock != value) {
          _dock = value;
          InvalidateCssStyle();
          OnDockChanged(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public AnchorStyles Anchor {
      get => _anchor;
      set {
        if (_anchor != value) {
          _anchor = value;
          NotifyStateChanged();
        }
      }
    }

    public Padding Padding {
      get => _padding;
      set {
        if (_padding != value) {
          _padding = value;
          InvalidateCssStyle();
          NotifyStateChanged();
        }
      }
    }

    public Padding Margin {
      get => _margin;
      set {
        if (_margin != value) {
          _margin = value;
          InvalidateCssStyle();
          NotifyStateChanged();
        }
      }
    }

    public virtual Color BackColor {
      get => _backColor;
      set {
        if (_backColor != value) {
          _backColor = value;
          InvalidateCssStyle();
          OnBackColorChanged(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public virtual Color ForeColor {
      get => _foreColor;
      set {
        if (_foreColor != value) {
          _foreColor = value;
          InvalidateCssStyle();
          OnForeColorChanged(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public virtual Font Font {
      get => _font;
      set {
        if (_font != value) {
          _font = value;
          InvalidateCssStyle();
          OnFontChanged(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public bool Enabled {
      get => _enabled;
      set {
        if (_enabled != value) {
          _enabled = value;
          OnEnabledChanged(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public bool Visible {
      get => _visible;
      set {
        if (_visible != value) {
          _visible = value;
          OnVisibleChanged(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public int TabIndex {
      get => _tabIndex;
      set => _tabIndex = value;
    }

    public bool TabStop {
      get => _tabStop;
      set => _tabStop = value;
    }

    public virtual Cursor Cursor {
      get => _cursor;
      set {
        if (_cursor != value) {
          _cursor = value;
          InvalidateCssStyle();
          NotifyStateChanged();
        }
      }
    }

    public bool AutoSize {
      get => _autoSize;
      set => _autoSize = value;
    }

    public AutoScaleMode AutoScaleMode {
      get => _autoScaleMode;
      set => _autoScaleMode = value;
    }

    public SizeF AutoScaleDimensions {
      get => _autoScaleDimensions;
      set => _autoScaleDimensions = value;
    }

    public virtual Image BackgroundImage {
      get => _backgroundImage;
      set {
        if (_backgroundImage != value) {
          _backgroundImage = value;
          InvalidateCssStyle();
          NotifyStateChanged();
        }
      }
    }

    public ImageLayout BackgroundImageLayout {
      get => _backgroundImageLayout;
      set {
        if (_backgroundImageLayout != value) {
          _backgroundImageLayout = value;
          InvalidateCssStyle();
          NotifyStateChanged();
        }
      }
    }

    public virtual RightToLeft RightToLeft {
      get => _rightToLeft;
      set => _rightToLeft = value;
    }

    public bool IsDisposed => _isDisposed;

    public bool Disposing { get; protected set; }

    public AutoSizeMode AutoSizeMode { get; set; }

    public AutoValidate AutoValidate { get; set; }

    protected virtual CreateParams CreateParams => new CreateParams();

    public Control Parent {
      get => _parent;
      set {
        if (_parent != value) {
          _parent?.Controls.Remove(this);
          value?.Controls.Add(this);
        }
      }
    }

    public ControlCollection Controls => _controls;

    public IntPtr Handle => IntPtr.Zero;

    public System.ComponentModel.ISite Site { get; set; }

    /// <summary>
    /// In Blazor, always returns false so that callers execute directly on the current context.
    /// The Blazor event pipeline already dispatches onto the correct sync context.
    /// Background threads should use BeginInvoke which routes through InvokeAsync.
    /// </summary>
    public bool InvokeRequired => false;

    public bool IsHandleCreated => true;

    protected virtual bool DoubleBuffered { get; set; }

    public bool Capture {
      get => _capture;
      set => _capture = value;
    }

    public bool AllowDrop {
      get => _allowDrop;
      set => _allowDrop = value;
    }

    public bool UseWaitCursor {
      get => _useWaitCursor;
      set {
        _useWaitCursor = value;
        NotifyStateChanged();
      }
    }

    public ContextMenuStrip ContextMenuStrip {
      get => _contextMenuStrip;
      set => _contextMenuStrip = value;
    }

    public ContextMenu ContextMenu { get; set; }

    public string AccessibleName {
      get => _accessibleName;
      set => _accessibleName = value;
    }

    public string AccessibleDescription {
      get => _accessibleDescription;
      set => _accessibleDescription = value;
    }

    public AccessibleRole AccessibleRole {
      get => _accessibleRole;
      set => _accessibleRole = value;
    }

    public ImeMode ImeMode { get; set; } = ImeMode.NoControl;

    public bool UseCompatibleTextRendering { get; set; }

    public static Color DefaultBackColor => Color.FromArgb(255, 240, 240, 240);
    public static Color DefaultForeColor => Color.Black;

    public System.Drawing.Region Region { get; set; }

    public ScrollableControl.DockPaddingEdges DockPadding => new ScrollableControl.DockPaddingEdges();

    public bool CanFocus => _visible && _enabled;

    public bool CanSelect => _visible && _enabled;

    public bool Focused => _focused;

    public bool ContainsFocus {
      get {
        if (_focused) return true;
        foreach (Control c in _controls)
          if (c.ContainsFocus) return true;
        return false;
      }
    }

    public Control TopLevelControl {
      get {
        Control c = this;
        while (c._parent != null)
          c = c._parent;
        return c;
      }
    }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event EventHandler Click;
    public event EventHandler DoubleClick;

    public event MouseEventHandler MouseDown;
    public event MouseEventHandler MouseUp;
    public event MouseEventHandler MouseMove;
    public event EventHandler MouseEnter;
    public event EventHandler MouseLeave;
    public event EventHandler MouseHover;
    public event MouseEventHandler MouseClick;
    public event MouseEventHandler MouseDoubleClick;
    public event MouseEventHandler MouseWheel;

    public event KeyEventHandler KeyDown;
    public event KeyEventHandler KeyUp;
    public event KeyPressEventHandler KeyPress;
    public event PreviewKeyDownEventHandler PreviewKeyDown;

    public event EventHandler GotFocus;
    public event EventHandler LostFocus;
    public event EventHandler Enter;
    public event EventHandler Leave;
    public event EventHandler Validated;
    public event System.ComponentModel.CancelEventHandler Validating;

    public event PaintEventHandler Paint;

    public event EventHandler Resize;
    public event EventHandler SizeChanged;
    public event EventHandler LocationChanged;
    public event EventHandler Move;

    public event LayoutEventHandler Layout;

    public event EventHandler TextChanged;
    public event EventHandler EnabledChanged;
    public event EventHandler VisibleChanged;
    public event EventHandler FontChanged;
    public event EventHandler ForeColorChanged;
    public event EventHandler BackColorChanged;

    public event EventHandler ParentChanged;
    public event EventHandler DockChanged;

    public event DragEventHandler DragEnter;
    public event DragEventHandler DragOver;
    public event DragEventHandler DragDrop;
    public event EventHandler DragLeave;

    public event GiveFeedbackEventHandler GiveFeedback;
    public event QueryContinueDragEventHandler QueryContinueDrag;

    public event EventHandler HandleCreated;
    public event EventHandler HandleDestroyed;
    public event EventHandler Disposed;

    public event ControlEventHandler ControlAdded;
    public event ControlEventHandler ControlRemoved;

    // ═══════════════════════════════════════════════
    // Virtual event raisers
    // ═══════════════════════════════════════════════

    protected virtual void OnClick(EventArgs e) { Click?.Invoke(this, e); }
    protected virtual void OnDoubleClick(EventArgs e) { DoubleClick?.Invoke(this, e); }

    protected virtual void OnMouseDown(MouseEventArgs e) { MouseDown?.Invoke(this, e); }
    protected virtual void OnMouseUp(MouseEventArgs e) { MouseUp?.Invoke(this, e); }
    protected virtual void OnMouseMove(MouseEventArgs e) { MouseMove?.Invoke(this, e); }
    protected virtual void OnMouseEnter(EventArgs e) { MouseEnter?.Invoke(this, e); }
    protected virtual void OnMouseLeave(EventArgs e) { MouseLeave?.Invoke(this, e); }
    protected virtual void OnMouseHover(EventArgs e) { MouseHover?.Invoke(this, e); }
    protected virtual void OnMouseWheel(MouseEventArgs e) { MouseWheel?.Invoke(this, e); }
    protected virtual void OnMouseClick(MouseEventArgs e) { MouseClick?.Invoke(this, e); }
    protected virtual void OnMouseDoubleClick(MouseEventArgs e) { MouseDoubleClick?.Invoke(this, e); }

    protected virtual void OnKeyDown(KeyEventArgs e) { KeyDown?.Invoke(this, e); }
    protected virtual void OnKeyUp(KeyEventArgs e) { KeyUp?.Invoke(this, e); }
    protected virtual void OnKeyPress(KeyPressEventArgs e) { KeyPress?.Invoke(this, e); }

    protected virtual void OnPaint(PaintEventArgs e) { Paint?.Invoke(this, e); }

    protected virtual void OnResize(EventArgs e) { Resize?.Invoke(this, e); }
    protected virtual void OnSizeChanged(EventArgs e) { SizeChanged?.Invoke(this, e); }
    protected virtual void OnLocationChanged(EventArgs e) { LocationChanged?.Invoke(this, e); }
    protected virtual void OnMove(EventArgs e) { Move?.Invoke(this, e); }

    protected virtual void OnLayout(LayoutEventArgs e) { Layout?.Invoke(this, e); }

    protected virtual void OnTextChanged(EventArgs e) { TextChanged?.Invoke(this, e); }
    protected virtual void OnEnabledChanged(EventArgs e) { EnabledChanged?.Invoke(this, e); }
    protected virtual void OnVisibleChanged(EventArgs e) { VisibleChanged?.Invoke(this, e); }
    protected virtual void OnFontChanged(EventArgs e) { FontChanged?.Invoke(this, e); }
    protected virtual void OnForeColorChanged(EventArgs e) { ForeColorChanged?.Invoke(this, e); }
    protected virtual void OnBackColorChanged(EventArgs e) { BackColorChanged?.Invoke(this, e); }

    protected virtual void OnParentChanged(EventArgs e) { ParentChanged?.Invoke(this, e); }
    protected virtual void OnDockChanged(EventArgs e) { DockChanged?.Invoke(this, e); }

    protected virtual void OnGotFocus(EventArgs e) { GotFocus?.Invoke(this, e); }
    protected virtual void OnLostFocus(EventArgs e) { LostFocus?.Invoke(this, e); }

    protected virtual void OnDragEnter(DragEventArgs e) { DragEnter?.Invoke(this, e); }
    protected virtual void OnDragOver(DragEventArgs e) { DragOver?.Invoke(this, e); }
    protected virtual void OnDragDrop(DragEventArgs e) { DragDrop?.Invoke(this, e); }
    protected virtual void OnDragLeave(EventArgs e) { DragLeave?.Invoke(this, e); }

    protected virtual void OnHandleCreated(EventArgs e) { HandleCreated?.Invoke(this, e); }
    protected virtual void OnHandleDestroyed(EventArgs e) { HandleDestroyed?.Invoke(this, e); }

    protected virtual void OnPaintBackground(PaintEventArgs e) { }
    protected virtual void OnInvalidated(InvalidateEventArgs e) { }
    protected virtual void OnParentBackColorChanged(EventArgs e) { }
    protected virtual void OnResizeEnd(EventArgs e) { }
    protected virtual void OnImeModeChanged(EventArgs e) { }
    protected virtual void OnNotifyMessage(Message m) { }

    protected internal virtual void OnControlAdded(ControlEventArgs e) { ControlAdded?.Invoke(this, e); }
    protected internal virtual void OnControlRemoved(ControlEventArgs e) { ControlRemoved?.Invoke(this, e); }

    // ═══════════════════════════════════════════════
    // Layout methods
    // ═══════════════════════════════════════════════

    public void SuspendLayout() { _layoutSuspended = true; }

    public void ResumeLayout() { ResumeLayout(true); }

    public void ResumeLayout(bool performLayout) {
      _layoutSuspended = false;
      if (performLayout)
        PerformLayout();
    }

    public void PerformLayout() {
      if (!_layoutSuspended)
        OnLayout(new LayoutEventArgs(this, null));
    }

    // ═══════════════════════════════════════════════
    // Invalidation / painting
    // ═══════════════════════════════════════════════

    public void Invalidate() { NotifyStateChanged(); }
    public void Invalidate(Rectangle rc) { NotifyStateChanged(); }
    public void Invalidate(bool invalidateChildren) { NotifyStateChanged(); }
    public void Update() { NotifyStateChanged(); }

    protected void UpdateStyles() { }
    public virtual void Refresh() { Invalidate(); Update(); }

    // ═══════════════════════════════════════════════
    // Visibility / ordering
    // ═══════════════════════════════════════════════

    public void Show() { Visible = true; }
    public void Hide() { Visible = false; }

    public void BringToFront() {
      if (_parent != null) {
        // Use z-index instead of DOM reordering to avoid expensive Blazor re-diff
        _zIndex = System.Threading.Interlocked.Increment(ref _zIndexCounter);
        InvalidateCssStyle();
        NotifyStateChanged();
      }
    }

    public void SendToBack() {
      if (_parent != null) {
        _zIndex = 0;
        InvalidateCssStyle();
        NotifyStateChanged();
      }
    }

    // ═══════════════════════════════════════════════
    // Focus / selection
    // ═══════════════════════════════════════════════

    public bool Focus() {
      if (CanFocus) {
        _focused = true;
        OnGotFocus(EventArgs.Empty);
        OnEnter(EventArgs.Empty);
        return true;
      }
      return false;
    }

    public void Select() { Focus(); }
    public void Select(bool directed, bool forward) { Focus(); }

    protected virtual void OnEnter(EventArgs e) { Enter?.Invoke(this, e); }
    protected virtual void OnLeave(EventArgs e) { Leave?.Invoke(this, e); }

    // ═══════════════════════════════════════════════
    // Invoke (threading compatibility)
    // ═══════════════════════════════════════════════

    public object Invoke(Delegate method) {
      return method?.DynamicInvoke();
    }

    public object Invoke(Delegate method, params object[] args) {
      return method?.DynamicInvoke(args);
    }

    public void Invoke(Action action) {
      action?.Invoke();
    }

    public IAsyncResult BeginInvoke(Delegate method) {
      var invokeAsync = GetBlazorInvokeAsync();
      if (invokeAsync != null)
        return invokeAsync(() => method?.DynamicInvoke());
      method?.DynamicInvoke();
      return System.Threading.Tasks.Task.CompletedTask;
    }

    public IAsyncResult BeginInvoke(Delegate method, params object[] args) {
      var invokeAsync = GetBlazorInvokeAsync();
      if (invokeAsync != null)
        return invokeAsync(() => method?.DynamicInvoke(args));
      method?.DynamicInvoke(args);
      return System.Threading.Tasks.Task.CompletedTask;
    }

    public void BeginInvoke(Action action) {
      var invokeAsync = GetBlazorInvokeAsync();
      if (invokeAsync != null) {
        _ = invokeAsync(action);
        return;
      }
      action?.Invoke();
    }

    // ═══════════════════════════════════════════════
    // Coordinate translation
    // ═══════════════════════════════════════════════

    public Point PointToScreen(Point p) {
      int x = p.X, y = p.Y;
      Control c = this;
      while (c != null) { x += c.Left; y += c.Top; c = c._parent; }
      return new Point(x, y);
    }

    public Point PointToClient(Point p) {
      var screen = PointToScreen(Point.Empty);
      return new Point(p.X - screen.X, p.Y - screen.Y);
    }

    public Rectangle RectangleToScreen(Rectangle r) {
      var pt = PointToScreen(r.Location);
      return new Rectangle(pt, r.Size);
    }

    public Rectangle RectangleToClient(Rectangle r) {
      var pt = PointToClient(r.Location);
      return new Rectangle(pt, r.Size);
    }

    // ═══════════════════════════════════════════════
    // Misc methods
    // ═══════════════════════════════════════════════

    public Graphics CreateGraphics() {
      return Graphics.FromHwnd(IntPtr.Zero);
    }

    public Form FindForm() {
      Control c = this;
      while (c != null) {
        if (c is Form f) return f;
        c = c._parent;
      }
      return null;
    }

    public Control GetChildAtPoint(Point pt) {
      for (int i = _controls.Count - 1; i >= 0; i--) {
        var child = _controls[i];
        if (child.Visible && child.Bounds.Contains(pt))
          return child;
      }
      return null;
    }

    public bool Contains(Control ctl) {
      Control c = ctl;
      while (c != null) {
        if (c == this) return true;
        c = c._parent;
      }
      return false;
    }

    protected void SetStyle(ControlStyles flag, bool value) {
      if (value)
        _controlStyles |= flag;
      else
        _controlStyles &= ~flag;
    }

    protected bool GetStyle(ControlStyles flag) {
      return (_controlStyles & flag) == flag;
    }

    public void SetBounds(int x, int y, int width, int height) {
      Location = new Point(x, y);
      Size = new Size(width, height);
    }

    public DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects) {
      return DragDropEffects.None;
    }

    public virtual void Scale(SizeF factor) {
      _location = new Point((int)(_location.X * factor.Width), (int)(_location.Y * factor.Height));
      _size = new Size((int)(_size.Width * factor.Width), (int)(_size.Height * factor.Height));
      foreach (Control c in _controls)
        c.Scale(factor);
    }

    public virtual void CreateControl() { OnCreateControl(); }
    protected virtual void OnCreateControl() { }

    public virtual bool PreProcessMessage(ref Message msg) { return false; }

    protected virtual bool ProcessDialogKey(Keys keyData) { return false; }

    public virtual void ResetBackColor() { BackColor = Color.Empty; }

    protected virtual void WndProc(ref Message m) { }

    public virtual void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds) {
      // Stub: no-op in Blazor - real rendering is done by the browser
    }

    protected virtual bool IsInputKey(Keys keyData) { return false; }

    protected virtual bool IsInputChar(char charCode) { return false; }

    private bool _handleCreated;

    protected virtual void CreateHandle() { }

    /// <summary>
    /// Ensures CreateHandle() has been called exactly once.
    /// Called automatically before first render and when added to a parent.
    /// </summary>
    internal void EnsureHandleCreated() {
      if (!_handleCreated) {
        _handleCreated = true;
        _layoutSuspended = true;
        try {
          CreateHandle();
        } finally {
          _layoutSuspended = false;
        }
      }
    }

    // ═══════════════════════════════════════════════
    // ISupportInitialize
    // ═══════════════════════════════════════════════

    public virtual void BeginInit() { }
    public virtual void EndInit() { }

    // IDisposable
    // ═══════════════════════════════════════════════

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
      if (!_isDisposed) {
        Disposing = true;
        _isDisposed = true;
        if (disposing) {
          Disposed?.Invoke(this, EventArgs.Empty);
          foreach (Control c in _controls)
            c.Dispose();
        }
      }
    }

    // ═══════════════════════════════════════════════
    // Blazor state notification
    // ═══════════════════════════════════════════════

    internal void NotifyStateChanged() {
      // Skip during layout suspension or before first render
      if (_layoutSuspended)
        return;
      if (!_handleCreated && _stateChanged == null)
        return;

      // Walk up - skip if any ancestor is suspended
      var p = _parent;
      while (p != null) {
        if (p._layoutSuspended)
          return;
        if (p._stateChanged != null) {
          p._stateChanged.Invoke();
          return;
        }
        p = p._parent;
      }
      // Direct callback on this control
      _stateChanged?.Invoke();
    }

    /// <summary>
    /// Gets the Blazor component receiver for EventCallback.
    /// Walks up the parent chain to find the root form's _blazorReceiver.
    /// Falls back to 'this' if no receiver is found (events still fire but re-render may not trigger).
    /// </summary>
    internal object GetBlazorReceiver() {
      if (_blazorReceiver != null)
        return _blazorReceiver;
      if (_parent != null)
        return _parent.GetBlazorReceiver();
      return this;
    }

    /// <summary>
    /// Gets the Blazor InvokeAsync delegate, walking up the parent chain to find it on the root form.
    /// </summary>
    internal Func<Action, System.Threading.Tasks.Task> GetBlazorInvokeAsync() {
      if (_blazorInvokeAsync != null)
        return _blazorInvokeAsync;
      if (_parent != null)
        return _parent.GetBlazorInvokeAsync();
      return null;
    }

    // ═══════════════════════════════════════════════
    // HTML / Blazor rendering
    // ═══════════════════════════════════════════════

    internal virtual void BuildRenderTree(RenderTreeBuilder builder, ref int seq) {
      if (!_visible) return;

      EnsureHandleCreated();

      builder.OpenElement(seq++, GetHtmlTag());
      builder.SetKey(this);

      // id attribute
      if (!string.IsNullOrEmpty(_name))
        builder.AddAttribute(seq++, "id", string.Concat("swf-", _name));

      // CSS class – use cached string so Blazor's diff sees the same reference
      var css = _cachedCssClasses ?? (_cachedCssClasses = GetCssClasses());
      builder.AddAttribute(seq++, "class", css);

      // Computed inline style – cached to keep the same object reference across renders
      var style = _cachedCssStyle ?? (_cachedCssStyle = BuildCssStyle());
      if (!string.IsNullOrEmpty(style))
        builder.AddAttribute(seq++, "style", style);

      // Tooltip
      if (_toolTipText != null)
        builder.AddAttribute(seq++, "title", _toolTipText);

      // Disabled
      if (!_enabled)
        builder.AddAttribute(seq++, "disabled", true);

      // Tabindex
      if (_tabStop && _tabIndex >= 0)
        builder.AddAttribute(seq++, "tabindex", _tabIndex);

      // Accessible attributes
      if (!string.IsNullOrEmpty(_accessibleName))
        builder.AddAttribute(seq++, "aria-label", _accessibleName);
      if (!string.IsNullOrEmpty(_accessibleDescription))
        builder.AddAttribute(seq++, "aria-description", _accessibleDescription);

      // Event handlers
      AddEventAttributes(builder, ref seq);

      // Content (subclass-specific)
      RenderContent(builder, ref seq);

      // Child controls
      RenderChildren(builder, ref seq);

      builder.CloseElement();
    }

    protected virtual string GetHtmlTag() => "div";

    protected virtual string GetCssClasses() {
      return string.Concat("swf-control swf-", GetType().Name.ToLowerInvariant());
    }

    protected virtual string BuildCssStyle() {
      var sb = new StringBuilder(256);

      // --- Position and Size ---
      if (_dock == DockStyle.None) {
        if (_parent != null && _parent.ManagesChildLayout) {
          // Parent manages layout via flex/grid - fill the parent's container slot
          sb.Append("position:relative;width:100%;height:100%;");
        } else {
          // Non-docked: absolute positioning within parent
          sb.Append("position:absolute;");
          sb.Append("left:").Append(_location.X).Append("px;");
          sb.Append("top:").Append(_location.Y).Append("px;");

          if (!_autoSize) {
            sb.Append("width:").Append(_size.Width).Append("px;");
            sb.Append("height:").Append(_size.Height).Append("px;");
          }
        }
      } else if (_dock == DockStyle.Fill) {
        // Fill: flex:1 1 auto, stretch in both directions
        sb.Append("position:relative;");
        sb.Append("flex:1 1 auto;");
        sb.Append("min-width:0;min-height:0;");
        sb.Append("overflow:auto;");
      } else if (_dock == DockStyle.Top) {
        sb.Append("position:relative;");
        sb.Append("flex:0 0 auto;");
        sb.Append("width:100%;");
        sb.Append("height:").Append(_size.Height).Append("px;");
        sb.Append("overflow:hidden;");
      } else if (_dock == DockStyle.Bottom) {
        sb.Append("position:relative;");
        sb.Append("flex:0 0 auto;");
        sb.Append("width:100%;");
        sb.Append("height:").Append(_size.Height).Append("px;");
        sb.Append("overflow:hidden;");
      } else if (_dock == DockStyle.Left) {
        sb.Append("position:relative;");
        sb.Append("flex:0 0 auto;");
        sb.Append("width:").Append(_size.Width).Append("px;");
        sb.Append("height:100%;");
        sb.Append("overflow:hidden;");
      } else if (_dock == DockStyle.Right) {
        sb.Append("position:relative;");
        sb.Append("flex:0 0 auto;");
        sb.Append("width:").Append(_size.Width).Append("px;");
        sb.Append("height:100%;");
        sb.Append("overflow:hidden;");
      }

      // --- Min/Max constraints ---
      if (_minimumSize.Width > 0)
        sb.Append("min-width:").Append(_minimumSize.Width).Append("px;");
      if (_minimumSize.Height > 0)
        sb.Append("min-height:").Append(_minimumSize.Height).Append("px;");
      if (_maximumSize.Width > 0)
        sb.Append("max-width:").Append(_maximumSize.Width).Append("px;");
      if (_maximumSize.Height > 0)
        sb.Append("max-height:").Append(_maximumSize.Height).Append("px;");

      // --- Colors ---
      if (!_backColor.IsEmpty)
        sb.Append("background-color:").Append(_backColor.ToCss()).Append(';');
      if (!_foreColor.IsEmpty)
        sb.Append("color:").Append(_foreColor.ToCss()).Append(';');

      // --- Font ---
      if (_font != null) {
        sb.Append("font:");
        if (_font.Italic) sb.Append("italic ");
        if (_font.Bold) sb.Append("bold ");
        sb.Append(_font.SizeInPoints.ToString("0.##", CultureInfo.InvariantCulture)).Append("pt");
        sb.Append(" '").Append(_font.Name).Append("';");
        if (_font.Underline && _font.Strikeout)
          sb.Append("text-decoration:underline line-through;");
        else if (_font.Underline)
          sb.Append("text-decoration:underline;");
        else if (_font.Strikeout)
          sb.Append("text-decoration:line-through;");
      }

      // --- Padding ---
      if (_padding != Padding.Empty)
        sb.Append("padding:").Append(_padding.ToCss()).Append(';');

      // --- Margin ---
      if (_margin != Padding.Empty)
        sb.Append("margin:").Append(_margin.ToCss()).Append(';');

      // --- Cursor ---
      Cursor cursorToUse = _useWaitCursor ? Cursors.WaitCursor : _cursor;
      if (cursorToUse != null)
        sb.Append("cursor:").Append(cursorToUse.ToCss()).Append(';');

      // --- Background image ---
      if (_backgroundImage != null) {
        var dataUri = _backgroundImage.ToDataUri();
        if (!string.IsNullOrEmpty(dataUri)) {
          sb.Append("background-image:url('").Append(dataUri).Append("');");
          switch (_backgroundImageLayout) {
            case ImageLayout.None:
              sb.Append("background-repeat:no-repeat;background-size:auto;");
              break;
            case ImageLayout.Tile:
              sb.Append("background-repeat:repeat;background-size:auto;");
              break;
            case ImageLayout.Center:
              sb.Append("background-repeat:no-repeat;background-position:center;background-size:auto;");
              break;
            case ImageLayout.Stretch:
              sb.Append("background-repeat:no-repeat;background-size:100% 100%;");
              break;
            case ImageLayout.Zoom:
              sb.Append("background-repeat:no-repeat;background-position:center;background-size:contain;");
              break;
          }
        }
      }

      // --- Right-to-left ---
      if (_rightToLeft == RightToLeft.Yes)
        sb.Append("direction:rtl;");

      // --- Z-index (for BringToFront/SendToBack) ---
      if (_zIndex > 0)
        sb.Append("z-index:").Append(_zIndex).Append(';');

      // --- Box sizing ---
      sb.Append("box-sizing:border-box;");

      return sb.ToString();
    }

    protected virtual void RenderContent(RenderTreeBuilder builder, ref int seq) {
      // Base implementation: nothing. Subclasses override for their specific content.
    }

    protected internal virtual void RenderChildren(RenderTreeBuilder builder, ref int seq) {
      if (_controls.Count == 0) return;

      // Determine if any children are docked - if so, parent uses flex layout
      bool hasDocked = false;
      bool hasNonDocked = false;

      for (int i = 0; i < _controls.Count; i++) {
        var child = _controls[i];
        if (!child.Visible) continue;
        if (child.Dock != DockStyle.None)
          hasDocked = true;
        else
          hasNonDocked = true;
      }

      if (hasDocked) {
        // WinForms dock layout using a flex container.
        // Use position:relative + 100% sizing instead of position:absolute
        // so the container participates in parent flow and scrolling works.
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "swf-dock-container");
        builder.AddAttribute(seq++, "style", "display:flex;flex-direction:column;width:100%;height:100%;overflow:hidden;");

        // Render Top-docked first
        RenderDockedGroup(builder, ref seq, DockStyle.Top);

        // Middle section: Left, Fill, Right
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "style", "display:flex;flex-direction:row;flex:1 1 auto;min-height:0;overflow:hidden;");

        RenderDockedGroup(builder, ref seq, DockStyle.Left);
        RenderDockedGroup(builder, ref seq, DockStyle.Fill);
        RenderDockedGroup(builder, ref seq, DockStyle.Right);

        builder.CloseElement(); // middle row

        // Bottom-docked
        RenderDockedGroup(builder, ref seq, DockStyle.Bottom);

        builder.CloseElement(); // dock container
      }

      // Render non-docked children (absolute positioned within parent)
      if (hasNonDocked) {
        for (int i = 0; i < _controls.Count; i++) {
          var child = _controls[i];
          if (child.Visible && child.Dock == DockStyle.None)
            child.BuildRenderTree(builder, ref seq);
        }
      }
    }

    private void RenderDockedGroup(RenderTreeBuilder builder, ref int seq, DockStyle dock) {
      if (dock == DockStyle.Fill) {
        // For Fill-docked children, only render the one with the highest z-index (topmost).
        // Falls back to the last visible Fill-docked child if z-indexes are equal.
        Control topmost = null;
        for (int i = 0; i < _controls.Count; i++) {
          var child = _controls[i];
          if (child.Visible && child.Dock == DockStyle.Fill) {
            if (topmost == null || child._zIndex >= topmost._zIndex)
              topmost = child;
          }
        }
        if (topmost != null)
          topmost.BuildRenderTree(builder, ref seq);
      } else {
        for (int i = 0; i < _controls.Count; i++) {
          var child = _controls[i];
          if (child.Visible && child.Dock == dock)
            child.BuildRenderTree(builder, ref seq);
        }
      }
    }

    protected virtual void AddEventAttributes(RenderTreeBuilder builder, ref int seq) {
      // onclick
      if (Click != null || DoubleClick != null || MouseClick != null) {
        builder.AddAttribute(seq++, "onclick",
          EventCallback.Factory.Create<BlazorMouseEventArgs>(
            GetBlazorReceiver(),
            (BlazorMouseEventArgs e) => OnBrowserClick(e)));
      }

      // ondblclick
      if (DoubleClick != null || MouseDoubleClick != null) {
        builder.AddAttribute(seq++, "ondblclick",
          EventCallback.Factory.Create<BlazorMouseEventArgs>(
            GetBlazorReceiver(),
            (BlazorMouseEventArgs e) => OnBrowserDoubleClick(e)));
      }

      // onmousedown
      if (MouseDown != null) {
        builder.AddAttribute(seq++, "onmousedown",
          EventCallback.Factory.Create<BlazorMouseEventArgs>(
            GetBlazorReceiver(),
            (BlazorMouseEventArgs e) => OnBrowserMouseDown(e)));
      }

      // onmouseup
      if (MouseUp != null) {
        builder.AddAttribute(seq++, "onmouseup",
          EventCallback.Factory.Create<BlazorMouseEventArgs>(
            GetBlazorReceiver(),
            (BlazorMouseEventArgs e) => OnBrowserMouseUp(e)));
      }

      // onmousemove
      if (MouseMove != null) {
        builder.AddAttribute(seq++, "onmousemove",
          EventCallback.Factory.Create<BlazorMouseEventArgs>(
            GetBlazorReceiver(),
            (BlazorMouseEventArgs e) => OnBrowserMouseMove(e)));
      }

      // onmouseenter
      if (MouseEnter != null || MouseHover != null) {
        builder.AddAttribute(seq++, "onmouseenter",
          EventCallback.Factory.Create<BlazorMouseEventArgs>(
            GetBlazorReceiver(),
            (BlazorMouseEventArgs e) => {
              OnMouseEnter(EventArgs.Empty);
              OnMouseHover(EventArgs.Empty);
            }));
      }

      // onmouseleave
      if (MouseLeave != null) {
        builder.AddAttribute(seq++, "onmouseleave",
          EventCallback.Factory.Create<BlazorMouseEventArgs>(
            GetBlazorReceiver(),
            (BlazorMouseEventArgs e) => OnMouseLeave(EventArgs.Empty)));
      }

      // onmousewheel
      if (MouseWheel != null) {
        builder.AddAttribute(seq++, "onwheel",
          EventCallback.Factory.Create<BlazorWheelEventArgs>(
            GetBlazorReceiver(),
            (BlazorWheelEventArgs e) => {
              var mea = new MouseEventArgs(MouseButtons.None, 0, (int)e.OffsetX, (int)e.OffsetY, (int)(-e.DeltaY));
              OnMouseWheel(mea);
            }));
      }

      // onkeydown
      if (KeyDown != null || PreviewKeyDown != null) {
        builder.AddAttribute(seq++, "onkeydown",
          EventCallback.Factory.Create<BlazorKeyboardEventArgs>(
            GetBlazorReceiver(),
            (BlazorKeyboardEventArgs e) => OnBrowserKeyDown(e)));
      }

      // onkeyup
      if (KeyUp != null) {
        builder.AddAttribute(seq++, "onkeyup",
          EventCallback.Factory.Create<BlazorKeyboardEventArgs>(
            GetBlazorReceiver(),
            (BlazorKeyboardEventArgs e) => {
              var kea = new KeyEventArgs(MapBrowserKey(e));
              OnKeyUp(kea);
            }));
      }

      // onkeypress -> KeyPress
      if (KeyPress != null) {
        builder.AddAttribute(seq++, "onkeypress",
          EventCallback.Factory.Create<BlazorKeyboardEventArgs>(
            GetBlazorReceiver(),
            (BlazorKeyboardEventArgs e) => {
              if (!string.IsNullOrEmpty(e.Key) && e.Key.Length == 1)
                OnKeyPress(new KeyPressEventArgs(e.Key[0]));
            }));
      }

      // onfocus
      if (GotFocus != null || Enter != null) {
        builder.AddAttribute(seq++, "onfocus",
          EventCallback.Factory.Create<BlazorFocusEventArgs>(
            GetBlazorReceiver(),
            (BlazorFocusEventArgs e) => {
              _focused = true;
              OnGotFocus(EventArgs.Empty);
              OnEnter(EventArgs.Empty);
            }));
      }

      // onblur
      if (LostFocus != null || Leave != null || Validated != null || Validating != null) {
        builder.AddAttribute(seq++, "onblur",
          EventCallback.Factory.Create<BlazorFocusEventArgs>(
            GetBlazorReceiver(),
            (BlazorFocusEventArgs e) => {
              _focused = false;
              OnLostFocus(EventArgs.Empty);
              OnLeave(EventArgs.Empty);
              Validating?.Invoke(this, new System.ComponentModel.CancelEventArgs());
              Validated?.Invoke(this, EventArgs.Empty);
            }));
      }
    }

    // ═══════════════════════════════════════════════
    // Browser event translation helpers
    // ═══════════════════════════════════════════════

    private void OnBrowserClick(BlazorMouseEventArgs e) {
      var btn = MapBrowserButton(e);
      var mea = new MouseEventArgs(btn, 1, (int)e.OffsetX, (int)e.OffsetY, 0);
      OnClick(EventArgs.Empty);
      OnMouseClick(mea);
    }

    private void OnBrowserDoubleClick(BlazorMouseEventArgs e) {
      var btn = MapBrowserButton(e);
      var mea = new MouseEventArgs(btn, 2, (int)e.OffsetX, (int)e.OffsetY, 0);
      OnDoubleClick(EventArgs.Empty);
      OnMouseDoubleClick(mea);
    }

    private void OnBrowserMouseDown(BlazorMouseEventArgs e) {
      var btn = MapBrowserButton(e);
      OnMouseDown(new MouseEventArgs(btn, 1, (int)e.OffsetX, (int)e.OffsetY, 0));
    }

    private void OnBrowserMouseUp(BlazorMouseEventArgs e) {
      var btn = MapBrowserButton(e);
      OnMouseUp(new MouseEventArgs(btn, 0, (int)e.OffsetX, (int)e.OffsetY, 0));
    }

    private void OnBrowserMouseMove(BlazorMouseEventArgs e) {
      // For mousemove, use e.Buttons (bitmask of held buttons) not e.Button
      // In browsers, e.Button is 0 for mousemove regardless of button state
      var btn = MapBrowserButtons(e.Buttons);
      OnMouseMove(new MouseEventArgs(btn, 0, (int)e.OffsetX, (int)e.OffsetY, 0));
    }

    private void OnBrowserKeyDown(BlazorKeyboardEventArgs e) {
      var keys = MapBrowserKey(e);
      if (PreviewKeyDown != null) {
        var pkd = new PreviewKeyDownEventArgs(keys);
        PreviewKeyDown(this, pkd);
      }
      var kea = new KeyEventArgs(keys);
      OnKeyDown(kea);
    }

    private static MouseButtons MapBrowserButton(BlazorMouseEventArgs e) {
      switch (e.Button) {
        case 0: return MouseButtons.Left;
        case 1: return MouseButtons.Middle;
        case 2: return MouseButtons.Right;
        default: return MouseButtons.None;
      }
    }

    /// <summary>
    /// Maps the browser Buttons bitmask (held buttons) to WinForms MouseButtons.
    /// Browser: 1=primary, 2=secondary, 4=middle, 8=back, 16=forward
    /// </summary>
    private static MouseButtons MapBrowserButtons(long buttons) {
      var result = MouseButtons.None;
      if ((buttons & 1) != 0) result |= MouseButtons.Left;
      if ((buttons & 2) != 0) result |= MouseButtons.Right;
      if ((buttons & 4) != 0) result |= MouseButtons.Middle;
      return result;
    }

    private static Keys MapBrowserKey(BlazorKeyboardEventArgs e) {
      Keys key = Keys.None;

      if (!string.IsNullOrEmpty(e.Code)) {
        // Map common key codes
        if (e.Code.StartsWith("Key") && e.Code.Length == 4)
          key = (Keys)((int)Keys.A + (e.Code[3] - 'A'));
        else if (e.Code.StartsWith("Digit") && e.Code.Length == 6)
          key = (Keys)((int)Keys.D0 + (e.Code[5] - '0'));
        else if (e.Code.StartsWith("Numpad") && e.Code.Length == 7 && char.IsDigit(e.Code[6]))
          key = (Keys)((int)Keys.NumPad0 + (e.Code[6] - '0'));
        else if (e.Code.StartsWith("F") && e.Code.Length >= 2 && e.Code.Length <= 3 && char.IsDigit(e.Code[1])) {
          if (int.TryParse(e.Code.Substring(1), out int fNum) && fNum >= 1 && fNum <= 12)
            key = (Keys)((int)Keys.F1 + (fNum - 1));
        } else {
          switch (e.Code) {
            case "Space": key = Keys.Space; break;
            case "Enter": key = Keys.Enter; break;
            case "Tab": key = Keys.Tab; break;
            case "Escape": key = Keys.Escape; break;
            case "Backspace": key = Keys.Back; break;
            case "Delete": key = Keys.Delete; break;
            case "Insert": key = Keys.Insert; break;
            case "Home": key = Keys.Home; break;
            case "End": key = Keys.End; break;
            case "PageUp": key = Keys.PageUp; break;
            case "PageDown": key = Keys.PageDown; break;
            case "ArrowLeft": key = Keys.Left; break;
            case "ArrowRight": key = Keys.Right; break;
            case "ArrowUp": key = Keys.Up; break;
            case "ArrowDown": key = Keys.Down; break;
            case "ShiftLeft": case "ShiftRight": key = Keys.ShiftKey; break;
            case "ControlLeft": case "ControlRight": key = Keys.ControlKey; break;
            case "AltLeft": case "AltRight": key = Keys.Menu; break;
            case "CapsLock": key = Keys.CapsLock; break;
            case "NumLock": key = Keys.NumLock; break;
            case "ScrollLock": key = Keys.Scroll; break;
            default: break;
          }
        }
      }

      // Apply modifiers
      if (e.ShiftKey) key |= Keys.Shift;
      if (e.CtrlKey) key |= Keys.Control;
      if (e.AltKey) key |= Keys.Alt;

      return key;
    }
  }
}
