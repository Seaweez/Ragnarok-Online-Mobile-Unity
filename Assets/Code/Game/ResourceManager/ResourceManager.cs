using UnityEngine;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class ResourceManager:SingleTonGO<ResourceManager>, ILoaderStrategy
	{
		// for lua
		public static ResourceManager Instance {
			get {
				return Me;
			}
		}
		
		public GameObject monoGameObject {
			get {
				return gameObject;
			}
		}
		
		public static ILoaderStrategy Loader {
			get {
				if (null != Me) {
					return Me;
				}
				return ResStrategy.Global;
			}
		}
		
		AssetDestroyer _assetDestroyer;
		static ILoaderStrategy loadWay;
		
		public static ILoaderStrategy LoaderStrategy{ get {
				return loadWay;
			} }
		
		public bool ShowDebug = false;
		
		protected override void Awake ()
		{
			base.Awake ();
			if (loadWay == null) {
				#if RESOURCE_LOAD
				loadWay = new ResStrategy ();
				#else
				loadWay = new ManagedBundleLoaderStrategy ();
				//				loadWay = new BundleLoaderStrategy ();
				#endif
			}
		}
		
		#region 同步加载
		public Object Load (ResourceID ID)
		{
			return loadWay.Load (ID);
		}
		
		public Object Load (ResourceID ID, string assetName)
		{
			return loadWay.Load (ID, assetName);
		}
		
		public Object LoadScene (ResourceID ID)
		{
			return loadWay.LoadScene (ID);
		}
		
		public Object Load (ResourceID ID, System.Type resType)
		{
			return loadWay.Load (ID, resType);
		}
		
		public Object Load (ResourceID ID, System.Type resType, string assetName)
		{
			return loadWay.Load (ID, resType, assetName);
		}
		
		public T Load<T> (ResourceID ID, string assetName = null) where T : Object
		{
			return loadWay.Load<T> (ID, assetName);
		}
		
		public TextAsset LoadScript (ResourceID ID)
		{
			return loadWay.LoadScript (ID);
		}
		
		public SharedLoadedAB GetSharedLoaded (string bundleName)
		{
			return loadWay.GetSharedLoaded (bundleName);
		}

		public Object SLoad (string ID)
		{
			return loadWay.SLoad (ID);
		}
		
		public Object SLoadAsset (string ID, string assetName)
		{
			return loadWay.SLoadAsset (ID, assetName);
		}
		
		public Object SLoadScene (string ID)
		{
			return loadWay.SLoadScene (ID);
		}
		
		public Object SLoadByType (string ID, System.Type resType)
		{
			return loadWay.SLoadByType (ID, resType);
		}
		
		public Object SLoadAssetByType (string ID, System.Type resType, string assetName)
		{
			return loadWay.SLoadAssetByType (ID, resType, assetName);
		}
		
		public T SLoad<T> (string ID, string assetName = null) where T : Object
		{
			return loadWay.SLoad<T> (ID,assetName);
		}
		
		public TextAsset SLoadScript (string ID)
		{
			return loadWay.SLoadScript (ID);
		}
		
		#endregion
		
		#region 异步加载
		
		public void AsyncLoad (ResourceID ID, System.Action<Object> loadedHandler)
		{
			loadWay.AsyncLoad (ID, loadedHandler);
		}
		
		public void AsyncLoad (ResourceID ID, System.Type resType, System.Action<Object> loadedHandler)
		{
			loadWay.AsyncLoad (ID, resType, loadedHandler);
		}
		
		public void AsyncLoad<T> (ResourceID ID, System.Action<Object> loadedHandler) where T : Object
		{
			loadWay.AsyncLoad<T> (ID, loadedHandler);
		}
		
		#endregion
		
		#region Unload
		public void UnLoadAll (bool unloadAllLoadedObjects)
		{
			loadWay.UnLoadAll (unloadAllLoadedObjects);
		}
		
		public void UnLoad (ResourceID ID, bool unloadAllLoadedObjects)
		{
			loadWay.UnLoad (ID, unloadAllLoadedObjects);
		}
		
		public void UnLoadScene (ResourceID ID, bool unloadAllLoadedObjects = false)
		{
			loadWay.UnLoadScene (ID, unloadAllLoadedObjects);
		}

		public void SUnLoad (string ID, bool unloadAllLoadedObjects)
		{
			loadWay.SUnLoad (ID, unloadAllLoadedObjects);
		}
		
		public void SUnLoadScene (string ID, bool unloadAllLoadedObjects = false)
		{
			loadWay.SUnLoadScene (ID, unloadAllLoadedObjects);
		}

		
		#endregion
		
		#region ILoaderStrategy implementation
		
		public void Dispose ()
		{
			if (loadWay != null)
				loadWay.Dispose ();
			loadWay = null;
			if (_assetDestroyer != null) {
				_assetDestroyer.DestroyRightNow ();
			}
		}
		
		public void DestroySelf ()
		{
			Dispose ();
		}
		
		#endregion
		
		public void LateUpdate ()
		{
			if (loadWay != null) {
				loadWay.LateUpdate ();
			}
		}
		
		public void GC ()
		{
			UnLoadAll (false);
			//			System.GC.Collect ();
			//			System.GC.WaitForPendingFinalizers ();
			//			Resources.UnloadUnusedAssets ();
		}
		
		public void LateDestroyAsset (Object asset)
		{
			if (_assetDestroyer == null) {
				_assetDestroyer = this.gameObject.AddComponent<AssetDestroyer> ();
			}
			_assetDestroyer.AddToDestroy (asset);
		}
	}
} // namespace RO
