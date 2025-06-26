//#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using CloudVoiceVideoTroops;

namespace CloudVoiceVideoTroops
{
	public class AndroidInterface : MonoBehaviour
	{
		private   AndroidJavaObject _plugin = null;

		public AndroidInterface()
		{
            using (AndroidJavaClass jpushClass = new AndroidJavaClass("com.cloudvoice.udprealtime.u3d.api.ChatManageUnity3dExtension"))
			{
                _plugin = jpushClass.CallStatic<AndroidJavaObject>("Instance");
			}
		}


	    //初始化接口
		public void ChatSDKInit(int env,string appId,int mPlayMode,string gameObjectName)
		{
			if (_plugin == null) return;
			_plugin.Call("ChatSDKInit", env, appId,mPlayMode, gameObjectName);
		}

	    //登录
	    public void ChatSDKLogin(string userId, string roomId)
	    {
			if (_plugin == null) return;
            _plugin.Call("ChatSDKLogin", userId, roomId);
	    }

	    //登出
	    public void logout()
	    {
			if (_plugin == null) return;
            _plugin.Call("logout");
	    }

	    //实时语音上麦，下麦
        public void ChatMic(bool onOff)
	    {
			if (_plugin == null) return;
            _plugin.Call("ChatMic", onOff);
	    }
        /**
     *  设置是否暂停播放实时语音聊天
     *
     *  @param isPause       true:暂停实时播放  false:恢复实时播放
     */
        public void setPausePlayRealAudio(bool isPause)
        {
            if (_plugin == null) return;
            _plugin.Call("setPausePlayRealAudio", isPause);
        }

		public void sendMessage(string text, string url,int msgType,string expand)
		{
			if (_plugin == null) return;
            _plugin.Call("sendMessage", text, url, msgType, expand);
		}

		public void AudioToolsStartRecord(int mode)
		{
            if (_plugin == null) return;
			_plugin.Call ("startRecord", mode);
		}

		public void AudioToolsStopRecord()
		{
			if (_plugin == null) return;
            _plugin.Call("stopRecord");
		}

		public bool AudioToolsIsRecording()
		{
			if (_plugin == null) return false;
            return _plugin.Call<bool>("isRecording");
		}

        public bool audioToolsisPlaying()
        {
            if (_plugin == null) return false;
            return _plugin.Call<bool>("isPlaying");
        }
   
		public void  AudioToolsPlayAudio(string filePath)
		{
			if (_plugin == null) return;
            _plugin.Call("startPlayVoice", filePath);
		}

		public void audioToolsPlayOnlineAudio(string fileUrl)
		{
			if (_plugin == null) return;
            _plugin.Call("startPlayVoiceByUrl", fileUrl);
		}

		public void AudioToolsStopPlayAudio()
		{
			if (_plugin == null) return;
            _plugin.Call("stopPlayVoice");
		}

        public void startPull(string userId)
        {
            if (_plugin == null) return;
            _plugin.Call("startPull", userId);
        }

        public void stopPull(string userId)
        {
            if (_plugin == null) return;
            _plugin.Call("stopPull", userId);
        }
	}
}
//#endif