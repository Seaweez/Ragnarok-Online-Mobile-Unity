using UnityEngine;
using System.Collections.Generic;
using SLua;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class LuaWorker : IDisposable
	{
		protected string _LuaFile;
		protected string _LuaClassName;
		protected Dictionary<string,LuaFunction> _cachedFunc;

		public LuaTable table{ get; protected set; }

		public Action<LuaTable> DoInit;
		protected bool _isLuaReady;

		public string LuaFile {
			get {
				return _LuaFile;
			}
			set {
				_LuaFile = value;
				if (string.IsNullOrEmpty (_LuaFile) == false) {
					string[] splits = _LuaFile.Split ('.', '/');
					_LuaClassName = splits [splits.Length - 1];
				}
			}
		}

		public bool isLuaReady {
			get {
				return _isLuaReady;
			}
		}

		public LuaWorker (string Lua = null)
		{
			LuaFile = Lua;
		}
		
		public string LuaClassName {
			get {
				return _LuaClassName;
			}
		}

		protected MyLuaSrv env {
			get {
				return MyLuaSrv.Instance;
			}
		}

		//获取绑定的lua脚本
		public LuaTable GetChunk ()
		{
			return table;
		}

		//设置lua脚本可直接使用变量
		public void SetEnv (string key, object val, bool isGlobal)
		{
			if (isGlobal) {
				env [key] = val;
			} else {
				if (table != null) {
					table [key] = val;
				}
			}
		}

		virtual public void DoString (string content)
		{
			if (string.IsNullOrEmpty (content)) {
				RO.LoggerUnused.Log ("dostring调用lua，传入Lua为空");
				return;
			}
			try {
				object chunk = env.DoString (content);
				if (chunk != null && (chunk is LuaTable)) {
					Dispose ();
					table = (LuaTable)chunk;
				}
			} catch (System.Exception e) {
				RO.LoggerUnused.LogError (FormatException (e));
			}
		}

		
		//加载脚本文件
		virtual public void DoFile (string fn = null)
		{
			if (string.IsNullOrEmpty (fn)) {
				fn = LuaFile;
				if (string.IsNullOrEmpty (LuaFile))
					return;
			}
			try {
				object chunk = env.DoFile (fn);
				if (chunk != null && (chunk is LuaTable)) {
					Dispose ();
					table = (LuaTable)chunk;
					if (DoInit != null)
						DoInit ((LuaTable)chunk);
					_isLuaReady = true;
				}
			} catch (System.Exception e) {
				_isLuaReady = false;
				RO.LoggerUnused.LogError (FormatException (e));
			}
		}

		virtual public object CallLuaStaticMethod (string func, params object[] args)
		{
			if (table == null)
				return null;

			LuaFunction function = GetFunction(func);
			
			if (function == null)
				return null;
			try {
				if (args != null && args.Length > 0) {
					return function.call (args);
				}
				return function.call ();
			} catch (System.Exception e) {
				RO.LoggerUnused.LogWarning (FormatException (e));
			}
			return null;
		}

		virtual public object CallLuaInstanceMethod (string func, params object[] args)
		{
			if (table == null)
				return null;
			
			LuaFunction function = GetFunction(func);
			
			if (function == null)
				return null;
			try {
				if (args != null && args.Length > 0) {
					return function.call (args);
				}
				return function.call (table);
			} catch (System.Exception e) {
				RO.LoggerUnused.LogWarning (FormatException (e));
			}
			return null;
		}

		LuaFunction GetFunction (string func)
		{
			LuaFunction luafunc = null;
			if (_cachedFunc != null) {
				if (_cachedFunc.TryGetValue (func, out luafunc)) {
					return luafunc;
				}
			}
			if (luafunc == null) {
				luafunc = (LuaFunction)table [func];
				if (luafunc != null) {
					if (_cachedFunc == null) {
						_cachedFunc = new Dictionary<string, LuaFunction> ();
					}
					_cachedFunc.Add (func, luafunc);
				}
			}
			return luafunc;
		}

		public void Dispose ()
		{
			_cachedFunc = null;
			if (table != null)
				table.Dispose ();
		}

		#region static method
		public static string FormatException (System.Exception e)
		{
			string source = (string.IsNullOrEmpty (e.Source)) ? "<no source>" : e.Source.Substring (0, e.Source.Length - 2);
			return string.Format ("{0}\nLua (at {2})", e.Message, string.Empty, source);
		}

		public static int GetFieldInt (LuaTable t, string k)
		{
			if (null == t) {
				return 0;
			}
			var v = t [k];
			if (null == v) {
				return 0;
			}
			return System.Convert.ToInt32 (v);
		}

		public static int GetFieldInt (LuaTable t, int k)
		{
			if (null == t) {
				return 0;
			}
			var v = t [k];
			if (null == v) {
				return 0;
			}
			return System.Convert.ToInt32 (v);
		}

		public static long GetFieldLong (LuaTable t, string k)
		{
			if (null == t) {
				return 0;
			}
			var v = t [k];
			if (null == v) {
				return 0;
			}
			return System.Convert.ToInt64 (v);
		}

		public static long GetFieldLong (LuaTable t, int k)
		{
			if (null == t) {
				return 0;
			}
			var v = t [k];
			if (null == v) {
				return 0;
			}
			return System.Convert.ToInt64 (v);
		}

		public static float GetFieldFloat (LuaTable t, string k)
		{
			if (null == t) {
				return 0;
			}
			var v = t [k];
			if (null == v) {
				return 0;
			}
			return System.Convert.ToSingle (v);
		}

		public static float GetFieldFloat (LuaTable t, int k)
		{
			if (null == t) {
				return 0;
			}
			var v = t [k];
			if (null == v) {
				return 0;
			}
			return System.Convert.ToSingle (v);
		}

		public static string GetFieldString (LuaTable t, string k)
		{
			if (null == t) {
				return null;
			}
			var v = t [k];
			if (null == v) {
				return null;
			}
			return System.Convert.ToString (v);
		}

		public static string GetFieldString (LuaTable t, int k)
		{
			if (null == t) {
				return null;
			}
			var v = t [k];
			if (null == v) {
				return null;
			}
			return System.Convert.ToString (v);
		}

		public static LuaTable GetFieldTable (LuaTable t, string k)
		{
			if (null == t) {
				return null;
			}
			var v = t [k];
			if (null == v) {
				return null;
			}
			return v as LuaTable;
		}

		public static LuaTable GetFieldTable (LuaTable t, int k)
		{
			if (null == t) {
				return null;
			}
			var v = t [k];
			if (null == v) {
				return null;
			}
			return v as LuaTable;
		}
		#endregion static method

	}
} // namespace RO
