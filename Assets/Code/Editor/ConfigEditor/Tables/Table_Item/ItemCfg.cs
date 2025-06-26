using System;
using SLua;
using System.Collections.Generic;

namespace EditorTool
{
    public class ItemCfgCell : CfgCellBase<ItemCfg>
    {
        public ItemCfgCell() : base() { }
    }

    public class ItemCfg : CfgBase
	{
		private static string _ErrorFormat = "Item表错误： ID = {0} , {1}";
		public override string errorFormat { get { return _ErrorFormat; } }
		private static string _WarnsFormat = "Item表警告： ID = {0} , {1}";
		public override string warnsFormat { get { return _WarnsFormat; } }
		private static string _CfgMissErrorFormat = "{0}表中 id = {1} 配置缺失！";

		//搜索时匹配id和Name两个属性
		static string[] _SearchFieldArr = new string[] { "id", "NameZh", "NameEn" };
		public override string[] searchFieldArr { get { return _SearchFieldArr; } }
		//初始化时根据data初始化map
		Dictionary<string, CfgEntry> _Map = new Dictionary<string, CfgEntry>(){};
		public override Dictionary<string, CfgEntry> map { get{ return _Map; } set { _Map = value; } }
		//不使用Dictionary，保证顺序
		public override List<CfgEntry> data { get { return _Data; } set { _Data = value; } }
		List< CfgEntry> _Data = new List<CfgEntry>()
		{
			new CfgEntry("id"),
            new CfgEntry("NameZh"),
            new CfgEntry("NameEn"),
            new CfgEntry("Icon"),
			new CfgEntry("Type"),
			new CfgEntry("Quality"),
			new CfgEntry("MaxNum"),
			new CfgEntry("SellPrice"),
			new CfgEntry("AdventureReward"),
			new CfgEntry("StorageReward"),
			new CfgEntry("Desc"),
			new CfgEntry("ItemTarget"),
			new CfgEntry("NoStorage"),
			new CfgEntry("MediaPath"),
			new CfgEntry("GetLimit"),
			new CfgEntry("JobLimit")
		};

		private static Dictionary<string, string> _BindMap = new Dictionary<string, string>()
		{
			{"Type", "Table_ItemType" }
		};

		public ItemCfg () : base() { }

		public override void Check()
		{
			SpriteCheck("Icon", "Item");
			TryNotifyChanged();
		}

		public override void DeepCheck()
		{
			TryNotifyChanged(false);
		}

		public override void InitBind()
		{
			foreach(var item in _BindMap)
			{
				CfgEntry entry = FindEntry(item.Key);
				ITableBase table = ConfigManager.Instance.GetTable(item.Value);
				if(table != null && !string.IsNullOrEmpty(entry.value) && entry.value != "0")
				{
					CfgBase cfg = table.FindCfgByKey(entry.value);
					if(cfg != null)
						cfg.AddObserver(entry);
					else
					{
						string error = string.Format(_CfgMissErrorFormat, item.Value, entry.value);
						AddError(error, entry);
					}
				}
			}
		}

		private void SpriteCheck(string spriteKey, string atlasKey)
		{
			CfgEntry entry = FindEntry(spriteKey);
			SpriteCheck(entry, atlasKey);
		}

		private void SpriteCheck(CfgEntry entry, string atlasKey)
		{
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

