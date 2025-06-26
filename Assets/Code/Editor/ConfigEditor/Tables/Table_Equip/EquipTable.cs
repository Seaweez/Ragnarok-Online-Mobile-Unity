using System;

namespace EditorTool
{
	public class EquipTable : TableBase<EquipCfgCell, EquipCfg>
	{
		private GenAssets.WorkerContainer container;

		public EquipTable(string name) : base(name) { }

		public override void LoadLuaTable()
		{
			luaTable = GenAssets.GetAssetsInfo(out container, "Equip", tableName);
			if(luaTable == null)
				throw new ArgumentOutOfRangeException(string.Format("{0}表不存在，或者读取错误！", tableName));
		}
	}
}

