using UnityEngine;
using System.Collections;
using System;

namespace cn.sharesdk.unity3d 
{
	[Serializable]
	public class DevInfoSet
	{
		public SinaWeiboDevInfo sinaweibo;

		public QQ qq;
		public QZone qzone;
		public WeChat wechat;
		public WeChatMoments wechatMoments; 
		public WeChatFavorites wechatFavorites;


		#if UNITY_ANDROID

		#elif UNITY_IPHONE

		public WechatSeries wechatSeries;						//iOS端微信系列, 可直接配置微信三个子平台 		[仅支持iOS端]
		public QQSeries qqSeries;								//iOS端QQ系列,  可直接配置QQ系列两个子平台		[仅支持iOS端]

		//安卓配置印象笔记国内与国际版直接在Evernote中配置														
		#endif

	}

	public class DevInfo 
	{	
		public bool Enable = true;
	}

	[Serializable]
	public class SinaWeiboDevInfo : DevInfo 
	{
#if UNITY_ANDROID
		public const int type = (int) PlatformType.SinaWeibo;
		public string SortId = "4";
		public string AppKey = "650343694";
		public string AppSecret = "5bd99ffa23bd05cbb649ea540963ab86";
		public string RedirectUrl = "http://api.pinidea.online";
		public bool ShareByAppClient = true;
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.SinaWeibo;
		public string app_key = "650343694";
		public string app_secret = "5bd99ffa23bd05cbb649ea540963ab86";
		public string redirect_uri = "http://api.pinidea.online";
		public string auth_type = "both";	//can pass "both","sso",or "web"  
#else
		public const int type = 9992;

		#endif
		}

		[Serializable]
		public class QQ : DevInfo 
		{
		#if UNITY_ANDROID
		public const int type = (int) PlatformType.QQ;
		public string SortId = "2";
		public string AppId = "1105442815";
		public string AppKey = "2b723a9b2c445b174b5bc60e6f7234cb";
		public bool ShareByAppClient = true;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QQ;
		public string app_id = "1105442815";
		public string app_key = "2b723a9b2c445b174b5bc60e6f7234cb";
		public string auth_type = "both";  //can pass "both","sso",or "web" 
		#else
		public const int type = 9993;

		#endif
		}

		[Serializable]
		public class QZone : DevInfo 
		{
		#if UNITY_ANDROID
		public string SortId = "1";
		public const int type = (int) PlatformType.QZone;
		public string AppId = "100371282";
		public string AppKey = "ae36f4ee3946e1cbb98d6965b0b2ff5c";
		public bool ShareByAppClient = true;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QZone;
		public string app_id = "100371282";
		public string app_key = "aed9b0303e3ed1e27bae87c33761161d";
		public string auth_type = "both";  //can pass "both","sso",or "web" 
		#else
		public const int type = 9994;

		#endif
		}



		[Serializable]
		public class WeChat : DevInfo 
		{	
		#if UNITY_ANDROID
		public string SortId = "5";
		public const int type = (int) PlatformType.WeChat;
		public string AppId = "wx9fdd68bd6b3c85a2";
		public string AppSecret = "b3558e97106af65d2326d43fcfd606aa";
		public string UserName = "gh_afb25ac019c9@app";
		public string Path = "/page/API/pages/share/share";
		public bool BypassApproval = false;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChat;
		public string app_id = "wx9fdd68bd6b3c85a2";
		public string app_secret = "b3558e97106af65d2326d43fcfd606aa";
		#else
		public const int type = 9995;

		#endif
		}

		[Serializable]
		public class WeChatMoments : DevInfo 
		{
		#if UNITY_ANDROID
		public string SortId = "6";
		public const int type = (int) PlatformType.WeChatMoments;
		public string AppId = "wx9fdd68bd6b3c85a2";
		public string AppSecret = "b3558e97106af65d2326d43fcfd606aa";
		public bool BypassApproval = false;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatMoments;
		public string app_id = "wx9fdd68bd6b3c85a2";
		public string app_secret = "b3558e97106af65d2326d43fcfd606aa";
		#else
		public const int type = 9996;

		#endif
		}

		[Serializable]
		public class WeChatFavorites : DevInfo 
		{
		#if UNITY_ANDROID
		public string SortId = "7";
		public const int type = (int) PlatformType.WeChatFavorites;
		public string AppId = "wx9fdd68bd6b3c85a2";
		public string AppSecret = "b3558e97106af65d2326d43fcfd606aa";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatFavorites;
		public string app_id = "wx9fdd68bd6b3c85a2";
		public string app_secret = "b3558e97106af65d2326d43fcfd606aa";
		#else
		public const int type = 9997;

		#endif
		}

		[Serializable]
		public class WechatSeries : DevInfo 
		{	
		#if UNITY_ANDROID
		//for android,please set the configuraion in class "Wechat" ,class "WechatMoments" or class "WechatFavorite"
		//对于安卓端，请在类Wechat,WechatMoments或WechatFavorite中配置相关信息↑	
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WechatPlatform;
		public string app_id = "wx9fdd68bd6b3c85a2";
		public string app_secret = "b3558e97106af65d2326d43fcfd606aa";
		#else
		public const int type = 9999;

		#endif
		}

		[Serializable]
		public class QQSeries : DevInfo 
		{	
		#if UNITY_ANDROID
		//for android,please set the configuraion in class "QQ" and  class "QZone"
		//对于安卓端，请在类QQ或QZone中配置相关信息↑	
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QQPlatform;
		public string app_id = "1105442815";
		public string app_key = "2b723a9b2c445b174b5bc60e6f7234cb";
		public string auth_type = "both";  //can pass "both","sso",or "web" 
		#else
		public const int type = 9998;

		#endif
		}


		}
