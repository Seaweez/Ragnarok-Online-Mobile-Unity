using UnityEngine;
using System.Collections.Generic;

namespace EditorTool
{
	public class ChildrenTransformExporter : MonoBehaviour 
	{
		public TextAsset config;

		void Awake()
		{
			var childCount = transform.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				var child = transform.GetChild(i);
				GameObject.Destroy(child.gameObject);
			}
		}
	
	}
} // namespace EditorTool
