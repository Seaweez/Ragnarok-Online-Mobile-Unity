using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Ghost.Extensions;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(NavMeshStatic))]
	public class NavMeshStaticEditor : Editor
	{
		private int[] areas_ = null;

		public override void OnInspectorGUI()
		{
			var navMeshStatic = target as NavMeshStatic;

			var areaNames = GameObjectUtility.GetNavMeshAreaNames();
			if (null == areas_ || areas_.Length != areaNames.Length)
			{
				areas_ = new int[areaNames.Length];
			}
			for (int i = 0; i < areas_.Length; ++i)
			{
				areas_[i] = GameObjectUtility.GetNavMeshAreaFromName(areaNames[i]);
			}

			if (!ArrayUtility.Contains(areas_, navMeshStatic.navMeshArea))
			{
				navMeshStatic.navMeshArea = areas_.IsNullOrEmpty() ? 0 : areas_[0];
			}

			navMeshStatic.editoOnly = EditorGUILayout.ToggleLeft("EditorOnly", navMeshStatic.editoOnly);
			navMeshStatic.navMeshArea = EditorGUILayout.IntPopup("Area", navMeshStatic.navMeshArea, areaNames, areas_);
		}
	
	}
} // namespace EditorTool
