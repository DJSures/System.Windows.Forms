// =============================================================================
// Synthiam.Web.Forms - CheckBox control for Blazor
// =============================================================================

using System;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  public class CheckBox : Control {

    private bool _checked;
    private CheckState _checkState = CheckState.Unchecked;
    private bool _threeState;
    private Appearance _appearance = Appearance.Normal;
    private ContentAlignment _checkAlign = ContentAlignment.MiddleLeft;
    private bool _autoCheck = true;
    private FlatStyle _flatStyle = FlatStyle.Standard;
    private ContentAlignment _textAlign = ContentAlignment.MiddleLeft;
    private FlatButtonAppearance _flatAppearance = new FlatButtonAppearance();

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public bool Checked {
      get => _checked;
      set {
        if (_checked != value) {
          _checked = value;
          _checkState = value ? CheckState.Checked : CheckState.Unchecked;
          OnCheckedChanged(EventArgs.Empty);
          OnCheckStateChanged(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public CheckState CheckState {
      get => _checkState;
      set {
        if (_checkState != value) {
          _checkState = value;
          _checked = value != CheckState.Unchecked;
          OnCheckedChanged(EventArgs.Empty);
          OnCheckStateChanged(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public bool ThreeState {
      get => _threeState;
      set => _threeState = value;
    }

    public Appearance Appearance {
      get => _appearance;
      set { _appearance = value; InvalidateCssClasses(); NotifyStateChanged(); }
    }

    public ContentAlignment CheckAlign {
      get => _checkAlign;
      set { _checkAlign = value; NotifyStateChanged(); }
    }

    public bool AutoCheck {
      get => _autoCheck;
      set => _autoCheck = value;
    }

    public FlatStyle FlatStyle {
      get => _flatStyle;
      set { _flatStyle = value; NotifyStateChanged(); }
    }

    public ContentAlignment TextAlign {
      get => _textAlign;
      set { _textAlign = value; NotifyStateChanged(); }
    }

    public FlatButtonAppearance FlatAppearance => _flatAppearance;

    public bool UseVisualStyleBackColor { get; set; } = true;

    public new bool UseCompatibleTextRendering { get; set; }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event EventHandler CheckedChanged;
    public event EventHandler CheckStateChanged;

    protected virtual void OnCheckedChanged(EventArgs e) {
      CheckedChanged?.Invoke(this, e);
    }

    protected virtual void OnCheckStateChanged(EventArgs e) {
      CheckStateChanged?.Invoke(this, e);
    }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "label";

    protected override string GetCssClasses() {
      var css = base.GetCssClasses();
      if (_appearance == Appearance.Button) css += " swf-checkbox-button";
      return css;
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "display:flex;align-items:center;gap:4px;cursor:pointer;";
      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      // Render the checkbox input
      builder.OpenElement(seq++, "input");
      builder.AddAttribute(seq++, "type", "checkbox");

      if (_checked)
        builder.AddAttribute(seq++, "checked", true);

      if (!Enabled)
        builder.AddAttribute(seq++, "disabled", true);

      if (_threeState && _checkState == CheckState.Indeterminate)
        builder.AddAttribute(seq++, "indeterminate", true);

      // Handle change
      builder.AddAttribute(seq++, "onchange",
        EventCallback.Factory.Create<ChangeEventArgs>(
          GetBlazorReceiver(),
          (ChangeEventArgs e) => {
            if (_autoCheck) {
              if (_threeState) {
                switch (_checkState) {
                  case CheckState.Unchecked:
                    CheckState = CheckState.Checked;
                    break;
                  case CheckState.Checked:
                    CheckState = CheckState.Indeterminate;
                    break;
                  case CheckState.Indeterminate:
                    CheckState = CheckState.Unchecked;
                    break;
                }
              } else {
                Checked = !_checked;
              }
            }
          }));

      builder.AddAttribute(seq++, "style", "pointer-events:auto;margin:0;");
      builder.CloseElement(); // input

      // Render label text
      if (!string.IsNullOrEmpty(Text)) {
        builder.OpenElement(seq++, "span");
        builder.AddContent(seq++, Text);
        builder.CloseElement();
      }
    }

    protected override void AddEventAttributes(RenderTreeBuilder builder, ref int seq) {
      // Wire up base mouse/key/focus events; onclick is handled via input's onchange
      base.AddEventAttributes(builder, ref seq);
    }
  }
}
