using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Ghost.Utils;
using Ghost.Extensions;

namespace EditorTool
{
	public static class CheckMaterial 
	{
		private static void DoCheckHasTexture(Material m)
		{
			if (null == m)
			{
				return;
			}
			if (null != m.mainTexture)
			{
				return;
			}
			var combineTextureName = "_CombineTex1";
			if (m.HasProperty(combineTextureName) && null != m.GetTexture(combineTextureName))
			{
				return;
			}
			Debug.LogErrorFormat(m, "[CheckMaterialTexture Failed]\n{0}", AssetDatabase.GetAssetPath(m));
		}

		private static void DoCheckTexture(Object selectionObj)
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			Debug.LogFormat ("[CheckMaterialTexture]: {0}", path);
			if (AssetDatabase.IsValidFolder(path))
			{
				var filterString = StringUtils.ConnectToString("t:", typeof(Material).Name);
				var guids = AssetDatabase.FindAssets(filterString, new string[]{path}); 
				
				if (guids.IsNullOrEmpty())
				{
					return;
				}
				
				foreach (var guid in guids)
				{
					var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Material)) as Material;
					DoCheckHasTexture(obj);
				}
			}
			else
			{
				DoCheckHasTexture(selectionObj as Material);
			}
		}

		private static void DoCheckTexture()
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoCheckTexture(obj);
			}
			Debug.LogFormat("[CheckMaterialTexture Finished]");
		}

		[MenuItem("Assets/CheckMaterial/Texture")]
		static void CheckTexture()
		{
			DoCheckTexture();
		}
	}
} // namespace EditorTool
