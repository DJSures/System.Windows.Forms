// =============================================================================
// Synthiam.Web.Forms - WinForms Enums for Blazor
// =============================================================================

namespace System.Windows.Forms {

  public enum DockStyle {
    None = 0,
    Top = 1,
    Bottom = 2,
    Left = 3,
    Right = 4,
    Fill = 5
  }

  [Flags]
  public enum AnchorStyles {
    None = 0,
    Top = 1,
    Bottom = 2,
    Left = 4,
    Right = 8
  }

  public enum AutoScaleMode {
    None = 0,
    Font = 1,
    Dpi = 2,
    Inherit = 3
  }

  public enum BorderStyle {
    None = 0,
    FixedSingle = 1,
    Fixed3D = 2
  }

  public enum FlatStyle {
    Flat = 0,
    Popup = 1,
    Standard = 2,
    System = 3
  }

  public enum FormBorderStyle {
    None = 0,
    FixedSingle = 1,
    Fixed3D = 2,
    FixedDialog = 3,
    Sizable = 4,
    FixedToolWindow = 5,
    SizableToolWindow = 6
  }

  public enum FormStartPosition {
    Manual = 0,
    CenterScreen = 1,
    WindowsDefaultLocation = 2,
    WindowsDefaultBounds = 3,
    CenterParent = 4
  }

  public enum FormWindowState {
    Normal = 0,
    Minimized = 1,
    Maximized = 2
  }

  public enum DialogResult {
    None = 0,
    OK = 1,
    Cancel = 2,
    Abort = 3,
    Retry = 4,
    Ignore = 5,
    Yes = 6,
    No = 7,
    Continue = 8
  }

  public enum MessageBoxButtons {
    OK = 0,
    OKCancel = 1,
    AbortRetryIgnore = 2,
    YesNoCancel = 3,
    YesNo = 4,
    RetryCancel = 5
  }

  public enum MessageBoxIcon {
    None = 0,
    Error = 16,
    Hand = 16,
    Stop = 16,
    Question = 32,
    Exclamation = 48,
    Warning = 48,
    Asterisk = 64,
    Information = 64
  }

  public enum MessageBoxDefaultButton {
    Button1 = 0,
    Button2 = 256,
    Button3 = 512
  }

  [Flags]
  public enum Keys {
    None = 0,
    LButton = 1,
    RButton = 2,
    Cancel = 3,
    MButton = 4,
    Back = 8,
    Tab = 9,
    Clear = 12,
    Return = 13,
    Enter = 13,
    ShiftKey = 16,
    ControlKey = 17,
    Menu = 18,
    Pause = 19,
    Capital = 20,
    CapsLock = 20,
    Escape = 27,
    Space = 32,
    PageUp = 33,
    PageDown = 34,
    End = 35,
    Home = 36,
    Left = 37,
    Up = 38,
    Right = 39,
    Down = 40,
    Select = 41,
    Print = 42,
    Execute = 43,
    PrintScreen = 44,
    Insert = 45,
    Delete = 46,
    Help = 47,
    D0 = 48,
    D1 = 49,
    D2 = 50,
    D3 = 51,
    D4 = 52,
    D5 = 53,
    D6 = 54,
    D7 = 55,
    D8 = 56,
    D9 = 57,
    A = 65,
    B = 66,
    C = 67,
    D = 68,
    E = 69,
    F = 70,
    G = 71,
    H = 72,
    I = 73,
    J = 74,
    K = 75,
    L = 76,
    M = 77,
    N = 78,
    O = 79,
    P = 80,
    Q = 81,
    R = 82,
    S = 83,
    T = 84,
    U = 85,
    V = 86,
    W = 87,
    X = 88,
    Y = 89,
    Z = 90,
    LWin = 91,
    RWin = 92,
    Apps = 93,
    Sleep = 95,
    NumPad0 = 96,
    NumPad1 = 97,
    NumPad2 = 98,
    NumPad3 = 99,
    NumPad4 = 100,
    NumPad5 = 101,
    NumPad6 = 102,
    NumPad7 = 103,
    NumPad8 = 104,
    NumPad9 = 105,
    Multiply = 106,
    Add = 107,
    Separator = 108,
    Subtract = 109,
    Decimal = 110,
    Divide = 111,
    F1 = 112,
    F2 = 113,
    F3 = 114,
    F4 = 115,
    F5 = 116,
    F6 = 117,
    F7 = 118,
    F8 = 119,
    F9 = 120,
    F10 = 121,
    F11 = 122,
    F12 = 123,
    NumLock = 144,
    Scroll = 145,
    LShiftKey = 160,
    RShiftKey = 161,
    LControlKey = 162,
    RControlKey = 163,
    LMenu = 164,
    RMenu = 165,
    OemSemicolon = 186,
    Oemplus = 187,
    Oemcomma = 188,
    OemMinus = 189,
    OemPeriod = 190,
    OemQuestion = 191,
    Oemtilde = 192,
    OemOpenBrackets = 219,
    OemPipe = 220,
    OemCloseBrackets = 221,
    OemQuotes = 222,
    Oem8 = 223,
    OemBackslash = 226,
    Shift = 65536,
    Control = 131072,
    Alt = 262144,
    Modifiers = -65536,
    KeyCode = 65535
  }

