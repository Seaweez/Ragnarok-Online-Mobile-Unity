using UnityEngine;
using System.Collections.Generic;
using System;
using Ghost.Utility;
using System.Text;
using System.Collections;

namespace RO
{
	public class ManagedLoadedBundle :IDisposable
	{
		public AssetBundle bundle{ get; private set; }

		public bool disposeUnLoadAllLoaded{ get; private set; }

		public bool IsUnloaded {
			get;
			set;
		}

		public bool Isloaded {
			get;
			set;
		}

		public bool isAsyncMode = false;
		string _bundleName;
		SDictionary<string,UnityEngine.Object> _assetMap;
		UnityEngine.Object _mainAsset;
		bool _mappedAssets;
		Dictionary<string,List<Action<UnityEngine.Object>>> _assetCalls;
		List<Action<UnityEngine.Object>> _mainAssetCalls;
		Action<UnityEngine.Object> _firstTimeLoadedAsset;
		Coroutine _co;
		bool _mainAssetNeedAddRefCount;

		public Action<UnityEngine.Object> firstTimeLoadedAsset {
			get {
				return _firstTimeLoadedAsset;
			}
		}

		public UnityEngine.Object mainAsset {
			get {
				return _mainAsset;
			}
		}

		public ManagedLoadedBundle (AssetBundle bundle, bool disposeUnloadLoaded, string bundleName)
		{
			_bundleName = bundleName;
			this.bundle = bundle;
			this.disposeUnLoadAllLoaded = disposeUnloadLoaded;
			Isloaded = (this.bundle != null);
		}

		public void MapAssets ()
		{
			if (_mappedAssets)
				return;
			_mappedAssets = true;
			if (bundle != null) {
//				Profiler.BeginSample ("MapAssets");
				string[] names = bundle.GetAllAssetNames ();
				_mainAsset = bundle.mainAsset;
				if (_mainAsset == null && names.Length > 0)
					_mainAsset = bundle.LoadAsset (names [0]);
				if (names.Length > 0) {
					_assetMap = new SDictionary<string, UnityEngine.Object> ();
					for (int i=0; i<names.Length; i++) {
						UnityEngine.Object obj = bundle.LoadAsset (names [i]);
						_assetMap [names [i]] = obj;
					}
				}
//				Profiler.EndSample ();
			}
		}

		public void MainAssetAddRefCount ()
		{
			if (_mainAsset != null) {
				_mainAssetNeedAddRefCount = false;
				GameObject go = _mainAsset as GameObject;
				if (go != null) {
					SharedABRefCounter counter = go.GetComponent<SharedABRefCounter> ();
					if (counter == null) {
						counter = go.AddComponent<SharedABRefCounter> ();
					}
					counter.AddDepends (_bundleName);
				}
			} else
				_mainAssetNeedAddRefCount = true;
		}

		public UnityEngine.Object LoadAsset (string assetName)
		{
			if (string.IsNullOrEmpty (assetName))
				return _mainAsset;
			return _assetMap [assetName.ToLower ()];
		}

		public void UnLoadAssets ()
		{
			if (_assetMap != null) {
				foreach (KeyValuePair<string,UnityEngine.Object> kvp in _assetMap) {
					if (kvp.Value != null) {
						UnLoadAsset (kvp.Value);
					}
				}
			}
			_assetMap = null;
			if (_mainAsset != null) {
				UnLoadAsset (_mainAsset);
			}
			_mainAsset = null;
		}

		protected void UnLoadAsset (UnityEngine.Object obj)
		{
			if ((obj is GameObject) == false && (obj is Component) == false && (obj is AssetBundle) == false) {
				Resources.UnloadAsset (obj);
			} else {
				if (ResourceManager.Instance != null) {
					ResourceManager.Instance.LateDestroyAsset (obj);
				}
			}
		}


        public void UnLoadBundle ()
		{
            if (bundle != null)
            {
                if (!_bundleName.Contains("gui/pic"))
                    bundle.Unload(disposeUnLoadAllLoaded);
                else
                    BundleUnload.GetInstance().Add(_bundleName, bundle, disposeUnLoadAllLoaded);

                
            }
			bundle = null;
			IsUnloaded = true;
		}

