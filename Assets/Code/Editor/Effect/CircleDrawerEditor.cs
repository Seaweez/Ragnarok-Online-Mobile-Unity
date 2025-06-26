using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(CircleDrawer))]
	public class CircleDrawerEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			var drawer = target as CircleDrawer;

			EditorGUI.BeginChangeCheck();
			base.OnInspectorGUI ();
			if (EditorGUI.EndChangeCheck())
			{
				drawer.Draw();
			}
			else if (GUILayout.Button("Draw"))
			{
				drawer.Draw();
			}
		}
	}
} // namespace EditorTool
