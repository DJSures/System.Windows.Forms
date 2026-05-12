// =============================================================================
// Synthiam.Web.Forms - TextBox control for Blazor
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // AutoCompleteStringCollection
  // ---------------------------------------------------------------------------
  public class AutoCompleteStringCollection : IList<string>, IList {

    private readonly List<string> _list = new List<string>();

    public int Count => _list.Count;
    public bool IsReadOnly => false;
    public bool IsFixedSize => false;
    public bool IsSynchronized => false;
    public object SyncRoot => _list;

    public string this[int index] {
      get => _list[index];
      set => _list[index] = value;
    }

    object IList.this[int index] {
      get => _list[index];
      set => _list[index] = (string)value;
    }

    public void Add(string item) => _list.Add(item);
    public void AddRange(string[] items) => _list.AddRange(items);
    public void Clear() => _list.Clear();
    public bool Contains(string item) => _list.Contains(item);
    public void CopyTo(string[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
    public int IndexOf(string item) => _list.IndexOf(item);
    public void Insert(int index, string item) => _list.Insert(index, item);
    public bool Remove(string item) => _list.Remove(item);
    public void RemoveAt(int index) => _list.RemoveAt(index);
    public IEnumerator<string> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

    int IList.Add(object value) { _list.Add((string)value); return _list.Count - 1; }
    bool IList.Contains(object value) => _list.Contains((string)value);
    int IList.IndexOf(object value) => _list.IndexOf((string)value);
    void IList.Insert(int index, object value) => _list.Insert(index, (string)value);
    void IList.Remove(object value) => _list.Remove((string)value);
    void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);
  }

  // ---------------------------------------------------------------------------
  // TextBox
  // ---------------------------------------------------------------------------
  public class TextBox : TextBoxBase {

    private CharacterCasing _characterCasing = CharacterCasing.Normal;
    private char _passwordChar = '\0';
    private bool _useSystemPasswordChar;
    private AutoCompleteMode _autoCompleteMode = AutoCompleteMode.None;
    private AutoCompleteSource _autoCompleteSource = AutoCompleteSource.None;
    private AutoCompleteStringCollection _autoCompleteCustomSource = new AutoCompleteStringCollection();
    private string _placeholderText = string.Empty;
    private HorizontalAlignment _textAlign = HorizontalAlignment.Left;

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public CharacterCasing CharacterCasing {
      get => _characterCasing;
      set { _characterCasing = value; InvalidateCssStyle(); NotifyStateChanged(); }
    }

    public char PasswordChar {
      get => _passwordChar;
      set { _passwordChar = value; NotifyStateChanged(); }
    }

    public bool UseSystemPasswordChar {
      get => _useSystemPasswordChar;
      set { _useSystemPasswordChar = value; NotifyStateChanged(); }
    }

    public AutoCompleteMode AutoCompleteMode {
      get => _autoCompleteMode;
      set => _autoCompleteMode = value;
    }

    public AutoCompleteSource AutoCompleteSource {
      get => _autoCompleteSource;
      set => _autoCompleteSource = value;
    }

    public AutoCompleteStringCollection AutoCompleteCustomSource {
      get => _autoCompleteCustomSource;
      set => _autoCompleteCustomSource = value ?? new AutoCompleteStringCollection();
    }

    public string PlaceholderText {
      get => _placeholderText;
      set { _placeholderText = value ?? string.Empty; NotifyStateChanged(); }
    }

    public HorizontalAlignment TextAlign {
      get => _textAlign;
      set { _textAlign = value; InvalidateCssStyle(); NotifyStateChanged(); }
    }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public int GetFirstCharIndexOfCurrentLine() {
      // Stub: approximate based on SelectionStart
      if (string.IsNullOrEmpty(Text) || SelectionStart <= 0) return 0;
      int idx = Text.LastIndexOf('\n', Math.Min(SelectionStart - 1, Text.Length - 1));
      return idx < 0 ? 0 : idx + 1;
    }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => Multiline ? "textarea" : "input";

    protected override string GetCssClasses() {
      var css = base.GetCssClasses();
      if (Multiline) css += " swf-textbox-multiline";
      return css;
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();

      // Text alignment
      switch (_textAlign) {
        case HorizontalAlignment.Center: style += "text-align:center;"; break;
        case HorizontalAlignment.Right: style += "text-align:right;"; break;
        default: style += "text-align:left;"; break;
      }

      // Character casing
      if (_characterCasing == CharacterCasing.Upper)
        style += "text-transform:uppercase;";
      else if (_characterCasing == CharacterCasing.Lower)
        style += "text-transform:lowercase;";

      // Resize for textarea
      if (Multiline)
        style += "resize:none;";

      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      // ReadOnly attribute must come before any content (Blazor requirement)
      if (ReadOnly)
        builder.AddAttribute(seq++, "readonly", true);

      if (!Multiline) {
        // For <input>, set type and value as attributes
        bool isPassword = _useSystemPasswordChar || _passwordChar != '\0';
        builder.AddAttribute(seq++, "type", isPassword ? "password" : "text");
        builder.AddAttribute(seq++, "value", Text ?? string.Empty);

        if (MaxLength > 0 && MaxLength < 32767)
          builder.AddAttribute(seq++, "maxlength", MaxLength);

        if (!string.IsNullOrEmpty(_placeholderText))
          builder.AddAttribute(seq++, "placeholder", _placeholderText);
      } else {
        // For <textarea>, attributes must come before content
        if (MaxLength > 0 && MaxLength < 32767)
          builder.AddAttribute(seq++, "maxlength", MaxLength);

        if (!string.IsNullOrEmpty(_placeholderText))
          builder.AddAttribute(seq++, "placeholder", _placeholderText);

        // Content must be last (no more attributes after this)
        builder.AddContent(seq++, Text ?? string.Empty);
      }
    }

    protected override void AddEventAttributes(RenderTreeBuilder builder, ref int seq) {
      base.AddEventAttributes(builder, ref seq);

      // Two-way binding via oninput
      builder.AddAttribute(seq++, "oninput",
        EventCallback.Factory.Create<ChangeEventArgs>(
          GetBlazorReceiver(),
          (ChangeEventArgs e) => {
            var newValue = e.Value?.ToString() ?? string.Empty;
            Text = newValue;
            Modified = true;
          }));
    }

    internal override void BuildRenderTree(RenderTreeBuilder builder, ref int seq) {
      if (!Visible) return;

      EnsureHandleCreated();

      builder.OpenElement(seq++, GetHtmlTag());
      builder.SetKey(this);

      if (!string.IsNullOrEmpty(Name))
        builder.AddAttribute(seq++, "id", string.Concat("swf-", Name));

      // Use cached CSS strings for consistent Blazor diffing
      var css = _cachedCssClasses ?? (_cachedCssClasses = GetCssClasses());
      builder.AddAttribute(seq++, "class", css);

      var style = _cachedCssStyle ?? (_cachedCssStyle = BuildCssStyle());
      if (!string.IsNullOrEmpty(style))
        builder.AddAttribute(seq++, "style", style);

      if (_toolTipText != null)
        builder.AddAttribute(seq++, "title", _toolTipText);

      if (!Enabled)
        builder.AddAttribute(seq++, "disabled", true);

      if (TabStop && TabIndex >= 0)
        builder.AddAttribute(seq++, "tabindex", TabIndex);

      // Event handlers first
      AddEventAttributes(builder, ref seq);

      // Content (includes attributes for input and textarea content)
      RenderContent(builder, ref seq);

      builder.CloseElement();
    }
  }
}
