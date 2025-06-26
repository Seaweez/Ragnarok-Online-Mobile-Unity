using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(AreaTrigger), true), CanEditMultipleObjects]
	public class AreaTriggerEditor : Editor
	{
		protected virtual void DoOnInspectorGUI()
		{
			var areaTrigger = target as AreaTrigger;
			
			switch (areaTrigger.type)
			{
			case AreaTrigger.Type.CIRCLE:
				areaTrigger.range = EditorGUILayout.FloatField("Range", areaTrigger.range);
				areaTrigger.innerRange = EditorGUILayout.FloatField("Inner Range", areaTrigger.innerRange);
				break;
			case AreaTrigger.Type.RECTANGE:
				areaTrigger.size = EditorGUILayout.Vector2Field("Size", areaTrigger.size);
				break;
			}
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();
			DoOnInspectorGUI();
		}
	
	}
} // namespace EditorTool