  [Flags]
  public enum MouseButtons {
    None = 0,
    Left = 1048576,
    Right = 2097152,
    Middle = 4194304,
    XButton1 = 8388608,
    XButton2 = 16777216
  }

  [Flags]
  public enum ControlStyles {
    ContainerControl = 1,
    UserPaint = 2,
    Opaque = 4,
    ResizeRedraw = 16,
    FixedWidth = 32,
    FixedHeight = 64,
    StandardClick = 256,
    Selectable = 512,
    UserMouse = 1024,
    SupportsTransparentBackColor = 2048,
    StandardDoubleClick = 4096,
    AllPaintingInWmPaint = 8192,
    CacheText = 16384,
    EnableNotifyMessage = 32768,
    DoubleBuffer = 65536,
    OptimizedDoubleBuffer = 131072,
    UseTextForAccessibility = 262144
  }

  public enum Orientation {
    Horizontal = 0,
    Vertical = 1
  }

  public enum TickStyle {
    None = 0,
    TopLeft = 1,
    BottomRight = 2,
    Both = 3
  }

  public enum ComboBoxStyle {
    Simple = 0,
    DropDown = 1,
    DropDownList = 2
  }

  public enum SelectionMode {
    None = 0,
    One = 1,
    MultiSimple = 2,
    MultiExtended = 3
  }

  public enum SortOrder {
    None = 0,
    Ascending = 1,
    Descending = 2
  }

  public enum View {
    LargeIcon = 0,
    Details = 1,
    SmallIcon = 2,
    List = 3,
    Tile = 4
  }

  public enum ScrollBars {
    None = 0,
    Horizontal = 1,
    Vertical = 2,
    Both = 3
  }

  public enum RichTextBoxScrollBars {
    None = 0,
    Horizontal = 1,
    Vertical = 2,
    Both = 3,
    ForcedHorizontal = 17,
    ForcedVertical = 18,
    ForcedBoth = 19
  }

  public enum PictureBoxSizeMode {
    Normal = 0,
    StretchImage = 1,
    AutoSize = 2,
    CenterImage = 3,
    Zoom = 4
  }

  public enum HorizontalAlignment {
    Left = 0,
    Right = 1,
    Center = 2
  }

  public enum ColumnHeaderStyle {
    None = 0,
    Nonclickable = 1,
    Clickable = 2
  }

  public enum ListViewAlignment {
    Default = 0,
    Left = 1,
    Top = 2,
    SnapToGrid = 5
  }

  public enum ItemActivation {
    Standard = 0,
    OneClick = 1,
    TwoClick = 2
  }

  public enum Appearance {
    Normal = 0,
    Button = 1
  }

  public enum CheckState {
    Unchecked = 0,
    Checked = 1,
    Indeterminate = 2
  }

  public enum CharacterCasing {
    Normal = 0,
    Upper = 1,
    Lower = 2
  }

  public enum AutoCompleteMode {
    None = 0,
    Suggest = 1,
    Append = 2,
    SuggestAppend = 3
  }

  public enum AutoCompleteSource {
    None = 0,
    FileSystem = 1,
    HistoryList = 2,
    RecentlyUsedList = 3,
    AllUrl = 6,
    AllSystemSources = 7,
    FileSystemDirectories = 32,
    CustomSource = 64,
    ListItems = 256
  }

  public enum ToolStripRenderMode {
    Custom = 0,
    System = 1,
    Professional = 2,
    ManagerRenderMode = 3
  }

  public enum ToolStripLayoutStyle {
    StackWithOverflow = 0,
    HorizontalStackWithOverflow = 1,
    VerticalStackWithOverflow = 2,
    Flow = 3,
    Table = 4
  }

  public enum ToolStripItemDisplayStyle {
    None = 0,
    Text = 1,
    Image = 2,
    ImageAndText = 3
  }

