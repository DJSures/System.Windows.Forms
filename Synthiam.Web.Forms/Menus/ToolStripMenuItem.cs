// =============================================================================
// Synthiam.Web.Forms - ToolStripMenuItem and related items for Blazor
// =============================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Synthiam.Web.Forms;
using BlazorMouseEventArgs = Microsoft.AspNetCore.Components.Web.MouseEventArgs;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // ToolStripMenuItem
  // ---------------------------------------------------------------------------
  public class ToolStripMenuItem : ToolStripItem {

    private ToolStripItemCollection _dropDownItems;
    private bool _checked;
    private CheckState _checkState = CheckState.Unchecked;
    private bool _checkOnClick;
    private Keys _shortcutKeys = Keys.None;
    private string _shortcutKeyDisplayString;
    private bool _showShortcutKeys = true;
    private ToolStripDropDown _dropDown;

    public ToolStripMenuItem() { }
    public ToolStripMenuItem(string text) { Text = text; }
    public ToolStripMenuItem(string text, Image image) { Text = text; Image = image; }
    public ToolStripMenuItem(string text, Image image, EventHandler onClick) {
      Text = text;
      Image = image;
      if (onClick != null) Click += onClick;
    }
    public ToolStripMenuItem(string text, Image image, EventHandler onClick, string name) {
      Text = text;
      Image = image;
      Name = name;
      if (onClick != null) Click += onClick;
    }
    public ToolStripMenuItem(string text, Image image, EventHandler onClick, Keys shortcutKeys) {
      Text = text;
      Image = image;
      _shortcutKeys = shortcutKeys;
      if (onClick != null) Click += onClick;
    }
    public ToolStripMenuItem(string text, Image image, params ToolStripItem[] dropDownItems) {
      Text = text;
      Image = image;
      if (dropDownItems != null) DropDownItems.AddRange(dropDownItems);
    }

    public ToolStripItemCollection DropDownItems {
      get {
        if (_dropDownItems == null) {
          // Create a temporary ToolStrip to host the collection
          if (_dropDown == null) _dropDown = new ToolStripDropDownMenu();
          _dropDownItems = _dropDown.Items;
        }
        return _dropDownItems;
      }
    }

    public bool Checked {
      get => _checked;
      set {
        if (_checked != value) {
          _checked = value;
          _checkState = value ? CheckState.Checked : CheckState.Unchecked;
          CheckedChanged?.Invoke(this, EventArgs.Empty);
          CheckStateChanged?.Invoke(this, EventArgs.Empty);
          Owner?.NotifyStateChanged();
        }
      }
    }

    public CheckState CheckState {
      get => _checkState;
      set {
        if (_checkState != value) {
          _checkState = value;
          _checked = value != CheckState.Unchecked;
          CheckStateChanged?.Invoke(this, EventArgs.Empty);
          CheckedChanged?.Invoke(this, EventArgs.Empty);
          Owner?.NotifyStateChanged();
        }
      }
    }

    public bool CheckOnClick {
      get => _checkOnClick;
      set => _checkOnClick = value;
    }

    public Keys ShortcutKeys {
      get => _shortcutKeys;
      set => _shortcutKeys = value;
    }

    public string ShortcutKeyDisplayString {
      get => _shortcutKeyDisplayString;
      set => _shortcutKeyDisplayString = value;
    }

    public bool ShowShortcutKeys {
      get => _showShortcutKeys;
      set => _showShortcutKeys = value;
    }

    public bool HasDropDownItems => _dropDownItems != null && _dropDownItems.Count > 0;

    public ToolStripDropDown DropDown {
      get => _dropDown ?? (_dropDown = new ToolStripDropDownMenu());
      set => _dropDown = value;
    }

    public event EventHandler CheckedChanged;
    public event EventHandler CheckStateChanged;
    public event EventHandler DropDownOpening;
    public event EventHandler DropDownOpened;
    public event EventHandler DropDownClosed;

    public void ShowDropDown() { DropDownOpening?.Invoke(this, EventArgs.Empty); DropDownOpened?.Invoke(this, EventArgs.Empty); }
    public void HideDropDown() { DropDownClosed?.Invoke(this, EventArgs.Empty); }

    internal override void BuildRenderTree(RenderTreeBuilder builder, ref int seq) {
      if (!Visible) return;

      builder.OpenElement(seq++, "li");
      builder.AddAttribute(seq++, "class", "swf-toolstrip-menuitem");
      builder.AddAttribute(seq++, "style", "position:relative;list-style:none;");

      builder.OpenElement(seq++, "button");
      builder.AddAttribute(seq++, "class", _checked ? "swf-menuitem-btn swf-menuitem-checked" : "swf-menuitem-btn");
      builder.AddAttribute(seq++, "style", "border:none;background:transparent;cursor:pointer;padding:4px 8px;display:flex;align-items:center;width:100%;text-align:left;white-space:nowrap;");

      if (!Enabled)
        builder.AddAttribute(seq++, "disabled", true);

      if (ToolTipText != null)
        builder.AddAttribute(seq++, "title", ToolTipText);

      builder.AddAttribute(seq++, "onclick",
        EventCallback.Factory.Create<BlazorMouseEventArgs>(
          GetBlazorReceiver(),
          (BlazorMouseEventArgs _) => {
            if (_checkOnClick) Checked = !Checked;
            PerformClick();
          }));

      // Checkbox indicator
      if (_checked) {
        builder.OpenElement(seq++, "span");
        builder.AddAttribute(seq++, "style", "margin-right:4px;");
        builder.AddContent(seq++, "\u2713");
        builder.CloseElement();
      }

      // Image
      if (Image != null && (DisplayStyle == ToolStripItemDisplayStyle.Image || DisplayStyle == ToolStripItemDisplayStyle.ImageAndText)) {
        var dataUri = Image.ToDataUri();
        if (!string.IsNullOrEmpty(dataUri)) {
          builder.OpenElement(seq++, "img");
          builder.AddAttribute(seq++, "src", dataUri);
          builder.AddAttribute(seq++, "style", "width:16px;height:16px;margin-right:4px;");
          builder.CloseElement();
        }
      }

      // Text
      if (!string.IsNullOrEmpty(Text) && (DisplayStyle == ToolStripItemDisplayStyle.Text || DisplayStyle == ToolStripItemDisplayStyle.ImageAndText)) {
        builder.OpenElement(seq++, "span");
        builder.AddContent(seq++, Text);
        builder.CloseElement();
      }

      // Shortcut key display
      if (_showShortcutKeys && _shortcutKeys != Keys.None) {
        var display = _shortcutKeyDisplayString ?? _shortcutKeys.ToString();
        builder.OpenElement(seq++, "span");
        builder.AddAttribute(seq++, "style", "margin-left:auto;padding-left:16px;color:#888;font-size:0.85em;");
        builder.AddContent(seq++, display);
        builder.CloseElement();
      }

      // Sub-menu arrow
      if (HasDropDownItems) {
        builder.OpenElement(seq++, "span");
        builder.AddAttribute(seq++, "style", "margin-left:8px;");
        builder.AddContent(seq++, "\u25B6");
        builder.CloseElement();
      }

      builder.CloseElement(); // button

      // Drop-down sub-menu
      if (HasDropDownItems) {
        builder.OpenElement(seq++, "ul");
        builder.AddAttribute(seq++, "class", "swf-toolstrip-dropdown");
        builder.AddAttribute(seq++, "style", "display:none;position:absolute;left:100%;top:0;list-style:none;margin:0;padding:2px;background:#fff;border:1px solid #ccc;box-shadow:2px 2px 4px rgba(0,0,0,0.2);z-index:1000;");

        for (int i = 0; i < _dropDownItems.Count; i++) {
          _dropDownItems[i].BuildRenderTree(builder, ref seq);
        }

        builder.CloseElement(); // ul
      }

      builder.CloseElement(); // li
    }
  }

  // ---------------------------------------------------------------------------
  // ToolStripSeparator
  // ---------------------------------------------------------------------------
  public class ToolStripSeparator : ToolStripItem {

    public ToolStripSeparator() { }

    internal override void BuildRenderTree(RenderTreeBuilder builder, ref int seq) {
      if (!Visible) return;

      builder.OpenElement(seq++, "li");
      builder.AddAttribute(seq++, "class", "swf-toolstrip-separator");
      builder.AddAttribute(seq++, "style", "list-style:none;");

      builder.OpenElement(seq++, "hr");
      builder.AddAttribute(seq++, "style", "margin:2px 0;border:none;border-top:1px solid #ccc;");
      builder.CloseElement();

      builder.CloseElement(); // li
    }
  }

  // ---------------------------------------------------------------------------
  // ToolStripButton
  // ---------------------------------------------------------------------------
  public class ToolStripButton : ToolStripItem {

    private bool _checked;
    private CheckState _checkState = CheckState.Unchecked;
    private bool _checkOnClick;

    public ToolStripButton() { }
    public ToolStripButton(string text) { Text = text; }
    public ToolStripButton(string text, Image image) { Text = text; Image = image; }
    public ToolStripButton(string text, Image image, EventHandler onClick) {
      Text = text;
      Image = image;
      if (onClick != null) Click += onClick;
    }
    public ToolStripButton(string text, Image image, EventHandler onClick, string name) {
      Text = text;
      Image = image;
      Name = name;
      if (onClick != null) Click += onClick;
    }

    public bool Checked {
      get => _checked;
      set {
        if (_checked != value) {
          _checked = value;
          _checkState = value ? CheckState.Checked : CheckState.Unchecked;
          CheckedChanged?.Invoke(this, EventArgs.Empty);
          CheckStateChanged?.Invoke(this, EventArgs.Empty);
          Owner?.NotifyStateChanged();
        }
      }
    }

    public CheckState CheckState {
      get => _checkState;
      set {
        if (_checkState != value) {
          _checkState = value;
          _checked = value != CheckState.Unchecked;
          CheckStateChanged?.Invoke(this, EventArgs.Empty);
          CheckedChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    public bool CheckOnClick {
      get => _checkOnClick;
      set => _checkOnClick = value;
    }

    public event EventHandler CheckedChanged;
    public event EventHandler CheckStateChanged;

    internal override void BuildRenderTree(RenderTreeBuilder builder, ref int seq) {
      if (!Visible) return;

      builder.OpenElement(seq++, "li");
      builder.AddAttribute(seq++, "class", "swf-toolstrip-item");
      builder.AddAttribute(seq++, "style", "display:inline-flex;align-items:center;list-style:none;");

      builder.OpenElement(seq++, "button");
      builder.AddAttribute(seq++, "class", _checked ? "swf-toolstrip-btn swf-toolstrip-btn-checked" : "swf-toolstrip-btn");
      builder.AddAttribute(seq++, "style", _checked
        ? "border:1px solid #999;background:#ddd;cursor:pointer;padding:2px 6px;display:inline-flex;align-items:center;"
        : "border:none;background:transparent;cursor:pointer;padding:2px 6px;display:inline-flex;align-items:center;");

      if (!Enabled)
        builder.AddAttribute(seq++, "disabled", true);

      if (ToolTipText != null)
        builder.AddAttribute(seq++, "title", ToolTipText);

      builder.AddAttribute(seq++, "onclick",
        EventCallback.Factory.Create<BlazorMouseEventArgs>(
          GetBlazorReceiver(),
          (BlazorMouseEventArgs _) => {
            if (_checkOnClick) Checked = !Checked;
            PerformClick();
          }));

      // Image
      if (Image != null && (DisplayStyle == ToolStripItemDisplayStyle.Image || DisplayStyle == ToolStripItemDisplayStyle.ImageAndText)) {
        var dataUri = Image.ToDataUri();
        if (!string.IsNullOrEmpty(dataUri)) {
          builder.OpenElement(seq++, "img");
          builder.AddAttribute(seq++, "src", dataUri);
          builder.AddAttribute(seq++, "style", "width:16px;height:16px;margin-right:2px;");
          builder.CloseElement();
        }
      }

      // Text
      if (!string.IsNullOrEmpty(Text) && (DisplayStyle == ToolStripItemDisplayStyle.Text || DisplayStyle == ToolStripItemDisplayStyle.ImageAndText)) {
        builder.OpenElement(seq++, "span");
        builder.AddContent(seq++, Text);
        builder.CloseElement();
      }

      builder.CloseElement(); // button
      builder.CloseElement(); // li
    }
  }

  // ---------------------------------------------------------------------------
  // ToolStripLabel
  // ---------------------------------------------------------------------------
  public class ToolStripLabel : ToolStripItem {

    private bool _isLink;
    private Color _linkColor = Color.Blue;
    private Color _visitedLinkColor = Color.Purple;
    private Color _activeLinkColor = Color.Red;
    private LinkBehavior _linkBehavior = LinkBehavior.SystemDefault;
    private bool _linkVisited;

    public ToolStripLabel() { }
    public ToolStripLabel(string text) { Text = text; }
    public ToolStripLabel(string text, Image image, bool isLink) {
      Text = text;
      Image = image;
      _isLink = isLink;
    }
    public ToolStripLabel(string text, Image image, bool isLink, EventHandler onClick) {
      Text = text;
      Image = image;
      _isLink = isLink;
      if (onClick != null) Click += onClick;
    }

    public bool IsLink {
      get => _isLink;
      set { _isLink = value; Owner?.NotifyStateChanged(); }
    }

    public Color LinkColor {
      get => _linkColor;
      set => _linkColor = value;
    }

    public Color VisitedLinkColor {
      get => _visitedLinkColor;
      set => _visitedLinkColor = value;
    }

    public Color ActiveLinkColor {
      get => _activeLinkColor;
      set => _activeLinkColor = value;
    }

    public LinkBehavior LinkBehavior {
      get => _linkBehavior;
      set => _linkBehavior = value;
    }

    public bool LinkVisited {
      get => _linkVisited;
      set { _linkVisited = value; Owner?.NotifyStateChanged(); }
    }

    internal override void BuildRenderTree(RenderTreeBuilder builder, ref int seq) {
      if (!Visible) return;

      builder.OpenElement(seq++, "li");
      builder.AddAttribute(seq++, "class", "swf-toolstrip-label");
      builder.AddAttribute(seq++, "style", "display:inline-flex;align-items:center;list-style:none;padding:2px 6px;");

      if (ToolTipText != null)
        builder.AddAttribute(seq++, "title", ToolTipText);

      if (_isLink) {
        var linkColor = _linkVisited ? _visitedLinkColor : _linkColor;
        builder.OpenElement(seq++, "a");
        builder.AddAttribute(seq++, "href", "#");
        builder.AddAttribute(seq++, "style", $"color:{linkColor.ToCss()};text-decoration:underline;cursor:pointer;");
        builder.AddAttribute(seq++, "onclick",
          EventCallback.Factory.Create<BlazorMouseEventArgs>(
            GetBlazorReceiver(),
            (BlazorMouseEventArgs _) => PerformClick()));
        builder.AddContent(seq++, Text ?? string.Empty);
        builder.CloseElement();
      } else {
        builder.OpenElement(seq++, "span");
        builder.AddContent(seq++, Text ?? string.Empty);
        builder.CloseElement();
      }

      builder.CloseElement(); // li
    }
  }

  // ---------------------------------------------------------------------------
  // ToolStripTextBox
  // ---------------------------------------------------------------------------
  public class ToolStripTextBox : ToolStripItem {

    private int _maxLength;
    private bool _readOnly;
    private CharacterCasing _characterCasing = CharacterCasing.Normal;
    private BorderStyle _borderStyle = BorderStyle.Fixed3D;
    private bool _acceptsReturn;
    private bool _acceptsTab;
    private bool _shortcutsEnabled = true;

    public ToolStripTextBox() { }
    public ToolStripTextBox(string name) { Name = name; }

    public TextBox TextBox { get; } = new TextBox();

    public int MaxLength {
      get => _maxLength;
      set => _maxLength = value;
    }

    public bool ReadOnly {
      get => _readOnly;
      set => _readOnly = value;
    }

    public CharacterCasing CharacterCasing {
      get => _characterCasing;
      set => _characterCasing = value;
    }

    public new BorderStyle BorderStyle {
      get => _borderStyle;
      set => _borderStyle = value;
    }

    public bool AcceptsReturn {
      get => _acceptsReturn;
      set => _acceptsReturn = value;
    }

    public bool AcceptsTab {
      get => _acceptsTab;
      set => _acceptsTab = value;
    }

    public bool ShortcutsEnabled {
      get => _shortcutsEnabled;
      set => _shortcutsEnabled = value;
    }

    public new event EventHandler TextChanged;
    public event KeyEventHandler KeyDown;
    public event KeyEventHandler KeyUp;
    public event KeyPressEventHandler KeyPress;

    internal override void BuildRenderTree(RenderTreeBuilder builder, ref int seq) {
      if (!Visible) return;

      builder.OpenElement(seq++, "li");
      builder.AddAttribute(seq++, "class", "swf-toolstrip-textbox");
      builder.AddAttribute(seq++, "style", "display:inline-flex;align-items:center;list-style:none;padding:1px 4px;");

      builder.OpenElement(seq++, "input");
      builder.AddAttribute(seq++, "type", "text");
      builder.AddAttribute(seq++, "value", Text ?? string.Empty);
      builder.AddAttribute(seq++, "style", "padding:1px 4px;border:1px solid #999;");

      if (_maxLength > 0)
        builder.AddAttribute(seq++, "maxlength", _maxLength);
      if (_readOnly)
        builder.AddAttribute(seq++, "readonly", true);
      if (!Enabled)
        builder.AddAttribute(seq++, "disabled", true);

      builder.CloseElement(); // input
      builder.CloseElement(); // li
    }
  }

  // ---------------------------------------------------------------------------
  // ToolStripComboBox
  // ---------------------------------------------------------------------------
  public class ToolStripComboBox : ToolStripItem {

    private ComboBox _comboBox;
    private int _selectedIndex = -1;
    private object _selectedItem;
    private ComboBoxStyle _dropDownStyle = ComboBoxStyle.DropDown;
    private bool _sorted;
    private FlatStyle _flatStyle = FlatStyle.Standard;
    private int _maxDropDownItems = 8;
    private int _dropDownWidth;
    private int _dropDownHeight = 106;
    private int _maxLength;
    private bool _droppedDown;
    private bool _integralHeight = true;
    private readonly List<object> _items = new List<object>();

    public ToolStripComboBox() { _comboBox = new ComboBox(); }
    public ToolStripComboBox(string name) : this() { Name = name; }

    public ComboBox ComboBox => _comboBox;
    public List<object> Items => _items;

    public int SelectedIndex {
      get => _selectedIndex;
      set {
        if (_selectedIndex != value) {
          _selectedIndex = value;
          _selectedItem = value >= 0 && value < _items.Count ? _items[value] : null;
          SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
          Owner?.NotifyStateChanged();
        }
      }
    }

    public object SelectedItem {
      get => _selectedItem;
      set {
        _selectedItem = value;
        _selectedIndex = _items.IndexOf(value);
      }
    }

    public ComboBoxStyle DropDownStyle {
      get => _dropDownStyle;
      set => _dropDownStyle = value;
    }

    public bool Sorted {
      get => _sorted;
      set => _sorted = value;
    }

    public FlatStyle FlatStyle {
      get => _flatStyle;
      set => _flatStyle = value;
    }

    public int MaxDropDownItems {
      get => _maxDropDownItems;
      set => _maxDropDownItems = value;
    }

    public int DropDownWidth {
      get => _dropDownWidth;
      set => _dropDownWidth = value;
    }

    public int DropDownHeight {
      get => _dropDownHeight;
      set => _dropDownHeight = value;
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

    public event EventHandler SelectedIndexChanged;
    public event EventHandler DropDown;
    public event EventHandler DropDownClosed;

    internal override void BuildRenderTree(RenderTreeBuilder builder, ref int seq) {
      if (!Visible) return;

      builder.OpenElement(seq++, "li");
      builder.AddAttribute(seq++, "class", "swf-toolstrip-combobox");
      builder.AddAttribute(seq++, "style", "display:inline-flex;align-items:center;list-style:none;padding:1px 4px;");

      builder.OpenElement(seq++, "select");
      builder.AddAttribute(seq++, "style", "padding:1px 4px;");
      if (!Enabled)
        builder.AddAttribute(seq++, "disabled", true);

      for (int i = 0; i < _items.Count; i++) {
        builder.OpenElement(seq++, "option");
        if (i == _selectedIndex)
          builder.AddAttribute(seq++, "selected", true);
        builder.AddContent(seq++, _items[i]?.ToString() ?? string.Empty);
        builder.CloseElement();
      }

      builder.CloseElement(); // select
      builder.CloseElement(); // li
    }
  }

  // ---------------------------------------------------------------------------
  // ToolStripDropDown
  // ---------------------------------------------------------------------------
  public class ToolStripDropDown : ToolStrip {

    private ToolStripItem _ownerItem;
    private bool _autoClose = true;
    private double _opacity = 1.0;

    public ToolStripDropDown() { }

    public ToolStripItem OwnerItem {
      get => _ownerItem;
      set => _ownerItem = value;
    }

    public bool AutoClose {
      get => _autoClose;
      set => _autoClose = value;
    }

    public double Opacity {
      get => _opacity;
      set => _opacity = value;
    }

    public event System.ComponentModel.CancelEventHandler Opening;
    public event ToolStripDropDownClosingEventHandler Closing;
    public event EventHandler Opened;
    public event ToolStripDropDownClosedEventHandler Closed;

    public void Show(Point location) { Show(location.X, location.Y); }
    public void Show(int x, int y) {
      Visible = true;
      Location = new Point(x, y);
      Opening?.Invoke(this, new System.ComponentModel.CancelEventArgs());
      Opened?.Invoke(this, EventArgs.Empty);
    }
    public void Show(Control control, Point offset) {
      Show(control.PointToScreen(offset));
    }

    public void Close() {
      Visible = false;
      Closing?.Invoke(this, new ToolStripDropDownClosingEventArgs(ToolStripDropDownCloseReason.CloseCalled));
      Closed?.Invoke(this, new ToolStripDropDownClosedEventArgs(ToolStripDropDownCloseReason.CloseCalled));
    }

    protected override string GetCssClasses() {
      return "swf-control swf-toolstrip-dropdown";
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "position:absolute;z-index:1000;background:#fff;border:1px solid #ccc;box-shadow:2px 2px 4px rgba(0,0,0,0.2);";
      if (_opacity < 1.0)
        style += $"opacity:{_opacity.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)};";
      return style;
    }
  }

  // ---------------------------------------------------------------------------
  // ToolStripDropDownMenu
  // ---------------------------------------------------------------------------
  public class ToolStripDropDownMenu : ToolStripDropDown {

    private bool _showImageMargin = true;
    private bool _showCheckMargin;

    public bool ShowImageMargin {
      get => _showImageMargin;
      set => _showImageMargin = value;
    }

    public bool ShowCheckMargin {
      get => _showCheckMargin;
      set => _showCheckMargin = value;
    }
  }

  // ---------------------------------------------------------------------------
  // ToolStripOverflowButton
  // ---------------------------------------------------------------------------
  public class ToolStripOverflowButton : ToolStripButton {

    public ToolStripOverflowButton() {
      Text = "\u25BC";
    }

    public bool HasDropDownItems => false;
  }
}
