// =============================================================================
// Synthiam.Web.Forms - StatusStrip and related items for Blazor
// =============================================================================

using System;
using System.Drawing;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Synthiam.Web.Forms;
using BlazorMouseEventArgs = Microsoft.AspNetCore.Components.Web.MouseEventArgs;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // StatusStrip
  // ---------------------------------------------------------------------------
  public class StatusStrip : ToolStrip {

    private bool _sizingGrip = true;

    public StatusStrip() {
      Dock = DockStyle.Bottom;
      Stretch = true;
      LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
    }

    public bool SizingGrip {
      get => _sizingGrip;
      set { _sizingGrip = value; NotifyStateChanged(); }
    }

    protected override string GetCssClasses() {
      return "swf-control swf-statusstrip";
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "background:#f0f0f0;border-top:1px solid #ccc;padding:2px 4px;";
      return style;
    }
  }

  // ---------------------------------------------------------------------------
  // ToolStripStatusLabel
  // ---------------------------------------------------------------------------
  public class ToolStripStatusLabel : ToolStripLabel {

    private bool _spring;
    private BorderStyle _borderStyle = BorderStyle.None;
    private ToolStripStatusLabelBorderSides _borderSides = ToolStripStatusLabelBorderSides.None;

    public ToolStripStatusLabel() { }
    public ToolStripStatusLabel(string text) { Text = text; }
    public ToolStripStatusLabel(string text, Image image) { Text = text; Image = image; }
    public ToolStripStatusLabel(string text, Image image, EventHandler onClick) {
      Text = text;
      Image = image;
      if (onClick != null) Click += onClick;
    }

    public bool Spring {
      get => _spring;
      set { _spring = value; Owner?.NotifyStateChanged(); }
    }

    public new BorderStyle BorderStyle {
      get => _borderStyle;
      set { _borderStyle = value; Owner?.NotifyStateChanged(); }
    }

    public ToolStripStatusLabelBorderSides BorderSides {
      get => _borderSides;
      set => _borderSides = value;
    }

    internal override void BuildRenderTree(RenderTreeBuilder builder, ref int seq) {
      if (!Visible) return;

      builder.OpenElement(seq++, "li");
      builder.AddAttribute(seq++, "class", "swf-toolstrip-statuslabel");

      var style = "display:inline-flex;align-items:center;list-style:none;padding:0 6px;";
      if (_spring) style += "flex:1 1 auto;";
      if (_borderStyle == BorderStyle.FixedSingle) style += "border:1px solid #999;";
      else if (_borderStyle == BorderStyle.Fixed3D) style += "border:1px inset #ccc;";
      builder.AddAttribute(seq++, "style", style);

      if (ToolTipText != null)
        builder.AddAttribute(seq++, "title", ToolTipText);

      // Image
      if (Image != null) {
        var dataUri = Image.ToDataUri();
        if (!string.IsNullOrEmpty(dataUri)) {
          builder.OpenElement(seq++, "img");
          builder.AddAttribute(seq++, "src", dataUri);
          builder.AddAttribute(seq++, "style", "width:16px;height:16px;margin-right:2px;");
          builder.CloseElement();
        }
      }

      builder.OpenElement(seq++, "span");
      builder.AddContent(seq++, Text ?? string.Empty);
      builder.CloseElement();

      builder.CloseElement(); // li
    }
  }

  // ---------------------------------------------------------------------------
  // ToolStripProgressBar
  // ---------------------------------------------------------------------------
  public class ToolStripProgressBar : ToolStripItem {

    private int _value;
    private int _minimum;
    private int _maximum = 100;
    private int _step = 10;
    private ProgressBarStyle _style = ProgressBarStyle.Blocks;
    private int _marqueeAnimationSpeed = 100;

    public ToolStripProgressBar() { }
    public ToolStripProgressBar(string name) { Name = name; }

    public bool IsDisposed { get; private set; }

    public ProgressBar ProgressBar { get; } = new ProgressBar();

    public int Value {
      get => _value;
      set {
        _value = Math.Max(_minimum, Math.Min(_maximum, value));
        Owner?.NotifyStateChanged();
      }
    }

    public int Minimum {
      get => _minimum;
      set { _minimum = value; if (_value < _minimum) _value = _minimum; }
    }

    public int Maximum {
      get => _maximum;
      set { _maximum = value; if (_value > _maximum) _value = _maximum; }
    }

    public int Step {
      get => _step;
      set => _step = value;
    }

    public new ProgressBarStyle Style {
      get => _style;
      set => _style = value;
    }

    public int MarqueeAnimationSpeed {
      get => _marqueeAnimationSpeed;
      set => _marqueeAnimationSpeed = value;
    }

    public void PerformStep() {
      Value += _step;
    }

    public void Increment(int value) {
      Value += value;
    }

    internal override void BuildRenderTree(RenderTreeBuilder builder, ref int seq) {
      if (!Visible) return;

      builder.OpenElement(seq++, "li");
      builder.AddAttribute(seq++, "class", "swf-toolstrip-progressbar");
      builder.AddAttribute(seq++, "style", "display:inline-flex;align-items:center;list-style:none;padding:1px 4px;");

      if (ToolTipText != null)
        builder.AddAttribute(seq++, "title", ToolTipText);

      double percent = _maximum > _minimum ? (double)(_value - _minimum) / (_maximum - _minimum) * 100.0 : 0;

      builder.OpenElement(seq++, "div");
      builder.AddAttribute(seq++, "style", "width:100px;height:16px;background:#e0e0e0;border:1px solid #999;position:relative;");

      builder.OpenElement(seq++, "div");
      builder.AddAttribute(seq++, "style",
        $"width:{percent.ToString("0.#", CultureInfo.InvariantCulture)}%;height:100%;background:#06b025;transition:width 0.2s;");
      builder.CloseElement();

      builder.CloseElement(); // outer div
      builder.CloseElement(); // li
    }
  }

}
