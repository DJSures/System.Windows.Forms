// =============================================================================
// Synthiam.Web.Forms - Button control for Blazor
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Synthiam.Web.Forms;
using BlazorMouseEventArgs = Microsoft.AspNetCore.Components.Web.MouseEventArgs;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // TextImageRelation enum
  // ---------------------------------------------------------------------------
  public enum TextImageRelation {
    Overlay = 0,
    ImageBeforeText = 1,
    TextBeforeImage = 2,
    ImageAboveText = 4,
    TextAboveImage = 8
  }

  // ---------------------------------------------------------------------------
  // FlatButtonAppearance
  // ---------------------------------------------------------------------------
  public class FlatButtonAppearance {
    public Color BorderColor { get; set; } = Color.Empty;
    public int BorderSize { get; set; } = 1;
    public Color MouseDownBackColor { get; set; } = Color.Empty;
    public Color MouseOverBackColor { get; set; } = Color.Empty;
    public Color CheckedBackColor { get; set; } = Color.Empty;
  }

  // ---------------------------------------------------------------------------
  // Button
  // ---------------------------------------------------------------------------
  public partial class Button : Control {

    private FlatStyle _flatStyle = FlatStyle.Standard;
    private FlatButtonAppearance _flatAppearance = new FlatButtonAppearance();
    private DialogResult _dialogResult = DialogResult.None;
    private Image _image;
    private ContentAlignment _imageAlign = ContentAlignment.MiddleCenter;
    private ContentAlignment _textAlign = ContentAlignment.MiddleCenter;
    private TextImageRelation _textImageRelation = TextImageRelation.Overlay;
    private bool _useVisualStyleBackColor = true;
    private bool _autoEllipsis;

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public FlatStyle FlatStyle {
      get => _flatStyle;
      set { _flatStyle = value; InvalidateCssStyle(); InvalidateCssClasses(); NotifyStateChanged(); }
    }

    public FlatButtonAppearance FlatAppearance => _flatAppearance;

    public DialogResult DialogResult {
      get => _dialogResult;
      set => _dialogResult = value;
    }

    public Image Image {
      get => _image;
      set { _image = value; NotifyStateChanged(); }
    }

    public ContentAlignment ImageAlign {
      get => _imageAlign;
      set { _imageAlign = value; NotifyStateChanged(); }
    }

    public ContentAlignment TextAlign {
      get => _textAlign;
      set { _textAlign = value; InvalidateCssStyle(); NotifyStateChanged(); }
    }

    public TextImageRelation TextImageRelation {
      get => _textImageRelation;
      set { _textImageRelation = value; NotifyStateChanged(); }
    }

    public bool UseVisualStyleBackColor {
      get => _useVisualStyleBackColor;
      set => _useVisualStyleBackColor = value;
    }

    public bool AutoEllipsis {
      get => _autoEllipsis;
      set { _autoEllipsis = value; InvalidateCssStyle(); NotifyStateChanged(); }
    }

    public int ImageIndex { get; set; } = -1;
    public string ImageKey { get; set; } = string.Empty;
    public ImageList ImageList { get; set; }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "button";

    protected override string GetCssClasses() {
      var css = base.GetCssClasses();
      if (_flatStyle == FlatStyle.Flat) css += " swf-button-flat";
      else if (_flatStyle == FlatStyle.Popup) css += " swf-button-popup";
      return css;
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();

      // Text alignment
      style += ContentAlignmentToCss(_textAlign);

      // Auto ellipsis
      if (_autoEllipsis)
        style += "text-overflow:ellipsis;overflow:hidden;white-space:nowrap;";

      // Flat appearance border
      if (_flatStyle == FlatStyle.Flat) {
        if (!_flatAppearance.BorderColor.IsEmpty)
          style += "border-color:" + _flatAppearance.BorderColor.ToCss() + ";";
        style += "border-width:" + _flatAppearance.BorderSize + "px;border-style:solid;";
      }

      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      // Render image if present
      if (_image != null) {
        var dataUri = _image.ToDataUri();
        if (!string.IsNullOrEmpty(dataUri)) {
          builder.OpenElement(seq++, "img");
          builder.AddAttribute(seq++, "src", dataUri);
          builder.AddAttribute(seq++, "style", "vertical-align:middle;pointer-events:none;");
          builder.CloseElement();
        }
      }

      // Render text
      if (!string.IsNullOrEmpty(Text)) {
        builder.OpenElement(seq++, "span");
        builder.AddContent(seq++, Text);
        builder.CloseElement();
      }
    }

    protected override void AddEventAttributes(RenderTreeBuilder builder, ref int seq) {
      base.AddEventAttributes(builder, ref seq);

      // Always add onclick for button behavior (DialogResult support).
      // This overwrites any onclick added by base, but our handler also fires
      // OnClick and OnMouseClick so all events still propagate correctly.
      builder.AddAttribute(seq++, "onclick",
        EventCallback.Factory.Create<BlazorMouseEventArgs>(
          GetBlazorReceiver(),
          (BlazorMouseEventArgs e) => {
            if (_dialogResult != DialogResult.None) {
              var form = FindForm();
              if (form != null)
                form.DialogResult = _dialogResult;
            }
            var mea = new MouseEventArgs(
              e.Button == 0 ? MouseButtons.Left : e.Button == 2 ? MouseButtons.Right : MouseButtons.Middle,
              1, (int)e.OffsetX, (int)e.OffsetY, 0);
            OnClick(EventArgs.Empty);
            OnMouseClick(mea);
          }));

      // Set button type
      builder.AddAttribute(seq++, "type", "button");
    }

    // ═══════════════════════════════════════════════
    // Helpers
    // ═══════════════════════════════════════════════

    internal static string ContentAlignmentToCss(ContentAlignment align) {
      string justifyContent;
      string textAlign;
      string verticalAlign;

      switch (align) {
        case ContentAlignment.TopLeft:
          justifyContent = "flex-start"; textAlign = "left"; verticalAlign = "flex-start"; break;
        case ContentAlignment.TopCenter:
          justifyContent = "center"; textAlign = "center"; verticalAlign = "flex-start"; break;
        case ContentAlignment.TopRight:
          justifyContent = "flex-end"; textAlign = "right"; verticalAlign = "flex-start"; break;
        case ContentAlignment.MiddleLeft:
          justifyContent = "flex-start"; textAlign = "left"; verticalAlign = "center"; break;
        case ContentAlignment.MiddleCenter:
          justifyContent = "center"; textAlign = "center"; verticalAlign = "center"; break;
        case ContentAlignment.MiddleRight:
          justifyContent = "flex-end"; textAlign = "right"; verticalAlign = "center"; break;
        case ContentAlignment.BottomLeft:
          justifyContent = "flex-start"; textAlign = "left"; verticalAlign = "flex-end"; break;
        case ContentAlignment.BottomCenter:
          justifyContent = "center"; textAlign = "center"; verticalAlign = "flex-end"; break;
        case ContentAlignment.BottomRight:
          justifyContent = "flex-end"; textAlign = "right"; verticalAlign = "flex-end"; break;
        default:
          justifyContent = "center"; textAlign = "center"; verticalAlign = "center"; break;
      }

      return "display:flex;justify-content:" + justifyContent +
             ";align-items:" + verticalAlign + ";text-align:" + textAlign + ";";
    }

    public void PerformClick() {
      OnClick(EventArgs.Empty);
    }

    public void NotifyDefault(bool value) {
      // Stub for IButtonControl compatibility
    }
  }

  // ---------------------------------------------------------------------------
  // ImageList (stub for controls that reference it)
  // ---------------------------------------------------------------------------
  public class ImageList : IDisposable {

    private ImageCollection _images;

    public ImageList() {
      _images = new ImageCollection(this);
    }

    public ImageList(System.ComponentModel.IContainer container) : this() {
    }

    public Size ImageSize { get; set; } = new Size(16, 16);
    public ColorDepth ColorDepth { get; set; } = ColorDepth.Depth32Bit;
    public Color TransparentColor { get; set; } = Color.Transparent;
    public ImageCollection Images => _images;
    public ImageListStreamer ImageStream { get; set; }
    public string Tag { get; set; }

    public void Draw(Graphics g, Point pt, int index) { }
    public void Draw(Graphics g, int x, int y, int index) { }
    public void Draw(Graphics g, int x, int y, int width, int height, int index) { }

    public class ImageCollection : IList<Image>, System.Collections.IList {
      private readonly List<Image> _list = new List<Image>();
      private readonly ImageList _owner;
      internal ImageCollection(ImageList owner) { _owner = owner; }

      public int Count => _list.Count;
      public bool IsReadOnly => false;
      public bool IsFixedSize => false;
      public bool IsSynchronized => false;
      public object SyncRoot => _list;

      public Image this[int index] {
        get => _list[index];
        set => _list[index] = value;
      }

      object System.Collections.IList.this[int index] {
        get => _list[index];
        set => _list[index] = (Image)value;
      }

      public void Add(Image image) => _list.Add(image);
      public void AddRange(Image[] images) => _list.AddRange(images);
      public void Clear() => _list.Clear();
      public bool Contains(Image item) => _list.Contains(item);
      public void CopyTo(Image[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
      public int IndexOf(Image item) => _list.IndexOf(item);
      public void Insert(int index, Image item) => _list.Insert(index, item);
      public bool Remove(Image item) => _list.Remove(item);
      public void RemoveAt(int index) => _list.RemoveAt(index);
      public IEnumerator<Image> GetEnumerator() => _list.GetEnumerator();
      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _list.GetEnumerator();

      int System.Collections.IList.Add(object value) { _list.Add((Image)value); return _list.Count - 1; }
      bool System.Collections.IList.Contains(object value) => _list.Contains((Image)value);
      int System.Collections.IList.IndexOf(object value) => _list.IndexOf((Image)value);
      void System.Collections.IList.Insert(int index, object value) => _list.Insert(index, (Image)value);
      void System.Collections.IList.Remove(object value) => _list.Remove((Image)value);
      void System.Collections.ICollection.CopyTo(Array array, int index) => ((System.Collections.IList)_list).CopyTo(array, index);

      public void Add(string key, Image image) => _list.Add(image);
      public void SetKeyName(int index, string name) { }
      public bool ContainsKey(string key) => false;
      public int IndexOfKey(string key) => -1;
      public void RemoveByKey(string key) { }
    }

    public void Dispose() { }
  }

  public class ImageListStreamer : IDisposable {
    public void Dispose() { }
  }

  public enum ColorDepth {
    Depth4Bit = 4,
    Depth8Bit = 8,
    Depth16Bit = 16,
    Depth24Bit = 24,
    Depth32Bit = 32
  }
}
