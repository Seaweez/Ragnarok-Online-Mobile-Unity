using System;

namespace EditorTool
{
	public class ItemTypeTable : TableBase<ItemTypeCfgCell, ItemTypeCfg>
	{
		public ItemTypeTable (string name) : base(name) { }
		private GenAssets.WorkerContainer container;

		public override void LoadLuaTable()
		{
			luaTable = GenAssets.GetAssetsInfo (out container, "ItemType", tableName);
			if (luaTable == null)
				throw new ArgumentOutOfRangeException (string.Format ("{0}表不存在，或者读取错误！", tableName));
		}
	}
}

