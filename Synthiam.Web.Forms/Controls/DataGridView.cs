// =============================================================================
// Synthiam.Web.Forms - DataGridView control for Blazor
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Synthiam.Web.Forms;
using BlazorMouseEventArgs = Microsoft.AspNetCore.Components.Web.MouseEventArgs;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // DataGridView Enums
  // ---------------------------------------------------------------------------
  public enum DataGridViewAutoSizeRowsMode {
    None = 0,
    AllHeaders = 1,
    DisplayedHeaders = 2,
    AllCellsExceptHeaders = 4,
    AllCells = 6,
    DisplayedCellsExceptHeaders = 8,
    DisplayedCells = 10
  }

  public enum DataGridViewClipboardCopyMode {
    Disable = 0,
    EnableWithAutoHeaderText = 1,
    EnableWithoutHeaderText = 2,
    EnableAlwaysIncludeHeaderText = 3
  }

  public enum DataGridViewCellBorderStyle {
    Custom = 0,
    Single = 1,
    Raised = 2,
    Sunken = 3,
    None = 4,
    SingleVertical = 5,
    RaisedVertical = 6,
    SunkenVertical = 7,
    SingleHorizontal = 8,
    RaisedHorizontal = 9,
    SunkenHorizontal = 10
  }

  public enum DataGridViewHeaderBorderStyle {
    Custom = 0,
    Single = 1,
    Raised = 2,
    Sunken = 3,
    None = 4
  }

  public enum DataGridViewRowHeadersWidthSizeMode {
    EnableResizing = 0,
    DisableResizing = 1,
    AutoSizeToAllHeaders = 2,
    AutoSizeToDisplayedHeaders = 3,
    AutoSizeToFirstHeader = 4
  }

  public enum DataGridViewColumnHeadersHeightSizeMode {
    EnableResizing = 0,
    DisableResizing = 1,
    AutoSize = 2
  }

  public enum DataGridViewTriState {
    NotSet = 0,
    True = 1,
    False = 2
  }

  public enum DataGridViewCellStyleScopes {
    None = 0,
    Cell = 1,
    Column = 2,
    Row = 4,
    DataGridView = 8,
    ColumnHeaders = 16,
    RowHeaders = 32,
    Rows = 64,
    AlternatingRows = 128
  }

  // NOTE: ListSortDirection is provided by System.ComponentModel, not duplicated here

  // ---------------------------------------------------------------------------
  // IDataGridViewEditingControl
  // ---------------------------------------------------------------------------
  public interface IDataGridViewEditingControl {
    DataGridView EditingControlDataGridView { get; set; }
    object EditingControlFormattedValue { get; set; }
    int EditingControlRowIndex { get; set; }
    bool EditingControlValueChanged { get; set; }
    Cursor EditingPanelCursor { get; }
    bool RepositionEditingControlOnValueChange { get; }
    void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle);
    bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey);
    object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context);
    void PrepareEditingControlForEdit(bool selectAll);
  }

  // ---------------------------------------------------------------------------
  // DataGridViewTextBoxEditingControl
  // ---------------------------------------------------------------------------
  public class DataGridViewTextBoxEditingControl : TextBox, IDataGridViewEditingControl {
    public DataGridView EditingControlDataGridView { get; set; }
    public object EditingControlFormattedValue { get; set; }
    public int EditingControlRowIndex { get; set; }
    public bool EditingControlValueChanged { get; set; }
    public Cursor EditingPanelCursor => Cursors.Default;
    public bool RepositionEditingControlOnValueChange => false;
    public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle) { }
    public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey) => false;
    public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) => Text;
    public void PrepareEditingControlForEdit(bool selectAll) { }
  }

  // ---------------------------------------------------------------------------
  // DataGridViewCellStyle (extends ForwardDeclarations partial)
  // ---------------------------------------------------------------------------
  public partial class DataGridViewCellStyle : ICloneable {

    public Color BackColor { get; set; } = Color.Empty;
    public Color ForeColor { get; set; } = Color.Empty;
    public Font Font { get; set; }
    public Color SelectionBackColor { get; set; } = Color.Empty;
    public Color SelectionForeColor { get; set; } = Color.Empty;
    public string Format { get; set; } = string.Empty;
    public DataGridViewContentAlignment Alignment { get; set; } = DataGridViewContentAlignment.NotSet;
    public DataGridViewTriState WrapMode { get; set; } = DataGridViewTriState.NotSet;
    public object NullValue { get; set; }
    public object Tag { get; set; }
    public Padding Padding { get; set; }
    public object DataSourceNullValue { get; set; }

    public object Clone() {
      return new DataGridViewCellStyle {
        BackColor = BackColor,
        ForeColor = ForeColor,
        Font = Font,
        SelectionBackColor = SelectionBackColor,
        SelectionForeColor = SelectionForeColor,
        Format = Format,
        Alignment = Alignment,
        WrapMode = WrapMode,
        NullValue = NullValue,
        Tag = Tag,
        Padding = Padding,
        DataSourceNullValue = DataSourceNullValue
      };
    }
  }

  // ---------------------------------------------------------------------------
  // DataGridViewCell
  // ---------------------------------------------------------------------------
  public class DataGridViewCell {

    public object Value { get; set; }

    public object FormattedValue {
      get {
        if (Value == null) return OwningColumn?.DefaultCellStyle?.NullValue ?? string.Empty;
        return Value;
      }
    }

    public object EditedFormattedValue => FormattedValue;

    public int ColumnIndex { get; internal set; } = -1;
    public int RowIndex { get; internal set; } = -1;
    public DataGridViewColumn OwningColumn { get; internal set; }
    public DataGridViewRow OwningRow { get; internal set; }
    public bool Selected { get; set; }
    public bool ReadOnly { get; set; }
    public bool Visible { get; set; } = true;
    public DataGridViewCellStyle Style { get; set; } = new DataGridViewCellStyle();
    public object Tag { get; set; }
    public string ToolTipText { get; set; } = string.Empty;
    public string ErrorText { get; set; } = string.Empty;
    public Type FormattedValueType => Value?.GetType() ?? typeof(string);
    public virtual Type ValueType { get; set; }
    public virtual Type EditType => typeof(DataGridViewTextBoxEditingControl);
    public virtual object DefaultNewRowValue => null;
    public Rectangle ContentBounds => Rectangle.Empty;

    public virtual object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter formattedValueTypeConverter, System.ComponentModel.TypeConverter valueTypeConverter) {
      return formattedValue;
    }

    protected virtual object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter valueTypeConverter, System.ComponentModel.TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context) {
      return value;
    }

    public virtual void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle) {
    }

    public override string ToString() => Value?.ToString() ?? string.Empty;
  }

  public class DataGridViewTextBoxCell : DataGridViewCell { }
  public class DataGridViewButtonCell : DataGridViewCell { }
  public class DataGridViewCheckBoxCell : DataGridViewCell { }
  public class DataGridViewComboBoxCell : DataGridViewCell { }
  public class DataGridViewImageCell : DataGridViewCell { }
  public class DataGridViewLinkCell : DataGridViewCell { }

  // ---------------------------------------------------------------------------
  // DataGridViewCellCollection
  // ---------------------------------------------------------------------------
  public class DataGridViewCellCollection : IList<DataGridViewCell>, IList {

    private readonly List<DataGridViewCell> _list = new List<DataGridViewCell>();
    private readonly DataGridViewRow _owner;

    internal DataGridViewCellCollection(DataGridViewRow owner) { _owner = owner; }

    public int Count => _list.Count;
    public bool IsReadOnly => false;
    public bool IsFixedSize => false;
    public bool IsSynchronized => false;
    public object SyncRoot => _list;

    public DataGridViewCell this[int index] {
      get => _list[index];
      set {
        value.ColumnIndex = index;
        value.OwningRow = _owner;
        _list[index] = value;
      }
    }

    public DataGridViewCell this[string columnName] {
      get {
        if (_owner?.DataGridView != null) {
          for (int i = 0; i < _owner.DataGridView.Columns.Count; i++) {
            if (string.Equals(_owner.DataGridView.Columns[i].Name, columnName, StringComparison.OrdinalIgnoreCase))
              return (i < _list.Count) ? _list[i] : null;
          }
        }
        return null;
      }
    }

    object IList.this[int index] {
      get => _list[index];
      set => this[index] = (DataGridViewCell)value;
    }

    public void Add(DataGridViewCell cell) {
      cell.ColumnIndex = _list.Count;
      cell.OwningRow = _owner;
      _list.Add(cell);
    }

    public void AddRange(params DataGridViewCell[] cells) {
      if (cells == null) return;
      foreach (var cell in cells) Add(cell);
    }

    internal void EnsureSize(int count) {
      while (_list.Count < count) {
        var cell = new DataGridViewTextBoxCell();
        cell.ColumnIndex = _list.Count;
        cell.OwningRow = _owner;
        _list.Add(cell);
      }
    }

    public void Clear() => _list.Clear();
    public bool Contains(DataGridViewCell item) => _list.Contains(item);
    public void CopyTo(DataGridViewCell[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
    public int IndexOf(DataGridViewCell item) => _list.IndexOf(item);
    public void Insert(int index, DataGridViewCell item) { item.ColumnIndex = index; item.OwningRow = _owner; _list.Insert(index, item); }
    public bool Remove(DataGridViewCell item) => _list.Remove(item);
    public void RemoveAt(int index) => _list.RemoveAt(index);
    public IEnumerator<DataGridViewCell> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

    int IList.Add(object value) { Add((DataGridViewCell)value); return _list.Count - 1; }
    bool IList.Contains(object value) => _list.Contains((DataGridViewCell)value);
    int IList.IndexOf(object value) => _list.IndexOf((DataGridViewCell)value);
    void IList.Insert(int index, object value) => Insert(index, (DataGridViewCell)value);
    void IList.Remove(object value) => Remove((DataGridViewCell)value);
    void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);
  }

  // ---------------------------------------------------------------------------
  // DataGridViewRow
  // ---------------------------------------------------------------------------
  public class DataGridViewRow {

    private DataGridViewCellCollection _cells;

    public DataGridViewRow() {
      _cells = new DataGridViewCellCollection(this);
    }

    public DataGridViewCellCollection Cells => _cells;
    public int Height { get; set; } = 22;
    public int MinimumHeight { get; set; } = 3;
    public bool Selected { get; set; }
    public bool ReadOnly { get; set; }
    public bool Visible { get; set; } = true;
    public bool Frozen { get; set; }
    public DataGridViewCellStyle DefaultCellStyle { get; set; } = new DataGridViewCellStyle();
    public int Index { get; internal set; } = -1;
    public DataGridView DataGridView { get; internal set; }
    public object DataBoundItem { get; set; }
    public object Tag { get; set; }
    public bool IsNewRow { get; set; }
    public DataGridViewCell HeaderCell { get; set; }
    public int DividerHeight { get; set; }
    public string ErrorText { get; set; } = string.Empty;

    public DataGridViewRow Clone() {
      var clone = new DataGridViewRow();
      clone.Height = Height;
      clone.ReadOnly = ReadOnly;
      clone.DefaultCellStyle = (DataGridViewCellStyle)DefaultCellStyle.Clone();
      return clone;
    }

    internal void EnsureCellCount(int count) {
      _cells.EnsureSize(count);
    }
  }

  // ---------------------------------------------------------------------------
  // DataGridViewColumn
  // ---------------------------------------------------------------------------
  public class DataGridViewColumn {

    public DataGridViewColumn() { }

    public DataGridViewColumn(DataGridViewCell cellTemplate) {
      CellTemplate = cellTemplate;
    }

    public string Name { get; set; } = string.Empty;
    public string HeaderText { get; set; } = string.Empty;
    public string DataPropertyName { get; set; } = string.Empty;
    public Type ValueType { get; set; }
    public virtual DataGridViewCell CellTemplate { get; set; }
    public int Width { get; set; } = 100;
    public int MinimumWidth { get; set; } = 5;
    public float FillWeight { get; set; } = 100f;
    public DataGridViewAutoSizeColumnMode AutoSizeMode { get; set; } = DataGridViewAutoSizeColumnMode.NotSet;
    public DataGridViewColumnSortMode SortMode { get; set; } = DataGridViewColumnSortMode.NotSortable;
    public bool ReadOnly { get; set; }
    public bool Visible { get; set; } = true;
    public bool Frozen { get; set; }
    public DataGridViewTriState Resizable { get; set; } = DataGridViewTriState.True;
    public int DividerWidth { get; set; }
    public DataGridViewCellStyle DefaultCellStyle { get; set; } = new DataGridViewCellStyle();
    public DataGridViewCell HeaderCell { get; set; }
    public int DisplayIndex { get; set; }
    public int Index { get; internal set; } = -1;
    public DataGridView DataGridView { get; internal set; }
    public object Tag { get; set; }

    public override string ToString() => HeaderText ?? Name ?? "DataGridViewColumn";
  }

  // Column subtypes
  public class DataGridViewTextBoxColumn : DataGridViewColumn {
    public DataGridViewTextBoxColumn() { CellTemplate = new DataGridViewTextBoxCell(); }
    public int MaxInputLength { get; set; } = 32767;
  }

  public class DataGridViewButtonColumn : DataGridViewColumn {
    public DataGridViewButtonColumn() { CellTemplate = new DataGridViewButtonCell(); }
    public string Text { get; set; } = string.Empty;
    public bool UseColumnTextForButtonValue { get; set; }
    public FlatStyle FlatStyle { get; set; } = FlatStyle.Standard;
  }

  public class DataGridViewCheckBoxColumn : DataGridViewColumn {
    public DataGridViewCheckBoxColumn() { CellTemplate = new DataGridViewCheckBoxCell(); }
    public bool ThreeState { get; set; }
    public object FalseValue { get; set; }
    public object TrueValue { get; set; }
    public object IndeterminateValue { get; set; }
  }

  public class DataGridViewComboBoxColumn : DataGridViewColumn {
    public DataGridViewComboBoxColumn() { CellTemplate = new DataGridViewComboBoxCell(); }
    private readonly List<object> _items = new List<object>();
    public List<object> Items => _items;
    public object DataSource { get; set; }
    public string DisplayMember { get; set; } = string.Empty;
    public string ValueMember { get; set; } = string.Empty;
    public int DropDownWidth { get; set; }
    public FlatStyle FlatStyle { get; set; } = FlatStyle.Standard;
    public DataGridViewComboBoxDisplayStyle DisplayStyle { get; set; } = DataGridViewComboBoxDisplayStyle.DropDownButton;
  }

  public enum DataGridViewComboBoxDisplayStyle {
    ComboBox = 0,
    DropDownButton = 1,
    Nothing = 2
  }

  public class DataGridViewImageColumn : DataGridViewColumn {
    public DataGridViewImageColumn() { CellTemplate = new DataGridViewImageCell(); }
    public Image Image { get; set; }
    public DataGridViewImageCellLayout ImageLayout { get; set; } = DataGridViewImageCellLayout.Normal;
    public bool ValuesAreIcons { get; set; }
  }

  public enum DataGridViewImageCellLayout {
    NotSet = 0,
    Normal = 1,
    Stretch = 2,
    Zoom = 3
  }

  public class DataGridViewLinkColumn : DataGridViewColumn {
    public DataGridViewLinkColumn() { CellTemplate = new DataGridViewLinkCell(); }
    public string Text { get; set; } = string.Empty;
    public bool UseColumnTextForLinkValue { get; set; }
    public LinkBehavior LinkBehavior { get; set; } = LinkBehavior.SystemDefault;
    public Color LinkColor { get; set; } = Color.FromArgb(0, 0, 255);
    public Color VisitedLinkColor { get; set; } = Color.FromArgb(128, 0, 128);
    public Color ActiveLinkColor { get; set; } = Color.Red;
    public bool TrackVisitedState { get; set; } = true;
  }

  // ---------------------------------------------------------------------------
  // DataGridViewColumnCollection
  // ---------------------------------------------------------------------------
  public class DataGridViewColumnCollection : IList<DataGridViewColumn>, IList {

    private readonly List<DataGridViewColumn> _list = new List<DataGridViewColumn>();
    private readonly DataGridView _owner;

    internal DataGridViewColumnCollection(DataGridView owner) { _owner = owner; }

    public int Count => _list.Count;
    public bool IsReadOnly => false;
    public bool IsFixedSize => false;
    public bool IsSynchronized => false;
    public object SyncRoot => _list;

    public DataGridViewColumn this[int index] {
      get => _list[index];
      set {
        value.Index = index;
        value.DataGridView = _owner;
        _list[index] = value;
      }
    }

    public DataGridViewColumn this[string name] {
      get {
        foreach (var col in _list)
          if (string.Equals(col.Name, name, StringComparison.OrdinalIgnoreCase)) return col;
        return null;
      }
    }

    object IList.this[int index] {
      get => _list[index];
      set => this[index] = (DataGridViewColumn)value;
    }

    public int Add(DataGridViewColumn column) {
      column.Index = _list.Count;
      column.DataGridView = _owner;
      column.DisplayIndex = _list.Count;
      _list.Add(column);
      // Ensure all existing rows have enough cells
      foreach (DataGridViewRow row in _owner.Rows)
        row.EnsureCellCount(_list.Count);
      _owner.NotifyStateChanged();
      return _list.Count - 1;
    }

    public int Add(string columnName, string headerText) {
      var col = new DataGridViewTextBoxColumn { Name = columnName, HeaderText = headerText };
      return Add(col);
    }

    public void AddRange(params DataGridViewColumn[] columns) {
      foreach (var col in columns) Add(col);
    }

    public void Clear() {
      _list.Clear();
      _owner.NotifyStateChanged();
    }

    public bool Contains(DataGridViewColumn item) => _list.Contains(item);
    public void CopyTo(DataGridViewColumn[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
    public int IndexOf(DataGridViewColumn item) => _list.IndexOf(item);
    public int GetColumnCount(DataGridViewElementStates includeFilter) => _list.Count;

    public void Insert(int index, DataGridViewColumn column) {
      column.Index = index;
      column.DataGridView = _owner;
      _list.Insert(index, column);
      for (int i = index; i < _list.Count; i++) _list[i].Index = i;
      _owner.NotifyStateChanged();
    }

    public bool Remove(DataGridViewColumn item) {
      bool r = _list.Remove(item);
      for (int i = 0; i < _list.Count; i++) _list[i].Index = i;
      if (r) _owner.NotifyStateChanged();
      return r;
    }

    public void RemoveAt(int index) {
      _list.RemoveAt(index);
      for (int i = index; i < _list.Count; i++) _list[i].Index = i;
      _owner.NotifyStateChanged();
    }

    public IEnumerator<DataGridViewColumn> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

    int IList.Add(object value) => Add((DataGridViewColumn)value);
    bool IList.Contains(object value) => _list.Contains((DataGridViewColumn)value);
    int IList.IndexOf(object value) => _list.IndexOf((DataGridViewColumn)value);
    void IList.Insert(int index, object value) => Insert(index, (DataGridViewColumn)value);
    void IList.Remove(object value) => Remove((DataGridViewColumn)value);
    void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);

    void ICollection<DataGridViewColumn>.Add(DataGridViewColumn item) => Add(item);
  }

  public enum DataGridViewElementStates {
    None = 0,
    Displayed = 1,
    Frozen = 2,
    ReadOnly = 4,
    Resizable = 8,
    ResizableSet = 16,
    Selected = 32,
    Visible = 64
  }

  // ---------------------------------------------------------------------------
  // DataGridViewRowCollection
  // ---------------------------------------------------------------------------
  public class DataGridViewRowCollection : IList<DataGridViewRow>, IList {

    private readonly List<DataGridViewRow> _list = new List<DataGridViewRow>();
    private readonly DataGridView _owner;

    internal DataGridViewRowCollection(DataGridView owner) { _owner = owner; }

    public int Count => _list.Count;
    public bool IsReadOnly => false;
    public bool IsFixedSize => false;
    public bool IsSynchronized => false;
    public object SyncRoot => _list;

    public DataGridViewRow this[int index] {
      get => _list[index];
      set {
        value.Index = index;
        value.DataGridView = _owner;
        _list[index] = value;
      }
    }

    object IList.this[int index] {
      get => _list[index];
      set => this[index] = (DataGridViewRow)value;
    }

    public int Add() {
      var row = new DataGridViewRow();
      return Add(row);
    }

    public int Add(DataGridViewRow row) {
      row.Index = _list.Count;
      row.DataGridView = _owner;
      row.EnsureCellCount(_owner.Columns.Count);
      // Set cell ownership
      for (int c = 0; c < row.Cells.Count && c < _owner.Columns.Count; c++) {
        row.Cells[c].OwningColumn = _owner.Columns[c];
        row.Cells[c].RowIndex = row.Index;
      }
      _list.Add(row);
      _owner.NotifyStateChanged();
      return _list.Count - 1;
    }

    public int Add(int count) {
      int first = _list.Count;
      for (int i = 0; i < count; i++) Add();
      return first;
    }

    public int Add(params object[] values) {
      var row = new DataGridViewRow();
      row.EnsureCellCount(values.Length);
      for (int i = 0; i < values.Length; i++)
        row.Cells[i].Value = values[i];
      return Add(row);
    }

    public void AddRange(params DataGridViewRow[] rows) {
      foreach (var row in rows) Add(row);
    }

    public void Clear() {
      _list.Clear();
      _owner.NotifyStateChanged();
    }

    public bool Contains(DataGridViewRow item) => _list.Contains(item);
    public void CopyTo(DataGridViewRow[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
    public int IndexOf(DataGridViewRow item) => _list.IndexOf(item);

    public void Insert(int index, DataGridViewRow row) {
      row.Index = index;
      row.DataGridView = _owner;
      row.EnsureCellCount(_owner.Columns.Count);
      _list.Insert(index, row);
      for (int i = index; i < _list.Count; i++) _list[i].Index = i;
      _owner.NotifyStateChanged();
    }

    public void Insert(int index, params object[] values) {
      var row = new DataGridViewRow();
      row.EnsureCellCount(values.Length);
      for (int i = 0; i < values.Length; i++)
        row.Cells[i].Value = values[i];
      Insert(index, row);
    }

    public bool Remove(DataGridViewRow item) {
      bool r = _list.Remove(item);
      for (int i = 0; i < _list.Count; i++) _list[i].Index = i;
      if (r) _owner.NotifyStateChanged();
      return r;
    }

    public void RemoveAt(int index) {
      _list.RemoveAt(index);
      for (int i = index; i < _list.Count; i++) _list[i].Index = i;
      _owner.NotifyStateChanged();
    }

    public int GetRowCount(DataGridViewElementStates includeFilter) => _list.Count;
    public int GetFirstRow(DataGridViewElementStates includeFilter) => _list.Count > 0 ? 0 : -1;
    public int GetLastRow(DataGridViewElementStates includeFilter) => _list.Count > 0 ? _list.Count - 1 : -1;

    public IEnumerator<DataGridViewRow> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

    int IList.Add(object value) => Add((DataGridViewRow)value);
    bool IList.Contains(object value) => _list.Contains((DataGridViewRow)value);
    int IList.IndexOf(object value) => _list.IndexOf((DataGridViewRow)value);
    void IList.Insert(int index, object value) => Insert(index, (DataGridViewRow)value);
    void IList.Remove(object value) => Remove((DataGridViewRow)value);
    void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);

    void ICollection<DataGridViewRow>.Add(DataGridViewRow item) => Add(item);
  }

  // ---------------------------------------------------------------------------
  // Selected collections
  // ---------------------------------------------------------------------------
  public class DataGridViewSelectedCellCollection : IList<DataGridViewCell>, IList {
    private readonly List<DataGridViewCell> _list = new List<DataGridViewCell>();
    internal void AddInternal(DataGridViewCell c) => _list.Add(c);
    public int Count => _list.Count;
    public bool IsReadOnly => true;
    public bool IsFixedSize => true;
    public bool IsSynchronized => false;
    public object SyncRoot => _list;
    public DataGridViewCell this[int index] { get => _list[index]; set { } }
    object IList.this[int index] { get => _list[index]; set { } }
    public bool Contains(DataGridViewCell item) => _list.Contains(item);
    public void CopyTo(DataGridViewCell[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
    public int IndexOf(DataGridViewCell item) => _list.IndexOf(item);
    public IEnumerator<DataGridViewCell> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    public void Add(DataGridViewCell item) { } public void Clear() { } public void Insert(int index, DataGridViewCell item) { } public bool Remove(DataGridViewCell item) => false; public void RemoveAt(int index) { }
    int IList.Add(object value) => -1; bool IList.Contains(object value) => _list.Contains((DataGridViewCell)value); int IList.IndexOf(object value) => _list.IndexOf((DataGridViewCell)value);
    void IList.Insert(int index, object value) { } void IList.Remove(object value) { } void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);
  }

  public class DataGridViewSelectedRowCollection : IList<DataGridViewRow>, IList {
    private readonly List<DataGridViewRow> _list = new List<DataGridViewRow>();
    internal void AddInternal(DataGridViewRow r) => _list.Add(r);
    public int Count => _list.Count;
    public bool IsReadOnly => true;
    public bool IsFixedSize => true;
    public bool IsSynchronized => false;
    public object SyncRoot => _list;
    public DataGridViewRow this[int index] { get => _list[index]; set { } }
    object IList.this[int index] { get => _list[index]; set { } }
    public bool Contains(DataGridViewRow item) => _list.Contains(item);
    public void CopyTo(DataGridViewRow[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
    public int IndexOf(DataGridViewRow item) => _list.IndexOf(item);
    public IEnumerator<DataGridViewRow> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    public void Add(DataGridViewRow item) { } public void Clear() { } public void Insert(int index, DataGridViewRow item) { } public bool Remove(DataGridViewRow item) => false; public void RemoveAt(int index) { }
    int IList.Add(object value) => -1; bool IList.Contains(object value) => _list.Contains((DataGridViewRow)value); int IList.IndexOf(object value) => _list.IndexOf((DataGridViewRow)value);
    void IList.Insert(int index, object value) { } void IList.Remove(object value) { } void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);
  }

  public class DataGridViewSelectedColumnCollection : IList<DataGridViewColumn>, IList {
    private readonly List<DataGridViewColumn> _list = new List<DataGridViewColumn>();
    internal void AddInternal(DataGridViewColumn c) => _list.Add(c);
    public int Count => _list.Count;
    public bool IsReadOnly => true;
    public bool IsFixedSize => true;
    public bool IsSynchronized => false;
    public object SyncRoot => _list;
    public DataGridViewColumn this[int index] { get => _list[index]; set { } }
    object IList.this[int index] { get => _list[index]; set { } }
    public bool Contains(DataGridViewColumn item) => _list.Contains(item);
    public void CopyTo(DataGridViewColumn[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
    public int IndexOf(DataGridViewColumn item) => _list.IndexOf(item);
    public IEnumerator<DataGridViewColumn> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    public void Add(DataGridViewColumn item) { } public void Clear() { } public void Insert(int index, DataGridViewColumn item) { } public bool Remove(DataGridViewColumn item) => false; public void RemoveAt(int index) { }
    int IList.Add(object value) => -1; bool IList.Contains(object value) => _list.Contains((DataGridViewColumn)value); int IList.IndexOf(object value) => _list.IndexOf((DataGridViewColumn)value);
    void IList.Insert(int index, object value) { } void IList.Remove(object value) { } void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);
  }

  // ---------------------------------------------------------------------------
  // DataGridView
  // ---------------------------------------------------------------------------
  public class DataGridView : Control {

    private DataGridViewColumnCollection _columns;
    private DataGridViewRowCollection _rows;
    private DataGridViewCell _currentCell;

    private object _dataSource;
    private string _dataMember = string.Empty;
    private bool _autoGenerateColumns = true;
    private bool _readOnly;
    private bool _allowUserToAddRows = true;
    private bool _allowUserToDeleteRows = true;
    private bool _allowUserToResizeRows = true;
    private bool _allowUserToResizeColumns = true;
    private bool _allowUserToOrderColumns;
    private bool _multiSelect = true;
    private DataGridViewSelectionMode _selectionMode = DataGridViewSelectionMode.RowHeaderSelect;
    private DataGridViewEditMode _editMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
    private bool _columnHeadersVisible = true;
    private bool _rowHeadersVisible = true;
    private int _columnHeadersHeight = 23;
    private DataGridViewColumnHeadersHeightSizeMode _columnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
    private int _rowHeadersWidth = 43;
    private DataGridViewRowHeadersWidthSizeMode _rowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
    private DataGridViewCellStyle _defaultCellStyle = new DataGridViewCellStyle();
    private DataGridViewCellStyle _alternatingRowsDefaultCellStyle = new DataGridViewCellStyle();
    private DataGridViewCellStyle _columnHeadersDefaultCellStyle = new DataGridViewCellStyle();
    private DataGridViewCellStyle _rowHeadersDefaultCellStyle = new DataGridViewCellStyle();
    private Color _gridColor = Color.FromArgb(204, 204, 204);
    private Color _backgroundColor = Color.Empty;
    private BorderStyle _borderStyle = BorderStyle.FixedSingle;
    private DataGridViewCellBorderStyle _cellBorderStyle = DataGridViewCellBorderStyle.Single;
    private DataGridViewHeaderBorderStyle _columnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
    private DataGridViewHeaderBorderStyle _rowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
    private ScrollBars _scrollBars = ScrollBars.Both;
    private DataGridViewAutoSizeColumnsMode _autoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
    private int _firstDisplayedScrollingRowIndex;
    private bool _showCellToolTips = true;
    private bool _showCellErrors = true;
    private bool _showEditingIcon = true;
    private bool _showRowErrors = true;
    private bool _enableHeadersVisualStyles = true;
    private DataGridViewAutoSizeRowsMode _autoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
    private DataGridViewClipboardCopyMode _clipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText;
    private bool _virtualMode;
    private DataGridViewRow _rowTemplate;
    private DataGridViewColumn _sortedColumn;
    private SortOrder _sortOrder = SortOrder.None;

    public DataGridView() {
      _columns = new DataGridViewColumnCollection(this);
      _rows = new DataGridViewRowCollection(this);
    }

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public DataGridViewColumnCollection Columns => _columns;
    public DataGridViewRowCollection Rows => _rows;

    public DataGridViewSelectedCellCollection SelectedCells {
      get {
        var coll = new DataGridViewSelectedCellCollection();
        foreach (DataGridViewRow row in _rows)
          foreach (DataGridViewCell cell in row.Cells)
            if (cell.Selected) coll.AddInternal(cell);
        return coll;
      }
    }

    public DataGridViewSelectedRowCollection SelectedRows {
      get {
        var coll = new DataGridViewSelectedRowCollection();
        foreach (DataGridViewRow row in _rows)
          if (row.Selected) coll.AddInternal(row);
        return coll;
      }
    }

    public DataGridViewSelectedColumnCollection SelectedColumns {
      get { return new DataGridViewSelectedColumnCollection(); }
    }

    public DataGridViewCell CurrentCell {
      get => _currentCell;
      set {
        _currentCell = value;
        OnCurrentCellChanged(EventArgs.Empty);
      }
    }

    public DataGridViewRow CurrentRow {
      get => _currentCell?.OwningRow;
    }

    public object DataSource {
      get => _dataSource;
      set {
        _dataSource = value;
        NotifyStateChanged();
      }
    }

    public string DataMember { get => _dataMember; set => _dataMember = value ?? string.Empty; }
    public bool AutoGenerateColumns { get => _autoGenerateColumns; set => _autoGenerateColumns = value; }
    public new bool ReadOnly { get => _readOnly; set => _readOnly = value; }
    public bool AllowUserToAddRows { get => _allowUserToAddRows; set { _allowUserToAddRows = value; NotifyStateChanged(); } }
    public bool AllowUserToDeleteRows { get => _allowUserToDeleteRows; set => _allowUserToDeleteRows = value; }
    public bool AllowUserToResizeRows { get => _allowUserToResizeRows; set => _allowUserToResizeRows = value; }
    public bool AllowUserToResizeColumns { get => _allowUserToResizeColumns; set => _allowUserToResizeColumns = value; }
    public bool AllowUserToOrderColumns { get => _allowUserToOrderColumns; set => _allowUserToOrderColumns = value; }
    public bool MultiSelect { get => _multiSelect; set => _multiSelect = value; }
    public DataGridViewSelectionMode SelectionMode { get => _selectionMode; set => _selectionMode = value; }
    public DataGridViewEditMode EditMode { get => _editMode; set => _editMode = value; }
    public bool ColumnHeadersVisible { get => _columnHeadersVisible; set { _columnHeadersVisible = value; NotifyStateChanged(); } }
    public bool RowHeadersVisible { get => _rowHeadersVisible; set { _rowHeadersVisible = value; NotifyStateChanged(); } }
    public int ColumnHeadersHeight { get => _columnHeadersHeight; set => _columnHeadersHeight = value; }
    public DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode { get => _columnHeadersHeightSizeMode; set => _columnHeadersHeightSizeMode = value; }
    public int RowHeadersWidth { get => _rowHeadersWidth; set => _rowHeadersWidth = value; }
    public DataGridViewRowHeadersWidthSizeMode RowHeadersWidthSizeMode { get => _rowHeadersWidthSizeMode; set => _rowHeadersWidthSizeMode = value; }
    public DataGridViewCellStyle DefaultCellStyle { get => _defaultCellStyle; set => _defaultCellStyle = value; }
    public DataGridViewCellStyle AlternatingRowsDefaultCellStyle { get => _alternatingRowsDefaultCellStyle; set => _alternatingRowsDefaultCellStyle = value; }
    public DataGridViewCellStyle ColumnHeadersDefaultCellStyle { get => _columnHeadersDefaultCellStyle; set => _columnHeadersDefaultCellStyle = value; }
    public DataGridViewCellStyle RowHeadersDefaultCellStyle { get => _rowHeadersDefaultCellStyle; set => _rowHeadersDefaultCellStyle = value; }
    public Color GridColor { get => _gridColor; set { _gridColor = value; NotifyStateChanged(); } }
    public Color BackgroundColor { get => _backgroundColor; set { _backgroundColor = value; NotifyStateChanged(); } }
    public BorderStyle BorderStyle { get => _borderStyle; set { _borderStyle = value; NotifyStateChanged(); } }
    public DataGridViewCellBorderStyle CellBorderStyle { get => _cellBorderStyle; set { _cellBorderStyle = value; NotifyStateChanged(); } }
    public DataGridViewHeaderBorderStyle ColumnHeadersBorderStyle { get => _columnHeadersBorderStyle; set => _columnHeadersBorderStyle = value; }
    public DataGridViewHeaderBorderStyle RowHeadersBorderStyle { get => _rowHeadersBorderStyle; set => _rowHeadersBorderStyle = value; }
    public ScrollBars ScrollBars { get => _scrollBars; set => _scrollBars = value; }
    public DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode { get => _autoSizeColumnsMode; set => _autoSizeColumnsMode = value; }

    public int ColumnCount {
      get => _columns.Count;
      set {
        while (_columns.Count < value) _columns.Add(new DataGridViewTextBoxColumn());
        while (_columns.Count > value) _columns.RemoveAt(_columns.Count - 1);
      }
    }

    public int RowCount {
      get => _rows.Count;
      set {
        while (_rows.Count < value) _rows.Add();
        while (_rows.Count > value) _rows.RemoveAt(_rows.Count - 1);
      }
    }

    public int FirstDisplayedScrollingRowIndex { get => _firstDisplayedScrollingRowIndex; set => _firstDisplayedScrollingRowIndex = value; }
    public bool ShowCellToolTips { get => _showCellToolTips; set => _showCellToolTips = value; }
    public bool ShowCellErrors { get => _showCellErrors; set => _showCellErrors = value; }
    public bool ShowEditingIcon { get => _showEditingIcon; set => _showEditingIcon = value; }
    public bool ShowRowErrors { get => _showRowErrors; set => _showRowErrors = value; }
    public bool EnableHeadersVisualStyles { get => _enableHeadersVisualStyles; set => _enableHeadersVisualStyles = value; }
    public DataGridViewAutoSizeRowsMode AutoSizeRowsMode { get => _autoSizeRowsMode; set => _autoSizeRowsMode = value; }
    public DataGridViewClipboardCopyMode ClipboardCopyMode { get => _clipboardCopyMode; set => _clipboardCopyMode = value; }
    public bool VirtualMode { get => _virtualMode; set => _virtualMode = value; }
    public DataGridViewRow RowTemplate { get => _rowTemplate; set => _rowTemplate = value; }
    public int NewRowIndex => _allowUserToAddRows ? _rows.Count : -1;
    public DataGridViewColumn SortedColumn => _sortedColumn;
    public SortOrder SortOrder => _sortOrder;

    public DataGridViewCell this[int columnIndex, int rowIndex] {
      get {
        if (rowIndex >= 0 && rowIndex < _rows.Count) {
          var row = _rows[rowIndex];
          row.EnsureCellCount(_columns.Count);
          if (columnIndex >= 0 && columnIndex < row.Cells.Count)
            return row.Cells[columnIndex];
        }
        return null;
      }
      set {
        if (rowIndex >= 0 && rowIndex < _rows.Count) {
          var row = _rows[rowIndex];
          row.EnsureCellCount(_columns.Count);
          if (columnIndex >= 0 && columnIndex < row.Cells.Count)
            row.Cells[columnIndex] = value;
        }
      }
    }

    public DataGridViewCell this[string columnName, int rowIndex] {
      get {
        for (int c = 0; c < _columns.Count; c++) {
          if (string.Equals(_columns[c].Name, columnName, StringComparison.OrdinalIgnoreCase))
            return this[c, rowIndex];
        }
        return null;
      }
      set {
        for (int c = 0; c < _columns.Count; c++) {
          if (string.Equals(_columns[c].Name, columnName, StringComparison.OrdinalIgnoreCase)) {
            this[c, rowIndex] = value;
            return;
          }
        }
      }
    }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event DataGridViewCellEventHandler CellClick;
    public event DataGridViewCellEventHandler CellDoubleClick;
    public event DataGridViewCellEventHandler CellValueChanged;
    public event DataGridViewCellFormattingEventHandler CellFormatting;
    public event EventHandler CellPainting;
    public event DataGridViewCellEventHandler CellContentClick;
    public event EventHandler SelectionChanged;
    public event DataGridViewCellEventHandler RowEnter;
    public event DataGridViewCellEventHandler RowLeave;
    public event EventHandler CurrentCellChanged;
    public event EventHandler DataError;
    public event DataGridViewCellEventHandler CellBeginEdit;
    public event DataGridViewCellEventHandler CellEndEdit;
    public event EventHandler EditingControlShowing;
    public event EventHandler ColumnAdded;
    public event EventHandler ColumnRemoved;
    public event EventHandler UserAddedRow;
    public event EventHandler UserDeletedRow;
    public event EventHandler UserDeletingRow;
    public event EventHandler Sorted;
    public event DataGridViewRowsAddedEventHandler RowsAdded;
    public event DataGridViewRowsRemovedEventHandler RowsRemoved;
    public event DataGridViewCellEventHandler CellMouseClick;
    public event DataGridViewCellEventHandler CellMouseDoubleClick;
    public event EventHandler ColumnHeaderMouseClick;

    protected virtual void OnCellClick(DataGridViewCellEventArgs e) { CellClick?.Invoke(this, e); }
    protected virtual void OnCellValueChanged(DataGridViewCellEventArgs e) { CellValueChanged?.Invoke(this, e); }
    protected virtual void OnSelectionChanged(EventArgs e) { SelectionChanged?.Invoke(this, e); }
    protected virtual void OnCurrentCellChanged(EventArgs e) { CurrentCellChanged?.Invoke(this, e); }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public void Sort(DataGridViewColumn column, System.ComponentModel.ListSortDirection direction) {
      _sortedColumn = column;
      _sortOrder = direction == System.ComponentModel.ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;
      Sorted?.Invoke(this, EventArgs.Empty);
      NotifyStateChanged();
    }

    public void InvalidateCell(int columnIndex, int rowIndex) { NotifyStateChanged(); }
    public void InvalidateRow(int rowIndex) { NotifyStateChanged(); }
    public void RefreshEdit() { NotifyStateChanged(); }
    public bool BeginEdit(bool selectAll) { return true; }
    public bool EndEdit() { return true; }
    public bool CancelEdit() { return true; }

    public void NotifyCurrentCellDirty(bool dirty) {
      // Stub: signals the DataGridView that the current cell has uncommitted changes
    }

    public static Control EditingControl { get; set; }

    public void ClearSelection() {
      foreach (DataGridViewRow row in _rows) {
        row.Selected = false;
        foreach (DataGridViewCell cell in row.Cells) cell.Selected = false;
      }
      OnSelectionChanged(EventArgs.Empty);
      NotifyStateChanged();
    }

    public void SelectAll() {
      foreach (DataGridViewRow row in _rows) {
        row.Selected = true;
        foreach (DataGridViewCell cell in row.Cells) cell.Selected = true;
      }
      OnSelectionChanged(EventArgs.Empty);
      NotifyStateChanged();
    }

    public void AutoResizeColumns() { }
    public void AutoResizeColumn(int columnIndex) { }
    public void AutoResizeColumn(int columnIndex, DataGridViewAutoSizeColumnMode mode) { }
    public void AutoResizeRows() { }
    public void BeginUpdate() { }
    public void EndUpdate() { NotifyStateChanged(); }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "div";

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();

      if (_borderStyle == BorderStyle.FixedSingle)
        style += "border:1px solid #808080;";
      else if (_borderStyle == BorderStyle.Fixed3D)
        style += "border:2px inset;";

      if (!_backgroundColor.IsEmpty)
        style += "background-color:" + _backgroundColor.ToCss() + ";";

      style += "overflow:auto;";
      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      builder.OpenElement(seq++, "table");
      builder.AddAttribute(seq++, "style",
        "width:100%;border-collapse:collapse;table-layout:fixed;" +
        "border-color:" + _gridColor.ToCss() + ";");

      // Column headers
      if (_columnHeadersVisible && _columns.Count > 0) {
        builder.OpenElement(seq++, "thead");
        builder.OpenElement(seq++, "tr");

        if (_rowHeadersVisible) {
          builder.OpenElement(seq++, "th");
          builder.AddAttribute(seq++, "style", "width:" + _rowHeadersWidth + "px;padding:4px;border:1px solid " + _gridColor.ToCss() + ";background-color:#f0f0f0;");
          builder.CloseElement();
        }

        for (int c = 0; c < _columns.Count; c++) {
          var col = _columns[c];
          if (!col.Visible) continue;

          int colIdx = c;
          builder.OpenElement(seq++, "th");
          builder.AddAttribute(seq++, "style",
            "padding:4px;border:1px solid " + _gridColor.ToCss() + ";background-color:#f0f0f0;" +
            "width:" + col.Width + "px;text-align:left;cursor:pointer;");

          if (col.SortMode != DataGridViewColumnSortMode.NotSortable) {
            builder.AddAttribute(seq++, "onclick",
              EventCallback.Factory.Create<BlazorMouseEventArgs>(
                GetBlazorReceiver(),
                (BlazorMouseEventArgs e) => {
                  ColumnHeaderMouseClick?.Invoke(this, EventArgs.Empty);
                }));
          }

          builder.AddContent(seq++, col.HeaderText ?? col.Name);
          builder.CloseElement(); // th
        }

        builder.CloseElement(); // tr
        builder.CloseElement(); // thead
      }

      // Body
      builder.OpenElement(seq++, "tbody");
      for (int r = 0; r < _rows.Count; r++) {
        var row = _rows[r];
        if (!row.Visible) continue;
        int rowIdx = r;

        row.EnsureCellCount(_columns.Count);

        builder.OpenElement(seq++, "tr");

        string rowStyle = "";
        if (row.Selected)
          rowStyle += "background-color:#0078d7;color:white;";
        else if (r % 2 == 1 && !_alternatingRowsDefaultCellStyle.BackColor.IsEmpty)
          rowStyle += "background-color:" + _alternatingRowsDefaultCellStyle.BackColor.ToCss() + ";";

        builder.AddAttribute(seq++, "style", rowStyle);

        builder.AddAttribute(seq++, "onclick",
          EventCallback.Factory.Create<BlazorMouseEventArgs>(
            GetBlazorReceiver(),
            (BlazorMouseEventArgs e) => {
              if (_selectionMode == DataGridViewSelectionMode.FullRowSelect ||
                  _selectionMode == DataGridViewSelectionMode.RowHeaderSelect) {
                if (!_multiSelect) ClearSelection();
                row.Selected = !row.Selected;
                OnSelectionChanged(EventArgs.Empty);
              }
              OnCellClick(new DataGridViewCellEventArgs(0, rowIdx));
              NotifyStateChanged();
            }));

        // Row header
        if (_rowHeadersVisible) {
          builder.OpenElement(seq++, "td");
          builder.AddAttribute(seq++, "style",
            "width:" + _rowHeadersWidth + "px;padding:2px;border:1px solid " + _gridColor.ToCss() + ";background-color:#f0f0f0;text-align:center;");
          builder.CloseElement();
        }

        // Cells
        for (int c = 0; c < _columns.Count; c++) {
          var col = _columns[c];
          if (!col.Visible) continue;

          var cell = (c < row.Cells.Count) ? row.Cells[c] : null;
          string cellValue = cell?.Value?.ToString() ?? string.Empty;

          builder.OpenElement(seq++, "td");

          string cellStyle = "padding:4px;border:1px solid " + _gridColor.ToCss() + ";" +
                             "overflow:hidden;text-overflow:ellipsis;white-space:nowrap;";

          if (cell != null && cell.Selected && !row.Selected)
            cellStyle += "background-color:#0078d7;color:white;";

          builder.AddAttribute(seq++, "style", cellStyle);
          builder.AddContent(seq++, cellValue);
          builder.CloseElement(); // td
        }

        builder.CloseElement(); // tr
      }
      builder.CloseElement(); // tbody
      builder.CloseElement(); // table
    }
  }
}
