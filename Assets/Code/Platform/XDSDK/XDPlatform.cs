using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  
using System.Runtime.InteropServices;
using com.taptap.sdk;

public class XDPlatform : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    void OnGUI()
    {

        if (GUI.Button(new Rect(80, 80, 100, 100), "初始化"))
        {

            xdsdk.XDSDK.SetCallback(new XDSDKHandler());

            string[] entries = { "WX_LOGIN", "TAPTAP_LOGIN" };
            xdsdk.XDSDK.SetLoginEntries(entries);

            xdsdk.XDSDK.InitSDK("d4bjgwom9zk84wk", 0, "", "", true);
        }

        if (GUI.Button(new Rect(280, 80, 100, 100), "登录"))
        {

            xdsdk.XDSDK.Login();
        }

        if (GUI.Button(new Rect(480, 80, 100, 100), "登出"))
        {

            xdsdk.XDSDK.Logout();
        }


        if (GUI.Button(new Rect(80, 200, 100, 100), "用户中心"))
        {

            xdsdk.XDSDK.OpenUserCenter();
        }

        if (GUI.Button(new Rect(80, 880, 100, 100), "用户中心"))
        {

            xdsdk.XDSDK.OpenRealName();
        }


        if (GUI.Button(new Rect(280, 200, 100, 100), "支付"))
        {

            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("OrderId", "1234567890123456789012345678901234567890");
            info.Add("Product_Price", "1");
            info.Add("EXT", "abcd|efgh|1234|5678");
            info.Add("Sid", "2");
            info.Add("Role_Id", "3");
            info.Add("Product_Id", "XDSDKPoint");
            info.Add("Product_Name", "648大礼包");
            xdsdk.XDSDK.Pay(info);
        }

        if (GUI.Button(new Rect(480, 200, 100, 100), "账号ID"))
        {

            //sdk_debug_msg(xdsdk.XDSDK.GetAccessToken());
        }



        if (GUI.Button(new Rect(80, 320, 100, 100), "版本号"))
        {

            //sdk_debug_msg(xdsdk.XDSDK.GetSDKVersion());
        }

        if (GUI.Button(new Rect(280, 320, 100, 100), "隐藏游客"))
        {

            xdsdk.XDSDK.HideGuest();
            xdsdk.XDSDK.HideQQ();
            xdsdk.XDSDK.HideWX();
            xdsdk.XDSDK.ShowVC();
            xdsdk.XDSDK.SetWXWeb();
            xdsdk.XDSDK.SetQQWeb();

        }

        if (GUI.Button(new Rect(480, 320, 100, 100), "隐藏QQ"))
        {

            xdsdk.XDSDK.HideQQ();
        }

        if (GUI.Button(new Rect(80, 440, 100, 100), "隐藏WX"))
        {

            xdsdk.XDSDK.HideWX();
        }

        if (GUI.Button(new Rect(280, 440, 100, 100), "显示VC"))
        {

            xdsdk.XDSDK.ShowVC();
        }

        if (GUI.Button(new Rect(480, 440, 100, 100), "QQWeb"))
        {

            xdsdk.XDSDK.SetQQWeb();
        }

        if (GUI.Button(new Rect(80, 560, 100, 100), "WXWeb"))
        {

            xdsdk.XDSDK.SetWXWeb();
        }

        if (GUI.Button(new Rect(80, 680, 100, 100), "hideTapTap"))
        {

            xdsdk.XDSDK.HideTapTap();
        }

        if (GUI.Button(new Rect(280, 680, 100, 100), "用户反馈"))
        {

            xdsdk.XDSDK.UserFeedback();
        }

        if (GUI.Button(new Rect(280, 560, 100, 100), "isLogedIn"))
        {

            //sdk_debug_msg(xdsdk.XDSDK.IsLoggedIn().ToString());
        }


        if (GUI.Button(new Rect(480, 560, 100, 100), "Share"))
        {

            Dictionary<string, string> shareData = new Dictionary<string, string>();

            shareData.Add("text", "text");
            shareData.Add("bText", "1");
            shareData.Add("scene", "0");
            shareData.Add("shareType", "0");
            shareData.Add("title", "title");
            shareData.Add("description", "description");
            shareData.Add("thumbPath", "");
            shareData.Add("imageUrl", "");
            shareData.Add("musicUrl", "");
            shareData.Add("musicLowBandUrl", "");
            shareData.Add("musicDataUrl", "");
            shareData.Add("musicLowBandDataUrl", "");
            shareData.Add("videoUrl", "");
            shareData.Add("videoLowBandUrl", "");
            shareData.Add("webpageUrl", "http://api.pinidea.online");

            xdsdk.XDSDK.Share(shareData);
        }

        if (GUI.Button(new Rect(80, 800, 300, 100), "论坛"))
        {
            TapTapSDK.Instance.OpenTapTapForum("58881");
        }
    }
		
}
