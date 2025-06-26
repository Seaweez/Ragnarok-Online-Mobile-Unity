using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Threading;
using UnityEngine;
using CloudVoiceVideoTroops;

namespace CloudVoiceVideoTroops
{
    public enum PathType
    {
        filePath,
        urlPath,
    }
	public class VideoTroopsAPI:MonoSingleton<VideoTroopsAPI>
	{

		public static VideoTroopsAPI Instance {
			get { return ins;}
		}
		
		#if UNITY_ANDROID
		    private static AndroidInterface androidInterface;
		#endif

		public void Init()
		{
			//base.Init ();
			DontDestroyOnLoad (this);
			#if UNITY_ANDROID
			androidInterface = new AndroidInterface();
			#endif
		}
	
		//SDK初始化
		private System.Action<string> InitResp;
        public void ChatSDKInit(int environment, string appId, int voiceMode=1,System.Action<string> response = null)
		{
            CloudVoiceLogPrint.DebugLog("ChatSDKInit", string.Format("appId:{0},gameObjectName:{1}", appId, this.gameObject.name));
			InitResp = response;
			#if UNITY_IOS && !UNITY_EDITOR
			IOSInterface.ChatSDKInitWithenvironment (environment,appId, this.gameObject.name);
            #elif UNITY_ANDROID
            androidInterface.ChatSDKInit(environment, appId, voiceMode, this.gameObject.name);
			#endif
		}

		private System.Action<String> LoginResp;
		//登录
        public void ChatSDKLogin(string userId, string roomId, System.Action<string> response = null)
		{
            CloudVoiceLogPrint.DebugLog("ChatSDKLogin", string.Format("userId:{0},roomId:{1}", userId, roomId));
			LoginResp = response;
			#if UNITY_IOS
            IOSInterface.ChatSDKLogin(userId, roomId);
			#elif UNITY_ANDROID
            androidInterface.ChatSDKLogin(userId, roomId);
			#endif
		}

		//登出
		private System.Action<String> LogoutResp;
		public void Logout(System.Action<String> response = null)
		{
            CloudVoiceLogPrint.DebugLog("Logout", "");
			LogoutResp = response;
			#if UNITY_IOS
			IOSInterface.logout();
			#elif UNITY_ANDROID
			androidInterface.logout();
			#endif
		}

		//上，下麦操作
		private System.Action<String> ChatMicResp;
		public void ChatMic(bool onOff,System.Action<String> response = null)
		{
            CloudVoiceLogPrint.DebugLog("ChatMic", string.Format("onOff:{0}", onOff));
			ChatMicResp = response;
			#if UNITY_IOS
			IOSInterface.chatMic(onOff,0);
			#elif UNITY_ANDROID
			androidInterface.ChatMic(onOff);
			#endif
		}


		//暂停播放实时语音
		public void SetPausePlayRealAudio(bool isPause)
		{
            CloudVoiceLogPrint.DebugLog("SetPausePlayRealAudio", string.Format("isPause:{0}", isPause));
			#if UNITY_IOS
			IOSInterface.setPausePlayRealAudio(isPause);
			#elif UNITY_ANDROID
			androidInterface.setPausePlayRealAudio(isPause);
			#endif
		}

        //暂停播放某用户声音
        public void PausePlayRealAudioWithUserId(string userId)
        {
            CloudVoiceLogPrint.DebugLog("PausePlayRealAudioWithUserId", string.Format("userId:{0}", userId));
            #if UNITY_IOS
             IOSInterface.addShieldWithUserId(userId);   
            #elif UNITY_ANDROID
            androidInterface.stopPull(userId);
            #endif
        }

        //开始播放某用户声音
        public void StartPlayRealAudioWithUserId(string userId)
        {
            CloudVoiceLogPrint.DebugLog("StartPlayRealAudioWithUserId", string.Format("userId:{0}", userId));
            #if UNITY_IOS
               IOSInterface.removeShieldWithUserId(userId);
            #elif UNITY_ANDROID
            androidInterface.startPull(userId);
            #endif
        }

