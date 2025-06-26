using UnityEngine;
using System.Collections;
using UnityEditor;
using RO;
using System.IO;

[CustomEditor (typeof(NGUIShaderManager), true)]
public class NGUIShaderManagerEditor : Editor
{
	NGUIShaderManager _target;

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		_target = target as NGUIShaderManager;
		if (GUILayout.Button ("SelectShaders")) {
			string path = EditorUtility.OpenFolderPanel ("Select Resource Path", Path.Combine (Application.dataPath, "Resources/Public/Shader/UI/"), "");
			_target.ClearShaderList ();
			Object[] objs = AssetManager.getAllAssets (path);
			for (int i = 0; i < objs.Length; i++) {
				Shader shader = objs [i] as Shader;
				if (shader != null) {
					_target.AddShader (shader);
				}
			}
			EditorUtility.SetDirty (_target);
			AssetDatabase.SaveAssets ();
		}
	}
}