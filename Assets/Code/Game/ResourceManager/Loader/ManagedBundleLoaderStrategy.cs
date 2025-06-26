using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RO
{
	public enum BundleLoadWay
	{
		CreateFromFile = 1,
		CreateFromMemory = 2,
		AsyncLoad = 3,
		//TODO
	}

	public partial class ManagedBundleLoaderStrategy : ILoaderStrategy
	{
		ABManager _abManager = new ABManager();

		public ABManager abManager
		{
			get
			{
				if (_abManager == null)
				{
					Debug.LogWarning("abManager is null.");
					_abManager = new ABManager();
				}
				return _abManager;
			}
		}

		const string RESOURCES_PROFIX = "resources/";
		static AssetBundleManifest assetBundleManifest = null;
		SDictionary<string, string[]> _dependencies = new SDictionary<string, string[]>();
		static SDictionary<string, string> _mapBundleName = new SDictionary<string, string>();
		static SDictionary<string, string> fullAssetBundleFileName = new SDictionary<string, string>();
		static SDictionary<string, string> sceneNameMapFull = new SDictionary<string, string>();
		static SDictionary<string, string> lowerPath = new SDictionary<string, string>();

		StringBuilder _sb = new StringBuilder();

		public ManagedBundleLoaderStrategy()
		{
			ReInit();
			InitManifest();
			InitResourceIDPath();
			InitAssetConfig();
		}

		void ReInit()
		{
			assetBundleManifest = null;
			_dependencies = new SDictionary<string, string[]>();
			_mapBundleName = new SDictionary<string, string>();
			fullAssetBundleFileName = new SDictionary<string, string>();
			sceneNameMapFull = new SDictionary<string, string>();
			lowerPath = new SDictionary<string, string>();
			System.GC.Collect();
		}

		string ToLowerString(string src)
		{
			string res = null;
			if (!lowerPath.TryGetValue(src, out res))
			{
				res = src.ToLower();
				lowerPath[src] = res;
			}
			return res;
		}

		/// <summary>
		/// 加载清单文件
		/// </summary>
		void InitManifest()
		{
			LoadAssetBundle(ApplicationHelper.platformFolder, true);
			if (assetBundleManifest != null)
			{
				string[] assetbundles = assetBundleManifest.GetAllAssetBundles();
				string fileName = "";
				for (int i = 0; i < assetbundles.Length; i++)
				{
					fileName = assetbundles[i];
					if (string.IsNullOrEmpty(fileName) == false)
					{
						fileName = Path.GetDirectoryName(fileName) + "/" + Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileName));
						fullAssetBundleFileName[fileName] = assetbundles[i];
						if (fileName.StartsWith("scene"))
						{
							sceneNameMapFull[Path.GetFileName(fileName)] = assetbundles[i];
						}
					}
				}
			}
		}

		/// <summary>
		/// 加载resourceID路径配置文件
		/// </summary>
		void InitResourceIDPath()
		{
#if LUA_FASTPACKING
			TextAsset t = Resources.Load ("AutoGenerate/pathIdMap") as TextAsset;
#else
			TextAsset t = SLoad<TextAsset>("AutoGenerate/pathIdMap");
#endif
			if (t == null)
			{
				Debug.Log("The TextAsset is null.");
				return;
			}

			ResourceID.ReMap(t.text, true);
		}

		void InitAssetConfig()
		{
			TextAsset t = SLoad<TextAsset>("AssetManageConfig/AssetManageConfig");
			_abManager.ResetConfig(AssetManageConfig.CreateByStr(t.text));
		}

		#region ILoaderStrategy implementation

		public void LateUpdate()
		{
			_abManager.LateUpdate();
		}

		public void Dispose()
		{
			_abManager.Dispose();
		}

		#endregion

		#region IUnLoaderStrategy implementation

		public void UnLoad(ResourceID ID, bool unloadAllLoadedObjects)
		{
			Debug.LogError("Unload is no-used");
			//			SUnLoad (ID.getRealPath, unloadAllLoadedObjects);
		}

		public void UnLoadScene(ResourceID ID, bool unloadAllLoadedObjects = false)
		{
			Debug.LogError("UnLoadScene is no-used");
			//			SUnLoadScene (ID.getRealPath, unloadAllLoadedObjects);
		}

		public void UnLoadAll(bool unloadAllLoadedObjects)
		{
			_abManager.UnLoadUnNeccessary();
			Resources.UnloadUnusedAssets();
		}

		public void SUnLoad(string ID, bool unloadAllLoadedObjects)
		{
			ID = ToLowerString(ID);
			string idStr = null;
			if (SGetMappedAssetBundleName(ID, out idStr, RESOURCES_PROFIX))
			{
				UnloadAssetBundle(idStr);
			}
		}

		public void SUnLoadScene(string ID, bool unloadAllLoadedObjects = false)
		{
			ID = ToLowerString(ID);
			string idStr = null;
			if (SGetMappedAssetBundleName(ID, out idStr, "scene/"))
			{
				UnloadAssetBundle(idStr);
			}
		}


		#endregion

		#region IAsyncLoaderStrategy implementation

		public void AsyncLoad(ResourceID ID, System.Action<Object> loadedHandler)
		{
			AsyncLoad(ID, null, loadedHandler);
		}

		public void AsyncLoad(ResourceID ID, System.Type resType, System.Action<Object> loadedHandler)
		{
			string path = null;
			if (GetMappedAssetBundleName(ID, out path, RESOURCES_PROFIX))
			{
				_AsyncLoad(path, resType, loadedHandler);
			}
		}

		public void AsyncLoad<T>(ResourceID ID, System.Action<Object> loadedHandler) where T : Object
		{
			AsyncLoad(ID, typeof(T), loadedHandler);
		}

		#endregion

		#region ISyncLoaderStrategy implementation

		public Object Load(ResourceID ID)
		{
			Debug.LogError("Load is no-used");
			return null;
			//			return SLoad (ID.getRealPath);
		}

		public Object Load(ResourceID ID, string assetName)
		{
			Debug.LogError("Load is no-used");
			return null;
			//			return SLoadAsset (ID.getRealPath,assetName);
		}

		public Object LoadScene(ResourceID ID)
		{
			Debug.LogError("LoadScene is no-used");
			return null;
			//			return SLoadScene (ID.getRealPath);
		}

		public Object Load(ResourceID ID, System.Type resType)
		{
			Debug.LogError("LoadScene is no-used");
			return null;
			//			return SLoadByType (ID.getRealPath, resType);
		}

		public Object Load(ResourceID ID, System.Type resType, string assetName)
		{
			Debug.LogError("LoadScene is no-used");
			return null;
			//			return SLoadAssetByType (ID.getRealPath,resType,assetName);
		}

		public T Load<T>(ResourceID ID, string assetName = null) where T : Object
		{
			Debug.LogError("Load<T> is no-used");
			return default(T);
			//			return SLoad<T> (ID.getRealPath, assetName);
		}

		public TextAsset LoadScript(ResourceID ID)
		{
			Debug.LogError("LoadScript is no-used");
			return null;
			//			return SLoadScript (ID.getRealPath);
		}

		public SharedLoadedAB GetSharedLoaded(string bundleName)
		{
			return _abManager.GetLoadedBundle(bundleName);
		}

		public Object SLoad(string ID)
		{
			return SLoadAsset(ID, null);
		}

		public Object SLoadAsset(string ID, string assetName)
		{
			ID = ToLowerString(ID);
			string path = null;
			bool newLoad = false;
			if (SGetMappedAssetBundleName(ID, out path, RESOURCES_PROFIX))
			{
				SharedLoadedAB loadedBundle = GetLoadedBundle(path);
				//Debug.Log("loadedBundle: " + loadedBundle);
				if (loadedBundle == null)
				{
					//Debug.Log("loadedBundle: Asset is not loaded.");
					loadedBundle = SInternalSyncLoad(ID, RESOURCES_PROFIX);
					newLoad = true;
				}
				loadedBundle.disposableObject.MapAssets();
				Object asset = loadedBundle.disposableObject.LoadAsset(assetName);
				_abManager.AfterLoad(loadedBundle);
				if (newLoad)
				{
					_abManager.AddGameObjectDependencies(asset as GameObject, loadedBundle);
				}
				//Debug.Log("SLoadAsset: ID=" + ID + ", assetName=" + assetName + ", path=" + path + ", newLoad=" + newLoad);
				//Debug.Log("SLoadAsset: Asset is loaded.");
				return asset;
			}
			//Debug.Log("SLoadAsset: Asset is not loaded.");
			return null;
		}

		public Object SLoadScene(string ID)
		{
			ID = ToLowerString(ID);
			SharedLoadedAB loaded = SInternalSyncLoad(ID, "scene/");
			if (loaded != null && loaded.disposableObject.bundle != null)
			{
				return loaded.disposableObject.bundle;
			}
			return null;
		}

		public Object SLoadByType(string ID, System.Type resType)
		{
			return SLoadAssetByType(ID, resType, null);
		}

		public Object SLoadAssetByType(string ID, System.Type resType, string assetName)
		{
			ID = ToLowerString(ID);
			string path = null;
			bool newLoad = false;
			if (SGetMappedAssetBundleName(ID, out path, RESOURCES_PROFIX))
			{
				SharedLoadedAB loadedBundle = GetLoadedBundle(path);
				if (loadedBundle == null)
				{
					loadedBundle = SInternalSyncLoad(ID, RESOURCES_PROFIX);
					newLoad = true;
				}
				loadedBundle.disposableObject.MapAssets();
				Object asset = loadedBundle.disposableObject.LoadAsset(assetName);
				GameObject go = null;
				if (resType != null && asset != null && asset is GameObject)
				{
					go = (GameObject)asset;
					asset = ((GameObject)asset).GetComponent(resType);
				}
				_abManager.AfterLoad(loadedBundle);
				if (newLoad && go != null)
				{
					_abManager.AddGameObjectDependencies(go, loadedBundle);
				}
				return asset;
			}
			return null;
		}

		public T SLoad<T>(string ID, string assetName = null) where T : Object
		{
			Object asset = SLoadAsset(ID, assetName);
			if (asset != null && asset is GameObject && typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
				return ((GameObject)asset).GetComponent<T>();
			return asset != null ? (T)asset : default(T);
		}


		public TextAsset SLoadScript(string ID)
		{
			ID = ToLowerString(ID);
			string fileName = _sb.Append("assets/resources/").Append(ID).Append(".txt").ToString();
			_sb.Length = 0;
			return SLoad<TextAsset>("script", fileName);
		}

		#endregion

		protected void UnloadAssetBundle(string assetBundleName, bool justBundle = false)
		{
			//Logger.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory before unloading " + assetBundleName);
			UnloadAssetBundleInternal(assetBundleName, justBundle);
			//Logger.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + assetBundleName);
		}

		void UnloadAssetBundleInternal(string assetBundleName, bool justBundle)
		{
			_abManager.UnLoadBundle(assetBundleName, false);
		}

		public static string AssetsURL
		{
			get
			{
#if ARCHIVE_AB
			    return Application.streamingAssetsPath + "/";
#else
#if UNITY_EDITOR
				string editorRoot = Path.Combine(Application.dataPath, "AssetBundles/");
				if (Directory.Exists(editorRoot))
				{
					return editorRoot + ApplicationHelper.platformFolder + "/";
				}
				else
				{
					return Application.streamingAssetsPath + "/";
				}
#else
				return Application.persistentDataPath + "/" + ApplicationHelper.platformFolder + "/";
#endif
#endif
			}
		}

		public static string AssetsPersistentURL
		{
			get
			{
#if ARCHIVE_AB
				return Application.persistentDataPath + "/" + ApplicationHelper.platformFolder + "/";
#else
#if UNITY_EDITOR
				string editorRoot = Path.Combine(Application.dataPath, "AssetBundles/");
				if (Directory.Exists(editorRoot))
				{
					return editorRoot + ApplicationHelper.platformFolder + "/";
				}
				else
				{
					return Application.persistentDataPath + "/" + ApplicationHelper.platformFolder + "/";
				}
#else
				return Application.persistentDataPath + "/" + ApplicationHelper.platformFolder + "/";
#endif
#endif
			}
		}

		AssetBundle SyncCreateBundle(string url, BundleLoadWay way, string id)
		{
			AssetBundle ab = BundleUnload.GetInstance().GetBundle(id);
			if (ab != null)
				return ab;
			switch (way)
			{
				case BundleLoadWay.CreateFromFile:
					return AssetBundle.LoadFromFile(url);
				case BundleLoadWay.CreateFromMemory:
					RO.LoggerUnused.Log("BundleLoadWay.CreateFromMemory :" + url);
					byte[] contents = File.ReadAllBytes(url);
					return AssetBundle.LoadFromMemory(contents);
			}
			return null;
		}

		SharedLoadedAB LoadAssetBundleInternal(string assetBundleName, bool isLoadingAssetBundleManifest, BundleLoadWay loadWay)
		{
			if (string.IsNullOrEmpty(assetBundleName))
				return null;
			SharedLoadedAB loaded = _abManager.GetLoadedBundle(assetBundleName);
			if (loaded == null)
			{
				string url = AssetsPersistentURL + assetBundleName;
				if (!File.Exists(url))
				{
					url = AssetsURL + assetBundleName;
				}
				AssetBundle ab = SyncCreateBundle(url, loadWay, assetBundleName);
				if (isLoadingAssetBundleManifest)
				{
					assetBundleManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
					loaded = _abManager.AddLoadedBundle(assetBundleName, ab);
					loaded.disposableObject.UnLoadBundle();
				}
				else
				{
					loaded = _abManager.AddLoadedBundle(assetBundleName, ab);
				}
			}
			return loaded;
		}

		List<SharedLoadedAB> LoadDependencies(string assetBundleName, BundleLoadWay loadWay)
		{
			if (assetBundleManifest == null)
			{
				RO.LoggerUnused.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
				return null;
			}
			// Get dependecies from the AssetBundleManifest object..
			string[] dependencies = _dependencies[assetBundleName];
			if (dependencies == null || dependencies.Length == 0)
			{
				dependencies = assetBundleManifest.GetAllDependencies(assetBundleName);
				_dependencies[assetBundleName] = dependencies;
			}
			if (dependencies.Length == 0)
				return null;
			List<SharedLoadedAB> depends = new List<SharedLoadedAB>();
			for (int i = 0; i < dependencies.Length; i++)
			{
				if (!string.IsNullOrEmpty(dependencies[i]))
				{
					depends.Add(LoadAssetBundleInternal(dependencies[i], false, loadWay));
				}
			}
			return depends;
		}

		protected SharedLoadedAB LoadAssetBundle(string assetBundleName, bool isLoadingAssetBundleManifest = false, BundleLoadWay selfWay = BundleLoadWay.CreateFromFile, BundleLoadWay dependsWay = BundleLoadWay.CreateFromFile)
		{
			bool isAlreadyProcessed = _abManager.IsLoaded(assetBundleName);
			List<SharedLoadedAB> dependBundles = null;
			if (!isAlreadyProcessed && !isLoadingAssetBundleManifest)
				dependBundles = LoadDependencies(assetBundleName, dependsWay);
			SharedLoadedAB loadedBundle = LoadAssetBundleInternal(assetBundleName, isLoadingAssetBundleManifest, selfWay);
			if (loadedBundle != null)
			{
				if (dependBundles != null)
				{
					SharedLoadedAB depend = null;
					for (int i = 0; i < dependBundles.Count; i++)
					{
						depend = dependBundles[i];
						loadedBundle.AddDependency(depend.info.assetBundleName, depend);
					}
				}
			}
			else
				RO.LoggerUnused.LogError("fuck??" + assetBundleName);
			return loadedBundle;
		}

		SharedLoadedAB InternalSyncLoad(ResourceID ID, string prefix = "", BundleLoadWay selfWay = BundleLoadWay.CreateFromFile, BundleLoadWay dependsWay = BundleLoadWay.CreateFromFile)
		{
			string idStr = null;
			SharedLoadedAB loaded;
			if (GetMappedAssetBundleName(ID, out idStr, prefix))
			{
				loaded = _abManager.GetLoadedBundle(idStr);
				if (loaded == null)
				{
					loaded = LoadAssetBundle(idStr, false, selfWay, dependsWay);
				}
				return loaded != null ? loaded : null;
			}
			return null;
		}

		SharedLoadedAB SInternalSyncLoad(string ID, string prefix = "", BundleLoadWay selfWay = BundleLoadWay.CreateFromFile, BundleLoadWay dependsWay = BundleLoadWay.CreateFromFile)
		{
			string idStr = null;
			SharedLoadedAB loaded;
			if (SGetMappedAssetBundleName(ID, out idStr, prefix))
			{
				loaded = _abManager.GetLoadedBundle(idStr);
				if (loaded == null)
				{
					loaded = LoadAssetBundle(idStr, false, selfWay, dependsWay);
				}
				return loaded != null ? loaded : null;
			}
			return null;
		}

		SharedLoadedAB GetLoadedBundle(string path)
		{
			SharedLoadedAB sab = _abManager.GetLoadedBundle(path);
			Debug.Log("sabBundle: " + sab);
			return sab;
		}

		bool GetMappedAssetBundleName(ResourceID ID, out string path, string prefix = "resources-")
		{
			Debug.LogError("GetMappedAssetBundleName is no-used");
			path = string.Empty;
			return false;
			//			path = _mapBundleName [ID.getRealPath];
			//			if (string.IsNullOrEmpty (path)) {
			//				path = prefix + ID.getRealPath;
			//				string fullPath = fullAssetBundleFileName [path];
			//				if (string.IsNullOrEmpty (fullPath) == false)
			//					path = fullPath;
			//				else {
			//					fullPath = sceneNameMapFull [ID.getRealPath];
			//					if (string.IsNullOrEmpty (fullPath) == false)
			//						path = fullPath;
			//				}
			//				_mapBundleName [ID.getRealPath] = path;
			//			}
			//			return path.EndsWith (".unity3d");
		}

		bool SGetMappedAssetBundleName(string ID, out string path, string prefix = "resources-")
		{
			path = _mapBundleName[ID];
			if (string.IsNullOrEmpty(path))
			{
				path = prefix + ID;
				string fullPath = fullAssetBundleFileName[path];
				if (string.IsNullOrEmpty(fullPath) == false)
					path = fullPath;
				else
				{
					fullPath = sceneNameMapFull[ID];
					if (string.IsNullOrEmpty(fullPath) == false)
						path = fullPath;
				}

				// Ensure the path ends with ".unity3d"
				if (!path.EndsWith(".unity3d"))
				{
					path += ".unity3d";
				}

				_mapBundleName[ID] = path;
				Debug.Log("path :" + path);
			}

			bool isLoaded = path.EndsWith(".unity3d");
			Debug.Log("SGetMappedAssetBundleName: isLoaded = " + isLoaded);

			return isLoaded;
		}


		public void UnLoadShareAB(string ID)
		{
			ID = ToLowerString(ID);
			string path = null;
			path = _mapBundleName[ID];
			if (null == path)
				return;
			_abManager.UnLoadBundle(path, true);
		}

	}
} // namespace RO
