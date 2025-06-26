using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(CameraPointGroup)), CanEditMultipleObjects]
	public class CameraPointGroupEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (GUILayout.Button("Apply"))
			{
				foreach (var t in targets)
				{
					(t as CameraPointGroup).ResetValidity();
				}
			}
		}
	}
} // namespace EditorTool
