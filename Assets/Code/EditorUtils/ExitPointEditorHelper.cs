using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;
using RO;

namespace EditorTool
{
	public class ExitPointEditorHelper : MonoBehaviour
	{
		public int nextID
		{
			get
			{
				int maxID = -1;
				var eps = GetComponentsInChildren<ExitPoint>();
				foreach (var ep in eps)
				{
					maxID = System.Math.Max(maxID, ep.ID);
				}
				return maxID+1;
			}
		}

		public KeyCode shortcutsKey = KeyCode.B;

		public bool PlacePoint(Vector3 point)
		{
			if (shortcutsKey == Event.current.keyCode)
			{
				var ID = nextID;
				var go = new GameObject();
				go.transform.position = point;
				go.transform.parent = transform;
				var bp = go.AddComponent<ExitPoint>();
				bp.ID = ID;
				return true;
			}
			return false;
		}

		public List<int> GetInvalidUniqueIDs()
		{
			var eps = GetComponentsInChildren<ExitPoint>();
			if (!eps.IsNullOrEmpty())
			{
				var UniqueIDs = new List<int>();
				
				foreach (var ep in eps)
				{
					UniqueIDs.Add(ep.ID);
				}
				
				return UniqueIDs.ToNotUnique();
			}
			
			return null;
		}
	
	}
} // namespace EditorTool
