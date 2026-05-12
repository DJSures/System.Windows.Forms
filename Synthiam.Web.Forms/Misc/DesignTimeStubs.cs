// =============================================================================
// Synthiam.Web.Forms - Design-time stubs for Blazor
// Provides stub types needed by dependency projects (Alsing.SyntaxBox,
// EZ-RibbonMenu, EZ-B, aForge) that don't need real implementations.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Drawing;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // CreateParams
  // ---------------------------------------------------------------------------
  public class CreateParams {
    public string ClassName { get; set; }
    public string Caption { get; set; }
    public int Style { get; set; }
    public int ExStyle { get; set; }
    public int ClassStyle { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public IntPtr Parent { get; set; }
    public object Param { get; set; }
  }

  // ---------------------------------------------------------------------------
  // NativeWindow
  // ---------------------------------------------------------------------------
  public class NativeWindow {
    public IntPtr Handle { get; private set; }

    public void AssignHandle(IntPtr handle) {
      Handle = handle;
    }

    public void ReleaseHandle() {
      Handle = IntPtr.Zero;
    }

    protected virtual void WndProc(ref Message m) { }
  }

  // ---------------------------------------------------------------------------
  // PropertyGrid
  // ---------------------------------------------------------------------------
  public class PropertyGrid : Control {
    public object SelectedObject { get; set; }
    public object[] SelectedObjects { get; set; }
    public bool CommandsVisibleIfAvailable { get; set; }
    public bool HelpVisible { get; set; } = true;
    public bool LargeButtons { get; set; }
    public System.Drawing.Color LineColor { get; set; }
    public bool ToolbarVisible { get; set; } = true;
    public System.Drawing.Color ViewBackColor { get; set; }
    public System.Drawing.Color ViewForeColor { get; set; }
    public event PropertyValueChangedEventHandler PropertyValueChanged;

    public new void Refresh() { }

    protected virtual void OnPropertyValueChanged(PropertyValueChangedEventArgs e) {
      PropertyValueChanged?.Invoke(this, e);
    }
  }

  // ---------------------------------------------------------------------------
  // VScrollBar
  // ---------------------------------------------------------------------------
  public class VScrollBar : Control {
    public int Value { get; set; }
    public int Minimum { get; set; }
    public int Maximum { get; set; } = 100;
    public int SmallChange { get; set; } = 1;
    public int LargeChange { get; set; } = 10;
    public event ScrollEventHandler Scroll;
    public event EventHandler ValueChanged;
  }

  // ---------------------------------------------------------------------------
  // HScrollBar
  // ---------------------------------------------------------------------------
  public class HScrollBar : Control {
    public int Value { get; set; }
    public int Minimum { get; set; }
    public int Maximum { get; set; } = 100;
    public int SmallChange { get; set; } = 1;
    public int LargeChange { get; set; } = 10;
    public event ScrollEventHandler Scroll;
    public event EventHandler ValueChanged;
  }

  // ---------------------------------------------------------------------------
  // MenuItem (legacy menu system)
  // ---------------------------------------------------------------------------
  public class MenuItem {
    public MenuItem() { }
    public MenuItem(string text) { Text = text; }

    public string Text { get; set; } = string.Empty;
    public bool Checked { get; set; }
    public bool Enabled { get; set; } = true;
    public bool Visible { get; set; } = true;
    public int Index { get; set; }
    public MenuItemCollection MenuItems { get; } = new MenuItemCollection();

    public event EventHandler Click;

    public class MenuItemCollection : List<MenuItem> { }
  }

  // ---------------------------------------------------------------------------
  // ToolStripDropDownClosingEventArgs / ToolStripDropDownClosedEventArgs
  // ---------------------------------------------------------------------------
  public class ToolStripDropDownClosingEventArgs : System.ComponentModel.CancelEventArgs {
    public ToolStripDropDownClosingEventArgs(ToolStripDropDownCloseReason reason) {
      CloseReason = reason;
    }

    public ToolStripDropDownCloseReason CloseReason { get; }
  }

  public class ToolStripDropDownClosedEventArgs : EventArgs {
    public ToolStripDropDownClosedEventArgs(ToolStripDropDownCloseReason reason) {
      CloseReason = reason;
    }

    public ToolStripDropDownCloseReason CloseReason { get; }
  }

  public delegate void ToolStripDropDownClosingEventHandler(object sender, ToolStripDropDownClosingEventArgs e);
  public delegate void ToolStripDropDownClosedEventHandler(object sender, ToolStripDropDownClosedEventArgs e);

  public enum ToolStripDropDownCloseReason {
    AppFocusChange = 0,
    AppClicked = 1,
    ItemClicked = 2,
    Keyboard = 3,
    CloseCalled = 4
  }

  // ---------------------------------------------------------------------------
  // PropertyValueChangedEventArgs
  // ---------------------------------------------------------------------------
  public class PropertyValueChangedEventArgs : EventArgs {
    public PropertyValueChangedEventArgs(GridItem changedItem, object oldValue) {
      ChangedItem = changedItem;
      OldValue = oldValue;
    }

    public GridItem ChangedItem { get; }
    public object OldValue { get; }
  }

  public delegate void PropertyValueChangedEventHandler(object sender, PropertyValueChangedEventArgs e);

  // ---------------------------------------------------------------------------
  // GridItem (needed by PropertyValueChangedEventArgs)
  // ---------------------------------------------------------------------------
  public abstract class GridItem {
    public abstract string Label { get; }
    public abstract object Value { get; }
    public abstract GridItem Parent { get; }
    public abstract GridItemCollection GridItems { get; }
    public abstract GridItemType GridItemType { get; }
    public bool Expandable { get; set; }
    public bool Expanded { get; set; }
    public object Tag { get; set; }
    public System.ComponentModel.PropertyDescriptor PropertyDescriptor { get; set; }
  }

  public class GridItemCollection : List<GridItem> { }

  public enum GridItemType {
    Property = 0,
    Category = 1,
    ArrayValue = 2,
    Root = 3
  }

  // ---------------------------------------------------------------------------
  // ImeMode enum
  // ---------------------------------------------------------------------------
  // ---------------------------------------------------------------------------
  // IMessageFilter
  // ---------------------------------------------------------------------------
  public interface IMessageFilter {
    bool PreFilterMessage(ref Message m);
  }

  // ---------------------------------------------------------------------------
  // Border3DStyle
  // ---------------------------------------------------------------------------
  public enum Border3DStyle {
    Adjust = 8192,
    Bump = 9,
    Etched = 6,
    Flat = 16394,
    Raised = 5,
    RaisedInner = 4,
    RaisedOuter = 1,
    Sunken = 10,
    SunkenInner = 8,
    SunkenOuter = 2
  }

  public enum ImeMode {
    Inherit = -1,
    NoControl = 0,
    On = 1,
    Off = 2,
    Disable = 3,
    Hiragana = 4,
    Katakana = 5,
    KatakanaHalf = 6,
    AlphaFull = 7,
    Alpha = 8,
    HangulFull = 9,
    Hangul = 10,
    Close = 11,
    OnHalf = 12
  }

  // ---------------------------------------------------------------------------
  // Border3DSide
  // ---------------------------------------------------------------------------
  public enum Border3DSide {
    Left = 1,
    Top = 2,
    Right = 4,
    Bottom = 8,
    Middle = 2048,
    All = 2063
  }

  // ---------------------------------------------------------------------------
  // ControlPaint
  // ---------------------------------------------------------------------------
  public static class ControlPaint {
    public static void DrawBorder3D(System.Drawing.Graphics g, System.Drawing.Rectangle rect, Border3DStyle style) { }
    public static void DrawBorder3D(System.Drawing.Graphics g, System.Drawing.Rectangle rect, Border3DStyle style, Border3DSide side) { }
    public static void DrawBorder3D(System.Drawing.Graphics g, int x, int y, int width, int height, Border3DStyle style) { }
    public static void DrawBorder3D(System.Drawing.Graphics g, int x, int y, int width, int height, Border3DStyle style, Border3DSide side) { }
    public static void DrawFocusRectangle(System.Drawing.Graphics g, System.Drawing.Rectangle rect) { }
    public static void DrawFocusRectangle(System.Drawing.Graphics g, System.Drawing.Rectangle rect, System.Drawing.Color foreColor, System.Drawing.Color backColor) { }
    public static void FillReversibleRectangle(System.Drawing.Rectangle rect, System.Drawing.Color color) { }
    public static System.Drawing.Color Light(System.Drawing.Color baseColor) => baseColor;
    public static System.Drawing.Color Light(System.Drawing.Color baseColor, float percOfLightLight) => baseColor;
    public static System.Drawing.Color Dark(System.Drawing.Color baseColor) => baseColor;
    public static System.Drawing.Color Dark(System.Drawing.Color baseColor, float percOfDarkDark) => baseColor;
    public static void DrawMenuGlyph(System.Drawing.Graphics g, System.Drawing.Rectangle rect, MenuGlyph glyph) { }
    public static void DrawMenuGlyph(System.Drawing.Graphics g, System.Drawing.Rectangle rect, MenuGlyph glyph, System.Drawing.Color foreColor, System.Drawing.Color backColor) { }
    public static void DrawMenuGlyph(System.Drawing.Graphics g, int x, int y, int w, int h, MenuGlyph glyph) { }

    public static void DrawStringDisabled(System.Drawing.Graphics g, string s, System.Drawing.Font font, System.Drawing.Color color, System.Drawing.RectangleF layoutRectangle, System.Drawing.StringFormat format) { }
    public static void DrawStringDisabled(System.Drawing.IDeviceContext dc, string s, System.Drawing.Font font, System.Drawing.Color color, System.Drawing.Rectangle layoutRectangle, TextFormatFlags format) { }
  }

  // ---------------------------------------------------------------------------
  // MenuGlyph
  // ---------------------------------------------------------------------------
  public enum MenuGlyph {
    Arrow = 0,
    Checkmark = 1,
    Bullet = 2,
    Min = 0,
    Max = 2
  }

  // ---------------------------------------------------------------------------
  // ContextMenu (legacy)
  // ---------------------------------------------------------------------------
  public class ContextMenu {
    public MenuItem.MenuItemCollection MenuItems { get; } = new MenuItem.MenuItemCollection();
    public void Show(Control control, System.Drawing.Point pos) { }
  }

  // ---------------------------------------------------------------------------
  // ToolStripControlHost
  // ---------------------------------------------------------------------------
  public class ToolStripControlHost : ToolStripItem {
    public ToolStripControlHost(Control c) { Control = c; }
    public Control Control { get; }
  }

  // ---------------------------------------------------------------------------
  // ToolStripDropDownButton
  // ---------------------------------------------------------------------------
  public class ToolStripDropDownButton : ToolStripItem {
    public ToolStripDropDown DropDown { get; set; } = new ToolStripDropDown();
    public bool ShowDropDownArrow { get; set; } = true;
    public event EventHandler DropDownOpening;
    public void ShowDropDown() { }
    public void HideDropDown() { }
  }

  // ---------------------------------------------------------------------------
  // KeysConverter
  // ---------------------------------------------------------------------------
  public class KeysConverter : System.ComponentModel.TypeConverter {
    public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
      return value?.ToString() ?? string.Empty;
    }
  }

  // ---------------------------------------------------------------------------
  // ToolStripTextDirection
  // ---------------------------------------------------------------------------
  public enum ToolStripTextDirection {
    Inherit = 0,
    Horizontal = 1,
    Vertical90 = 2,
    Vertical270 = 3
  }

  // ---------------------------------------------------------------------------
  // RadioButtonRenderer / RadioButtonState
  // ---------------------------------------------------------------------------
  public enum RadioButtonState {
    UncheckedNormal = 1,
    UncheckedHot = 2,
    UncheckedPressed = 3,
    UncheckedDisabled = 4,
    CheckedNormal = 5,
    CheckedHot = 6,
    CheckedPressed = 7,
    CheckedDisabled = 8
  }

  public static class RadioButtonRenderer {
    public static void DrawRadioButton(System.Drawing.Graphics g, System.Drawing.Point glyphLocation, RadioButtonState state) { }
    public static System.Drawing.Size GetGlyphSize(System.Drawing.Graphics g, RadioButtonState state) => new System.Drawing.Size(13, 13);
  }

  // ---------------------------------------------------------------------------
  // ScrollBarRenderer / enums
  // ---------------------------------------------------------------------------
  public enum ScrollBarState {
    Normal = 1,
    Hot = 2,
    Pressed = 3,
    Disabled = 4
  }

  public enum ScrollBarArrowButtonState {
    UpNormal = 1,
    UpHot = 2,
    UpPressed = 3,
    UpDisabled = 4,
    DownNormal = 5,
    DownHot = 6,
    DownPressed = 7,
    DownDisabled = 8,
    LeftNormal = 9,
    LeftHot = 10,
    LeftPressed = 11,
    LeftDisabled = 12,
    RightNormal = 13,
    RightHot = 14,
    RightPressed = 15,
    RightDisabled = 16
  }

  public static class ScrollBarRenderer {
    public static void DrawArrowButton(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, ScrollBarArrowButtonState state) { }
    public static void DrawHorizontalThumb(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, ScrollBarState state) { }
    public static void DrawHorizontalThumbGrip(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, ScrollBarState state) { }
    public static void DrawLeftHorizontalTrack(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, ScrollBarState state) { }
    public static void DrawLowerVerticalTrack(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, ScrollBarState state) { }
    public static void DrawRightHorizontalTrack(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, ScrollBarState state) { }
    public static void DrawUpperVerticalTrack(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, ScrollBarState state) { }
    public static void DrawVerticalThumb(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, ScrollBarState state) { }
    public static void DrawVerticalThumbGrip(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, ScrollBarState state) { }
    public static System.Drawing.Size GetThumbGripSize(System.Drawing.Graphics g, ScrollBarState state) => new System.Drawing.Size(8, 8);
    public static System.Drawing.Size GetSizeBoxSize(System.Drawing.Graphics g, ScrollBarState state) => new System.Drawing.Size(16, 16);
    public static bool IsSupported => false;
  }

  // ---------------------------------------------------------------------------
  // TextRenderer
  // ---------------------------------------------------------------------------
  public static class TextRenderer {
    public static void DrawText(System.Drawing.IDeviceContext dc, string text, System.Drawing.Font font, System.Drawing.Point pt, System.Drawing.Color foreColor) { }
    public static void DrawText(System.Drawing.IDeviceContext dc, string text, System.Drawing.Font font, System.Drawing.Rectangle bounds, System.Drawing.Color foreColor) { }
    public static void DrawText(System.Drawing.IDeviceContext dc, string text, System.Drawing.Font font, System.Drawing.Point pt, System.Drawing.Color foreColor, TextFormatFlags flags) { }
    public static void DrawText(System.Drawing.IDeviceContext dc, string text, System.Drawing.Font font, System.Drawing.Rectangle bounds, System.Drawing.Color foreColor, TextFormatFlags flags) { }
    public static void DrawText(System.Drawing.IDeviceContext dc, string text, System.Drawing.Font font, System.Drawing.Point pt, System.Drawing.Color foreColor, System.Drawing.Color backColor) { }
    public static void DrawText(System.Drawing.IDeviceContext dc, string text, System.Drawing.Font font, System.Drawing.Rectangle bounds, System.Drawing.Color foreColor, System.Drawing.Color backColor) { }
    public static void DrawText(System.Drawing.IDeviceContext dc, string text, System.Drawing.Font font, System.Drawing.Point pt, System.Drawing.Color foreColor, System.Drawing.Color backColor, TextFormatFlags flags) { }
    public static void DrawText(System.Drawing.IDeviceContext dc, string text, System.Drawing.Font font, System.Drawing.Rectangle bounds, System.Drawing.Color foreColor, System.Drawing.Color backColor, TextFormatFlags flags) { }
    public static void DrawText(System.Drawing.Graphics g, string text, System.Drawing.Font font, System.Drawing.Point pt, System.Drawing.Color foreColor) { }
    public static void DrawText(System.Drawing.Graphics g, string text, System.Drawing.Font font, System.Drawing.Rectangle bounds, System.Drawing.Color foreColor) { }
    public static void DrawText(System.Drawing.Graphics g, string text, System.Drawing.Font font, System.Drawing.Rectangle bounds, System.Drawing.Color foreColor, TextFormatFlags flags) { }
    public static System.Drawing.Size MeasureText(string text, System.Drawing.Font font) => new System.Drawing.Size(text?.Length * 7 ?? 0, 16);
    public static System.Drawing.Size MeasureText(System.Drawing.IDeviceContext dc, string text, System.Drawing.Font font) => MeasureText(text, font);
    public static System.Drawing.Size MeasureText(string text, System.Drawing.Font font, System.Drawing.Size proposedSize) => MeasureText(text, font);
    public static System.Drawing.Size MeasureText(System.Drawing.IDeviceContext dc, string text, System.Drawing.Font font, System.Drawing.Size proposedSize) => MeasureText(text, font);
    public static System.Drawing.Size MeasureText(string text, System.Drawing.Font font, System.Drawing.Size proposedSize, TextFormatFlags flags) => MeasureText(text, font);
    public static System.Drawing.Size MeasureText(System.Drawing.IDeviceContext dc, string text, System.Drawing.Font font, System.Drawing.Size proposedSize, TextFormatFlags flags) => MeasureText(text, font);
  }

  // ---------------------------------------------------------------------------
  // TabControlEventHandler delegate
  // ---------------------------------------------------------------------------
  public delegate void TabControlEventHandler(object sender, TabControlEventArgs e);
}

