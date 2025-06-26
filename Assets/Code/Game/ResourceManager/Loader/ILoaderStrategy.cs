using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace RO
{
	public interface ILoaderStrategy:ISyncLoaderStrategy,IAsyncLoaderStrategy,IUnLoaderStrategy,ISyncStringLoaderStrategy,IStringUnLoaderStrategy
	{
		void Dispose ();

		void LateUpdate ();
	}

	public interface ISyncLoaderStrategy
	{
		Object Load (ResourceID ID);

		Object Load (ResourceID ID, string assetName);

		Object LoadScene (ResourceID ID);
		
		Object Load (ResourceID ID, System.Type resType);

		Object Load (ResourceID ID, System.Type resType, string assetName);
		
		T Load<T> (ResourceID ID, string assetName = null)where T:Object;

		TextAsset LoadScript (ResourceID ID);

		SharedLoadedAB GetSharedLoaded (string bundleName);
	}

	public interface IAsyncLoaderStrategy
	{
		void AsyncLoad (ResourceID ID, System.Action<Object> loadedHandler);

		void AsyncLoad (ResourceID ID, System.Type resType, System.Action<Object> loadedHandler);

		void AsyncLoad<T> (ResourceID ID, System.Action<Object> loadedHandler)where T:Object;
	}

	public interface IUnLoaderStrategy
	{
		void UnLoad (ResourceID ID, bool unloadAllLoadedObjects);

		void UnLoadScene (ResourceID ID, bool unloadAllLoadedObjects = false);
		
		void UnLoadAll (bool unloadAllLoadedObjects);
	}

	public interface ISyncStringLoaderStrategy
	{
		Object SLoad (string ID);
		
		Object SLoadAsset (string ID, string assetName);
		
		Object SLoadScene (string ID);
		
		Object SLoadByType (string ID, System.Type resType);
		
		Object SLoadAssetByType (string ID, System.Type resType, string assetName);
		
		T SLoad<T> (string ID, string assetName = null)where T:Object;
		
		TextAsset SLoadScript (string ID);
	}

	public interface IStringUnLoaderStrategy
	{
		void SUnLoad (string ID, bool unloadAllLoadedObjects);
		
		void SUnLoadScene (string ID, bool unloadAllLoadedObjects = false);
		
	}

} // namespace RO
