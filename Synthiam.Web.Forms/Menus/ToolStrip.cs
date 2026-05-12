// =============================================================================
// Synthiam.Web.Forms - ToolStrip control for Blazor
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // Supporting enums
  // ---------------------------------------------------------------------------

  public enum ToolStripGripStyle {
    Hidden = 0,
    Visible = 1
  }

  public enum ToolStripGripDisplayStyle {
    Horizontal = 0,
    Vertical = 1
  }

  // ---------------------------------------------------------------------------
  // ToolStripItemCollection
  // ---------------------------------------------------------------------------
  public class ToolStripItemCollection : IList<ToolStripItem>, IList {

    private readonly List<ToolStripItem> _list = new List<ToolStripItem>();
    private readonly ToolStrip _owner;

    internal ToolStripItemCollection(ToolStrip owner) {
      _owner = owner;
    }

    public int Count => _list.Count;
    public bool IsReadOnly => false;
    public bool IsFixedSize => false;
    public bool IsSynchronized => false;
    public object SyncRoot => _list;

    public ToolStripItem this[int index] {
      get => _list[index];
      set {
        _list[index] = value;
        value.Owner = _owner;
      }
    }

    public ToolStripItem this[string key] {
      get {
        for (int i = 0; i < _list.Count; i++) {
          if (string.Equals(_list[i].Name, key, StringComparison.OrdinalIgnoreCase))
            return _list[i];
        }
        return null;
      }
    }

    object IList.this[int index] {
      get => _list[index];
      set {
        var item = (ToolStripItem)value;
        _list[index] = item;
        item.Owner = _owner;
      }
    }

    public int Add(ToolStripItem item) {
      item.Owner = _owner;
      _list.Add(item);
      _owner?.NotifyStateChanged();
      return _list.Count - 1;
    }

    void ICollection<ToolStripItem>.Add(ToolStripItem item) => Add(item);

    public ToolStripItem Add(string text) {
      var item = new ToolStripButton { Text = text };
      Add(item);
      return item;
    }

    public ToolStripItem Add(string text, Image image) {
      var item = new ToolStripButton { Text = text, Image = image };
      Add(item);
      return item;
    }

    public ToolStripItem Add(string text, Image image, EventHandler onClick) {
      var item = new ToolStripButton { Text = text, Image = image };
      if (onClick != null) item.Click += onClick;
      Add(item);
      return item;
    }

    public void AddRange(ToolStripItem[] items) {
      foreach (var item in items) Add(item);
    }

    public void Remove(ToolStripItem item) {
      item.Owner = null;
      _list.Remove(item);
      _owner?.NotifyStateChanged();
    }

    public void RemoveAt(int index) {
      var item = _list[index];
      item.Owner = null;
      _list.RemoveAt(index);
      _owner?.NotifyStateChanged();
    }

    public void Clear() {
      foreach (var item in _list) item.Owner = null;
      _list.Clear();
      _owner?.NotifyStateChanged();
    }

    public bool Contains(ToolStripItem item) => _list.Contains(item);

    public bool ContainsKey(string key) => this[key] != null;

    public int IndexOf(ToolStripItem item) => _list.IndexOf(item);

    public int IndexOfKey(string key) {
      for (int i = 0; i < _list.Count; i++) {
        if (string.Equals(_list[i].Name, key, StringComparison.OrdinalIgnoreCase))
          return i;
      }
      return -1;
    }

    public ToolStripItem[] Find(string key, bool searchAllChildren) {
      var results = new List<ToolStripItem>();
      for (int i = 0; i < _list.Count; i++) {
        if (string.Equals(_list[i].Name, key, StringComparison.OrdinalIgnoreCase))
          results.Add(_list[i]);
      }
      return results.ToArray();
    }

    public void Insert(int index, ToolStripItem item) {
      item.Owner = _owner;
      _list.Insert(index, item);
      _owner?.NotifyStateChanged();
    }

    public void CopyTo(ToolStripItem[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
    public IEnumerator<ToolStripItem> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

    // IList explicit
    int IList.Add(object value) { return Add((ToolStripItem)value); }
    bool IList.Contains(object value) => _list.Contains((ToolStripItem)value);
    int IList.IndexOf(object value) => _list.IndexOf((ToolStripItem)value);
    void IList.Insert(int index, object value) => Insert(index, (ToolStripItem)value);
    void IList.Remove(object value) => Remove((ToolStripItem)value);
    void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);

    bool ICollection<ToolStripItem>.Remove(ToolStripItem item) {
      if (_list.Contains(item)) { Remove(item); return true; }
      return false;
    }
  }

  // ---------------------------------------------------------------------------
  // ToolStrip
  // ---------------------------------------------------------------------------
  public class ToolStrip : ScrollableControl {

    private ToolStripItemCollection _items;
    private ImageList _imageList;
    private ToolStripLayoutStyle _layoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
    private ToolStripRenderMode _renderMode = ToolStripRenderMode.ManagerRenderMode;
    private ToolStripGripStyle _gripStyle = ToolStripGripStyle.Visible;
    private bool _showItemToolTips = true;
    private bool _stretch;
    private bool _canOverflow = true;
    private Padding _gripMargin;
    private ToolStripGripDisplayStyle _gripDisplayStyle = ToolStripGripDisplayStyle.Vertical;

    public ToolStrip() {
      _items = new ToolStripItemCollection(this);
      Dock = DockStyle.Top;
    }

    public ToolStrip(params ToolStripItem[] items) : this() {
      if (items != null) _items.AddRange(items);
    }

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public ToolStripItemCollection Items => _items;

    public ToolStripTextDirection TextDirection { get; set; } = ToolStripTextDirection.Horizontal;

    public ImageList ImageList {
      get => _imageList;
      set => _imageList = value;
    }

    public ToolStripLayoutStyle LayoutStyle {
      get => _layoutStyle;
      set { _layoutStyle = value; NotifyStateChanged(); }
    }

    public ToolStripRenderMode RenderMode {
      get => _renderMode;
      set => _renderMode = value;
    }

    public ToolStripRenderer Renderer { get; set; }

    public ToolStripGripStyle GripStyle {
      get => _gripStyle;
      set { _gripStyle = value; NotifyStateChanged(); }
    }

    public bool ShowItemToolTips {
      get => _showItemToolTips;
      set => _showItemToolTips = value;
    }

    public bool Stretch {
      get => _stretch;
      set { _stretch = value; NotifyStateChanged(); }
    }

    public bool CanOverflow {
      get => _canOverflow;
      set => _canOverflow = value;
    }

    public ToolStripOverflowButton OverflowButton { get; } = new ToolStripOverflowButton();

    public Padding GripMargin {
      get => _gripMargin;
      set => _gripMargin = value;
    }

    public ToolStripGripDisplayStyle GripDisplayStyle {
      get => _gripDisplayStyle;
      set => _gripDisplayStyle = value;
    }

    public System.Drawing.Size ImageScalingSize { get; set; } = new System.Drawing.Size(16, 16);

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event ToolStripItemClickedEventHandler ItemClicked;
    public event EventHandler LayoutCompleted;

    protected virtual void OnItemClicked(ToolStripItemClickedEventArgs e) { ItemClicked?.Invoke(this, e); }

    // ═══════════════════════════════════════════════
    // Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "nav";

    protected override string GetCssClasses() {
      return "swf-control swf-toolstrip";
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "display:flex;align-items:center;";
      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      builder.OpenElement(seq++, "ul");
      builder.AddAttribute(seq++, "style", "list-style:none;margin:0;padding:0;display:flex;align-items:center;");

      for (int i = 0; i < _items.Count; i++) {
        _items[i].BuildRenderTree(builder, ref seq);
      }

      builder.CloseElement(); // ul
    }

    protected internal override void RenderChildren(RenderTreeBuilder builder, ref int seq) {
      // ToolStrip renders items via RenderContent, not child controls
    }
  }
}
