using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(UIScrollViewEx))]
	public class UIScrollViewExEditor:UIScrollViewEditor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			
			serializedObject.UpdateIfRequiredOrScript ();
			NGUIEditorTools.DrawProperty ("BoundTarget", serializedObject, "BoundTarget");
			serializedObject.ApplyModifiedProperties ();
		}
	}
} // namespace EditorTool
