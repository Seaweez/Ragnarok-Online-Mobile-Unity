using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	public class AutoUnloadABCachePool
	{
		int _maxCount;
		Action<SharedLoadedAB> _removeCallBack;
		Dictionary<BundleInfo,SharedLoadedAB> _cache;
		List<BundleInfo> _keyQueue;

		public AutoUnloadABCachePool (int maxCount, Action<SharedLoadedAB> removeCallBack)
		{
			_maxCount = Mathf.Max (maxCount, 10);
			_removeCallBack = removeCallBack;
			_cache = new Dictionary<BundleInfo, SharedLoadedAB> ();
			_keyQueue = new List<BundleInfo> ();
		}

		public void ResetMaxCount(int maxCount)
		{
			if (maxCount > 0) {
				_maxCount = maxCount;
				Trim();
			}
		}

		public void Clear()
		{
			int tmp = _maxCount;
			_maxCount = 0;
			Trim();
			_maxCount = tmp;
		}

		public int Count
		{
			get{
				return _cache.Count;
			}
		}
	
		public void Put (BundleInfo key, SharedLoadedAB sab)
		{
			if (_cache.ContainsKey (key) == false) {
				_cache.Add (key, sab);
			} else {
				_cache [key] = sab;
			}
			AddOrRefreshKey (key);
		}

		public SharedLoadedAB Get (BundleInfo key)
		{
			SharedLoadedAB sab = null;
			if (_cache.TryGetValue (key, out sab)) {
				_cache.Remove (key);
			}
			RemoveKey (key);
			return sab;
		}

		protected void FireOnRemoveSAB (SharedLoadedAB sab)
		{
			if (_removeCallBack != null && sab != null) {
				_removeCallBack (sab);
			}
		}

		protected bool RemoveKey (BundleInfo key)
		{
			return _keyQueue.Remove (key);
		}

		protected void AddOrRefreshKey (BundleInfo key)
		{
			if (_keyQueue.Count > 0 && _keyQueue [_keyQueue.Count - 1] == key) {
				return;
			}
			bool removed = RemoveKey (key);
			_keyQueue.Add (key);
			if (removed == false) {
				Trim ();
			}
		}

		protected void Trim ()
		{
			int keyCount = _keyQueue.Count;
			int needRemoveCount = keyCount - _maxCount;
			if (needRemoveCount > 0) {
				SharedLoadedAB needRemove = null;
				for (int i=0; i<needRemoveCount; i++) {
					if (_cache.TryGetValue (_keyQueue [0], out needRemove)) {
						_cache.Remove (_keyQueue [0]);
						_keyQueue.RemoveAt (0);
						FireOnRemoveSAB (needRemove);
					}
				}
			}
		}
	}
} // namespace RO
