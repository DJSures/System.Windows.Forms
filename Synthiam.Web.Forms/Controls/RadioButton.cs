// =============================================================================
// Synthiam.Web.Forms - RadioButton control for Blazor
// =============================================================================

using System;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  public class RadioButton : Control {

    private bool _checked;
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
          OnCheckedChanged(EventArgs.Empty);
          NotifyStateChanged();

          // Uncheck other radio buttons in the same parent
          if (value && _autoCheck && _parent != null) {
            foreach (Control sibling in _parent.Controls) {
              if (sibling is RadioButton rb && rb != this && rb._checked) {
                rb._checked = false;
                rb.OnCheckedChanged(EventArgs.Empty);
                rb.NotifyStateChanged();
              }
            }
          }
        }
      }
    }

    public Appearance Appearance {
      get => _appearance;
      set { _appearance = value; NotifyStateChanged(); }
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

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event EventHandler CheckedChanged;

    protected virtual void OnCheckedChanged(EventArgs e) {
      CheckedChanged?.Invoke(this, e);
    }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "label";

    protected override string GetCssClasses() {
      var css = base.GetCssClasses();
      if (_appearance == Appearance.Button) css += " swf-radio-button-style";
      return css;
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "display:flex;align-items:center;gap:4px;cursor:pointer;";
      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      // Determine the group name from the parent control
      string groupName = _parent != null ? "swf-radio-" + _parent.GetHashCode().ToString() : "swf-radio-default";

      builder.OpenElement(seq++, "input");
      builder.AddAttribute(seq++, "type", "radio");
      builder.AddAttribute(seq++, "name", groupName);

      if (_checked)
        builder.AddAttribute(seq++, "checked", true);

      if (!Enabled)
        builder.AddAttribute(seq++, "disabled", true);

      builder.AddAttribute(seq++, "onchange",
        EventCallback.Factory.Create<ChangeEventArgs>(
          GetBlazorReceiver(),
          (ChangeEventArgs e) => {
            if (_autoCheck)
              Checked = true;
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
      base.AddEventAttributes(builder, ref seq);
    }

    public void PerformClick() {
      Checked = true;
      OnClick(EventArgs.Empty);
    }
  }
}
