using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(Map2D))]
	public class Map2DEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			var map = target as Map2D;

			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			map.textureSavePath = EditorGUILayout.TextField("Save", map.textureSavePath);
			if (GUILayout.Button("..."))
			{
				map.textureSavePath = EditorUtility.SaveFilePanel("Save Map2D", "", "Map2D", "png");
			}
			EditorGUILayout.EndHorizontal();

			var texture = map.CachedTexture as Texture2D;
			var path = map.textureSavePath;
			if (null != texture && !string.IsNullOrEmpty(path))
			{
				EditorGUILayout.Separator();
				if (GUILayout.Button("Save Texture"))
				{
					ScreenShot.SavePNG(texture, path);
				}
			}
		}
	
	}
} // namespace EditorTool
