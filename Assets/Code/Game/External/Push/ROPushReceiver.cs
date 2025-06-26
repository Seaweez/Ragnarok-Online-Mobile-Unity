using UnityEngine;
using System.Collections;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class ROPushReceiver : MonoBehaviour {

		public static ROPushReceiver _Instance;


		public static ROPushReceiver Instance
		{
			get{ 
				if (_Instance == null) {
					GameObject go = new GameObject ();
					_Instance = go.AddComponent<ROPushReceiver> ();
				}
				return _Instance;
			}
		}

		public XDSDKCallback _OnReceiveNotification;
		public XDSDKCallback _OnReceiveMessage;
		public XDSDKCallback _OnOpenNotification;

		public XDSDKCallback _OnJPushTagOperateResult;
		public XDSDKCallback _OnJPushAliasOperateResult;


		void Awake()
		{
			_Instance = this;
			this.gameObject.name = "ROPushReceiver";
		}
		/**
	     * {
	     *	"title": "notiTitle",
	     *   "content": "content",
	     *   "extras": {
	     *		"key1": "value1",
	     *       "key2": "value2"
	     * 	}
	     * }
	     */
		// 获取的是 json 格式数据，开发者根据自己的需要进行处理。
		void OnReceiveNotification(string jsonStr)
		{
			if (_OnReceiveNotification != null) {
				_OnReceiveNotification (jsonStr);
			} else {
				Debug.Log("jpush _OnReceiveNotification == null");
			}
		}

		/* data format
	     {
	        "message": "hhh",
	        "extras": {
	            "f": "fff",
	            "q": "qqq",
	            "a": "aaa"
	        }
	     }
	     */
		// 开发者自己处理由 JPush 推送下来的消息。
		void OnReceiveMessage(string jsonStr)
		{
			if (_OnReceiveMessage != null) {
				_OnReceiveMessage (jsonStr);
			} else {
				Debug.Log("jpush _OnReceiveMessage == null");
			}
		}

		//开发者自己处理点击通知栏中的通知
		void OnOpenNotification(string jsonStr)
		{
			if (_OnOpenNotification != null) {
				_OnOpenNotification (jsonStr);
			} else {
				Debug.Log("jpush _OnOpenNotification == null");
			}
		}

		/// <summary>
		/// JPush 的 tag 操作回调。
		/// </summary>
		/// <param name="result">操作结果，为 json 字符串。</param>
		void OnJPushTagOperateResult(string result)
		{
			if (_OnJPushTagOperateResult != null) {
				_OnJPushTagOperateResult (result);
			} else {
				Debug.Log("jpush _OnJPushTagOperateResult == null");
			}
		}

		/// <summary>
		/// JPush 的 alias 操作回调。
		/// </summary>
		/// <param name="result">操作结果，为 json 字符串。</param>
		void OnJPushAliasOperateResult(string result)
		{
			if (_OnJPushAliasOperateResult != null) {
				_OnJPushAliasOperateResult (result);
			} else {
				Debug.Log("jpush _OnJPushAliasOperateResult == null");
			}
		}
	}
}
