// =============================================================================
// Synthiam.Web.Forms - Panel control for Blazor
// =============================================================================

using System;
using System.Drawing;
using System.Text;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // AutoSizeMode enum
  // ---------------------------------------------------------------------------
  public enum AutoSizeMode {
    GrowAndShrink = 0,
    GrowOnly = 1
  }

  // ---------------------------------------------------------------------------
  // Panel
  // ---------------------------------------------------------------------------
  public class Panel : ScrollableControl {

    private BorderStyle _borderStyle = BorderStyle.None;
    private AutoSizeMode _autoSizeMode = AutoSizeMode.GrowOnly;

    public Panel() { }

    public BorderStyle BorderStyle {
      get => _borderStyle;
      set {
        if (_borderStyle != value) {
          _borderStyle = value;
          InvalidateCssStyle();
          NotifyStateChanged();
        }
      }
    }

    public AutoSizeMode AutoSizeMode {
      get => _autoSizeMode;
      set {
        if (_autoSizeMode != value) {
          _autoSizeMode = value;
          NotifyStateChanged();
        }
      }
    }

    protected override string GetCssClasses() {
      return "swf-control swf-panel";
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
