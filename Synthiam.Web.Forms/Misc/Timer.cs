// =============================================================================
// Synthiam.Web.Forms - Timer for Blazor
// =============================================================================

using System;
using System.ComponentModel;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // Timer
  // ---------------------------------------------------------------------------
  public class Timer : IComponent, IDisposable {

    private bool _enabled;
    private int _interval = 100;
    private object _tag;
    private System.Threading.Timer _internalTimer;
    private bool _isDisposed;

    public Timer() { }

    public Timer(IContainer container) {
      container?.Add(this);
    }

    // IComponent
    public event EventHandler Disposed;
    public ISite Site { get; set; }

    public bool Enabled {
      get => _enabled;
      set {
        if (_enabled != value) {
          _enabled = value;
          if (_enabled)
            StartInternal();
          else
            StopInternal();
        }
      }
    }

    public int Interval {
      get => _interval;
      set {
        if (value < 1)
          throw new ArgumentOutOfRangeException(nameof(value), "Interval must be greater than zero.");
        _interval = value;
        if (_enabled) {
          StopInternal();
          StartInternal();
        }
      }
    }

    public object Tag {
      get => _tag;
      set => _tag = value;
    }

    public event EventHandler Tick;

    public void Start() {
      Enabled = true;
    }

    public void Stop() {
      Enabled = false;
    }

    private void StartInternal() {
      StopInternal();
      _internalTimer = new System.Threading.Timer(
        _ => OnTick(EventArgs.Empty),
        null,
        _interval,
        _interval);
    }

    private void StopInternal() {
      _internalTimer?.Dispose();
      _internalTimer = null;
    }

    protected virtual void OnTick(EventArgs e) {
      Tick?.Invoke(this, e);
    }

    public void Dispose() {
      if (!_isDisposed) {
        _isDisposed = true;
        StopInternal();
        Disposed?.Invoke(this, EventArgs.Empty);
      }
    }
  }
}
