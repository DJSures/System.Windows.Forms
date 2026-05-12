// =============================================================================
// Synthiam.Web.Forms - WPF Media stubs for Blazor
// These stubs replace System.Windows.Media.Imaging types used by EZ-B
// for AR Drone video and MJPEG streaming.
// =============================================================================

using System;
using System.IO;

namespace System.Windows.Media {

  /// <summary>Stub for System.Windows.Media.Brushes (WPF brushes).</summary>
  public static class Brushes {
    public static System.Drawing.Brush Transparent => new System.Drawing.SolidBrush(System.Drawing.Color.Transparent);
    public static System.Drawing.Brush Black => new System.Drawing.SolidBrush(System.Drawing.Color.Black);
    public static System.Drawing.Brush White => new System.Drawing.SolidBrush(System.Drawing.Color.White);
    public static System.Drawing.Brush Red => new System.Drawing.SolidBrush(System.Drawing.Color.Red);
    public static System.Drawing.Brush Green => new System.Drawing.SolidBrush(System.Drawing.Color.Green);
    public static System.Drawing.Brush Blue => new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
    public static System.Drawing.Brush Yellow => new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
    public static System.Drawing.Brush Gray => new System.Drawing.SolidBrush(System.Drawing.Color.Gray);
    public static System.Drawing.Brush LightGray => new System.Drawing.SolidBrush(System.Drawing.Color.LightGray);
    public static System.Drawing.Brush LightBlue => new System.Drawing.SolidBrush(System.Drawing.Color.LightBlue);
    public static System.Drawing.Brush Orange => new System.Drawing.SolidBrush(System.Drawing.Color.Orange);
    public static System.Drawing.Brush DarkGray => new System.Drawing.SolidBrush(System.Drawing.Color.DarkGray);
  }
}

namespace System.Windows.Media.Imaging {

  public class BitmapImage {

    public BitmapImage() { }

    public BitmapImage(Uri uri) {
      UriSource = uri;
    }

    public Uri UriSource { get; set; }
    public int PixelWidth { get; set; }
    public int PixelHeight { get; set; }
    public BitmapCacheOption CacheOption { get; set; }
    public Stream StreamSource { get; set; }

    public void BeginInit() { }
    public void EndInit() { }
    public void Freeze() { }
  }

  public class WriteableBitmap {

    public WriteableBitmap(int pixelWidth, int pixelHeight, double dpiX, double dpiY, PixelFormat format, object palette) {
      PixelWidth = pixelWidth;
      PixelHeight = pixelHeight;
    }

    public int PixelWidth { get; }
    public int PixelHeight { get; }
    public int Width => PixelWidth;
    public int Height => PixelHeight;
    public IntPtr BackBuffer { get; } = IntPtr.Zero;
    public int BackBufferStride { get; }

    public void Lock() { }
    public void Unlock() { }
    public void AddDirtyRect(Int32Rect dirtyRect) { }
    public void WritePixels(Int32Rect sourceRect, IntPtr buffer, int bufferSize, int stride) { }
    public void WritePixels(Int32Rect sourceRect, byte[] pixels, int stride, int offset) { }
    public object GetAsFrozen() { return this; }
  }

  public struct Int32Rect {

    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Int32Rect(int x, int y, int width, int height) {
      X = x; Y = y; Width = width; Height = height;
    }
  }

  public class PixelFormat {
    public int BitsPerPixel { get; set; } = 32;
  }

  public static class PixelFormats {
    public static PixelFormat Bgr32 { get; } = new PixelFormat { BitsPerPixel = 32 };
    public static PixelFormat Bgra32 { get; } = new PixelFormat { BitsPerPixel = 32 };
    public static PixelFormat Rgb24 { get; } = new PixelFormat { BitsPerPixel = 24 };
    public static PixelFormat Pbgra32 { get; } = new PixelFormat { BitsPerPixel = 32 };
    public static PixelFormat Bgr565 { get; } = new PixelFormat { BitsPerPixel = 16 };
  }

  public enum BitmapCacheOption {
    Default = 0,
    OnDemand = 0,
    OnLoad = 1,
    None = 2
  }
}