		//发送消息
		private System.Action<String> SendTextMessageResp;
		public void SendTextMessage(int type, string text, string voiceUrl,string expand, System.Action<string> response = null)
		{
            CloudVoiceLogPrint.DebugLog("SendTextMessage", string.Format("type:{0},text:{1},voiceUrl:{2},expand:{3}", type, text, voiceUrl, expand));
			SendTextMessageResp = response;
			#if UNITY_IOS
            IOSInterface.sendMessageWithType(type, text, voiceUrl, 1200, expand);
			#elif UNITY_ANDROID
            androidInterface.sendMessage(text, voiceUrl, type, expand);
			#endif
		}

        /**
        *  获取当前上下麦状态
        */
        public bool getMicUpOrDown()
        {
            CloudVoiceLogPrint.DebugLog("getMicUpOrDown", string.Format(""));
			#if UNITY_IOS
			return IOSInterface.getCurrentMicState();
			#elif UNITY_ANDROID
               // return androidInterface.getMicUpOrDown();
            #endif
                return false;
        }

//		//声音工具初始化
		public void AudioToolsInit()
		{
            CloudVoiceLogPrint.DebugLog("AudioToolsInit", string.Format("gameObjectName:{0}", this.gameObject.name));
			#if UNITY_IOS
			    IOSInterface.audioTools_Init (this.gameObject.name);
			#elif UNITY_ANDROID

			#endif
		}

//		//开始录音
		private System.Action<string> audioToolsStopRecordResp;
		private System.Action<string> VoiceRecognizeReqAction;
		private System.Action<string> UploadVoiceAction;

        public void AudioToolsStartRecord(int recordMode, System.Action<string> response = null, System.Action<string> UploadVoiceresponse = null, System.Action<string> VoiceRecognizeresponse = null)
		{
            //CloudVoiceLogPrint.DebugLog("AudioToolsStartRecord", string.Format("storeDataFilePath:{0},minMillSeconds:{1},maxMillSeconds:{2}", storeDataFilePath, minMillSeconds, maxMillSeconds));
			audioToolsStopRecordResp = response;
			VoiceRecognizeReqAction = VoiceRecognizeresponse;
			UploadVoiceAction = UploadVoiceresponse;
			#if UNITY_IOS
            bool isRecognize = true;
            if (recordMode == 1)
            {
                isRecognize = false;
            }
            else
            {
                isRecognize = true;
            }
			IOSInterface.audioTools_startRecord (0, 60000,isRecognize,0);
			#elif UNITY_ANDROID
            androidInterface.AudioToolsStartRecord(recordMode);
			#endif
		}
//
//		//停止录音
		public void AudioToolsStopRecord()
		{
            CloudVoiceLogPrint.DebugLog("AudioToolsStopRecord", "");
			#if UNITY_IOS
			IOSInterface.audioTools_stopRecord ();
			#elif UNITY_ANDROID
			androidInterface.AudioToolsStopRecord ();
			#endif
		}
//
//		//判断是否正在录音
		public bool AudioToolsIsRecording()
		{
            CloudVoiceLogPrint.DebugLog("AudioToolsIsRecording", "");
			#if UNITY_IOS
			return IOSInterface.audioTools_isRecording ();
			#elif UNITY_ANDROID
			return androidInterface.AudioToolsIsRecording ();
			#else
			return false;
			#endif
		}

//		//播放本地录音文件
		private System.Action<string> audioToolsPlayAudioFinishResp;
		public void  AudioToolsPlayAudio(string filePath ,System.Action<string> response = null)
		{
            CloudVoiceLogPrint.DebugLog("AudioToolsPlayAudio", string.Format("filePath:{0}", filePath));
			audioToolsPlayAudioFinishResp = response;
			#if UNITY_IOS
		    IOSInterface.audioTools_playAudio (filePath);
			#elif UNITY_ANDROID
		    androidInterface.AudioToolsPlayAudio (filePath);
			#endif
		}

//		//在线播放录音文件
		public void AudioToolsPlayOnlineAudio(string fileUrl ,System.Action<string> response = null)
		{
            CloudVoiceLogPrint.DebugLog("AudioToolsPlayOnlineAudio", string.Format("fileUrl:{0}", fileUrl));
			audioToolsPlayAudioFinishResp = response;
			#if UNITY_IOS
		    IOSInterface.audioTools_playOnlineAudio (fileUrl);
			#elif UNITY_ANDROID
		    androidInterface.audioToolsPlayOnlineAudio (fileUrl);
			#endif
		}
//
//		//停止播放录音文件
		public void AudioToolsStopPlayAudio()
		{
            CloudVoiceLogPrint.DebugLog("AudioToolsStopPlayAudio", "");
			#if UNITY_IOS
			IOSInterface.audioTools_stopPlayAudio ();
			#elif UNITY_ANDROID
			androidInterface.AudioToolsStopPlayAudio ();
			#endif
		}

//		//录音文件是否正在播放
		public bool AudioToolsisPlaying()
		{
            CloudVoiceLogPrint.DebugLog("AudioToolsisPlaying", "");
			#if UNITY_IOS
			return IOSInterface.audioTools_isPlaying ();
			#elif UNITY_ANDROID
			return androidInterface.audioToolsisPlaying ();
			#else
			return false;
			#endif
		}

