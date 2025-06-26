using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(ChangeRqByTex), true)]
	public class ChangeRqByTexEditor:UITextureInspector
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			serializedObject.UpdateIfRequiredOrScript ();
			NGUIEditorTools.DrawProperty ("Excute", serializedObject, "excute");
			NGUIEditorTools.DrawProperty ("RenderQ", serializedObject, "RenderQ");
			serializedObject.ApplyModifiedProperties ();
		}
	}
} // namespace EditorTool
