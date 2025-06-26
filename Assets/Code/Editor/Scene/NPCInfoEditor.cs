using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(NPCInfo)), CanEditMultipleObjects]
	public class NPCInfoEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			if (GUILayout.Button("Get Unique ID"))
			{
				foreach (var t in targets)
				{
					var np = t as NPCInfo;
					np.UniqueID = 0;
				}
				foreach (var t in targets)
				{
					var np = t as NPCInfo;
					var helper = np.GetComponentInParent<NPCInfoEditorHelper>();
					if (null != helper)
					{
						np.UniqueID = helper.GetUniqueID();
						EditorUtility.SetDirty(t);
					}
				}
			}
			base.OnInspectorGUI ();

			if (1 == targets.Length)
			{
				var np = target as NPCInfo;
				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(string.Format("Behaviors: {0}", (int)np.behaviours));
				if (GUILayout.Button("Copy"))
				{
					TextEditor te = new TextEditor();
					te.text = string.Format("{0}", (int)np.behaviours);
					te.OnFocus();
					te.Copy();
				}
				EditorGUILayout.EndHorizontal();
			}
		}
	
	}
} // namespace EditorTool
