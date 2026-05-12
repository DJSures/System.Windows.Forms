// =============================================================================
// Synthiam.Web.Forms - File dialogs for Blazor
// =============================================================================

using System;
using System.IO;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // FileDialog (abstract base)
  // ---------------------------------------------------------------------------
  public abstract class FileDialog : CommonDialog {

    private string _fileName = string.Empty;
    private string[] _fileNames = Array.Empty<string>();
    private string _filter = string.Empty;
    private int _filterIndex = 1;
    private string _initialDirectory = string.Empty;
    private string _title = string.Empty;
    private string _defaultExt = string.Empty;
    private bool _addExtension = true;
    private bool _checkFileExists;
    private bool _checkPathExists = true;
    private bool _restoreDirectory;
    private bool _autoUpgradeEnabled = true;
    private bool _supportMultiDottedExtensions;
    private bool _validateNames = true;

    public string FileName {
      get => _fileName;
      set => _fileName = value ?? string.Empty;
    }

    public string[] FileNames {
      get => _fileNames;
      protected set => _fileNames = value ?? Array.Empty<string>();
    }

    public string Filter {
      get => _filter;
      set => _filter = value ?? string.Empty;
    }

    public int FilterIndex {
      get => _filterIndex;
      set => _filterIndex = value;
    }

    public string InitialDirectory {
      get => _initialDirectory;
      set => _initialDirectory = value ?? string.Empty;
    }

    public string Title {
      get => _title;
      set => _title = value ?? string.Empty;
    }

    public string DefaultExt {
      get => _defaultExt;
      set => _defaultExt = value ?? string.Empty;
    }

    public bool AddExtension {
      get => _addExtension;
      set => _addExtension = value;
    }

    public bool CheckFileExists {
      get => _checkFileExists;
      set => _checkFileExists = value;
    }

    public bool CheckPathExists {
      get => _checkPathExists;
      set => _checkPathExists = value;
    }

    public bool RestoreDirectory {
      get => _restoreDirectory;
      set => _restoreDirectory = value;
    }

    public bool AutoUpgradeEnabled {
      get => _autoUpgradeEnabled;
      set => _autoUpgradeEnabled = value;
    }

    public bool SupportMultiDottedExtensions {
      get => _supportMultiDottedExtensions;
      set => _supportMultiDottedExtensions = value;
    }

    public bool ValidateNames {
      get => _validateNames;
      set => _validateNames = value;
    }

    public event System.ComponentModel.CancelEventHandler FileOk;

    protected virtual void OnFileOk(System.ComponentModel.CancelEventArgs e) {
      FileOk?.Invoke(this, e);
    }

    public override void Reset() {
      _fileName = string.Empty;
      _fileNames = Array.Empty<string>();
      _filter = string.Empty;
      _filterIndex = 1;
      _initialDirectory = string.Empty;
      _title = string.Empty;
      _defaultExt = string.Empty;
      _addExtension = true;
      _checkFileExists = false;
      _checkPathExists = true;
      _restoreDirectory = false;
    }
  }

  // ---------------------------------------------------------------------------
  // OpenFileDialog
  // ---------------------------------------------------------------------------
  public class OpenFileDialog : FileDialog {

    private bool _multiselect;
    private bool _readOnlyChecked;
    private bool _showReadOnly;

    public bool Multiselect {
      get => _multiselect;
      set => _multiselect = value;
    }

    public bool ReadOnlyChecked {
      get => _readOnlyChecked;
      set => _readOnlyChecked = value;
    }

    public bool ShowReadOnly {
      get => _showReadOnly;
      set => _showReadOnly = value;
    }

    public string SafeFileName => Path.GetFileName(FileName);

    public string[] SafeFileNames {
      get {
        var names = FileNames;
        var safe = new string[names.Length];
        for (int i = 0; i < names.Length; i++)
          safe[i] = Path.GetFileName(names[i]);
        return safe;
      }
    }

    public Stream OpenFile() {
      if (string.IsNullOrEmpty(FileName)) return null;
      // Stub: in web context this would need JS interop
      return Stream.Null;
    }

    protected override bool RunDialog(IntPtr hwndOwner) {
      // Stub: will need JS interop to show file picker in browser
      return false;
    }

    public override void Reset() {
      base.Reset();
      _multiselect = false;
      _readOnlyChecked = false;
      _showReadOnly = false;
    }
  }

  // ---------------------------------------------------------------------------
  // SaveFileDialog
  // ---------------------------------------------------------------------------
  public class SaveFileDialog : FileDialog {

    private bool _createPrompt;
    private bool _overwritePrompt = true;

    public bool CreatePrompt {
      get => _createPrompt;
      set => _createPrompt = value;
    }

    public bool OverwritePrompt {
      get => _overwritePrompt;
      set => _overwritePrompt = value;
    }

    public Stream OpenFile() {
      // Stub: in web context this would need JS interop
      return Stream.Null;
    }

    protected override bool RunDialog(IntPtr hwndOwner) {
      // Stub: will need JS interop
      return false;
    }

    public override void Reset() {
      base.Reset();
      _createPrompt = false;
      _overwritePrompt = true;
    }
  }

  // ---------------------------------------------------------------------------
  // FolderBrowserDialog
  // ---------------------------------------------------------------------------
  public class FolderBrowserDialog : CommonDialog {

    private string _selectedPath = string.Empty;
    private string _description = string.Empty;
    private bool _showNewFolderButton = true;
    private Environment.SpecialFolder _rootFolder = Environment.SpecialFolder.Desktop;

    public string SelectedPath {
      get => _selectedPath;
      set => _selectedPath = value ?? string.Empty;
    }

    public string Description {
      get => _description;
      set => _description = value ?? string.Empty;
    }

    public bool ShowNewFolderButton {
      get => _showNewFolderButton;
      set => _showNewFolderButton = value;
    }

    public Environment.SpecialFolder RootFolder {
      get => _rootFolder;
      set => _rootFolder = value;
    }

    protected override bool RunDialog(IntPtr hwndOwner) {
      // Stub
      return false;
    }

    public override void Reset() {
      _selectedPath = string.Empty;
      _description = string.Empty;
      _showNewFolderButton = true;
      _rootFolder = Environment.SpecialFolder.Desktop;
    }
  }
}
