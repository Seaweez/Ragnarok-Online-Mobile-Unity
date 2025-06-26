using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class LuaHoldObject 
	{
		public static LuaHoldObject Me = new LuaHoldObject();

		private List<object> objects = new List<object>();

		private LuaHoldObject()
		{
		}
		
		public void HoldObject(object obj)
		{
			if (objects.Contains(obj))
			{
				return;
			}
			objects.Add(obj);
		}

		public void ReleaseObject(object obj)
		{
			objects.Remove(obj);
		}
	}
} // namespace RO
