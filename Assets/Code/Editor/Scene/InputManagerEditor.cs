using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(InputManager))]
	public class InputManagerEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (Application.isPlaying && GUILayout.Button("Reset Params"))
			{
				var inputManager = target as InputManager;
				inputManager.ResetParams();
			}
		}
	
	}
} // namespace EditorTool
