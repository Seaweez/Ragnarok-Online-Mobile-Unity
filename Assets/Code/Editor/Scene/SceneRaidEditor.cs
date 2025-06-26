using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(SceneRaid))]
	public class SceneRaidEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			var raid = target as SceneRaid;

			raid.cameraInfoEnable = EditorGUILayout.ToggleLeft("Camera Info Enable", raid.cameraInfoEnable);
			if (raid.cameraInfoEnable)
			{
				var info = raid.cameraInfo;
				info.focusOffset = EditorGUILayout.Vector3Field("Focus offset", info.focusOffset);
				info.focusViewPort = EditorGUILayout.Vector3Field("Focus View Port", info.focusViewPort);
				info.rotation = EditorGUILayout.Vector3Field("Roation", info.rotation);
				info.fieldOfView = EditorGUILayout.Slider("Field Of View", info.fieldOfView, 1f, 179f);
			}

			var invalidNPCIDs = raid.GetInvalidNPCUniqueIDs();
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
	
	}
} // namespace EditorTool
