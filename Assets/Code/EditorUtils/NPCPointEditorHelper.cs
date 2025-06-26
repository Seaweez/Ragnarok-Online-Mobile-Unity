using UnityEngine;
using System.Collections.Generic;
using RO;
using Ghost.Extensions;

namespace EditorTool
{
	public class NPCPointEditorHelper : NPCInfoEditorHelper
	{
		public KeyCode shortcutsKey = KeyCode.N;

		public bool PlacePoint(Vector3 point)
		{
			if (shortcutsKey == Event.current.keyCode)
			{
				var go = new GameObject ();
				go.transform.position = point;
				go.transform.parent = transform;
				var np = go.AddComponent<NPCPoint> ();	
				np.ID = 0;	
				return true;
			}
			return false;
		}

		public List<long> GetInvalidNPCUniqueIDs()
		{
			var nps = GetComponentsInChildren<NPCPoint>();
			if (!nps.IsNullOrEmpty())
			{
				var UniqueIDs = new List<long>();
				
				foreach (var np in nps)
				{
					if (0 == np.UniqueID)
					{
						continue;
					}
					UniqueIDs.Add(np.UniqueID);
				}
				
				return UniqueIDs.ToNotUnique();
			}
			
			return null;
		}
	
	}
} // namespace EditorTool
