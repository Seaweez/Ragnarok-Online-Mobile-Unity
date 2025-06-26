using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

#if UNITY_EDITOR	
using UnityEditor;
#endif


namespace RO
{
	public class Loader
	{
		public System.Collections.Queue callBacks = new System.Collections.Queue ();
		public WWW www;
		
		public void Dispose ()
		{
			www.Dispose ();
			www = null;
		}
	}
	
	public class BundleLoaderStrategy
	{
		BundleCacher _cacher = new BundleCacher ();
		static string[] m_Variants = {  };
		static AssetBundleManifest assetBundleManifest = null;
		static SDictionary<string,Object> _cacheAsset = new SDictionary<string, Object> ();
		static SDictionary<string, string[]> m_Dependencies = new SDictionary<string, string[]> ();
		static SDictionary<string,string> fullAssetBundleFileName = new SDictionary<string, string> ();
		static SDictionary<string,string> sceneNameMapFull = new SDictionary<string, string> ();
		
		public BundleLoaderStrategy ()
		{
			InitManifest ();
			InitResourceIDPath ();
		}

		/// <summary>
		/// 加载清单文件
		/// </summary>
		void InitManifest ()
		{
			LoadAssetBundle (ApplicationHelper.platformFolder, true);
			if (assetBundleManifest != null) {
				string[] assetbundles = assetBundleManifest.GetAllAssetBundles ();
				string fileName = "";
				for (int i=0; i<assetbundles.Length; i++) {
					fileName = assetbundles [i];
					if (string.IsNullOrEmpty (fileName) == false) {
						fileName = Path.GetDirectoryName (fileName) + "/" + Path.GetFileNameWithoutExtension (Path.GetFileNameWithoutExtension (fileName));
						fullAssetBundleFileName [fileName] = assetbundles [i];
						if (fileName.StartsWith ("scene")) {
							sceneNameMapFull [Path.GetFileName (fileName)] = assetbundles [i];
						}
					}
				}
			}
		}

		/// <summary>
		/// 加载resourceID路径配置文件
		/// </summary>
		void InitResourceIDPath ()
		{
//			TextAsset t = Load<TextAsset> (ResourceID.Make ("AutoGenerate/pathIdMap"));
//			ResourceID.ReMap (t != null ? t.text : null);
		}
		
		#region IUnLoaderStrategy implementation
		
		public void UnLoad (ResourceID ID, bool unloadAllLoadedObjects)
		{
			string idStr = "";
			if (GetMappedAssetBundleName (ID, out idStr, "resouces/")) {
				UnloadAssetBundle (idStr);
			}
		}
		
		public void UnLoadScene (ResourceID ID, bool unloadAllLoadedObjects = false)
		{
			string idStr = "";
			if (GetMappedAssetBundleName (ID, out idStr, "scene/")) {
				UnloadAssetBundle (idStr);
			}
		}
		
		public void UnLoadAll (bool unloadAllLoadedObjects)
		{
			_cacher.UnLoadAll (unloadAllLoadedObjects, 1);
//			m_Dependencies.Clear ();
			Resources.UnloadUnusedAssets ();
		}

		protected void JustUnloadBundle (ResourceID ID)
		{
			string idStr = "";
			if (GetMappedAssetBundleName (ID, out idStr, "resouces/")) {
				UnloadAssetBundle (idStr, true);
			}
		}
		
		protected void UnloadAssetBundle (string assetBundleName, bool justBundle = false, int priority = 999)
		{
			//Logger.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory before unloading " + assetBundleName);
			UnloadAssetBundleInternal (assetBundleName, justBundle, priority);
			UnloadDependencies (assetBundleName, justBundle, priority);
			//Logger.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + assetBundleName);
		}
		
		protected void UnloadDependencies (string assetBundleName, bool justBundle = false, int priority = 999)
		{
			string[] dependencies = null;
			if (!m_Dependencies.TryGetValue (assetBundleName, out dependencies))
				return;
			
			// Loop dependencies.
			foreach (var dependency in dependencies) {
				UnloadAssetBundleInternal (dependency, justBundle, priority);
			}
			if (!justBundle)
				m_Dependencies.Remove (assetBundleName);
		}
		
		protected void UnloadAssetBundleInternal (string assetBundleName, bool justBundle = false, int priority = 999)
		{
			LoadedAssetBundle bundle = null;
			_cacher.GetLoaded (assetBundleName, out bundle);
			if (bundle == null)
				return;
			if (!justBundle) {
				if (bundle.UnCount () <= 0) {
					_cacher.UnLoad (assetBundleName, false, priority);
//					Logger.Log ("AssetBundle " + assetBundleName + " has been unloaded successfully");
				}
			} else {
				bundle.TryUnloadBundle (false, priority);
//				Logger.Log ("just--AssetBundle " + assetBundleName + " has been unloaded successfully");
			}
		}
		
