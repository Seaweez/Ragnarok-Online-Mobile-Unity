using System;

namespace EditorTool
{
	public class ItemTable : TableBase<ItemCfgCell, ItemCfg>
	{
		private GenAssets.WorkerContainer container;
		public ItemTable (string name) : base(name) { }

		public override void LoadLuaTable()
		{
			luaTable = GenAssets.GetAssetsInfo (out container, "Item", tableName);
			if (luaTable == null)
				throw new ArgumentOutOfRangeException (string.Format ("{0}表不存在，或者读取错误！", tableName));
		}
	}
}

