using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class RolePartMount : RolePartBodyMount 
	{
		#region override
		public override void ApplyLayer ()
		{
			ApplyLayerIgnoreCPs();
		}
		#endregion override
	}
} // namespace RO
