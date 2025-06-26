using UnityEngine;
using System.Collections;
using SLua;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RO.Test;

[SLua.CustomLuaClassAttribute]
public class MyLuaSrv
{
    static MyLuaSrv _Instance;
    LuaSvr _origin;
    static Func<string, string> processFile;
    private static System.Collections.Generic.HashSet<string> ms_includedFiles = new System.Collections.Generic.HashSet<string>();

#if UNITY_IPHONE && !UNITY_EDITOR
	const string PB = "__Internal";
#else
    const string PB = "slua";
#endif

    //	[DllImport(PB, CallingConvention = CallingConvention.Cdecl)]
    //	public static extern int luaopen_pb (IntPtr luaState);

    //	[DllImport(PB, CallingConvention = CallingConvention.Cdecl)]
    //	public static extern int luaopen_snapshot (IntPtr luaState);

    //	[DllImport(PB, CallingConvention = CallingConvention.Cdecl)]
    //	public static extern int luaopen_bitLib (IntPtr luaState);

    public LuaState luaState {
        get { return LuaSvr.mainState; }
    }

    public LuaSvr origin {
        get {
            return _origin;
        }
    }

    public static bool EnablePrint =
#if UNITY_EDITOR || UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
        true;
#else
		false;
#endif

    public static MyLuaSrv Instance {
        get {
            if (_Instance == null)
                _Instance = new MyLuaSrv();
            return _Instance;
        }
    }

    public static bool IsRunning {
        get {
            return _Instance != null;
        }
    }

    public static void SDispose()
    {
        if (_Instance != null)
            _Instance.Dispose();
    }

    public void Dispose()
    {
        if (_origin != null) {
            ms_includedFiles.Clear();
            GameObject go = GameObject.Find("LuaSvrProxy");
            if (go != null)
                GameObject.DestroyImmediate(go);
            luaState.Dispose();
            _origin = null;
            _Instance = null;
        }
    }

    private MyLuaSrv(string main = null)
    {
        _origin = new LuaSvr();
        _origin.init(null, () => {
            //			MyLuaSrv.luaopen_pb (luaState.L);
            //			MyLuaSrv.luaopen_snapshot (luaState.L);
            //			MyLuaSrv.luaopen_bitLib (luaState.L);
            LuaDLL.luaS_openextlibs(luaState.L);
            LuaDLL.lua_settop(luaState.L, 0);

            LuaDLL.lua_pushcfunction(luaState.L, import);
            LuaDLL.lua_setglobal(luaState.L, "using");

            LuaDLL.lua_pushcfunction(luaState.L, printStack);
            LuaDLL.lua_setglobal(luaState.L, "stack");

            LuaDLL.lua_pushcfunction(luaState.L, print);
            LuaDLL.lua_setglobal(luaState.L, "print");

            LuaDLL.lua_pushcfunction(luaState.L, buglyError);
            LuaDLL.lua_setglobal(luaState.L, "buglyError");

            LuaDLL.lua_pushcfunction(luaState.L, SetParent);
            LuaDLL.lua_setglobal(luaState.L, "SetParent");

            RO.Net.NetManagerHelper.reg(luaState.L);
            RO.ROProtolFactory.reg(luaState.L);
            RO.GameObjectMapHelper.reg(luaState.L);
        });
    }

    public void SetGlobalImportFunction(Func<string, string> load)
    {
        processFile = load;
        //		LuaDLL.lua_pushcfunction (luaState.L, autoImport);
        //		LuaDLL.lua_setglobal (luaState.L, "autoImport");
    }

    public object this[string path] {
        get {
            return luaState.getObject(path);
        }
        set {
            luaState.setObject(path, value);
        }
    }

    public object DoFile(string fn)
    {
        return luaState.doFile(fn);
    }

    public object DoString(string content)
    {
        return luaState.doString(content);
    }

    public LuaFunction GetFunction(string fn)
    {
        return luaState.getFunction(fn);
    }

    public object CallReceiveProtol(LuaFunction func, int id1, int id2, byte[] data, int dataLength)
    {
        var state = luaState;
        int error = LuaObject.pushTry(state.L);

