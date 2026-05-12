// =============================================================================
// Synthiam.Web.Forms - TrackBar control for Blazor
// =============================================================================

using System;
using System.Drawing;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  public class TrackBar : Control {

    private int _value;
    private int _minimum;
    private int _maximum = 10;
    private int _smallChange = 1;
    private int _largeChange = 5;
    private int _tickFrequency = 1;
    private TickStyle _tickStyle = TickStyle.BottomRight;
    private Orientation _orientation = Orientation.Horizontal;

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public int Value {
      get => _value;
      set {
        value = Math.Max(_minimum, Math.Min(_maximum, value));
        if (_value != value) {
          _value = value;
          OnValueChanged(EventArgs.Empty);
          OnScroll(EventArgs.Empty);
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

    public int SmallChange {
      get => _smallChange;
      set => _smallChange = Math.Max(0, value);
    }

    public int LargeChange {
      get => _largeChange;
      set => _largeChange = Math.Max(0, value);
    }

    public int TickFrequency {
      get => _tickFrequency;
      set => _tickFrequency = value;
    }

    public TickStyle TickStyle {
      get => _tickStyle;
      set { _tickStyle = value; NotifyStateChanged(); }
    }

    public Orientation Orientation {
      get => _orientation;
      set { _orientation = value; InvalidateCssStyle(); NotifyStateChanged(); }
    }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event EventHandler Scroll;
    public event EventHandler ValueChanged;

    protected virtual void OnScroll(EventArgs e) {
      Scroll?.Invoke(this, e);
    }

    protected virtual void OnValueChanged(EventArgs e) {
      ValueChanged?.Invoke(this, e);
    }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "input";

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();

      if (_orientation == Orientation.Vertical)
        style += "writing-mode:vertical-lr;direction:rtl;-webkit-appearance:slider-vertical;";

      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      builder.AddAttribute(seq++, "type", "range");
      builder.AddAttribute(seq++, "value", _value.ToString(CultureInfo.InvariantCulture));
      builder.AddAttribute(seq++, "min", _minimum.ToString(CultureInfo.InvariantCulture));
      builder.AddAttribute(seq++, "max", _maximum.ToString(CultureInfo.InvariantCulture));
      builder.AddAttribute(seq++, "step", _smallChange.ToString(CultureInfo.InvariantCulture));
    }

    protected override void AddEventAttributes(RenderTreeBuilder builder, ref int seq) {
      base.AddEventAttributes(builder, ref seq);

      builder.AddAttribute(seq++, "oninput",
        EventCallback.Factory.Create<ChangeEventArgs>(
          GetBlazorReceiver(),
          (ChangeEventArgs e) => {
            if (int.TryParse(e.Value?.ToString(), out int v))
              Value = v;
          }));
    }

    internal override void BuildRenderTree(RenderTreeBuilder builder, ref int seq) {
      if (!Visible) return;

      EnsureHandleCreated();

      builder.OpenElement(seq++, GetHtmlTag());
      builder.SetKey(this);

      if (!string.IsNullOrEmpty(Name))
        builder.AddAttribute(seq++, "id", string.Concat("swf-", Name));

      var css = _cachedCssClasses ?? (_cachedCssClasses = GetCssClasses());
      builder.AddAttribute(seq++, "class", css);

      var style = _cachedCssStyle ?? (_cachedCssStyle = BuildCssStyle());
      if (!string.IsNullOrEmpty(style))
        builder.AddAttribute(seq++, "style", style);

      if (_toolTipText != null)
        builder.AddAttribute(seq++, "title", _toolTipText);

      if (!Enabled)
        builder.AddAttribute(seq++, "disabled", true);

      if (TabStop && TabIndex >= 0)
        builder.AddAttribute(seq++, "tabindex", TabIndex);

      AddEventAttributes(builder, ref seq);
      RenderContent(builder, ref seq);

      builder.CloseElement();
    }
  }
}
