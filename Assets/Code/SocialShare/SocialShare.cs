using UnityEngine;
using System.Collections;
using cn.sharesdk.unity3d;
using System.IO;
using System;
using System.Collections.Generic;

[SLua.CustomLuaClassAttribute]
public enum E_PlatformType
{
	WechatMoments = PlatformType.WeChatMoments,
	Wechat = PlatformType.WeChat,
	QQ = PlatformType.QQ,
	Sina = PlatformType.SinaWeibo,
}

[SLua.CustomLuaClassAttribute]
public class SocialShare : MonoSingleton<SocialShare>
{
	public class Callback
	{
		public int requestID;
		public Action<string> onSuccess;
		public Action<int, string> onFail;
		public Action onCancel;
	}

	public static SocialShare Instance
	{
		get
		{
			return ins;
		}
	}
	
	public ShareSDK shareSDK;

	private PlatformType[] m_platforms;
	private PlatformType[] m_platformsWithoutShareText;
	
	private List<Callback> m_cachedCallback;
	
	protected override void OnAwake()
	{
		shareSDK = gameObject.GetComponent<ShareSDK>();
		if (shareSDK == null) {
			shareSDK= gameObject.AddComponent<ShareSDK>();
		}
		shareSDK.authHandler = OnAuthResultHandler;
		shareSDK.shareHandler = OnShareResultHandler;
		shareSDK.showUserHandler = OnGetUserInfoResultHandler;
		shareSDK.getFriendsHandler = OnGetFriendsResultHandler;
		shareSDK.followFriendHandler = OnFollowFriendResultHandler;


		m_platforms = new PlatformType[5] {
			PlatformType.SinaWeibo,
			PlatformType.QQ,
			PlatformType.WeChat,
			PlatformType.WeChatMoments,
			PlatformType.WeChatFavorites
		};
		m_platformsWithoutShareText = new PlatformType[4] {
			PlatformType.SinaWeibo,
			PlatformType.WeChat,
			PlatformType.WeChatMoments,
			PlatformType.WeChatFavorites
		};
	}

	public void Initialize(
		string sina_weibo_app_key,
		string sina_weibo_app_secret,
		string qq_app_id,
		string qq_app_key,
		string wechat_app_id,
		string wechat_app_secret
	)
	{
		shareSDK = gameObject.GetComponent<ShareSDK>();
		if (shareSDK == null) {
			shareSDK= gameObject.AddComponent<ShareSDK>();
		}
		#if UNITY_IOS
		shareSDK.devInfo.sinaweibo.app_key = sina_weibo_app_key;
		shareSDK.devInfo.sinaweibo.app_secret = sina_weibo_app_secret;

		shareSDK.devInfo.qq.app_id = qq_app_id;
		shareSDK.devInfo.qq.app_key = qq_app_key;

		shareSDK.devInfo.wechat.app_id = wechat_app_id;
		shareSDK.devInfo.wechat.app_secret = wechat_app_secret;
		#elif UNITY_ANDORID

		shareSDK.devInfo.sinaweibo.AppId = sina_weibo_app_key;
		shareSDK.devInfo.sinaweibo.AppKey = sina_weibo_app_secret;

		shareSDK.devInfo.qq.AppId = qq_app_id;
		shareSDK.devInfo.qq.AppKey = qq_app_key;

		shareSDK.devInfo.wechat.AppId = wechat_app_id;
		shareSDK.devInfo.wechat.AppSecret = wechat_app_secret;
		#endif


		shareSDK.CustomAwake ();
	}

	public static ShareContent CreateShareContent()
	{
		return new ShareContent ();
	}

	public void ShareText(string title, string body, Action<string> on_success, Action<int, string> on_fail, Action on_cancel)
	{
		if (!string.IsNullOrEmpty(body))
		{
			ShareContent content = new ShareContent();
			if (!string.IsNullOrEmpty(title))
			{
				content.SetTitle(title);
			}
			content.SetText(body);
			content.SetShareType(ContentType.Text);
			if (shareSDK != null)
			{
				int requestID = shareSDK.ShowPlatformList(m_platformsWithoutShareText, content, 100, 100);
				Callback callback = new Callback();
				callback.requestID = requestID;
				callback.onSuccess = on_success;
				callback.onFail = on_fail;
				callback.onCancel = on_cancel;
				if (m_cachedCallback == null)
					m_cachedCallback = new List<Callback>();
				m_cachedCallback.Add(callback);
			}
		}
	}

