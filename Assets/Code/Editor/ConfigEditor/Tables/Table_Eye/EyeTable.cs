using System;

namespace EditorTool
{
	public class EyeTable : TableBase<EyeCfgCell, EyeCfg>
	{
		private GenAssets.WorkerContainer container;
		public EyeTable (string name) : base(name) { }

		public override void LoadLuaTable()
		{
			luaTable = GenAssets.GetAssetsInfo (out container, "Eye", tableName);
			if(luaTable == null)
				throw new ArgumentOutOfRangeException(string.Format("{0}表不存在，或者读取错误！", tableName));
		}
	}
}

