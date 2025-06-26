using UnityEngine;
using System.Collections.Generic;
using RO;
using System;
using System.Collections;
using System.Threading;
using SLua;
using LitJson;
using System.Net;
using System.Net.Sockets;
using RO.Test;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class LuaLuancher : SingleTonGO<LuaLuancher>
	{
		public static int originalScreenWidth;
		public static int originalScreenHeight;
		static LuaLuancher()
		{
			originalScreenWidth = Screen.width;
			originalScreenHeight = Screen.height;
		}

		public static LuaLuancher Instance
		{
			get
			{
				return Me;
			}
		}
		
		public GameObject monoGameObject
		{
			get
			{
				return gameObject;
			}
		}

		public GameObject[] prefabs;
		public Component[] objects;

		#region config
		public string defaultAction = "wait";
		public int defaultActionNameHash{get;private set;}
		public Vector3 roleColliderMinSize = new Vector3(0.5f, 0.5f, 0.5f);
		public Vector3 roleColliderMaxSize = new Vector3(1f, 2f, 1f);
		public float roleColliderUpdateDelay = 0.1f;

		private int _ignoreAreaTrigger = 0;
		public bool ignoreAreaTrigger
		{
			get
			{
				return 0 < _ignoreAreaTrigger;
			}
			set
			{
				if (value)
				{
					++_ignoreAreaTrigger;
				}
				else
				{
					--_ignoreAreaTrigger;
				}
			}
		}
		#endregion config

		#region object
		public RoleComplete myself = null;
		#endregion object
		
		public string scriptPath = "Script/Refactory/Game/GameLauncher";
		private LuaWorker worker = null;
		
		public object Call(string methodName, params object[] args)
		{
			if (null == worker) 
			{
				return null;
			}
			return worker.CallLuaStaticMethod (methodName, args);
		}
		
		#region behaviour
		protected override void Awake ()
		{
			base.Awake ();

			#region config
			defaultActionNameHash = Animator.StringToHash(defaultAction);
			#endregion config
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			if (null != worker) 
			{
				worker.Dispose ();
				worker = null;
			}
		}

		void Start ()
		{
			GameObjPool.Instance.OnPoolNoGet = LoadFromPool;
			worker = new LuaWorker();
			worker.DoFile(scriptPath);
            LuaLoadOverrider.Me.ClearLuaMapAsset();
        }

		GameObject LoadFromPool (ResourceID res, string arg2)
		{
            UnityEngine.Object source = ResourceManager.Me.Load (res);
			if (source is GameObject) {
				GameObject go = GameObject.Instantiate (source) as GameObject;
				go.name = go.name.Replace ("(Clone)", "");
				return go;
			}
			return null;
		}

		GameObject SLoadFromPool (string res, string arg2)
		{
            UnityEngine.Object source = ResourceManager.Me.SLoad (res);
			if (source is GameObject) {
				GameObject go = GameObject.Instantiate (source) as GameObject;
				go.name = go.name.Replace ("(Clone)", "");
				return go;
			}
			return null;
		}

		void Update()
		{
			Call("Update");
		}

		void LateUpdate()
		{
			if (null != GameObjectMap.Me)
			{
//				Profiler.BeginSample("[Ghost] UpdateCullingGroup");
				GameObjectMap.Me.UpdateCullingGroup();
//				Profiler.EndSample();
			}
			Call("LateUpdate");
		}

#if DEBUG_DRAW
		void OnDrawGizmos()
		{
			Call("OnDrawGizmos");
		}
#endif // DEBUG_DRAW
		#endregion behaviour
	}
} // namespace RO
