using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(NPCPointEditorHelper))]
	public class NPCPointEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			
			var helper = target as NPCPointEditorHelper;
			
			var invalidNPCIDs = helper.GetInvalidNPCUniqueIDs();
			if (null != invalidNPCIDs && 0 < invalidNPCIDs.Count)
			{
				var sb = new StringBuilder();
				
				sb.AppendLine("Invalid NPC Unique ID: ");
				foreach (var ID in invalidNPCIDs)
				{
					sb.AppendFormat("\t{0}", ID);
				}
				EditorGUILayout.HelpBox(sb.ToString(), MessageType.Error);
			}
		}

		void OnSceneGUI ()
		{
			var ray = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);

			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, float.PositiveInfinity, LayerMask.GetMask (RO.Config.Layer.TERRAIN.Key))) {
				var oldColor = Handles.color;
				Handles.color = Color.red;
				Handles.ArrowHandleCap (0, hit.point, Quaternion.LookRotation (hit.normal), 0.5f,EventType.MouseDown);
				Handles.color = oldColor;

				switch (Event.current.type) {
				case EventType.KeyDown:
					{
						var helper = target as NPCPointEditorHelper;
						helper.PlacePoint(hit.point);
					}
					break;
				}
			}
		}
	}
} // namespace EditorTool
