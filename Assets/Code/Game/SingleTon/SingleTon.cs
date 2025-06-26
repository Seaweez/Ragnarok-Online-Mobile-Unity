using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class SingletonM<T>  where T:MonoBehaviour
	{
		protected static T _instance;
		private static  string objName = "Singleton";
		private static GameObject singletonObj;
		
		public static T instance {
			get {
				if (_instance == null) {
					singletonObj = GameObject.Find (objName);
					if (singletonObj == null)
						singletonObj = new GameObject (objName);
					_instance = singletonObj.AddComponent<T> () as T;
				}
				return _instance;
			}
		} 
	}
	/// <summary>
	/// Singleton Based on class not inherited from MonoBehaviour
	/// </summary>
	public class SingletonO	<T> where T:new()
	{
		protected static T _instance;
		private static GameObject singletonObj;
		
		public static T instance {
			get {
				if (typeof(T).IsSubclassOf (typeof(MonoBehaviour))) {
					RO.LoggerUnused.LogError ("please use SingletonW if you want to create singleton based on MonoBehaviour!");
					return default(T);
				}
				if (_instance == null) {
					_instance = new T ();
				}
				return _instance;
			}
			
		}
	}
} // namespace RO
