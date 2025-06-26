using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class BundleCacher
	{
		private SDictionary<string, Loader> _loadingRes = new SDictionary<string, Loader> ();
		private SDictionary<string, LoadedAssetBundle> _preparedRes = new SDictionary<string, LoadedAssetBundle> ();
	
		public void UnLoadAll (bool unloadAllLoadedObjects, int priority = 9999)
		{
			List<string> removed = new List<string> ();
			foreach (KeyValuePair<string, LoadedAssetBundle> ab in _preparedRes) {
				if (ab.Value.priority <= priority) {
					ab.Value.Unload (unloadAllLoadedObjects);
					removed.Add (ab.Key);
				}
			}
			for (int i=0; i<removed.Count; i++) {
				_preparedRes.Remove (removed [i]);
			}
//			_preparedRes.Clear ();
		}

		public void UnLoad (ResourceID ID, bool unloadAllLoadedObjects, int priority = 9999)
		{
//			UnLoad (ID.getRealPath, unloadAllLoadedObjects, priority);
		}

		public void UnLoad (string IDStr, bool unloadAllLoadedObjects, int priority = 9999)
		{
			LoadedAssetBundle ab = null;
			if (_preparedRes.TryGetValue (IDStr, out ab)) {
				if (ab.priority <= priority) {
					ab.Unload (unloadAllLoadedObjects);
					_preparedRes.Remove (IDStr);
				}
			}
		}

		public bool GetLoaded (ResourceID ID, out LoadedAssetBundle ab)
		{
			return GetLoaded (string.Empty, out ab);
		}

		public bool GetLoaded (string IDStr, out LoadedAssetBundle loadedBundle)
		{
			if (_preparedRes.TryGetValue (IDStr, out loadedBundle)) {
//				loadedBundle.Count ();
//				Debuger.Log (string.Format ("{0} times loaded : {1}", loadedBundle.referencedCount, IDStr));
			} else
				loadedBundle = null;
			return loadedBundle != null;
		}

		public bool GetLoading (ResourceID ID, out Loader loader)
		{
			return GetLoading (string.Empty, out loader);
		}

		public bool GetLoading (string IDStr, out Loader loader)
		{
			return _loadingRes.TryGetValue (IDStr, out loader);
		}

		public bool IsLoading (string IDStr)
		{
			return _loadingRes.ContainsKey (IDStr);
		}

		public bool IsLoaded (string IDStr)
		{
			return _preparedRes.ContainsKey (IDStr);
		}

		public void AddLoading (ResourceID ID, Loader loader)
		{
//			AddLoading (ID.getRealPath, loader);
		}

		public void AddLoading (string IDStr, Loader loader)
		{
			_loadingRes.Add (IDStr, loader);
		}

		public LoadedAssetBundle AddLoaded (ResourceID ID, AssetBundle ab)
		{
//			return AddLoaded (ID.getRealPath, ab);
			return null;
		}

		public LoadedAssetBundle AddLoaded (string IDStr, AssetBundle ab)
		{
			LoadedAssetBundle cab = _preparedRes [IDStr];
			if (cab == null) {
				cab = new LoadedAssetBundle (IDStr, ab);
				if (IDStr.Contains ("/gui/atlas") || IDStr.Contains ("script.unity3d") || IDStr.Contains("resources/public/shader"))
					cab.priority = 10;
				_preparedRes.Add (IDStr, cab);
				_loadingRes.Remove (IDStr);
			}
			return cab;
		}
	}
} // namespace RO
