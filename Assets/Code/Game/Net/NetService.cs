using SLua;
using System;
using System.Collections;

namespace RO.Net
{
	/**
	 * Call lua net service proxy in c#
	 */
	public class NetService
	{
		public const string ServiceConnProxy = "ServiceConnProxy";
		public const string ServiceUserProxy = "ServiceUserProxy";
		public const string ServicePlayerProxy = "ServicePlayerProxy";
		public const string ServiceErrorProxy = "ServiceErrorProxy";

		/// <summary>
		/// Call service proxy
		/// </summary>
		/// <param name="serviceName">Service name.</param>
		/// <param name="data">Data.</param>
		public static void Call(string serviceProxyName, string funcName = null, params object[] datas)
		{
			try {
				LuaTable serviceProxy = MyLuaSrv.Instance.luaState.getTable(serviceProxyName);
				LuaTable self = serviceProxy["Instance"] as LuaTable;

				funcName = funcName != null && funcName.Length > 0 ? funcName : "Call";
				LuaFunction func = self[funcName] as LuaFunction;

				// arguments
				object[] args = new object[datas.Length + 1];
				args[0] = self;
				if (datas.Length > 0)
					datas.CopyTo(args, 1);

				// call function
				func.call(args);
			}
			catch (Exception e) {
				NetLog.LogE("NetService::Call Error: serviceProxyName_" + serviceProxyName + " " + e.Message);
			}
		}
	}
}
