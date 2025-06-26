using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ghost.Extensions
{
	public static class GameObjectExtensions {
		
		public static Rect CalcCompositeRect2D(this GameObject obj)
		{
			Vector2 min = Vector2.zero;
			Vector2 max = Vector2.zero;
			if (obj.GetComponent<Renderer>())
			{
				min = obj.GetComponent<Renderer>().bounds.min;
				max = obj.GetComponent<Renderer>().bounds.max;
			}
			var renderers = obj.GetComponentsInChildren<Renderer>();
			if (!renderers.IsNullOrEmpty())
			{
				foreach (var renderer in renderers)
				{
					var bounds = renderer.bounds;
					min.x = Mathf.Min(min.x, bounds.min.x);
					min.y = Mathf.Min(min.y, bounds.min.y);
					max.x = Mathf.Max(max.x, bounds.max.x);
					max.y = Mathf.Max(max.y, bounds.max.y);
				}
			}
			return new Rect(min.x-obj.transform.position.x, min.y-obj.transform.position.y, max.x-min.x, max.y-max.y);
		}

		public static Bounds CalcCompositeBounds2D(this GameObject obj)
		{
			var rect = obj.CalcCompositeRect2D();
			var center = rect.center + (Vector2)obj.transform.position;
			return new Bounds(new Vector3(center.x, center.y, obj.transform.position.z), new Vector3(rect.size.x, rect.size.y, obj.transform.position.z));
		}

		public static GameObject[] FindGameObjectsInChildren(this GameObject obj, System.Predicate<GameObject> pred)
		{
			var objs = new List<GameObject>();
			obj.transform.Foreach(delegate(Transform t) {
				if (pred(t.gameObject))
				{
					objs.Add(t.gameObject);
				}
				return true;
			});
			return objs.ToArray();
		}

		public static GameObject FindGameObjectInChildren(this GameObject obj, System.Predicate<GameObject> pred)
		{
			GameObject ret = null;
			obj.transform.Foreach(delegate(Transform t) {
				if (pred(t.gameObject))
				{
					ret = t.gameObject;
					return false;
				}
				return true;
			});
			return ret;
		}

		public static T FindComponentInChildren<T>(this GameObject obj) where T:Component
		{
			T ret = default(T);
			obj.transform.Foreach(delegate(Transform t) {
				var c = t.GetComponent<T>();
				if (null != c)
				{
					ret = c;
					return false;
				}
				return true;
			});
			return ret;
		}

		public static T[] FindComponentsInChildren<T>(this GameObject obj) where T:Component
		{
			var components = new List<T>();
			obj.transform.Foreach(delegate(Transform t) {
				var c = t.GetComponent<T>();
				if (null != c)
				{
					components.Add(c);
				}
				return true;
			});
			return components.ToArray();
		}

		public static T GetComponentInChildrenTopLevel<T>(this GameObject obj) where T:Component
		{
			var component = obj.GetComponent<T>();
			if (null != component)
			{
				return component;
			}
			var transform = obj.transform;
			var childCount = transform.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				var child = transform.GetChild(i);
				component = child.GetComponent<T>();
				if (null != component)
				{
					return component;
				}
			}
			return null;
		}
		
	}
} // namespace Ghost.Extensions
