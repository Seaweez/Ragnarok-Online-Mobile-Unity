using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(ROUIScrollView))]
	public class ROUIScrollViewEditor:UIScrollViewEditor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			serializedObject.UpdateIfRequiredOrScript ();
			SerializedProperty sp = NGUIEditorTools.DrawProperty ("StopCheckEnable", serializedObject, "_stopCheckEnable");
			if (sp.boolValue) {
				NGUIEditorTools.DrawProperty ("BackStrength", serializedObject, "BackStrength");
				NGUIEditorTools.DrawProperty ("RefreshTarget", serializedObject, "RefreshTarget");
				NGUIEditorTools.DrawProperty ("MainTarget", serializedObject, "MainTarget");
			}
			serializedObject.ApplyModifiedProperties ();
		}
	}
} // namespace EditorTool
