using UnityEngine;
using System.Collections.Generic;

namespace RO.Config
{
	[SLua.CustomLuaClassAttribute]
	public static class RolePoint
	{
		#region connect point
		public const int CONNECT_HAIR = 1;
		public const int CONNECT_LEFT_HAND = 2;
		public const int CONNECT_RIGHT_HAND = 3;
		public const int CONNECT_HEAD = 4;
		public const int CONNECT_WING = 5;
		public const int CONNECT_FACE = 6;
		public const int CONNECT_TAIL = 7;

		public const int CONNECT_BODY = 0;
		#endregion

		#region effect point
		public const int EFFECT_TOP = 1;
		public const int EFFECT_BOTTOM = 2;
		public const int EFFECT_MIDDLE = 3;
		public const int EFFECT_LEFT_HAND = 4;
		public const int EFFECT_RIGHT_HAND = 5;
		public const int EFFECT_CHEST = 6;
		public const int EFFECT_BACK = 7;
		#endregion
	}
} // namespace RO.Config
