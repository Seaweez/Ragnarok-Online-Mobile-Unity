using UnityEngine;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	public class RoomPointEditorHelper : MonoBehaviour
	{
		public KeyCode shortcutsKey = KeyCode.R;

		public bool PlacePoint(Vector3 point)
		{
			if (shortcutsKey == Event.current.keyCode)
			{
				var go = new GameObject();
				go.name = "room";
				go.transform.position = point;
				go.transform.parent = transform;
				go.AddComponent<RoomPoint>();
				return true;
			}
			return false;
		}
	}
} // namespace EditorTool
