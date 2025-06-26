using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using CloudVoiceVideoTroops;
using LitJson;
using System.Net.Sockets;
using System.Net;
using System.Text;


public class TestVoice:MonoBehaviour
{
	public GUISkin guiSkin;
	public GameObject cube;
	private string sModeType = "消息类型:1文字消息，2语音url，3语音url+文字";	
	private string sTest = "初始化";
	private string sjson = "显示json";
	private string voiceUrl=string.Empty;
	private string filePath = string.Empty;
	private string filePathUrl = string.Empty;
	private int duration=0;

	bool mic=true;
	bool isPause=true;
	int modeType=0;
	bool mode1 = false;
	bool mode2 = false;
	bool mode3 = true;

	void Start()
	{
        
	}

	void Update()
	{
		//cube.transform.Rotate (Vector3.up, Time.deltaTime * 150, Space.World);
	}

    void OnGUI()
    {
        if (guiSkin != null)
        {
            GUI.skin = guiSkin;
        }
        sModeType = GUI.TextField(new Rect(20, 0, 400, 100), sModeType);

        if (GUI.Button(new Rect(20, 200, 200, 100), "初始化"))
        {
            VideoTroopsAPI.Instance.ChatSDKInit(0,"1003051",1, (msg) =>
                {
                    sTest = "初始化完成";
                });
        }

        if (GUI.Button(new Rect(230, 200, 200, 100), "登录房间"))
        {

            string seq = "id123";
            VideoTroopsAPI.Instance.ChatSDKLogin(DateTime.Now.ToFileTime().ToString(), seq, (msg) =>
            {
            


                sjson = "登录队伍" + msg+"  userString:"+DateTime.Now.ToFileTime().ToString();
          
            });
        }

        if (GUI.Button(new Rect(440, 200, 200, 100), "登出房间"))
        {
            VideoTroopsAPI.Instance.Logout((msg) =>
            {
                sjson = "登出队伍" + msg;
            });
        }

        if (GUI.Button(new Rect(20, 310, 200, 100), "上麦"))
        {
            mic = true;
            VideoTroopsAPI.Instance.ChatMic(mic, (msg) =>
            {
                sjson = "上麦" + msg;
            });

        }
        if (GUI.Button(new Rect(230, 310, 200, 100), "下麦"))
        {
            mic = false;
            VideoTroopsAPI.Instance.ChatMic(mic, (msg) =>
            {
                sjson = "下麦" + msg;
            });
        }

        if (GUI.Button (new Rect (20, 420, 200, 100), "暂停实时播放")) 
        {
            isPause = true;
            VideoTroopsAPI.Instance.SetPausePlayRealAudio(isPause);
        }

        if (GUI.Button(new Rect(230, 420, 200, 100), "恢复实时播放"))
        {
            isPause = false;
            VideoTroopsAPI.Instance.SetPausePlayRealAudio(isPause);
        }

        //======
            if (GUI.Button(new Rect(440, 420, 200, 100), "播放本地语音"))
            {
                VideoTroopsAPI.Instance.AudioToolsPlayAudio(filePath, (data1) =>
                    {
                        sjson = "播放本地完成:" + data1;
                        Debug.Log("播放本地完成:" + data1);
                    });
            }
            if (GUI.Button(new Rect(20, 530, 100, 100), "开始录音"))
            {
		    	VideoTroopsAPI.Instance.AudioToolsStartRecord(2,(msg) =>
                {
                    Debug.Log("录音完成:" + msg);
                    sjson = "录音完成:" + msg;
                    JsonData data = JsonMapper.ToObject(msg);
                    filePath = JsonHelp.getStringValue(data, "filePath");
                    duration = JsonHelp.getIntValue(data, "voiceDuration");
                },
				(data1)=>
				{
					Debug.Log("上传完成:" + data1);
                    sjson = "上传完成:" + data1;
					JsonData dataUrl = JsonMapper.ToObject(data1);
					filePathUrl = JsonHelp.getStringValue(dataUrl, "voiceUrl");
				},
				(data2)=>
				{
					Debug.Log("识别完成:" + data2);
                    sjson = "识别完成:" + data2;
					JsonData dataobject = JsonMapper.ToObject(data2);
					string text = JsonHelp.getStringValue(dataobject, "text");
					string voiceUrl = JsonHelp.getStringValue(dataobject, "voiceUrl");
                    VideoTroopsAPI.Instance.AudioToolsPlayOnlineAudio(voiceUrl, (data1) =>
                    {
                        sjson = "播放url完成:" + data1;
                        Debug.Log("播放url完成:" + data1);
                    });
				});
            }
            if (GUI.Button(new Rect(130, 530, 100, 100), "停止录音"))
            {
                VideoTroopsAPI.Instance.AudioToolsStopRecord();
            }
            if (GUI.Button(new Rect(430, 0, 100, 100), "播放url"))
            {
			    VideoTroopsAPI.Instance.AudioToolsPlayOnlineAudio (filePathUrl, (data1) => {
                sjson = "播放url完成:" + data1;
				Debug.Log ("播放url完成:" + data1);
			});
            }

            mode1 = GUI.Toggle(new Rect(250, 540, 100, 100), mode1, "文字消息");
            if (mode1)
            {
                mode2 = false;
                mode3 = false;
            }
            mode2 = GUI.Toggle(new Rect(350, 540, 100, 100), mode2, "语音url");
            if (mode2)
            {
                mode1 = false;
                mode3 = false;
            }
            mode3 = GUI.Toggle(new Rect(450, 540, 100, 100), mode3, "语音url+文字");
            if (mode3)
            {
                mode1 = false;
                mode2 = false;
            }

		if (GUI.Button(new Rect(440, 310, 200, 100), "发送消息"))
            {
                int modeType = 0;
                if (mode1)
                {
                    modeType = 1;
                }
                else if (mode2)
                {
                    modeType = 2;
                }
                else if (mode3)
                {
                    modeType = 3;
                }
                VideoTroopsAPI.Instance.SendTextMessage(modeType, "hello world,i am a text message", filePathUrl, "扩展字段",(msgs) =>
				{
					sjson = "发送声音：" + msgs;
				});
            }
            GUI.Label(new Rect(100, 120, 800, 60), sTest);
            GUI.Label(new Rect(100, 150, 800, 60), sjson);
        }

