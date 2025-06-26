using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class UIGridActiveSelf : UIGrid
	{
		new public List<Transform> GetChildList ()
		{
			Transform myTrans = transform;
			List<Transform> list = new List<Transform> ();
			
			for (int i = 0; i < myTrans.childCount; ++i) {
				Transform t = myTrans.GetChild (i);
				if (!hideInactive || (t && t.gameObject && t.gameObject.activeSelf))
					list.Add (t);
			}
			
			// Sort the list using the desired sorting logic
			if (sorting != Sorting.None && arrangement != Arrangement.CellSnap) {
				if (sorting == Sorting.Alphabetic)
					list.Sort (SortByName);
				else if (sorting == Sorting.Horizontal)
					list.Sort (SortHorizontal);
				else if (sorting == Sorting.Vertical)
					list.Sort (SortVertical);
				else if (onCustomSort != null)
					list.Sort (onCustomSort);
				else
					Sort (list);
			}
			return list;
		}

		[ContextMenu("Execute")]
		public override void Reposition ()
		{
			if (Application.isPlaying && !mInitDone && NGUITools.GetActive (gameObject))
				Init ();
			
			// Get the list of children in their current order
			List<Transform> list = GetChildList ();
			
			// Reset the position and order of all objects in the list
			ResetPosition (list);
			
			// Constrain everything to be within the panel's bounds
			if (keepWithinPanel)
				ConstrainWithinPanel ();
			
			// Notify the listener
			if (onReposition != null)
				onReposition ();
		}
	}
} // namespace RO
