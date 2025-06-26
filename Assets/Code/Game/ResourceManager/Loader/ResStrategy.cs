using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using Ghost.Config;

namespace RO
{
	public class ResStrategy:ILoaderStrategy
	{
		public static string holderAssetPath = "Assets/Resources/AutoGenerate/";
		private static SDictionary<string, Object> _dicPrefabs = new SDictionary<string, Object> ();
		private static Dictionary<string,AsyncResourceLoad> _asyncMap = new Dictionary<string, AsyncResourceLoad> ();
		public static ResStrategy Global = new ResStrategy ();
		
		public ResStrategy ()
		{
			TextAsset t = Resources.Load ("AutoGenerate/pathIdMap") as TextAsset;
			ResourceID.ReMap (t != null ? t.text : null);
		}
		
		#region ILoaderStrategy implementation
		
		public void LateUpdate ()
		{
			
		}
		
		public void Dispose ()
		{
		}
		
		#endregion
		
		
		#region IUnLoaderStrategy implementation
		
		public void UnLoad (ResourceID ID, bool unloadAllLoadedObjects)
		{
			SUnLoad (ID.IDStr,unloadAllLoadedObjects);
		}
		
		public void UnLoadAll (bool unloadAllLoadedObjects)
		{
			//			foreach (KeyValuePair<string,Object> kvp in _dicPrefabs) {
			//				Resources.UnloadAsset (kvp.Value);
			//			}
			_dicPrefabs.Clear ();
			_asyncMap.Clear ();
			Resources.UnloadUnusedAssets ();
		}
		
		public void UnLoadScene (ResourceID ID, bool unloadAllLoadedObjects = false)
		{
		}

		public void SUnLoad (string ID, bool unloadAllLoadedObjects)
		{
			Object asset;
			if (_dicPrefabs.TryGetValue (ID, out asset)) {
				if (asset != null) {
					if ((asset is GameObject) == false && (asset is Component) == false && (asset is AssetBundle) == false) {
						Resources.UnloadAsset (asset);
					}
				}
			}
			if (_asyncMap.ContainsKey (ID)) {
				_asyncMap.Remove (ID);
			}
			_dicPrefabs.Remove (ID);
		}
		
		public void SUnLoadScene (string ID, bool unloadAllLoadedObjects = false)
		{
		}
		
		#endregion

		IEnumerator _Load (ResourceID ID, System.Type resType, System.Action<Object> loadedHandler)
		{
			return null;
//			string path = ID.getRealPath;
//			Object asset = null;
//			if (_dicPrefabs.TryGetValue (ID.getRealPath, out asset) == true) {
//				loadedHandler (asset);
//				yield break;
//			}
//			AsyncResourceLoad arl = null;
//			if (_asyncMap.ContainsKey (path)) {//已经在加载队列,等待完成
//				arl = _asyncMap [path];
//				arl.AddLoadedCall (loadedHandler);
//				yield break;
//			}
//			
//			ResourceRequest req = resType == null ? Resources.LoadAsync (path) : Resources.LoadAsync (path, resType);
//			arl = new AsyncResourceLoad (path, req);
//			arl.AddLoadedCall (loadedHandler);
//			_asyncMap.Add (path, arl);
//			while (!req.isDone) {
//				yield return 0;
//			}
//			if (_asyncMap.ContainsKey (path)) {
//				//否则的话，是被Unload掉了
//				_asyncMap.Remove (path);
//				if (req.asset != null) {
//					_dicPrefabs.Add (path, req.asset);
//					arl.LoadedCall ();
//				}
//				arl.Dispose ();
//			}
		}
		
		#region IAsyncLoaderStrategy implementation
		
		public void AsyncLoad (ResourceID ID, System.Action<Object> loadedHandler)
		{
			ResourceManager.Me.StartCoroutine (_Load (ID, null, loadedHandler));
		}
		
		public void AsyncLoad (ResourceID ID, System.Type resType, System.Action<Object> loadedHandler)
		{
			ResourceManager.Me.StartCoroutine (_Load (ID, resType, loadedHandler));
		}
		
		public void AsyncLoad<T> (ResourceID ID, System.Action<Object> loadedHandler) where T : Object
		{
			ResourceManager.Me.StartCoroutine (_Load (ID, typeof(T), loadedHandler));
		}
		
		#endregion
		
		#region ISyncLoaderStrategy implementation
		
		public Object Load (ResourceID ID)
		{
			return Load (ID, string.Empty);
		}
		
		public Object Load (ResourceID ID, string assetName)
		{
			Debug.LogError ("Load is no-used,use sload instead");
			return null;
//			return SLoad (ID.getRealPath);
		}
		
		public Object LoadScene (ResourceID ID)
		{
			return null;
		}
		
		public Object Load (ResourceID ID, System.Type resType, string assetName)
		{
			return Load (ID, resType);
		}
		
		public Object Load (ResourceID ID, System.Type resType)
		{
			Debug.LogError ("Load is no-used,use SLoadByType instead");
			return null;
//			return SLoadByType (ID.getRealPath, resType);
		}
		
		public T Load<T> (ResourceID ID, string assetName = null) where T : Object
		{
			Debug.LogError ("Load<T> is no-used,use SLoad<T> instead");
			return null;
//			return SLoad<T> (ID.getRealPath, assetName);
		}
		
		public TextAsset LoadScript (ResourceID ID)
		{
			Debug.LogError ("LoadScript is no-used,use SLoadScript instead");
			return null;
//			return SLoad<TextAsset> (ID.getRealPath);
		}
		
		public SharedLoadedAB GetSharedLoaded (string bundleName)
		{
			return null;
		}

		public Object SLoad (string ID)
		{
			return SLoadAsset (ID,null);
		}
		
		public Object SLoadAsset (string ID, string assetName)
		{
			Object res = null;
			if (_dicPrefabs.TryGetValue (ID, out res) == false) {
				res = Resources.Load (ID);
				if (res != null)
					_dicPrefabs.Add (ID, res);
			}
			return res;
		}
		
		public Object SLoadScene (string ID)
		{
			return null;
		}
		
		public Object SLoadByType (string ID, System.Type resType)
		{
			Object res = null;
			if (_dicPrefabs.TryGetValue (ID, out res) == false) {
				res = Resources.Load (ID, resType);
				if (res != null)
					_dicPrefabs.Add (ID, res);
			}
			return res;
		}
		
		public Object SLoadAssetByType (string ID, System.Type resType, string assetName)
		{
			return SLoadByType (ID, resType);
		}
		
		public T SLoad<T> (string ID, string assetName = null) where T : Object
		{
			T res = Resources.Load<T> (ID);
			return res;
		}
		
		public TextAsset SLoadScript (string ID)
		{
			return SLoad<TextAsset> (ID);
		}
		#endregion
	}
	
	public class AsyncResourceLoad
	{
		string _loadPath;
		ResourceRequest _rr;
		
		public string loadPath {
			get {
				return _loadPath;
			}
		}
		
		List<System.Action<Object>> _calls;
		
		public AsyncResourceLoad (string path, ResourceRequest rr)
		{
			_loadPath = path;
			_rr = rr;
		}
		
		public void AddLoadedCall (System.Action<Object> call)
		{
			if (_calls == null)
				_calls = new List<System.Action<Object>> ();
			_calls.Add (call);
		}

		public void LoadedCall ()
		{
			if (_calls != null && _rr != null) {
				Object asset = _rr.asset;
				for (int i=0; i<_calls.Count; i++) {
					_calls [i] (asset);
				}
			}
		}

		public void Dispose ()
		{
			_calls = null;
			_rr = null;
		}
	}
} // namespace RO
