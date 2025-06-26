using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(CameraController))]
	public class CameraControllerEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (GUILayout.Button("Default"))
			{
				var controller = target as CameraController;
				controller.SetDefault();
				controller.ForceApplyCurrentInfo();
				controller.UpdatePosition();
			}
			if (Application.isPlaying && GUILayout.Button("RestoreZoom"))
			{
				var controller = target as CameraController;
				controller.RestoreZoom(0.5f);
			}
		}
	
	}
} // namespace EditorTool
