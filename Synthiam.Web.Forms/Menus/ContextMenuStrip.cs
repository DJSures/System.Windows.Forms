// =============================================================================
// Synthiam.Web.Forms - ContextMenuStrip for Blazor
// =============================================================================

using System;
using System.Drawing;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // ContextMenuStrip (partial - extends Form.cs stub)
  // ---------------------------------------------------------------------------
  public partial class ContextMenuStrip : ToolStripDropDown {

    private Control _sourceControl;

    public ContextMenuStrip() { }
    public ContextMenuStrip(System.ComponentModel.IContainer container) : this() { }

    public Control SourceControl {
      get => _sourceControl;
      internal set => _sourceControl = value;
    }

    public System.Drawing.Size ImageScalingSize { get; set; } = new System.Drawing.Size(16, 16);

    public void Show(Control control, Point pos) {
      _sourceControl = control;
      var screenPt = control.PointToScreen(pos);
      Show(screenPt);
    }

    public void Show(Control control, int x, int y) {
      Show(control, new Point(x, y));
    }

    public new void Show(Point screenLocation) {
      base.Show(screenLocation);
    }

    protected override string GetCssClasses() {
      return "swf-control swf-contextmenustrip";
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "position:fixed;z-index:10000;background:#fff;border:1px solid #999;box-shadow:2px 2px 6px rgba(0,0,0,0.3);min-width:120px;";
      return style;
    }
  }
}
