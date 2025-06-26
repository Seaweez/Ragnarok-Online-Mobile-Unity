using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace RO
{
	public partial class ManagedBundleLoaderStrategy
	{
		public bool isLoaded (SharedLoadedAB sab)
		{
			if (sab != null) {
				// 1.check self is loaded
				bool selfLoaded = sab.disposableObject != null ? sab.disposableObject.Isloaded : false;
				if (selfLoaded) {
					// 2.check dependencies are loaded
					return sab.AllDependsLoaded;
				}
			}
			return false;
		}
	
		public void _AsyncLoad (string assetBundleName, System.Type resType, System.Action<Object> call, BundleLoadWay bundleLoadway = BundleLoadWay.AsyncLoad, BundleLoadWay dependsWay = BundleLoadWay.AsyncLoad)
		{
			SharedLoadedAB loadedBundle = GetLoadedBundle (assetBundleName);
			// 1. check already have
			if (loadedBundle == null) {
				List<SharedLoadedAB> dependBundles = AsyncLoadDependencies (assetBundleName);
				loadedBundle = AsyncLoadAssetBundleInternal (assetBundleName,true);
				if (loadedBundle != null) {
					if (dependBundles != null) {
						SharedLoadedAB depend = null;
						for (int i=0; i<dependBundles.Count; i++) {
							depend = dependBundles [i];
							loadedBundle.AddDependency (depend.info.assetBundleName, depend);
						}
					}
				} else
					RO.LoggerUnused.LogError ("fuck??" + assetBundleName);
			} else {
				// 2. check alread loaded
				if (isLoaded (loadedBundle)) {
					Object asset = loadedBundle.disposableObject.LoadAsset (null);
					if (asset != null) {
						if (call != null) {
							call (asset);
						}
						return;
					}
				}
			}
			loadedBundle.disposableObject.AddCalls (null,call);
		}

		// Where we get all the dependencies and load them all.
		protected List<SharedLoadedAB> AsyncLoadDependencies (string assetBundleName)
		{
			string[] dependencies = _dependencies [assetBundleName];
			if (dependencies == null || dependencies.Length == 0) {
				dependencies = assetBundleManifest.GetAllDependencies (assetBundleName);
				_dependencies [assetBundleName] = dependencies;
			}
			if (dependencies.Length == 0)
				return null;
			
			// Record and load all dependencies.
			List<SharedLoadedAB> depends = new List<SharedLoadedAB> ();
			for (int i=0; i<dependencies.Length; i++) {
				if (!string.IsNullOrEmpty (dependencies [i])) {
					depends.Add (AsyncLoadAssetBundleInternal (dependencies [i]));
				}
			}
			return depends;
		}

		SharedLoadedAB AsyncLoadAssetBundleInternal (string assetBundleName,bool flag = false)
		{
			if (string.IsNullOrEmpty (assetBundleName))
				return null;
			SharedLoadedAB loaded = _abManager.GetLoadedBundle (assetBundleName);
			if (loaded == null) {
				string url = AssetsURL + assetBundleName;
				if (File.Exists (url)) {
					loaded = _abManager.AddLoadedBundle (assetBundleName, null,flag);
					loaded.disposableObject.isAsyncMode = true;
					loaded.disposableObject.StartLoad (url);
					loaded.info.abManager.AfterLoad(loaded);
				} else
					RO.LoggerUnused.LogError ("未找到bundle.." + url);
			}
			return loaded;
		}
	}
} // namespace RO
