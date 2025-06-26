using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using LitJson;
using System.Net.Sockets;
using System.Net;
using System.Text;
using CloudVoiceVideoTroops;

[SLua.CustomLuaClassAttribute]
public class ROVoice {
	private static string filePath = string.Empty;
	private static string filePathUrl = string.Empty;
	private static int duration=0;
	public delegate void CallBack(string msg);


	public static void ChatSDKInit(int environment, string appId, int voiceModeCallBack,CallBack mCallBack)
	{
		VideoTroopsAPI.Instance.Init();
		VideoTroopsAPI.Instance.ChatSDKInit(environment,appId,voiceModeCallBack, (msg) =>
			{
				mCallBack(msg);
				VideoTroopsAPI.Instance.SetLogLevel (0);
			});
	}

	public static void  ChatSDKLogin(string userId, string roomId,CallBack mCallBack)
	{
		VideoTroopsAPI.Instance.ChatSDKLogin(userId, roomId, (msg) =>
            {
				mCallBack(msg);
            });
	}

	public static void  Logout(CallBack mCallBack)
	{
        VideoTroopsAPI.Instance.Logout((msg) =>
            {
                mCallBack(msg);
            });
	}

	public static void  ChatMic(bool mic,CallBack mCallBack)
	{
		VideoTroopsAPI.Instance.ChatMic(mic, (msg) =>
            {
                mCallBack(msg);
            });
	}

	public static void SetPausePlayRealAudio(bool isPause)
	{
		VideoTroopsAPI.Instance.SetPausePlayRealAudio(isPause);
	}

	public static void AudioToolsPlayAudio(CallBack mCallBack)
	{
		VideoTroopsAPI.Instance.AudioToolsPlayAudio(filePath, (data1) =>
			{
				mCallBack(data1);
			});
	}

	public static void AudioToolsStartRecord(CallBack mCallBack,CallBack mCallBack2,CallBack mCallBack3,CallBack mCallBack4)
	{
		VideoTroopsAPI.Instance.AudioToolsStartRecord(2,(msg) =>
			{
				mCallBack(msg);
				JsonData data = JsonMapper.ToObject(msg);
                filePath = JsonHelp.getStringValue(data, "filePath");
                duration = JsonHelp.getIntValue(data, "voiceDuration");
			},
			(data1)=>
			{
				mCallBack2(data1);
				JsonData dataUrl = JsonMapper.ToObject(data1);
				filePathUrl = JsonHelp.getStringValue(dataUrl, "voiceUrl");
			},
			(data2)=>
			{
				mCallBack3(data2);
				JsonData dataobject = JsonMapper.ToObject(data2);
				string text = JsonHelp.getStringValue(dataobject, "text");
				string voiceUrl = JsonHelp.getStringValue(dataobject, "voiceUrl");
				VideoTroopsAPI.Instance.AudioToolsPlayOnlineAudio(voiceUrl, (data1) =>
				{
					mCallBack4(data1);
				});
			});
	}

	public static void AudioToolsStopRecord()
	{
		VideoTroopsAPI.Instance.AudioToolsStopRecord();
	}

	public static void AudioToolsPlayOnlineAudio(CallBack mCallBack)
	{
		VideoTroopsAPI.Instance.AudioToolsPlayOnlineAudio(filePathUrl, (data1) => {
			mCallBack(data1);
		});
	}

	public static void ReceiveTextMessageNotify(CallBack mCallBack,CallBack mCallBack2,CallBack getuserId)
	{
		CvEventListenerManager.AddListener(CvListenerEven.ReceiveTextMessageNotify, (obj) =>
        {
            string sjson = obj.ToString();
            mCallBack(sjson);
            JsonData data = JsonMapper.ToObject(obj.ToString());
            int msgType = JsonHelp.getIntValue(data, "msgType");
            string textMsg = JsonHelp.getStringValue(data, "textMsg");
            string voiceUrl = JsonHelp.getStringValue(data, "voiceUrl");
            string userId = JsonHelp.getStringValue(data, "userId");
			getuserId(userId);
            if (msgType == 2 || msgType == 3)
            {
                VideoTroopsAPI.Instance.AudioToolsPlayOnlineAudio(voiceUrl, (data1) =>
                {
					mCallBack2(data1);
                });
            }
        });
	}



	public static void LoginNotify(CallBack mCallBack)
	{
		CvEventListenerManager.AddListener(CvListenerEven.LoginNotify, (obj) =>
        {
            mCallBack(obj.ToString());
        });
	}

	public static void LogoutNotify(CallBack mCallBack)
	{
		CvEventListenerManager.AddListener(CvListenerEven.LogoutNotify, (obj) =>
        {
			mCallBack(obj.ToString());
        });
	}
	//===
	public static XDSDKCallback _SendRealTimeVoiceMessageErrorNotify;

	public static XDSDKCallback _ReceiveRealTimeVoiceMessageNofify;

	public static XDSDKCallback _ReceiveTextMessageNotify;

	public static XDSDKCallback _RecorderMeteringPeakPowerNotify;

	public static XDSDKCallback _PlayMeteringPeakPowerNotify;

	public static XDSDKCallback _AudioToolsRecorderMeteringPeakPowerNotify;

	public static XDSDKCallback _AudioToolsPlayMeteringPeakPowerNotify;

	public static XDSDKCallback _MicStateNotify;

	public static XDSDKCallback _onConnectFail;

	public static XDSDKCallback _onReconnectSuccess;

	public static XDSDKCallback _LoginNotify;

	public static XDSDKCallback _LogoutNotify;

}
