// =============================================================================
// Synthiam.Web.Forms - WinForms Event Arguments and Delegates for Blazor
// =============================================================================

using System.Drawing;

namespace System.Windows.Forms {

  // =========================================================================
  // Supporting enums used by EventArgs classes
  // =========================================================================

  [Flags]
  public enum DragDropEffects {
    None = 0,
    Copy = 1,
    Move = 2,
    Link = 4,
    Scroll = -2147483648,
    All = -2147483645
  }

  [Flags]
  public enum DrawItemState {
    None = 0,
    Selected = 1,
    Grayed = 2,
    Disabled = 4,
    Checked = 8,
    Focus = 16,
    Default = 32,
    HotLight = 64,
    Inactive = 128,
    NoAccelerator = 256,
    NoFocusRect = 512,
    ComboBoxEdit = 4096
  }

  public enum ScrollEventType {
    SmallDecrement = 0,
    SmallIncrement = 1,
    LargeDecrement = 2,
    LargeIncrement = 3,
    ThumbPosition = 4,
    ThumbTrack = 5,
    First = 6,
    Last = 7,
    EndScroll = 8
  }

  public enum ScrollOrientation {
    HorizontalScroll = 0,
    VerticalScroll = 1
  }

  public enum TreeViewAction {
    Unknown = 0,
    ByKeyboard = 1,
    ByMouse = 2,
    Collapse = 3,
    Expand = 4
  }

  // =========================================================================
  // Delegate types
  // =========================================================================

  public delegate void KeyEventHandler(object sender, KeyEventArgs e);
  public delegate void KeyPressEventHandler(object sender, KeyPressEventArgs e);
  public delegate void MouseEventHandler(object sender, MouseEventArgs e);
  public delegate void PaintEventHandler(object sender, PaintEventArgs e);
  public delegate void FormClosingEventHandler(object sender, FormClosingEventArgs e);
  public delegate void FormClosedEventHandler(object sender, FormClosedEventArgs e);
  public delegate void DragEventHandler(object sender, DragEventArgs e);
  public delegate void DrawItemEventHandler(object sender, DrawItemEventArgs e);
  public delegate void MeasureItemEventHandler(object sender, MeasureItemEventArgs e);
  public delegate void ItemDragEventHandler(object sender, ItemDragEventArgs e);
  public delegate void TreeViewEventHandler(object sender, TreeViewEventArgs e);
  public delegate void TreeViewCancelEventHandler(object sender, TreeViewCancelEventArgs e);
  public delegate void TreeNodeMouseClickEventHandler(object sender, TreeNodeMouseClickEventArgs e);
  public delegate void DataGridViewCellEventHandler(object sender, DataGridViewCellEventArgs e);
  public delegate void DataGridViewCellFormattingEventHandler(object sender, DataGridViewCellFormattingEventArgs e);
  public delegate void LinkLabelLinkClickedEventHandler(object sender, LinkLabelLinkClickedEventArgs e);
  public delegate void ScrollEventHandler(object sender, ScrollEventArgs e);
  public delegate void SplitterEventHandler(object sender, SplitterEventArgs e);
  public delegate void ColumnClickEventHandler(object sender, ColumnClickEventArgs e);
  public delegate void ToolStripItemClickedEventHandler(object sender, ToolStripItemClickedEventArgs e);
  public delegate void TabControlCancelEventHandler(object sender, TabControlCancelEventArgs e);
  public delegate void WebBrowserNavigatingEventHandler(object sender, WebBrowserNavigatingEventArgs e);
  public delegate void WebBrowserDocumentCompletedEventHandler(object sender, WebBrowserDocumentCompletedEventArgs e);
  public delegate void PreviewKeyDownEventHandler(object sender, PreviewKeyDownEventArgs e);
  public delegate void ItemCheckedEventHandler(object sender, ItemCheckedEventArgs e);
  public delegate void ItemCheckEventHandler(object sender, ItemCheckEventArgs e);
  public delegate void ListViewItemSelectionChangedEventHandler(object sender, ListViewItemSelectionChangedEventArgs e);

