using UnityEngine;
using System.Collections.Generic;
using SLua;
using System;
using UnityEngine.SceneManagement;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class LoadScene : MonoBehaviour
	{
		public static string LoadSceneLoaded = "LoadSceneLoaded";
		public string syncLoadScene = "";
		public string asyncLoadScene { get; set; }

		public void Start()
		{
			if (string.IsNullOrEmpty(syncLoadScene) && string.IsNullOrEmpty(asyncLoadScene))
				Call("sendNotification", LoadSceneLoaded);
			else
			{
				if (string.IsNullOrEmpty(syncLoadScene) == false)
				{
					ResourceManager.Me.SLoadScene(syncLoadScene);
					SceneManager.LoadScene(syncLoadScene);

				}
				else if (string.IsNullOrEmpty(asyncLoadScene) == false)
				{
					ResourceManager.Me.SLoadScene(asyncLoadScene);
					SceneManager.LoadSceneAsync(asyncLoadScene);
				}
			}
		}

		public static void Call(string funcName = null, params object[] datas)
		{
			try
			{
				LuaTable facade = MyLuaSrv.Instance.luaState.getTable("GameFacade");
				LuaTable self = facade["Instance"] as LuaTable;

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
			catch (Exception)
			{
				//				NetLog.LogE("NetService::Call Error: serviceProxyName_" + serviceProxyName + " " + e.Message);
			}
		}
	}
} // namespace RO
