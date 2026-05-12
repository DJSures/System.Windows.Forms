// =============================================================================
// Synthiam.Web.Forms - ColorDialog for Blazor
// =============================================================================

using System;
using System.Drawing;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // ColorDialog
  // ---------------------------------------------------------------------------
  public class ColorDialog : CommonDialog {

    private Color _color = Color.Black;
    private bool _allowFullOpen = true;
    private bool _anyColor;
    private bool _fullOpen;
    private bool _solidColorOnly;
    private int[] _customColors;
    private bool _showHelp;

    public Color Color {
      get => _color;
      set => _color = value;
    }

    public bool AllowFullOpen {
      get => _allowFullOpen;
      set => _allowFullOpen = value;
    }

    public bool AnyColor {
      get => _anyColor;
      set => _anyColor = value;
    }

    public bool FullOpen {
      get => _fullOpen;
      set => _fullOpen = value;
    }

    public bool SolidColorOnly {
      get => _solidColorOnly;
      set => _solidColorOnly = value;
    }

    public int[] CustomColors {
      get => _customColors;
      set => _customColors = value;
    }

    public bool ShowHelp {
      get => _showHelp;
      set => _showHelp = value;
    }

    protected override bool RunDialog(IntPtr hwndOwner) {
      // Stub: will need JS interop (HTML5 <input type="color"> or custom picker)
      return false;
    }

    public override void Reset() {
      _color = Color.Black;
      _allowFullOpen = true;
      _anyColor = false;
      _fullOpen = false;
      _solidColorOnly = false;
      _customColors = null;
      _showHelp = false;
    }
  }
}
