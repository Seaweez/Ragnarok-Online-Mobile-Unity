using UnityEngine;
using System.Collections.Generic;
using Ghost.Utility;
using System;

namespace RO
{
	public class SharedLoadedAB : ResourceHolder
	{
		public BundleInfo info{ get; private set; }

		SDictionary<string,SharedLoadedAB> _dependencies;
		protected List<string> _autoUnloadDepends;

		public List<string> autoUnloadDepends {
			get {
				return _autoUnloadDepends;
			}
		}

		class RefCount
		{
			public int count = 0;
		}

		private ManagedLoadedBundle obj;
		private RefCount refCount;
		
		public ManagedLoadedBundle disposableObject {
			get {
				return obj;
			}
		}
		
		public int referenceCount {
			get {
				return refCount.count;
			}
		}

		public SharedLoadedAB (BundleInfo info, ManagedLoadedBundle disposable)
		{
			this.info = info;
			obj = disposable;
			refCount = new RefCount ();
			++refCount.count;
		}
		
		public SharedLoadedAB (BundleInfo info, SharedLoadedAB other)
		{
			this.info = info;
			obj = other.obj;
			refCount = other.refCount;
			++refCount.count;
		}
		
		public SharedLoadedAB Share ()
		{
			return new SharedLoadedAB (this.info, this);
		}

		public void ManagerTryUnLoadThis ()
		{
			if (info != null && info.abManager != null) {
				info.abManager.UnLoadBundle (info, false);
			}
		}
		
		private void TryRelease ()
		{
			if (0 == --refCount.count) {
				HandleDispose ();
			}
		}

		private void HandleDispose ()
		{
			if (obj != null) {
				obj.Dispose ();
			}
			obj = null;
			if (_dependencies != null) {
				foreach (KeyValuePair<string, SharedLoadedAB> kvp in _dependencies) {
					kvp.Value.Dispose ();
//					Logger.Log (string.Format ("[{0},{1}] try unload ref sharedBundle->{2} now refCount:{3}"
//					                        , kvp.Value.info.abManager, kvp.Value.info.assetInfo.manageBundleMode, kvp.Value.info.assetBundleName, kvp.Value.referenceCount));
					kvp.Value.info.abManager.UnLoadReferenceSharedLoadedAB (kvp.Value);
				}
				_dependencies = null;
			}
		}

		public void AddDependency (string key, SharedLoadedAB shared)
		{
			if (shared.info.assetInfo.NeedRecordAsDepends) {
				if (_dependencies == null) {
					_dependencies = new SDictionary<string, SharedLoadedAB> ();
				}
				SharedLoadedAB alreadyHave = _dependencies [key];
				if (alreadyHave == null) {
					_dependencies [key] = shared.Share ();
					if (shared.info.assetInfo.AutoUnloadNoDepend) {
						if (_autoUnloadDepends == null) {
							_autoUnloadDepends = new List<string> ();
						}
						_autoUnloadDepends.Add (shared.info.assetBundleName);
						if(disposableObject.firstTimeLoadedAsset==null)
						{
							disposableObject.SetFirstTimeLoadedAsset(this.LoadedAssetAddRefCount);
						}
					}
				}
			}
		}

		public void LoadedAssetAddRefCount(UnityEngine.Object obj)
		{
			GameObject go = (GameObject)obj;
			if (go!=null && _autoUnloadDepends!=null) {
				SharedABRefCounter counter = go.GetComponent<SharedABRefCounter> ();
				if (counter == null) {
					counter = go.AddComponent<SharedABRefCounter> ();
				}
				counter.AddDepends (_autoUnloadDepends);
			}
		}

		public bool AllDependsLoaded {
			get {
				if (_dependencies != null && _dependencies.Count > 0) {
					foreach (KeyValuePair<string, SharedLoadedAB> kvp in _dependencies) {
						if (kvp.Value.disposableObject.Isloaded == false) {
							return false;
						}
					}
				}
				return true;

			}
		}
		
		#region override
		protected override void ReleaseManagedResource ()
		{
			base.ReleaseManagedResource ();
			TryRelease ();
		}
		#endregion 

		public override string ToString ()
		{
			return string.Format ("[SharedLoadedAB: manager={0}, autoUnloadDepends={1}, disposableObject={2}, referenceCount={3}]", info.abManager.ToString (), autoUnloadDepends, disposableObject, referenceCount);
		}
	}
} // namespace RO
