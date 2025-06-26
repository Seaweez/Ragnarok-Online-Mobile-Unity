using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace Ghost.EditorTool
{
	[CustomEditor(typeof(RoleCompleteHelper)), CanEditMultipleObjects]
	public class E_RoleCompleteHelperEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
		
			if (Application.isPlaying)
			{
				EditorGUILayout.Separator();
				if (GUILayout.Button("Apply"))
				{
					foreach (var t in targets)
					{
						(t as RoleCompleteHelper).Apply();
					}
				}
			}
		}
	
	}
} // namespace Ghost.EditorTool