// =============================================================================
// System.Windows.Forms.VisualStyles namespace
// =============================================================================
namespace System.Windows.Forms.VisualStyles {

  public enum ColorProperty {
    BorderColor = 3801,
    FillColor = 3802,
    TextColor = 3803,
    EdgeLightColor = 3804,
    EdgeHighlightColor = 3805,
    EdgeShadowColor = 3806,
    EdgeDarkShadowColor = 3807,
    EdgeFillColor = 3808,
    TransparentColor = 3809,
    GradientColor1 = 3810,
    GradientColor2 = 3811,
    GradientColor3 = 3812,
    GradientColor4 = 3813,
    GradientColor5 = 3814,
    ShadowColor = 3815,
    GlowColor = 3816,
    TextBorderColor = 3817,
    TextShadowColor = 3818,
    GlyphTextColor = 3819,
    GlyphTransparentColor = 3820,
    FillColorHint = 3821,
    BorderColorHint = 3822,
    AccentColorHint = 3823
  }

  public class VisualStyleRenderer {
    public VisualStyleRenderer(VisualStyleElement element) { }
    public void DrawBackground(System.Drawing.Graphics g, System.Drawing.Rectangle bounds) { }
    public System.Drawing.Rectangle GetBackgroundContentRectangle(System.Drawing.Graphics g, System.Drawing.Rectangle bounds) => bounds;
    public IntPtr Handle => IntPtr.Zero;
    public static bool IsSupported => false;
    public static bool IsElementDefined(VisualStyleElement element) => false;

