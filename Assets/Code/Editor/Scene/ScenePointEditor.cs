using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(ScenePointEditorHelper))]
	public class ScenePointEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			var helper = target as ScenePointEditorHelper;

			EditorGUILayout.Separator();
			if (null != helper.bpHelper)
			{
				helper.bpHelper.shortcutsKey = (KeyCode)EditorGUILayout.EnumPopup("Born Point Shortcut Key", helper.bpHelper.shortcutsKey);
			}
			if (null != helper.epHelper)
			{
				helper.epHelper.shortcutsKey = (KeyCode)EditorGUILayout.EnumPopup("Exit Point Shortcut Key", helper.epHelper.shortcutsKey);
			}
			if (null != helper.cpHelper)
			{
				helper.cpHelper.shortcutsKey = (KeyCode)EditorGUILayout.EnumPopup("Camera Point Shortcut Key", helper.cpHelper.shortcutsKey);
			}
			if (null != helper.rpHelper)
			{
				helper.rpHelper.shortcutsKey = (KeyCode)EditorGUILayout.EnumPopup("Room Point Shortcut Key", helper.rpHelper.shortcutsKey);
			}
			if (null != helper.npHelper)
			{
				helper.npHelper.shortcutsKey = (KeyCode)EditorGUILayout.EnumPopup("NPC Point Shortcut Key", helper.npHelper.shortcutsKey);
			}
			if (null != helper.rnpHelper)
			{
				helper.rnpHelper.shortcutsKey = (KeyCode)EditorGUILayout.EnumPopup("Raid NPC Point Shortcut Key", helper.rnpHelper.shortcutsKey);
			}
		}

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
					var helper = target as ScenePointEditorHelper;
					if (null != helper.bpHelper && helper.bpHelper.PlacePoint(hit.point))
					{
						return;
					}
					if (null != helper.epHelper && helper.epHelper.PlacePoint(hit.point))
					{
						return;
					}
					if (null != helper.cpHelper && helper.cpHelper.PlacePoint(hit.point))
					{
						return;
					}
					if (null != helper.rpHelper && helper.rpHelper.PlacePoint(hit.point))
					{
						return;
					}
					if (null != helper.npHelper && helper.npHelper.PlacePoint(hit.point))
					{
						return;
					}
					if (null != helper.rnpHelper && helper.rnpHelper.PlacePoint(hit.point))
					{
						return;
					}
				}
					break;
				}
			}
		}
	}
} // namespace EditorTool
