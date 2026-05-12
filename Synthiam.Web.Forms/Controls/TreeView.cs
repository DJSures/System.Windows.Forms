// =============================================================================
// Synthiam.Web.Forms - TreeView control for Blazor
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Synthiam.Web.Forms;
using BlazorMouseEventArgs = Microsoft.AspNetCore.Components.Web.MouseEventArgs;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // TreeNode (extends ForwardDeclarations partial)
  // ---------------------------------------------------------------------------
  public partial class TreeNode {

    private string _text = string.Empty;
    private TreeNodeCollection _nodes;
    private bool _isExpanded;

    public TreeNode() {
      _nodes = new TreeNodeCollection(this);
    }

    public TreeNode(string text) : this() {
      _text = text ?? string.Empty;
    }

    public TreeNode(string text, TreeNode[] children) : this(text) {
      if (children != null)
        _nodes.AddRange(children);
    }

    public TreeNode(string text, int imageIndex, int selectedImageIndex) : this(text) {
      ImageIndex = imageIndex;
      SelectedImageIndex = selectedImageIndex;
    }

    public TreeNode(string text, int imageIndex, int selectedImageIndex, TreeNode[] children) : this(text, children) {
      ImageIndex = imageIndex;
      SelectedImageIndex = selectedImageIndex;
    }

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public string Text {
      get => _text;
      set => _text = value ?? string.Empty;
    }

    public string Name { get; set; } = string.Empty;
    public object Tag { get; set; }
    public TreeNodeCollection Nodes => _nodes;
    public TreeNode Parent { get; internal set; }
    public TreeView TreeView { get; internal set; }
    public int ImageIndex { get; set; } = -1;
    public int SelectedImageIndex { get; set; } = -1;
    public string ImageKey { get; set; } = string.Empty;
    public string SelectedImageKey { get; set; } = string.Empty;

    public bool IsExpanded {
      get => _isExpanded;
      internal set => _isExpanded = value;
    }

    public bool IsSelected {
      get => TreeView?.SelectedNode == this;
    }

    public bool Checked { get; set; }

    public int Level {
      get {
        int level = 0;
        TreeNode p = Parent;
        while (p != null) { level++; p = p.Parent; }
        return level;
      }
    }

    public string FullPath {
      get {
        if (Parent == null) return _text;
        string separator = TreeView?.PathSeparator ?? "\\";
        return Parent.FullPath + separator + _text;
      }
    }

    public TreeNode FirstNode => _nodes.Count > 0 ? _nodes[0] : null;
    public TreeNode LastNode => _nodes.Count > 0 ? _nodes[_nodes.Count - 1] : null;

    public TreeNode NextNode {
      get {
        if (Parent == null) return null;
        int idx = Parent._nodes.IndexOf(this);
        return (idx >= 0 && idx + 1 < Parent._nodes.Count) ? Parent._nodes[idx + 1] : null;
      }
    }

    public TreeNode PrevNode {
      get {
        if (Parent == null) return null;
        int idx = Parent._nodes.IndexOf(this);
        return (idx > 0) ? Parent._nodes[idx - 1] : null;
      }
    }

    public TreeNode NextVisibleNode => NextNode;
    public TreeNode PrevVisibleNode => PrevNode;

    public Font NodeFont { get; set; }
    public Color ForeColor { get; set; } = Color.Empty;
    public Color BackColor { get; set; } = Color.Empty;
    public string ToolTipText { get; set; } = string.Empty;
    public int Index { get; internal set; }
    public Rectangle Bounds => Rectangle.Empty;
    public int StateImageIndex { get; set; } = -1;
    public string StateImageKey { get; set; } = string.Empty;

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public void Expand() {
      _isExpanded = true;
      TreeView?.NotifyStateChanged();
    }

    public void Collapse() {
      _isExpanded = false;
      TreeView?.NotifyStateChanged();
    }

    public void ExpandAll() {
      _isExpanded = true;
      foreach (TreeNode child in _nodes)
        child.ExpandAll();
      TreeView?.NotifyStateChanged();
    }

    public void Toggle() {
      if (_isExpanded) Collapse(); else Expand();
    }

    public void Remove() {
      Parent?.Nodes.Remove(this);
    }

    public void EnsureVisible() {
      // Expand all parents
      TreeNode p = Parent;
      while (p != null) {
        p._isExpanded = true;
        p = p.Parent;
      }
      TreeView?.NotifyStateChanged();
    }

    public int GetNodeCount(bool includeSubTrees) {
      int count = _nodes.Count;
      if (includeSubTrees) {
        foreach (TreeNode child in _nodes)
          count += child.GetNodeCount(true);
      }
      return count;
    }

    public object Clone() {
      var clone = new TreeNode(_text);
      clone.ImageIndex = ImageIndex;
      clone.SelectedImageIndex = SelectedImageIndex;
      clone.Tag = Tag;
      clone.Checked = Checked;
      foreach (TreeNode child in _nodes)
        clone.Nodes.Add((TreeNode)child.Clone());
      return clone;
    }

    public override string ToString() => "TreeNode: " + _text;
  }

  // ---------------------------------------------------------------------------
  // TreeNodeCollection
  // ---------------------------------------------------------------------------
  public class TreeNodeCollection : IList<TreeNode>, IList {

    private readonly List<TreeNode> _list = new List<TreeNode>();
    private readonly TreeNode _ownerNode;
    private readonly TreeView _ownerTree;

    internal TreeNodeCollection(TreeNode owner) {
      _ownerNode = owner;
    }

    internal TreeNodeCollection(TreeView owner) {
      _ownerTree = owner;
    }

    private TreeView GetTreeView() => _ownerNode?.TreeView ?? _ownerTree;

    public int Count => _list.Count;
    public bool IsReadOnly => false;
    public bool IsFixedSize => false;
    public bool IsSynchronized => false;
    public object SyncRoot => _list;

    public TreeNode this[int index] {
      get => _list[index];
      set {
        value.Parent = _ownerNode;
        value.TreeView = GetTreeView();
        value.Index = index;
        _list[index] = value;
      }
    }

    public TreeNode this[string key] {
      get {
        foreach (var node in _list)
          if (string.Equals(node.Name, key, StringComparison.OrdinalIgnoreCase)) return node;
        return null;
      }
    }

    object IList.this[int index] {
      get => _list[index];
      set => this[index] = (TreeNode)value;
    }

    private void SetOwnership(TreeNode node, int index) {
      node.Parent = _ownerNode;
      node.TreeView = GetTreeView();
      node.Index = index;
    }

    public TreeNode Add(string text) {
      var node = new TreeNode(text);
      return Add(node);
    }

    public TreeNode Add(string key, string text) {
      var node = new TreeNode(text) { Name = key };
      return Add(node);
    }

    public TreeNode Add(string key, string text, int imageIndex) {
      var node = new TreeNode(text, imageIndex, imageIndex) { Name = key };
      return Add(node);
    }

    public TreeNode Add(string key, string text, string imageKey) {
      var node = new TreeNode(text) { Name = key, ImageKey = imageKey };
      return Add(node);
    }

    public TreeNode Add(string key, string text, int imageIndex, int selectedImageIndex) {
      var node = new TreeNode(text, imageIndex, selectedImageIndex) { Name = key };
      return Add(node);
    }

    public TreeNode Add(TreeNode node) {
      SetOwnership(node, _list.Count);
      _list.Add(node);
      GetTreeView()?.NotifyStateChanged();
      return node;
    }

    public void AddRange(TreeNode[] nodes) {
      if (nodes == null) return;
      foreach (var n in nodes) Add(n);
    }

    public void Clear() {
      _list.Clear();
      GetTreeView()?.NotifyStateChanged();
    }

    public bool Contains(TreeNode node) => _list.Contains(node);

    public bool ContainsKey(string key) => this[key] != null;

    public void CopyTo(TreeNode[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    public TreeNode[] Find(string key, bool searchAllChildren) {
      var result = new List<TreeNode>();
      FindRecursive(key, searchAllChildren, result);
      return result.ToArray();
    }

    private void FindRecursive(string key, bool searchChildren, List<TreeNode> result) {
      foreach (var node in _list) {
        if (string.Equals(node.Name, key, StringComparison.OrdinalIgnoreCase))
          result.Add(node);
        if (searchChildren)
          node.Nodes.FindRecursive(key, true, result);
      }
    }

    public int IndexOf(TreeNode node) => _list.IndexOf(node);

    public int IndexOfKey(string key) {
      for (int i = 0; i < _list.Count; i++)
        if (string.Equals(_list[i].Name, key, StringComparison.OrdinalIgnoreCase)) return i;
      return -1;
    }

    public void Insert(int index, TreeNode node) {
      SetOwnership(node, index);
      _list.Insert(index, node);
      for (int i = index; i < _list.Count; i++) _list[i].Index = i;
      GetTreeView()?.NotifyStateChanged();
    }

    public void Insert(int index, string text) {
      Insert(index, new TreeNode(text));
    }

    public bool Remove(TreeNode node) {
      bool result = _list.Remove(node);
      if (result) {
        node.Parent = null;
        for (int i = 0; i < _list.Count; i++) _list[i].Index = i;
        GetTreeView()?.NotifyStateChanged();
      }
      return result;
    }

    public void RemoveAt(int index) {
      var node = _list[index];
      _list.RemoveAt(index);
      node.Parent = null;
      for (int i = index; i < _list.Count; i++) _list[i].Index = i;
      GetTreeView()?.NotifyStateChanged();
    }

    public void RemoveByKey(string key) {
      int idx = IndexOfKey(key);
      if (idx >= 0) RemoveAt(idx);
    }

    public IEnumerator<TreeNode> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

    int IList.Add(object value) { Add((TreeNode)value); return _list.Count - 1; }
    bool IList.Contains(object value) => _list.Contains((TreeNode)value);
    int IList.IndexOf(object value) => _list.IndexOf((TreeNode)value);
    void IList.Insert(int index, object value) => Insert(index, (TreeNode)value);
    void IList.Remove(object value) => Remove((TreeNode)value);
    void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);

    void ICollection<TreeNode>.Add(TreeNode item) => Add(item);
  }

  // ---------------------------------------------------------------------------
  // TreeView
  // ---------------------------------------------------------------------------
  public class TreeView : Control {

    private TreeNodeCollection _nodes;
    private TreeNode _selectedNode;
    private ImageList _imageList;
    private ImageList _stateImageList;
    private int _imageIndex = -1;
    private int _selectedImageIndex = -1;
    private string _imageKey = string.Empty;
    private string _selectedImageKey = string.Empty;
    private string _pathSeparator = "\\";
    private int _indent = 19;
    private int _itemHeight = 16;
    private bool _showLines = true;
    private bool _showPlusMinus = true;
    private bool _showRootLines = true;
    private bool _hideSelection = true;
    private bool _labelEdit;
    private bool _scrollable = true;
    private bool _hotTracking;
    private bool _checkBoxes;
    private bool _fullRowSelect;
    private bool _showNodeToolTips;
    private bool _sorted;
    private TreeViewDrawMode _drawMode = TreeViewDrawMode.Normal;
    private Color _lineColor = Color.Black;
    private BorderStyle _borderStyle = BorderStyle.Fixed3D;

    public TreeView() {
      _nodes = new TreeNodeCollection(this);
    }

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public TreeNodeCollection Nodes => _nodes;

    public TreeNode SelectedNode {
      get => _selectedNode;
      set {
        if (_selectedNode != value) {
          var oldNode = _selectedNode;
          _selectedNode = value;
          OnAfterSelect(new TreeViewEventArgs(value, TreeViewAction.ByMouse));
          NotifyStateChanged();
        }
      }
    }

    public TreeNode TopNode => _nodes.Count > 0 ? _nodes[0] : null;
    public ImageList ImageList { get => _imageList; set => _imageList = value; }
    public ImageList StateImageList { get => _stateImageList; set => _stateImageList = value; }
    public int ImageIndex { get => _imageIndex; set => _imageIndex = value; }
    public int SelectedImageIndex { get => _selectedImageIndex; set => _selectedImageIndex = value; }
    public string ImageKey { get => _imageKey; set => _imageKey = value ?? string.Empty; }
    public string SelectedImageKey { get => _selectedImageKey; set => _selectedImageKey = value ?? string.Empty; }
    public string PathSeparator { get => _pathSeparator; set => _pathSeparator = value ?? "\\"; }
    public int Indent { get => _indent; set => _indent = value; }
    public int ItemHeight { get => _itemHeight; set { _itemHeight = value; NotifyStateChanged(); } }
    public bool ShowLines { get => _showLines; set { _showLines = value; NotifyStateChanged(); } }
    public bool ShowPlusMinus { get => _showPlusMinus; set { _showPlusMinus = value; NotifyStateChanged(); } }
    public bool ShowRootLines { get => _showRootLines; set { _showRootLines = value; NotifyStateChanged(); } }
    public bool HideSelection { get => _hideSelection; set => _hideSelection = value; }
    public bool LabelEdit { get => _labelEdit; set => _labelEdit = value; }
    public bool Scrollable { get => _scrollable; set => _scrollable = value; }
    public bool HotTracking { get => _hotTracking; set => _hotTracking = value; }
    public bool CheckBoxes { get => _checkBoxes; set { _checkBoxes = value; NotifyStateChanged(); } }
    public bool FullRowSelect { get => _fullRowSelect; set => _fullRowSelect = value; }
    public bool ShowNodeToolTips { get => _showNodeToolTips; set => _showNodeToolTips = value; }
    public bool Sorted { get => _sorted; set => _sorted = value; }
    public TreeViewDrawMode DrawMode { get => _drawMode; set => _drawMode = value; }
    public Color LineColor { get => _lineColor; set => _lineColor = value; }
    public BorderStyle BorderStyle { get => _borderStyle; set { _borderStyle = value; NotifyStateChanged(); } }

    public int VisibleCount => 10; // Stub

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event TreeViewEventHandler AfterSelect;
    public event TreeViewCancelEventHandler BeforeSelect;
    public event TreeViewEventHandler AfterExpand;
    public event TreeViewCancelEventHandler BeforeExpand;
    public event TreeViewEventHandler AfterCollapse;
    public event TreeViewCancelEventHandler BeforeCollapse;
    public event TreeViewEventHandler AfterCheck;
    public event TreeViewCancelEventHandler BeforeCheck;
    public event TreeNodeMouseClickEventHandler NodeMouseClick;
    public event TreeNodeMouseClickEventHandler NodeMouseDoubleClick;
    public event EventHandler AfterLabelEdit;
    public event EventHandler BeforeLabelEdit;
    public event EventHandler DrawNode;
    public event ItemDragEventHandler ItemDrag;

    protected virtual void OnAfterSelect(TreeViewEventArgs e) { AfterSelect?.Invoke(this, e); }
    protected virtual void OnBeforeSelect(TreeViewCancelEventArgs e) { BeforeSelect?.Invoke(this, e); }
    protected virtual void OnAfterExpand(TreeViewEventArgs e) { AfterExpand?.Invoke(this, e); }
    protected virtual void OnBeforeExpand(TreeViewCancelEventArgs e) { BeforeExpand?.Invoke(this, e); }
    protected virtual void OnAfterCollapse(TreeViewEventArgs e) { AfterCollapse?.Invoke(this, e); }
    protected virtual void OnBeforeCollapse(TreeViewCancelEventArgs e) { BeforeCollapse?.Invoke(this, e); }
    protected virtual void OnAfterCheck(TreeViewEventArgs e) { AfterCheck?.Invoke(this, e); }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public TreeNode GetNodeAt(Point pt) => null;
    public TreeNode GetNodeAt(int x, int y) => null;

    public void ExpandAll() {
      foreach (TreeNode node in _nodes)
        node.ExpandAll();
      NotifyStateChanged();
    }

    public void CollapseAll() {
      CollapseAllRecursive(_nodes);
      NotifyStateChanged();
    }

    private void CollapseAllRecursive(TreeNodeCollection nodes) {
      foreach (TreeNode node in nodes) {
        node.IsExpanded = false;
        CollapseAllRecursive(node.Nodes);
      }
    }

    public new void Sort() {
      if (_sorted) {
        SortRecursive(_nodes);
        NotifyStateChanged();
      }
    }

    private void SortRecursive(TreeNodeCollection nodes) {
      // Simple sort by text
      var arr = new TreeNode[nodes.Count];
      for (int i = 0; i < nodes.Count; i++) arr[i] = nodes[i];
      Array.Sort(arr, (a, b) => string.Compare(a.Text, b.Text, StringComparison.OrdinalIgnoreCase));
      nodes.Clear();
      nodes.AddRange(arr);
      foreach (var n in arr) SortRecursive(n.Nodes);
    }

    public void BeginUpdate() { }
    public void EndUpdate() { NotifyStateChanged(); }

    public int GetNodeCount(bool includeSubTrees) {
      int count = _nodes.Count;
      if (includeSubTrees) {
        foreach (TreeNode node in _nodes)
          count += node.GetNodeCount(true);
      }
      return count;
    }

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

      if (_scrollable)
        style += "overflow:auto;";

      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      builder.OpenElement(seq++, "ul");
      builder.AddAttribute(seq++, "style", "list-style:none;margin:0;padding:0;");

      RenderNodes(builder, ref seq, _nodes, 0);

      builder.CloseElement(); // ul
    }

    private void RenderNodes(RenderTreeBuilder builder, ref int seq, TreeNodeCollection nodes, int level) {
      foreach (TreeNode node in nodes) {
        builder.OpenElement(seq++, "li");
        builder.AddAttribute(seq++, "style", "list-style:none;");

        // Node row
        builder.OpenElement(seq++, "div");

        string nodeStyle = "display:flex;align-items:center;padding:2px 4px;cursor:pointer;" +
                           "padding-left:" + (level * _indent) + "px;" +
                           "min-height:" + _itemHeight + "px;";

        if (node == _selectedNode)
          nodeStyle += "background-color:#0078d7;color:white;";
        else if (!node.BackColor.IsEmpty)
          nodeStyle += "background-color:" + node.BackColor.ToCss() + ";";
        if (!node.ForeColor.IsEmpty && node != _selectedNode)
          nodeStyle += "color:" + node.ForeColor.ToCss() + ";";

        builder.AddAttribute(seq++, "style", nodeStyle);

        // Capture node for closure
        var capturedNode = node;

        builder.AddAttribute(seq++, "onclick",
          EventCallback.Factory.Create<BlazorMouseEventArgs>(
            GetBlazorReceiver(),
            (BlazorMouseEventArgs e) => {
              SelectedNode = capturedNode;
            }));

        // Expand/collapse toggle
        if (_showPlusMinus && node.Nodes.Count > 0) {
          builder.OpenElement(seq++, "span");
          builder.AddAttribute(seq++, "style", "width:16px;text-align:center;user-select:none;flex-shrink:0;");
          builder.AddAttribute(seq++, "onclick",
            EventCallback.Factory.Create<BlazorMouseEventArgs>(
              GetBlazorReceiver(),
              (BlazorMouseEventArgs e) => {
                capturedNode.Toggle();
              }));
          builder.AddContent(seq++, node.IsExpanded ? "\u25BC" : "\u25B6");
          builder.CloseElement(); // toggle span
        } else if (_showPlusMinus) {
          builder.OpenElement(seq++, "span");
          builder.AddAttribute(seq++, "style", "width:16px;flex-shrink:0;");
          builder.CloseElement(); // spacer
        }

        // Checkbox
        if (_checkBoxes) {
          builder.OpenElement(seq++, "input");
          builder.AddAttribute(seq++, "type", "checkbox");
          if (node.Checked) builder.AddAttribute(seq++, "checked", true);
          builder.AddAttribute(seq++, "onchange",
            EventCallback.Factory.Create<ChangeEventArgs>(
              GetBlazorReceiver(),
              (ChangeEventArgs ev) => {
                capturedNode.Checked = !capturedNode.Checked;
                OnAfterCheck(new TreeViewEventArgs(capturedNode, TreeViewAction.ByMouse));
                NotifyStateChanged();
              }));
          builder.AddAttribute(seq++, "style", "margin:0 4px 0 0;flex-shrink:0;");
          builder.CloseElement(); // input
        }

        // Node text
        builder.OpenElement(seq++, "span");
        builder.AddAttribute(seq++, "style", "white-space:nowrap;overflow:hidden;text-overflow:ellipsis;");
        builder.AddContent(seq++, node.Text);
        builder.CloseElement(); // text span

        builder.CloseElement(); // node row div

        // Children
        if (node.IsExpanded && node.Nodes.Count > 0) {
          builder.OpenElement(seq++, "ul");
          builder.AddAttribute(seq++, "style", "list-style:none;margin:0;padding:0;");
          RenderNodes(builder, ref seq, node.Nodes, level + 1);
          builder.CloseElement(); // ul
        }

        builder.CloseElement(); // li
      }
    }
  }
}
