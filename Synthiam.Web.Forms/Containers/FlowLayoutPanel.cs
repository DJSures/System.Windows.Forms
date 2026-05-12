// =============================================================================
// Synthiam.Web.Forms - FlowLayoutPanel control for Blazor
// =============================================================================

using System;
using System.Drawing;
using System.Text;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // FlowLayoutPanel
  // ---------------------------------------------------------------------------
  public class FlowLayoutPanel : Panel {

    private FlowDirection _flowDirection = FlowDirection.LeftToRight;
    private bool _wrapContents = true;

    public FlowLayoutPanel() { }

    internal override bool ManagesChildLayout => true;

    public FlowDirection FlowDirection {
      get => _flowDirection;
      set {
        if (_flowDirection != value) {
          _flowDirection = value;
          InvalidateCssStyle();
          NotifyStateChanged();
        }
      }
    }

    public bool WrapContents {
      get => _wrapContents;
      set {
        if (_wrapContents != value) {
          _wrapContents = value;
          InvalidateCssStyle();
          NotifyStateChanged();
        }
      }
    }

    protected override string GetCssClasses() {
      return "swf-control swf-flowlayoutpanel";
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();

      style += "display:flex;";

      switch (_flowDirection) {
        case FlowDirection.LeftToRight:
          style += "flex-direction:row;";
          break;
        case FlowDirection.RightToLeft:
          style += "flex-direction:row-reverse;";
          break;
        case FlowDirection.TopDown:
          style += "flex-direction:column;";
          break;
        case FlowDirection.BottomUp:
          style += "flex-direction:column-reverse;";
          break;
      }

      style += _wrapContents ? "flex-wrap:wrap;" : "flex-wrap:nowrap;";

      return style;
    }

    protected internal override void RenderChildren(RenderTreeBuilder builder, ref int seq) {
      // Each child is wrapped in a flex-item div that provides sizing and spacing.
      // The child itself uses position:relative;width:100%;height:100% (via ManagesChildLayout)
      // to fill this wrapper, so there's no absolute positioning conflict with flex.
      for (int i = 0; i < Controls.Count; i++) {
        var child = Controls[i];
        if (!child.Visible) continue;

        builder.OpenElement(seq++, "div");
        builder.SetKey(child);

        var sb = new StringBuilder(96);
        sb.Append("position:relative;flex-shrink:0;overflow:hidden;");
        if (child.Size.Width > 0)
          sb.Append("width:").Append(child.Size.Width).Append("px;");
        if (child.Size.Height > 0)
          sb.Append("height:").Append(child.Size.Height).Append("px;");
        if (child.Margin != Padding.Empty)
          sb.Append("margin:").Append(child.Margin.Top).Append("px ")
            .Append(child.Margin.Right).Append("px ")
            .Append(child.Margin.Bottom).Append("px ")
            .Append(child.Margin.Left).Append("px;");
        builder.AddAttribute(seq++, "style", sb.ToString());

        child.BuildRenderTree(builder, ref seq);
        builder.CloseElement();
      }
    }
  }
}
