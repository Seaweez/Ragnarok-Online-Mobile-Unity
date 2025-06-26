using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(Bus))]
	public class BusEditor : Editor
	{
		private int line = 1;

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			var bus = target as Bus;
			if (Application.isPlaying)
			{
				EditorGUILayout.BeginHorizontal();
				line = EditorGUILayout.IntField("Line", line);
				if (GUILayout.Button("GO"))
				{
					bus.GO (line);
				}
				EditorGUILayout.EndHorizontal();
			}
		}
	
	}
} // namespace EditorTool