		public void AddCalls (string assetName, Action<UnityEngine.Object> call)
		{
			List<Action<UnityEngine.Object>> calls = null;
			if (string.IsNullOrEmpty (assetName)) {
				_mainAssetCalls = new List<Action<UnityEngine.Object>> ();
				calls = _mainAssetCalls;
			} else {
				if (_assetCalls == null)
					_assetCalls = new Dictionary<string, List<Action<UnityEngine.Object>>> ();
				if (!_assetCalls.TryGetValue (assetName, out calls)) {
					calls = new List<Action<UnityEngine.Object>> ();
					_assetCalls.Add (assetName, calls);
				}
			}
			calls.Add (call);
		}

		public void SetFirstTimeLoadedAsset (Action<UnityEngine.Object> call)
		{
			_firstTimeLoadedAsset = call;
		}

		public void StartLoad (string url)
		{
			#if UNITY_EDITOR || UNITY_IPHONE
			url = "file://"+url;
			#endif
			_co = ResourceManager.Me.StartCoroutine (_LoadBundle (url));
		}

		public void CancelLoad ()
		{
			if (_co != null) {
				ResourceManager.Me.StopCoroutine (_co);
			}
			_co = null;
		}

		IEnumerator _LoadBundle (string url)
		{
			WWW www = new WWW (url);
			yield return www;
			bundle = www.assetBundle;
			Isloaded = true;
			www.Dispose ();
			_co = null;
		}

		public void StartAsyncLoadAssets ()
		{
			if (this.bundle != null) {
				//for test
				int count = _mainAssetCalls != null ? 1 : 0;
				if (_assetCalls != null) {
					count += _assetCalls.Count;
				}
//				Debug.LogFormat ("{0}开始加载资源 total:{1}", _bundleName, count);
				_co = ResourceManager.Me.StartCoroutine (_LoadAssets ());
			}
		}

		IEnumerator _LoadAssets ()
		{
			List<Action<UnityEngine.Object>> calls = _mainAssetCalls;
			if (calls != null && _mainAsset == null) {
				// 1 load main
				string[] names = bundle.GetAllAssetNames ();
				_mainAsset = bundle.mainAsset;
				if (_mainAsset == null && names.Length > 0) {
					AssetBundleRequest abr = bundle.LoadAssetAsync (names [0]);
					yield return abr;
					_mainAsset = abr.asset;
					if (_firstTimeLoadedAsset != null) {
						_firstTimeLoadedAsset (_mainAsset);
					}
					if (_mainAssetNeedAddRefCount) {
						MainAssetAddRefCount ();
					}
				}
				if (calls != null) {
					for (int i=0; i<calls.Count; i++) {
						if (calls [i] != null) {
							calls [i] (_mainAsset);
						}
					}
				}
			}
			if (_assetCalls != null) {
				// 2 load other asset
				UnityEngine.Object obj = null;
				_assetMap = new SDictionary<string, UnityEngine.Object> ();
				foreach (KeyValuePair<string, List<Action<UnityEngine.Object>>> kvp in _assetCalls) {
					obj = LoadAsset (kvp.Key);
					if (obj == null) {
						AssetBundleRequest abr = bundle.LoadAssetAsync (kvp.Key);
						yield return abr;
						obj = abr.asset;
						if (_firstTimeLoadedAsset != null) {
							_firstTimeLoadedAsset (obj);
						}
						_assetMap [kvp.Key] = obj;
					}
					calls = kvp.Value;
					if (calls != null) {
						for (int i=0; i<calls.Count; i++) {
							if (calls [i] != null) {
								calls [i] (obj);
							}
						}
					}
				}
			}
			_co = null;
			if (IsUnloaded) {
				UnLoadBundle ();
			}
		}

		#region IDisposable implementation
		public void Dispose ()
		{
			UnLoadBundle ();
			UnLoadAssets ();
			CancelLoad ();
			_mainAssetCalls = null;
			_assetCalls = null;
			_firstTimeLoadedAsset = null;
		}
		#endregion
	}
} // namespace RO
