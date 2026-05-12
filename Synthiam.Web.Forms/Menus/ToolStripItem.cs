// =============================================================================
// Synthiam.Web.Forms - ToolStripItem for Blazor
// =============================================================================

using System;
using System.Drawing;
using Microsoft.AspNetCore.Components.Rendering;
using Synthiam.Web.Forms;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // Supporting enums
  // ---------------------------------------------------------------------------

  public enum ToolStripItemImageScaling {
    None = 0,
    SizeToFit = 1
  }

  public enum ToolStripItemOverflow {
    Never = 0,
    Always = 1,
    AsNeeded = 2
  }

  public enum MergeAction {
    Append = 0,
    Insert = 1,
    Replace = 2,
    Remove = 3,
    MatchOnly = 4
  }

  // ---------------------------------------------------------------------------
  // ToolStripItem (partial - extends ForwardDeclarations.cs stub)
  // ---------------------------------------------------------------------------
  public partial class ToolStripItem : System.ComponentModel.IComponent, IDisposable {

    /// <summary>
    /// Gets the Blazor component receiver for EventCallback, walking up via Owner to the hosting component.
    /// </summary>
    internal object GetBlazorReceiver() {
      // Walk up: ToolStripItem → Owner (ToolStrip, which is a Control) → parent chain → root Form
      if (_owner != null)
        return _owner.GetBlazorReceiver();
      return this;
    }

    private string _text = string.Empty;
    private string _name = string.Empty;
    private Image _image;
    private ToolStripItemImageScaling _imageScaling = ToolStripItemImageScaling.SizeToFit;
    private ContentAlignment _imageAlign = ContentAlignment.MiddleCenter;
    private int _imageIndex = -1;
    private string _imageKey = string.Empty;
    private Color _imageTransparentColor = Color.Empty;
    private ToolStripItemDisplayStyle _displayStyle = ToolStripItemDisplayStyle.ImageAndText;
    private TextImageRelation _textImageRelation = TextImageRelation.ImageBeforeText;
    private ContentAlignment _textAlign = ContentAlignment.MiddleCenter;
    private Font _font;
    private Color _foreColor = Color.Empty;
    private Color _backColor = Color.Empty;
    private bool _enabled = true;
    private bool _visible = true;
    private bool _selected;
    private bool _pressed;
    private string _toolTipText;
    private object _tag;
    private ToolStrip _owner;
    private Size _size = new Size(23, 23);
    private Padding _padding;
    private Padding _margin;
    private ToolStripItemAlignment _alignment = ToolStripItemAlignment.Left;
    private bool _autoSize = true;
    private bool _autoToolTip = true;
    private bool _available = true;
    private MergeAction _mergeAction = MergeAction.Append;
    private int _mergeIndex = -1;
    private ToolStripItemOverflow _overflow = ToolStripItemOverflow.AsNeeded;
    private RightToLeft _rightToLeft = RightToLeft.Inherit;
    private bool _isDisposed;

    // IComponent
    public event EventHandler Disposed;
    public System.ComponentModel.ISite Site { get; set; }

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public virtual string Text {
      get => _text;
      set {
        if (_text != value) {
          _text = value ?? string.Empty;
          TextChanged?.Invoke(this, EventArgs.Empty);
          _owner?.NotifyStateChanged();
        }
      }
    }

    public string Name {
      get => _name;
      set => _name = value ?? string.Empty;
    }

    public Image Image {
      get => _image;
      set { _image = value; _owner?.NotifyStateChanged(); }
    }

    public ToolStripItemImageScaling ImageScaling {
      get => _imageScaling;
      set => _imageScaling = value;
    }

    public ContentAlignment ImageAlign {
      get => _imageAlign;
      set => _imageAlign = value;
    }

    public int ImageIndex {
      get => _imageIndex;
      set => _imageIndex = value;
    }

    public string ImageKey {
      get => _imageKey;
      set => _imageKey = value ?? string.Empty;
    }

    public Color ImageTransparentColor {
      get => _imageTransparentColor;
      set => _imageTransparentColor = value;
    }

    public ToolStripItemDisplayStyle DisplayStyle {
      get => _displayStyle;
      set { _displayStyle = value; _owner?.NotifyStateChanged(); }
    }

    public TextImageRelation TextImageRelation {
      get => _textImageRelation;
      set => _textImageRelation = value;
    }

    public ContentAlignment TextAlign {
      get => _textAlign;
      set => _textAlign = value;
    }

    public virtual Font Font {
      get => _font;
      set { _font = value; FontChanged?.Invoke(this, EventArgs.Empty); }
    }

    public virtual Color ForeColor {
      get => _foreColor;
      set { _foreColor = value; ForeColorChanged?.Invoke(this, EventArgs.Empty); }
    }

    public virtual Color BackColor {
      get => _backColor;
      set { _backColor = value; BackColorChanged?.Invoke(this, EventArgs.Empty); }
    }

    public virtual bool Enabled {
      get => _enabled;
      set {
        if (_enabled != value) {
          _enabled = value;
          EnabledChanged?.Invoke(this, EventArgs.Empty);
          _owner?.NotifyStateChanged();
        }
      }
    }

    public virtual bool Visible {
      get => _visible;
      set {
        if (_visible != value) {
          _visible = value;
          VisibleChanged?.Invoke(this, EventArgs.Empty);
          _owner?.NotifyStateChanged();
        }
      }
    }

    public bool Selected {
      get => _selected;
      internal set => _selected = value;
    }

    public bool Pressed {
      get => _pressed;
      internal set => _pressed = value;
    }

    public string ToolTipText {
      get => _toolTipText;
      set => _toolTipText = value;
    }

    public object Tag {
      get => _tag;
      set => _tag = value;
    }

    public ToolStrip Owner {
      get => _owner;
      internal set => _owner = value;
    }

    public ToolStrip GetCurrentParent() => _owner;

    public virtual Size Size {
      get => _size;
      set => _size = value;
    }

    public int Width {
      get => _size.Width;
      set => _size = new Size(value, _size.Height);
    }

    public int Height {
      get => _size.Height;
      set => _size = new Size(_size.Width, value);
    }

    public Rectangle Bounds => new Rectangle(Point.Empty, _size);

    public Padding Padding {
      get => _padding;
      set => _padding = value;
    }

    public Padding Margin {
      get => _margin;
      set => _margin = value;
    }

    public ToolStripItemAlignment Alignment {
      get => _alignment;
      set => _alignment = value;
    }

    public bool AutoSize {
      get => _autoSize;
      set => _autoSize = value;
    }

    public bool AutoToolTip {
      get => _autoToolTip;
      set => _autoToolTip = value;
    }

    public bool Available {
      get => _available;
      set { _available = value; Visible = value; }
    }

    public MergeAction MergeAction {
      get => _mergeAction;
      set => _mergeAction = value;
    }

    public int MergeIndex {
      get => _mergeIndex;
      set => _mergeIndex = value;
    }

    public ToolStripItemOverflow Overflow {
      get => _overflow;
      set => _overflow = value;
    }

    public RightToLeft RightToLeft {
      get => _rightToLeft;
      set => _rightToLeft = value;
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
    public event PaintEventHandler Paint;
    public event EventHandler VisibleChanged;
    public event EventHandler EnabledChanged;
    public event EventHandler TextChanged;
    public event EventHandler BackColorChanged;
    public event EventHandler ForeColorChanged;
    public event EventHandler FontChanged;

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public void PerformClick() {
      Click?.Invoke(this, EventArgs.Empty);
    }

    public void Invalidate() {
      _owner?.NotifyStateChanged();
    }

    public void Select() {
      _selected = true;
    }

    public override string ToString() {
      return !string.IsNullOrEmpty(_text) ? _text : base.ToString();
    }

    public void Dispose() {
      if (!_isDisposed) {
        _isDisposed = true;
        Disposed?.Invoke(this, EventArgs.Empty);
      }
    }

    // ═══════════════════════════════════════════════
    // Rendering (called by ToolStrip)
    // ═══════════════════════════════════════════════

    internal virtual void BuildRenderTree(RenderTreeBuilder builder, ref int seq) {
      if (!_visible) return;

      builder.OpenElement(seq++, "li");
      builder.AddAttribute(seq++, "class", "swf-toolstrip-item");

      var style = "display:inline-flex;align-items:center;";
      if (!_backColor.IsEmpty) style += $"background-color:{_backColor.ToCss()};";
      if (!_foreColor.IsEmpty) style += $"color:{_foreColor.ToCss()};";
      builder.AddAttribute(seq++, "style", style);

      if (_toolTipText != null)
        builder.AddAttribute(seq++, "title", _toolTipText);

      builder.OpenElement(seq++, "button");
      builder.AddAttribute(seq++, "class", "swf-toolstrip-item-btn");
      builder.AddAttribute(seq++, "style", "border:none;background:transparent;cursor:pointer;padding:2px 6px;display:inline-flex;align-items:center;");

      if (!_enabled)
        builder.AddAttribute(seq++, "disabled", true);

      builder.AddAttribute(seq++, "onclick",
        Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(
          GetBlazorReceiver(),
          (Microsoft.AspNetCore.Components.Web.MouseEventArgs _) => PerformClick()));

      // Image
      if (_image != null && (_displayStyle == ToolStripItemDisplayStyle.Image || _displayStyle == ToolStripItemDisplayStyle.ImageAndText)) {
        var dataUri = _image.ToDataUri();
        if (!string.IsNullOrEmpty(dataUri)) {
          builder.OpenElement(seq++, "img");
          builder.AddAttribute(seq++, "src", dataUri);
          builder.AddAttribute(seq++, "style", "width:16px;height:16px;margin-right:2px;");
          builder.CloseElement();
        }
      }

      // Text
      if (!string.IsNullOrEmpty(_text) && (_displayStyle == ToolStripItemDisplayStyle.Text || _displayStyle == ToolStripItemDisplayStyle.ImageAndText)) {
        builder.OpenElement(seq++, "span");
        builder.AddContent(seq++, _text);
        builder.CloseElement();
      }

      builder.CloseElement(); // button
      builder.CloseElement(); // li
    }
  }
}
