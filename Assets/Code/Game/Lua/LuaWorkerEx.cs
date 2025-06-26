using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class LuaWorkerEx : LuaWorker 
	{
		public LuaWorkerEx(SLua.LuaTable t)
		{
			table = t;
		}
	
	}
} // namespace RO