	public void ShareText(string title, string body)
	{
		ShareText(title, body, null, null, null);
	}

	public void ShareText(string title, string body, PlatformType platform_type, Action<string> on_success, Action<int, string> on_fail, Action on_cancel)
	{
		if (!string.IsNullOrEmpty(body))
		{
			ShareContent content = new ShareContent();
			if (!string.IsNullOrEmpty(title))
			{
				content.SetTitle(title);
			}
			content.SetText(body);
			content.SetShareType(ContentType.Text);
			if (shareSDK != null)
			{
				int requestID = shareSDK.ShareContent(platform_type, content);

				Callback callback = new Callback();
				callback.requestID = requestID;
				callback.onSuccess = on_success;
				callback.onFail = on_fail;
				callback.onCancel = on_cancel;
				if (m_cachedCallback == null)
					m_cachedCallback = new List<Callback>();
				m_cachedCallback.Add(callback);
			}
		}
	}

	public void ShareText(string title, string body, PlatformType platform_type)
	{
		ShareText(title, body, platform_type, null, null, null);
	}

	public void ShareImage(string imagePath, string title, string body, Action<string> on_success, Action<int, string> on_fail, Action on_cancel)
	{
		if (File.Exists(imagePath))
		{
			ShareContent content = new ShareContent();
			content.SetImagePath(imagePath);
			if (!string.IsNullOrEmpty(title))
			{
				content.SetTitle(title);
			}
			if (!string.IsNullOrEmpty(body))
			{
				content.SetText(body);
			}
			content.SetShareType(ContentType.Image);
			if (shareSDK != null)
			{
				int requestID = shareSDK.ShowPlatformList(m_platforms, content, 100, 100);
				Callback callback = new Callback();
				callback.requestID = requestID;
				callback.onSuccess = on_success;
				callback.onFail = on_fail;
				callback.onCancel = on_cancel;
				if (m_cachedCallback == null)
					m_cachedCallback = new List<Callback>();
				m_cachedCallback.Add(callback);
			}
		}
	}
	
	public void ShareImage(string imagePath, string title, string body)
	{
		ShareImage(imagePath, title, body, null, null, null);
	}

	public static float AndroidWxShareFenZi = 256f;

	public string WXShareImage (string imagePath, int sceneType)
	{
		byte[] fileData = File.ReadAllBytes (imagePath);

		Texture2D tex = new Texture2D ((int)(Screen.width), (int)(Screen.height), TextureFormat.RGB24, true);
		tex.LoadImage (fileData);

		float miniSize = Mathf.Max (tex.width, tex.height);

		float scale = AndroidWxShareFenZi / miniSize;
		if (scale > 1.0f) {
			scale = 1.0f;
		}
		Texture2D temp = ScaleTexture (tex, (int)(tex.width * scale), (int)(tex.height * scale));
		byte[] pngData = temp.EncodeToJPG ();
		string miniImagePath = imagePath.Replace (".jpg", "_min.jpg");
		File.WriteAllBytes (miniImagePath, pngData);
		Destroy (tex);
		Destroy (temp);
		return miniImagePath;
	}
		
