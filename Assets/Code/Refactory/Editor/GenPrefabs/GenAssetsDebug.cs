using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Ghost.Utils;

namespace EditorTool
{
	public class GenAssetsDebug : ScriptableObject
	{
		[System.Serializable]
		public class Info
		{
			public bool enable = true;
			public string tableName;
			public int[] debugIDs;
		}
		
		public Info[] infos;

		[MenuItem("Assets/Create/GenAssetsDebugInfo")]
		static void CreateAsset()
		{
			var selectObj = Selection.activeObject;
			var path = AssetDatabase.GetAssetPath(selectObj);
			if (string.IsNullOrEmpty(path))
			{
				path = "Assets";
			}
			
			if (!AssetDatabase.IsValidFolder(path))
			{
				path = Path.GetDirectoryName(path);
			}
			
			path = PathUnity.Combine(path, "GenAssetsDebugInfo.asset");
			
			var asset = ScriptableObject.CreateInstance<GenAssetsDebug>();
			AssetDatabase.CreateAsset(asset, path);
			
			Selection.activeObject = AssetDatabase.LoadAssetAtPath(path, typeof(GenAssetsDebug));
		}
	}
} // namespace EditorTool
