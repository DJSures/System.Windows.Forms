// =============================================================================
// Synthiam.Web.Forms - SplitContainer control for Blazor
// =============================================================================

using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // SplitterCancelEventArgs
  // ---------------------------------------------------------------------------
  public class SplitterCancelEventArgs : CancelEventArgs {
    public int SplitX { get; set; }
    public int SplitY { get; set; }
    public int MouseCursorX { get; }
    public int MouseCursorY { get; }

    public SplitterCancelEventArgs(int mouseCursorX, int mouseCursorY, int splitX, int splitY)
      : base(false) {
      MouseCursorX = mouseCursorX;
      MouseCursorY = mouseCursorY;
      SplitX = splitX;
      SplitY = splitY;
    }
  }

  public delegate void SplitterCancelEventHandler(object sender, SplitterCancelEventArgs e);

  // ---------------------------------------------------------------------------
  // SplitterPanel
  // ---------------------------------------------------------------------------
  public class SplitterPanel : Panel {

    private readonly SplitContainer _owner;

    internal SplitterPanel(SplitContainer owner) {
      _owner = owner;
    }

    public SplitContainer Owner => _owner;
  }

  // ---------------------------------------------------------------------------
  // SplitContainer
  // ---------------------------------------------------------------------------
  public class SplitContainer : ContainerControl {

    private SplitterPanel _panel1;
    private SplitterPanel _panel2;
    private Orientation _orientation = Orientation.Vertical;
    private int _splitterDistance = 50;
    private int _splitterWidth = 4;
    private int _splitterIncrement = 1;
    private FixedPanel _fixedPanel = FixedPanel.None;
    private bool _isSplitterFixed;
    private bool _panel1Collapsed;
    private bool _panel2Collapsed;
    private int _panel1MinSize = 25;
    private int _panel2MinSize = 25;
    private BorderStyle _borderStyle = BorderStyle.None;

    public SplitContainer() {
      _panel1 = new SplitterPanel(this);
      _panel2 = new SplitterPanel(this);
      Controls.Add(_panel1);
      Controls.Add(_panel2);
    }

    internal override bool ManagesChildLayout => true;

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public SplitterPanel Panel1 => _panel1;
    public SplitterPanel Panel2 => _panel2;

    public Orientation Orientation {
      get => _orientation;
      set { _orientation = value; NotifyStateChanged(); }
    }

    public int SplitterDistance {
      get => _splitterDistance;
      set { _splitterDistance = value; NotifyStateChanged(); }
    }

    public int SplitterWidth {
      get => _splitterWidth;
      set { _splitterWidth = value; NotifyStateChanged(); }
    }

    public int SplitterIncrement {
      get => _splitterIncrement;
      set => _splitterIncrement = value;
    }

    public FixedPanel FixedPanel {
      get => _fixedPanel;
      set => _fixedPanel = value;
    }

    public bool IsSplitterFixed {
      get => _isSplitterFixed;
      set => _isSplitterFixed = value;
    }

    public bool Panel1Collapsed {
      get => _panel1Collapsed;
      set { _panel1Collapsed = value; NotifyStateChanged(); }
    }

    public bool Panel2Collapsed {
      get => _panel2Collapsed;
      set { _panel2Collapsed = value; NotifyStateChanged(); }
    }

    public int Panel1MinSize {
      get => _panel1MinSize;
      set => _panel1MinSize = value;
    }

    public int Panel2MinSize {
      get => _panel2MinSize;
      set => _panel2MinSize = value;
    }

    public new BorderStyle BorderStyle {
      get => _borderStyle;
      set { _borderStyle = value; NotifyStateChanged(); }
    }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event SplitterEventHandler SplitterMoved;
    public event SplitterCancelEventHandler SplitterMoving;

    protected virtual void OnSplitterMoved(SplitterEventArgs e) { SplitterMoved?.Invoke(this, e); }
    protected virtual void OnSplitterMoving(SplitterCancelEventArgs e) { SplitterMoving?.Invoke(this, e); }

    // ═══════════════════════════════════════════════
    // Rendering
    // ═══════════════════════════════════════════════

    protected override string GetCssClasses() {
      return "swf-control swf-splitcontainer";
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();

      style += "display:flex;";
      style += _orientation == Orientation.Vertical ? "flex-direction:row;" : "flex-direction:column;";

      switch (_borderStyle) {
        case BorderStyle.FixedSingle:
          style += "border:1px solid #888;";
          break;
        case BorderStyle.Fixed3D:
          style += "border:2px inset #ccc;";
          break;
      }

      return style;
    }

    protected internal override void RenderChildren(RenderTreeBuilder builder, ref int seq) {
      bool isVertical = _orientation == Orientation.Vertical;
      string sizeProperty = isVertical ? "width" : "height";

      // Panel 1
      if (!_panel1Collapsed) {
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "swf-splitcontainer-panel1");
        builder.AddAttribute(seq++, "style",
          $"flex:0 0 {_splitterDistance}px;min-{sizeProperty}:{_panel1MinSize}px;position:relative;overflow:auto;");
        _panel1.BuildRenderTree(builder, ref seq);
        builder.CloseElement();
      }

      // Splitter bar
      if (!_panel1Collapsed && !_panel2Collapsed) {
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "swf-splitcontainer-splitter");
        var splitterStyle = isVertical
          ? $"flex:0 0 {_splitterWidth}px;cursor:col-resize;background:#e0e0e0;"
          : $"flex:0 0 {_splitterWidth}px;cursor:row-resize;background:#e0e0e0;";
        builder.AddAttribute(seq++, "style", splitterStyle);
        builder.CloseElement();
      }

      // Panel 2
      if (!_panel2Collapsed) {
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "swf-splitcontainer-panel2");
        builder.AddAttribute(seq++, "style",
          $"flex:1 1 auto;min-{sizeProperty}:{_panel2MinSize}px;position:relative;overflow:auto;");
        _panel2.BuildRenderTree(builder, ref seq);
        builder.CloseElement();
      }
    }
  }
}
