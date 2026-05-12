// =============================================================================
// Synthiam.Web.Forms - MessageBox for Blazor
// =============================================================================

using System;
using System.Collections.Generic;

namespace System.Windows.Forms {

  /// <summary>
  /// Represents a pending message box to be rendered by the FormRenderer.
  /// </summary>
  public class MessageBoxRequest {

    public string Text { get; set; }
    public string Caption { get; set; }
    public MessageBoxButtons Buttons { get; set; }
    public MessageBoxIcon Icon { get; set; }
    public DialogResult Result { get; set; } = DialogResult.None;
    public bool Completed { get; set; }

    public void Complete(DialogResult result) {
      Result = result;
      Completed = true;
    }
  }

  public static class MessageBox {

    /// <summary>
    /// Global queue of message boxes to display. The FormRenderer renders these as modal overlays.
    /// </summary>
    internal static readonly List<MessageBoxRequest> _pendingMessages = new List<MessageBoxRequest>();

    /// <summary>
    /// Callback to notify the FormRenderer to re-render when a message box is shown.
    /// Set by FormRenderer.
    /// </summary>
    internal static Action _notifyRenderer;

    private static DialogResult ShowCore(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon) {
      var request = new MessageBoxRequest {
        Text = text,
        Caption = caption ?? string.Empty,
        Buttons = buttons,
        Icon = icon
      };
      _pendingMessages.Add(request);
      _notifyRenderer?.Invoke();
      // Can't block in Blazor - return OK for compatibility
      return DialogResult.OK;
    }

    public static DialogResult Show(string text) {
      return ShowCore(text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    public static DialogResult Show(string text, string caption) {
      return ShowCore(text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons) {
      return ShowCore(text, caption, buttons, MessageBoxIcon.None);
    }

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon) {
      return ShowCore(text, caption, buttons, icon);
    }

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton) {
      return ShowCore(text, caption, buttons, icon);
    }

    public static DialogResult Show(IWin32Window owner, string text) {
      return ShowCore(text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption) {
      return ShowCore(text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons) {
      return ShowCore(text, caption, buttons, MessageBoxIcon.None);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon) {
      return ShowCore(text, caption, buttons, icon);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton) {
      return ShowCore(text, caption, buttons, icon);
    }
  }
}
