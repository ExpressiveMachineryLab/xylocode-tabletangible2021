mergeInto(LibraryManager.library, {
    initWebGLCopyAndPaste__postset: '_initWebGLCopyAndPaste();',
    initWebGLCopyAndPaste: function() {
      // for some reason only on Safari does Unity call
      // preventDefault so let's prevent preventDefault
      // so the browser will generate copy and paste events
      window.addEventListener = function(origFn) {
        function noop() {}

        // I hope c,x,v are universal
        const keys = {'c': true, 'x': true, 'v': true};

        // Emscripten doesn't support the spread operator or at
        // least the one used by Unity 2019.4.1
        return function(name, fn) {
          const args = Array.prototype.slice.call(arguments);
          if (name !== 'keypress') {
            return origFn.apply(this, args);
          }
          args[1] = function(event) {
            const hArgs = Array.prototype.slice.call(arguments);
            if (keys[event.key.toLowerCase()] &&
                ((event.metaKey ? 1 : 0) + (event.ctrlKey ? 1 : 0)) === 1) {
              event.preventDefault = noop;
            }
            return fn.apply(this, hArgs);
          };
          return origFn.apply(this, args);
        };
      }(window.addEventListener);
      _initWebGLCopyAndPaste = function(objectNamePtr, cutCopyFuncNamePtr, pasteFuncNamePtr) {
        window.becauseUnityIsBadWithJavascript_webglCopyAndPaste =
            window.becauseUnityIsBadWithJavascript_webglCopyAndPaste || {
           initialized: false,
           objectName: Pointer_stringify(objectNamePtr),
           cutCopyFuncName: Pointer_stringify(cutCopyFuncNamePtr),
           pasteFuncName: Pointer_stringify(pasteFuncNamePtr),
        };
        const g = window.becauseUnityIsBadWithJavascript_webglCopyAndPaste;

        if (!g.initialized) {
          window.addEventListener('cut', function(e) {
            e.preventDefault();
            SendMessage(g.objectName, g.cutCopyFuncName, 'x');
            event.clipboardData.setData('text/plain', g.clipboardStr);
          });
          window.addEventListener('copy', function(e) {
            e.preventDefault();
            SendMessage(g.objectName, g.cutCopyFuncName, 'c');
            event.clipboardData.setData('text/plain', g.clipboardStr);
          });
          window.addEventListener('paste', function(e) {
            const str = e.clipboardData.getData('text');
            SendMessage(g.objectName, g.pasteFuncName, str);
          });
        }
      };
    },
    passCopyToBrowser: function(stringPtr) {
      const g = window.becauseUnityIsBadWithJavascript_webglCopyAndPaste;
      const str = Pointer_stringify(stringPtr);
      g.clipboardStr = str;
    },
});