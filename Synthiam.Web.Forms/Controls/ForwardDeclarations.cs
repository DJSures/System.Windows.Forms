// =============================================================================
// Synthiam.Web.Forms - Forward declarations for types referenced by EventArgs
// These are minimal stubs that will be replaced with full implementations later.
// =============================================================================

using System.Drawing;

namespace System.Windows.Forms {

  // -------------------------------------------------------------------------
  // Base Control class (minimal stub)
  // -------------------------------------------------------------------------
  public partial class Control {
  }

  // -------------------------------------------------------------------------
  // TreeNode (minimal stub)
  // -------------------------------------------------------------------------
  public partial class TreeNode {
  }

  // -------------------------------------------------------------------------
  // ListViewItem (minimal stub)
  // -------------------------------------------------------------------------
  public partial class ListViewItem {
  }

  // -------------------------------------------------------------------------
  // ToolStripItem (minimal stub)
  // -------------------------------------------------------------------------
  public partial class ToolStripItem {
  }

  // -------------------------------------------------------------------------
  // TabPage (minimal stub)
  // -------------------------------------------------------------------------
  public partial class TabPage {
  }

  // -------------------------------------------------------------------------
  // LinkLabel with nested Link class (minimal stub)
  // -------------------------------------------------------------------------
  public partial class LinkLabel {

    public class Link {

      public object LinkData { get; set; }
      public int Start { get; set; }
      public int Length { get; set; }
      public bool Enabled { get; set; } = true;
      public bool Visited { get; set; }
    }
  }

  // -------------------------------------------------------------------------
  // DataGridViewCellStyle (minimal stub)
  // -------------------------------------------------------------------------
  public partial class DataGridViewCellStyle {
  }
}
