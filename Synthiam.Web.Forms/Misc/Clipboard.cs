// =============================================================================
// Synthiam.Web.Forms - Clipboard, IDataObject, DataObject, DataFormats for Blazor
// =============================================================================

using System.Drawing;

namespace System.Windows.Forms {

  public static class Clipboard {

    public static void SetText(string text) {
    }

    public static string GetText() => string.Empty;

    public static string GetText(TextDataFormat format) => string.Empty;

    public static void SetText(string text, TextDataFormat format) {
    }

    public static bool ContainsText() => false;

    public static bool ContainsData(string format) => false;

    public static void SetDataObject(object data) {
    }

    public static void SetDataObject(object data, bool copy) {
    }

    public static IDataObject GetDataObject() => null;

    public static void SetImage(Image image) {
    }

    public static Image GetImage() => null;

    public static bool ContainsImage() => false;

    public static void Clear() {
    }
  }

  public interface IDataObject {

    object GetData(string format);

    object GetData(Type format);

    object GetData(string format, bool autoConvert);

    bool GetDataPresent(string format);

    bool GetDataPresent(Type format);

    bool GetDataPresent(string format, bool autoConvert);

    string[] GetFormats();

    string[] GetFormats(bool autoConvert);

    void SetData(object data);

    void SetData(string format, object data);

    void SetData(Type format, object data);

    void SetData(string format, bool autoConvert, object data);
  }

  public class DataObject : IDataObject {

    private readonly System.Collections.Generic.Dictionary<string, object> _data = new System.Collections.Generic.Dictionary<string, object>();

    public DataObject() {
    }

    public DataObject(object data) {
      if (data != null) {
        _data[data.GetType().FullName] = data;
      }
    }

    public DataObject(string format, object data) {
      if (format != null) {
        _data[format] = data;
      }
    }

    public object GetData(string format) {
      _data.TryGetValue(format, out var value);
      return value;
    }

    public object GetData(Type format) {
      return format != null ? GetData(format.FullName) : null;
    }

    public object GetData(string format, bool autoConvert) {
      return GetData(format);
    }

    public bool GetDataPresent(string format) {
      return _data.ContainsKey(format);
    }

    public bool GetDataPresent(Type format) {
      return format != null && GetDataPresent(format.FullName);
    }

    public bool GetDataPresent(string format, bool autoConvert) {
      return GetDataPresent(format);
    }

    public string[] GetFormats() {
      var keys = new string[_data.Count];
      _data.Keys.CopyTo(keys, 0);
      return keys;
    }

    public string[] GetFormats(bool autoConvert) {
      return GetFormats();
    }

    public void SetData(object data) {
      if (data != null) {
        _data[data.GetType().FullName] = data;
      }
    }

    public void SetData(string format, object data) {
      if (format != null) {
        _data[format] = data;
      }
    }

    public void SetData(Type format, object data) {
      if (format != null) {
        _data[format.FullName] = data;
      }
    }

    public void SetData(string format, bool autoConvert, object data) {
      SetData(format, data);
    }
  }

  public enum TextDataFormat {
    Text = 0,
    UnicodeText = 1,
    Rtf = 2,
    Html = 3,
    CommaSeparatedValue = 4
  }

  public static class DataFormats {

    public static readonly string Text = "Text";
    public static readonly string UnicodeText = "UnicodeText";
    public static readonly string Rtf = "Rich Text Format";
    public static readonly string Html = "HTML Format";
    public static readonly string CommaSeparatedValue = "Csv";
    public static readonly string FileDrop = "FileDrop";
    public static readonly string Bitmap = "Bitmap";
    public static readonly string Serializable = "WindowsForms10PersistentObject";
    public static readonly string StringFormat = "System.String";
  }
}
