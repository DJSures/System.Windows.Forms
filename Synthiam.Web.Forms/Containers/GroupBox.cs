// =============================================================================
// Synthiam.Web.Forms - GroupBox control for Blazor
// =============================================================================

using System;
using System.Drawing;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // GroupBox
  // ---------------------------------------------------------------------------
  public class GroupBox : Control {

    private FlatStyle _flatStyle = FlatStyle.Standard;

    public GroupBox() { }
    public GroupBox(string text) : this() { Text = text; }

    public FlatStyle FlatStyle {
      get => _flatStyle;
      set {
        if (_flatStyle != value) {
          _flatStyle = value;
          NotifyStateChanged();
        }
      }
    }

    protected override string GetHtmlTag() => "fieldset";

    protected override string GetCssClasses() {
      return "swf-control swf-groupbox";
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "border:1px solid #999;padding:4px;overflow:hidden;";
      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      if (!string.IsNullOrEmpty(Text)) {
        builder.OpenElement(seq++, "legend");
        builder.AddAttribute(seq++, "class", "swf-groupbox-legend");
        builder.AddContent(seq++, Text);
        builder.CloseElement();
      }
    }
  }
}
