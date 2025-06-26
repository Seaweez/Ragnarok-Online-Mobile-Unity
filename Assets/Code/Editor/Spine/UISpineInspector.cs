using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(UISpine))]
	public class UISpineInspector:UIWidgetInspector
	{
		protected override void DrawCustomProperties ()
		{
			NGUIEditorTools.DrawProperty("NGUISpine", serializedObject, "spine", GUILayout.MinWidth(20f));
			NGUIEditorTools.DrawProperty("Material", serializedObject, "spineMaterial", GUILayout.MinWidth(20f));
			base.DrawCustomProperties ();
		}
	
	}
} // namespace EditorTool