    void Awake()
    {
        CvEventListenerManager.AddListener(CvListenerEven.ReceiveTextMessageNotify, (obj) =>
        {
            sjson = "收到消息:" + obj.ToString();
            Debug.Log(sjson);
            JsonData data = JsonMapper.ToObject(obj.ToString());
            int msgType = JsonHelp.getIntValue(data, "msgType");
            string textMsg = JsonHelp.getStringValue(data, "textMsg");
            string voiceUrl = JsonHelp.getStringValue(data, "voiceUrl");
            string userId = JsonHelp.getStringValue(data, "userId");
            if (msgType == 2 || msgType == 3)
            {
                VideoTroopsAPI.Instance.AudioToolsPlayOnlineAudio(voiceUrl, (data1) =>
                {
                    sjson = "播放url完成:" + data1;
                    Debug.Log("播放url完成:" + data1);
                });
            }

        });

        CvEventListenerManager.AddListener(CvListenerEven.LoginNotify, (obj) =>
        {
            JsonData data = JsonMapper.ToObject(obj.ToString());
            string userId = JsonHelp.getStringValue(data, "userId");
            string roomid = JsonHelp.getStringValue(data, "roomId");
            sjson = userId + " 登录房间:" + roomid;
            Debug.Log(sjson);
        });

        CvEventListenerManager.AddListener(CvListenerEven.LogoutNotify, (obj) =>
        {
            JsonData data = JsonMapper.ToObject(obj.ToString());
            string userId = JsonHelp.getStringValue(data, "userId");
            string roomid = JsonHelp.getStringValue(data, "roomId");
            sjson = userId + " 登出房间:" + roomid;
            Debug.Log(sjson);
        });
    }
}