  public enum ToolStripItemAlignment {
    Left = 0,
    Right = 1
  }

  [Flags]
  public enum ToolStripStatusLabelBorderSides {
    None = 0,
    Left = 1,
    Top = 2,
    Right = 4,
    Bottom = 8,
    All = 15
  }

  public enum DataGridViewContentAlignment {
    NotSet = 0,
    TopLeft = 1,
    TopCenter = 2,
    TopRight = 4,
    MiddleLeft = 16,
    MiddleCenter = 32,
    MiddleRight = 64,
    BottomLeft = 256,
    BottomCenter = 512,
    BottomRight = 1024
  }

  public enum DataGridViewAutoSizeColumnMode {
    NotSet = 0,
    None = 1,
    ColumnHeader = 2,
    AllCellsExceptHeader = 4,
    AllCells = 6,
    DisplayedCellsExceptHeader = 8,
    DisplayedCells = 10,
    Fill = 16
  }

  public enum DataGridViewAutoSizeColumnsMode {
    NotSet = 0,
    None = 1,
    ColumnHeader = 2,
    AllCellsExceptHeader = 4,
    AllCells = 6,
    DisplayedCellsExceptHeader = 8,
    DisplayedCells = 10,
    Fill = 16
  }

  public enum DataGridViewSelectionMode {
    CellSelect = 0,
    FullRowSelect = 1,
    FullColumnSelect = 2,
    RowHeaderSelect = 3,
    ColumnHeaderSelect = 4
  }

  public enum DataGridViewEditMode {
    EditOnEnter = 0,
    EditOnKeystroke = 1,
    EditOnKeystrokeOrF2 = 2,
    EditOnF2 = 3,
    EditProgrammatically = 4
  }

  public enum DataGridViewColumnSortMode {
    NotSortable = 0,
    Automatic = 1,
    Programmatic = 2
  }

  public enum CloseReason {
    None = 0,
    WindowsShutDown = 1,
    MdiFormClosing = 2,
    UserClosing = 3,
    TaskManagerClosing = 4,
    FormOwnerClosing = 5,
    ApplicationExitCall = 6
  }

  public enum RightToLeft {
    No = 0,
    Yes = 1,
    Inherit = 2
  }

  public enum ImageLayout {
    None = 0,
    Tile = 1,
    Center = 2,
    Stretch = 3,
    Zoom = 4
  }

  public enum DrawMode {
    Normal = 0,
    OwnerDrawFixed = 1,
    OwnerDrawVariable = 2
  }

  public enum TabAlignment {
    Top = 0,
    Bottom = 1,
    Left = 2,
    Right = 3
  }

  public enum TabSizeMode {
    Normal = 0,
    Fixed = 1,
    FillToRight = 2
  }

  public enum FixedPanel {
    None = 0,
    Panel1 = 1,
    Panel2 = 2
  }

  public enum FlowDirection {
    LeftToRight = 0,
    TopDown = 1,
    RightToLeft = 2,
    BottomUp = 3
  }

  public enum TableLayoutPanelCellBorderStyle {
    None = 0,
    Single = 1,
    Inset = 2,
    InsetDouble = 3,
    Outset = 4,
    OutsetDouble = 5,
    OutsetPartial = 6
  }

  public enum SizeType {
    Absolute = 0,
    Percent = 1,
    AutoSize = 2
  }

  public enum TreeViewDrawMode {
    Normal = 0,
    OwnerDrawText = 1,
    OwnerDrawAll = 2
  }

  public enum PowerState {
    Suspend = 1,
    Hibernate = 2
  }

  [Flags]
  public enum DataGridViewDataErrorContexts {
    Formatting = 1,
    Display = 2,
    PreferredSize = 4,
    RowDeletion = 8,
    Parsing = 256,
    Commit = 512,
    InitialValueRestoration = 1024,
    LeaveControl = 2048,
    CurrentCellChange = 4096,
    Scroll = 8192,
    ClipboardContent = 16384
  }

  public enum AutoValidate {
    Disable = 0,
    EnablePreventFocusChange = 1,
    EnableAllowFocusChange = 2,
    Inherit = -1
  }

  public enum TabAppearance {
    Normal = 0,
    Buttons = 1,
    FlatButtons = 2
  }

  // CopyPixelOperation is now provided by System.Drawing.Common

  /// <summary>
  /// Delegate used for invoking methods on the UI thread (replaces System.Windows.Forms.MethodInvoker).
  /// </summary>
  public delegate void MethodInvoker();
}
