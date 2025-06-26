using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(RoomHideObject)), CanEditMultipleObjects]
	public class RoomHideObjectEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (1 < targets.Length)
			{
				var objs = new Dictionary<int, List<RoomHideObject>>();
				foreach (var t in targets)
				{
					var obj = t as RoomHideObject;
					List<RoomHideObject> objList;
					if (!objs.TryGetValue(obj.ID, out objList))
					{
						objList = new List<RoomHideObject>();
						objs.Add(obj.ID, objList);
					}
					objList.Add(obj);
				}

				var nonunique = new Dictionary<int, List<RoomHideObject>>();
				foreach (var key_value in objs)
				{
					if (1 < key_value.Value.Count)
					{
						nonunique.Add(key_value.Key, key_value.Value);
					}
				}

				if (0 < nonunique.Count)
				{
					EditorGUILayout.Separator();
					foreach (var key_value in nonunique)
					{
						var sb = new System.Text.StringBuilder();
						sb.AppendLine(string.Format ("Duplicate ID: {0}", key_value.Key));
						var count = key_value.Value.Count;
						for (int i = 0; i < count-1; ++i)
						{
							sb.AppendLine(key_value.Value[i].name);
						}
						sb.Append(key_value.Value[count-1].name);
						EditorGUILayout.HelpBox(sb.ToString(), MessageType.Error, true);
					}

				}
			}
		}
	
	}
} // namespace EditorTool