        /**设置日志级别:0--关闭日志  1--error  2--debug（不设置为默认该级别） 3--warn  4--info  5--trace*/
		public void SetLogLevel(int level)
		{
            CloudVoiceLogPrint.DebugLog("SetLogLevel", string.Format("level:{0}", level));
			#if UNITY_IOS
			IOSInterface.SetLogLevel (level);
			#elif UNITY_ANDROID
			//androidInterface.SetLogLevel(level);
			#endif
		}

        public void AudioToolsSetMeteringEnabled(bool enable)
        {
            CloudVoiceLogPrint.DebugLog("AudioToolsSetMeteringEnabled", string.Format("enable:{0}", enable));
            #if UNITY_IOS
            IOSInterface.audioTools_setMeteringEnabled (enable);
			IOSInterface.setMeteringEnabled(enable);
            #elif UNITY_ANDROID

            #endif
        }

		#region  CallBack
		public void onSDKInitDidFinish (string msg) 
		{
            CloudVoiceLogPrint.DebugLog("onSDKInitDidFinish", string.Format("JsonMsg:{0}", msg));
            AudioToolsInit();
            #if UNITY_IOS
                //AudioToolsSetMeteringEnabled(true);
            #endif
         
			if (InitResp != null) {
				InitResp (msg);
				InitResp = null;
			}
		}

		public void onLoginResp (string msg) 
		{
            CloudVoiceLogPrint.DebugLog("onLoginResp", string.Format("JsonMsg:{0}", msg));
			if (LoginResp != null) {
				LoginResp (msg);
				LoginResp = null;
			}
		}

		public void onLogoutResp (string msg) 
		{
            CloudVoiceLogPrint.DebugLog("onLogoutResp", string.Format("JsonMsg:{0}", msg));
			if (LogoutResp != null) {
				LogoutResp (msg);
				LogoutResp = null;
			}
		}

		public void onChatMicResp (string msg) 
		{
            CloudVoiceLogPrint.DebugLog("onChatMicResp", string.Format("JsonMsg:{0}", msg));
			if (ChatMicResp != null) {
				ChatMicResp (msg);
				ChatMicResp = null;
			}
		}

		public void onSendRealTimeVoiceMessageError (string msg) 
		{
            CloudVoiceLogPrint.DebugLog("onSendRealTimeVoiceMessageError", string.Format("JsonMsg:{0}", msg));
			CvEventListenerManager.Invoke (CvListenerEven.SendRealTimeVoiceMessageErrorNotify, (object)msg);
		}

		//发送消息回调 msg: {"result":(int)result, "msg":(string)msg, "expand":
		public void onSendTextMessageResp(string msg)
		{
            CloudVoiceLogPrint.DebugLog("onSendTextMessageResp", string.Format("JsonMsg:{0}", msg));
			if (SendTextMessageResp != null) {
				SendTextMessageResp (msg);
				SendTextMessageResp = null;
			}
		}

		//收到消息通知
		public void onTextMessageNotify(string msg)
		{
            CloudVoiceLogPrint.DebugLog("onTextMessageNotify", string.Format("JsonMsg:{0}", msg));
			CvEventListenerManager.Invoke (CvListenerEven.ReceiveTextMessageNotify, msg);
		}

        //房间里其他用户登录通知
        public void onLoginNotify(string msg)
        {
            CloudVoiceLogPrint.DebugLog("onLoginNotify", string.Format("JsonMsg:{0}", msg));
            CvEventListenerManager.Invoke(CvListenerEven.LoginNotify, msg);
        }

