// =============================================================================
// Synthiam.Web.Forms - FontDialog for Blazor
// =============================================================================

using System;
using System.Drawing;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // FontDialog
  // ---------------------------------------------------------------------------
  public class FontDialog : CommonDialog {

    private Font _font;
    private Color _color = Color.Black;
    private bool _showColor;
    private bool _showApply;
    private bool _showEffects = true;
    private bool _showHelp;
    private int _minSize;
    private int _maxSize;
    private bool _allowSimulations = true;
    private bool _allowVectorFonts = true;
    private bool _allowVerticalFonts = true;
    private bool _allowScriptChange = true;
    private bool _fixedPitchOnly;
    private bool _fontMustExist;
    private bool _scriptsOnly;

    public Font Font {
      get => _font;
      set => _font = value;
    }

    public Color Color {
      get => _color;
      set => _color = value;
    }

    public bool ShowColor {
      get => _showColor;
      set => _showColor = value;
    }

    public bool ShowApply {
      get => _showApply;
      set => _showApply = value;
    }

    public bool ShowEffects {
      get => _showEffects;
      set => _showEffects = value;
    }

    public bool ShowHelp {
      get => _showHelp;
      set => _showHelp = value;
    }

    public int MinSize {
      get => _minSize;
      set => _minSize = value;
    }

    public int MaxSize {
      get => _maxSize;
      set => _maxSize = value;
    }

    public bool AllowSimulations {
      get => _allowSimulations;
      set => _allowSimulations = value;
    }

    public bool AllowVectorFonts {
      get => _allowVectorFonts;
      set => _allowVectorFonts = value;
    }

    public bool AllowVerticalFonts {
      get => _allowVerticalFonts;
      set => _allowVerticalFonts = value;
    }

    public bool AllowScriptChange {
      get => _allowScriptChange;
      set => _allowScriptChange = value;
    }

    public bool FixedPitchOnly {
      get => _fixedPitchOnly;
      set => _fixedPitchOnly = value;
    }

    public bool FontMustExist {
      get => _fontMustExist;
      set => _fontMustExist = value;
    }

    public bool ScriptsOnly {
      get => _scriptsOnly;
      set => _scriptsOnly = value;
    }

    public event EventHandler Apply;

    protected virtual void OnApply(EventArgs e) {
      Apply?.Invoke(this, e);
    }

    protected override bool RunDialog(IntPtr hwndOwner) {
      // Stub: will need custom Blazor modal for font selection
      return false;
    }

    public override void Reset() {
      _font = null;
      _color = Color.Black;
      _showColor = false;
      _showApply = false;
      _showEffects = true;
      _showHelp = false;
      _minSize = 0;
      _maxSize = 0;
      _allowSimulations = true;
      _allowVectorFonts = true;
      _allowVerticalFonts = true;
      _allowScriptChange = true;
      _fixedPitchOnly = false;
      _fontMustExist = false;
      _scriptsOnly = false;
    }
  }
}