    public System.Drawing.Color GetColor(ColorProperty prop) {
      return System.Drawing.Color.Black;
    }
  }

  public class VisualStyleElement {
    public int ClassName { get; }
    public int Part { get; }
    public int State { get; }

    public static class Button {
      public static class GroupBox {
        public static VisualStyleElement Normal => new VisualStyleElement();
        public static VisualStyleElement Disabled => new VisualStyleElement();
      }
      public static class PushButton {
        public static VisualStyleElement Normal => new VisualStyleElement();
        public static VisualStyleElement Hot => new VisualStyleElement();
        public static VisualStyleElement Pressed => new VisualStyleElement();
        public static VisualStyleElement Disabled => new VisualStyleElement();
        public static VisualStyleElement Default => new VisualStyleElement();
      }
      public static class RadioButton {
        public static VisualStyleElement UncheckedNormal => new VisualStyleElement();
        public static VisualStyleElement CheckedNormal => new VisualStyleElement();
      }
      public static class CheckBox {
        public static VisualStyleElement UncheckedNormal => new VisualStyleElement();
        public static VisualStyleElement CheckedNormal => new VisualStyleElement();
      }
    }

    public static class Window {
      public static class Caption {
        public static VisualStyleElement Active => new VisualStyleElement();
        public static VisualStyleElement Inactive => new VisualStyleElement();
      }
    }

