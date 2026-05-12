// =============================================================================
// Synthiam.Web.Forms - TextBoxBase control for Blazor
// =============================================================================

using System;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  public class TextBoxBase : Control {

    private bool _readOnly;
    private bool _multiline;
    private bool _wordWrap = true;
    private int _maxLength = 32767;
    private string[] _lines = Array.Empty<string>();
    private int _selectionStart;
    private int _selectionLength;
    private bool _modified;
    private ScrollBars _scrollBars = ScrollBars.None;
    private bool _hideSelection = true;
    private bool _acceptsTab;
    private bool _acceptsReturn;
    private bool _shortcutsEnabled = true;
    private BorderStyle _borderStyle = BorderStyle.Fixed3D;

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public bool ReadOnly {
      get => _readOnly;
      set { _readOnly = value; NotifyStateChanged(); }
    }

    public virtual bool Multiline {
      get => _multiline;
      set { _multiline = value; NotifyStateChanged(); }
    }

    public bool WordWrap {
      get => _wordWrap;
      set { _wordWrap = value; InvalidateCssStyle(); NotifyStateChanged(); }
    }

    public virtual int MaxLength {
      get => _maxLength;
      set => _maxLength = value;
    }

    public string[] Lines {
      get => Text?.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None) ?? Array.Empty<string>();
      set {
        _lines = value ?? Array.Empty<string>();
        Text = string.Join(Environment.NewLine, _lines);
      }
    }

    public int SelectionStart {
      get => _selectionStart;
      set => _selectionStart = Math.Max(0, value);
    }

    public int SelectionLength {
      get => _selectionLength;
      set => _selectionLength = Math.Max(0, value);
    }

    public string SelectedText {
      get {
        if (string.IsNullOrEmpty(Text) || _selectionLength == 0) return string.Empty;
        int start = Math.Min(_selectionStart, Text.Length);
        int len = Math.Min(_selectionLength, Text.Length - start);
        return Text.Substring(start, len);
      }
      set {
        if (Text == null) return;
        int start = Math.Min(_selectionStart, Text.Length);
        int len = Math.Min(_selectionLength, Text.Length - start);
        Text = Text.Remove(start, len).Insert(start, value ?? string.Empty);
      }
    }

    public bool Modified {
      get => _modified;
      set {
        if (_modified != value) {
          _modified = value;
          OnModifiedChanged(EventArgs.Empty);
        }
      }
    }

    public int TextLength => Text?.Length ?? 0;

    public ScrollBars ScrollBars {
      get => _scrollBars;
      set { _scrollBars = value; InvalidateCssStyle(); NotifyStateChanged(); }
    }

    public bool HideSelection {
      get => _hideSelection;
      set => _hideSelection = value;
    }

    public bool AcceptsTab {
      get => _acceptsTab;
      set => _acceptsTab = value;
    }

    public bool AcceptsReturn {
      get => _acceptsReturn;
      set => _acceptsReturn = value;
    }

    public bool ShortcutsEnabled {
      get => _shortcutsEnabled;
      set => _shortcutsEnabled = value;
    }

    public BorderStyle BorderStyle {
      get => _borderStyle;
      set { _borderStyle = value; InvalidateCssStyle(); NotifyStateChanged(); }
    }

    public bool CanUndo => false;

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event EventHandler ModifiedChanged;

    protected virtual void OnModifiedChanged(EventArgs e) {
      ModifiedChanged?.Invoke(this, e);
    }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public void Select(int start, int length) {
      _selectionStart = Math.Max(0, start);
      _selectionLength = Math.Max(0, length);
    }

    public void SelectAll() {
      _selectionStart = 0;
      _selectionLength = Text?.Length ?? 0;
    }

    public void Clear() {
      Text = string.Empty;
    }

    public void Copy() {
      // Stub: clipboard access requires JS interop
    }

    public void Cut() {
      // Stub
    }

    public void Paste() {
      // Stub
    }

    public void Undo() {
      // Stub
    }

    public void AppendText(string text) {
      Text += text ?? string.Empty;
    }

    public void ScrollToCaret() {
      // Stub: handled by browser
    }

    public void DeselectAll() {
      _selectionLength = 0;
    }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();

      if (_borderStyle == BorderStyle.FixedSingle)
        style += "border:1px solid #808080;";
      else if (_borderStyle == BorderStyle.Fixed3D)
        style += "border:2px inset;";
      else if (_borderStyle == BorderStyle.None)
        style += "border:none;";

      if (!_wordWrap)
        style += "white-space:pre;overflow-x:auto;";

      if (_scrollBars == ScrollBars.Vertical || _scrollBars == ScrollBars.Both)
        style += "overflow-y:auto;";
      if (_scrollBars == ScrollBars.Horizontal || _scrollBars == ScrollBars.Both)
        style += "overflow-x:auto;";

      return style;
    }
  }
}
