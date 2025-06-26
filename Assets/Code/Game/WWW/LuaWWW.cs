using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class LuaWWW : MonoBehaviour
	{
		static GameObject container;
		static List<LuaWWW> cached = new List<LuaWWW> ();
		public Action<WWW> loadedHandler;
		public Action<WWW> loadingHandler;
		bool _isLoading;
		string _path;
		WWW _www;

		public static void WWWLoad (string path, Action<WWW> loadedHandler)
		{
			WWWLoad (path, null, loadedHandler);
		}
		
		public static void WWWLoad (string path, Action<WWW> loadingHandler, Action<WWW> loadedHandler)
		{
			if (container == null)
				container = new GameObject ("LuaWWW");
			LuaWWW lwww = container.AddComponent<LuaWWW> ();
			cached.Add (lwww);
			lwww.StartLoad (path, loadingHandler, loadedHandler);
		}
	
		public void StartLoad (string path, Action<WWW> loadingHandler, Action<WWW> loadedHandler)
		{
			if (!_isLoading) {
				this.loadedHandler = loadedHandler;
				this.loadingHandler = loadingHandler;
				_path = path;
				StartCoroutine (WWWLoad ());
			}
		}

		IEnumerator WWWLoad ()
		{
			_isLoading = true;
			_www = new WWW (_path);
			yield return _www;
			if (loadedHandler != null)
				loadedHandler (_www);
			_isLoading = false;
			Component.Destroy (this);
		}

		void Update ()
		{
			if (_www != null && _isLoading) {
				if (loadingHandler != null)
					loadingHandler (_www);
			}
		}

		void OnDestroy ()
		{
			loadedHandler = null;
			loadingHandler = null;
			if (_www != null)
				_www.Dispose ();
			cached.Remove (this);
		}
	}
} // namespace RO
