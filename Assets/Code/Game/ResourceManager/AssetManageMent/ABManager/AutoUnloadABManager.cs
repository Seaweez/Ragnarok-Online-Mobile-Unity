using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class AutoUnloadABManager : DefaultABManager
	{
		override protected SharedLoadedAB CreateSAB (BundleInfo key, AssetBundle ab, bool unloadAsset = false)
		{
			return new SharedLoadedAB (key, new ManagedLoadedBundle (ab, true, key.assetBundleName));
		}

		public override void UnLoadBundle (BundleInfo key, bool b)
		{
			SharedLoadedAB cacheSource = GetLoadedBundle (key);
			if (cacheSource != null) {
				//no one reference to this bundle,but itself , we could auto unload it!
//				Debug.LogFormat ("{0} referenceCount:{1}", key.assetBundleName, cacheSource.referenceCount);
				if (cacheSource.referenceCount == 1) {
//					Logger.Log (string.Format ("<color=red>无依赖自动释放{0}</color>", key.assetBundleName));
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

	public class ResourceAutoUnloadABManager: AutoUnloadABManager
	{

		override protected SharedLoadedAB CreateSAB (BundleInfo key, AssetBundle ab, bool unloadAsset = false)
		{
			return new SharedLoadedAB (key, new ManagedLoadedBundle (ab, false, key.assetBundleName));
		}

		public override void AfterLoad (SharedLoadedAB sab)
		{
			if (sab.disposableObject != null) {
				sab.disposableObject.MainAssetAddRefCount();
				sab.disposableObject.UnLoadBundle ();
			}
		}
	}

	public class AutoUnloadCachedPoolABManager : DefaultABManager
	{
		AutoUnloadABCachePool _cachedPool;

		public AutoUnloadCachedPoolABManager (int poolMaxNum = 30):base()
		{
			_cachedPool = new AutoUnloadABCachePool (poolMaxNum, RealUnloadSAB);
		}

		public void ResetPoolMaxNum (int poolMaxNum)
		{
			if (poolMaxNum > 0) {
				RO.LoggerUnused.LogFormat ("重设 AutoUnloadCachedPoolABManager _cachedPool max : {0}", poolMaxNum);
				_cachedPool.ResetMaxCount (poolMaxNum);
			}
		}

		override protected SharedLoadedAB CreateSAB (BundleInfo key, AssetBundle ab, bool unloadAsset = false)
		{
			return new SharedLoadedAB (key, new ManagedLoadedBundle (ab, true, key.assetBundleName));
		}

		void RealUnloadSAB (SharedLoadedAB obj)
		{
			if (obj != null) {
				RO.LoggerUnused.LogFormat ("{0} 释放ab包", obj.info.assetBundleName);
				obj.Dispose ();
				_cache.Remove (obj.info);
			}
		}

		public override SharedLoadedAB GetLoadedBundle (BundleInfo key)
		{
			SharedLoadedAB sab = base.GetLoadedBundle (key);
			if (sab == null) {
				sab = _cachedPool.Get (key);
				if (sab != null) {
//					Logger.LogFormat ("{0} 从待销毁池获取", key.assetBundleName);
					_cache [key] = sab;
				}
			}
			return sab;
		}
		
		public override void UnLoadBundle (BundleInfo key, bool b)
		{
			SharedLoadedAB cacheSource = base.GetLoadedBundle (key);
			if (cacheSource != null) {
				//no one reference to this bundle,but itself , we could auto unload it!
				//				Debug.LogFormat ("{0} referenceCount:{1}", key.assetBundleName, cacheSource.referenceCount);
				if (cacheSource.referenceCount == 1) {
					//					Logger.Log (string.Format ("<color=red>无依赖自动释放{0}</color>", key.assetBundleName));
//					base.UnLoadBundle (key, false);
//					Logger.LogFormat ("{0} 进入待销毁池", key.assetBundleName);
					_cache.Remove (key);
					_cachedPool.Put (key, cacheSource);
				}
			}
		}

		public void LateUpdate ()
		{
		}
		
		public override void UnLoadReferenceSharedLoadedAB (SharedLoadedAB sab)
		{
			if (sab != null && sab.info != null && sab.info.abManager == this) {
				UnLoadBundle (sab.info, false);
			}
		}
	
		public override int CachedBundle {
			get {
				int baseCount = base.CachedBundle;
				return baseCount + _cachedPool.Count;
			}
		}

		public override int LoadedSAB {
			get {
				return base.LoadedSAB + _cachedPool.Count;
			}
		}

		public override void Dispose ()
		{
			base.Dispose ();
			_cachedPool.Clear ();
		}
	}
} // namespace RO
