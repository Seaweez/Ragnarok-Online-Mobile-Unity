using UnityEngine;
using UnityEditor;
using RO;
using System.Collections.Generic;
using SLua;
using System;

namespace EditorTool
{
    public class NpcTable : TableBase<NpcCfgCell, NpcCfg>
    {
        private GenAssets.WorkerContainer container;

        public NpcTable(string name) : base(name) { }

        public override void LoadLuaTable()
        {
			luaTable = GenAssets.GetAssetsInfo(out container, "NPC", tableName);
			if(luaTable == null)
				throw new ArgumentOutOfRangeException(string.Format("{0}表不存在，或者读取错误！", tableName));
        }

        NpcCfg _CurrentShow = null;
        public void ShowAvatar(NpcCfg cfg)
        {
            if (_CurrentShow != null)
                _CurrentShow.DestroyAvatar();
            _CurrentShow = cfg;
            _CurrentShow.CreateAvatar();
        }
    }
}
