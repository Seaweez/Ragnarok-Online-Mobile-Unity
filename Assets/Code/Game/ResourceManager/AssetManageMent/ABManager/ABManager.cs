using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class BundleInfo
	{
		public string assetBundleName;
		public AssetManageInfo assetInfo;
		public IABManagement abManager;
	}

	public interface IABManagement
	{
		bool IsLoaded (BundleInfo key);

		SharedLoadedAB AddLoadedBundle (BundleInfo key, AssetBundle ab,bool asyncMode);

		void UnLoadBundle (BundleInfo key, bool b);

		SharedLoadedAB GetLoadedBundle (BundleInfo key);

		void UnLoadReferenceSharedLoadedAB (SharedLoadedAB sab);

		void UnLoadUnNeccessary ();

		int LoadedSAB{ get; }

		int CachedBundle{ get; }

		void AfterLoad (SharedLoadedAB sab);

		void Update();

		void Dispose();
	}

	public class ABManager:IABManagement
	{
		AssetManageConfig _config;
		SDictionary<string,BundleInfo> _mapAssetInfo = new SDictionary<string, BundleInfo> ();
		SDictionary<AssetManageMode,IABManagement> _modeABManager = new SDictionary<AssetManageMode, IABManagement> ();
		AutoUnloadCachedPoolABManager _autoUnloadCachedPoolABmanager;

		public SDictionary<AssetManageMode, IABManagement> modeABManager {
			get {
				return _modeABManager;
			}
		}

		public ABManager ()
		{
			_modeABManager [AssetManageMode.NeverUnLoad] = new NeverUnloadABManager ();
			_modeABManager [AssetManageMode.LRU] = new ABLRUManager ();
			_modeABManager [AssetManageMode.AutoUnloadNoDepends] = new AutoUnloadABManager ();
			_modeABManager [AssetManageMode.Custom] = new CustomABManager ();
			_modeABManager [AssetManageMode.UnLoadImmediately] = new UnLoadABImmdiatelyManager ();
			_modeABManager [AssetManageMode.ResourceAutoUnloadNoDepends] = new ResourceAutoUnloadABManager ();
			//
			_autoUnloadCachedPoolABmanager = new AutoUnloadCachedPoolABManager ();
			_modeABManager [AssetManageMode.AutoUnloadNoDependsCachePool] = _autoUnloadCachedPoolABmanager;
		}

		public void ResetConfig (AssetManageConfig config)
		{
			_config = config;
			Init ();
		}

		void Init ()
		{
			ABLRUManager manager = _modeABManager [AssetManageMode.LRU] as ABLRUManager;
			for (int i=0; i<_config.infos.Count; i++) {
				InitWithInfo (_config.infos [i], manager);
			}
			if (_autoUnloadCachedPoolABmanager != null) {
				_autoUnloadCachedPoolABmanager.ResetPoolMaxNum (_config.cachePoolMaxNum);
			}
		}
		
		void InitWithInfo (AssetManageInfo info, ABLRUManager manager)
		{
			if (info.subs != null) {
				AssetManageInfo sub;
				for (int i=0; i<info.subs.Count; i++) {
					sub = info.subs [i];
					if (sub.manageBundleMode == AssetManageMode.LRU) {
						manager.InitLRUCache (sub.id, sub.manageBundleLRUCount);
					}
					InitWithInfo (sub, manager);
				}
			}
			if (info.manageBundleMode == AssetManageMode.LRU) {
				manager.InitLRUCache (info.id, info.manageBundleLRUCount);
			}
		}

		public BundleInfo GetCacheBundleInfo (string key)
		{
			BundleInfo info = _mapAssetInfo [key];
			if (info == null) {
				info = new BundleInfo ();
				info.assetBundleName = key;
				if (_config != null) {
					info.assetInfo = _config.GetInfo (key);
					info.abManager = _modeABManager [info.assetInfo.manageBundleMode];
				} else {
					info.assetInfo = AssetManageInfo.Default;
					info.abManager = _modeABManager [AssetManageMode.UnLoadImmediately];
				}
				_mapAssetInfo [key] = info;
			}
			return info;
		}

		public void LateUpdate()
		{
			if (_autoUnloadCachedPoolABmanager != null) {
				_autoUnloadCachedPoolABmanager.LateUpdate();
			}
			Update ();
		}

		public void AfterLoad (SharedLoadedAB sab)
		{

			if (sab != null && sab.info != null && sab.info.abManager != null) {
				sab.info.abManager.AfterLoad (sab);
			}
		}

		public void AddGameObjectDependencies (GameObject go, SharedLoadedAB sab)
		{
			if (go != null) {
				if (sab.autoUnloadDepends != null) {
					sab.LoadedAssetAddRefCount(go);
				}
			}
		}

		public void UnLoadUnNeccessary ()
		{
			foreach (KeyValuePair<AssetManageMode,IABManagement> kvp in _modeABManager) {
				kvp.Value.UnLoadUnNeccessary ();
			}
		}

		public bool IsLoaded (string key)
		{
			return IsLoaded (GetCacheBundleInfo (key));
		}
		
		public SharedLoadedAB GetLoadedBundle (string key)
		{
			return GetLoadedBundle (GetCacheBundleInfo (key));
		}
		
		public SharedLoadedAB AddLoadedBundle (string key, AssetBundle ab,bool flag = false)
		{
			return AddLoadedBundle (GetCacheBundleInfo (key), ab,flag);
		}
		
		public void UnLoadBundle (string key, bool b)
		{
			UnLoadBundle (GetCacheBundleInfo (key), b);
		}
		
		#region IABManagement implementation

		public bool IsLoaded (BundleInfo key)
		{
			return key.abManager.IsLoaded (key);
		}

		public SharedLoadedAB AddLoadedBundle (BundleInfo key, AssetBundle ab,bool asyncMode = false)
		{
			return key.abManager.AddLoadedBundle (key, ab, asyncMode);
		}

		public void UnLoadBundle (BundleInfo key, bool b)
		{
			key.abManager.UnLoadBundle (key, b);
		}

		public SharedLoadedAB GetLoadedBundle (BundleInfo key)
		{
			return key.abManager.GetLoadedBundle (key);
		}

		public void UnLoadReferenceSharedLoadedAB (SharedLoadedAB sab)
		{
			if (sab != null) {
				sab.info.abManager.UnLoadReferenceSharedLoadedAB (sab);
			}
		}

		public int LoadedSAB {
			get {
				return 0;
			}
		}

		public int CachedBundle {
			get {
				return 0;
			}
		}

		virtual public void Update()
		{
			foreach (KeyValuePair<AssetManageMode, IABManagement> kvp in _modeABManager) {
				kvp.Value.Update();
			}
		}

		virtual public void Dispose()
		{
			if (_modeABManager.Count > 0) {
				_modeABManager [AssetManageMode.NeverUnLoad].Dispose();
				_modeABManager [AssetManageMode.LRU].Dispose();
				_modeABManager [AssetManageMode.Custom].Dispose();
				_modeABManager [AssetManageMode.UnLoadImmediately].Dispose();
				_modeABManager [AssetManageMode.AutoUnloadNoDepends].Dispose();
				_modeABManager [AssetManageMode.ResourceAutoUnloadNoDepends].Dispose();
				_autoUnloadCachedPoolABmanager.Dispose ();
			}
		}

		#endregion


	}
} // namespace RO
