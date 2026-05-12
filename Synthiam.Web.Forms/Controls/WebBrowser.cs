// =============================================================================
// Synthiam.Web.Forms - WebBrowser control for Blazor
// =============================================================================

using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  public enum WebBrowserReadyState {
    Uninitialized = 0,
    Loading = 1,
    Loaded = 2,
    Interactive = 3,
    Complete = 4
  }

  public class WebBrowser : Control {

    private Uri _url;
    private string _documentText = string.Empty;
    private bool _canGoBack;
    private bool _canGoForward;
    private bool _isBusy;
    private WebBrowserReadyState _readyState = WebBrowserReadyState.Uninitialized;
    private bool _scriptErrorsSuppressed;
    private bool _isWebBrowserContextMenuEnabled = true;
    private bool _allowNavigation = true;
    private bool _allowWebBrowserDrop = true;

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public Uri Url {
      get => _url;
      set {
        _url = value;
        NotifyStateChanged();
      }
    }

    public string DocumentText {
      get => _documentText;
      set {
        _documentText = value ?? string.Empty;
        NotifyStateChanged();
      }
    }

    public HtmlDocument Document => null;

    public bool CanGoBack {
      get => _canGoBack;
      set => _canGoBack = value;
    }

    public bool CanGoForward {
      get => _canGoForward;
      set => _canGoForward = value;
    }

    public bool IsBusy => _isBusy;

    public WebBrowserReadyState ReadyState => _readyState;

    public bool ScriptErrorsSuppressed {
      get => _scriptErrorsSuppressed;
      set => _scriptErrorsSuppressed = value;
    }

    public bool IsWebBrowserContextMenuEnabled {
      get => _isWebBrowserContextMenuEnabled;
      set => _isWebBrowserContextMenuEnabled = value;
    }

    public bool AllowNavigation {
      get => _allowNavigation;
      set => _allowNavigation = value;
    }

    public bool AllowWebBrowserDrop {
      get => _allowWebBrowserDrop;
      set => _allowWebBrowserDrop = value;
    }

    public string StatusText { get; set; } = string.Empty;
    public string DocumentTitle => string.Empty;
    public object ObjectForScripting { get; set; }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event WebBrowserNavigatingEventHandler Navigating;
    public event EventHandler Navigated;
    public event WebBrowserDocumentCompletedEventHandler DocumentCompleted;
    public event EventHandler ProgressChanged;
    public event EventHandler StatusTextChanged;

    protected virtual void OnNavigating(WebBrowserNavigatingEventArgs e) {
      Navigating?.Invoke(this, e);
    }

    protected virtual void OnNavigated(EventArgs e) {
      Navigated?.Invoke(this, e);
    }

    protected virtual void OnDocumentCompleted(WebBrowserDocumentCompletedEventArgs e) {
      DocumentCompleted?.Invoke(this, e);
    }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public void Navigate(string urlString) {
      if (Uri.TryCreate(urlString, UriKind.Absolute, out Uri uri))
        Url = uri;
    }

    public void Navigate(Uri url) {
      Url = url;
    }

    public void Navigate(string urlString, string targetFrameName, byte[] postData, string additionalHeaders) {
      Navigate(urlString);
    }

    public void GoBack() {
      // Stub: browser navigation requires JS interop
    }

    public void GoForward() {
      // Stub
    }

    public new void Refresh() {
      NotifyStateChanged();
    }

    public void Stop() {
      // Stub
    }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "iframe";

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "border:none;";
      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      if (_url != null)
        builder.AddAttribute(seq++, "src", _url.ToString());
      else if (!string.IsNullOrEmpty(_documentText))
        builder.AddAttribute(seq++, "srcdoc", _documentText);

      builder.AddAttribute(seq++, "sandbox", "allow-scripts allow-same-origin allow-forms");
    }
  }

  // Stub for WebBrowser.Document
  public class HtmlDocument {
    public string Body { get; set; }
    public string Title { get; set; }
  }
}
