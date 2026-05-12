// =============================================================================
// Synthiam.Web.Forms - PictureBox control for Blazor
// =============================================================================

using System;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Synthiam.Web.Forms;

namespace System.Windows.Forms {

  public class PictureBox : Control {

    private Image _image;
    private string _imageLocation = string.Empty;
    private PictureBoxSizeMode _sizeMode = PictureBoxSizeMode.Normal;
    private BorderStyle _borderStyle = BorderStyle.None;
    private Image _errorImage;
    private Image _initialImage;
    private bool _waitOnLoad;

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public Image Image {
      get => _image;
      set { _image = value; NotifyStateChanged(); }
    }

    public string ImageLocation {
      get => _imageLocation;
      set {
        _imageLocation = value ?? string.Empty;
        NotifyStateChanged();
      }
    }

    public PictureBoxSizeMode SizeMode {
      get => _sizeMode;
      set { _sizeMode = value; OnSizeModeChanged(EventArgs.Empty); NotifyStateChanged(); }
    }

    public BorderStyle BorderStyle {
      get => _borderStyle;
      set { _borderStyle = value; InvalidateCssStyle(); NotifyStateChanged(); }
    }

    public Image ErrorImage {
      get => _errorImage;
      set => _errorImage = value;
    }

    public Image InitialImage {
      get => _initialImage;
      set => _initialImage = value;
    }

    public bool WaitOnLoad {
      get => _waitOnLoad;
      set => _waitOnLoad = value;
    }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event EventHandler LoadCompleted;
    public event EventHandler LoadProgressChanged;
    public event EventHandler SizeModeChanged;

    protected virtual void OnLoadCompleted(EventArgs e) {
      LoadCompleted?.Invoke(this, e);
    }

    protected virtual void OnSizeModeChanged(EventArgs e) {
      SizeModeChanged?.Invoke(this, e);
    }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public void Load() {
      // Stub: browser handles image loading
    }

    public void Load(string url) {
      _imageLocation = url ?? string.Empty;
      NotifyStateChanged();
    }

    public void LoadAsync() {
      // Stub
    }

    public void LoadAsync(string url) {
      _imageLocation = url ?? string.Empty;
      NotifyStateChanged();
    }

    public void CancelAsync() {
      // Stub
    }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "div";

    protected override string GetCssClasses() {
      var css = base.GetCssClasses();
      return css;
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();

      if (_borderStyle == BorderStyle.FixedSingle)
        style += "border:1px solid #808080;";
      else if (_borderStyle == BorderStyle.Fixed3D)
        style += "border:2px inset;";

      style += "overflow:hidden;";
      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      // Determine image source
      string src = null;
      if (_image != null) {
        src = _image.ToDataUri();
      } else if (!string.IsNullOrEmpty(_imageLocation)) {
        src = _imageLocation;
      }

      if (!string.IsNullOrEmpty(src)) {
        builder.OpenElement(seq++, "img");
        builder.AddAttribute(seq++, "src", src);
        builder.AddAttribute(seq++, "alt", Text ?? string.Empty);

        // Map SizeMode to CSS object-fit
        string imgStyle;
        switch (_sizeMode) {
          case PictureBoxSizeMode.StretchImage:
            imgStyle = "width:100%;height:100%;object-fit:fill;";
            break;
          case PictureBoxSizeMode.Zoom:
            imgStyle = "width:100%;height:100%;object-fit:contain;";
            break;
          case PictureBoxSizeMode.CenterImage:
            imgStyle = "position:absolute;top:50%;left:50%;transform:translate(-50%,-50%);";
            break;
          case PictureBoxSizeMode.AutoSize:
            imgStyle = "width:auto;height:auto;";
            break;
          default: // Normal
            imgStyle = "width:auto;height:auto;";
            break;
        }
        builder.AddAttribute(seq++, "style", imgStyle + "display:block;pointer-events:none;");
        builder.CloseElement(); // img
      }
    }
  }
}
