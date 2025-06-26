using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(CircleDrawerSmooth))]
	public class CircleDrawerSmoothEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (Application.isPlaying)
			{
				EditorGUILayout.Separator();
				if (GUILayout.Button("Apply"))
				{
					var smooth = target as CircleDrawerSmooth;
					smooth.SmoothSet();
				}
			}
		}
	}
} // namespace EditorTool
