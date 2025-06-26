using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ghost.Extensions
{
	public static class ComponentExtensions {

		public static T GetComponentInChildrenTopLevel<T>(this Component obj) where T:Component
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