    public static class ScrollBar {
      public static class ArrowButton {
        public static VisualStyleElement UpNormal => new VisualStyleElement();
      }
      public static class ThumbButtonHorizontal {
        public static VisualStyleElement Normal => new VisualStyleElement();
      }
      public static class ThumbButtonVertical {
        public static VisualStyleElement Normal => new VisualStyleElement();
      }
    }
  }
}

// =============================================================================
// System.Windows.Forms.Design namespace
// =============================================================================
namespace System.Windows.Forms.Design {

  // ---------------------------------------------------------------------------
  // ControlDesigner
  // ---------------------------------------------------------------------------
  public class ControlDesigner : System.ComponentModel.Design.ComponentDesigner {
    public virtual System.Windows.Forms.Control Control { get; set; }

    public override System.ComponentModel.Design.DesignerVerbCollection Verbs => new System.ComponentModel.Design.DesignerVerbCollection();

    protected Behavior.BehaviorService BehaviorService => new Behavior.BehaviorService();

    protected virtual void OnPaintAdornments(System.Windows.Forms.PaintEventArgs pe) { }

    protected virtual void WndProc(ref System.Windows.Forms.Message message) { }
  }

  // ---------------------------------------------------------------------------
  // IWindowsFormsEditorService (also in System.Drawing.Design, aliased here)
  // ---------------------------------------------------------------------------
  public interface IWindowsFormsEditorService {
    void DropDownControl(System.Windows.Forms.Control control);
    void CloseDropDown();
    System.Windows.Forms.DialogResult ShowDialog(System.Windows.Forms.Form dialog);
  }

}

