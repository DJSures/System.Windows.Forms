// =============================================================================
// Synthiam.Web.Forms - CheckedListBox control for Blazor
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Windows.Forms {

  public class CheckedListBox : ListBox {

    private readonly CheckedItemCollection _checkedItems;
    private readonly CheckedIndexCollection _checkedIndices;
    private readonly HashSet<int> _checkedSet = new HashSet<int>();
    private bool _checkOnClick;
    private bool _threeDCheckBoxes;

    public CheckedListBox() {
      _checkedItems = new CheckedItemCollection(this);
      _checkedIndices = new CheckedIndexCollection(this);
    }

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public CheckedItemCollection CheckedItems => _checkedItems;
    public CheckedIndexCollection CheckedIndices => _checkedIndices;

    public bool CheckOnClick {
      get => _checkOnClick;
      set => _checkOnClick = value;
    }

    public bool ThreeDCheckBoxes {
      get => _threeDCheckBoxes;
      set { _threeDCheckBoxes = value; NotifyStateChanged(); }
    }

    public new bool UseCompatibleTextRendering { get; set; }

    // ═══════════════════════════════════════════════
    // Methods
    // ═══════════════════════════════════════════════

    public bool GetItemChecked(int index) {
      return _checkedSet.Contains(index);
    }

    public void SetItemChecked(int index, bool value) {
      SetItemCheckState(index, value ? CheckState.Checked : CheckState.Unchecked);
    }

    public CheckState GetItemCheckState(int index) {
      return _checkedSet.Contains(index) ? CheckState.Checked : CheckState.Unchecked;
    }

    public void SetItemCheckState(int index, CheckState value) {
      if (index < 0 || index >= Items.Count) return;

      var oldState = GetItemCheckState(index);
      if (oldState == value) return;

      var args = new ItemCheckEventArgs(index, oldState, value);
      OnItemCheck(args);

      if (args.NewValue == CheckState.Checked || args.NewValue == CheckState.Indeterminate)
        _checkedSet.Add(index);
      else
        _checkedSet.Remove(index);

      NotifyStateChanged();
    }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event ItemCheckEventHandler ItemCheck;

    protected virtual void OnItemCheck(ItemCheckEventArgs e) {
      ItemCheck?.Invoke(this, e);
    }

    // ═══════════════════════════════════════════════
    // HTML Rendering - override to render as checkboxes
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "div";

    protected override string GetCssClasses() {
      return "swf-control swf-checkedlistbox";
    }

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();
      style += "overflow:auto;";
      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      for (int i = 0; i < Items.Count; i++) {
        int index = i; // capture for closure

        builder.OpenElement(seq++, "label");
        builder.AddAttribute(seq++, "style", "display:flex;align-items:center;gap:4px;padding:2px 4px;cursor:pointer;");

        builder.OpenElement(seq++, "input");
        builder.AddAttribute(seq++, "type", "checkbox");

        if (_checkedSet.Contains(i))
          builder.AddAttribute(seq++, "checked", true);

        if (!Enabled)
          builder.AddAttribute(seq++, "disabled", true);

        builder.AddAttribute(seq++, "onchange",
          EventCallback.Factory.Create<ChangeEventArgs>(
            GetBlazorReceiver(),
            (ChangeEventArgs e) => {
              bool isChecked = _checkedSet.Contains(index);
              SetItemCheckState(index, isChecked ? CheckState.Unchecked : CheckState.Checked);
              SelectedIndex = index;
            }));

        builder.CloseElement(); // input

        builder.OpenElement(seq++, "span");
        builder.AddContent(seq++, GetItemText(Items[i]));
        builder.CloseElement(); // span

        builder.CloseElement(); // label
      }
    }

    protected override void AddEventAttributes(RenderTreeBuilder builder, ref int seq) {
      base.AddEventAttributes(builder, ref seq);
    }

    // ═══════════════════════════════════════════════
    // CheckedIndexCollection
    // ═══════════════════════════════════════════════

    public class CheckedIndexCollection : IList<int>, IList {

      private readonly CheckedListBox _owner;
      internal CheckedIndexCollection(CheckedListBox owner) { _owner = owner; }

      private List<int> GetList() {
        var list = new List<int>(_owner._checkedSet);
        list.Sort();
        return list;
      }

      public int Count => _owner._checkedSet.Count;
      public bool IsReadOnly => true;
      public bool IsFixedSize => true;
      public bool IsSynchronized => false;
      public object SyncRoot => this;

      public int this[int index] {
        get => GetList()[index];
        set { }
      }

      object IList.this[int index] {
        get => GetList()[index];
        set { }
      }

      public bool Contains(int item) => _owner._checkedSet.Contains(item);
      public void CopyTo(int[] array, int arrayIndex) => GetList().CopyTo(array, arrayIndex);
      public int IndexOf(int item) => GetList().IndexOf(item);
      public IEnumerator<int> GetEnumerator() => GetList().GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => GetList().GetEnumerator();

      public void Add(int item) { }
      public void Clear() { }
      public void Insert(int index, int item) { }
      public bool Remove(int item) => false;
      public void RemoveAt(int index) { }

      int IList.Add(object value) => -1;
      bool IList.Contains(object value) => value is int i && Contains(i);
      int IList.IndexOf(object value) => value is int i ? IndexOf(i) : -1;
      void IList.Insert(int index, object value) { }
      void IList.Remove(object value) { }
      void ICollection.CopyTo(Array array, int index) => ((IList)GetList()).CopyTo(array, index);
    }

    // ═══════════════════════════════════════════════
    // CheckedItemCollection
    // ═══════════════════════════════════════════════

    public class CheckedItemCollection : IList<object>, IList {

      private readonly CheckedListBox _owner;
      internal CheckedItemCollection(CheckedListBox owner) { _owner = owner; }

      private List<object> GetList() {
        var list = new List<object>();
        var sorted = new List<int>(_owner._checkedSet);
        sorted.Sort();
        foreach (int idx in sorted) {
          if (idx >= 0 && idx < _owner.Items.Count)
            list.Add(_owner.Items[idx]);
        }
        return list;
      }

      public int Count => _owner._checkedSet.Count;
      public bool IsReadOnly => true;
      public bool IsFixedSize => true;
      public bool IsSynchronized => false;
      public object SyncRoot => this;

      public object this[int index] {
        get => GetList()[index];
        set { }
      }

      public bool Contains(object item) => GetList().Contains(item);
      public void CopyTo(object[] array, int arrayIndex) => GetList().CopyTo(array, arrayIndex);
      public int IndexOf(object item) => GetList().IndexOf(item);
      public IEnumerator<object> GetEnumerator() => GetList().GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => GetList().GetEnumerator();

      public void Add(object item) { }
      public void Clear() { }
      public void Insert(int index, object item) { }
      public bool Remove(object item) => false;
      public void RemoveAt(int index) { }

      int IList.Add(object value) => -1;
      bool IList.Contains(object value) => Contains(value);
      int IList.IndexOf(object value) => IndexOf(value);
      void IList.Insert(int index, object value) { }
      void IList.Remove(object value) { }
      void ICollection.CopyTo(Array array, int index) => ((IList)GetList()).CopyTo(array, index);
    }
  }
}
