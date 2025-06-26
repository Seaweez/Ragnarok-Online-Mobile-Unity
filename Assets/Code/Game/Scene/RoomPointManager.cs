using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class RoomPointManager : AreaTriggerManager<RoomPointManager, RoomPoint>
	{
		private Dictionary<int, RoomHideObject> hideObjects = new Dictionary<int, RoomHideObject>();

		public bool Add(RoomHideObject obj)
		{
			if (!obj.IDValid)
			{
				return false;
			}
			hideObjects.Add(obj.ID, obj);
			return true;
		}
		
		public void Remove(RoomHideObject obj)
		{
			RoomHideObject test;
			if (hideObjects.TryGetValue(obj.ID, out test) && test == obj)
			{
				hideObjects.Remove(obj.ID);
			}
		}

		public RoomHideObject GetRoomHideObject(int ID)
		{
			RoomHideObject obj;
			if (!hideObjects.TryGetValue(ID, out obj))
			{
				return null;
			}
			return obj;
		}
	}
} // namespace RO
