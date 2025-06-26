using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Created by sunyi on 30/11/2017.
 */


namespace com.taptap.sdk
{
	public sealed class TapTapSDK
	{
		private static volatile TapTapSDK instance;
		private static object syncRoot = new System.Object ();

		private TapTapSDKImpl taptapSDKimpl;
	

		private TapTapSDK ()
		{
		}

		public static TapTapSDK Instance {
			get {
				if (instance == null) {
					lock (syncRoot) {
						if (instance == null)
							instance = new TapTapSDK ();
					}
				}
				return instance;
			}
		}

		public void OpenTapTapForum(string appid){
       		if(taptapSDKimpl == null){
#if UNITY_ANDROID
        		taptapSDKimpl = new AndroidImpl ();
#elif UNITY_IPHONE
        		taptapSDKimpl = new iOSImpl ();
#endif
			}
			taptapSDKimpl.OpenTapTapForum(appid);
		}	
		
	}
}

