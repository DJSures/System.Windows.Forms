// =============================================================================
// Synthiam.Web.Forms - Cursor and Cursors for Blazor
// =============================================================================

using System.Drawing;

namespace System.Windows.Forms {

  public class Cursor : IDisposable {

    private readonly string _cursorType;

    internal Cursor() {
      _cursorType = "default";
    }

    internal Cursor(string cursorType) {
      _cursorType = cursorType;
    }

    public Cursor(System.IO.Stream stream) {
      _cursorType = "default";
    }

    public Cursor(Type type, string resource) {
      _cursorType = "default";
    }

    public static Point Position { get; set; } = new Point(0, 0);

    public static void Hide() {
    }

    public static void Show() {
    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
    }

    /// <summary>
    /// Returns the CSS cursor name for use in Blazor rendering.
    /// </summary>
    public string ToCss() => _cursorType;

    public override string ToString() => $"[Cursor: {_cursorType}]";
  }

  public static class Cursors {

    public static Cursor Default { get; } = new Cursor("default");
    public static Cursor Arrow { get; } = new Cursor("default");
    public static Cursor Cross { get; } = new Cursor("crosshair");
    public static Cursor Hand { get; } = new Cursor("pointer");
    public static Cursor Help { get; } = new Cursor("help");
    public static Cursor HSplit { get; } = new Cursor("row-resize");
    public static Cursor VSplit { get; } = new Cursor("col-resize");
    public static Cursor IBeam { get; } = new Cursor("text");
    public static Cursor No { get; } = new Cursor("not-allowed");
    public static Cursor NoMove2D { get; } = new Cursor("move");
    public static Cursor NoMoveHoriz { get; } = new Cursor("ew-resize");
    public static Cursor NoMoveVert { get; } = new Cursor("ns-resize");
    public static Cursor SizeAll { get; } = new Cursor("move");
    public static Cursor SizeNESW { get; } = new Cursor("nesw-resize");
    public static Cursor SizeNS { get; } = new Cursor("ns-resize");
    public static Cursor SizeNWSE { get; } = new Cursor("nwse-resize");
    public static Cursor SizeWE { get; } = new Cursor("ew-resize");
    public static Cursor WaitCursor { get; } = new Cursor("wait");
    public static Cursor AppStarting { get; } = new Cursor("progress");
    public static Cursor UpArrow { get; } = new Cursor("default");
    public static Cursor PanNorth { get; } = new Cursor("n-resize");
    public static Cursor PanSouth { get; } = new Cursor("s-resize");
    public static Cursor PanEast { get; } = new Cursor("e-resize");
    public static Cursor PanWest { get; } = new Cursor("w-resize");
    public static Cursor PanNE { get; } = new Cursor("ne-resize");
    public static Cursor PanNW { get; } = new Cursor("nw-resize");
    public static Cursor PanSE { get; } = new Cursor("se-resize");
    public static Cursor PanSW { get; } = new Cursor("sw-resize");
  }
}
