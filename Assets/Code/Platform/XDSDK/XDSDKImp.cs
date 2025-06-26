using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

namespace xdsdk
{

	public class XDSDKImp
	{
		private static XDSDKImp _instance;

		public XDCallback xdCallback;
		
		public static XDSDKImp GetInstance() {
			if (_instance == null) {
				_instance = new XDSDKImp ();
			}
			return _instance;
		}

		public void SetCallback(XDCallback callback){
			xdCallback = callback;
		}

		public XDCallback GetXDCallback(){
			return xdCallback;
		}

		public void HideGuest(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			hideGuest();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("hideGuest");
			#endif
		}

		public void HideWX(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			hideWeiChat();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("hideWX");
			#endif
		}

		public void HideQQ(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			hideQQ();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("hideQQ");
			#endif
		}

		public void ShowVC(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			showVC();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("showVC");
			#endif
		}

		public void SetQQWeb(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			setQQWeb();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("setQQWeb");
			#endif
		}

		public void SetWXWeb(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			setWXWeb();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("setWXWeb");
			#endif
		}

		public void HideTapTap(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			hideTapTap();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("hideTapTap");
			#endif
		}

		public void SetLoginEntries(string[] entries){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			setLoginEntries(entries);

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("setLoginEntries", CSStringArrayToJavaStringArray(entries));
			#endif
		}

		public string GetSDKVersion(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			return getXDSDKVersion();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			return jc.CallStatic<string> ("getSDKVersion");
			#endif
			return "0.0.0";
		}

		public void InitSDK(string appid, int aOrientation, string channel, string version, bool enableTapDB){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			initXDSDK(appid,aOrientation,channel,version,enableTapDB);

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("initSDK",appid,aOrientation,channel,version,enableTapDB);
			#endif
		}

		public void Login(){

            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			xdLogin();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("login");
			#endif
		}

		public string GetAccessToken(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			return getXDAccessToken();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			return jc.CallStatic<string> ("getAccessToken");
			#endif
			return "";
		}

		public bool IsLoggedIn(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			return isXdLoggedIn();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			return jc.CallStatic<bool> ("isLoggedIn");
			#endif
			return true;
		}

		public bool OpenUserCenter(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			return openUserCenter();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			return jc.CallStatic<bool> ("openUserCenter");
			#endif
			return false;
		}

		public bool UserFeedback(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			userFeedback();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("userFeedback");
			#endif
			return false;
		}


		public void OpenRealName(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			openRealName();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic("openRealName");
			#endif
		}

		public bool Pay(Dictionary<string, string> info){

            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			xdPay(info["Product_Name"],info["Product_Id"],info["Product_Price"],info["Sid"],info["Role_Id"],info["OrderId"],info["EXT"]);

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			return jc.CallStatic<bool> ("pay", DicToMap(info));
			#endif
			return false;

		}

		public void Logout(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
			xdLogout();

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("logout");

			#endif
		}

		public void Exit(){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_


			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("exit");
			#endif
		}

		public void Share(Dictionary<string, string> content){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_

			xdShare(content["text"],content["bText"],content["scene"],content["shareType"],content["title"],content["description"],content["thumbPath"],content["imageUrl"],content["musicUrl"],
			content["musicLowBandUrl"],content["musicDataUrl"],content["musicLowBandDataUrl"],content["videoUrl"],content["videoLowBandUrl"],content["webpageUrl"]);

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("shareToWX", DicToMap(content));
			#endif
		}

		public void SetLevel(int level){
            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_

			setLevel(level);
				
			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("setLevel", level);
			#endif
		}

		public void SetServer(string server){

            #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_

			setServer(server);

			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.xd.unitysdk.UnitySDK");
			jc.CallStatic ("setServer", server);
			#endif
		}
						

        #if UNITY_IOS && !UNITY_EDITOR && _XDSDK_LINK_NATIVE_
		[DllImport("__Internal")]
		private static extern void initSDK(string appid, int aOrientation);

		[DllImport("__Internal")]
		private static extern void initXDSDK(string appid, int aOrientation, string channel, string version, bool enableTapdb);

		[DllImport("__Internal")]
		private static extern void setLevel(int level);

		[DllImport("__Internal")]
		private static extern void setServer(string server);

		[DllImport("__Internal")]
		private static extern void xdLogin();

		[DllImport("__Internal")]
		private static extern bool isXdLoggedIn();

		[DllImport("__Internal")]
		private static extern void xdLogout();

		[DllImport("__Internal")]
		private static extern bool openUserCenter();

		[DllImport("__Internal")]
		private static extern void xdPay(string proudct_name, string product_id, string product_price, string sid, string role_id, string orderid, string ext);

		[DllImport("__Internal")]
		private static extern string getXDSDKVersion();

		[DllImport("__Internal")]
		private static extern string getXDAccessToken();

		[DllImport("__Internal")]
		private static extern void hideGuest();

		[DllImport("__Internal")]
		private static extern void hideQQ();

		[DllImport("__Internal")]
		private static extern void hideWeiChat();

		[DllImport("__Internal")]
		private static extern void showVC();

		[DllImport("__Internal")]
		private static extern void setQQWeb();

		[DllImport("__Internal")]
		private static extern void setWXWeb();

		[DllImport("__Internal")]
		private static extern void setLoginEntries(string[] entries);

		[DllImport("__Internal")]
		private static extern void userFeedback();

		[DllImport("__Internal")]
		private static extern void hideTapTap();

		[DllImport("__Internal")]
		private static extern void openRealName();

		[DllImport("__Internal")]
		private static extern void xdShare (string text, string bText, string scene, string shareType, string title,string description, string thumbPath, 
		string imageUrl, string musicUrl, string musicLowBandUrl, string musicDataUrl, string musicLowBandDataUrl, string videoUrl,string videoLowBandUrl,
		string webPageUrl
		);

		#endif


		#if UNITY_IOS && !UNITY_EDITOR

		#elif UNITY_ANDROID && !UNITY_EDITOR
		private AndroidJavaObject CSStringArrayToJavaStringArray(string [] values) {
			AndroidJavaClass arrayClass  = new AndroidJavaClass("java.lang.reflect.Array");
			AndroidJavaObject arrayObject = arrayClass.CallStatic<AndroidJavaObject>("newInstance",
                                             new AndroidJavaClass("java.lang.String"),
											 values.Count());
			for (int i=0; i<values.Count(); ++i) {
				arrayClass.CallStatic("set", 
				arrayObject, 
				i,
				new AndroidJavaObject("java.lang.String", 
				values[i]));
			}
			return arrayObject;
		}
		public static AndroidJavaObject DicToMap(Dictionary<string, string> dictionary)
		{
			if(dictionary == null)
			{
				return null;
			}
			AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
			foreach(KeyValuePair<string, string> pair in dictionary)
			{
				map.Call<string>("put", pair.Key, pair.Value);
			}
			return map;
		}
		#endif

	}

}




