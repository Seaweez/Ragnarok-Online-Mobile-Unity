using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(SpriteFade))]
	public class SpriteFadeEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			if (Application.isPlaying)
			{
				if (GUILayout.Button("FadeIn"))
				{
					(target as SpriteFade).FadeIn();
				}
				if (GUILayout.Button("FadeOut"))
				{
					(target as SpriteFade).FadeOut();
				}
			}
		}
	
	}
} // namespace EditorTool
