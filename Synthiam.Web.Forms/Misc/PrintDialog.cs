// =============================================================================
// Synthiam.Web.Forms - Print-related stubs for Blazor
// System.Drawing.Printing types (PrintDocument, PrinterSettings, PageSettings,
// PrintPageEventArgs, etc.) are now provided by System.Drawing.Common.
// Only System.Windows.Forms dialog stubs remain here.
// =============================================================================

using System;
using System.Drawing;
using System.Drawing.Printing;

// =============================================================================
// Print dialogs in System.Windows.Forms namespace
// =============================================================================
namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // PrintDialog
  // ---------------------------------------------------------------------------
  public class PrintDialog : CommonDialog {

    private PrintDocument _document;
    private bool _allowSomePages;
    private bool _allowSelection;
    private bool _allowCurrentPage;
    private bool _allowPrintToFile = true;
    private bool _showHelp;
    private bool _showNetwork = true;
    private bool _useEXDialog = true;
    private PrinterSettings _printerSettings = new PrinterSettings();

    public PrintDocument Document {
      get => _document;
      set { _document = value; if (value != null) _printerSettings = value.PrinterSettings; }
    }

    public bool AllowSomePages {
      get => _allowSomePages;
      set => _allowSomePages = value;
    }

    public bool AllowSelection {
      get => _allowSelection;
      set => _allowSelection = value;
    }

    public bool AllowCurrentPage {
      get => _allowCurrentPage;
      set => _allowCurrentPage = value;
    }

    public bool AllowPrintToFile {
      get => _allowPrintToFile;
      set => _allowPrintToFile = value;
    }

    public bool ShowHelp {
      get => _showHelp;
      set => _showHelp = value;
    }

    public bool ShowNetwork {
      get => _showNetwork;
      set => _showNetwork = value;
    }

    public bool UseEXDialog {
      get => _useEXDialog;
      set => _useEXDialog = value;
    }

    public PrinterSettings PrinterSettings {
      get => _printerSettings;
      set => _printerSettings = value ?? new PrinterSettings();
    }

    protected override bool RunDialog(IntPtr hwndOwner) {
      return true;
    }

    public override void Reset() {
      _document = null;
      _allowSomePages = false;
      _allowSelection = false;
      _allowCurrentPage = false;
      _allowPrintToFile = true;
      _showHelp = false;
      _showNetwork = true;
      _printerSettings = new PrinterSettings();
    }
  }

  // ---------------------------------------------------------------------------
  // PrintPreviewDialog
  // ---------------------------------------------------------------------------
  public class PrintPreviewDialog : Form {

    private PrintDocument _document;
    private bool _useAntiAlias;

    public PrintDocument Document {
      get => _document;
      set => _document = value;
    }

    public bool UseAntiAlias {
      get => _useAntiAlias;
      set => _useAntiAlias = value;
    }
  }

  // ---------------------------------------------------------------------------
  // PageSetupDialog
  // ---------------------------------------------------------------------------
  public class PageSetupDialog : CommonDialog {

    private PrintDocument _document;
    private PageSettings _pageSettings;
    private PrinterSettings _printerSettings;
    private bool _allowMargins = true;
    private bool _allowOrientation = true;
    private bool _allowPaper = true;
    private bool _allowPrinter = true;
    private bool _showHelp;
    private bool _showNetwork = true;
    private Margins _minMargins = new Margins(0, 0, 0, 0);

    public PrintDocument Document {
      get => _document;
      set => _document = value;
    }

    public PageSettings PageSettings {
      get => _pageSettings;
      set => _pageSettings = value;
    }

    public PrinterSettings PrinterSettings {
      get => _printerSettings;
      set => _printerSettings = value;
    }

    public bool AllowMargins {
      get => _allowMargins;
      set => _allowMargins = value;
    }

    public bool AllowOrientation {
      get => _allowOrientation;
      set => _allowOrientation = value;
    }

    public bool AllowPaper {
      get => _allowPaper;
      set => _allowPaper = value;
    }

    public bool AllowPrinter {
      get => _allowPrinter;
      set => _allowPrinter = value;
    }

    public bool ShowHelp {
      get => _showHelp;
      set => _showHelp = value;
    }

    public bool ShowNetwork {
      get => _showNetwork;
      set => _showNetwork = value;
    }

    public Margins MinMargins {
      get => _minMargins;
      set => _minMargins = value ?? new Margins(0, 0, 0, 0);
    }

    protected override bool RunDialog(IntPtr hwndOwner) {
      return true;
    }

    public override void Reset() {
      _document = null;
      _pageSettings = null;
      _printerSettings = null;
      _allowMargins = true;
      _allowOrientation = true;
      _allowPaper = true;
      _allowPrinter = true;
      _showHelp = false;
      _showNetwork = true;
      _minMargins = new Margins(0, 0, 0, 0);
    }
  }
}
