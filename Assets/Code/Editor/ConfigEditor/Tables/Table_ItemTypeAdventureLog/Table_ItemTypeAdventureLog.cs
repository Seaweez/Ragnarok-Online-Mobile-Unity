using System;
using System.Collections.Generic;
using SLua;

namespace EditorTool
{
	public class Table_ItemTypeAdventureLog : TableBase<ItemTypeAdventureLogCfgCell, ItemTypeAdventureLogCfg>
	{
		public Table_ItemTypeAdventureLog (string name) : base(name) { }

		private GenAssets.WorkerContainer container;

		public override void LoadLuaTable()
		{
			luaTable = GenAssets.GetAssetsInfo (out container, "ItemTypeAdventureLog", tableName);
			if (luaTable == null)
				throw new ArgumentOutOfRangeException (string.Format ("{0}表不存在，或者读取错误！", tableName));
		}
	}

	public class ItemTypeAdventureLogCfgCell : CfgCellBase<ItemTypeAdventureLogCfg>
	{
		public ItemTypeAdventureLogCfgCell () : base() { }

		public override void ReInitUI()
		{
			base.ReInitUI ();
			_ItemLabel.content.text = cfgdata.id + " " + cfgdata.name;
		}
	}

	public class ItemTypeAdventureLogCfg : CfgBase
	{
		private static string _ErrorFormat = "ItemTypeAdventureLogCfg表错误： ID = {0} , {1}";
		public override string errorFormat { get { return _ErrorFormat; } }
		private static string _WarnsFormat = "ItemTypeAdventureLogCfg表警告： ID = {0} , {1}";
		public override string warnsFormat { get { return _WarnsFormat; } }

		//搜索时匹配id和Name两个属性
		static string[] _SearchFieldArr = new string[] { "id", "Name", "NameEn" };
		public override string[] searchFieldArr { get { return _SearchFieldArr; } }
		//初始化时根据data初始化map
		Dictionary<string, CfgEntry> _Map = new Dictionary<string, CfgEntry>(){};
		public override Dictionary<string, CfgEntry> map { get{ return _Map; } set { _Map = value; } }
		//不使用Dictionary，保证顺序
		public override List<CfgEntry> data { get { return _Data; } set { _Data = value; } }
		List< CfgEntry> _Data = new List<CfgEntry>()
		{
			new CfgEntry("id"),
			new CfgEntry("Name"),
            new CfgEntry("NameEn"),
            new CfgEntry("icon"),
			new CfgEntry("Order"),
			new CfgEntry("Position"),
			new CfgEntry("RidTip"),
			new CfgEntry("Classify"),
			new CfgEntry("MenuID"),
			new CfgEntry("GuideID"),
			new CfgEntry("ExchangeOrder"),
			new CfgEntry("ExchangeType"),
			new CfgEntry("RefineOption"),
			new CfgEntry("JobOption"),
			new CfgEntry("LevelOption")
		};

		public ItemTypeAdventureLogCfg () : base() { nameArr = new string[] { "Name", "NameEn" }; }

		public override void Check()
		{
//			SpriteCheck ("icon", "uiicon");
			TryNotifyChanged();
		}

		public override void DeepCheck()
		{
			TryNotifyChanged(false);
		}

		private void SpriteCheck(string spriteKey, string atlasKey)
		{
			CfgEntry entry = FindEntry(spriteKey);
			if (!string.IsNullOrEmpty(entry.value))
			{
				bool checkResult = AssetChecker.CheckAtlasSpriteByCfg(entry.value, atlasKey);
				if (!checkResult)
				{
					string error = string.Format(AssetChecker.atlasSpriteMissErrorFormat, entry.key, entry.value, atlasKey);
					AddError(error, entry);
				}
			}
		}
	}
}

