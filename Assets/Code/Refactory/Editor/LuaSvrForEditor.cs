using UnityEngine;
using UnityEditor;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SLua;

namespace EditorTool
{
	public class LuaSvrForEditor : IDisposable
	{
		public static readonly LuaSvrForEditor Me = new LuaSvrForEditor();

		public LuaState luaState;
		int errorReported = 0;

		public bool inited = false;
		
		private LuaSvrForEditor()
		{
			init();
		}

		public void Dispose()
		{
			uninit();
		}
		
		void checkTop(IntPtr L)
		{
			if (LuaDLL.lua_gettop(luaState.L) != errorReported)
			{
				Debug.LogError("Some function not remove temp value from lua stack. You should fix it.");
				errorReported = LuaDLL.lua_gettop(luaState.L);
			}
		}
		
		public void init()
		{
			if (inited)
			{
				return;
			}
			inited = true;

			luaState = new LuaState();

			IntPtr L = luaState.L;
			LuaObject.init(L);
			checkTop(L);
		}

		public void uninit()
		{
			if (!inited)
			{
				return;
			}
			inited = false;

			luaState.Dispose();
			luaState = null;
		}

		public object DoString (string content)
		{
			if (!inited)
			{
				return null;
			}
			return luaState.doString (content);
		}
	
	}
} // namespace EditorTool
