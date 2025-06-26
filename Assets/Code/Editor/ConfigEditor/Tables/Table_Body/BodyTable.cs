using UnityEngine;
using UnityEditor;
using RO;
using System.Collections.Generic;
using SLua;
using System;

namespace EditorTool
{
    public class BodyTable : TableBase<BodyCfgCell, BodyCfg>
    {
        private GenAssets.WorkerContainer container;

        public BodyTable(string name) : base(name) { }

        public override void LoadLuaTable()
        {
            luaTable = GenAssets.GetAssetsInfo(out container, "Body", tableName);
			if(luaTable == null)
				throw new ArgumentOutOfRangeException(string.Format("{0}表不存在，或者读取错误！", tableName));
        }
    }
}
