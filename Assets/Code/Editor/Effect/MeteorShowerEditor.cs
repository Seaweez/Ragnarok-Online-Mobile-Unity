using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(MeteorShower)), CanEditMultipleObjects]
	public class MeteorShowerEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (Application.isPlaying)
			{
				EditorGUILayout.Separator();
				if (GUILayout.Button("Launch"))
				{
					foreach (var t in targets)
					{
						(t as MeteorShower).Launch();
					}
				}
			}
		}
	
	}
} // namespace EditorTool