// =============================================================================
// System.Windows.Forms.Design.Behavior namespace
// =============================================================================
namespace System.Windows.Forms.Design.Behavior {

  // ---------------------------------------------------------------------------
  // BehaviorService
  // ---------------------------------------------------------------------------
  public class BehaviorService {
    public BehaviorServiceAdornerCollection Adorners { get; } = new BehaviorServiceAdornerCollection();

    public Behavior GetNextBehavior() => null;

    public System.Drawing.Point AdornerWindowPointToScreen(System.Drawing.Point p) => p;
    public System.Drawing.Point ScreenToAdornerWindow(System.Drawing.Point p) => p;
    public System.Drawing.Point ControlToAdornerWindow(System.Windows.Forms.Control c) => System.Drawing.Point.Empty;
    public void Invalidate() { }
    public void Invalidate(System.Drawing.Rectangle rect) { }

    public class BehaviorServiceAdornerCollection : List<Adorner> { }
  }

  // ---------------------------------------------------------------------------
  // Behavior
  // ---------------------------------------------------------------------------
  public abstract class Behavior {
    public virtual System.Windows.Forms.Cursor Cursor => System.Windows.Forms.Cursors.Default;
    public virtual bool OnMouseDown(Glyph g, System.Windows.Forms.MouseButtons button, System.Drawing.Point mouseLoc) => false;
    public virtual bool OnMouseUp(Glyph g, System.Windows.Forms.MouseButtons button) => false;
    public virtual bool OnMouseMove(Glyph g, System.Windows.Forms.MouseButtons button, System.Drawing.Point mouseLoc) => false;
  }

