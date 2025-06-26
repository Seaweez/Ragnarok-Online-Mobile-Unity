using UnityEngine;
using System;
using System.Collections;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

[SLua.CustomLuaClassAttribute]
public class SafeArea : MonoSingleton<SafeArea>
{
	public delegate void _OnOrientationChanged(bool isLandscapeLeft);
	public _OnOrientationChanged onOrientationChanged = null;

	public void SwitchEdgeProtect(bool bSwitch)
	{
#if !UNITY_EDITOR && UNITY_IOS
		SwitchIPhoneXEdgeProtect(bSwitch);
#endif
	}

#if UNITY_EDITOR
	private string _ScreenRes = string.Empty;
	private float ScreenWidth = 2688f;
	private float _l = 132f, _r = 132f, _b = 63f, _t = 0f, _w = 2172f, _h = 1062f;
	public float l { get { return _l * scale; } }
	public float r { get { return _r * scale; } }
	public float b { get { return _b * scale; } }
	public float t { get { return _t * scale; } }
	public float w { get { return _w * scale; } }
	public float h { get { return _h * scale; } }
	public float left { get { return _l * scale; } }
	public float right { get { return _r * scale; } }
	public float bottom { get { return _b * scale; } }
	public float top { get { return _t * scale; } }
	public float width { get { return _w * scale; } }
	public float height { get { return _h * scale; } }

	float _Scale;
	private float scale
	{
		get
		{
			if (_ScreenRes != UnityStats.screenRes)
			{
				_ScreenRes = UnityStats.screenRes;
				int truthWidth = int.Parse(_ScreenRes.Split('x')[0]);
				_Scale = truthWidth / ScreenWidth;
			}
			return _Scale;
		}
	}

	public static SafeArea Instance
	{
		get
		{
			if (Application.isPlaying)
				return ins;
			else
				return null;
		}
	}

	static System.Reflection.PropertyInfo _Prop = null;
	static object _GameView = null;
	static Vector2 GetCurrentGameViewSize()
	{

		var gameviewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
		var gameView = EditorWindow.GetWindow(gameviewType);

		if (gameView != null)
		{
			var currentGameViewSizeProp = gameView.GetType().GetProperty("currentGameViewSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

			if (currentGameViewSizeProp != null)
			{
				var currentGameViewSize = currentGameViewSizeProp.GetValue(gameView, null);
				var gameViewSizeType = currentGameViewSize.GetType();

				var w = gameViewSizeType.GetProperty("width", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
				var h = gameViewSizeType.GetProperty("height", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

				Vector2 size = new Vector2();
				size.x = (int)w.GetValue(currentGameViewSize, null);
				size.y = (int)h.GetValue(currentGameViewSize, null);
				return size;
			}
		}

		return new Vector2(Screen.width, Screen.height);
	}




	static string GetGameViewResolutionName()
	{
		GetCurrentGameViewSize();
		if (_Prop == null || _GameView == null)
		{
			Debug.LogWarning("反射函数UnityEditor.GameView.GetMainGameView返回值为空！");
			return string.Empty;
		}
		var _gvsize = _Prop.GetValue(_GameView, new object[0] { });
		var _gvsizeType = _gvsize.GetType();
		return (string)_gvsizeType.GetProperty("baseText", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(_gvsize, new object[0] { });
	}

    static bool _IsiPhoneX = false;
    static int mSizeFrame = -1;
    public static bool isiPhoneX
    {
        get
        {
            int frame = Time.frameCount;
            if (mSizeFrame != frame || !Application.isPlaying)
            {
                mSizeFrame = frame;
                _IsiPhoneX = GetGameViewResolutionName() == "iPhoneX";
            }
            return _IsiPhoneX;
        }
    }

#elif UNITY_IOS
	[DllImport("__Internal")]
	private extern static void GetSafeAreaImpl(out float l, out float r, out float b, out float t, out float w, out float h);
	[DllImport("__Internal")]
	private extern static void AddChangeOrientationListener();
	[DllImport("__Internal")]
	private extern static void RemoveChangeOrientationListener();
	[DllImport("__Internal")]
	private extern static void SwitchIPhoneXEdgeProtect(bool bSwitch);

	private float _l = 132f, _r = 132f, _b = 63f, _t = 0f, _w = 2172f, _h = 1062f;
	public float l { get{ return _l; } }
	public float r { get{ return _r; } }
	public float b { get{ return _b; } }
	public float t { get{ return _t; } }
	public float w { get{ return _w; } }
	public float h { get{ return _h; } }
	public float left { get{ return _l; } }
	public float right { get{ return _r; } }
	public float bottom { get{ return _b; } }
	public float top { get{ return _t; } }
	public float width { get{ return _w; } }
	public float height { get{ return _h; } }

	static int _IsiPhoneX = -1;
	public static bool isiPhoneX
	{
		get
		{
			if (_IsiPhoneX < 0)
			{
				// ตรวจสอบว่าเป็น iPhone และมี notch (เริ่มต้นจาก iPhone X)
				_IsiPhoneX = SystemInfo.deviceModel.StartsWith("iPhone") && (Screen.height >= 812 || Screen.width >= 812) ? 1 : 0;
			}
			return _IsiPhoneX > 0;
		}
	}

	public static SafeArea Instance { get { return ins; } }

	protected override void OnAwake()
	{
		AddChangeOrientationListener();
		RefreshSafeArea ();
	}

	void OnDisable()
	{
		RemoveChangeOrientationListener();
	}

	private void RefreshSafeArea()
	{
		if(isiPhoneX)
			GetSafeAreaImpl(out _l, out _r, out _b, out _t, out _w, out _h);
		else
		{
			_l = 0f;
			_r = 0f;
			_w = Screen.width;
			_h = Screen.height;
		}
	}	

	public void OnChangeOrientation(string isLandscapeLeft)
	{
		RefreshSafeArea();
		if(onOrientationChanged != null)
			onOrientationChanged (bool.Parse(isLandscapeLeft));
	}
#else
	private float _l = 132f, _r = 132f, _b = 63f, _t = 0f, _w = 2172f, _h = 1062f;
	public float l { get{ return _l; } }
	public float r { get{ return _r; } }
	public float b { get{ return _b; } }
	public float t { get{ return _t; } }
	public float w { get{ return _w; } }
	public float h { get{ return _h; } }
	public float left { get{ return _l; } }
	public float right { get{ return _r; } }
	public float bottom { get{ return _b; } }
	public float top { get{ return _t; } }
	public float width { get{ return _w; } }
	public float height { get{ return _h; } }

	static int _IsiPhoneX = -1;
	public static bool isiPhoneX
	{
		get
		{
			return false;
		}
	}

	public static SafeArea Instance { get { return ins; } }
#endif
}
