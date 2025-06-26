using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RO
{
	public class DefaultABManager : IABManagement
	{
		protected SDictionary<BundleInfo,SharedLoadedAB> _cache;
		protected SDictionary<BundleInfo,SharedLoadedAB> _asyncNeedCheckMap;
		protected List<BundleInfo> _removes;

		public DefaultABManager ()
		{
			_cache = new SDictionary<BundleInfo, SharedLoadedAB> ();
			_asyncNeedCheckMap = new SDictionary<BundleInfo, SharedLoadedAB> ();
			_removes = new List<BundleInfo> ();
		}


		#region IABManagement implementation
		
		virtual public bool IsLoaded (BundleInfo key)
		{
			return _cache.ContainsKey (key);
		}
		
		virtual public SharedLoadedAB AddLoadedBundle (BundleInfo key, AssetBundle ab, bool asyncMode)
		{
			if (_cache [key] == null) {
				SharedLoadedAB sab = CreateSAB (key, ab, false);
				_cache [key] = sab;
				if (asyncMode) {
					_asyncNeedCheckMap [key] = sab;
				}
				return sab;
			}
			return null;
		}
		
		virtual public void UnLoadBundle (BundleInfo key, bool b)
		{
			UnLoadSharedAB (key);
			RemoveSharedAB (key);
		}
		
		virtual public SharedLoadedAB GetLoadedBundle (BundleInfo key)
		{
			return _cache [key];
		}
		
		virtual public void UnLoadReferenceSharedLoadedAB (SharedLoadedAB sab)
		{
		}

		virtual public void UnLoadUnNeccessary ()
		{
		}

		virtual public void AfterLoad (SharedLoadedAB sab)
		{
		}

		virtual public int LoadedSAB {
			get {
				return _cache.Count;
			}
		}

		virtual public int CachedBundle {
			get {
				int count = 0;
				foreach (KeyValuePair<BundleInfo,SharedLoadedAB> kvp in _cache) {
					if (kvp.Value.disposableObject.IsUnloaded == false) {
						count += 1;
					}
				}
				return count;
			}
		}

		virtual public void Update ()
		{
			foreach (KeyValuePair<BundleInfo,SharedLoadedAB> kvp in _asyncNeedCheckMap) {
				if (kvp.Value.disposableObject != null && kvp.Value.disposableObject.Isloaded && kvp.Value.AllDependsLoaded) {
					kvp.Value.disposableObject.StartAsyncLoadAssets ();
					_removes.Add (kvp.Key);
				}
			}
			int count = _removes.Count;
			if (count > 0) {
				for (int i=0; i<count; i++) {
					_asyncNeedCheckMap.Remove (_removes [i]);
				}
				_removes.Clear ();
			}
		}

		virtual public void Dispose ()
		{
			ManagedLoadedBundle ml = null;
			foreach (KeyValuePair<BundleInfo,SharedLoadedAB> kvp in _cache) {
				ml = kvp.Value.disposableObject;
				if (ml != null) {
					ml.Dispose ();
				}
			}
			_cache.Clear ();
		}

		#endregion

		virtual protected void UnLoadSharedAB (BundleInfo key)
		{
			SharedLoadedAB sab = _cache [key];
			if (sab != null) {
				sab.Dispose ();
			}
		}

		virtual protected void RemoveSharedAB (BundleInfo key)
		{
			_cache.Remove (key);
			if (_asyncNeedCheckMap.ContainsKey (key)) {
				_asyncNeedCheckMap.Remove (key);
			}
		}

		virtual protected SharedLoadedAB CreateSAB (BundleInfo key, AssetBundle ab, bool unloadAsset = false)
		{
			return new SharedLoadedAB (key, new ManagedLoadedBundle (ab, unloadAsset, key.assetBundleName));
		}
	}

	public class CustomABManager:DefaultABManager
	{
		public override void UnLoadBundle (BundleInfo key, bool b)
		{
			SharedLoadedAB cacheSource = GetLoadedBundle (key);
			if (cacheSource != null) {
				//no one reference to this bundle,but itself , we could auto unload it!
				if (cacheSource.referenceCount == 1) {
					RO.LoggerUnused.Log (string.Format ("CustomABManager<color=red>无依赖自动释放{0}</color>", key.assetBundleName));
					base.UnLoadBundle (key, false);
				}
			}
		}
		
		public override void UnLoadReferenceSharedLoadedAB (SharedLoadedAB sab)
		{
			if (sab != null && sab.info != null && sab.info.abManager == this) {
				UnLoadBundle (sab.info, false);
			}
		}
	}

	public class NeverUnloadABManager:DefaultABManager
	{
		public override void UnLoadBundle (BundleInfo key, bool b)
		{
			RO.LoggerUnused.Log ("fuck??" + key.assetBundleName);
		}

		public override void UnLoadReferenceSharedLoadedAB (SharedLoadedAB sab)
		{
			RO.LoggerUnused.Log ("UnLoadReferenceSharedLoadedAB " + sab.info.assetBundleName + " " + sab.referenceCount.ToString ());
		}

		public override int CachedBundle {
			get {
				return LoadedSAB;
			}
		}
	}

	public class UnLoadABImmdiatelyManager:DefaultABManager
	{
//		public override void UnLoadBundle (BundleInfo key, bool b)
//		{
//			if (JustUnloadBundle (key) == false) {
//				base.UnLoadBundle (key, b);
//			}
//		}
//
//		bool JustUnloadBundle (BundleInfo key)
//		{
//			if (key.assetInfo.manageAssetMode != AssetManageMode.UnLoadImmediately && key.assetInfo.manageAssetMode != AssetManageMode.Custom) {
//				SharedLoadedAB sab = GetLoadedBundle (key);
//				sab.disposableObject.Dispose ();
//				return true;
//			}
//			return false;
//		}

		public override void AfterLoad (SharedLoadedAB sab)
		{
			if (sab.disposableObject != null) {
				sab.disposableObject.UnLoadBundle ();
			}
		}

		public override void UnLoadReferenceSharedLoadedAB (SharedLoadedAB sab)
		{
		}

		public override void UnLoadUnNeccessary ()
		{
			List<BundleInfo> removes = new List<BundleInfo> ();
			foreach (KeyValuePair<BundleInfo,SharedLoadedAB> kvp in _cache) {
				if (kvp.Key.assetInfo.manageAssetMode == AssetManageMode.Custom) {
					removes.Add (kvp.Key);
					if (kvp.Value != null) {
						kvp.Value.disposableObject.Dispose ();
					}
				}
			}
			BundleInfo sab = null;

			for (int i=0; i<removes.Count; i++) {
				sab = removes [i];
				RemoveSharedAB (sab);
			}
		}

		public override int CachedBundle {
			get {
				return 0;
			}
		}
	}
} // namespace RO