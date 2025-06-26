using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(RoomPointEditorHelper))]
	public class RoomPointEditor : Editor
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
					var helper = target as RoomPointEditorHelper;
					helper.PlacePoint(hit.point);
				}
					break;
				}
			}
		}
	}
} // namespace EditorTool