		#endregion
		
		#region IAsyncLoaderStrategy implementation
		
		public void AsyncLoad (ResourceID ID, System.Action<Object> loadedHandler)
		{
			throw new System.NotImplementedException ();
		}
		
		public void AsyncLoad (ResourceID ID, System.Type resType, System.Action<Object> loadedHandler)
		{
			throw new System.NotImplementedException ();
		}
		
		public void AsyncLoad<T> (ResourceID ID, System.Action<Object> loadedHandler) where T : Object
		{
			throw new System.NotImplementedException ();
		}
		
		#endregion
		
		#region ISyncLoaderStrategy implementation

		Object TryGetLoadedAsset (ResourceID ID, string prefix="resources/")
		{
			string bundleName = null;
			if (GetMappedAssetBundleName (ID, out bundleName, prefix)) {
				return _cacheAsset [bundleName];
			}
			return null;
		}

		void AddLoadedAsset (string name, Object asset)
		{
			_cacheAsset [name] = asset;
		}
		
		public Object Load (ResourceID ID)
		{
			return Load (ID, "");
		}
		
		public Object Load (ResourceID ID, string assetName)
		{
			Object asset = null;
			if (string.IsNullOrEmpty (assetName)) {
				asset = TryGetLoadedAsset (ID);
			}
			if (asset == null) {
				LoadedAssetBundle loaded = InternalDynamicLoad (ID, "resources/");
				if (loaded != null) {
					if (loaded.assetBundle != null) {
						AssetBundle ab = loaded.assetBundle;
						if (string.IsNullOrEmpty (assetName)) {
							asset = ab.mainAsset != null ? ab.mainAsset : ab.LoadAsset (ab.GetAllAssetNames () [0]);
							AddLoadedAsset (loaded.name, asset);
//							JustUnloadBundle (ID);
							_cacher.UnLoad (loaded.name, false, 1);
						} else {
							loaded.MapAssets ();
							asset = loaded.Load (assetName);
							loaded.TryUnloadBundle (false, 1);
						}
					} else
						asset = loaded.Load (assetName);
				}
			}
			return asset;
		}
		
		public Object LoadScene (ResourceID ID)
		{
			LoadedAssetBundle loaded = InternalDynamicLoad (ID, "scene/");
			if (loaded != null && loaded.assetBundle != null) {
				return loaded.assetBundle;
			}
			return null;
		}
		
		public Object Load (ResourceID ID, System.Type resType)
		{
			return Load (ID, resType, "");
		}
		
		public Object Load (ResourceID ID, System.Type resType, string assetName = null)
		{
			LoadedAssetBundle loaded = InternalDynamicLoad (ID, "resources/");
			if (loaded != null && loaded.assetBundle != null) {
				AssetBundle ab = loaded.assetBundle;
				if (string.IsNullOrEmpty (assetName)) {
					return ab.mainAsset != null ? ab.mainAsset : ab.LoadAsset (ab.GetAllAssetNames () [0]);
				} else {
					return ab.LoadAsset (assetName);
				}
			}
			return null;
		}
		
		public T Load<T> (ResourceID ID, string assetName = null) where T : Object
		{
			Object asset = Load (ID, assetName);
			if (asset != null && asset is GameObject && typeof(T).IsSubclassOf (typeof(MonoBehaviour)))
				return ((GameObject)asset).GetComponent<T> ();
			return asset != null ? (T)asset : default(T);
		}

		public TextAsset LoadScript (ResourceID ID)
		{
//			string fileName = ID.getRealPath;
//			fileName = "assets/resources/" + fileName + ".txt";
//			return Load<TextAsset> (ResourceID.Make ("script"), fileName);
			return null;
		}

		public SharedLoadedAB GetSharedLoaded (string bundleName)
		{
			return null;
		}
		
		#endregion
		
		#region ILoaderStrategy implementation

		public void LateUpdate()
		{

		}

		public void Dispose ()
		{
			_cacher.UnLoadAll (true);
			Resources.UnloadUnusedAssets ();
			_cacher = null;
			assetBundleManifest = null;
		}

		#endregion
		
		LoadedAssetBundle InternalDynamicLoad (ResourceID ID, string prefix = "")
		{
			LoadedAssetBundle loaded;
			string idStr = "";
			if (GetMappedAssetBundleName (ID, out idStr, prefix)) {
				if (!_cacher.GetLoaded (ID, out loaded)) {
					LoadAssetBundle (idStr);
					_cacher.GetLoaded (idStr, out loaded);
//					if (loaded == null || loaded.assetBundle == null)
//						return null;
				}
				return loaded;
			}
			return null;
		}
		
		bool GetMappedAssetBundleName (ResourceID ID, out string path, string prefix = "resources-")
		{
			path = string.Empty;
			return false;
		}
		
		public static string EditorRoot = "AssetBundles/";
		
