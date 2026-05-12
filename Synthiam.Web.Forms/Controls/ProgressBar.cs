// =============================================================================
// Synthiam.Web.Forms - ProgressBar control for Blazor
// =============================================================================

using System;
using System.Globalization;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  public enum ProgressBarStyle {
    Blocks = 0,
    Continuous = 1,
    Marquee = 2
  }

  public class ProgressBar : Control {

    private int _value;
    private int _minimum;
    private int _maximum = 100;
    private int _step = 10;
    private ProgressBarStyle _style = ProgressBarStyle.Continuous;
    private int _marqueeAnimationSpeed = 100;

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public int Value {
      get => _value;
      set {
        value = Math.Max(_minimum, Math.Min(_maximum, value));
        if (_value != value) {
          _value = value;
          NotifyStateChanged();
        }
      }
    }

    public int Minimum {
      get => _minimum;
      set {
        _minimum = value;
        if (_value < _minimum) Value = _minimum;
      }
    }

    public int Maximum {
      get => _maximum;
      set {
        _maximum = value;
        if (_value > _maximum) Value = _maximum;
      }
    }

    public int Step {
      get => _step;
      set => _step = value;
    }

    public ProgressBarStyle Style {
      get => _style;
      set { _style = value; NotifyStateChanged(); }
    }

    public int MarqueeAnimationSpeed {
      get => _marqueeAnimationSpeed;
      set { _marqueeAnimationSpeed = value; NotifyStateChanged(); }
    }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public void PerformStep() {
      Value += _step;
    }

    public void Increment(int value) {
      Value += value;
    }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "div";

    protected override string GetCssClasses() {
      var css = base.GetCssClasses();
      if (_style == ProgressBarStyle.Marquee) css += " swf-progressbar-marquee";
      return css;
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "overflow:hidden;background-color:#e0e0e0;";
      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      if (_style == ProgressBarStyle.Marquee) {
        // Marquee: animated bar
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "style",
          "width:30%;height:100%;background-color:#06b025;" +
          "animation:swf-marquee " + (_marqueeAnimationSpeed * 10).ToString() + "ms linear infinite;");
        builder.CloseElement();
      } else {
        // Standard/Continuous: sized bar
        double range = _maximum - _minimum;
        double percent = range > 0 ? ((_value - _minimum) * 100.0 / range) : 0;

        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "style",
          "width:" + percent.ToString("0.##", CultureInfo.InvariantCulture) + "%;" +
          "height:100%;background-color:#06b025;transition:width 0.2s;");
        builder.CloseElement();
      }
    }
  }
}
