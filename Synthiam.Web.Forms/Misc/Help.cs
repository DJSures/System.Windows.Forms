// =============================================================================
// Synthiam.Web.Forms - Help class for Blazor
// =============================================================================

using System;
using System.Drawing;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // HelpNavigator enum
  // ---------------------------------------------------------------------------
  public enum HelpNavigator {
    Topic = 0,
    TableOfContents = 1,
    Index = 2,
    Find = 3,
    AssociateIndex = 4,
    KeywordIndex = 5,
    TopicId = 6
  }

  // ---------------------------------------------------------------------------
  // Help
  // ---------------------------------------------------------------------------
  public static class Help {

    public static void ShowHelp(Control parent, string url) {
      // Stub: will open URL in browser via JS interop
    }

    public static void ShowHelp(Control parent, string url, string keyword) {
      // Stub
    }

    public static void ShowHelp(Control parent, string url, HelpNavigator navigator) {
      // Stub
    }

    public static void ShowHelp(Control parent, string url, HelpNavigator navigator, object parameter) {
      // Stub
    }

    public static void ShowHelpIndex(Control parent, string url) {
      // Stub
    }

    public static void ShowPopup(Control parent, string caption, Point location) {
      // Stub
    }
  }
}
