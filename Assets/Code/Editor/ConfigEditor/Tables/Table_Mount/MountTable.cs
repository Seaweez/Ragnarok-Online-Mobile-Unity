using UnityEngine;
using UnityEditor;
using RO;
using System.Collections.Generic;
using SLua;
using System;

namespace EditorTool
{
	public class MountTable : TableBase<MountCfgCell, MountCfg>
	{
		private GenAssets.WorkerContainer container;

		public MountTable(string name) : base(name) { }

		public override void LoadLuaTable()
		{
			luaTable = GenAssets.GetAssetsInfo(out container, "Mount", tableName);
			if(luaTable == null)
				throw new ArgumentOutOfRangeException(string.Format("{0}表不存在，或者读取错误！", tableName));
		}
	}
}