  // ---------------------------------------------------------------------------
  // Glyph
  // ---------------------------------------------------------------------------
  public abstract class Glyph {
    protected Glyph(Behavior behavior) {
      this.Behavior = behavior;
    }

    public virtual System.Drawing.Rectangle Bounds { get; }
    public Behavior Behavior { get; set; }

    public abstract System.Windows.Forms.Cursor GetHitTest(System.Drawing.Point p);
    public abstract void Paint(System.Windows.Forms.PaintEventArgs pe);
  }

  // ---------------------------------------------------------------------------
  // Adorner
  // ---------------------------------------------------------------------------
  public class Adorner {
    public GlyphCollection Glyphs { get; } = new GlyphCollection();
    public bool Enabled { get; set; } = true;
    public BehaviorService BehaviorService { get; set; }
  }

  public class GlyphCollection : List<Glyph> { }
}

// =============================================================================
// System.ComponentModel.Design namespace
// =============================================================================
namespace System.ComponentModel.Design {

  // ---------------------------------------------------------------------------
  // ComponentDesigner
  // ---------------------------------------------------------------------------
  public class ComponentDesigner : IDisposable {
    public IComponent Component { get; set; }
    public virtual System.Collections.ICollection AssociatedComponents => Array.Empty<IComponent>();

