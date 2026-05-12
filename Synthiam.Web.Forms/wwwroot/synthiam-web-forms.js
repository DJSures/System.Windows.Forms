// Synthiam.Web.Forms - JavaScript Interop
// Provides browser-side functionality for WinForms-to-Web controls

window.SynthiamWebForms = {

  // Clipboard operations
  clipboard: {
    setText: async function (text) {
      try {
        await navigator.clipboard.writeText(text);
        return true;
      } catch (e) {
        console.warn('Clipboard write failed:', e);
        return false;
      }
    },
    getText: async function () {
      try {
        return await navigator.clipboard.readText();
      } catch (e) {
        console.warn('Clipboard read failed:', e);
        return '';
      }
    }
  },

  // File dialog simulation using <input type="file">
  fileDialog: {
    _fileInput: null,

    showOpen: function (filter, multiselect) {
      return new Promise((resolve) => {
        if (this._fileInput) {
          document.body.removeChild(this._fileInput);
        }
        const input = document.createElement('input');
        input.type = 'file';
        input.style.display = 'none';
        input.multiple = multiselect || false;
        if (filter) {
          input.accept = this._convertFilter(filter);
        }
        input.addEventListener('change', () => {
          const files = Array.from(input.files).map(f => ({
            name: f.name,
            size: f.size,
            type: f.type
          }));
          document.body.removeChild(input);
          this._fileInput = null;
          resolve(files);
        });
        input.addEventListener('cancel', () => {
          document.body.removeChild(input);
          this._fileInput = null;
          resolve([]);
        });
        document.body.appendChild(input);
        this._fileInput = input;
        input.click();
      });
    },

    showSave: function (defaultFileName, filter) {
      // Browser limitation: can't truly show a save dialog
      // Return the default filename; actual saving happens via download
      return Promise.resolve(defaultFileName || 'download');
    },

    _convertFilter: function (filter) {
      // Convert WinForms filter format "Images|*.png;*.jpg|All|*.*" to accept attribute
      if (!filter) return '';
      const parts = filter.split('|');
      const extensions = [];
      for (let i = 1; i < parts.length; i += 2) {
        parts[i].split(';').forEach(ext => {
          ext = ext.trim();
          if (ext && ext !== '*.*') {
            extensions.push(ext.replace('*', ''));
          }
        });
      }
      return extensions.join(',');
    }
  },

  // Download a file (for SaveFileDialog)
  downloadFile: function (filename, contentBase64, mimeType) {
    const link = document.createElement('a');
    link.href = 'data:' + (mimeType || 'application/octet-stream') + ';base64,' + contentBase64;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  },

  // MessageBox using browser dialogs
  messageBox: {
    show: function (text, caption, buttons) {
      // buttons: 0=OK, 1=OKCancel, 2=AbortRetryIgnore, 3=YesNoCancel, 4=YesNo, 5=RetryCancel
      switch (buttons) {
        case 0: // OK
          alert(caption ? caption + '\n\n' + text : text);
          return 1; // DialogResult.OK
        case 1: // OKCancel
          return confirm(caption ? caption + '\n\n' + text : text) ? 1 : 2;
        case 4: // YesNo
          return confirm(caption ? caption + '\n\n' + text : text) ? 6 : 7;
        case 3: // YesNoCancel
          var r = confirm(caption ? caption + '\n\n' + text : text);
          return r ? 6 : 7; // Can't distinguish Cancel from No in browser
        default:
          alert(caption ? caption + '\n\n' + text : text);
          return 1;
      }
    }
  },

  // Color dialog using <input type="color">
  colorDialog: {
    show: function (initialColor) {
      return new Promise((resolve) => {
        const input = document.createElement('input');
        input.type = 'color';
        input.value = initialColor || '#000000';
        input.style.display = 'none';
        input.addEventListener('input', () => {
          resolve(input.value);
          document.body.removeChild(input);
        });
        input.addEventListener('change', () => {
          resolve(input.value);
          if (input.parentNode) document.body.removeChild(input);
        });
        document.body.appendChild(input);
        input.click();
        // Fallback: if dialog was cancelled (no events fired), resolve after timeout
        setTimeout(() => {
          if (input.parentNode) {
            document.body.removeChild(input);
            resolve(null);
          }
        }, 60000);
      });
    }
  },

  // Help - open URL in new tab
  showHelp: function (url) {
    window.open(url, '_blank');
  },

  // Focus management
  focus: function (elementId) {
    const el = document.getElementById(elementId);
    if (el) el.focus();
  },

  blur: function (elementId) {
    const el = document.getElementById(elementId);
    if (el) el.blur();
  },

  // Scroll management
  scrollIntoView: function (elementId) {
    const el = document.getElementById(elementId);
    if (el) el.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
  },

  // Element measurements
  getBoundingRect: function (elementId) {
    const el = document.getElementById(elementId);
    if (!el) return null;
    const rect = el.getBoundingClientRect();
    return { x: rect.x, y: rect.y, width: rect.width, height: rect.height };
  },

  // SplitContainer drag support
  splitter: {
    _active: null,

    startDrag: function (dotnetRef, splitterElement, orientation) {
      this._active = {
        ref: dotnetRef,
        element: splitterElement,
        orientation: orientation,
        startX: 0,
        startY: 0
      };

      const onMouseMove = (e) => {
        if (!this._active) return;
        e.preventDefault();
        if (orientation === 'horizontal') {
          dotnetRef.invokeMethodAsync('OnSplitterMove', e.clientX);
        } else {
          dotnetRef.invokeMethodAsync('OnSplitterMove', e.clientY);
        }
      };

      const onMouseUp = () => {
        this._active = null;
        document.removeEventListener('mousemove', onMouseMove);
        document.removeEventListener('mouseup', onMouseUp);
        document.body.style.cursor = '';
        document.body.style.userSelect = '';
      };

      document.addEventListener('mousemove', onMouseMove);
      document.addEventListener('mouseup', onMouseUp);
      document.body.style.cursor = orientation === 'horizontal' ? 'col-resize' : 'row-resize';
      document.body.style.userSelect = 'none';
    }
  },

  // Context menu positioning
  contextMenu: {
    show: function (elementId, x, y) {
      const el = document.getElementById(elementId);
      if (el) {
        el.style.left = x + 'px';
        el.style.top = y + 'px';
        el.style.display = 'block';
      }
    },
    hide: function (elementId) {
      const el = document.getElementById(elementId);
      if (el) el.style.display = 'none';
    }
  },

  // Print support
  print: function () {
    window.print();
  },

  // Drag and drop support
  dragDrop: {
    makeDraggable: function (elementId, data) {
      const el = document.getElementById(elementId);
      if (!el) return;
      el.draggable = true;
      el.addEventListener('dragstart', (e) => {
        e.dataTransfer.setData('text/plain', data || '');
      });
    },
    makeDropTarget: function (elementId, dotnetRef) {
      const el = document.getElementById(elementId);
      if (!el) return;
      el.addEventListener('dragover', (e) => {
        e.preventDefault();
      });
      el.addEventListener('drop', (e) => {
        e.preventDefault();
        const data = e.dataTransfer.getData('text/plain');
        dotnetRef.invokeMethodAsync('OnDrop', data, e.clientX, e.clientY);
      });
    }
  }
};
