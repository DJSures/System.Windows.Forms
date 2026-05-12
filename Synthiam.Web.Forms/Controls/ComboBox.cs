// =============================================================================
// Synthiam.Web.Forms - ComboBox control for Blazor
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  public class ComboBox : Control {

    private ObjectCollection _items;
    private int _selectedIndex = -1;
    private ComboBoxStyle _dropDownStyle = ComboBoxStyle.DropDown;
    private int _dropDownWidth;
    private int _dropDownHeight = 106;
    private int _maxDropDownItems = 8;
    private bool _sorted;
    private FlatStyle _flatStyle = FlatStyle.Standard;
    private bool _formattingEnabled;
    private string _formatString = string.Empty;
    private string _displayMember = string.Empty;
    private string _valueMember = string.Empty;
    private object _dataSource;
    private int _maxLength;
    private bool _droppedDown;
    private bool _integralHeight = true;
    private DrawMode _drawMode = DrawMode.Normal;

    public ComboBox() {
      _items = new ObjectCollection(this);
    }

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public ObjectCollection Items => _items;

    public int SelectedIndex {
      get => _selectedIndex;
      set {
        if (_selectedIndex != value) {
          _selectedIndex = value;
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

    public string SelectedText {
      get {
        var item = SelectedItem;
        return item?.ToString() ?? string.Empty;
      }
      set { }
    }

    public object SelectedValue {
      get => SelectedItem;
      set => SelectedItem = value;
    }

    public ComboBoxStyle DropDownStyle {
      get => _dropDownStyle;
      set { _dropDownStyle = value; NotifyStateChanged(); }
    }

    public int DropDownWidth {
      get => _dropDownWidth;
      set => _dropDownWidth = value;
    }

    public int DropDownHeight {
      get => _dropDownHeight;
      set => _dropDownHeight = value;
    }

    public int MaxDropDownItems {
      get => _maxDropDownItems;
      set => _maxDropDownItems = value;
    }

    public bool Sorted {
      get => _sorted;
      set {
        _sorted = value;
        if (value) _items.Sort();
      }
    }

    public FlatStyle FlatStyle {
      get => _flatStyle;
      set { _flatStyle = value; NotifyStateChanged(); }
    }

    public bool FormattingEnabled {
      get => _formattingEnabled;
      set => _formattingEnabled = value;
    }

    public string FormatString {
      get => _formatString;
      set => _formatString = value ?? string.Empty;
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

    public int MaxLength {
      get => _maxLength;
      set => _maxLength = value;
    }

    public bool DroppedDown {
      get => _droppedDown;
      set => _droppedDown = value;
    }

    public bool IntegralHeight {
      get => _integralHeight;
      set => _integralHeight = value;
    }

    public DrawMode DrawMode {
      get => _drawMode;
      set => _drawMode = value;
    }

    public int ItemHeight { get; set; } = 15;

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event EventHandler SelectedIndexChanged;
    public event EventHandler SelectedValueChanged;
    public event EventHandler DropDown;
    public event EventHandler DropDownClosed;
    public event DrawItemEventHandler DrawItem;
    public event MeasureItemEventHandler MeasureItem;
    public event EventHandler Format;
    public event EventHandler SelectionChangeCommitted;

    protected virtual void OnSelectedIndexChanged(EventArgs e) {
      SelectedIndexChanged?.Invoke(this, e);
    }

    protected virtual void OnSelectedValueChanged(EventArgs e) {
      SelectedValueChanged?.Invoke(this, e);
    }

    protected virtual void OnDropDown(EventArgs e) {
      DropDown?.Invoke(this, e);
    }

    protected virtual void OnDropDownClosed(EventArgs e) {
      DropDownClosed?.Invoke(this, e);
    }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public int FindString(string s) {
      return FindString(s, -1);
    }

    public int FindString(string s, int startIndex) {
      if (string.IsNullOrEmpty(s)) return -1;
      for (int i = startIndex + 1; i < _items.Count; i++) {
        if (_items[i]?.ToString()?.StartsWith(s, StringComparison.OrdinalIgnoreCase) == true)
          return i;
      }
      return -1;
    }

    public int FindStringExact(string s) {
      return FindStringExact(s, -1);
    }

    public int FindStringExact(string s, int startIndex) {
      if (s == null) return -1;
      for (int i = startIndex + 1; i < _items.Count; i++) {
        if (string.Equals(_items[i]?.ToString(), s, StringComparison.OrdinalIgnoreCase))
          return i;
      }
      return -1;
    }

    public string GetItemText(object item) {
      return item?.ToString() ?? string.Empty;
    }

    public void BeginUpdate() { }
    public void EndUpdate() { NotifyStateChanged(); }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "select";

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      // Render options
      for (int i = 0; i < _items.Count; i++) {
        builder.OpenElement(seq++, "option");
        builder.AddAttribute(seq++, "value", i.ToString());
        if (i == _selectedIndex)
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
      private readonly ComboBox _owner;

      internal ObjectCollection(ComboBox owner) {
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
  }
}