    public virtual DesignerVerbCollection Verbs => new DesignerVerbCollection();

    public virtual void Initialize(IComponent component) {
      Component = component;
    }

    public virtual void InitializeNewComponent(System.Collections.IDictionary defaultValues) { }

    protected virtual object GetService(Type serviceType) => null;

    protected void RaiseComponentChanged(System.ComponentModel.MemberDescriptor member, object oldValue, object newValue) { }

    protected void RaiseComponentChanging(System.ComponentModel.MemberDescriptor member) { }

    public void Dispose() {
      Dispose(true);
    }

    protected virtual void Dispose(bool disposing) { }
  }


  // ---------------------------------------------------------------------------
  // CollectionEditor
  // ---------------------------------------------------------------------------
  public class CollectionEditor : System.Drawing.Design.UITypeEditor {
    private Type _collectionItemType;

    public CollectionEditor(Type type) {
      CollectionType = type;
    }

    public Type CollectionType { get; }

    protected Type CollectionItemType {
      get => _collectionItemType ?? typeof(object);
      set => _collectionItemType = value;
    }

    protected object GetService(Type serviceType) => null;

    protected virtual Type CreateCollectionItemType() {
      return typeof(object);
    }

    protected virtual Type[] CreateNewItemTypes() {
      return new Type[] { CollectionItemType };
    }

    protected virtual CollectionForm CreateCollectionForm() {
      return new CollectionForm(this);
    }

    protected virtual object CreateInstance(Type itemType) {
      return Activator.CreateInstance(itemType);
    }

    protected virtual void DestroyInstance(object instance) {
      if (instance is IDisposable d) d.Dispose();
    }

    protected virtual string GetDisplayText(object value) {
      return value?.ToString() ?? string.Empty;
    }

    public System.ComponentModel.ITypeDescriptorContext Context { get; set; }
    public System.ComponentModel.Design.IDesignerHost DesignerHost { get; set; }

    // -----------------------------------------------------------------------
    // CollectionForm (nested class)
    // -----------------------------------------------------------------------
    public class CollectionForm : System.Windows.Forms.Form {
      public CollectionForm(CollectionEditor editor) {
        Editor = editor;
      }

      protected CollectionEditor Editor { get; }
      protected Type CollectionItemType => Editor?.CollectionItemType ?? typeof(object);
      protected Type[] NewItemTypes => Editor?.CreateNewItemTypes() ?? new Type[] { typeof(object) };

      public object[] Items { get; set; } = Array.Empty<object>();
      protected object[] EditValue { get; set; }

      protected virtual void OnEditValueChanged() { }

      protected virtual System.Windows.Forms.DialogResult ShowEditorDialog(System.Windows.Forms.Design.IWindowsFormsEditorService edSvc) {
        return System.Windows.Forms.DialogResult.OK;
      }

      protected void DestroyInstance(object instance) {
        Editor?.DestroyInstance(instance);
      }

      protected void DisplayError(Exception e) { }
      protected void DisplayError(string message) { }

      protected object CreateInstance(Type itemType) {
        return Editor?.CreateInstance(itemType);
      }
    }
  }
}

// =============================================================================
// System.Drawing.Design namespace additions
// =============================================================================
namespace System.Drawing.Design {

  // ---------------------------------------------------------------------------
  // PaintValueEventArgs
  // ---------------------------------------------------------------------------
  public class PaintValueEventArgs : EventArgs {
    public PaintValueEventArgs(System.ComponentModel.ITypeDescriptorContext context, object value, Graphics graphics, Rectangle bounds) {
      Context = context;
      Value = value;
      Graphics = graphics;
      Bounds = bounds;
    }

    public System.ComponentModel.ITypeDescriptorContext Context { get; }
    public object Value { get; }
    public Graphics Graphics { get; }
    public Rectangle Bounds { get; }
  }
}
