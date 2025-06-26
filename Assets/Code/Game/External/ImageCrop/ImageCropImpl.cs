using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
using UnityEngine.UI;

[SLua.CustomLuaClassAttribute]
public class ImageCropImpl : MonoBehaviour
{
    public delegate void ImageChooseDoneEvt(Texture2D tex);
    public static ImageChooseDoneEvt chooseDone = null;
#if UNITY_IOS
	[DllImport("__Internal")]
	private extern static void OpenAlbum (int w, int h);
#elif UNITY_ANDROID
    private static void OpenAlbum(int w, int h)
    {
        mainAvtivity.Call("OpenAlbum", w, h);
    }

    private static AndroidJavaObject _MainActivity = null;
    private static AndroidJavaObject mainAvtivity
    {
        get
        {
            if (_MainActivity == null)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                _MainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return _MainActivity;
        }
    }
#else
	private static void OpenAlbum(int w, int h) { }
#endif

    private static int width = 128;
	private static int height = 128;

    void Awake()
    {
        this.name = "ImageCrop";
    }

    public static void ChooseStart(int w = 128, int h = 128)
	{
		OpenAlbum (w, h);
		width = w;
		height = h;
	}

	public void ChooseComplete(string texBytes)
	{
		Texture2D tex = Parse2Texture (texBytes);
		if (tex == null)
		{
			Debug.LogError ("解析字节流为Texture2D失败！");
			return;
        }
        if (chooseDone != null)
		{
			chooseDone(tex);
		}
	}

	public void ChooseFailed(string error)
	{
		Debug.LogError ("ImageCropImpl Error: " + error);
	}

	private Texture2D Parse2Texture(string texBytes)
	{
		Texture2D tex = null;
		if(!string.IsNullOrEmpty(texBytes))
		{	
			tex = new Texture2D (width, height);
			byte[] bytes = System.Convert.FromBase64String (texBytes);
			tex.LoadImage (bytes);
		}
		return tex;
	}
}
