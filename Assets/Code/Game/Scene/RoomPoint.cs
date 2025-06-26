using UnityEngine;
using System.Collections.Generic;
using Ghost.Utils;
using Ghost.Extensions;
using Ghost.Attribute;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class RoomPoint : AreaTrigger 
	{
		public int[] hideObjects = null;

		public override void OnRoleEnter(Transform t)
		{
			if (!hideObjects.IsNullOrEmpty())
			{
				var manager = RoomPointManager.Me;
				foreach (var ID in hideObjects)
				{
					var obj = manager.GetRoomHideObject(ID);
					if (null != obj)
					{
						obj.gameObject.SetActive(false);
					}
				}
			}
		}
		
		public override void OnRoleExit(Transform t)
		{
			if (!hideObjects.IsNullOrEmpty())
			{
				var manager = RoomPointManager.Me;
				foreach (var ID in hideObjects)
				{
					var obj = manager.GetRoomHideObject(ID);
					if (null != obj)
					{
						obj.gameObject.SetActive(true);
					}
				}
			}
		}

		void Start()
		{
			if (!(null != RoomPointManager.Me && RoomPointManager.Me.Add(this)))
			{
				GameObject.Destroy(gameObject);
			}
		}

		void OnDestroy()
		{
			if (null != RoomPointManager.Me)
			{
				RoomPointManager.Me.Remove(this);
			}
		}

	}

} // namespace RO
