using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class GameObjPool : SingleTonGO<GameObjPool>
	{
		public Func<ResourceID,string,GameObject> OnPoolNoGet;
		private SDictionary<string, Pool> pool = new SDictionary<string, Pool> ();

		//for lua
		public static GameObjPool Instance {
			get{ return Me;}
		}
		
		public GameObject monoGameObject
		{
			get
			{
				return gameObject;
			}
		}

		Pool GetPool (string poolName)
		{
			return GetPoolEx(poolName, false);
		}

		Pool GetPoolEx (string poolName, bool ignoreActive)
		{
			if (pool [poolName] == null) {
				pool [poolName] = new Pool (poolName, this.transform, ignoreActive);
			}
			return pool [poolName];
		}

		public GameObject Add (GameObject go, string name, string poolName)
		{
			return AddEx (go, name, poolName, false);
		}

		public GameObject AddEx (GameObject go, string name, string poolName, bool ignoreActive)
		{
			if (go != null) {
				Pool pool = GetPoolEx (poolName, ignoreActive);
				if (pool != null) {
					pool.Add (name, go);
				}
			}
			return go;
		}

		public GameObject RAdd (GameObject go, ResourceID name, string poolName)
		{
			return RAddEx(go, name, poolName, false);
		}

		public GameObject RAddEx (GameObject go, ResourceID name, string poolName, bool ignoreActive)
		{
			if (go != null) {
				Pool pool = GetPoolEx (poolName, ignoreActive);
				if (pool != null) {
//					pool.Add (name.getRealPath, go);
					Debug.LogError("RAddEx is no-used");
				}
			}
			return go;
		}

		public GameObject Get (string name, string poolName, GameObject parent = null)
		{
			return GetEx (name, poolName, parent, false);
		}

		public GameObject GetEx (string name, string poolName, GameObject parent, bool ignoreActive)
		{
			Pool p = GetPoolEx (poolName, ignoreActive);
			return p.Get (name, parent);
		}

		public GameObject RGet (ResourceID name, string poolName = null, GameObject parent = null)
		{
			return RGetEx(name, poolName, parent, false);
		}

		public GameObject RGetEx (ResourceID name, string poolName, GameObject parent, bool ignoreActive)
		{
			Debug.LogError("RAddEx is no-used");
			return null;
//			if (poolName == null)
//				poolName = RO.Config.Pool.NAME_DEFAULT;
//			GameObject res = GetEx (name.getRealPath, poolName, parent, ignoreActive);
//			if (res == null) {
//				if (OnPoolNoGet != null) {
//					GameObject go = OnPoolNoGet (name, poolName);
//					if (go != null && parent != null) {
//						go.transform.SetParent (parent.transform, true);
//						go.transform.localScale = Vector3.one;
//						go.transform.localRotation = Quaternion.identity;
//					}
//					return go;
//				} else {
//					UnityEngine.Object source = ResourceManager.Me.Load (name);
//					if (source is GameObject) {
//						GameObject go = GameObject.Instantiate (source) as GameObject;
//						go.name = go.name.Replace ("(Clone)", "");
//						return go;
//					}
//					return null;
//				}
//			}
//			return res;
		}

		public void ClearPool (string poolName)
		{
			ClearPoolEx(poolName, false);
		}

		public void ClearPoolEx (string poolName, bool ignoreActive)
		{
			Pool pool = GetPoolEx (poolName, ignoreActive);
			if (pool != null)
				pool.Clear ();
		}

		public void ClearAll ()
		{
			foreach (KeyValuePair<string, Pool> kvp in pool) {
				kvp.Value.Clear ();
			}
		}
	}

	class Pool
	{
		public string name;
		public GameObject pool;
		public bool ignoreActive;
		private SDictionary<string , BetterList<GameObject>> _poolGo = new SDictionary<string, BetterList<GameObject>> ();

		public Pool (string name, Transform parent, bool ignoreActive = false)
		{
			this.name = name;
			this.ignoreActive = ignoreActive;
			pool = new GameObject (name);
			if (ignoreActive)
			{
				pool.transform.position = new Vector3(-10000,-10000,-10000);
			}
			else
			{
				pool.SetActive (false);
			}
			pool.transform.parent = parent;
		}
		
		public void Add (string name, GameObject go)
		{
			BetterList<GameObject> gos = _poolGo [name];
			if (gos == null)
				_poolGo [name] = gos = new BetterList<GameObject> ();
			if (gos.Contains (go) == false)
				gos.Add (go);
			go.transform.SetParent(pool.transform, false);
			if (!ignoreActive)
			{
				go.SetActive (false);
			}
		}
		
		public GameObject Get (string name, GameObject parent = null)
		{
			BetterList<GameObject> gos = _poolGo [name];
			if (gos != null && gos.size > 0) {
				GameObject go = gos.Pop ();
				if (go != null) {
					go.transform.SetParent(parent != null ? parent.transform : null, false);
					if (!ignoreActive)
					{
						go.SetActive (true);
					}
					return go;
				}
				return null;
			}
			return null;
		}
		
		public void Clear ()
		{
			Transform trans = pool.transform;
			int count = trans.childCount;
			for (int i=0; i<count; i++) {
				GameObject.Destroy (trans.GetChild (i).gameObject);
			}
			_poolGo.Clear ();
		}
	}
} // namespace RO
