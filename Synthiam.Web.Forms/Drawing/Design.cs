// =============================================================================
// Synthiam.Web.Forms - System.Drawing.Design stubs for Blazor
// =============================================================================

namespace System.Drawing.Design {

  // ---------------------------------------------------------------------------
  // UITypeEditorEditStyle
  // ---------------------------------------------------------------------------
  public enum UITypeEditorEditStyle {
    None = 1,
    Modal = 2,
    DropDown = 3
  }

  // ---------------------------------------------------------------------------
  // UITypeEditor
  // ---------------------------------------------------------------------------
  public class UITypeEditor {

    public UITypeEditor() {
    }

    public virtual UITypeEditorEditStyle GetEditStyle() {
      return UITypeEditorEditStyle.None;
    }

    public virtual UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context) {
      return GetEditStyle();
    }

    public virtual object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value) {
      return value;
    }

    public virtual bool GetPaintValueSupported() {
      return false;
    }

    public virtual bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context) {
      return GetPaintValueSupported();
    }

    public virtual bool IsDropDownResizable => false;

    public virtual void PaintValue(PaintValueEventArgs e) { }
  }
}
