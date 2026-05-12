// =============================================================================
// Synthiam.Web.Forms - VisualStyles stubs for Blazor
// =============================================================================

using System.Drawing;

namespace System.Windows.Forms.VisualStyles {

  // ---------------------------------------------------------------------------
  // CheckBoxState enum
  // ---------------------------------------------------------------------------
  public enum CheckBoxState {
    UncheckedNormal = 1,
    UncheckedHot = 2,
    UncheckedPressed = 3,
    UncheckedDisabled = 4,
    CheckedNormal = 5,
    CheckedHot = 6,
    CheckedPressed = 7,
    CheckedDisabled = 8,
    MixedNormal = 9,
    MixedHot = 10,
    MixedPressed = 11,
    MixedDisabled = 12
  }

  // ---------------------------------------------------------------------------
  // GroupBoxState enum
  // ---------------------------------------------------------------------------
  public enum GroupBoxState {
    Normal = 1,
    Disabled = 2
  }

  // ---------------------------------------------------------------------------
  // CheckBoxRenderer
  // ---------------------------------------------------------------------------
  public static class CheckBoxRenderer {

    public static void DrawCheckBox(Graphics g, Point glyphLocation, CheckBoxState state) {
      // Stub: no-op in Blazor
    }

    public static void DrawCheckBox(Graphics g, Point glyphLocation, Rectangle textBounds, string checkBoxText, Font font, bool focused, CheckBoxState state) {
      // Stub: no-op in Blazor
    }

    public static void DrawCheckBox(Graphics g, Point glyphLocation, Rectangle textBounds, string checkBoxText, Font font, TextFormatFlags flags, bool focused, CheckBoxState state) {
      // Stub: no-op in Blazor
    }

    public static void DrawCheckBox(Graphics g, Point glyphLocation, Rectangle textBounds, string checkBoxText, Font font, Image image, Rectangle imageBounds, bool focused, CheckBoxState state) {
      // Stub: no-op in Blazor
    }

    public static Size GetGlyphSize(Graphics g, CheckBoxState state) {
      return new Size(13, 13);
    }

    public static bool IsBackgroundPartiallyTransparent(CheckBoxState state) {
      return false;
    }

    public static void DrawParentBackground(Graphics g, Rectangle bounds, Control childControl) {
      // Stub: no-op
    }

    public static bool RenderMatchingApplicationState { get; set; } = true;
  }

  // ---------------------------------------------------------------------------
  // GroupBoxRenderer
  // ---------------------------------------------------------------------------
  public static class GroupBoxRenderer {

    public static void DrawGroupBox(Graphics g, Rectangle bounds, GroupBoxState state) {
      // Stub: no-op in Blazor
    }

    public static void DrawGroupBox(Graphics g, Rectangle bounds, string groupBoxText, Font font, GroupBoxState state) {
      // Stub: no-op in Blazor
    }

    public static void DrawGroupBox(Graphics g, Rectangle bounds, string groupBoxText, Font font, Color textColor, GroupBoxState state) {
      // Stub: no-op in Blazor
    }

    public static void DrawGroupBox(Graphics g, Rectangle bounds, string groupBoxText, Font font, TextFormatFlags flags, GroupBoxState state) {
      // Stub: no-op in Blazor
    }

    public static void DrawParentBackground(Graphics g, Rectangle bounds, Control childControl) {
      // Stub: no-op
    }

    public static bool IsBackgroundPartiallyTransparent(GroupBoxState state) {
      return false;
    }

    public static bool RenderMatchingApplicationState { get; set; } = true;
  }
}
