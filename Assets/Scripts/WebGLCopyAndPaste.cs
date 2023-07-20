using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

// #define WEBGL_COPY_AND_PASTE_SUPPORT_TEXTMESH_PRO

//Makes it possible to copy and past in a WebGL build
public class WebGLCopyAndPasteAPI {

#if UNITY_WEBGL

	[DllImport("__Internal")]
	private static extern void initWebGLCopyAndPaste(string objectName, string cutCopyCallbackFuncName, string pasteCallbackFuncName);
	[DllImport("__Internal")]
	private static extern void passCopyToBrowser(string str);

#endif

	static public void Init(string objectName, string cutCopyCallbackFuncName, string pasteCallbackFuncName) {
#if UNITY_WEBGL

		initWebGLCopyAndPaste(objectName, cutCopyCallbackFuncName, pasteCallbackFuncName);

#endif
	}

	static public void PassCopyToBrowser(string str) {
#if UNITY_WEBGL

		passCopyToBrowser(str);

#endif
	}
}

public class WebGLCopyAndPaste : MonoBehaviour {
	void Start() {
		if (!Application.isEditor) {
			WebGLCopyAndPasteAPI.Init(this.name, "GetClipboard", "ReceivePaste");
		}
	}

	private void SendKey(string baseKey) {
		string appleKey = "%" + baseKey;
		string naturalKey = "^" + baseKey;

		var currentObj = EventSystem.current.currentSelectedGameObject;
		if (currentObj == null) {
			return;
		}
		{
			var input = currentObj.GetComponent<UnityEngine.UI.InputField>();
			if (input != null) {
				// I don't know what's going on here. The code in InputField
				// is looking for ctrl-c but that fails on Mac Chrome/Firefox
				input.ProcessEvent(Event.KeyboardEvent(naturalKey));
				input.ProcessEvent(Event.KeyboardEvent(appleKey));
				// so let's hope one of these is basically a noop
				return;
			}
		}
#if WEBGL_COPY_AND_PASTE_SUPPORT_TEXTMESH_PRO
    {
      var input = currentObj.GetComponent<TMPro.TMP_InputField>();
      if (input != null) {
        // I don't know what's going on here. The code in InputField
        // is looking for ctrl-c but that fails on Mac Chrome/Firefox
        // so let's hope one of these is basically a noop
        input.ProcessEvent(Event.KeyboardEvent(naturalKey));
        input.ProcessEvent(Event.KeyboardEvent(appleKey));
        return;
      }
    }
#endif
	}

	public void GetClipboard(string key) {
		SendKey(key);
		WebGLCopyAndPasteAPI.PassCopyToBrowser(GUIUtility.systemCopyBuffer);
	}

	public void ReceivePaste(string str) {
		GUIUtility.systemCopyBuffer = str;
	}
}