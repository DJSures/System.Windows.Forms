// =============================================================================
// Synthiam.Web.Forms - MenuStrip for Blazor
// =============================================================================

using System;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // MenuStrip (partial - extends Form.cs stub)
  // ---------------------------------------------------------------------------
  public partial class MenuStrip : ToolStrip {

    private ToolStripMenuItem _mdiWindowListItem;

    public MenuStrip() {
      Dock = DockStyle.Top;
    }

    public ToolStripMenuItem MdiWindowListItem {
      get => _mdiWindowListItem;
      set => _mdiWindowListItem = value;
    }

    protected override string GetCssClasses() {
      return "swf-control swf-menustrip";
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "background:#f0f0f0;border-bottom:1px solid #ccc;";
      return style;
    }
  }
}
