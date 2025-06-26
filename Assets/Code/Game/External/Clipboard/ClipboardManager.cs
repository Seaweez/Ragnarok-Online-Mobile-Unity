using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

[SLua.CustomLuaClassAttribute]
public class ClipboardManager
{
#if UNITY_IOS
	[DllImport("__Internal")]
	private extern static int _CopyToClipBoard(string contents);
#elif UNITY_ANDROID
    private static int _CopyToClipBoard(string contents)
    {
		AndroidJavaClass jc = new AndroidJavaClass("com.xindong.RODevelop.Clipboard");
        return jc.CallStatic<int>("CopyToClipBoard", contents);
    }
#else
	private static int _CopyToClipBoard(string contents)
	{
		return -1;
	}
#endif

	//  0:复制到剪切板成功
	// -1:复制到剪切板失败
	public static int CopyToClipBoard(string contents)
	{
		return _CopyToClipBoard(contents);
	}
}
