using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(ModelUtilsEditorHelper))]
	public class ModelUtilsEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();

			var helper = target as ModelUtilsEditorHelper;

			if (GUILayout.Button("AdjustShadow"))
			{
				ModelUtils.AdjustSprite(helper.gameObject, helper.shadowPlane);
			}
		}
	}
} // namespace EditorTool
