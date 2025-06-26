using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public abstract class CoreEvent
	{
		public int typeID{get; protected set;}
	}

	public static class CoreEventSingleton<T> where T:CoreEvent,new()
	{
		public static readonly T Me = new T();
	}

} // namespace RO