	private Texture2D ScaleTexture (Texture2D source, int targetWidth, int targetHeight)
	{
		Texture2D result = new Texture2D (targetWidth, targetHeight, source.format, true);
		Color[] rpixels = result.GetPixels (0);
		float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
		float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);
		for (int px = 0; px < rpixels.Length; px++) {
			rpixels [px] = source.GetPixelBilinear (incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor (px / targetWidth)));
		}
		result.SetPixels (rpixels, 0);
		result.Apply ();
		return result;
	}

	public void ShareImage(string imagePath, string title, string body, PlatformType platform_type, Action<string> on_success, Action<int, string> on_fail, Action on_cancel)
	{
		if (File.Exists(imagePath))
		{
			bool thisIsAndroid  = false;
			bool thisIsWX = false;
			if(platform_type == PlatformType.WeChat||platform_type== PlatformType.WeChatFavorites||platform_type==PlatformType.WeChatMoments)
			{
				thisIsWX = true;
			}

			#if UNITY_ANDROID
				thisIsAndroid = true;
				if (platform_type==PlatformType.WechatPlatform)
				{
				thisIsWX = true;
				}
			#endif
			if (thisIsAndroid == true && thisIsWX==true)
			{
				Dictionary<string, string> content = new Dictionary<string, string> ();
				content.Add ("title",title);  //标题
				content.Add ("description", body);  //描述
				content.Add ("bText", "0");  //描述
				Debug.Log("imagePath>"+imagePath);
				string miniImagePath = WXShareImage ( imagePath, (int)platform_type);
				Debug.Log("miniImagePath>"+miniImagePath);
				content.Add ("thumbPath", miniImagePath); //预览图路径
				content.Add ("imageUrl", imagePath); //图片路径

				if(platform_type == PlatformType.WeChat)
				{
					content.Add ("scene", "SESSION"); //scene场景值
				}else if(platform_type== PlatformType.WeChatFavorites)
				{
					content.Add ("scene", "FAVOURITE"); //scene场景值
				}else if(platform_type== PlatformType.WeChatMoments)
				{
					content.Add ("scene", "TIMELINE"); //scene场景值
				}
				#if UNITY_ANDROID
				else if(platform_type==PlatformType.WechatPlatform)
				{
					content.Add ("scene", "TIMELINE"); //scene场景值
				}
				#endif
				content.Add ("shareType", "IMAGE");  //分享类型 图片
				xdsdk.XDSDK.Share (content);
				Callback callback = new Callback();
				callback.requestID = 0;
				callback.onSuccess = on_success;
				callback.onFail = on_fail;
				callback.onCancel = on_cancel;
				if (m_cachedCallback == null)
					m_cachedCallback = new List<Callback>();
				m_cachedCallback.Add(callback);
			}
			else
			{
				ShareContent content = new ShareContent();
				content.SetImagePath(imagePath);
				if (!string.IsNullOrEmpty(title))
				{
					content.SetTitle(title);
				}
				if (!string.IsNullOrEmpty(body))
				{
					content.SetText(body);
				}
				content.SetShareType(ContentType.Image);
				if (shareSDK != null)
				{
					int requestID = shareSDK.ShareContent(platform_type, content);
					Callback callback = new Callback();
					callback.requestID = requestID;
					callback.onSuccess = on_success;
					callback.onFail = on_fail;
					callback.onCancel = on_cancel;
					if (m_cachedCallback == null)
						m_cachedCallback = new List<Callback>();
					m_cachedCallback.Add(callback);
				}
			}
		}
	}

	public void ShareImage(string imagePath, string title, string body, PlatformType platform_type)
	{
		ShareImage(imagePath, title, body, platform_type, null, null, null);
	}

	public void ShareInternetImage(string image_url, string title, string body, PlatformType platform_type, Action<string> on_success, Action<int, string> on_fail, Action on_cancel)
	{
		ShareContent content = new ShareContent();
		content.SetImageUrl(image_url);
		if (!string.IsNullOrEmpty(title))
		{
			content.SetTitle(title);
		}
		if (!string.IsNullOrEmpty(body))
		{
			content.SetText(body);
		}
		content.SetShareType(ContentType.Image);
		if (shareSDK != null)
		{
			int requestID = shareSDK.ShareContent(platform_type, content);
			Callback callback = new Callback();
			callback.requestID = requestID;
			callback.onSuccess = on_success;
			callback.onFail = on_fail;
			callback.onCancel = on_cancel;
			if (m_cachedCallback == null)
				m_cachedCallback = new List<Callback>();
			m_cachedCallback.Add(callback);
		}
	}

	public void ShareInternetImage(string image_url, string title, string body, PlatformType platform_type)
	{
		ShareInternetImage(image_url, title, body, platform_type, null, null, null);
	}

	public void ShareHyperlink(string url, string body, Action<string> on_success, Action<int, string> on_fail, Action on_cancel)
	{
		if (!string.IsNullOrEmpty(url))
		{
			ShareContent content = new ShareContent();
			content.SetUrl(url);
			if (!string.IsNullOrEmpty(body))
			{
				content.SetText(body);
			}
			content.SetShareType(ContentType.Webpage);
			if (shareSDK != null)
			{
				int requestID = shareSDK.ShowPlatformList(m_platforms, content, 100, 100);
				Callback callback = new Callback();
				callback.requestID = requestID;
				callback.onSuccess = on_success;
				callback.onFail = on_fail;
				callback.onCancel = on_cancel;
				if (m_cachedCallback == null)
					m_cachedCallback = new List<Callback>();
				m_cachedCallback.Add(callback);
			}
		}
	}

	public void ShareHyperlink(string url, string body)
	{
		ShareHyperlink(url, body, null, null, null);
	}

	public void ShareHyperlink(string url, string body, PlatformType platform_type, Action<string> on_success, Action<int, string> on_fail, Action on_cancel)
	{
		if (!string.IsNullOrEmpty(url))
		{
			ShareContent content = new ShareContent();
			content.SetUrl(url);
			if (!string.IsNullOrEmpty(body))
			{
				content.SetText(body);
			}
			content.SetShareType(ContentType.Webpage);
			if (shareSDK != null)
			{
				int requestID = shareSDK.ShareContent(platform_type, content);
				Callback callback = new Callback();
				callback.requestID = requestID;
				callback.onSuccess = on_success;
				callback.onFail = on_fail;
				callback.onCancel = on_cancel;
				if (m_cachedCallback == null)
					m_cachedCallback = new List<Callback>();
				m_cachedCallback.Add(callback);
			}
		}
	}

	public void ShareHyperlink(string url, string body, PlatformType platform_type)
	{
		ShareHyperlink(url, body, platform_type, null, null, null);
	}

	public void ShareResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		// debug
		Debug.Log(string.Format("SocialShare:ShareResultHandler\nreqID:{0}\nstate:{1}\nplatformType:{2}\nresult:{3}", reqID, state, type, MiniJSON.jsonEncode(result)));

		Callback callback = null;
		if (m_cachedCallback != null && m_cachedCallback.Count > 0)
		{
			callback = m_cachedCallback.Find(x => x.requestID == reqID);
		}

		if (state == ResponseState.Success)
		{
			string strResult = MiniJSON.jsonEncode(result);
			if (callback != null)
			{
				if (callback.onSuccess != null)
				{
					callback.onSuccess(strResult);
				}
			}
		}
		else if (state == ResponseState.Fail)
		{
			int errorCode = -1;
			object objErrCode = result["error_code"];
			if (objErrCode != null)
			{
				int.TryParse(objErrCode.ToString(), out errorCode);
			}
			string errorMessage = "";
			object objErrMsg = result["error_msg"];
			if (objErrMsg != null)
			{
				errorMessage = objErrMsg.ToString();
			}
			if (string.IsNullOrEmpty(errorMessage))
			{
				objErrMsg = result["msg"];
				if (objErrMsg != null)
				{
					errorMessage = objErrMsg.ToString();
				}
			}
			if (callback != null)
			{
				if (callback.onFail != null)
				{
					callback.onFail(errorCode, errorMessage);
				}
			}
		}
		else if (state == ResponseState.Cancel)
		{
			if (callback != null)
			{
				if (callback.onCancel != null)
				{
					callback.onCancel();
				}
			}
		}

		m_cachedCallback.Remove (callback);
	}

	public bool IsClientValid(PlatformType platform_type)
	{
		return shareSDK.IsClientValid (platform_type);
	}

	public void CancelAuthorize(PlatformType platform_type)
	{
		shareSDK.CancelAuthorize (platform_type);
	}

	void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			if (result != null && result.Count > 0) {
				print ("authorize success !" + "Platform :" + type + "result:" + MiniJSON.jsonEncode(result));
			} else {
				print ("authorize success !" + "Platform :" + type);
			}
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnGetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("get user info result :");
			print (MiniJSON.jsonEncode(result));
			print ("AuthInfo:" + MiniJSON.jsonEncode (shareSDK.GetAuthInfo (PlatformType.QQ)));
			print ("Get userInfo success !Platform :" + type );
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnShareResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("share successfully - share result :");
			print (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnGetFriendsResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{			
			print ("get friend list result :");
			print (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnFollowFriendResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("Follow friend successfully !");
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

}
