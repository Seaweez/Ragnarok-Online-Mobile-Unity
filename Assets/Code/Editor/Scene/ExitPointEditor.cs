using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(ExitPointEditorHelper))]
	public class ExitPointEditorHelperEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			
			var helper = target as ExitPointEditorHelper;
			
			var invalidIDs = helper.GetInvalidUniqueIDs();
			if (null != invalidIDs && 0 < invalidIDs.Count)
			{
				var sb = new StringBuilder();
				
				sb.AppendLine("NonUnique Exit Point ID: ");
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
				Handles.ArrowHandleCap(0, hit.point, Quaternion.LookRotation(hit.normal), 1f,EventType.MouseDown);
				Handles.color = oldColor;

				switch (Event.current.type)
				{
				case EventType.KeyDown:
				{
					var epHelper = target as ExitPointEditorHelper;
					epHelper.PlacePoint(hit.point);
				}
					break;
				}
			}
		}
	}

//	[CustomEditor(typeof(ExitPoint))]
//	public class ExitPointEditor : AreaTriggerEditor
//	{
//		private static string[] typeStrs = {
//			"Normal",
//			"Team Raid",
//		};
//		private static int[] types = {
//			0,
//			1,
//		};
//
//		protected override void DoOnInspectorGUI()
//		{
//			var ep = target as ExitPoint;
//
//			ep.ID = EditorGUILayout.IntField("ID", ep.ID);
////			ep.nextSceneType = EditorGUILayout.IntPopup("Next Scene Type", ep.nextSceneType, typeStrs, types);
//			ep.nextSceneID = EditorGUILayout.IntField("Next Scene ID", ep.nextSceneID);
//			ep.nextSceneBornPointID = EditorGUILayout.IntField("Next Scene Born Point ID", ep.nextSceneBornPointID);
////			switch (ep.nextSceneType)
////			{
////			case 0:
////				break;
////			case 1:
////				ep.raidID = EditorGUILayout.IntField("Raid ID", ep.raidID);
////				break;
////			}
//
//			EditorGUILayout.Separator();
//			ep.type = (AreaTrigger.Type)EditorGUILayout.EnumPopup("Type", ep.type);
//			base.DoOnInspectorGUI();
//		}
//
//		public override void OnInspectorGUI ()
//		{
//			DoOnInspectorGUI();
//		}
//	}
} // namespace EditorTool
