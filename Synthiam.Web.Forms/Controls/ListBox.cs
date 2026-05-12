// =============================================================================
// Synthiam.Web.Forms - ListBox control for Blazor
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  public class ListBox : Control {

    private ObjectCollection _items;
    private int _selectedIndex = -1;
    private SelectionMode _selectionMode = SelectionMode.One;
    private bool _sorted;
    private bool _multiColumn;
    private int _columnWidth;
    private bool _horizontalScrollbar;
    private bool _scrollAlwaysVisible;
    private bool _integralHeight = true;
    private int _itemHeight = 13;
    private int _topIndex;
    private string _displayMember = string.Empty;
    private string _valueMember = string.Empty;
    private object _dataSource;
    private DrawMode _drawMode = DrawMode.Normal;
    private BorderStyle _borderStyle = BorderStyle.Fixed3D;
    private int _horizontalExtent;
    private readonly SelectedIndexCollection _selectedIndices;
    private readonly SelectedObjectCollection _selectedItems;

    public ListBox() {
      _items = new ObjectCollection(this);
      _selectedIndices = new SelectedIndexCollection(this);
      _selectedItems = new SelectedObjectCollection(this);
    }

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public ObjectCollection Items => _items;

    public int SelectedIndex {
      get => _selectedIndex;
      set {
        if (_selectedIndex != value) {
          _selectedIndices.ClearInternal();
          _selectedIndex = value;
          if (value >= 0) _selectedIndices.AddInternal(value);
          OnSelectedIndexChanged(EventArgs.Empty);
          OnSelectedValueChanged(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public object SelectedItem {
      get => (_selectedIndex >= 0 && _selectedIndex < _items.Count) ? _items[_selectedIndex] : null;
      set {
        int idx = _items.IndexOf(value);
        if (idx >= 0) SelectedIndex = idx;
      }
    }

    public SelectedIndexCollection SelectedIndices => _selectedIndices;
    public SelectedObjectCollection SelectedItems => _selectedItems;

    public object SelectedValue {
      get => SelectedItem;
      set => SelectedItem = value;
    }

    public SelectionMode SelectionMode {
      get => _selectionMode;
      set { _selectionMode = value; NotifyStateChanged(); }
    }

    public bool Sorted {
      get => _sorted;
      set {
        _sorted = value;
        if (value) _items.Sort();
      }
    }

    public bool MultiColumn {
      get => _multiColumn;
      set => _multiColumn = value;
    }

    public int ColumnWidth {
      get => _columnWidth;
      set => _columnWidth = value;
    }

    public bool HorizontalScrollbar {
      get => _horizontalScrollbar;
      set => _horizontalScrollbar = value;
    }

    public bool ScrollAlwaysVisible {
      get => _scrollAlwaysVisible;
      set => _scrollAlwaysVisible = value;
    }

    public bool IntegralHeight {
      get => _integralHeight;
      set => _integralHeight = value;
    }

    public int ItemHeight {
      get => _itemHeight;
      set => _itemHeight = value;
    }

    public int TopIndex {
      get => _topIndex;
      set => _topIndex = value;
    }

    public string DisplayMember {
      get => _displayMember;
      set => _displayMember = value ?? string.Empty;
    }

    public string ValueMember {
      get => _valueMember;
      set => _valueMember = value ?? string.Empty;
    }

    public object DataSource {
      get => _dataSource;
      set {
        _dataSource = value;
        if (value is IEnumerable enumerable) {
          _items.Clear();
          foreach (var item in enumerable)
            _items.Add(item);
        }
        NotifyStateChanged();
      }
    }

    public DrawMode DrawMode {
      get => _drawMode;
      set => _drawMode = value;
    }

    public BorderStyle BorderStyle {
      get => _borderStyle;
      set { _borderStyle = value; NotifyStateChanged(); }
    }

    public int HorizontalExtent {
      get => _horizontalExtent;
      set => _horizontalExtent = value;
    }

    public bool FormattingEnabled { get; set; }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event EventHandler SelectedIndexChanged;
    public event EventHandler SelectedValueChanged;
    public event DrawItemEventHandler DrawItem;
    public event MeasureItemEventHandler MeasureItem;

    protected virtual void OnSelectedIndexChanged(EventArgs e) {
      SelectedIndexChanged?.Invoke(this, e);
    }

    protected virtual void OnSelectedValueChanged(EventArgs e) {
      SelectedValueChanged?.Invoke(this, e);
    }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public int FindString(string s) => FindString(s, -1);

    public int FindString(string s, int startIndex) {
      if (string.IsNullOrEmpty(s)) return -1;
      for (int i = startIndex + 1; i < _items.Count; i++) {
        if (_items[i]?.ToString()?.StartsWith(s, StringComparison.OrdinalIgnoreCase) == true)
          return i;
      }
      return -1;
    }

    public int FindStringExact(string s) => FindStringExact(s, -1);

    public int FindStringExact(string s, int startIndex) {
      if (s == null) return -1;
      for (int i = startIndex + 1; i < _items.Count; i++) {
        if (string.Equals(_items[i]?.ToString(), s, StringComparison.OrdinalIgnoreCase))
          return i;
      }
      return -1;
    }

    public void SetSelected(int index, bool value) {
      if (index < 0 || index >= _items.Count) return;
      if (value) {
        if (!_selectedIndices.Contains(index))
          _selectedIndices.AddInternal(index);
      } else {
        _selectedIndices.RemoveInternal(index);
      }
      if (_selectedIndices.Count > 0)
        _selectedIndex = _selectedIndices[0];
      else
        _selectedIndex = -1;
      OnSelectedIndexChanged(EventArgs.Empty);
      NotifyStateChanged();
    }

    public void ClearSelected() {
      _selectedIndices.ClearInternal();
      _selectedIndex = -1;
      NotifyStateChanged();
    }

    public string GetItemText(object item) {
      return item?.ToString() ?? string.Empty;
    }

    public int IndexFromPoint(Point p) => -1;
    public int IndexFromPoint(int x, int y) => -1;
    public Rectangle GetItemRectangle(int index) => Rectangle.Empty;

    public void BeginUpdate() { }
    public void EndUpdate() { NotifyStateChanged(); }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "select";

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();

      if (_borderStyle == BorderStyle.FixedSingle)
        style += "border:1px solid #808080;";
      else if (_borderStyle == BorderStyle.Fixed3D)
        style += "border:2px inset;";
      else if (_borderStyle == BorderStyle.None)
        style += "border:none;";

      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      // Multiple and size attributes
      if (_selectionMode == SelectionMode.MultiSimple || _selectionMode == SelectionMode.MultiExtended)
        builder.AddAttribute(seq++, "multiple", true);

      int visibleItems = Math.Max(2, Math.Min(_items.Count, 10));
      builder.AddAttribute(seq++, "size", visibleItems.ToString());

      // Render options
      for (int i = 0; i < _items.Count; i++) {
        builder.OpenElement(seq++, "option");
        builder.AddAttribute(seq++, "value", i.ToString());
        if (_selectedIndices.Contains(i))
          builder.AddAttribute(seq++, "selected", true);
        builder.AddContent(seq++, GetItemText(_items[i]));
        builder.CloseElement();
      }
    }

    protected override void AddEventAttributes(RenderTreeBuilder builder, ref int seq) {
      base.AddEventAttributes(builder, ref seq);

      builder.AddAttribute(seq++, "onchange",
        EventCallback.Factory.Create<ChangeEventArgs>(
          GetBlazorReceiver(),
          (ChangeEventArgs e) => {
            if (int.TryParse(e.Value?.ToString(), out int idx))
              SelectedIndex = idx;
          }));
    }

    // ═══════════════════════════════════════════════
    // ObjectCollection
    // ═══════════════════════════════════════════════

    public class ObjectCollection : IList<object>, IList {

      private readonly List<object> _list = new List<object>();
      private readonly ListBox _owner;

      internal ObjectCollection(ListBox owner) {
        _owner = owner;
      }

      public int Count => _list.Count;
      public bool IsReadOnly => false;
      public bool IsFixedSize => false;
      public bool IsSynchronized => false;
      public object SyncRoot => _list;

      public object this[int index] {
        get => _list[index];
        set => _list[index] = value;
      }

      public int Add(object item) {
        _list.Add(item);
        if (_owner._sorted) Sort();
        _owner.NotifyStateChanged();
        return _list.Count - 1;
      }

      public void AddRange(object[] items) {
        _list.AddRange(items);
        if (_owner._sorted) Sort();
        _owner.NotifyStateChanged();
      }

      public void Clear() {
        _list.Clear();
        _owner._selectedIndex = -1;
        _owner._selectedIndices.ClearInternal();
        _owner.NotifyStateChanged();
      }

      public bool Contains(object item) => _list.Contains(item);
      public void CopyTo(object[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
      public int IndexOf(object item) => _list.IndexOf(item);

      public void Insert(int index, object item) {
        _list.Insert(index, item);
        if (_owner._sorted) Sort();
        _owner.NotifyStateChanged();
      }

      public void Remove(object item) {
        int idx = _list.IndexOf(item);
        if (idx >= 0) RemoveAt(idx);
      }

      bool ICollection<object>.Remove(object item) {
        int idx = _list.IndexOf(item);
        if (idx >= 0) { RemoveAt(idx); return true; }
        return false;
      }

      public void RemoveAt(int index) {
        _list.RemoveAt(index);
        if (_owner._selectedIndex >= _list.Count)
          _owner._selectedIndex = _list.Count - 1;
        _owner.NotifyStateChanged();
      }

      public IEnumerator<object> GetEnumerator() => _list.GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

      void ICollection<object>.Add(object item) => Add(item);
      int IList.Add(object value) => Add(value);
      void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);

      internal void Sort() {
        _list.Sort((a, b) => string.Compare(a?.ToString(), b?.ToString(), StringComparison.OrdinalIgnoreCase));
      }
    }

    // ═══════════════════════════════════════════════
    // SelectedIndexCollection
    // ═══════════════════════════════════════════════

    public class SelectedIndexCollection : IList<int>, IList {

      private readonly List<int> _list = new List<int>();
      private readonly ListBox _owner;

      internal SelectedIndexCollection(ListBox owner) { _owner = owner; }

      public int Count => _list.Count;
      public bool IsReadOnly => true;
      public bool IsFixedSize => true;
      public bool IsSynchronized => false;
      public object SyncRoot => _list;

      public int this[int index] {
        get => _list[index];
        set { }
      }

      object IList.this[int index] {
        get => _list[index];
        set { }
      }

      internal void AddInternal(int index) { if (!_list.Contains(index)) _list.Add(index); }
      internal void RemoveInternal(int index) { _list.Remove(index); }
      internal void ClearInternal() { _list.Clear(); }

      public bool Contains(int item) => _list.Contains(item);
      public void CopyTo(int[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
      public int IndexOf(int item) => _list.IndexOf(item);
      public IEnumerator<int> GetEnumerator() => _list.GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

      // Read-only: these throw
      public void Add(int item) { }
      public void Clear() { }
      public void Insert(int index, int item) { }
      public bool Remove(int item) => false;
      public void RemoveAt(int index) { }

      int IList.Add(object value) => -1;
      bool IList.Contains(object value) => value is int i && _list.Contains(i);
      int IList.IndexOf(object value) => value is int i ? _list.IndexOf(i) : -1;
      void IList.Insert(int index, object value) { }
      void IList.Remove(object value) { }
      void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);
    }

    // ═══════════════════════════════════════════════
    // SelectedObjectCollection
    // ═══════════════════════════════════════════════

    public class SelectedObjectCollection : IList<object>, IList {

      private readonly ListBox _owner;

      internal SelectedObjectCollection(ListBox owner) { _owner = owner; }

      public int Count => _owner._selectedIndices.Count;
      public bool IsReadOnly => true;
      public bool IsFixedSize => true;
      public bool IsSynchronized => false;
      public object SyncRoot => this;

      public object this[int index] {
        get {
          int selIdx = _owner._selectedIndices[index];
          return (selIdx >= 0 && selIdx < _owner._items.Count) ? _owner._items[selIdx] : null;
        }
        set { }
      }

      public bool Contains(object item) => _owner._items.IndexOf(item) >= 0 && _owner._selectedIndices.Contains(_owner._items.IndexOf(item));
      public void CopyTo(object[] array, int arrayIndex) {
        for (int i = 0; i < Count; i++)
          array[arrayIndex + i] = this[i];
      }
      public int IndexOf(object item) {
        for (int i = 0; i < Count; i++)
          if (this[i] == item) return i;
        return -1;
      }

      public IEnumerator<object> GetEnumerator() {
        for (int i = 0; i < Count; i++)
          yield return this[i];
      }
      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

      public void Add(object item) { }
      public void Clear() { }
      public void Insert(int index, object item) { }
      public bool Remove(object item) => false;
      public void RemoveAt(int index) { }

      int IList.Add(object value) => -1;
      bool IList.Contains(object value) => Contains(value);
      int IList.IndexOf(object value) => IndexOf(value);
      void IList.Insert(int index, object value) { }
      void IList.Remove(object value) { }
      void ICollection.CopyTo(Array array, int index) {
        for (int i = 0; i < Count; i++)
          array.SetValue(this[i], index + i);
      }
    }
  }
}
