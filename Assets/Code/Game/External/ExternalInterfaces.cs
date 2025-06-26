using UnityEngine;
using System.Runtime.InteropServices;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public static class ExternalInterfaces
	{
		[DllImport("__Internal")]
		public static extern void RO_SavePhoto(string readAddr);

		public static object androidSavePic;

		public static bool AndroidSavePic(string srcPath)
		{
			#if UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android)
			{
			if (androidSavePic == null)
			androidSavePic = new AndroidJavaClass("com.xindong.RODevelop.UnitySavePicActivity");
			return ((AndroidJavaClass)androidSavePic).CallStatic<bool>("savePictureFileToDCIM", srcPath);
			}
			#endif
			return false;
		}

		public static bool SavePicToDCIM(string srcPath)
		{
            #if UNITY_EDITOR

            Debug.Log("EDITOR MODE srcPath:" + srcPath);
            return true;
			#elif UNITY_ANDROID
			return AndroidSavePic(srcPath);
			#elif UNITY_IOS
			RO_SavePhoto(srcPath);
			return true;
			#endif
			return false;
		}

		[DllImport("__Internal")]
		private static extern void InitSpeechRecognizer();

		[DllImport("__Internal")]
		private static extern void startRecognizerHandler();

		[DllImport("__Internal")]
		private static extern void stopRecognizerHandler();

		#if UNITY_ANDROID
		private static AndroidJavaObject jc;
		private static AndroidJavaObject jo;
		#endif

		public static void InitRecognizer(string name)
		{
			RO.LoggerUnused.Log("InitRecognizer");
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_ANDROID
			if(jc == null)
			{
			jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			}
			jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.Call("InitSpeechRecognizer",FileDirectoryHandler.GetAbsolutePath(name));
			#elif UNITY_IOS
			InitSpeechRecognizer();
			#endif
		}

		public static void StartRecognizer()
		{
			RO.LoggerUnused.Log("StartRecognizer");
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_ANDROID
			jo.Call("startRecognizerHandler");
			#elif UNITY_IOS
			startRecognizerHandler();
			#endif
		}

		public static void StopRecognizer()
		{
			RO.LoggerUnused.Log("StopRecognizer");
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_ANDROID
			jo.Call("stopRecognizerHandler");
			#elif UNITY_IOS
			stopRecognizerHandler();
			#endif
		}

		[DllImport("__Internal")]
		public static extern int getDeviceBatteryPct();

		[DllImport("__Internal")]
		public static extern bool getDeviceBatteryCharging();

		// Get Device Battery Pct (1-100)
		public static int GetSysBatteryPct()
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_ANDROID
			if (androidSavePic == null)
			{
			androidSavePic = new AndroidJavaClass ("com.xindong.RODevelop.UnitySavePicActivity");
			}
			return ((AndroidJavaClass)androidSavePic).CallStatic<int> ("GetSysBatteryPct");
			#elif UNITY_IOS
			return getDeviceBatteryPct();
			#endif
			return 100;
		}

		public static bool GetSysBatteryIsCharge()
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_ANDROID
			if (androidSavePic == null)
			{
			androidSavePic = new AndroidJavaClass ("com.xindong.RODevelop.UnitySavePicActivity");
			}
			return ((AndroidJavaClass)androidSavePic).CallStatic<bool> ("GetSysBatteryIsCharge");
			#elif UNITY_IOS
			return getDeviceBatteryCharging();
			#endif
			return false;
		}

		public static string GetPackageName()
		{
			#if UNITY_ANDROID
			if (androidSavePic == null)
			androidSavePic = new AndroidJavaClass("com.xindong.RODevelop.UnitySavePicActivity");
			return ((AndroidJavaClass)androidSavePic).CallStatic<string>("GetPackageName");
			#endif
			return "";
		}

		[DllImport("__Internal")]
		public static extern void setScreenBrightness(float value);

		[DllImport("__Internal")]
		public static extern float getSysScreenBrightness();

		public static void SetScreenBrightness(float value)
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_ANDROID
			if(jc == null)
			{
			jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			}
			jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.Call("SetCurrentBrightness", value);
			#elif UNITY_IOS
			setScreenBrightness(value);
			#endif
		}

		public static float GetSysScreenBrightness()
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_ANDROID
			if(jc == null)
			{
			jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			}
			jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			return jo.Call<float>("GetSysScreenBrightness");
			#elif UNITY_IOS
			return getSysScreenBrightness();
			#endif

			return 0.5f;
		}

		[DllImport("__Internal")]
		private static extern void OpenWifiSetting();

		[DllImport("__Internal")]
		private static extern void OpenAppSetting();

		[DllImport("__Internal")]
		private static extern float GetSystemVersion();

		public static void OpenWifiSet()
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_ANDROID

			#elif UNITY_IOS
			OpenWifiSetting();
			#endif
		}

		public static void OpenAppSet()
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_ANDROID

			#elif UNITY_IOS
			OpenAppSetting();
			#endif
		}

		public static float GetSysVer()
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_ANDROID

			#elif UNITY_IOS
			return GetSystemVersion();
			#endif
			return 0.0f;
		}

		#if _XDSDK_LINK_NATIVE_ 
		[DllImport("__Internal")]
		private static extern void sdk_debug_msg(string msg);
		#else
		private static  void sdk_debug_msg(string msg)
		{

		}
		#endif
        // ios 评分
        public static void RateReviewApp(string appid)
		{
			storeReviewCall(appid);
		}

		#if _XDSDK_LINK_NATIVE_ 
		[DllImport("__Internal")]
			public static extern void storeReviewCall(string appid);
		#else
			public static  void storeReviewCall(string appid)
		{

		}
			#endif

		#if UNITY_IOS && !UNITY_EDITOR
		[DllImport("__Internal")]
			public static extern string GetIOSVersion ( );
			#else
			public static string GetIOSVersion ( )
			{
			return "10.0.0";
			}
		#endif


		#if UNITY_IOS && !UNITY_EDITOR
		[DllImport("__Internal")]
		public static extern bool isUserNotificationEnable ( );
		#else
		public static bool isUserNotificationEnable ( )
		{
		return false;
		}
		#endif

		#if UNITY_IOS && !UNITY_EDITOR
		[DllImport("__Internal")]
		public static extern void ShowHintOpenPushView (string title,string message,string cancelButtonTitle,string otherButtonTitles);
		#else
		public static void ShowHintOpenPushView (string title,string message,string cancelButtonTitle,string otherButtonTitles)
		{
		}
		#endif


	}
}
// namespace RO
