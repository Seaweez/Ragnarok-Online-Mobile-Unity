using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ghost.Extensions
{
	public static class TransformExtensions
	{
		public static void ResetLocal(this Transform t)
		{
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
		}

		public static void ResetParent(this Transform t, Transform parent)
		{
			t.parent = parent;
			t.ResetLocal();
		}

		public static void Foreach(this Transform root, System.Predicate<Transform> func)
		{
			var parents = new List<Transform>();
			parents.Add(root);
			var nextParents = new List<Transform>();
			
			while (0 < parents.Count)
			{
				foreach (var parent in parents)
				{
					if (!func(parent))
					{
						return;
					}
					var childCount = parent.childCount;
					for (int i = 0; i < childCount; ++i)
					{
						nextParents.Add(parent.GetChild(i));
					}
				}
				parents.Clear();
				var temp = parents;
				parents = nextParents;
				nextParents = temp;
			}
		}
	}
} // namespace Ghost.Extensions
