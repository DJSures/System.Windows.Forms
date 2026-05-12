// =============================================================================
// Synthiam.Web.Forms - Drawing Extension Methods
// Web-specific helper methods for System.Drawing types (ToCss, ToDataUri).
// =============================================================================

using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Synthiam.Web.Forms {

  /// <summary>
  /// Extension methods that add web-specific functionality (CSS, data URIs)
  /// to real System.Drawing types.
  /// </summary>
  public static class DrawingExtensions {

    // ──────────────────────────────────────────────
    // Color extensions
    // ──────────────────────────────────────────────

    /// <summary>
    /// Returns a CSS color string: rgb(r,g,b) or rgba(r,g,b,a).
    /// </summary>
    public static string ToCss(this Color c) {
      if (c.A == 255)
        return string.Format("rgb({0},{1},{2})", c.R, c.G, c.B);
      double alpha = Math.Round(c.A / 255.0, 3);
      return string.Format("rgba({0},{1},{2},{3})", c.R, c.G, c.B, alpha);
    }

    // ──────────────────────────────────────────────
    // Font extensions
    // ──────────────────────────────────────────────

    /// <summary>
    /// Returns a CSS font shorthand string, e.g. "italic bold 12pt 'Segoe UI'".
    /// Appends text-decoration if Underline or Strikeout is set.
    /// </summary>
    public static string ToCss(this Font font) {
      string style = "";
      if (font.Italic) style += "italic ";
      if (font.Bold) style += "bold ";

      float sizePt = font.SizeInPoints;
      string sizeStr = sizePt.ToString("0.##") + "pt";

      string css = string.Format("{0}{1} {2}", style, sizeStr, "'" + font.Name + "'");

      string extra = "";
      if (font.Underline && font.Strikeout) extra = "text-decoration:underline line-through";
      else if (font.Underline) extra = "text-decoration:underline";
      else if (font.Strikeout) extra = "text-decoration:line-through";

      if (extra.Length > 0) css += ";" + extra;

      return css;
    }

    // ──────────────────────────────────────────────
    // Image extensions
    // ──────────────────────────────────────────────

    /// <summary>
    /// Returns a data URI string (data:mime;base64,...) for the image.
    /// </summary>
    public static string ToDataUri(this Image image) {
      if (image == null)
        return "data:image/png;base64,";

      using (var ms = new MemoryStream()) {
        try {
          image.Save(ms, image.RawFormat);
        } catch {
          // If the raw format fails, fall back to PNG
          try {
            ms.SetLength(0);
            image.Save(ms, ImageFormat.Png);
          } catch {
            return "data:image/png;base64,";
          }
        }

        byte[] data = ms.ToArray();
        if (data.Length == 0)
          return "data:image/png;base64,";

        string mimeType = GetMimeType(image.RawFormat);
        return "data:" + mimeType + ";base64," + Convert.ToBase64String(data);
      }
    }

    private static string GetMimeType(ImageFormat format) {
      if (format == null) return "image/png";
      if (format.Guid == ImageFormat.Jpeg.Guid) return "image/jpeg";
      if (format.Guid == ImageFormat.Gif.Guid) return "image/gif";
      if (format.Guid == ImageFormat.Bmp.Guid) return "image/bmp";
      if (format.Guid == ImageFormat.Tiff.Guid) return "image/tiff";
      if (format.Guid == ImageFormat.Icon.Guid) return "image/x-icon";
      return "image/png";
    }
  }
}
