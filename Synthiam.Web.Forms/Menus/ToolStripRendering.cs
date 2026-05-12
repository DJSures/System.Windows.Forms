// =============================================================================
// Synthiam.Web.Forms - ToolStrip Rendering types for Blazor
// =============================================================================

using System.Drawing;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // ProfessionalColorTable
  // ---------------------------------------------------------------------------
  public class ProfessionalColorTable {

    public bool UseSystemColors { get; set; } = true;

    public virtual Color ButtonSelectedHighlight => SystemColors.ButtonHighlight;
    public virtual Color ButtonSelectedHighlightBorder => SystemColors.Highlight;
    public virtual Color ButtonPressedHighlight => SystemColors.ButtonHighlight;
    public virtual Color ButtonPressedHighlightBorder => SystemColors.Highlight;
    public virtual Color ButtonCheckedHighlight => SystemColors.ButtonHighlight;
    public virtual Color ButtonCheckedHighlightBorder => SystemColors.Highlight;
    public virtual Color ButtonPressedBorder => SystemColors.Highlight;
    public virtual Color ButtonSelectedBorder => SystemColors.Highlight;
    public virtual Color ButtonCheckedGradientBegin => SystemColors.ButtonHighlight;
    public virtual Color ButtonCheckedGradientMiddle => SystemColors.ButtonHighlight;
    public virtual Color ButtonCheckedGradientEnd => SystemColors.ButtonHighlight;
    public virtual Color ButtonSelectedGradientBegin => SystemColors.ButtonHighlight;
    public virtual Color ButtonSelectedGradientMiddle => SystemColors.ButtonHighlight;
    public virtual Color ButtonSelectedGradientEnd => SystemColors.ButtonHighlight;
    public virtual Color ButtonPressedGradientBegin => SystemColors.ButtonHighlight;
    public virtual Color ButtonPressedGradientMiddle => SystemColors.ButtonHighlight;
    public virtual Color ButtonPressedGradientEnd => SystemColors.ButtonHighlight;
    public virtual Color CheckBackground => SystemColors.ButtonHighlight;
    public virtual Color CheckSelectedBackground => SystemColors.ButtonHighlight;
    public virtual Color CheckPressedBackground => SystemColors.ButtonHighlight;
    public virtual Color GripDark => SystemColors.ControlDark;
    public virtual Color GripLight => SystemColors.ControlLight;
    public virtual Color ImageMarginGradientBegin => SystemColors.Control;
    public virtual Color ImageMarginGradientMiddle => SystemColors.Control;
    public virtual Color ImageMarginGradientEnd => SystemColors.Control;
    public virtual Color ImageMarginRevealedGradientBegin => SystemColors.Control;
    public virtual Color ImageMarginRevealedGradientMiddle => SystemColors.Control;
    public virtual Color ImageMarginRevealedGradientEnd => SystemColors.Control;
    public virtual Color MenuStripGradientBegin => SystemColors.Control;
    public virtual Color MenuStripGradientEnd => SystemColors.Control;
    public virtual Color MenuItemSelected => SystemColors.ButtonHighlight;
    public virtual Color MenuItemBorder => SystemColors.Highlight;
    public virtual Color MenuBorder => SystemColors.ControlDark;
    public virtual Color MenuItemSelectedGradientBegin => SystemColors.ButtonHighlight;
    public virtual Color MenuItemSelectedGradientEnd => SystemColors.ButtonHighlight;
    public virtual Color MenuItemPressedGradientBegin => SystemColors.ButtonHighlight;
    public virtual Color MenuItemPressedGradientMiddle => SystemColors.ButtonHighlight;
    public virtual Color MenuItemPressedGradientEnd => SystemColors.ButtonHighlight;
    public virtual Color RaftingContainerGradientBegin => SystemColors.Control;
    public virtual Color RaftingContainerGradientEnd => SystemColors.Control;
    public virtual Color SeparatorDark => SystemColors.ControlDark;
    public virtual Color SeparatorLight => SystemColors.ControlLight;
    public virtual Color StatusStripGradientBegin => SystemColors.Control;
    public virtual Color StatusStripGradientEnd => SystemColors.Control;
    public virtual Color ToolStripBorder => SystemColors.ControlDark;
    public virtual Color ToolStripDropDownBackground => SystemColors.Control;
    public virtual Color ToolStripGradientBegin => SystemColors.Control;
    public virtual Color ToolStripGradientMiddle => SystemColors.Control;
    public virtual Color ToolStripGradientEnd => SystemColors.Control;
    public virtual Color ToolStripContentPanelGradientBegin => SystemColors.Control;
    public virtual Color ToolStripContentPanelGradientEnd => SystemColors.Control;
    public virtual Color ToolStripPanelGradientBegin => SystemColors.Control;
    public virtual Color ToolStripPanelGradientEnd => SystemColors.Control;
    public virtual Color OverflowButtonGradientBegin => SystemColors.Control;
    public virtual Color OverflowButtonGradientMiddle => SystemColors.Control;
    public virtual Color OverflowButtonGradientEnd => SystemColors.Control;
  }

  // ---------------------------------------------------------------------------
  // ToolStripRenderEventArgs
  // ---------------------------------------------------------------------------
  public class ToolStripRenderEventArgs : EventArgs {

    public ToolStripRenderEventArgs(Graphics graphics, ToolStrip toolStrip) {
      Graphics = graphics;
      ToolStrip = toolStrip;
    }

    public Graphics Graphics { get; }
    public ToolStrip ToolStrip { get; }
    public Rectangle AffectedBounds => ToolStrip != null
      ? new Rectangle(Point.Empty, ToolStrip.Size)
      : Rectangle.Empty;
    public Color BackColor => ToolStrip?.BackColor ?? Color.Empty;
    public Rectangle ConnectedArea => Rectangle.Empty;
  }

  // ---------------------------------------------------------------------------
  // ToolStripItemRenderEventArgs
  // ---------------------------------------------------------------------------
  public class ToolStripItemRenderEventArgs : EventArgs {

    public ToolStripItemRenderEventArgs(Graphics graphics, ToolStripItem item) {
      Graphics = graphics;
      Item = item;
    }

    public Graphics Graphics { get; }
    public ToolStripItem Item { get; }
    public ToolStrip ToolStrip => Item?.Owner;
  }

  // ---------------------------------------------------------------------------
  // ToolStripItemTextRenderEventArgs
  // ---------------------------------------------------------------------------
  public class ToolStripItemTextRenderEventArgs : ToolStripItemRenderEventArgs {

    public ToolStripItemTextRenderEventArgs(Graphics graphics, ToolStripItem item, string text, Rectangle textRectangle, Color textColor, Font textFont, TextFormatFlags textFormat)
      : base(graphics, item) {
      Text = text;
      TextRectangle = textRectangle;
      TextColor = textColor;
      TextFont = textFont;
      TextFormat = textFormat;
    }

    public ToolStripItemTextRenderEventArgs(Graphics graphics, ToolStripItem item, string text, Rectangle textRectangle, Color textColor, Font textFont, ContentAlignment textAlign)
      : base(graphics, item) {
      Text = text;
      TextRectangle = textRectangle;
      TextColor = textColor;
      TextFont = textFont;
    }

    public string Text { get; set; }
    public Rectangle TextRectangle { get; set; }
    public Color TextColor { get; set; }
    public Font TextFont { get; set; }
    public TextFormatFlags TextFormat { get; set; }
    public ContentAlignment TextAlign { get; set; }
    public ToolStripItemTextRenderDirection TextDirection { get; set; }
  }

  // ---------------------------------------------------------------------------
  // ToolStripSeparatorRenderEventArgs
  // ---------------------------------------------------------------------------
  public class ToolStripSeparatorRenderEventArgs : ToolStripItemRenderEventArgs {

    public ToolStripSeparatorRenderEventArgs(Graphics graphics, ToolStripSeparator separator, bool vertical)
      : base(graphics, separator) {
      Vertical = vertical;
    }

    public bool Vertical { get; }
  }

  // ---------------------------------------------------------------------------
  // Supporting enums
  // ---------------------------------------------------------------------------

  public enum ToolStripItemTextRenderDirection {
    Horizontal = 0,
    Vertical90 = 1,
    Vertical270 = 2
  }

  [Flags]
  public enum TextFormatFlags {
    Default = 0,
    GlyphOverhangPadding = 0,
    Left = 0,
    Top = 0,
    HorizontalCenter = 1,
    Right = 2,
    VerticalCenter = 4,
    Bottom = 8,
    WordBreak = 16,
    SingleLine = 32,
    ExpandTabs = 64,
    NoClipping = 256,
    ExternalLeading = 512,
    NoPrefix = 2048,
    Internal = 4096,
    TextBoxControl = 8192,
    PathEllipsis = 16384,
    EndEllipsis = 32768,
    ModifyString = 65536,
    RightToLeft = 131072,
    WordEllipsis = 262144,
    NoFullWidthCharacterBreak = 524288,
    HidePrefix = 1048576,
    PrefixOnly = 2097152,
    PreserveGraphicsClipping = 16777216,
    PreserveGraphicsTranslateTransform = 33554432,
    NoPadding = 268435456,
    LeftAndRightPadding = 536870912
  }

  // ---------------------------------------------------------------------------
  // ToolStripRenderer (base class)
  // ---------------------------------------------------------------------------
  public class ToolStripRenderer {

    public event ToolStripRenderEventHandler RenderToolStripBackground;
    public event ToolStripRenderEventHandler RenderToolStripBorder;
    public event ToolStripItemRenderEventHandler RenderButtonBackground;
    public event ToolStripItemRenderEventHandler RenderDropDownButtonBackground;
    public event ToolStripItemRenderEventHandler RenderOverflowButtonBackground;
    public event ToolStripItemRenderEventHandler RenderItemBackground;
    public event ToolStripItemTextRenderEventHandler RenderItemText;
    public event ToolStripItemRenderEventHandler RenderItemImage;
    public event ToolStripItemRenderEventHandler RenderItemCheck;
    public event ToolStripSeparatorRenderEventHandler RenderSeparator;
    public event ToolStripRenderEventHandler RenderToolStripStatusLabelBackground;
    public event ToolStripRenderEventHandler RenderStatusStripSizingGrip;
    public event ToolStripItemRenderEventHandler RenderMenuItemBackground;
    public event ToolStripRenderEventHandler RenderToolStripContentPanelBackground;
    public event ToolStripRenderEventHandler RenderToolStripPanelBackground;
    public event ToolStripGripRenderEventHandler RenderGrip;
    public event ToolStripArrowRenderEventHandler RenderArrow;
    public event ToolStripItemRenderEventHandler RenderImageMargin;

    protected virtual void OnRenderToolStripBackground(ToolStripRenderEventArgs e) {
      RenderToolStripBackground?.Invoke(this, e);
    }

    protected virtual void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
      RenderToolStripBorder?.Invoke(this, e);
    }

    protected virtual void OnRenderButtonBackground(ToolStripItemRenderEventArgs e) {
      RenderButtonBackground?.Invoke(this, e);
    }

    protected virtual void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e) {
      RenderDropDownButtonBackground?.Invoke(this, e);
    }

    protected virtual void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e) {
      RenderOverflowButtonBackground?.Invoke(this, e);
    }

    protected virtual void OnRenderItemBackground(ToolStripItemRenderEventArgs e) {
      RenderItemBackground?.Invoke(this, e);
    }

    protected virtual void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {
      RenderItemText?.Invoke(this, e);
    }

    protected virtual void OnRenderItemImage(ToolStripItemRenderEventArgs e) {
      RenderItemImage?.Invoke(this, e);
    }

    protected virtual void OnRenderItemCheck(ToolStripItemRenderEventArgs e) {
      RenderItemCheck?.Invoke(this, e);
    }

    protected virtual void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e) {
      RenderSeparator?.Invoke(this, e);
    }

    protected virtual void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {
      RenderMenuItemBackground?.Invoke(this, e);
    }

    protected virtual void OnRenderGrip(ToolStripGripRenderEventArgs e) {
      RenderGrip?.Invoke(this, e);
    }

    protected virtual void OnRenderArrow(ToolStripArrowRenderEventArgs e) {
      RenderArrow?.Invoke(this, e);
    }

    protected virtual void OnRenderImageMargin(ToolStripItemRenderEventArgs e) {
      RenderImageMargin?.Invoke(this, e);
    }
  }

  // ---------------------------------------------------------------------------
  // ToolStripProfessionalRenderer
  // ---------------------------------------------------------------------------
  public class ToolStripProfessionalRenderer : ToolStripRenderer {

    public ToolStripProfessionalRenderer() {
      ColorTable = new ProfessionalColorTable();
    }

    public ToolStripProfessionalRenderer(ProfessionalColorTable professionalColorTable) {
      ColorTable = professionalColorTable ?? new ProfessionalColorTable();
    }

    public ProfessionalColorTable ColorTable { get; }
    public bool RoundedEdges { get; set; } = true;

    protected virtual void OnRenderLabelBackground(ToolStripItemRenderEventArgs e) { }
  }

  // ---------------------------------------------------------------------------
  // Additional ToolStrip render event args and delegates
  // ---------------------------------------------------------------------------

  public class ToolStripGripRenderEventArgs : ToolStripRenderEventArgs {

    public ToolStripGripRenderEventArgs(Graphics graphics, ToolStrip toolStrip)
      : base(graphics, toolStrip) {
    }

    public Rectangle GripBounds { get; set; }
    public ToolStripGripDisplayStyle GripDisplayStyle { get; set; }
    public ToolStripGripStyle GripStyle { get; set; }
  }

  public class ToolStripArrowRenderEventArgs : EventArgs {

    public ToolStripArrowRenderEventArgs(Graphics graphics, ToolStripItem item, Rectangle arrowRectangle, Color arrowColor, ArrowDirection direction) {
      Graphics = graphics;
      Item = item;
      ArrowRectangle = arrowRectangle;
      ArrowColor = arrowColor;
      Direction = direction;
    }

    public Graphics Graphics { get; }
    public ToolStripItem Item { get; }
    public Rectangle ArrowRectangle { get; set; }
    public Color ArrowColor { get; set; }
    public ArrowDirection Direction { get; set; }
  }

  public enum ArrowDirection {
    Left = 0,
    Up = 1,
    Right = 16,
    Down = 17
  }

  // Delegates
  public delegate void ToolStripRenderEventHandler(object sender, ToolStripRenderEventArgs e);
  public delegate void ToolStripItemRenderEventHandler(object sender, ToolStripItemRenderEventArgs e);
  public delegate void ToolStripItemTextRenderEventHandler(object sender, ToolStripItemTextRenderEventArgs e);
  public delegate void ToolStripSeparatorRenderEventHandler(object sender, ToolStripSeparatorRenderEventArgs e);
  public delegate void ToolStripGripRenderEventHandler(object sender, ToolStripGripRenderEventArgs e);
  public delegate void ToolStripArrowRenderEventHandler(object sender, ToolStripArrowRenderEventArgs e);
}
