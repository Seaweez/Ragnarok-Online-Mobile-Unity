using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Created by sunyi on 30/11/2017.
 */


namespace com.taptap.sdk
{
	public class AndroidImpl : TapTapSDKImpl
	{
		public override void OpenTapTapForum(string appid){
			AndroidJavaClass jc = new AndroidJavaClass ("com.taptap.sdk.TapTapUnity");
			jc.CallStatic ("openTapTapForum", appid);
		}
	}
}