        LuaObject.pushVar(state.L, id1);
        LuaObject.pushVar(state.L, id2);
        LuaDLL.lua_pushlstring(state.L, data, dataLength);
        bool ret = func.pcall(3, error);
        LuaDLL.lua_remove(state.L, error);
        if (ret) {
            return state.topObjects(error - 1);
        }
        return null;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    internal static int SetParent(IntPtr L)
    {
        LuaTypes t = LuaDLL.lua_type(L, 1);

        if (t == LuaTypes.LUA_TNIL) {
            LuaObject.pushValue(L, false);
        } else {
            if (t == LuaTypes.LUA_TUSERDATA) {
                object a1 = LuaObject.checkObj(L, 1);

                UnityEngine.Object a2;
                LuaObject.checkType(L, 2, out a2);
                if (a1 != null) {
                    t = LuaDLL.lua_type(L, 2);
                    if (a1 is GameObject && ((GameObject)a1) != null) {
                        if (a2 != null)
                        {
                            if (a2 is GameObject)
                                ((GameObject)a1).transform.parent = a2 != null ? ((GameObject)a2).transform : null;
                            else if (a2 is Component)
                                ((GameObject)a1).transform.parent = a2 != null ? ((Component)a2).transform : null;

                        } else
                            ((GameObject)a1).transform.parent = null;
                        LuaObject.pushValue(L, true);
                    } else if (a1 is Component && ((Component)a1) != null) {
                        if (a2 != null)
                        {
                            if (a2 is GameObject)
                                ((Component)a1).transform.parent = a2 != null ? ((GameObject)a2).transform : null;
                            else if (a2 is Component)
                                ((Component)a1).transform.parent = a2 != null ? ((Component)a2).transform : null;

                        } else
                            ((Component)a1).transform.parent = null;
                        LuaObject.pushValue(L, true);
                    } else
                        LuaObject.pushValue(L, false);
                } else
                    LuaObject.pushValue(L, false);
            } else
                LuaObject.pushValue(L, false);
        }
        return 1;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    internal static int buglyError(IntPtr L)
    {
        LuaDLL.lua_getglobal(L, "debug");
        LuaDLL.lua_getfield(L, -1, "traceback");
        LuaDLL.lua_pushvalue(L, 1);
        LuaDLL.lua_pushnumber(L, 2);
        LuaDLL.lua_call(L, 2, 1);
        LuaDLL.lua_remove(L, -2);
        string error = LuaDLL.lua_tostring(L, -1);
        SLua.SluaLogger.Log(error);
        BuglyAgent.PrintLog(LogSeverity.LogError, error);
        LuaDLL.lua_pop(L, 1);

        return 0;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    internal static int print(IntPtr L)
    {
        if (EnablePrint) {
            if (LuaLoadOverrider.Me != null && LuaLoadOverrider.Me.printWithStack) {
                return printStack(L);
            } else
                return SLua.LuaState.print(L);
        }
        return 0;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    internal static int printStack(IntPtr L)
    {
        if (EnablePrint) {
            LuaDLL.lua_getglobal(L, "debug");
            LuaDLL.lua_getfield(L, -1, "traceback");
            LuaDLL.lua_pushvalue(L, 1);
            LuaDLL.lua_pushnumber(L, 2);
            LuaDLL.lua_call(L, 2, 1);
            LuaDLL.lua_remove(L, -2);
            SLua.SluaLogger.Log(LuaDLL.lua_tostring(L, -1));
            LuaDLL.lua_pop(L, 1);
        }
        return 0;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    internal static int import(IntPtr l)
    {
        LuaDLL.luaL_checktype(l, 1, LuaTypes.LUA_TSTRING);
        string str = LuaDLL.lua_tostring(l, 1);
        if (ms_includedFiles.Contains(str)) {
            return 0;
        } else {
            ms_includedFiles.Add(str);
        }
        return LuaState.import(l);
    }

    public static string GetFullPath(string lua)
    {
        if (processFile != null)
            lua = processFile(lua);
        return lua;
    }

    public int LuaStateMemory {
        get {
            if (luaState != null) {
                return LuaDLL.lua_gc(luaState.L, LuaGCOptions.LUA_GCCOUNT, 0);
            }
            return -1;
        }
    }

    public void LuaManualGC()
    {
        if (luaState != null) {
            LuaDLL.lua_gc(luaState.L, LuaGCOptions.LUA_GCCOLLECT, 0);
        }
    }

    public static void MonoGC()
    {
        GC.Collect();
    }

    public static void ClearLuaMapAsset()
    {
        LuaLoadOverrider.Me.ClearLuaMapAsset();
    }
//
//	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
//	internal static int autoImport (IntPtr l)
//	{
//		LuaDLL.luaL_checktype (l, 1, LuaTypes.LUA_TSTRING);
//		string str = LuaDLL.lua_tostring (l, 1);
//		if (processFile != null)
//			str = processFile (str);
//		if (ms_includedFiles.Contains (str)) {
//			return 0;
//		} else {
//			ms_includedFiles.Add (str);
//		}
//		LuaDLL.lua_remove(l,1);
//		LuaDLL.lua_pushstring(l,str);
//		return LuaState.loader (l);
//	}
}
