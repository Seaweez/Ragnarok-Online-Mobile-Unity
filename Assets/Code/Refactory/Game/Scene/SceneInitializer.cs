using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class SceneInitializer : MonoBehaviour
	{
		public GameObject[] dontDestroyPrefabs;
		public GameObject[] prefabs;

		#region behaviour
		void Awake ()
		{
			if (!dontDestroyPrefabs.IsNullOrEmpty())
			{
				var dontDestroyParent = (null != LuaLuancher.Me) ? LuaLuancher.Me.transform : null;
				for (int i = 0; i < dontDestroyPrefabs.Length; ++i)
				{
					var p = dontDestroyPrefabs[i];
					if (null != p)
					{
						var go = GameObject.Instantiate<GameObject>(p);
						if (null != dontDestroyParent)
						{
							go.transform.SetParent(dontDestroyParent);
						}
						else
						{
							GameObject.DontDestroyOnLoad(go);
						}
					}
				}
			}
			
			if (!prefabs.IsNullOrEmpty())
			{
				for (int i = 0; i < prefabs.Length; ++i)
				{
					var p = prefabs[i];
					if (null != p)
					{
						GameObject.Instantiate<GameObject>(p);
					}
				}
			}

			if (null != LuaLuancher.Me)
			{
				LuaLuancher.Me.Call("OnSceneAwake", this);
			}
		}

		void Start()
		{
			if (null != LuaLuancher.Me)
			{
				LuaLuancher.Me.Call("OnSceneStart", this);
			}
			GameObject.Destroy(gameObject);

			ApplicationHelper.RegisterLowMemoryCall ();
		}
		#endregion behaviour
	}
} // namespace RO
