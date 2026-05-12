// =============================================================================
// Synthiam.Web.Forms - RichTextBox control for Blazor
// =============================================================================

using System;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  public class RichTextBox : TextBoxBase {

    private string _rtf = string.Empty;
    private bool _detectUrls = true;
    private RichTextBoxScrollBars _richScrollBars = RichTextBoxScrollBars.Both;
    private float _zoomFactor = 1.0f;
    private Color _selectionColor = Color.Empty;
    private Font _selectionFont;
    private HorizontalAlignment _selectionAlignment = HorizontalAlignment.Left;
    private bool _autoWordSelection;
    private int _bulletIndent;
    private bool _enableAutoDragDrop;
    private int _rightMargin;
    private bool _showSelectionMargin;
    private int _selectionIndent;
    private int _selectionRightIndent;
    private int _selectionHangingIndent;

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public string Rtf {
      get => _rtf;
      set { _rtf = value ?? string.Empty; NotifyStateChanged(); }
    }

    public bool DetectUrls {
      get => _detectUrls;
      set => _detectUrls = value;
    }

    public RichTextBoxScrollBars RichTextBoxScrollBars {
      get => _richScrollBars;
      set { _richScrollBars = value; NotifyStateChanged(); }
    }

    public new RichTextBoxScrollBars ScrollBars {
      get => _richScrollBars;
      set { _richScrollBars = value; NotifyStateChanged(); }
    }

    public float ZoomFactor {
      get => _zoomFactor;
      set {
        _zoomFactor = Math.Max(0.015625f, Math.Min(64f, value));
        NotifyStateChanged();
      }
    }

    public Color SelectionColor {
      get => _selectionColor;
      set => _selectionColor = value;
    }

    public Font SelectionFont {
      get => _selectionFont;
      set => _selectionFont = value;
    }

    public HorizontalAlignment SelectionAlignment {
      get => _selectionAlignment;
      set => _selectionAlignment = value;
    }

    public bool AutoWordSelection {
      get => _autoWordSelection;
      set => _autoWordSelection = value;
    }

    public int BulletIndent {
      get => _bulletIndent;
      set => _bulletIndent = value;
    }

    public bool EnableAutoDragDrop {
      get => _enableAutoDragDrop;
      set => _enableAutoDragDrop = value;
    }

    public int RightMargin {
      get => _rightMargin;
      set => _rightMargin = value;
    }

    public bool ShowSelectionMargin {
      get => _showSelectionMargin;
      set => _showSelectionMargin = value;
    }

    public string SelectedRtf { get; set; } = string.Empty;

    public int SelectionIndent {
      get => _selectionIndent;
      set => _selectionIndent = value;
    }

    public int SelectionRightIndent {
      get => _selectionRightIndent;
      set => _selectionRightIndent = value;
    }

    public int SelectionHangingIndent {
      get => _selectionHangingIndent;
      set => _selectionHangingIndent = value;
    }

    public bool SelectionProtected { get; set; }
    public int[] SelectionTabs { get; set; }
    public int SelectionCharOffset { get; set; }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public event LinkClickedEventHandler LinkClicked;

    protected virtual void OnLinkClicked(LinkClickedEventArgs e) {
      LinkClicked?.Invoke(this, e);
    }

    public int Find(string str) {
      if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(Text)) return -1;
      return Text.IndexOf(str, StringComparison.Ordinal);
    }

    public int Find(string str, int start, int end) {
      return Find(str, start, end, RichTextBoxFinds.None);
    }

    public int Find(string str, int start, RichTextBoxFinds options) {
      return Find(str, start, Text?.Length ?? 0, options);
    }

    public int Find(string str, int start, int end, RichTextBoxFinds options) {
      if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(Text)) return -1;
      var comparison = (options & RichTextBoxFinds.MatchCase) != 0
        ? StringComparison.Ordinal
        : StringComparison.OrdinalIgnoreCase;
      int maxLen = Math.Min(end, Text.Length);
      return Text.IndexOf(str, start, maxLen - start, comparison);
    }

    public int Find(string str, RichTextBoxFinds options) {
      return Find(str, 0, Text?.Length ?? 0, options);
    }

    public int Find(char[] characterSet) {
      if (characterSet == null || Text == null) return -1;
      return Text.IndexOfAny(characterSet);
    }

    public void LoadFile(string path) {
      // Stub: file system access not available in browser
    }

    public void LoadFile(string path, RichTextBoxStreamType fileType) {
      // Stub
    }

    public void SaveFile(string path) {
      // Stub
    }

    public void SaveFile(string path, RichTextBoxStreamType fileType) {
      // Stub
    }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "div";

    protected override string GetCssClasses() {
      return base.GetCssClasses() + " swf-richtextbox";
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "overflow:auto;white-space:pre-wrap;word-wrap:break-word;";

      if (_zoomFactor != 1.0f)
        style += "zoom:" + _zoomFactor.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + ";";

      var scrollBars = _richScrollBars;
      if (scrollBars == RichTextBoxScrollBars.Vertical || scrollBars == RichTextBoxScrollBars.ForcedVertical)
        style += "overflow-x:hidden;overflow-y:auto;";
      else if (scrollBars == RichTextBoxScrollBars.Horizontal || scrollBars == RichTextBoxScrollBars.ForcedHorizontal)
        style += "overflow-x:auto;overflow-y:hidden;";
      else if (scrollBars == RichTextBoxScrollBars.Both || scrollBars == RichTextBoxScrollBars.ForcedBoth)
        style += "overflow:auto;";
      else if (scrollBars == RichTextBoxScrollBars.None)
        style += "overflow:hidden;";

      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      builder.AddAttribute(seq++, "contenteditable", ReadOnly ? "false" : "true");
      builder.AddMarkupContent(seq++, Text ?? string.Empty);
    }

    protected override void AddEventAttributes(RenderTreeBuilder builder, ref int seq) {
      base.AddEventAttributes(builder, ref seq);

      builder.AddAttribute(seq++, "oninput",
        EventCallback.Factory.Create<ChangeEventArgs>(
          GetBlazorReceiver(),
          (ChangeEventArgs e) => {
            Text = e.Value?.ToString() ?? string.Empty;
            Modified = true;
          }));
    }
  }

  // ---------------------------------------------------------------------------
  // RichTextBox enums
  // ---------------------------------------------------------------------------
  [Flags]
  public enum RichTextBoxFinds {
    None = 0,
    WholeWord = 2,
    MatchCase = 4,
    NoHighlight = 8,
    Reverse = 16
  }

  public enum RichTextBoxStreamType {
    RichText = 0,
    PlainText = 1,
    RichNoOleObjs = 2,
    TextTextOleObjs = 3,
    UnicodePlainText = 4
  }
}
