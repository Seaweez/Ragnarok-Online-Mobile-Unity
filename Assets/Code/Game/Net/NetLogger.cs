using UnityEngine;
using System.Collections;
using System;
using System.Text;

namespace RO.Net
{
	/**
	 * The logger of all network connections
	 */ 
	public class NetLogger
	{
		public void Log(string log)
		{
			if (NetConnectionManager.Me != null && NetConnectionManager.Me.EnableLog)
			{
				ROLogger.Log(log);
			}
		}

		#region connect logs
		public void LogConn(string name, string msg = "")
		{
			if (NetConnectionManager.Me != null && NetConnectionManager.Me.EnableLog)
			{
				ROLogger.LogFormat("{0} connect request {1}", name, msg);
			}
		}

		public void LogUnConn(string name, string msg = "")
		{
			if (NetConnectionManager.Me != null && NetConnectionManager.Me.EnableLog)
			{
				ROLogger.LogFormat("{0} is unconnected {1}", name, msg);
			}
		}

		public void LogConnError(string name, string msg = "")
		{
			if (NetConnectionManager.Me != null && NetConnectionManager.Me.EnableLog)
			{
				ROLogger.LogFormat("{0} connect error {1}", name, msg);		
			}
		}
		#endregion

		#region receive logs
		public void LogReceiveStop(string name, string msg = "")
		{
			if (NetConnectionManager.Me != null && NetConnectionManager.Me.EnableLog)
			{
				ROLogger.LogFormat("{0} receive stop {1}", name, msg);
			}
		}	

		public void LogByteDatarintDataAndLen(string action, int length,byte[] msg = null)
		{
			if (NetConnectionManager.Me != null && NetConnectionManager.Me.EnableLog)
			{
				ROLogger.LogFormat("{0} data  leng:{1} ,content:{2}", action,length, msg == null ? "":NetUtil.BytesToStringByLen(msg,0,length));
			}
		}	
	
		public void LogReceiveError(string name, string msg = "")
		{
			if (NetConnectionManager.Me != null && NetConnectionManager.Me.EnableLog)
			{
				ROLogger.LogFormat("{0} receive data error {1}", name, msg);
			}
		}
		#endregion	

		public void LogSendSuccess(string name, string msg = "")
		{
			if (NetConnectionManager.Me != null && NetConnectionManager.Me.EnableLog)
			{
				ROLogger.LogFormat("{0} send data success {1}", name, msg);
			}
		}
	}
}
