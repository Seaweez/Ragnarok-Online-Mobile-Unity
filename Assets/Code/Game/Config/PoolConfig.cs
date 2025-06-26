using UnityEngine;
using System.Collections.Generic;

namespace RO.Config
{
	[SLua.CustomLuaClassAttribute]
	public static class Pool
	{
		public const string NAME_DEFAULT = "POOL_DEFAULT";
		public const string NAME_AUDIO = "POOL_AUDIO";
		public const string NAME_EFFECT = "POOL_EFFECT";
	}
} // namespace RO.Config
