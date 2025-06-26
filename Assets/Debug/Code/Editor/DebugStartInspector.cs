using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO.Test;

namespace EditorTool
{
	[CustomEditor (typeof(DebugStart), true)]
	public class DebugStartInspector:Editor
	{
		const string PATH_LUASCRIPT = "Debug/Scripts/";

		DebugStart _target;

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (GUILayout.Button ("RefreshLuaFile")) {
				_target = target as DebugStart;

				_target.luafiles.Clear ();

				Object[] objs = DebugGM.AssetManager.getAllAssets (System.IO.Path.Combine (Application.dataPath, PATH_LUASCRIPT));
				for (int i = 0; i < objs.Length; i++) {
					TextAsset text = objs [i] as TextAsset;
					_target.luafiles.Add (text);
				}
				EditorUtility.SetDirty (_target);
				AssetDatabase.SaveAssets ();
			}
		}
	}
}
// namespace EditorTool