  // =========================================================================
  // EventArgs classes
  // =========================================================================

  public class MouseEventArgs : EventArgs {

    public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta) {
      Button = button;
      Clicks = clicks;
      X = x;
      Y = y;
      Delta = delta;
    }

    public MouseButtons Button { get; }
    public int Clicks { get; }
    public int X { get; }
    public int Y { get; }
    public int Delta { get; }
    public Point Location => new Point(X, Y);
  }

  public class KeyEventArgs : EventArgs {

    public KeyEventArgs(Keys keyData) {
      KeyData = keyData;
    }

    public Keys KeyCode => KeyData & Keys.KeyCode;
    public Keys KeyData { get; }
    public Keys Modifiers => KeyData & Keys.Modifiers;
    public bool Alt => (KeyData & Keys.Alt) == Keys.Alt;
    public bool Control => (KeyData & Keys.Control) == Keys.Control;
    public bool Shift => (KeyData & Keys.Shift) == Keys.Shift;
    public bool Handled { get; set; }
    public bool SuppressKeyPress { get; set; }
    public int KeyValue => (int)(KeyData & Keys.KeyCode);
  }

  public class KeyPressEventArgs : EventArgs {

    public KeyPressEventArgs(char keyChar) {
      KeyChar = keyChar;
    }

    public char KeyChar { get; set; }
    public bool Handled { get; set; }
  }

  public class PaintEventArgs : EventArgs, IDisposable {

    public PaintEventArgs(Graphics graphics, Rectangle clipRectangle) {
      Graphics = graphics;
      ClipRectangle = clipRectangle;
    }

    public Graphics Graphics { get; }
    public Rectangle ClipRectangle { get; }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
    }
  }

  public class FormClosingEventArgs : System.ComponentModel.CancelEventArgs {

    public FormClosingEventArgs(CloseReason closeReason, bool cancel)
      : base(cancel) {
      CloseReason = closeReason;
    }

    public CloseReason CloseReason { get; }
  }

  public class FormClosedEventArgs : EventArgs {

    public FormClosedEventArgs(CloseReason closeReason) {
      CloseReason = closeReason;
    }

    public CloseReason CloseReason { get; }
  }

  public class DragEventArgs : EventArgs {

    public DragEventArgs(IDataObject data, int keyState, int x, int y, DragDropEffects allowedEffect, DragDropEffects effect) {
      Data = data;
      KeyState = keyState;
      X = x;
      Y = y;
      AllowedEffect = allowedEffect;
      Effect = effect;
    }

    public IDataObject Data { get; }
    public int KeyState { get; }
    public int X { get; }
    public int Y { get; }
    public DragDropEffects AllowedEffect { get; }
    public DragDropEffects Effect { get; set; }
  }

  public class GiveFeedbackEventArgs : EventArgs {

    public GiveFeedbackEventArgs(DragDropEffects effect, bool useDefaultCursors) {
      Effect = effect;
      UseDefaultCursors = useDefaultCursors;
    }

    public DragDropEffects Effect { get; }
    public bool UseDefaultCursors { get; set; }
  }

  public class QueryContinueDragEventArgs : EventArgs {

    public QueryContinueDragEventArgs(int keyState, bool escapePressed, DragDropEffects action) {
      KeyState = keyState;
      EscapePressed = escapePressed;
      Action = action;
    }

    public int KeyState { get; }
    public bool EscapePressed { get; }
    public DragDropEffects Action { get; set; }
  }

  public class DrawItemEventArgs : EventArgs {

    public DrawItemEventArgs(Graphics graphics, Font font, Rectangle bounds, int index, DrawItemState state) {
      Graphics = graphics;
      Font = font;
      Bounds = bounds;
      Index = index;
      State = state;
      ForeColor = Color.Black;
      BackColor = Color.White;
    }

    public DrawItemEventArgs(Graphics graphics, Font font, Rectangle bounds, int index, DrawItemState state, Color foreColor, Color backColor) {
      Graphics = graphics;
      Font = font;
      Bounds = bounds;
      Index = index;
      State = state;
      ForeColor = foreColor;
      BackColor = backColor;
    }

    public Graphics Graphics { get; }
    public Font Font { get; }
    public Rectangle Bounds { get; }
    public int Index { get; }
    public DrawItemState State { get; }
    public Color ForeColor { get; }
    public Color BackColor { get; }

    public virtual void DrawBackground() {
    }

    public virtual void DrawFocusRectangle() {
    }
  }

  public class MeasureItemEventArgs : EventArgs {

    public MeasureItemEventArgs(Graphics graphics, int index) {
      Graphics = graphics;
      Index = index;
    }

    public MeasureItemEventArgs(Graphics graphics, int index, int itemHeight) {
      Graphics = graphics;
      Index = index;
      ItemHeight = itemHeight;
    }

    public Graphics Graphics { get; }
    public int Index { get; }
    public int ItemHeight { get; set; }
    public int ItemWidth { get; set; }
  }

  public class ItemDragEventArgs : EventArgs {

    public ItemDragEventArgs(MouseButtons button) {
      Button = button;
    }

    public ItemDragEventArgs(MouseButtons button, object item) {
      Button = button;
      Item = item;
    }

    public MouseButtons Button { get; }
    public object Item { get; }
  }

  public class TreeViewEventArgs : EventArgs {

    public TreeViewEventArgs(TreeNode node) {
      Node = node;
      Action = TreeViewAction.Unknown;
    }

    public TreeViewEventArgs(TreeNode node, TreeViewAction action) {
      Node = node;
      Action = action;
    }

    public TreeNode Node { get; }
    public TreeViewAction Action { get; }
  }

  public class TreeViewCancelEventArgs : System.ComponentModel.CancelEventArgs {

    public TreeViewCancelEventArgs(TreeNode node, bool cancel, TreeViewAction action)
      : base(cancel) {
      Node = node;
      Action = action;
    }

    public TreeNode Node { get; }
    public TreeViewAction Action { get; }
  }

  public class TreeNodeMouseClickEventArgs : MouseEventArgs {

    public TreeNodeMouseClickEventArgs(TreeNode node, MouseButtons button, int clicks, int x, int y)
      : base(button, clicks, x, y, 0) {
      Node = node;
    }

    public TreeNode Node { get; }
  }

  public class NodeLabelEditEventArgs : EventArgs {

    public NodeLabelEditEventArgs(TreeNode node) {
      Node = node;
    }

    public NodeLabelEditEventArgs(TreeNode node, string label) {
      Node = node;
      Label = label;
    }

    public TreeNode Node { get; }
    public string Label { get; }
    public bool CancelEdit { get; set; }
  }

  public class DataGridViewCellEventArgs : EventArgs {

    public DataGridViewCellEventArgs(int columnIndex, int rowIndex) {
      ColumnIndex = columnIndex;
      RowIndex = rowIndex;
    }

    public int ColumnIndex { get; }
    public int RowIndex { get; }
  }

  public class ConvertEventArgs : EventArgs {

    public ConvertEventArgs(object value, Type desiredType) {
      Value = value;
      DesiredType = desiredType;
    }

    public object Value { get; set; }
    public Type DesiredType { get; }
  }

  public class DataGridViewCellFormattingEventArgs : ConvertEventArgs {

    public DataGridViewCellFormattingEventArgs(int columnIndex, int rowIndex, object value, Type desiredType, DataGridViewCellStyle cellStyle)
      : base(value, desiredType) {
      ColumnIndex = columnIndex;
      RowIndex = rowIndex;
      CellStyle = cellStyle;
    }

    public int ColumnIndex { get; }
    public int RowIndex { get; }
    public DataGridViewCellStyle CellStyle { get; set; }
    public bool FormattingApplied { get; set; }
  }

  public class LinkLabelLinkClickedEventArgs : EventArgs {

    public LinkLabelLinkClickedEventArgs(LinkLabel.Link link) {
      Link = link;
      Button = MouseButtons.Left;
    }

    public LinkLabelLinkClickedEventArgs(LinkLabel.Link link, MouseButtons button) {
      Link = link;
      Button = button;
    }

    public LinkLabel.Link Link { get; }
    public MouseButtons Button { get; }
  }

  public class ScrollEventArgs : EventArgs {

    public ScrollEventArgs(ScrollEventType type, int newValue) {
      Type = type;
      NewValue = newValue;
      OldValue = -1;
    }

    public ScrollEventArgs(ScrollEventType type, int oldValue, int newValue) {
      Type = type;
      OldValue = oldValue;
      NewValue = newValue;
    }

    public ScrollEventArgs(ScrollEventType type, int oldValue, int newValue, ScrollOrientation scrollOrientation) {
      Type = type;
      OldValue = oldValue;
      NewValue = newValue;
      ScrollOrientation = scrollOrientation;
    }

    public ScrollEventType Type { get; }
    public int NewValue { get; set; }
    public int OldValue { get; }
    public ScrollOrientation ScrollOrientation { get; }
  }

  public class SplitterEventArgs : EventArgs {

    public SplitterEventArgs(int x, int y, int splitX, int splitY) {
      X = x;
      Y = y;
      SplitX = splitX;
      SplitY = splitY;
    }

    public int X { get; }
    public int Y { get; }
    public int SplitX { get; set; }
    public int SplitY { get; set; }
  }

  public class ColumnClickEventArgs : EventArgs {

    public ColumnClickEventArgs(int column) {
      Column = column;
    }

    public int Column { get; }
  }

  public class ToolStripItemClickedEventArgs : EventArgs {

    public ToolStripItemClickedEventArgs(ToolStripItem clickedItem) {
      ClickedItem = clickedItem;
    }

    public ToolStripItem ClickedItem { get; }
  }

  public class TabControlCancelEventArgs : System.ComponentModel.CancelEventArgs {

    public TabControlCancelEventArgs(TabPage tabPage, int tabPageIndex, bool cancel, TabControlAction action)
      : base(cancel) {
      TabPage = tabPage;
      TabPageIndex = tabPageIndex;
      Action = action;
    }

    public TabPage TabPage { get; }
    public int TabPageIndex { get; }
    public TabControlAction Action { get; }
  }

  public enum TabControlAction {
    Selecting = 0,
    Selected = 1,
    Deselecting = 2,
    Deselected = 3
  }

  public class WebBrowserNavigatingEventArgs : System.ComponentModel.CancelEventArgs {

    public WebBrowserNavigatingEventArgs(Uri url, string targetFrameName) {
      Url = url;
      TargetFrameName = targetFrameName;
    }

    public Uri Url { get; }
    public string TargetFrameName { get; }
  }

  public class WebBrowserDocumentCompletedEventArgs : EventArgs {

    public WebBrowserDocumentCompletedEventArgs(Uri url) {
      Url = url;
    }

    public Uri Url { get; }
  }

  public class PreviewKeyDownEventArgs : EventArgs {

    public PreviewKeyDownEventArgs(Keys keyData) {
      KeyData = keyData;
    }

    public Keys KeyCode => KeyData & Keys.KeyCode;
    public Keys KeyData { get; }
    public Keys Modifiers => KeyData & Keys.Modifiers;
    public bool Alt => (KeyData & Keys.Alt) == Keys.Alt;
    public bool Control => (KeyData & Keys.Control) == Keys.Control;
    public bool Shift => (KeyData & Keys.Shift) == Keys.Shift;
    public bool IsInputKey { get; set; }
  }

  public class ItemCheckedEventArgs : EventArgs {

    public ItemCheckedEventArgs(ListViewItem item) {
      Item = item;
    }

    public ListViewItem Item { get; }
  }

  public class ItemCheckEventArgs : EventArgs {

    public ItemCheckEventArgs(int index, CheckState currentValue, CheckState newValue) {
      Index = index;
      CurrentValue = currentValue;
      NewValue = newValue;
    }

    public int Index { get; }
    public CheckState CurrentValue { get; }
    public CheckState NewValue { get; set; }
  }

  public class ListViewItemSelectionChangedEventArgs : EventArgs {

    public ListViewItemSelectionChangedEventArgs(ListViewItem item, int itemIndex, bool isSelected) {
      Item = item;
      ItemIndex = itemIndex;
      IsSelected = isSelected;
    }

    public ListViewItem Item { get; }
    public int ItemIndex { get; }
    public bool IsSelected { get; }
  }

  public class LayoutEventArgs : EventArgs {

    public LayoutEventArgs(object affectedComponent, string affectedProperty) {
      AffectedComponent = affectedComponent;
      AffectedProperty = affectedProperty;
    }

    public object AffectedComponent { get; }
    public string AffectedProperty { get; }
  }

  public class InvalidateEventArgs : EventArgs {

    public InvalidateEventArgs(Rectangle invalidRect) {
      InvalidRect = invalidRect;
    }

    public Rectangle InvalidRect { get; }
  }

  public class HelpEventArgs : EventArgs {

    public HelpEventArgs(Point mousePos) {
      MousePos = mousePos;
    }

    public Point MousePos { get; }
    public bool Handled { get; set; }
  }

  // =========================================================================
  // TabControlEventArgs
  // =========================================================================

  public class TabControlEventArgs : EventArgs {

    public TabControlEventArgs(TabPage tabPage, int tabPageIndex, TabControlAction action) {
      TabPage = tabPage;
      TabPageIndex = tabPageIndex;
      Action = action;
    }

    public TabPage TabPage { get; }
    public int TabPageIndex { get; }
    public TabControlAction Action { get; }
  }

  // =========================================================================
  // LinkClickedEventArgs (for RichTextBox link clicks)
  // =========================================================================

  public class LinkClickedEventArgs : EventArgs {

    public LinkClickedEventArgs(string linkText) {
      LinkText = linkText;
    }

    public string LinkText { get; }
  }

  public delegate void LinkClickedEventHandler(object sender, LinkClickedEventArgs e);

  // =========================================================================
  // DataGridViewRowsAddedEventArgs
  // =========================================================================

  public class DataGridViewRowsAddedEventArgs : EventArgs {

    public DataGridViewRowsAddedEventArgs(int rowIndex, int rowCount) {
      RowIndex = rowIndex;
      RowCount = rowCount;
    }

    public int RowIndex { get; }
    public int RowCount { get; }
  }

  public delegate void DataGridViewRowsAddedEventHandler(object sender, DataGridViewRowsAddedEventArgs e);

  // =========================================================================
  // DataGridViewDataErrorEventArgs
  // =========================================================================

  public class DataGridViewDataErrorEventArgs : DataGridViewCellEventArgs {

    public DataGridViewDataErrorEventArgs(Exception exception, int columnIndex, int rowIndex, DataGridViewDataErrorContexts context)
      : base(columnIndex, rowIndex) {
      Exception = exception;
      Context = context;
    }

    public Exception Exception { get; }
    public DataGridViewDataErrorContexts Context { get; }
    public bool ThrowException { get; set; }
    public bool Cancel { get; set; }
  }

  public delegate void DataGridViewDataErrorEventHandler(object sender, DataGridViewDataErrorEventArgs e);

  // =========================================================================
  // DataGridViewRowsRemovedEventArgs
  // =========================================================================

  public class DataGridViewRowsRemovedEventArgs : EventArgs {

    public DataGridViewRowsRemovedEventArgs(int rowIndex, int rowCount) {
      RowIndex = rowIndex;
      RowCount = rowCount;
    }

    public int RowIndex { get; }
    public int RowCount { get; }
  }

  public delegate void DataGridViewRowsRemovedEventHandler(object sender, DataGridViewRowsRemovedEventArgs e);
}
