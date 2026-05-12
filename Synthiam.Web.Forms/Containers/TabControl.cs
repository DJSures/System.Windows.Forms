// =============================================================================
// Synthiam.Web.Forms - TabControl and TabPage for Blazor
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using BlazorMouseEventArgs = Microsoft.AspNetCore.Components.Web.MouseEventArgs;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // TabControl
  // ---------------------------------------------------------------------------
  public class TabControl : Control {

    // =========================================================================
    // TabPageCollection (nested inside TabControl for WinForms API compatibility)
    // =========================================================================
    public class TabPageCollection : IList<TabPage>, IList {

      internal readonly List<TabPage> _list = new List<TabPage>();
      private readonly TabControl _owner;

      internal TabPageCollection(TabControl owner) {
        _owner = owner;
      }

      public int Count => _list.Count;
      public bool IsReadOnly => false;
      public bool IsFixedSize => false;
      public bool IsSynchronized => false;
      public object SyncRoot => _list;

      public TabPage this[int index] {
        get => _list[index];
        set => _list[index] = value;
      }

      public TabPage this[string key] {
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
        set => _list[index] = (TabPage)value;
      }

      public void Add(TabPage page) {
        _list.Add(page);
        _owner.Controls.Add(page);
        _owner.NotifyStateChanged();
      }

      public void Add(string text) {
        var page = new TabPage(text);
        Add(page);
      }

      public void AddRange(TabPage[] pages) {
        foreach (var p in pages) Add(p);
      }

      public void Remove(TabPage page) {
        _list.Remove(page);
        _owner.Controls.Remove(page);
        _owner.NotifyStateChanged();
      }

      public bool Remove(string key) {
        var page = this[key];
        if (page != null) { Remove(page); return true; }
        return false;
      }

      public void RemoveAt(int index) {
        var page = _list[index];
        _list.RemoveAt(index);
        _owner.Controls.Remove(page);
        _owner.NotifyStateChanged();
      }

      public void Clear() {
        var copy = _list.ToArray();
        _list.Clear();
        foreach (var p in copy) _owner.Controls.Remove(p);
        _owner.NotifyStateChanged();
      }

      public bool Contains(TabPage page) => _list.Contains(page);
      public bool ContainsKey(string key) => this[key] != null;
      public int IndexOf(TabPage page) => _list.IndexOf(page);
      public int IndexOfKey(string key) {
        for (int i = 0; i < _list.Count; i++) {
          if (string.Equals(_list[i].Name, key, StringComparison.OrdinalIgnoreCase))
            return i;
        }
        return -1;
      }

      public void Insert(int index, TabPage page) {
        _list.Insert(index, page);
        _owner.Controls.Add(page);
        _owner.NotifyStateChanged();
      }

      public void CopyTo(TabPage[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
      public IEnumerator<TabPage> GetEnumerator() => _list.GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

      // IList explicit
      int IList.Add(object value) { Add((TabPage)value); return _list.Count - 1; }
      bool IList.Contains(object value) => _list.Contains((TabPage)value);
      int IList.IndexOf(object value) => _list.IndexOf((TabPage)value);
      void IList.Insert(int index, object value) => Insert(index, (TabPage)value);
      void IList.Remove(object value) => Remove((TabPage)value);
      void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);

      bool ICollection<TabPage>.Remove(TabPage page) {
        if (_list.Contains(page)) { Remove(page); return true; }
        return false;
      }
    }

    private TabPageCollection _tabPages;
    private int _selectedIndex;
    private TabAlignment _alignment = TabAlignment.Top;
    private TabSizeMode _sizeMode = TabSizeMode.Normal;
    private TabAppearance _tabAppearance = TabAppearance.Normal;
    private bool _multiline;
    private Size _itemSize = Size.Empty;
    private bool _hotTrack;
    private DrawMode _drawMode = DrawMode.Normal;
    private ImageList _imageList;

    public TabControl() {
      _tabPages = new TabPageCollection(this);
    }

    internal override bool ManagesChildLayout => true;

    // When controls are added directly via Controls.Add (as Designer does),
    // auto-register TabPage instances into the TabPages collection.
    protected internal override void OnControlAdded(ControlEventArgs e) {
      base.OnControlAdded(e);
      if (e.Control is TabPage page && !_tabPages.Contains(page)) {
        _tabPages._list.Add(page);
      }
    }

    protected internal override void OnControlRemoved(ControlEventArgs e) {
      base.OnControlRemoved(e);
      if (e.Control is TabPage page && _tabPages.Contains(page)) {
        _tabPages._list.Remove(page);
      }
    }

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public TabPageCollection TabPages => _tabPages;

    public TabPage SelectedTab {
      get => _selectedIndex >= 0 && _selectedIndex < _tabPages.Count ? _tabPages[_selectedIndex] : null;
      set {
        int idx = _tabPages.IndexOf(value);
        if (idx >= 0) SelectedIndex = idx;
      }
    }

    public int SelectedIndex {
      get => _selectedIndex;
      set {
        if (_selectedIndex != value && value >= -1) {
          int oldIndex = _selectedIndex;
          _selectedIndex = value;
          OnSelectedIndexChanged(EventArgs.Empty);
          NotifyStateChanged();
        }
      }
    }

    public int TabCount => _tabPages.Count;

    public TabAlignment Alignment {
      get => _alignment;
      set { _alignment = value; NotifyStateChanged(); }
    }

    public TabSizeMode SizeMode {
      get => _sizeMode;
      set { _sizeMode = value; NotifyStateChanged(); }
    }

    public new TabAppearance Appearance {
      get => _tabAppearance;
      set { _tabAppearance = value; NotifyStateChanged(); }
    }

    public bool Multiline {
      get => _multiline;
      set { _multiline = value; NotifyStateChanged(); }
    }

    public Size ItemSize {
      get => _itemSize;
      set => _itemSize = value;
    }

    public bool HotTrack {
      get => _hotTrack;
      set => _hotTrack = value;
    }

    public DrawMode DrawMode {
      get => _drawMode;
      set => _drawMode = value;
    }

    public ImageList ImageList {
      get => _imageList;
      set => _imageList = value;
    }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event EventHandler SelectedIndexChanged;
    public event TabControlEventHandler Selected;
    public event TabControlEventHandler Deselected;
    public event TabControlCancelEventHandler Selecting;
    public event TabControlCancelEventHandler Deselecting;
    public event DrawItemEventHandler DrawItem;

    protected virtual void OnSelectedIndexChanged(EventArgs e) { SelectedIndexChanged?.Invoke(this, e); }
    protected virtual void OnSelected(TabControlEventArgs e) { Selected?.Invoke(this, e); }
    protected virtual void OnDeselected(TabControlEventArgs e) { Deselected?.Invoke(this, e); }
    protected virtual void OnSelecting(TabControlCancelEventArgs e) { Selecting?.Invoke(this, e); }
    protected virtual void OnDeselecting(TabControlCancelEventArgs e) { Deselecting?.Invoke(this, e); }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public void SelectTab(int index) { SelectedIndex = index; }
    public void SelectTab(TabPage tabPage) { SelectedTab = tabPage; }
    public void SelectTab(string tabPageName) {
      int idx = _tabPages.IndexOfKey(tabPageName);
      if (idx >= 0) SelectedIndex = idx;
    }
    public void DeselectTab(int index) {
      if (_selectedIndex == index && _tabPages.Count > 0)
        SelectedIndex = index > 0 ? index - 1 : 0;
    }

    public Rectangle GetTabRect(int index) {
      // Stub: returns an approximate tab rectangle
      if (index < 0 || index >= _tabPages.Count) return Rectangle.Empty;
      int tabWidth = 80;
      return new Rectangle(index * tabWidth, 0, tabWidth, 24);
    }

    // ═══════════════════════════════════════════════
    // Rendering
    // ═══════════════════════════════════════════════

    protected override string GetCssClasses() {
      return "swf-control swf-tabcontrol";
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "display:flex;flex-direction:column;";
      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      // Tab strip
      builder.OpenElement(seq++, "div");
      builder.AddAttribute(seq++, "class", "swf-tabcontrol-strip");
      builder.AddAttribute(seq++, "style", "display:flex;flex-direction:row;flex:0 0 auto;border-bottom:1px solid #ccc;");

      for (int i = 0; i < _tabPages.Count; i++) {
        var page = _tabPages[i];
        bool isSelected = i == _selectedIndex;

        builder.OpenElement(seq++, "button");
        builder.AddAttribute(seq++, "class", isSelected ? "swf-tab-header swf-tab-active" : "swf-tab-header");
        builder.AddAttribute(seq++, "style",
          isSelected
            ? "padding:4px 12px;border:1px solid #ccc;border-bottom:none;background:#fff;cursor:pointer;margin-right:2px;"
            : "padding:4px 12px;border:1px solid transparent;border-bottom:1px solid #ccc;background:#f0f0f0;cursor:pointer;margin-right:2px;");

        int tabIndex = i;
        builder.AddAttribute(seq++, "onclick",
          EventCallback.Factory.Create<BlazorMouseEventArgs>(
            GetBlazorReceiver(),
            (BlazorMouseEventArgs _) => { SelectedIndex = tabIndex; }));

        builder.AddContent(seq++, page.Text ?? string.Empty);
        builder.CloseElement();
      }

      builder.CloseElement(); // tab strip
    }

    protected internal override void RenderChildren(RenderTreeBuilder builder, ref int seq) {
      // Only render selected tab content
      if (_selectedIndex >= 0 && _selectedIndex < _tabPages.Count) {
        var selectedPage = _tabPages[_selectedIndex];
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "swf-tabcontrol-content");
        builder.AddAttribute(seq++, "style", "flex:1 1 auto;position:relative;overflow:auto;");
        selectedPage.BuildRenderTree(builder, ref seq);
        builder.CloseElement();
      }
    }
  }

  // ---------------------------------------------------------------------------
  // TabPage (partial - extends ForwardDeclarations.cs stub)
  // ---------------------------------------------------------------------------
  public partial class TabPage : Panel {

    private int _imageIndex = -1;
    private string _imageKey = string.Empty;
    private string _toolTipTextPage = string.Empty;
    private bool _useVisualStyleBackColor = true;

    public TabPage() { }
    public TabPage(string text) : this() { Text = text; }

    public int ImageIndex {
      get => _imageIndex;
      set => _imageIndex = value;
    }

    public string ImageKey {
      get => _imageKey;
      set => _imageKey = value ?? string.Empty;
    }

    public string ToolTipText {
      get => _toolTipTextPage;
      set => _toolTipTextPage = value ?? string.Empty;
    }

    public bool UseVisualStyleBackColor {
      get => _useVisualStyleBackColor;
      set => _useVisualStyleBackColor = value;
    }

    protected override string GetCssClasses() {
      return "swf-control swf-tabpage";
    }
  }
}
