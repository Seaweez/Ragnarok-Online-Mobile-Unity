using UnityEngine;
using System.Collections.Generic;
using SLua;
using RO;

namespace RO.Test
{
	public class DebugStart : MonoBehaviour
	{
		[RuntimeInitializeOnLoadMethod]
		static void StartInit ()
		{
			Object obj = Resources.Load ("Debug/Debug");
			GameObject go = GameObject.Instantiate (obj) as GameObject;
			GameObject.DontDestroyOnLoad (go);
		}

		public float updateInterval = 3;
		float _nextUpdateTime = 0;
		public Reporter reporter;
		public RODebugInfo roDebugInfo;
		public List<TextAsset> luafiles = new List<TextAsset> ();
		private Dictionary<string, string> luafileMap = new Dictionary<string, string> ();
		private bool running = false;

		void Awake ()
		{
			for (int i = 0; i < luafiles.Count; i++) {
				string fileName = luafiles [i].name;
				string fileContext = luafiles [i].ToString ();
				if (!luafileMap.ContainsKey (fileName)) {
					luafileMap [fileName] = fileContext;
				}
			}
		}

		void Update ()
		{
			if (_nextUpdateTime == 0 || _nextUpdateTime <= Time.realtimeSinceStartup) {
				_nextUpdateTime = Mathf.Max (0, updateInterval) + Time.realtimeSinceStartup;
				if (running) {
					return;
				}

				if (MyLuaSrv.IsRunning) {
					Launch ();
				}
			}
		}

		public void Launch ()
		{
			if (running) {
				return;
			}

//			GameObject lualuancherObj = LuaLuancher.Me.gameObject;
			GameObject lualuancherObj = GameObject.Find ("LuaSvrProxy");
			if (lualuancherObj == null) {
				return;
			}

			bool headerImported = (bool)MyLuaSrv.Instance.luaState.getObject ("HeaderImported");
			if (headerImported != true) {
				return;
			}

			running = true;

			LuaLuancherMonitor lm = lualuancherObj.GetComponent<LuaLuancherMonitor> ();
			if (lm == null) {
				lm = lualuancherObj.AddComponent<LuaLuancherMonitor> ();
			}
			lm.onDestroy = _luaLuncherDestroyCall;

			DoDebugFile ("DebugManager");
			LuaFunction init = MyLuaSrv.Instance.GetFunction ("DebugManager.Init");
			init.call (this);
		}

		void _luaLuncherDestroyCall (GameObject obj)
		{
			this.ShutDown ();
		}

		public void ShutDown ()
		{
			if (!running) {
				return;
			}

			running = false;

			if (MyLuaSrv.IsRunning) {
				LuaFunction reset = MyLuaSrv.Instance.GetFunction ("DebugManager.Reset");
				reset.call (this);
			}
		}

		public void DoDebugFile (string fileName)
		{
			if (luafileMap.ContainsKey (fileName)) {
				MyLuaSrv.Instance.DoString (luafileMap [fileName]);
			}
		}
	}
}
// namespace RO.Test
