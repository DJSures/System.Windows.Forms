// =============================================================================
// Synthiam.Web.Forms - TableLayoutPanel control for Blazor
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // Supporting types
  // ---------------------------------------------------------------------------

  public enum TableLayoutPanelGrowStyle {
    FixedSize = 0,
    AddRows = 1,
    AddColumns = 2
  }

  public struct TableLayoutPanelCellPosition : IEquatable<TableLayoutPanelCellPosition> {
    public int Column { get; set; }
    public int Row { get; set; }

    public TableLayoutPanelCellPosition(int column, int row) {
      Column = column;
      Row = row;
    }

    public bool Equals(TableLayoutPanelCellPosition other) => Column == other.Column && Row == other.Row;
    public override bool Equals(object obj) => obj is TableLayoutPanelCellPosition p && Equals(p);
    public override int GetHashCode() => Column ^ (Row << 16);
    public static bool operator ==(TableLayoutPanelCellPosition a, TableLayoutPanelCellPosition b) => a.Equals(b);
    public static bool operator !=(TableLayoutPanelCellPosition a, TableLayoutPanelCellPosition b) => !a.Equals(b);
    public override string ToString() => $"{Column},{Row}";
  }

  public abstract class TableLayoutStyle {
    public SizeType SizeType { get; set; } = SizeType.AutoSize;
  }

  public class ColumnStyle : TableLayoutStyle {
    private float _width;

    public ColumnStyle() { }
    public ColumnStyle(SizeType sizeType) { SizeType = sizeType; }
    public ColumnStyle(SizeType sizeType, float width) { SizeType = sizeType; _width = width; }

    public float Width {
      get => _width;
      set => _width = value;
    }
  }

  public class RowStyle : TableLayoutStyle {
    private float _height;

    public RowStyle() { }
    public RowStyle(SizeType sizeType) { SizeType = sizeType; }
    public RowStyle(SizeType sizeType, float height) { SizeType = sizeType; _height = height; }

    public float Height {
      get => _height;
      set => _height = value;
    }
  }

  public class TableLayoutColumnStyleCollection : List<ColumnStyle> {
    public TableLayoutColumnStyleCollection() { }
    public new void Add(ColumnStyle style) => base.Add(style);
  }

  public class TableLayoutRowStyleCollection : List<RowStyle> {
    public TableLayoutRowStyleCollection() { }
    public new void Add(RowStyle style) => base.Add(style);
  }

  // ---------------------------------------------------------------------------
  // TableLayoutPanel
  // ---------------------------------------------------------------------------
  public class TableLayoutPanel : Panel {

    private int _columnCount;
    private int _rowCount;
    private TableLayoutColumnStyleCollection _columnStyles = new TableLayoutColumnStyleCollection();
    private TableLayoutRowStyleCollection _rowStyles = new TableLayoutRowStyleCollection();
    private TableLayoutPanelCellBorderStyle _cellBorderStyle = TableLayoutPanelCellBorderStyle.None;
    private TableLayoutPanelGrowStyle _growStyle = TableLayoutPanelGrowStyle.AddRows;

    // Per-control cell assignment dictionaries
    private readonly Dictionary<Control, int> _columnSpans = new Dictionary<Control, int>();
    private readonly Dictionary<Control, int> _rowSpans = new Dictionary<Control, int>();
    private readonly Dictionary<Control, int> _columns = new Dictionary<Control, int>();
    private readonly Dictionary<Control, int> _rows = new Dictionary<Control, int>();

    public TableLayoutPanel() { }

    internal override bool ManagesChildLayout => true;

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public int ColumnCount {
      get => _columnCount;
      set { _columnCount = value; NotifyStateChanged(); }
    }

    public int RowCount {
      get => _rowCount;
      set { _rowCount = value; NotifyStateChanged(); }
    }

    public TableLayoutColumnStyleCollection ColumnStyles => _columnStyles;
    public TableLayoutRowStyleCollection RowStyles => _rowStyles;

    public TableLayoutPanelCellBorderStyle CellBorderStyle {
      get => _cellBorderStyle;
      set { _cellBorderStyle = value; NotifyStateChanged(); }
    }

    public TableLayoutPanelGrowStyle GrowStyle {
      get => _growStyle;
      set => _growStyle = value;
    }

    // ═══════════════════════════════════════════════
    // Cell layout methods
    // ═══════════════════════════════════════════════

    public int GetColumnSpan(Control control) => _columnSpans.TryGetValue(control, out var v) ? v : 1;
    public void SetColumnSpan(Control control, int value) { _columnSpans[control] = value; NotifyStateChanged(); }

    public int GetRowSpan(Control control) => _rowSpans.TryGetValue(control, out var v) ? v : 1;
    public void SetRowSpan(Control control, int value) { _rowSpans[control] = value; NotifyStateChanged(); }

    public int GetColumn(Control control) => _columns.TryGetValue(control, out var v) ? v : -1;
    public void SetColumn(Control control, int column) { _columns[control] = column; NotifyStateChanged(); }

    public int GetRow(Control control) => _rows.TryGetValue(control, out var v) ? v : -1;
    public void SetRow(Control control, int row) { _rows[control] = row; NotifyStateChanged(); }

    public Control GetControlFromPosition(int column, int row) {
      for (int i = 0; i < Controls.Count; i++) {
        var c = Controls[i];
        if (GetColumn(c) == column && GetRow(c) == row)
          return c;
      }
      return null;
    }

    public TableLayoutPanelCellPosition GetCellPosition(Control control) {
      return new TableLayoutPanelCellPosition(GetColumn(control), GetRow(control));
    }

    // ═══════════════════════════════════════════════
    // Rendering
    // ═══════════════════════════════════════════════

    protected override string GetCssClasses() {
      return "swf-control swf-tablelayoutpanel";
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "display:grid;";

      // grid-template-columns
      int cols = Math.Max(_columnCount, 1);
      var colTemplate = new StringBuilder();
      for (int i = 0; i < cols; i++) {
        if (i < _columnStyles.Count) {
          var cs = _columnStyles[i];
          switch (cs.SizeType) {
            case SizeType.Absolute:
              colTemplate.Append(cs.Width.ToString("0.##", CultureInfo.InvariantCulture)).Append("px ");
              break;
            case SizeType.Percent:
              colTemplate.Append(cs.Width.ToString("0.##", CultureInfo.InvariantCulture)).Append("% ");
              break;
            case SizeType.AutoSize:
              colTemplate.Append("auto ");
              break;
          }
        } else {
          colTemplate.Append("1fr ");
        }
      }
      style += "grid-template-columns:" + colTemplate.ToString().TrimEnd() + ";";

      // grid-template-rows
      int rows = Math.Max(_rowCount, 1);
      var rowTemplate = new StringBuilder();
      for (int i = 0; i < rows; i++) {
        if (i < _rowStyles.Count) {
          var rs = _rowStyles[i];
          switch (rs.SizeType) {
            case SizeType.Absolute:
              rowTemplate.Append(rs.Height.ToString("0.##", CultureInfo.InvariantCulture)).Append("px ");
              break;
            case SizeType.Percent:
              rowTemplate.Append(rs.Height.ToString("0.##", CultureInfo.InvariantCulture)).Append("% ");
              break;
            case SizeType.AutoSize:
              rowTemplate.Append("auto ");
              break;
          }
        } else {
          rowTemplate.Append("auto ");
        }
      }
      style += "grid-template-rows:" + rowTemplate.ToString().TrimEnd() + ";";

      // Cell border
      if (_cellBorderStyle != TableLayoutPanelCellBorderStyle.None)
        style += "gap:1px;";

      return style;
    }

    protected internal override void RenderChildren(RenderTreeBuilder builder, ref int seq) {
      // Place each child in its assigned grid cell
      int autoCol = 0, autoRow = 0;
      int cols = Math.Max(_columnCount, 1);

      for (int i = 0; i < Controls.Count; i++) {
        var child = Controls[i];
        if (!child.Visible) continue;

        int col = GetColumn(child);
        int row = GetRow(child);

        // Auto-assign if not explicitly set
        if (col < 0 || row < 0) {
          col = autoCol;
          row = autoRow;
          autoCol++;
          if (autoCol >= cols) {
            autoCol = 0;
            autoRow++;
          }
        }

        int colSpan = GetColumnSpan(child);
        int rowSpan = GetRowSpan(child);

        // Wrap child in a grid cell div
        builder.OpenElement(seq++, "div");

        var cellStyle = new StringBuilder();
        cellStyle.Append("grid-column:").Append(col + 1);
        if (colSpan > 1) cellStyle.Append("/span ").Append(colSpan);
        cellStyle.Append(";grid-row:").Append(row + 1);
        if (rowSpan > 1) cellStyle.Append("/span ").Append(rowSpan);
        cellStyle.Append(";position:relative;overflow:hidden;");

        if (_cellBorderStyle != TableLayoutPanelCellBorderStyle.None)
          cellStyle.Append("border:1px solid #ccc;");

        builder.AddAttribute(seq++, "style", cellStyle.ToString());

        child.BuildRenderTree(builder, ref seq);

        builder.CloseElement();

        // Advance auto position
        if (GetColumn(child) < 0) {
          // already advanced above
        }
      }
    }
  }
}
