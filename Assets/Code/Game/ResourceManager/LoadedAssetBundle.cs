using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class LoadedAssetBundle
	{
		public static int BundleCount = 0;
		public AssetBundle assetBundle;
		public int referencedCount;
		public int priority = 1;

		public string name { get; private set; }

		SDictionary<string,Object> _assetMap = new SDictionary<string, Object> ();
		Object _mainAsset;

		public Object mainAsset {
			get {
				return _mainAsset;
			}
		}
		
		public LoadedAssetBundle (string name, AssetBundle assetBundle)
		{
			this.name = name;
			this.assetBundle = assetBundle;
			referencedCount = 1;
			if (assetBundle != null) {
				BundleCount ++;
//				Logger.Log ("bundle loaded:" + BundleCount.ToString ());
			}
		}

		public void MapAssets ()
		{
			if (assetBundle != null && _assetMap.Count == 0) {
				string[] names = assetBundle.GetAllAssetNames ();
				_mainAsset = assetBundle.mainAsset;
				if (_mainAsset == null && names.Length > 0)
					_mainAsset = assetBundle.LoadAsset (names [0]);
				for (int i=0; i<names.Length; i++) {
					Object obj = assetBundle.LoadAsset (names [i]);
					_assetMap [names [i]] = obj;
				}
			}
		}

		public Object Load (string name)
		{
			if (string.IsNullOrEmpty (name))
				return _mainAsset;
			return _assetMap [name.ToLower ()];
		}

		public void Unload (bool unloadAllLoadedObjects)
		{
			ForceUnloadBundle (unloadAllLoadedObjects);
			_mainAsset = null;
			_assetMap.Clear ();
		}

		public void TryUnloadBundle (bool unloadAllLoadedObjects, int priority)
		{
			if (priority >= this.priority)
				ForceUnloadBundle (unloadAllLoadedObjects);
		}

		public void ForceUnloadBundle (bool unloadAllLoadedObjects)
		{
			if (assetBundle != null)
				assetBundle.Unload (unloadAllLoadedObjects);
			BundleCount --;
//			Logger.Log ("AssetBundle " + name + " unloaded");
//			Logger.Log ("bundle loaded:" + BundleCount.ToString ());
		}

		public void Count ()
		{
			referencedCount++;
		}

		public int UnCount ()
		{
			return --referencedCount;
		}
	}
} // namespace RO
