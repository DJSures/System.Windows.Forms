// =============================================================================
// Synthiam.Web.Forms - Padding struct for Blazor
// =============================================================================

using System.Drawing;

namespace System.Windows.Forms {

  public struct Padding : IEquatable<Padding> {

    public static readonly Padding Empty = new Padding(0);

    public Padding(int all) {
      Left = all;
      Top = all;
      Right = all;
      Bottom = all;
    }

    public Padding(int left, int top, int right, int bottom) {
      Left = left;
      Top = top;
      Right = right;
      Bottom = bottom;
    }

    public int Left { get; set; }
    public int Top { get; set; }
    public int Right { get; set; }
    public int Bottom { get; set; }

    public int All {
      get => (Left == Top && Left == Right && Left == Bottom) ? Left : -1;
      set {
        Left = value;
        Top = value;
        Right = value;
        Bottom = value;
      }
    }

    public int Horizontal => Left + Right;

    public int Vertical => Top + Bottom;

    public Size Size => new Size(Horizontal, Vertical);

    /// <summary>
    /// Returns a CSS padding string in the format "Tpx Rpx Bpx Lpx".
    /// </summary>
    public string ToCss() => $"{Top}px {Right}px {Bottom}px {Left}px";

    public bool Equals(Padding other) =>
      Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;

    public override bool Equals(object obj) =>
      obj is Padding other && Equals(other);

    public override int GetHashCode() =>
      HashCode.Combine(Left, Top, Right, Bottom);

    public static bool operator ==(Padding left, Padding right) => left.Equals(right);

    public static bool operator !=(Padding left, Padding right) => !left.Equals(right);

    public static Padding operator +(Padding p1, Padding p2) =>
      new Padding(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);

    public static Padding operator -(Padding p1, Padding p2) =>
      new Padding(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);

    public override string ToString() => $"{{Left={Left},Top={Top},Right={Right},Bottom={Bottom}}}";

    public static implicit operator Padding(Point point) => new Padding(point.X, point.Y, point.X, point.Y);
  }
}
