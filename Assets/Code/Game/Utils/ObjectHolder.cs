using UnityEngine;
using System.Collections.Generic;
using Ghost.Utility;

namespace RO
{
	public class ObjectHolder : ResourceHolder 
	{
		public Object obj{get;private set;}
		public bool immediately = false;
		public ObjectHolder(Object o, bool imd = false)
		{
			obj = o;
			immediately = imd;
		}

		#region override
		protected override void ReleaseUnmanagedResource()
		{
			if (null != obj)
			{
				if (immediately)
				{
					Object.DestroyImmediate(obj);
				}
				else
				{
					Object.Destroy(obj);
				}
				obj = null;
			}
		}
		#endregion override
	}
} // namespace RO
