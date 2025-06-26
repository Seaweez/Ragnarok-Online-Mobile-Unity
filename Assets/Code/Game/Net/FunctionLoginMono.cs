using UnityEngine;
using System.Collections;
using System.Net;
using System;
using System.IO;
using System.Text;
using System.Threading;
namespace RO.Net
{

	[SLua.CustomLuaClassAttribute]	
	public class FunctionLoginMono : SingleTonGO<FunctionLoginMono>
	{
		private Action<string> handler = null;
		public static FunctionLoginMono Instance {
			get {
				return Me;
			}
		}

		public void handleAndroidAliGetIp(string ip)
		{
			NetLog.Log (string.Format("handleAndroidAliGetIp ip:{0}",ip));
			if(handler != null)
			{
				handler(ip);
			}
		}

		public void getIpByGroupNameAsync(string groupName,Action<string> handler,int timeout)
		{
			try
			{
				#if UNITY_ANDROID && !UNITY_EDITOR
					this.handler = handler;
					AliyunSecurityIPSdk.Android_ALSDK_GetNextIPByGroupNameAsync(groupName,gameObject.name);
				#elif UNITY_IPHONE && !UNITY_EDITOR
					var request = new AliyunSecurityIPSdk(handler,groupName,timeout);
					ThreadPool.QueueUserWorkItem(new WaitCallback(exeGetAliIp), request);
					StartCoroutine (RequestBack (request));
				#else
					handler("");	
				#endif
			}			
			catch (NotSupportedException)
			{
				NetLog.Log ("These API's may fail when called on a non-Wind ows 2000 system.");
			}
		}

		static void exeGetAliIp(object obj)
		{
			var request = obj as AliyunSecurityIPSdk;
			request.getIpByGroupNameAsync();
		}
		
		public IEnumerator RequestBack (AliyunSecurityIPSdk sdk)
		{
			while (false == sdk.hasFinishGetAliIp && false == sdk.isOverTime) {
				sdk.updatePassedTime();
				yield return 0;
			}
			sdk.exeHandler();
		}

		public void startResolve (FunctionLoginDnsResolve fldr)
		{
			StartCoroutine(startResolveCoroutine(fldr));
		}

		public void startConnect (FunctionLoginChooseSocket flcs)
		{
			StartCoroutine(startConnectCoroutine(flcs));
		}

		public IEnumerator startResolveCoroutine (FunctionLoginDnsResolve fldr)
		{
			while (false == fldr.isComplete && false == fldr.isOverTime) {
				fldr.updatePassedTime();
				yield return 0;
			}
			fldr.finishRequest();
		}

		public IEnumerator startConnectCoroutine (FunctionLoginChooseSocket flcs)
		{
			while (false == flcs.isComplete && false == flcs.isOverTime) {
				flcs.updatePassedTime();
				yield return 0;
			}
			flcs.finishTest();
		}
	}
}
