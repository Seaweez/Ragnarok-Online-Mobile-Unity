using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public class ChannelInfo
{
	#if UNITY_ANDROID && !UNITY_EDITOR
	private static AndroidJavaObject m_currentActivity;
	private static AndroidJavaObject CurrentActivity
	{
		get
		{
			if (m_currentActivity == null)
			{
				AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				m_currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			}
			return m_currentActivity;
		}
	}
	#endif

	private static string[] m_keyValuePairFromApkTool;

	public static string GetChannelID()
	{
		string ret = null;
		#if UNITY_ANDROID && !UNITY_EDITOR
		if (m_keyValuePairFromApkTool == null) {
			m_keyValuePairFromApkTool = CurrentActivity.Call<string[]>("GetAllKeyValueFromApkToolWriting");
		}
		if (m_keyValuePairFromApkTool != null)
		{
			for (int i = 0; i < m_keyValuePairFromApkTool.Length; i = i + 2) {
				string key = m_keyValuePairFromApkTool [i];
				if (string.Equals (key, "channelID")) {
					int valueIndex = i + 1;
					if (valueIndex < m_keyValuePairFromApkTool.Length) {
						ret = m_keyValuePairFromApkTool [valueIndex];
					}
					break;
				}
			}
		}
		#endif
		return ret;
	}
}
