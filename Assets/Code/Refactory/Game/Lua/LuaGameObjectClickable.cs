using UnityEngine;
using System.Collections;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class LuaGameObjectClickable : LuaGameObject {
		public int clickPriority = 999;
	}
} // namespace RO
