using System;

namespace EditorTool
{
	public class AssesoriesTable : TableBase<AssesoriesCfgCell, AssesoriesCfg>
	{
		private GenAssets.WorkerContainer container;
		public AssesoriesTable (string name) : base(name) { }
		public override void LoadLuaTable()
		{
			luaTable = GenAssets.GetAssetsInfo(out container, "Assesories", tableName);
			if(luaTable == null)
				throw new ArgumentOutOfRangeException(string.Format("{0}表不存在，或者读取错误！", tableName));
		}
	}
}

