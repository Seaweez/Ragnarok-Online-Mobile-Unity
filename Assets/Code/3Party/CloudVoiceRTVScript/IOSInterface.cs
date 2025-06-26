using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace CloudVoiceVideoTroops
{
	public class IOSInterface {

		#region 实时语音部份

		[DllImport("__Internal")]
		public static extern void ChatSDKInitWithenvironment(int environment,string appId,string observer);
	
		[DllImport("__Internal")]
        public static extern void ChatSDKLogin(string userId, string roomId);

		[DllImport("__Internal")]
		public static extern void logout();

		[DllImport("__Internal")]
		public static extern void releaseResource();

		[DllImport("__Internal")]
		public static extern void chatMic(bool onOff, int timeLimit);

		[DllImport("__Internal")]
		public static extern void setPausePlayRealAudio(bool isPause);

		[DllImport("__Internal")]
		public static extern void SetLogLevel(int logLevel);

		/**获取当前是否是上麦状态**/
		[DllImport("__Internal")]
		public static extern bool getCurrentMicState();

        [DllImport("__Internal")]
		public static extern void setMeteringEnabled(bool enabled);

		#endregion

		#region 发送消息接口部份
		/**发送文本信息*/
		[DllImport("__Internal")]
		public static extern void sendMessageWithType(int type, string text, string voiceUrl,int vocieDuration, string expand);
		#endregion

		#region

        [DllImport("__Internal")]
        public static extern void speechDiscernByUrl(int recognizeLanguage,
                                                     int outputTextLanguageType, 
                                                     string UrlFilePath, 
                                                     string expand);
		#region 录音工具
		/*!
    	 @method
     	@brief 初始化
     	@param observer 回调接收对象
     	*/
		[DllImport("__Internal")]
		public static extern void audioTools_Init (string observer);

//		/*!
//     	@method
//    	@brief 开始语音录制
//     	@param minMillseconds 识别录音最短时间(录音少于该时间不做处理)
//     	@param maxMillseconds 录音最长时间(超过该时间会自动停止录制)
//     	*/
		[DllImport("__Internal")]
		public static extern void audioTools_startRecord(int minMillseconds, int maxMillseconds,bool isRecognize,int language);

//		/*!
//    	 @method
//     	@brief 停止语音录制
//     	*/
		[DllImport("__Internal")]
		public static extern void audioTools_stopRecord();

//		/*!
//     	@method
//     	@brief 查询是否正在录制
//     	*/
		[DllImport("__Internal")]
		public static extern bool audioTools_isRecording();

//		/*!
//     	@method
//     	@brief 播放语音文件
//     	@param 语音文件绝对路径
//     	*/
		[DllImport("__Internal")]
		public static extern int audioTools_playAudio(string filePath);

//		/*!
//     	@method
//     	@brief 在线播放语音
//     	@param 语音文件下载url
//     	*/
		[DllImport("__Internal")]
		public static extern int audioTools_playOnlineAudio(string fileUrl);

//		/*!
//     	@method
//     	@brief 主动停止语音播放
//     	*/
		[DllImport("__Internal")]
		public static extern void audioTools_stopPlayAudio();

//		/*!
//     	@method
//     	@brief 查询当前是否正在播放
//     	*/
		[DllImport("__Internal")]
		public static extern bool audioTools_isPlaying();

		[DllImport("__Internal")]
		public static extern  bool audioTools_deleteFile(string filePath);

        [DllImport("__Internal")]
        public static extern bool audioTools_setMeteringEnabled(bool filePath);

        [DllImport("__Internal")]
        public static extern int addShieldWithUserId(string userId);

        [DllImport("__Internal")]
        public static extern int removeShieldWithUserId(string userId);

		#endregion
		#endregion
       }
}
