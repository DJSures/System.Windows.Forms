// =============================================================================
// Synthiam.Web.Forms - ListView control for Blazor
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Synthiam.Web.Forms;
using BlazorMouseEventArgs = Microsoft.AspNetCore.Components.Web.MouseEventArgs;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // ColumnHeaderAutoResizeStyle enum
  // ---------------------------------------------------------------------------
  public enum ColumnHeaderAutoResizeStyle {
    None = 0,
    HeaderSize = 1,
    ColumnContent = 2
  }

  // ---------------------------------------------------------------------------
  // ColumnHeader
  // ---------------------------------------------------------------------------
  public class ColumnHeader {

    public string Text { get; set; } = string.Empty;
    public int Width { get; set; } = 60;
    public HorizontalAlignment TextAlign { get; set; } = HorizontalAlignment.Left;
    public int ImageIndex { get; set; } = -1;
    public string ImageKey { get; set; } = string.Empty;
    public object Tag { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Index { get; internal set; }
    public int DisplayIndex { get; set; }
    public ListView ListView { get; internal set; }

    public ColumnHeader() { }

    public ColumnHeader(string text) { Text = text; }

    public ColumnHeader(string text, int width) { Text = text; Width = width; }

    public ColumnHeader(string text, int width, HorizontalAlignment textAlign) {
      Text = text; Width = width; TextAlign = textAlign;
    }

    public void AutoResize(ColumnHeaderAutoResizeStyle headerAutoResize) {
      // Stub
    }

    public override string ToString() => Text ?? "ColumnHeader";
  }

  // ---------------------------------------------------------------------------
  // ListViewGroup
  // ---------------------------------------------------------------------------
  public class ListViewGroup {

    public string Header { get; set; } = string.Empty;
    public HorizontalAlignment HeaderAlignment { get; set; } = HorizontalAlignment.Left;
    public ListView.ListViewItemCollection Items { get; internal set; }
    public object Tag { get; set; }
    public string Name { get; set; } = string.Empty;

    public ListViewGroup() { }
    public ListViewGroup(string header) { Header = header; }
    public ListViewGroup(string key, string headerText) { Name = key; Header = headerText; }

    public override string ToString() => Header ?? "ListViewGroup";
  }

  // ---------------------------------------------------------------------------
  // ListViewItem (extends ForwardDeclarations partial)
  // ---------------------------------------------------------------------------
  public partial class ListViewItem {

    private string _text = string.Empty;
    private ListViewSubItemCollection _subItems;

    public ListViewItem() {
      _subItems = new ListViewSubItemCollection(this);
      _subItems.Add(new ListViewSubItem(this, string.Empty));
    }

    public ListViewItem(string text) : this() {
      _text = text ?? string.Empty;
      _subItems[0].Text = _text;
    }

    public ListViewItem(string[] items) : this() {
      if (items != null && items.Length > 0) {
        _text = items[0] ?? string.Empty;
        _subItems[0].Text = _text;
        for (int i = 1; i < items.Length; i++)
          _subItems.Add(new ListViewSubItem(this, items[i] ?? string.Empty));
      }
    }

    public ListViewItem(string text, int imageIndex) : this(text) {
      ImageIndex = imageIndex;
    }

    public ListViewItem(string[] items, int imageIndex) : this(items) {
      ImageIndex = imageIndex;
    }

    public ListViewItem(string text, string imageKey) : this(text) {
      ImageKey = imageKey;
    }

    public ListViewItem(string[] items, string imageKey) : this(items) {
      ImageKey = imageKey;
    }

    public ListViewItem(ListViewItem.ListViewSubItem[] subItems, int imageIndex) {
      _subItems = new ListViewSubItemCollection(this);
      if (subItems != null) {
        foreach (var si in subItems) _subItems.Add(si);
      }
      if (_subItems.Count > 0) _text = _subItems[0].Text;
      ImageIndex = imageIndex;
    }

    public ListViewItem(string text, int imageIndex, ListViewGroup group) : this(text, imageIndex) {
      Group = group;
    }

    public ListViewItem(string[] items, int imageIndex, ListViewGroup group) : this(items, imageIndex) {
      Group = group;
    }

    public string Text {
      get => _text;
      set {
        _text = value ?? string.Empty;
        if (_subItems.Count > 0)
          _subItems[0].Text = _text;
      }
    }

    public ListViewSubItemCollection SubItems => _subItems;
    public int ImageIndex { get; set; } = -1;
    public string ImageKey { get; set; } = string.Empty;
    public bool Checked { get; set; }
    public bool Selected { get; set; }
    public bool Focused { get; set; }
    public object Tag { get; set; }
    public string Name { get; set; } = string.Empty;
    public ListViewGroup Group { get; set; }
    public int Index { get; internal set; } = -1;
    public Color BackColor { get; set; } = Color.Empty;
    public Color ForeColor { get; set; } = Color.Empty;
    public Font Font { get; set; }
    public bool UseItemStyleForSubItems { get; set; } = true;
    public ListView ListView { get; internal set; }
    public Rectangle Bounds => Rectangle.Empty;
    public string ToolTipText { get; set; } = string.Empty;
    public int IndentCount { get; set; }
    public int StateImageIndex { get; set; } = -1;

    public void Remove() {
      ListView?.Items.Remove(this);
    }

    public void EnsureVisible() { }

    public ListViewItem Clone() {
      var clone = new ListViewItem(_text);
      clone.ImageIndex = ImageIndex;
      clone.Tag = Tag;
      clone.Checked = Checked;
      for (int i = 1; i < _subItems.Count; i++)
        clone._subItems.Add(new ListViewSubItem(clone, _subItems[i].Text));
      return clone;
    }

    public override string ToString() => "ListViewItem: {" + _text + "}";

    // ═══════════════════════════════════════════════
    // ListViewSubItem
    // ═══��═══════════════════════════════════════════

    public class ListViewSubItem {

      public string Text { get; set; } = string.Empty;
      public Color BackColor { get; set; } = Color.Empty;
      public Color ForeColor { get; set; } = Color.Empty;
      public Font Font { get; set; }
      public object Tag { get; set; }
      public string Name { get; set; } = string.Empty;

      internal ListViewItem _owner;

      public ListViewSubItem() { }

      public ListViewSubItem(ListViewItem owner, string text) {
        _owner = owner;
        Text = text ?? string.Empty;
      }

      public ListViewSubItem(ListViewItem owner, string text, Color foreColor, Color backColor, Font font) {
        _owner = owner;
        Text = text ?? string.Empty;
        ForeColor = foreColor;
        BackColor = backColor;
        Font = font;
      }

      public override string ToString() => Text ?? string.Empty;
    }

    // ════════════════════���══════════════════════════
    // ListViewSubItemCollection
    // ═══════════════════════════════════════��═══════

    public class ListViewSubItemCollection : IList<ListViewSubItem>, IList {

      private readonly List<ListViewSubItem> _list = new List<ListViewSubItem>();
      private readonly ListViewItem _owner;

      internal ListViewSubItemCollection(ListViewItem owner) { _owner = owner; }

      public int Count => _list.Count;
      public bool IsReadOnly => false;
      public bool IsFixedSize => false;
      public bool IsSynchronized => false;
      public object SyncRoot => _list;

      public ListViewSubItem this[int index] {
        get => _list[index];
        set => _list[index] = value;
      }

      public ListViewSubItem this[string key] {
        get {
          foreach (var si in _list)
            if (string.Equals(si.Name, key, StringComparison.OrdinalIgnoreCase)) return si;
          return null;
        }
      }

      object IList.this[int index] {
        get => _list[index];
        set => _list[index] = (ListViewSubItem)value;
      }

      public ListViewSubItem Add(string text) {
        var si = new ListViewSubItem(_owner, text);
        _list.Add(si);
        return si;
      }

      public ListViewSubItem Add(string text, Color foreColor, Color backColor, Font font) {
        var si = new ListViewSubItem(_owner, text, foreColor, backColor, font);
        _list.Add(si);
        return si;
      }

      public void Add(ListViewSubItem item) => _list.Add(item);
      public void AddRange(ListViewSubItem[] items) => _list.AddRange(items);
      public void AddRange(string[] items) { foreach (var s in items) Add(s); }
      public void AddRange(string[] items, Color foreColor, Color backColor, Font font) {
        foreach (var s in items) Add(s, foreColor, backColor, font);
      }
      public void Clear() => _list.Clear();
      public bool Contains(ListViewSubItem item) => _list.Contains(item);
      public bool ContainsKey(string key) => this[key] != null;
      public void CopyTo(ListViewSubItem[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
      public int IndexOf(ListViewSubItem item) => _list.IndexOf(item);
      public int IndexOfKey(string key) {
        for (int i = 0; i < _list.Count; i++)
          if (string.Equals(_list[i].Name, key, StringComparison.OrdinalIgnoreCase)) return i;
        return -1;
      }
      public void Insert(int index, ListViewSubItem item) => _list.Insert(index, item);
      public bool Remove(ListViewSubItem item) => _list.Remove(item);
      public void RemoveAt(int index) => _list.RemoveAt(index);
      public void RemoveByKey(string key) {
        int idx = IndexOfKey(key);
        if (idx >= 0) _list.RemoveAt(idx);
      }
      public IEnumerator<ListViewSubItem> GetEnumerator() => _list.GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

      int IList.Add(object value) { _list.Add((ListViewSubItem)value); return _list.Count - 1; }
      bool IList.Contains(object value) => _list.Contains((ListViewSubItem)value);
      int IList.IndexOf(object value) => _list.IndexOf((ListViewSubItem)value);
      void IList.Insert(int index, object value) => _list.Insert(index, (ListViewSubItem)value);
      void IList.Remove(object value) => _list.Remove((ListViewSubItem)value);
      void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);
    }
  }

  // ---------------------------------------------------------------------------
  // ListView
  // ---------------------------------------------------------------------------
  public class ListView : Control {

    private ListViewItemCollection _items;
    private ColumnHeaderCollection _columns;
    private readonly List<ListViewGroup> _groups = new List<ListViewGroup>();

    private View _view = View.LargeIcon;
    private ImageList _largeImageList;
    private ImageList _smallImageList;
    private ImageList _stateImageList;
    private SortOrder _sorting = SortOrder.None;
    private ColumnHeaderStyle _headerStyle = ColumnHeaderStyle.Clickable;
    private bool _gridLines;
    private bool _fullRowSelect;
    private bool _multiSelect = true;
    private bool _checkBoxes;
    private bool _hideSelection = true;
    private bool _labelEdit;
    private bool _labelWrap = true;
    private bool _scrollable = true;
    private bool _showGroups;
    private ItemActivation _activation = ItemActivation.Standard;
    private bool _allowColumnReorder;
    private bool _autoArrange = true;
    private bool _showItemToolTips;
    private bool _virtualMode;
    private int _virtualListSize;
    private bool _ownerDraw;
    private BorderStyle _borderStyle = BorderStyle.Fixed3D;
    private Size _tileSize = Size.Empty;

    public ListView() {
      _items = new ListViewItemCollection(this);
      _columns = new ColumnHeaderCollection(this);
    }

    // ═══════════════════════════════════════════════
    // Properties
    // ═════���═════════════════════════════════════════

    public ListViewItemCollection Items => _items;
    public ColumnHeaderCollection Columns => _columns;

    public SelectedListViewItemCollection SelectedItems {
      get {
        var sel = new SelectedListViewItemCollection();
        for (int i = 0; i < _items.Count; i++)
          if (_items[i].Selected) sel.AddInternal(_items[i]);
        return sel;
      }
    }

    public SelectedIndexCollection SelectedIndices {
      get {
        var sel = new SelectedIndexCollection();
        for (int i = 0; i < _items.Count; i++)
          if (_items[i].Selected) sel.AddInternal(i);
        return sel;
      }
    }

    public CheckedListViewItemCollection CheckedItems {
      get {
        var coll = new CheckedListViewItemCollection();
        for (int i = 0; i < _items.Count; i++)
          if (_items[i].Checked) coll.AddInternal(_items[i]);
        return coll;
      }
    }

    public CheckedIndexCollection CheckedIndices {
      get {
        var coll = new CheckedIndexCollection();
        for (int i = 0; i < _items.Count; i++)
          if (_items[i].Checked) coll.AddInternal(i);
        return coll;
      }
    }

    public View View {
      get => _view;
      set { _view = value; NotifyStateChanged(); }
    }

    public ImageList LargeImageList { get => _largeImageList; set => _largeImageList = value; }
    public ImageList SmallImageList { get => _smallImageList; set => _smallImageList = value; }
    public ImageList StateImageList { get => _stateImageList; set => _stateImageList = value; }

    public SortOrder Sorting { get => _sorting; set => _sorting = value; }
    public ColumnHeaderStyle HeaderStyle { get => _headerStyle; set { _headerStyle = value; NotifyStateChanged(); } }
    public bool GridLines { get => _gridLines; set { _gridLines = value; NotifyStateChanged(); } }
    public bool FullRowSelect { get => _fullRowSelect; set => _fullRowSelect = value; }
    public bool MultiSelect { get => _multiSelect; set => _multiSelect = value; }
    public bool CheckBoxes { get => _checkBoxes; set { _checkBoxes = value; NotifyStateChanged(); } }
    public bool HideSelection { get => _hideSelection; set => _hideSelection = value; }
    public bool LabelEdit { get => _labelEdit; set => _labelEdit = value; }
    public bool LabelWrap { get => _labelWrap; set => _labelWrap = value; }
    public bool Scrollable { get => _scrollable; set => _scrollable = value; }
    public bool ShowGroups { get => _showGroups; set { _showGroups = value; NotifyStateChanged(); } }
    public List<ListViewGroup> Groups => _groups;
    public ItemActivation Activation { get => _activation; set => _activation = value; }
    public bool AllowColumnReorder { get => _allowColumnReorder; set => _allowColumnReorder = value; }
    public bool AutoArrange { get => _autoArrange; set => _autoArrange = value; }
    public bool UseCompatibleStateImageBehavior { get; set; }
    public bool ShowItemToolTips { get => _showItemToolTips; set => _showItemToolTips = value; }
    public bool VirtualMode { get => _virtualMode; set => _virtualMode = value; }
    public int VirtualListSize { get => _virtualListSize; set => _virtualListSize = value; }
    public bool OwnerDraw { get => _ownerDraw; set => _ownerDraw = value; }
    public BorderStyle BorderStyle { get => _borderStyle; set { _borderStyle = value; NotifyStateChanged(); } }
    public Size TileSize { get => _tileSize; set => _tileSize = value; }

    public ListViewItem TopItem {
      get => _items.Count > 0 ? _items[0] : null;
      set { }
    }

    public ListViewItem FocusedItem {
      get {
        for (int i = 0; i < _items.Count; i++)
          if (_items[i].Focused) return _items[i];
        return null;
      }
      set { }
    }

    public System.Collections.IComparer ListViewItemSorter { get; set; }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event EventHandler SelectedIndexChanged;
    public event ItemCheckedEventHandler ItemChecked;
    public event ItemCheckEventHandler ItemCheck;
    public event ColumnClickEventHandler ColumnClick;
    public event ItemDragEventHandler ItemDrag;
    public event ListViewItemSelectionChangedEventHandler ItemSelectionChanged;
    public event EventHandler ItemActivate;
    public event EventHandler BeforeLabelEdit;
    public event EventHandler AfterLabelEdit;
    public event DrawItemEventHandler DrawItem;
    public event EventHandler DrawSubItem;
    public event EventHandler DrawColumnHeader;
    public event EventHandler RetrieveVirtualItem;

    protected virtual void OnSelectedIndexChanged(EventArgs e) {
      SelectedIndexChanged?.Invoke(this, e);
    }

    protected virtual void OnColumnClick(ColumnClickEventArgs e) {
      ColumnClick?.Invoke(this, e);
    }

    // ═══���═══════════════════════════════════════════
    // Methods
    // ══════════���════════════════════════════════════

    public void EnsureVisible(int index) { }

    public ListViewItem FindItemWithText(string text) {
      for (int i = 0; i < _items.Count; i++)
        if (_items[i].Text == text) return _items[i];
      return null;
    }

    public ListViewItem FindItemWithText(string text, bool includeSubItemsInSearch, int startIndex) {
      for (int i = startIndex; i < _items.Count; i++) {
        if (_items[i].Text == text) return _items[i];
        if (includeSubItemsInSearch) {
          foreach (ListViewItem.ListViewSubItem si in _items[i].SubItems)
            if (si.Text == text) return _items[i];
        }
      }
      return null;
    }

    public void Sort() {
      if (ListViewItemSorter != null) {
        var arr = new ListViewItem[_items.Count];
        for (int i = 0; i < _items.Count; i++) arr[i] = _items[i];
        Array.Sort(arr, ListViewItemSorter);
        _items.ClearInternal();
        for (int i = 0; i < arr.Length; i++) _items.AddInternal(arr[i]);
      }
      NotifyStateChanged();
    }

    public void AutoResizeColumns(ColumnHeaderAutoResizeStyle style) { }
    public void AutoResizeColumn(int columnIndex, ColumnHeaderAutoResizeStyle style) { }
    public ListViewItem GetItemAt(int x, int y) => null;
    public void BeginUpdate() { }
    public void EndUpdate() { NotifyStateChanged(); }

    // ═══════════════════════���═══════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════��═══

    protected override string GetHtmlTag() => "div";

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();

      if (_borderStyle == BorderStyle.FixedSingle)
        style += "border:1px solid #808080;";
      else if (_borderStyle == BorderStyle.Fixed3D)
        style += "border:2px inset;";

      style += "overflow:auto;";
      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      if (_view == View.Details)
        RenderDetailsView(builder, ref seq);
      else
        RenderIconView(builder, ref seq);
    }

    private void RenderDetailsView(RenderTreeBuilder builder, ref int seq) {
      builder.OpenElement(seq++, "table");
      builder.AddAttribute(seq++, "style",
        "width:100%;border-collapse:collapse;table-layout:fixed;" +
        (_gridLines ? "border:1px solid #ccc;" : ""));

      // Header row
      if (_headerStyle != ColumnHeaderStyle.None) {
        builder.OpenElement(seq++, "thead");
        builder.OpenElement(seq++, "tr");

        if (_checkBoxes) {
          builder.OpenElement(seq++, "th");
          builder.AddAttribute(seq++, "style", "width:30px;padding:4px;text-align:center;border-bottom:1px solid #ccc;");
          builder.CloseElement();
        }

        for (int c = 0; c < _columns.Count; c++) {
          int colIndex = c;
          builder.OpenElement(seq++, "th");

          string thStyle = "padding:4px;border-bottom:1px solid #ccc;";
          if (_columns[c].Width > 0)
            thStyle += "width:" + _columns[c].Width + "px;";

          switch (_columns[c].TextAlign) {
            case HorizontalAlignment.Center: thStyle += "text-align:center;"; break;
            case HorizontalAlignment.Right: thStyle += "text-align:right;"; break;
            default: thStyle += "text-align:left;"; break;
          }
          builder.AddAttribute(seq++, "style", thStyle);

          if (_headerStyle == ColumnHeaderStyle.Clickable) {
            builder.AddAttribute(seq++, "onclick",
              EventCallback.Factory.Create<BlazorMouseEventArgs>(
                GetBlazorReceiver(),
                (BlazorMouseEventArgs e) => OnColumnClick(new ColumnClickEventArgs(colIndex))));
            builder.AddAttribute(seq++, "style", thStyle + "cursor:pointer;");
          }

          builder.AddContent(seq++, _columns[c].Text);
          builder.CloseElement(); // th
        }

        builder.CloseElement(); // tr
        builder.CloseElement(); // thead
      }

      // Body rows
      builder.OpenElement(seq++, "tbody");
      for (int r = 0; r < _items.Count; r++) {
        var item = _items[r];
        int rowIndex = r;

        builder.OpenElement(seq++, "tr");

        string rowStyle = "cursor:pointer;";
        if (item.Selected)
          rowStyle += "background-color:#0078d7;color:white;";
        else if (!item.BackColor.IsEmpty)
          rowStyle += "background-color:" + item.BackColor.ToCss() + ";";
        if (!item.ForeColor.IsEmpty && !item.Selected)
          rowStyle += "color:" + item.ForeColor.ToCss() + ";";
        if (_gridLines)
          rowStyle += "border-bottom:1px solid #eee;";

        builder.AddAttribute(seq++, "style", rowStyle);
        builder.AddAttribute(seq++, "onclick",
          EventCallback.Factory.Create<BlazorMouseEventArgs>(
            GetBlazorReceiver(),
            (BlazorMouseEventArgs e) => {
              if (!_multiSelect) {
                for (int i = 0; i < _items.Count; i++) _items[i].Selected = false;
              }
              item.Selected = !item.Selected;
              OnSelectedIndexChanged(EventArgs.Empty);
              NotifyStateChanged();
            }));

        // Checkbox column
        if (_checkBoxes) {
          builder.OpenElement(seq++, "td");
          builder.AddAttribute(seq++, "style", "width:30px;text-align:center;padding:2px;");
          builder.OpenElement(seq++, "input");
          builder.AddAttribute(seq++, "type", "checkbox");
          if (item.Checked) builder.AddAttribute(seq++, "checked", true);
          builder.AddAttribute(seq++, "onchange",
            EventCallback.Factory.Create<ChangeEventArgs>(
              GetBlazorReceiver(),
              (ChangeEventArgs ev) => {
                item.Checked = !item.Checked;
                ItemChecked?.Invoke(this, new ItemCheckedEventArgs(item));
                NotifyStateChanged();
              }));
          builder.CloseElement(); // input
          builder.CloseElement(); // td
        }

        // Data columns
        for (int c = 0; c < _columns.Count; c++) {
          builder.OpenElement(seq++, "td");
          builder.AddAttribute(seq++, "style", "padding:4px;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;");

          string cellText = (c < item.SubItems.Count) ? item.SubItems[c].Text : string.Empty;
          builder.AddContent(seq++, cellText);
          builder.CloseElement(); // td
        }

        builder.CloseElement(); // tr
      }
      builder.CloseElement(); // tbody
      builder.CloseElement(); // table
    }

    private void RenderIconView(RenderTreeBuilder builder, ref int seq) {
      builder.OpenElement(seq++, "div");
      builder.AddAttribute(seq++, "style", "display:flex;flex-wrap:wrap;gap:8px;padding:4px;");

      for (int i = 0; i < _items.Count; i++) {
        var item = _items[i];
        int idx = i;

        builder.OpenElement(seq++, "div");

        string itemStyle = "display:flex;flex-direction:column;align-items:center;padding:4px;cursor:pointer;min-width:64px;max-width:80px;text-align:center;";
        if (item.Selected)
          itemStyle += "background-color:#0078d7;color:white;";

        builder.AddAttribute(seq++, "style", itemStyle);
        builder.AddAttribute(seq++, "onclick",
          EventCallback.Factory.Create<BlazorMouseEventArgs>(
            GetBlazorReceiver(),
            (BlazorMouseEventArgs e) => {
              if (!_multiSelect) {
                for (int j = 0; j < _items.Count; j++) _items[j].Selected = false;
              }
              item.Selected = !item.Selected;
              OnSelectedIndexChanged(EventArgs.Empty);
              NotifyStateChanged();
            }));

        // Text
        builder.OpenElement(seq++, "span");
        builder.AddAttribute(seq++, "style", "word-break:break-word;font-size:0.85em;");
        builder.AddContent(seq++, item.Text);
        builder.CloseElement(); // span

        builder.CloseElement(); // div
      }

      builder.CloseElement(); // flex container
    }

    // ═══════════════════════════════════════════════
    // ListViewItemCollection
    // ══════════════��════════════════════════════════

    public class ListViewItemCollection : IList<ListViewItem>, IList {

      private readonly List<ListViewItem> _list = new List<ListViewItem>();
      private readonly ListView _owner;

      internal ListViewItemCollection(ListView owner) { _owner = owner; }

      public int Count => _list.Count;
      public bool IsReadOnly => false;
      public bool IsFixedSize => false;
      public bool IsSynchronized => false;
      public object SyncRoot => _list;

      public ListViewItem this[int index] {
        get => _list[index];
        set {
          value.Index = index;
          value.ListView = _owner;
          _list[index] = value;
        }
      }

      public ListViewItem this[string key] {
        get {
          foreach (var item in _list)
            if (string.Equals(item.Name, key, StringComparison.OrdinalIgnoreCase)) return item;
          return null;
        }
      }

      object IList.this[int index] {
        get => _list[index];
        set => this[index] = (ListViewItem)value;
      }

      public ListViewItem Add(string text) {
        var item = new ListViewItem(text);
        return Add(item);
      }

      public ListViewItem Add(string text, int imageIndex) {
        var item = new ListViewItem(text, imageIndex);
        return Add(item);
      }

      public ListViewItem Add(string text, string imageKey) {
        var item = new ListViewItem(text, imageKey);
        return Add(item);
      }

      public ListViewItem Add(string key, string text, int imageIndex) {
        var item = new ListViewItem(text, imageIndex) { Name = key };
        return Add(item);
      }

      public ListViewItem Add(string key, string text, string imageKey) {
        var item = new ListViewItem(text, imageKey) { Name = key };
        return Add(item);
      }

      public ListViewItem Add(ListViewItem item) {
        item.Index = _list.Count;
        item.ListView = _owner;
        _list.Add(item);
        _owner.NotifyStateChanged();
        return item;
      }

      public void AddRange(ListViewItem[] items) {
        foreach (var item in items) Add(item);
      }

      public void Clear() {
        _list.Clear();
        _owner.NotifyStateChanged();
      }

      internal void ClearInternal() { _list.Clear(); }
      internal void AddInternal(ListViewItem item) { item.Index = _list.Count; item.ListView = _owner; _list.Add(item); }

      public bool Contains(ListViewItem item) => _list.Contains(item);
      public bool ContainsKey(string key) => this[key] != null;
      public void CopyTo(ListViewItem[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
      public int IndexOf(ListViewItem item) => _list.IndexOf(item);
      public int IndexOfKey(string key) {
        for (int i = 0; i < _list.Count; i++)
          if (string.Equals(_list[i].Name, key, StringComparison.OrdinalIgnoreCase)) return i;
        return -1;
      }
      public new ListViewItem Insert(int index, ListViewItem item) {
        item.Index = index;
        item.ListView = _owner;
        _list.Insert(index, item);
        for (int i = index; i < _list.Count; i++) _list[i].Index = i;
        _owner.NotifyStateChanged();
        return item;
      }

      void IList<ListViewItem>.Insert(int index, ListViewItem item) {
        Insert(index, item);
      }

      public bool Remove(ListViewItem item) {
        bool result = _list.Remove(item);
        if (result) {
          for (int i = 0; i < _list.Count; i++) _list[i].Index = i;
          _owner.NotifyStateChanged();
        }
        return result;
      }

      public void RemoveAt(int index) {
        _list.RemoveAt(index);
        for (int i = index; i < _list.Count; i++) _list[i].Index = i;
        _owner.NotifyStateChanged();
      }

      public void RemoveByKey(string key) {
        int idx = IndexOfKey(key);
        if (idx >= 0) RemoveAt(idx);
      }

      public ListViewItem[] Find(string key, bool searchAllSubItems) {
        var result = new List<ListViewItem>();
        foreach (var item in _list) {
          if (string.Equals(item.Name, key, StringComparison.OrdinalIgnoreCase))
            result.Add(item);
        }
        return result.ToArray();
      }

      public IEnumerator<ListViewItem> GetEnumerator() => _list.GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

      int IList.Add(object value) { Add((ListViewItem)value); return _list.Count - 1; }
      bool IList.Contains(object value) => _list.Contains((ListViewItem)value);
      int IList.IndexOf(object value) => _list.IndexOf((ListViewItem)value);
      void IList.Insert(int index, object value) => Insert(index, (ListViewItem)value);
      void IList.Remove(object value) => Remove((ListViewItem)value);
      void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);

      void ICollection<ListViewItem>.Add(ListViewItem item) => Add(item);
    }

    // ═══════════════════════════════════════════════
    // ColumnHeaderCollection
    // ═════════════════════════════════════════��═════

    public class ColumnHeaderCollection : IList<ColumnHeader>, IList {

      private readonly List<ColumnHeader> _list = new List<ColumnHeader>();
      private readonly ListView _owner;

      internal ColumnHeaderCollection(ListView owner) { _owner = owner; }

      public int Count => _list.Count;
      public bool IsReadOnly => false;
      public bool IsFixedSize => false;
      public bool IsSynchronized => false;
      public object SyncRoot => _list;

      public ColumnHeader this[int index] {
        get => _list[index];
        set {
          value.Index = index;
          value.ListView = _owner;
          _list[index] = value;
        }
      }

      public ColumnHeader this[string key] {
        get {
          foreach (var col in _list)
            if (string.Equals(col.Name, key, StringComparison.OrdinalIgnoreCase)) return col;
          return null;
        }
      }

      object IList.this[int index] {
        get => _list[index];
        set => this[index] = (ColumnHeader)value;
      }

      public int Add(ColumnHeader header) {
        header.Index = _list.Count;
        header.ListView = _owner;
        _list.Add(header);
        _owner.NotifyStateChanged();
        return _list.Count - 1;
      }

      public ColumnHeader Add(string text) {
        var header = new ColumnHeader(text);
        Add(header);
        return header;
      }

      public ColumnHeader Add(string text, int width) {
        var header = new ColumnHeader(text, width);
        Add(header);
        return header;
      }

      public ColumnHeader Add(string text, int width, HorizontalAlignment textAlign) {
        var header = new ColumnHeader(text, width, textAlign);
        Add(header);
        return header;
      }

      public ColumnHeader Add(string key, string text) {
        var header = new ColumnHeader(text) { Name = key };
        Add(header);
        return header;
      }

      public ColumnHeader Add(string key, string text, int width) {
        var header = new ColumnHeader(text, width) { Name = key };
        Add(header);
        return header;
      }

      public ColumnHeader Add(string key, string text, int width, HorizontalAlignment textAlign, int imageIndex) {
        var header = new ColumnHeader(text, width, textAlign) { Name = key, ImageIndex = imageIndex };
        Add(header);
        return header;
      }

      public void AddRange(ColumnHeader[] values) { foreach (var h in values) Add(h); }
      public void Clear() { _list.Clear(); _owner.NotifyStateChanged(); }
      public bool Contains(ColumnHeader item) => _list.Contains(item);
      public bool ContainsKey(string key) => this[key] != null;
      public void CopyTo(ColumnHeader[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
      public int IndexOf(ColumnHeader item) => _list.IndexOf(item);
      public int IndexOfKey(string key) {
        for (int i = 0; i < _list.Count; i++)
          if (string.Equals(_list[i].Name, key, StringComparison.OrdinalIgnoreCase)) return i;
        return -1;
      }
      public void Insert(int index, ColumnHeader header) {
        header.Index = index;
        header.ListView = _owner;
        _list.Insert(index, header);
        for (int i = index; i < _list.Count; i++) _list[i].Index = i;
        _owner.NotifyStateChanged();
      }
      public bool Remove(ColumnHeader item) { bool r = _list.Remove(item); if (r) _owner.NotifyStateChanged(); return r; }
      public void RemoveAt(int index) { _list.RemoveAt(index); _owner.NotifyStateChanged(); }
      public void RemoveByKey(string key) { int idx = IndexOfKey(key); if (idx >= 0) RemoveAt(idx); }

      public int GetColumnCount(ColumnHeaderAutoResizeStyle autoResize) => _list.Count;

      public IEnumerator<ColumnHeader> GetEnumerator() => _list.GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

      int IList.Add(object value) => Add((ColumnHeader)value);
      bool IList.Contains(object value) => _list.Contains((ColumnHeader)value);
      int IList.IndexOf(object value) => _list.IndexOf((ColumnHeader)value);
      void IList.Insert(int index, object value) => Insert(index, (ColumnHeader)value);
      void IList.Remove(object value) => Remove((ColumnHeader)value);
      void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);

      void ICollection<ColumnHeader>.Add(ColumnHeader item) => Add(item);
    }

    // ═══════════════════════════════════════════════
    // SelectedIndexCollection / SelectedListViewItemCollection
    // ═══════════════════════════════��═══════════════

    public class SelectedIndexCollection : IList<int>, IList {
      private readonly List<int> _list = new List<int>();
      internal void AddInternal(int i) => _list.Add(i);
      public int Count => _list.Count;
      public bool IsReadOnly => true;
      public bool IsFixedSize => true;
      public bool IsSynchronized => false;
      public object SyncRoot => _list;
      public int this[int index] { get => _list[index]; set { } }
      object IList.this[int index] { get => _list[index]; set { } }
      public bool Contains(int item) => _list.Contains(item);
      public void CopyTo(int[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
      public int IndexOf(int item) => _list.IndexOf(item);
      public IEnumerator<int> GetEnumerator() => _list.GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
      public void Add(int item) { } public void Clear() { } public void Insert(int index, int item) { } public bool Remove(int item) => false; public void RemoveAt(int index) { }
      int IList.Add(object value) => -1; bool IList.Contains(object value) => value is int i && _list.Contains(i); int IList.IndexOf(object value) => value is int i ? _list.IndexOf(i) : -1;
      void IList.Insert(int index, object value) { } void IList.Remove(object value) { } void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);
    }

    public class SelectedListViewItemCollection : IList<ListViewItem>, IList {
      private readonly List<ListViewItem> _list = new List<ListViewItem>();
      internal void AddInternal(ListViewItem i) => _list.Add(i);
      public int Count => _list.Count;
      public bool IsReadOnly => true;
      public bool IsFixedSize => true;
      public bool IsSynchronized => false;
      public object SyncRoot => _list;
      public ListViewItem this[int index] { get => _list[index]; set { } }
      object IList.this[int index] { get => _list[index]; set { } }
      public bool Contains(ListViewItem item) => _list.Contains(item);
      public bool ContainsKey(string key) { foreach (var i in _list) if (i.Name == key) return true; return false; }
      public void CopyTo(ListViewItem[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
      public int IndexOf(ListViewItem item) => _list.IndexOf(item);
      public int IndexOfKey(string key) { for (int i = 0; i < _list.Count; i++) if (_list[i].Name == key) return i; return -1; }
      public IEnumerator<ListViewItem> GetEnumerator() => _list.GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
      public void Add(ListViewItem item) { } public void Clear() { } public void Insert(int index, ListViewItem item) { } public bool Remove(ListViewItem item) => false; public void RemoveAt(int index) { }
      int IList.Add(object value) => -1; bool IList.Contains(object value) => _list.Contains((ListViewItem)value); int IList.IndexOf(object value) => _list.IndexOf((ListViewItem)value);
      void IList.Insert(int index, object value) { } void IList.Remove(object value) { } void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);
    }

    public class CheckedListViewItemCollection : IList<ListViewItem>, IList {
      private readonly List<ListViewItem> _list = new List<ListViewItem>();
      internal void AddInternal(ListViewItem i) => _list.Add(i);
      public int Count => _list.Count;
      public bool IsReadOnly => true;
      public bool IsFixedSize => true;
      public bool IsSynchronized => false;
      public object SyncRoot => _list;
      public ListViewItem this[int index] { get => _list[index]; set { } }
      object IList.this[int index] { get => _list[index]; set { } }
      public bool Contains(ListViewItem item) => _list.Contains(item);
      public void CopyTo(ListViewItem[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
      public int IndexOf(ListViewItem item) => _list.IndexOf(item);
      public IEnumerator<ListViewItem> GetEnumerator() => _list.GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
      public void Add(ListViewItem item) { } public void Clear() { } public void Insert(int index, ListViewItem item) { } public bool Remove(ListViewItem item) => false; public void RemoveAt(int index) { }
      int IList.Add(object value) => -1; bool IList.Contains(object value) => _list.Contains((ListViewItem)value); int IList.IndexOf(object value) => _list.IndexOf((ListViewItem)value);
      void IList.Insert(int index, object value) { } void IList.Remove(object value) { } void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);
    }

    public class CheckedIndexCollection : IList<int>, IList {
      private readonly List<int> _list = new List<int>();
      internal void AddInternal(int i) => _list.Add(i);
      public int Count => _list.Count;
      public bool IsReadOnly => true;
      public bool IsFixedSize => true;
      public bool IsSynchronized => false;
      public object SyncRoot => _list;
      public int this[int index] { get => _list[index]; set { } }
      object IList.this[int index] { get => _list[index]; set { } }
      public bool Contains(int item) => _list.Contains(item);
      public void CopyTo(int[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
      public int IndexOf(int item) => _list.IndexOf(item);
      public IEnumerator<int> GetEnumerator() => _list.GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
      public void Add(int item) { } public void Clear() { } public void Insert(int index, int item) { } public bool Remove(int item) => false; public void RemoveAt(int index) { }
      int IList.Add(object value) => -1; bool IList.Contains(object value) => value is int i && _list.Contains(i); int IList.IndexOf(object value) => value is int i ? _list.IndexOf(i) : -1;
      void IList.Insert(int index, object value) { } void IList.Remove(object value) { } void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);
    }
  }
}
