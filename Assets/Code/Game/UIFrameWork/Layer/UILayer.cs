using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public enum UILayer
	{
		Scene,
		BottomMost,
		Bottom,
		Lower,
		Normal,
		Upper,
		Top,
		TopMost,
		Screen,
		
		LayerCount
	}
} // namespace RO
