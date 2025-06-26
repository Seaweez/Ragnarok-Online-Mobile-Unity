using UnityEngine;
using System.Collections.Generic;
using Ghost.Utility;

namespace RO
{
	public class ABLRUManager :DefaultABManager
	{
		protected SDictionary<int,LRUCache<BundleInfo,SharedLoadedAB>> _LRUCache = new SDictionary<int, LRUCache<BundleInfo, SharedLoadedAB>> ();

		public ABLRUManager ()
		{
		}
	
		public void InitLRUCache (int id, int manageBundleLRUCount)
		{
			LRUCache<BundleInfo,SharedLoadedAB> cache = _LRUCache [id];
			if (cache == null) {
				cache = new LRUCache<BundleInfo, SharedLoadedAB> (manageBundleLRUCount);
				_LRUCache [id] = cache;
			} else {
				cache.capacity = manageBundleLRUCount;
			}
		}

		LRUCache<BundleInfo,SharedLoadedAB> GetMapCache (BundleInfo key)
		{
			AssetManageInfo info = key.assetInfo;
			if (info != null) {
				return _LRUCache [info.id];
			}
			return null;
		}

		#region IABManagement implementation

		public override bool IsLoaded (BundleInfo key)
		{
			return GetLoadedBundle (key) != null;
		}

		override public SharedLoadedAB AddLoadedBundle (BundleInfo key, AssetBundle ab,bool asyncMode)
		{
			LRUCache<BundleInfo,SharedLoadedAB> cache = GetMapCache (key);
			if (cache != null) {
				SharedLoadedAB sab = CreateSAB (key, ab);
				cache.Add (key, sab);
				return sab;
			}
			return null;
		}

		override public void UnLoadBundle (BundleInfo key, bool b)
		{
			LRUCache<BundleInfo,SharedLoadedAB> cache = GetMapCache (key);
			if (cache != null) {
				cache.Remove (key);
			}
		}

		override public SharedLoadedAB GetLoadedBundle (BundleInfo key)
		{
			LRUCache<BundleInfo,SharedLoadedAB> cache = GetMapCache (key);
			if (cache != null) {
				SharedLoadedAB loaded = null;
				if (cache.TryGetValue (key, out loaded)) {
					return loaded;
				}
			}
			return null;
		}

		public override void UnLoadUnNeccessary ()
		{
			foreach (KeyValuePair<int,LRUCache<BundleInfo,SharedLoadedAB>> kvp in _LRUCache) {
				kvp.Value.Clear ();
			}
		}

		override public void UnLoadReferenceSharedLoadedAB (SharedLoadedAB sab)
		{

		}

		public override int LoadedSAB {
			get {
				int count = 0;
				foreach (KeyValuePair<int,LRUCache<BundleInfo,SharedLoadedAB>> kvp in _LRUCache) {
					count += kvp.Value.cachedCount;
				}
				return count;
			}
		}

		public override int CachedBundle {
			get {
				return 0;
			}
		}

		public override void AfterLoad (SharedLoadedAB sab)
		{
			if (sab.disposableObject != null) {
				sab.disposableObject.UnLoadBundle ();
			}
		}

		public override void Dispose ()
		{
			base.Dispose ();
			UnLoadUnNeccessary ();
		}
		#endregion
	}
} // namespace RO