        //房间里其他用户登出通知
        public void onLogoutNotify(string msg)
        {
            CloudVoiceLogPrint.DebugLog("onLogoutNotify", string.Format("JsonMsg:{0}", msg));
            CvEventListenerManager.Invoke(CvListenerEven.LogoutNotify, msg);
        }

		//上传语音文件回调 
			public void onUploadVoiceResp(string msg)
		{
			CloudVoiceLogPrint.DebugLog("onUploadVoiceResp", string.Format("JsonMsg:{0}", msg));

			if (UploadVoiceAction != null) 
			{
			UploadVoiceAction (msg);
			UploadVoiceAction = null;
			}
		}

		//语音识别
			public void onRecognizeResp(string msg)
		{
			CloudVoiceLogPrint.DebugLog("onRecognizeResp", string.Format("JsonMsg:{0}", msg));
			if (VoiceRecognizeReqAction != null) 
			{
				VoiceRecognizeReqAction (msg);
				VoiceRecognizeReqAction = null;
			}
		}

		//录制结束回调  
		public void onRecordCompleteNotify(string msg)
		{
            CloudVoiceLogPrint.DebugLog("onRecordCompleteNotify", string.Format("JsonMsg:{0}", msg));
			if (audioToolsStopRecordResp != null) {
				audioToolsStopRecordResp (msg);
			}
		}

		//播放结束通知 
		public void onPlayAudioCompleteNotify(string msg)
		{
            CloudVoiceLogPrint.DebugLog("onPlayAudioCompleteNotify", string.Format("JsonMsg:{0}", msg));
			if (audioToolsPlayAudioFinishResp != null) {
				audioToolsPlayAudioFinishResp (msg);
			}
		}
		#endregion

		public void SendRealTimeVoiceMessageErrorNotify(string msg)
		{
			if (ROVoice._SendRealTimeVoiceMessageErrorNotify != null) {
				ROVoice._SendRealTimeVoiceMessageErrorNotify (msg);
			}
		}

		public void ReceiveRealTimeVoiceMessageNofify(string msg)
		{
			if (ROVoice._ReceiveRealTimeVoiceMessageNofify != null) {
				ROVoice._ReceiveRealTimeVoiceMessageNofify (msg);
			}
		}

		public void ReceiveTextMessageNotify(string msg)
		{
			if (ROVoice._ReceiveTextMessageNotify != null) {
				ROVoice._ReceiveTextMessageNotify (msg);
			}
		}

		public void RecorderMeteringPeakPowerNotify(string msg)
		{
			if (ROVoice._RecorderMeteringPeakPowerNotify != null) {
				ROVoice._RecorderMeteringPeakPowerNotify (msg);
			}	
		}

		public void PlayMeteringPeakPowerNotify(string msg)
		{
			if (ROVoice._PlayMeteringPeakPowerNotify != null) {
				ROVoice._PlayMeteringPeakPowerNotify (msg);
			}
		}

		public void AudioToolsRecorderMeteringPeakPowerNotify(string msg)
		{
			if (ROVoice._AudioToolsRecorderMeteringPeakPowerNotify != null) {
				ROVoice._AudioToolsRecorderMeteringPeakPowerNotify (msg);
			}
		}

		public void AudioToolsPlayMeteringPeakPowerNotify(string msg)
		{
			if (ROVoice._AudioToolsPlayMeteringPeakPowerNotify != null) {
				ROVoice._AudioToolsPlayMeteringPeakPowerNotify (msg);
			}
		}


		public void MicStateNotify(string msg)
		{
			if (ROVoice._MicStateNotify != null) {
				ROVoice._MicStateNotify (msg);
			}
		}


		public void onConnectFail(string msg)
		{
			if (ROVoice._onConnectFail != null) {
				ROVoice._onConnectFail (msg);
			}
		}

		public void onReconnectSuccess(string msg)
		{
			if (ROVoice._onReconnectSuccess != null) {
				ROVoice._onReconnectSuccess (msg);
			}
		}

		public void LoginNotify(string msg)
		{
			if (ROVoice._LoginNotify != null) {
				ROVoice._LoginNotify (msg);
			}
		}

		public void LogoutNotify(string msg)
		{
			if (ROVoice._LogoutNotify != null) {
				ROVoice._LogoutNotify (msg);
			}
		}

	}
}


