// =============================================================================
// Synthiam.Web.Forms - NumericUpDown control for Blazor
// =============================================================================

using System;
using System.Drawing;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  public class NumericUpDown : Control {

    private decimal _value;
    private decimal _minimum;
    private decimal _maximum = 100;
    private decimal _increment = 1;
    private int _decimalPlaces;
    private bool _thousandsSeparator;
    private bool _hexadecimal;
    private LeftRightAlignment _upDownAlign = LeftRightAlignment.Right;
    private bool _interceptArrowKeys = true;
    private bool _readOnly;
    private HorizontalAlignment _textAlign = HorizontalAlignment.Left;
    private BorderStyle _borderStyle = BorderStyle.Fixed3D;

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public decimal Value {
      get => _value;
      set {
        value = Math.Max(_minimum, Math.Min(_maximum, value));
        if (_value != value) {
          _value = value;
          OnValueChanged(EventArgs.Empty);
          OnTextChanged(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public decimal Minimum {
      get => _minimum;
      set {
        _minimum = value;
        if (_value < _minimum) Value = _minimum;
      }
    }

    public decimal Maximum {
      get => _maximum;
      set {
        _maximum = value;
        if (_value > _maximum) Value = _maximum;
      }
    }

    public decimal Increment {
      get => _increment;
      set => _increment = value;
    }

    public int DecimalPlaces {
      get => _decimalPlaces;
      set { _decimalPlaces = value; NotifyStateChanged(); }
    }

    public bool ThousandsSeparator {
      get => _thousandsSeparator;
      set { _thousandsSeparator = value; NotifyStateChanged(); }
    }

    public bool Hexadecimal {
      get => _hexadecimal;
      set { _hexadecimal = value; NotifyStateChanged(); }
    }

    public LeftRightAlignment UpDownAlign {
      get => _upDownAlign;
      set => _upDownAlign = value;
    }

    public bool InterceptArrowKeys {
      get => _interceptArrowKeys;
      set => _interceptArrowKeys = value;
    }

    public bool ReadOnly {
      get => _readOnly;
      set { _readOnly = value; NotifyStateChanged(); }
    }

    public HorizontalAlignment TextAlign {
      get => _textAlign;
      set { _textAlign = value; InvalidateCssStyle(); NotifyStateChanged(); }
    }

    public BorderStyle BorderStyle {
      get => _borderStyle;
      set { _borderStyle = value; InvalidateCssStyle(); NotifyStateChanged(); }
    }

    public override string Text {
      get {
        if (_hexadecimal)
          return ((long)_value).ToString("X");
        return _value.ToString("F" + _decimalPlaces, CultureInfo.CurrentCulture);
      }
      set {
        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal d))
          Value = d;
      }
    }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event EventHandler ValueChanged;

    protected virtual void OnValueChanged(EventArgs e) {
      ValueChanged?.Invoke(this, e);
    }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public void UpButton() {
      Value += _increment;
    }

    public void DownButton() {
      Value -= _increment;
    }

    public void Select(int start, int length) {
      // Stub: text selection not applicable for HTML number input
    }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "input";

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();

      if (_borderStyle == BorderStyle.FixedSingle)
        style += "border:1px solid #808080;";
      else if (_borderStyle == BorderStyle.Fixed3D)
        style += "border:2px inset;";
      else if (_borderStyle == BorderStyle.None)
        style += "border:none;";

      switch (_textAlign) {
        case HorizontalAlignment.Center: style += "text-align:center;"; break;
        case HorizontalAlignment.Right: style += "text-align:right;"; break;
        default: style += "text-align:left;"; break;
      }

      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      builder.AddAttribute(seq++, "type", "number");
      builder.AddAttribute(seq++, "value", _value.ToString(CultureInfo.InvariantCulture));
      builder.AddAttribute(seq++, "min", _minimum.ToString(CultureInfo.InvariantCulture));
      builder.AddAttribute(seq++, "max", _maximum.ToString(CultureInfo.InvariantCulture));

      // Compute step
      string step;
      if (_decimalPlaces > 0) {
        decimal s = _increment;
        step = s.ToString("F" + _decimalPlaces, CultureInfo.InvariantCulture);
      } else {
        step = _increment.ToString(CultureInfo.InvariantCulture);
      }
      builder.AddAttribute(seq++, "step", step);

      if (_readOnly)
        builder.AddAttribute(seq++, "readonly", true);
    }

    protected override void AddEventAttributes(RenderTreeBuilder builder, ref int seq) {
      base.AddEventAttributes(builder, ref seq);

      builder.AddAttribute(seq++, "onchange",
        EventCallback.Factory.Create<ChangeEventArgs>(
          GetBlazorReceiver(),
          (ChangeEventArgs e) => {
            if (decimal.TryParse(e.Value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d))
              Value = d;
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

  public enum LeftRightAlignment {
    Left = 0,
    Right = 1
  }
}
