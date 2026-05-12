// =============================================================================
// Synthiam.Web.Forms - Control.ControlCollection for Blazor
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Windows.Forms {

  partial class Control {

    public class ControlCollection : IList, ICollection, IEnumerable, IEnumerable<Control> {

      private readonly List<Control> _controls = new List<Control>();
      private readonly Control _owner;

      public ControlCollection(Control owner) {
        _owner = owner ?? throw new ArgumentNullException(nameof(owner));
      }

      // ═══════════════════════════════════════════════
      // Core operations
      // ═══════════════════════════════════════════════

      public virtual void Add(Control value) {
        if (value == null) return;
        if (value == _owner) throw new ArgumentException("Cannot add a control to itself.");
        if (_controls.Contains(value)) return;

        // Remove from previous parent
        if (value._parent != null)
          value._parent.Controls.Remove(value);

        _controls.Add(value);
        value._parent = _owner;
        value.OnParentChanged(EventArgs.Empty);
        _owner.OnControlAdded(new ControlEventArgs(value));
        _owner.NotifyStateChanged();

      }

      /// <summary>
      /// Adds a control to the collection at the specified cell position (for TableLayoutPanel compatibility).
      /// </summary>
      public virtual void Add(Control value, int column, int row) {
        Add(value);
        // If owner is a TableLayoutPanel, set the cell position
        if (_owner is TableLayoutPanel tlp) {
          tlp.SetColumn(value, column);
          tlp.SetRow(value, row);
        }
      }

      public virtual void AddRange(Control[] controls) {
        if (controls == null) return;
        _owner.SuspendLayout();
        try {
          for (int i = 0; i < controls.Length; i++)
            Add(controls[i]);
        } finally {
          _owner.ResumeLayout(true);
        }
      }

      public virtual void Remove(Control value) {
        if (value == null) return;
        if (!_controls.Contains(value)) return;

        _controls.Remove(value);
        value._parent = null;
        value.OnParentChanged(EventArgs.Empty);
        _owner.OnControlRemoved(new ControlEventArgs(value));
        _owner.NotifyStateChanged();
      }

      public virtual void RemoveAt(int index) {
        if (index < 0 || index >= _controls.Count) throw new ArgumentOutOfRangeException(nameof(index));
        Remove(_controls[index]);
      }

      public virtual void Clear() {
        _owner.SuspendLayout();
        try {
          while (_controls.Count > 0)
            Remove(_controls[_controls.Count - 1]);
        } finally {
          _owner.ResumeLayout(true);
        }
      }

      // ═══════════════════════════════════════════════
      // Query / search
      // ═══════════════════════════════════════════════

      public bool Contains(Control control) => _controls.Contains(control);

      public int Count => _controls.Count;

      public Control this[int index] {
        get {
          if (index < 0 || index >= _controls.Count) throw new ArgumentOutOfRangeException(nameof(index));
          return _controls[index];
        }
      }

      public Control this[string key] {
        get {
          if (key == null) return null;
          for (int i = 0; i < _controls.Count; i++) {
            if (string.Equals(_controls[i].Name, key, StringComparison.OrdinalIgnoreCase))
              return _controls[i];
          }
          return null;
        }
      }

      public int IndexOf(Control control) => _controls.IndexOf(control);

      public bool ContainsKey(string key) {
        if (key == null) return false;
        for (int i = 0; i < _controls.Count; i++) {
          if (string.Equals(_controls[i].Name, key, StringComparison.OrdinalIgnoreCase))
            return true;
        }
        return false;
      }

      public Control[] Find(string key, bool searchAllChildren) {
        if (key == null) throw new ArgumentNullException(nameof(key));
        var results = new List<Control>();
        FindInternal(key, searchAllChildren, _controls, results);
        return results.ToArray();
      }

      private static void FindInternal(string key, bool searchAllChildren, List<Control> controls, List<Control> results) {
        for (int i = 0; i < controls.Count; i++) {
          if (string.Equals(controls[i].Name, key, StringComparison.OrdinalIgnoreCase))
            results.Add(controls[i]);
          if (searchAllChildren && controls[i].Controls.Count > 0)
            FindInternal(key, searchAllChildren, controls[i].Controls._controls, results);
        }
      }

      // ═══════════════════════════════════════════════
      // Ordering
      // ═══════════════════════════════════════════════

      public void SetChildIndex(Control child, int newIndex) {
        if (child == null) throw new ArgumentNullException(nameof(child));
        int current = _controls.IndexOf(child);
        if (current < 0) throw new ArgumentException("Control is not a child of this collection.");
        if (current == newIndex) return;
        _controls.RemoveAt(current);
        if (newIndex >= _controls.Count)
          _controls.Add(child);
        else
          _controls.Insert(newIndex, child);
        _owner.NotifyStateChanged();
      }

      public int GetChildIndex(Control child) {
        int index = _controls.IndexOf(child);
        if (index < 0) throw new ArgumentException("Control is not a child of this collection.");
        return index;
      }

      public Control Owner => _owner;

      internal List<Control> GetAll() => _controls;

      // ═══════════════════════════════════════════════
      // IEnumerable / IEnumerable<Control>
      // ═══════════════════════════════════════════════

      public IEnumerator<Control> GetEnumerator() => _controls.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => _controls.GetEnumerator();

      // ═══════════════════════════════════════════════
      // IList explicit implementation
      // ═══════════════════════════════════════════════

      object IList.this[int index] {
        get => _controls[index];
        set { /* not supported */ }
      }

      bool IList.IsReadOnly => false;
      bool IList.IsFixedSize => false;

      int IList.Add(object value) {
        if (value is Control c) {
          Add(c);
          return _controls.IndexOf(c);
        }
        return -1;
      }

      bool IList.Contains(object value) => value is Control c && _controls.Contains(c);

      int IList.IndexOf(object value) => value is Control c ? _controls.IndexOf(c) : -1;

      void IList.Insert(int index, object value) {
        if (value is Control c) {
          if (c._parent != null)
            c._parent.Controls.Remove(c);
          _controls.Insert(index, c);
          c._parent = _owner;
          c.OnParentChanged(EventArgs.Empty);
          _owner.OnControlAdded(new ControlEventArgs(c));
          _owner.NotifyStateChanged();
        }
      }

      void IList.Remove(object value) {
        if (value is Control c) Remove(c);
      }

      void IList.RemoveAt(int index) => RemoveAt(index);

      void IList.Clear() => Clear();

      // ═══════════════════════════════════════════════
      // ICollection explicit implementation
      // ═══════════════════════════════════════════════

      bool ICollection.IsSynchronized => false;
      object ICollection.SyncRoot => _controls;

      void ICollection.CopyTo(Array array, int index) {
        for (int i = 0; i < _controls.Count; i++)
          array.SetValue(_controls[i], index + i);
      }
    }
  }
}
