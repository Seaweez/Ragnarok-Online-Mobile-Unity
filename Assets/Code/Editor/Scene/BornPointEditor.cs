using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(BornPointEditorHelper))]
	public class BornPointEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			
			var helper = target as BornPointEditorHelper;
			
			var invalidIDs = helper.GetInvalidUniqueIDs();
			if (null != invalidIDs && 0 < invalidIDs.Count)
			{
				var sb = new StringBuilder();
				
				sb.AppendLine("NonUnique Born Point ID: ");
				foreach (var ID in invalidIDs)
				{
					sb.AppendFormat("\t{0}", ID);
				}
				EditorGUILayout.HelpBox(sb.ToString(), MessageType.Error);
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
				Handles.ArrowHandleCap(0, hit.point, Quaternion.LookRotation(hit.normal), 0.5f,EventType.MouseDown);
				Handles.color = oldColor;

				switch (Event.current.type)
				{
				case EventType.KeyDown:
				{
					var bpHelper = target as BornPointEditorHelper;
					bpHelper.PlacePoint(hit.point);
				}
					break;
				}
			}
		}
	}
} // namespace EditorTool
