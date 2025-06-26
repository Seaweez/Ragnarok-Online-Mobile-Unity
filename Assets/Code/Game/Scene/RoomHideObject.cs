using UnityEngine;
using System.Collections.Generic;
using Ghost.Utils;
using Ghost.Extensions;

namespace RO
{
	public class RoomHideObject : MonoBehaviour 
	{
		public int ID = 0;
		public bool IDValid
		{
			get
			{
				return 0 != ID;
			}
		}

		void Start()
		{
			if (!(null != RoomPointManager.Me && RoomPointManager.Me.Add(this)))
			{
//				GameObject.Destroy(gameObject);
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
