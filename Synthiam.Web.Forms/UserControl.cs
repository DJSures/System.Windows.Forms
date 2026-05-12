// =============================================================================
// Synthiam.Web.Forms - UserControl for Blazor
// =============================================================================

using System;
using System.Drawing;

namespace System.Windows.Forms {

  public class UserControl : ContainerControl {

    private BorderStyle _borderStyle = BorderStyle.None;

    public BorderStyle BorderStyle {
      get => _borderStyle;
      set {
        _borderStyle = value;
        NotifyStateChanged();
      }
    }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event EventHandler Load;

    protected virtual void OnLoad(EventArgs e) {
      Load?.Invoke(this, e);
    }

    // ═══════════════════════════════════════════════
    // HTML rendering
    // ═══════════════════════════════════════════════

    protected override string GetCssClasses() {
      var css = base.GetCssClasses();
      switch (_borderStyle) {
        case BorderStyle.FixedSingle:
          css += " swf-border-single";
          break;
        case BorderStyle.Fixed3D:
          css += " swf-border-3d";
          break;
      }
      return css;
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
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
  }
}
