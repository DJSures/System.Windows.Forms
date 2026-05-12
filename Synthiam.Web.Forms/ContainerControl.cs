// =============================================================================
// Synthiam.Web.Forms - ScrollableControl and ContainerControl for Blazor
// =============================================================================

using System;
using System.Drawing;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // ScrollableControl
  // ---------------------------------------------------------------------------
  public class ScrollableControl : Control {

    private bool _autoScroll;
    private Size _autoScrollMinSize;
    private Point _autoScrollPosition;
    private Size _autoScrollMargin;

    public virtual bool AutoScroll {
      get => _autoScroll;
      set {
        _autoScroll = value;
        NotifyStateChanged();
      }
    }

    public Size AutoScrollMinSize {
      get => _autoScrollMinSize;
      set => _autoScrollMinSize = value;
    }

    public Point AutoScrollPosition {
      get => _autoScrollPosition;
      set => _autoScrollPosition = value;
    }

    public Size AutoScrollMargin {
      get => _autoScrollMargin;
      set => _autoScrollMargin = value;
    }

    public class DockPaddingEdges {
      public int All { get; set; }
      public int Left { get; set; }
      public int Right { get; set; }
      public int Top { get; set; }
      public int Bottom { get; set; }
    }

    // Scroll properties
    public ScrollProperties VerticalScroll { get; } = new VScrollProperties();
    public ScrollProperties HorizontalScroll { get; } = new HScrollProperties();

    public void ScrollControlIntoView(Control activeControl) {
      // Stub: no-op in Blazor (browser handles scroll)
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      if (_autoScroll)
        style += "overflow:auto;";
      return style;
    }
  }

  // ---------------------------------------------------------------------------
  // ContainerControl
  // ---------------------------------------------------------------------------
  public class ContainerControl : ScrollableControl {

    private Control _activeControl;

    public Control ActiveControl {
      get => _activeControl;
      set => _activeControl = value;
    }

    public SizeF CurrentAutoScaleDimensions => AutoScaleDimensions;

    public override void Scale(SizeF factor) {
      base.Scale(factor);
    }

    public bool Validate() { return true; }

    public bool Validate(bool checkAutoValidate) { return true; }
  }

  // ---------------------------------------------------------------------------
  // ScrollProperties (base class for VScrollProperties / HScrollProperties)
  // ---------------------------------------------------------------------------
  public abstract class ScrollProperties {
    public bool Enabled { get; set; } = true;
    public bool Visible { get; set; }
    public int Value { get; set; }
    public int Minimum { get; set; }
    public int Maximum { get; set; } = 100;
    public int SmallChange { get; set; } = 1;
    public int LargeChange { get; set; } = 10;
  }

  public class VScrollProperties : ScrollProperties { }
  public class HScrollProperties : ScrollProperties { }
}
