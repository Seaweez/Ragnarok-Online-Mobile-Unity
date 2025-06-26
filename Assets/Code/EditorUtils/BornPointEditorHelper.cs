using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;
using RO;

namespace EditorTool
{
	public class BornPointEditorHelper : MonoBehaviour
	{
		public int nextID
		{
			get
			{
				int maxID = -1;
				var bps = GetComponentsInChildren<BornPoint>();
				foreach (var bp in bps)
				{
					maxID = System.Math.Max(maxID, bp.ID);
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
				var bp = go.AddComponent<BornPoint>();
				bp.ID = ID;
				return true;
			}
			return false;
		}

		public List<int> GetInvalidUniqueIDs()
		{
			var bps = GetComponentsInChildren<BornPoint>();
			if (!bps.IsNullOrEmpty())
			{
				var UniqueIDs = new List<int>();
				
				foreach (var bp in bps)
				{
					UniqueIDs.Add(bp.ID);
				}
				
				return UniqueIDs.ToNotUnique();
			}
			
			return null;
		}
	
	}
} // namespace EditorTool
