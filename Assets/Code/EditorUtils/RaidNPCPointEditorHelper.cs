using UnityEngine;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	public class RaidNPCPointEditorHelper : NPCInfoEditorHelper
	{
		public KeyCode shortcutsKey = KeyCode.N;

		public bool PlacePoint(Vector3 point)
		{
			if (shortcutsKey == Event.current.keyCode)
			{
				var go = new GameObject ();
				go.transform.position = point;
				go.transform.parent = transform;
				var np = go.AddComponent<RaidNPCPoint> ();	
				np.ID = 0;
				return true;
			}
			return false;
		}
	
	}
} // namespace EditorTool
