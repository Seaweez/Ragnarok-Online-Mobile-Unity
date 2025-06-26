using UnityEngine;
using System.Collections.Generic;
using RO;
using Ghost.Extensions;

namespace EditorTool
{
	public class NPCInfoEditorHelper : MonoBehaviour
	{
		public long GetUniqueID()
		{
			long uID = 1;

			var nps = GetComponentsInChildren<NPCInfo>();
			if (!nps.IsNullOrEmpty())
			{
				var UniqueIDs = new List<long>();
				
				foreach (var np in nps)
				{
					UniqueIDs.Add(np.UniqueID);
				}
				UniqueIDs.MakeUnique();

				while (UniqueIDs.Contains(uID))
				{
					++uID;
				}
			}
			return uID;
		}
	}
} // namespace EditorTool
