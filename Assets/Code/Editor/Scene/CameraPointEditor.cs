using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(CameraPointEditorHelper))]
	public class CameraPointEditor : Editor
	{
		void OnSceneGUI()
		{
			var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask.GetMask(RO.Config.Layer.TERRAIN.Key)))
			{
				var oldColor = Handles.color;
				Handles.color = Color.red;
				Handles.ArrowHandleCap(0, hit.point, Quaternion.LookRotation(hit.normal), 1f,EventType.MouseDown);
				Handles.color = oldColor;

				switch (Event.current.type)
				{
				case EventType.KeyDown:
				{
					var cpHelper = target as CameraPointEditorHelper;
					cpHelper.PlacePoint(hit.point);
				}
					break;
				}
			}
		}
	}

	[CustomEditor(typeof(CameraPoint), true), CanEditMultipleObjects]
	public class CameraPointInspectorEditor : AreaTriggerEditor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (null != Camera.main)
			{
				var controller = Camera.main.GetComponent<CameraController>();
				if (null != controller)
				{
					EditorGUILayout.Separator();
					if (GUILayout.Button("Capture"))
					{
						var cp = target as CameraPoint;
						var oldFocus = cp.info.focus;
						cp.info.focus = cp.transform;
						controller.SetInfo(cp.info);
						cp.info.focus = oldFocus;
						controller.ForceApplyCurrentInfo();
						controller.UpdatePosition();
					}
				}
			}
		}
	}
} // namespace EditorTool