		public static string AssetsURL {
			get { 
				#if UNITY_EDITOR
				if(Directory.Exists(Path.Combine(Application.dataPath,EditorRoot)))
					return Path.Combine(Application.dataPath,EditorRoot) + ApplicationHelper.platformFolder + "/";
				else
					return Application.streamingAssetsPath + "/" + ApplicationHelper.platformFolder + "/";
				#else
				return Application.persistentDataPath + "/" + ApplicationHelper.platformFolder + "/";
				#endif
			}
		}
		
		protected void LoadAssetBundle (string assetBundleName, bool isLoadingAssetBundleManifest = false)
		{
			if (!isLoadingAssetBundleManifest)
				assetBundleName = RemapVariantName (assetBundleName);
			
			// Check if the assetBundle has already been processed.
			bool isAlreadyProcessed = _cacher.IsLoaded (assetBundleName) || _cacher.IsLoading (assetBundleName);
			//				LoadAssetBundleInternal (assetBundleName, isLoadingAssetBundleManifest);
			
			// Load dependencies.
			if (!isAlreadyProcessed && !isLoadingAssetBundleManifest)
				LoadDependencies (assetBundleName);
			LoadAssetBundleInternal (assetBundleName, isLoadingAssetBundleManifest);
		}
		
		// Remaps the asset bundle name to the best fitting asset bundle variant.
		protected string RemapVariantName (string assetBundleName)
		{
			string[] bundlesWithVariant = assetBundleManifest.GetAllAssetBundlesWithVariant ();
			
			// If the asset bundle doesn't have variant, simply return.
			if (System.Array.IndexOf (bundlesWithVariant, assetBundleName) < 0)
				return assetBundleName;
			
			string[] split = assetBundleName.Split ('.');
			
			int bestFit = int.MaxValue;
			int bestFitIndex = -1;
			// Loop all the assetBundles with variant to find the best fit variant assetBundle.
			for (int i = 0; i < bundlesWithVariant.Length; i++) {
				string[] curSplit = bundlesWithVariant [i].Split ('.');
				if (curSplit [0] != split [0])
					continue;
				
				int found = System.Array.IndexOf (m_Variants, curSplit [1]);
				if (found != -1 && found < bestFit) {
					bestFit = found;
					bestFitIndex = i;
				}
			}
			
			if (bestFitIndex != -1)
				return bundlesWithVariant [bestFitIndex];
			else
				return assetBundleName;
		}
		
		// Where we actuall call WWW to download the assetBundle.
		protected bool LoadAssetBundleInternal (string assetBundleName, bool isLoadingAssetBundleManifest)
		{
			if (string.IsNullOrEmpty (assetBundleName))
				return false;
			// Already loaded.
			LoadedAssetBundle bundle = null;
			_cacher.GetLoaded (assetBundleName, out bundle);
			if (bundle != null) {
				bundle.Count ();
				return true;
			}
			
			// @TODO: Do we need to consider the referenced count of WWWs?
			// In the demo, we never have duplicate WWWs as we wait LoadAssetAsync()/LoadLevelAsync() to be finished before calling another LoadAssetAsync()/LoadLevelAsync().
			// But in the real case, users can call LoadAssetAsync()/LoadLevelAsync() several times then wait them to be finished which might have duplicate WWWs.
			if (_cacher.IsLoading (assetBundleName))
				return true;
			
			string url = AssetsURL + assetBundleName;
//			Logger.Log (url);
			if (File.Exists (url)) {
				AssetBundle ab = AssetBundle.LoadFromFile (url);
//				byte[] bytes = File.ReadAllBytes(url);
//				AssetBundle ab = AssetBundle.CreateFromMemoryImmediate (bytes);
				if (isLoadingAssetBundleManifest)
					assetBundleManifest = ab.LoadAsset<AssetBundleManifest> ("AssetBundleManifest");
				else {
					bundle = _cacher.AddLoaded (assetBundleName, ab);
				}
			} else
				RO.LoggerUnused.LogError ("未找到bundle.." + url);
			return false;
		}
		
		// Where we get all the dependencies and load them all.
		protected void LoadDependencies (string assetBundleName)
		{
			if (assetBundleManifest == null) {
				RO.LoggerUnused.LogError ("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
				return;
			}
			
			// Get dependecies from the AssetBundleManifest object..
			string[] dependencies = assetBundleManifest.GetAllDependencies (assetBundleName);
			if (dependencies.Length == 0)
				return;
			
			for (int i=0; i<dependencies.Length; i++)
				dependencies [i] = RemapVariantName (dependencies [i]);
			
			// Record and load all dependencies.
			m_Dependencies [assetBundleName] = dependencies;
			for (int i=0; i<dependencies.Length; i++)
				LoadAssetBundleInternal (dependencies [i], false);
		}
	}
} // namespace RO
