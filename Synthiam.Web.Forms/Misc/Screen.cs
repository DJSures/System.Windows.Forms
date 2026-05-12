// =============================================================================
// Synthiam.Web.Forms - Screen and SystemInformation for Blazor
// =============================================================================

using System.Drawing;

namespace System.Windows.Forms {

  public class Screen {

    private static readonly Screen _primaryScreen = new Screen("\\\\. \\DISPLAY1", true, new Rectangle(0, 0, 1920, 1080), new Rectangle(0, 0, 1920, 1040));

    private Screen(string deviceName, bool primary, Rectangle bounds, Rectangle workingArea) {
      DeviceName = deviceName;
      Primary = primary;
      Bounds = bounds;
      WorkingArea = workingArea;
      BitsPerPixel = 32;
    }

    public Rectangle Bounds { get; }
    public Rectangle WorkingArea { get; }
    public string DeviceName { get; }
    public bool Primary { get; }
    public int BitsPerPixel { get; }

    public static Screen PrimaryScreen => _primaryScreen;

    public static Screen[] AllScreens => new[] { _primaryScreen };

    public static Screen FromControl(Control control) => PrimaryScreen;

    public static Screen FromPoint(Point point) => PrimaryScreen;

    public static Screen FromRectangle(Rectangle rect) => PrimaryScreen;

    public static Screen FromHandle(IntPtr hwnd) => PrimaryScreen;

    public static Rectangle GetWorkingArea(Control control) => PrimaryScreen.WorkingArea;
    public static Rectangle GetWorkingArea(Point point) => PrimaryScreen.WorkingArea;
    public static Rectangle GetWorkingArea(Rectangle rect) => PrimaryScreen.WorkingArea;

    public override string ToString() => $"Screen[Bounds={Bounds} WorkingArea={WorkingArea} Primary={Primary} DeviceName={DeviceName}]";
  }

  public static class SystemInformation {

    public static Size DoubleClickSize => new Size(4, 4);

    public static int DoubleClickTime => 500;

    public static int MouseWheelScrollDelta => 120;

    public static bool MouseWheelPresent => true;

    public static int VerticalScrollBarWidth => 17;

    public static int HorizontalScrollBarHeight => 17;

    public static Size SmallIconSize => new Size(16, 16);

    public static int MenuHeight => 20;

    public static int CaptionHeight => 23;

    public static int BorderSize => 1;

    public static Size Border3DSize => new Size(2, 2);

    public static Size CaptionButtonSize => new Size(26, 22);

    public static int ToolWindowCaptionHeight => 19;

    public static bool TerminalServerSession => false;

    public static int MouseWheelScrollLines => 3;

    public static bool DragFullWindows => true;

    public static Size DragSize => new Size(4, 4);

    public static Size MinimumWindowSize => new Size(112, 27);

    public static Size MinWindowTrackSize => new Size(112, 27);

    public static bool HighContrast => false;

    public static int VerticalScrollBarArrowHeight => 17;

    public static int HorizontalScrollBarArrowWidth => 17;

    public static Size WorkingArea => new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);

    public static Size PrimaryMonitorSize => new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

    public static Size FrameBorderSize => new Size(4, 4);

    public static int MenuAccessKeysUnderlined => 0;
  }
}
