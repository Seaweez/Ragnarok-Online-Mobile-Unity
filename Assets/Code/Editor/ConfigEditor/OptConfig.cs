using System;
using UnityEditor;
using UnityEngine;
using EditorTool;
using SLua;
using System.IO;
namespace EditorTool
{


    //优化结构
    public class ConfigOptInfo
    {
        public ConfigOptInfo(string sDirPath, string sLuaTableName, string sOptKey)
        {
            m_sDirPath = sDirPath;
            m_sLuaTableName = sLuaTableName;
            m_sOptKey = sOptKey;
        }

        //优化的文件夹
        public string m_sDirPath;

        //需要优化的lua table名字
        public string m_sLuaTableName;

        //优化的关键字
        public string m_sOptKey;
    }

    public class OptConfig : Singleton<OptConfig>
    {
        static protected string s_sLuaRequire = @"
		    return function(path)
			    require(path)
		    end
		    ";

        protected LuaFunction m_oLuaFunction;

        public void OptFile(string file, string savePath, string tableName, string OptKey)
        {
            LuaSvrForEditor.Me.Dispose();
            LuaSvrForEditor.Me.init();

            m_oLuaFunction = LuaSvrForEditor.Me.DoString(s_sLuaRequire) as LuaFunction;

            //加载优化Lua脚本
            m_oLuaFunction.call("Script/tttt");

            //加载脚本
            m_oLuaFunction.call(file);

            //执行Lua优化函数
            LuaSvrForEditor.Me.DoString("OptConfigFun('" + savePath + "','" + tableName + "','" + OptKey  + "')");

        }
        
        public void OptFileTableToString(string file, string savePath, string tableName,string path)
        {
          
            m_oLuaFunction = LuaSvrForEditor.Me.DoString(s_sLuaRequire) as LuaFunction;

            //加载优化Lua脚本
            m_oLuaFunction.call("Script/tttt");

            //加载脚本
            m_oLuaFunction.call(file);

            //执行Lua优化函数
            LuaSvrForEditor.Me.DoString("TableToString('" + savePath + "','" + tableName + "','" + path +"')");

        }
    }
}

