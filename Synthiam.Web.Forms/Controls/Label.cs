// =============================================================================
// Synthiam.Web.Forms - Label control for Blazor
// =============================================================================

using System;
using System.Drawing;
using Microsoft.AspNetCore.Components.Rendering;
using Synthiam.Web.Forms;

namespace System.Windows.Forms {

  public class Label : Control {

    private ContentAlignment _textAlign = ContentAlignment.TopLeft;
    private bool _autoEllipsis;
    private BorderStyle _borderStyle = BorderStyle.None;
    private FlatStyle _flatStyle = FlatStyle.Standard;
    private Image _image;
    private ContentAlignment _imageAlign = ContentAlignment.MiddleCenter;
    private bool _useMnemonic = true;
    private int _imageIndex = -1;
    private string _imageKey = string.Empty;
    private ImageList _imageList;

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public ContentAlignment TextAlign {
      get => _textAlign;
      set { _textAlign = value; InvalidateCssStyle(); NotifyStateChanged(); }
    }

    public new bool AutoSize {
      get => base.AutoSize;
      set => base.AutoSize = value;
    }

    public bool AutoEllipsis {
      get => _autoEllipsis;
      set { _autoEllipsis = value; InvalidateCssStyle(); NotifyStateChanged(); }
    }

    public BorderStyle BorderStyle {
      get => _borderStyle;
      set { _borderStyle = value; InvalidateCssStyle(); InvalidateCssClasses(); NotifyStateChanged(); }
    }

    public FlatStyle FlatStyle {
      get => _flatStyle;
      set { _flatStyle = value; NotifyStateChanged(); }
    }

    public Image Image {
      get => _image;
      set { _image = value; NotifyStateChanged(); }
    }

    public ContentAlignment ImageAlign {
      get => _imageAlign;
      set { _imageAlign = value; NotifyStateChanged(); }
    }

    public bool UseMnemonic {
      get => _useMnemonic;
      set => _useMnemonic = value;
    }

    public int ImageIndex {
      get => _imageIndex;
      set => _imageIndex = value;
    }

    public string ImageKey {
      get => _imageKey;
      set => _imageKey = value ?? string.Empty;
    }

    public ImageList ImageList {
      get => _imageList;
      set => _imageList = value;
    }

    public int PreferredWidth => Size.Width;
    public int PreferredHeight => Size.Height;

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "div";

    protected override string GetCssClasses() {
      var css = base.GetCssClasses();
      if (_borderStyle == BorderStyle.FixedSingle) css += " swf-label-border";
      else if (_borderStyle == BorderStyle.Fixed3D) css += " swf-label-border3d";
      return css;
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();

      // Text alignment
      style += Button.ContentAlignmentToCss(_textAlign);

      // Auto ellipsis
      if (_autoEllipsis)
        style += "text-overflow:ellipsis;overflow:hidden;white-space:nowrap;";

      // Border
      if (_borderStyle == BorderStyle.FixedSingle)
        style += "border:1px solid #808080;";
      else if (_borderStyle == BorderStyle.Fixed3D)
        style += "border:2px inset;";

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
        builder.AddContent(seq++, Text);
      }
    }
  }
}
