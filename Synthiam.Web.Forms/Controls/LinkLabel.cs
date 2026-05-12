// =============================================================================
// Synthiam.Web.Forms - LinkLabel control for Blazor
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Synthiam.Web.Forms;
using BlazorMouseEventArgs = Microsoft.AspNetCore.Components.Web.MouseEventArgs;

namespace System.Windows.Forms {

  // ---------------------------------------------------------------------------
  // LinkArea struct
  // ---------------------------------------------------------------------------
  public struct LinkArea {
    public int Start { get; set; }
    public int Length { get; set; }

    public LinkArea(int start, int length) {
      Start = start;
      Length = length;
    }

    public bool IsEmpty => Length == 0;

    public override bool Equals(object obj) => obj is LinkArea la && la.Start == Start && la.Length == Length;
    public override int GetHashCode() => Start ^ (Length << 16);
    public static bool operator ==(LinkArea a, LinkArea b) => a.Start == b.Start && a.Length == b.Length;
    public static bool operator !=(LinkArea a, LinkArea b) => !(a == b);
  }

  // ---------------------------------------------------------------------------
  // LinkBehavior enum
  // ---------------------------------------------------------------------------
  public enum LinkBehavior {
    SystemDefault = 0,
    AlwaysUnderline = 1,
    HoverUnderline = 2,
    NeverUnderline = 3
  }

  // ---------------------------------------------------------------------------
  // LinkLabel (extends ForwardDeclarations partial)
  // ---------------------------------------------------------------------------
  public partial class LinkLabel : Label {

    private Color _linkColor = Color.FromArgb(0, 0, 255);
    private Color _visitedLinkColor = Color.FromArgb(128, 0, 128);
    private Color _activeLinkColor = Color.Red;
    private Color _disabledLinkColor = Color.Empty;
    private LinkBehavior _linkBehavior = LinkBehavior.SystemDefault;
    private LinkArea _linkArea;
    private LinkCollection _links;

    public LinkLabel() {
      _links = new LinkCollection(this);
    }

    // ═══════════════════════════════════════════════
    // Properties
    // ═══════════════════════════════════════════════

    public Color LinkColor {
      get => _linkColor;
      set { _linkColor = value; NotifyStateChanged(); }
    }

    public Color VisitedLinkColor {
      get => _visitedLinkColor;
      set { _visitedLinkColor = value; NotifyStateChanged(); }
    }

    public Color ActiveLinkColor {
      get => _activeLinkColor;
      set { _activeLinkColor = value; NotifyStateChanged(); }
    }

    public Color DisabledLinkColor {
      get => _disabledLinkColor;
      set { _disabledLinkColor = value; NotifyStateChanged(); }
    }

    public LinkBehavior LinkBehavior {
      get => _linkBehavior;
      set { _linkBehavior = value; NotifyStateChanged(); }
    }

    public LinkArea LinkArea {
      get => _linkArea;
      set => _linkArea = value;
    }

    public LinkCollection Links => _links;

    public bool LinkVisited { get; set; }

    // ═══════════════════════════════════════════════
    // Events
    // ═══════════════════════════════════════════════

    public event LinkLabelLinkClickedEventHandler LinkClicked;

    protected virtual void OnLinkClicked(LinkLabelLinkClickedEventArgs e) {
      LinkClicked?.Invoke(this, e);
    }

    // ═══════════════════════════════════════════════
    // HTML Rendering
    // ═══════════════════════════════════════════════

    protected override string GetHtmlTag() => "a";

    protected override string BuildCssStyle() {
      var style = base.BuildCssStyle();

      // Link color
      if (LinkVisited && !_visitedLinkColor.IsEmpty)
        style += "color:" + _visitedLinkColor.ToCss() + ";";
      else if (!_linkColor.IsEmpty)
        style += "color:" + _linkColor.ToCss() + ";";

      // Link behavior
      switch (_linkBehavior) {
        case LinkBehavior.AlwaysUnderline:
        case LinkBehavior.SystemDefault:
          style += "text-decoration:underline;";
          break;
        case LinkBehavior.HoverUnderline:
          style += "text-decoration:none;";
          break;
        case LinkBehavior.NeverUnderline:
          style += "text-decoration:none;";
          break;
      }

      style += "cursor:pointer;";
      return style;
    }

    protected override void RenderContent(RenderTreeBuilder builder, ref int seq) {
      builder.AddContent(seq++, Text ?? string.Empty);
    }

    protected override void AddEventAttributes(RenderTreeBuilder builder, ref int seq) {
      base.AddEventAttributes(builder, ref seq);

      builder.AddAttribute(seq++, "href", "javascript:void(0)");
      builder.AddAttribute(seq++, "onclick",
        EventCallback.Factory.Create<BlazorMouseEventArgs>(
          GetBlazorReceiver(),
          (BlazorMouseEventArgs e) => {
            var link = _links.Count > 0 ? _links[0] : new Link();
            OnLinkClicked(new LinkLabelLinkClickedEventArgs(link, MouseButtons.Left));
          }));
    }

    // ═══════════════════════════════════════════════
    // LinkCollection
    // ═══════════════════════════════════════════════

    public class LinkCollection : IList<Link>, IList {

      private readonly List<Link> _list = new List<Link>();
      private readonly LinkLabel _owner;

      internal LinkCollection(LinkLabel owner) {
        _owner = owner;
      }

      public int Count => _list.Count;
      public bool IsReadOnly => false;
      public bool IsFixedSize => false;
      public bool IsSynchronized => false;
      public object SyncRoot => _list;

      public Link this[int index] {
        get => _list[index];
        set => _list[index] = value;
      }

      object IList.this[int index] {
        get => _list[index];
        set => _list[index] = (Link)value;
      }

      public Link Add(int start, int length) {
        var link = new Link { Start = start, Length = length };
        _list.Add(link);
        return link;
      }

      public Link Add(int start, int length, object linkData) {
        var link = new Link { Start = start, Length = length, LinkData = linkData };
        _list.Add(link);
        return link;
      }

      public void Add(Link item) => _list.Add(item);
      public void Clear() => _list.Clear();
      public bool Contains(Link item) => _list.Contains(item);
      public void CopyTo(Link[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
      public int IndexOf(Link item) => _list.IndexOf(item);
      public void Insert(int index, Link item) => _list.Insert(index, item);
      public bool Remove(Link item) => _list.Remove(item);
      public void RemoveAt(int index) => _list.RemoveAt(index);
      public IEnumerator<Link> GetEnumerator() => _list.GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

      int IList.Add(object value) { _list.Add((Link)value); return _list.Count - 1; }
      bool IList.Contains(object value) => _list.Contains((Link)value);
      int IList.IndexOf(object value) => _list.IndexOf((Link)value);
      void IList.Insert(int index, object value) => _list.Insert(index, (Link)value);
      void IList.Remove(object value) => _list.Remove((Link)value);
      void ICollection.CopyTo(Array array, int index) => ((IList)_list).CopyTo(array, index);
    }
  }
}
