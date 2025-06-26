using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor (typeof(NoFlyPoint), true), CanEditMultipleObjects]
	public class MaskAreaPointEditor : Editor
	{
		protected virtual void DoOnInspectorGUI ()
		{
			var areaTrigger = target as AreaTrigger;
			areaTrigger.type = AreaTrigger.Type.RECTANGE;
			areaTrigger.size = EditorGUILayout.Vector2Field ("Size", areaTrigger.size);
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			DoOnInspectorGUI ();
		}

	}
}
 // namespace EditorTool
