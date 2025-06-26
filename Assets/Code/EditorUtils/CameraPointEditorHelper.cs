using UnityEngine;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	public class CameraPointEditorHelper : MonoBehaviour
	{
		public KeyCode shortcutsKey = KeyCode.C;

		public bool PlacePoint(Vector3 point)
		{
			if (shortcutsKey == Event.current.keyCode)
			{
				var go = new GameObject();
				go.name = "cp";
				go.transform.position = point;
				go.transform.parent = transform;
				go.AddComponent<CameraPoint>();
				return true;
			}
			return false;
		}
	}
} // namespace EditorTool
