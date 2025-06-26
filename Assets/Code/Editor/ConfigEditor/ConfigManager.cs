using Ghost.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace EditorTool
{
	public enum ConfigFilterType
	{
		None = 0,
		Search = 1 << 0,
		Error = 1 << 1,
		Warning = 1 << 2,
		All = Search | Error | Warning
	}

    public class ConfigManager
    {
        public List<ITableBase> configList = new List<ITableBase>() {
            {new NpcTable("Table_Npc") },
            {new NpcTable("Table_Monster") },
            {new BodyTable("Table_Body") },
			{new MountTable("Table_Mount") },
			{new HairStyleTable("Table_HairStyle") },
			{new EquipTable("Table_Equip") },
			{new ItemTable("Table_Item") },
			{new ItemTypeTable("Table_ItemType") },
			{new Table_ItemTypeAdventureLog("Table_ItemTypeAdventureLog") },
			{new AssesoriesTable("Table_Assesories") },
			{new EyeTable("Table_Eye") },
			{new EffectTable("Effects") },
            {new AudioTable("Audios") }
        };

        static ConfigManager _Instance;
        public static ConfigManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ConfigManager();
                return _Instance;
            }
        }

        public ConfigManager()
        {

        }

        public void InitCfgManager()
        {
            //1. 加载所有Manager的LuaTable，并初始化为c#数据结构
            var infos = DisableGenAssetsDebugInfo();
            for(int i=0; i<configList.Count; i++)
            {
				try
				{
					DisplayProgressBar ("【1/4】第一步加载所有表", string.Format ("{0}加载中，加载进度【{1}/{2}】", configList[i].tableName, i+1, configList.Count), i+1);
					configList[i].InitData();
				}
				catch { EditorUtility.ClearProgressBar (); }
            }
            RevertGenAssetsDebugInfo(infos);

            //2. 创建所有Manager的绑定关系
            for(int i=0; i<configList.Count; i++)
			{
				try
				{
					DisplayProgressBar ("【2/4】第二步创建所有表的绑定关系", string.Format ("{0}创建绑定关系中，绑定进度【{1}/{2}】", configList[i].tableName, i+1, configList.Count), i+1);
					configList[i].CreateCfgBind();
				}
				catch { EditorUtility.ClearProgressBar (); }
            }

            //3. 检查所有错误信息
            for(int i=0; i<configList.Count; i++)
			{
				try
				{
					DisplayProgressBar ("【3/4】第三步检查所有表", string.Format ("{0}检查中，检查进度【{1}/{2}】", configList[i].tableName, i+1, configList.Count), i+1);
					configList[i].CfgCheck();
				}
				catch { EditorUtility.ClearProgressBar (); }
            }

            //4. 初始化UI数据
            for(int i=0; i<configList.Count; i++)
			{
				try
				{
					DisplayProgressBar ("【4/4】第四步初始化所有表的UI", string.Format ("{0}初始化中，初始化进度【{1}/{2}】，", configList[i].tableName, i+1, configList.Count), i+1);
					configList[i].InitUI();
				}
				catch { EditorUtility.ClearProgressBar (); }
			}
			EditorUtility.ClearProgressBar ();
        }

		public void Clear()
		{
			_Instance = null;
		}

        public void DeepCheckAllTable()
        {
            for(int i=0; i<configList.Count; i++)
            {
                configList[i].CfgDeepCheck();
            }
        }

        public ITableBase GetTable(string name)
        {
            for(int i=0; i<configList.Count; i++)
            {
                if(configList[i].tableName == name)
                    return configList[i];
            }
            return null;
        }

		public void StatisticErrorNum(ref int errornum, ref int warnnum)
        {
            for(int i=0; i<configList.Count; i++)
            {
                configList[i].StatisticErrorNum(ref errornum, ref warnnum);
            }
        }

        Dictionary<string, bool> DisableGenAssetsDebugInfo()
        {
            Dictionary<string, bool> dic = null;
            var debugInfos = UnityEditor.AssetDatabase.LoadAssetAtPath<GenAssetsDebug>("Assets/GenAssetsDebugInfo.asset");
            if (null != debugInfos && !debugInfos.infos.IsNullOrEmpty())
            {
                dic = new Dictionary<string, bool>();
                foreach (var debugInfo in debugInfos.infos)
                {
                    if(!dic.ContainsKey(debugInfo.tableName))
                        dic.Add(debugInfo.tableName, debugInfo.enable);
                    debugInfo.enable = false;
                }
                UnityEditor.EditorUtility.SetDirty(debugInfos);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEngine.Resources.UnloadAsset(debugInfos);
            }
            return dic;
        }

        void RevertGenAssetsDebugInfo(Dictionary<string, bool> infos)
        {
            var debugInfos = UnityEditor.AssetDatabase.LoadAssetAtPath<GenAssetsDebug>("Assets/GenAssetsDebugInfo.asset");
            if (infos != null && null != debugInfos && !debugInfos.infos.IsNullOrEmpty())
            {
                foreach (var debugInfo in debugInfos.infos)
                {
                    if (infos.ContainsKey(debugInfo.tableName))
                        debugInfo.enable = infos[debugInfo.tableName];
                }
                UnityEditor.EditorUtility.SetDirty(debugInfos);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEngine.Resources.UnloadAsset(debugInfos);
            }
		}

		private void DisplayProgressBar(string title, string info, int idx)
		{
			float v = idx > 0 && configList.Count > 0 ? (float)idx / configList.Count : 0;
			EditorUtility.DisplayProgressBar(title, info, v);
		}

		#region 筛选和搜索相关

		public ConfigFilterType currentFilter = ConfigFilterType.None;
		public string searchContent = string.Empty;

		public void SetSearchContent(string content)
		{
			bool _IsNewStr = false;
			if (!searchContent.Equals (content))
				_IsNewStr = true;
			searchContent = content;
			if (string.IsNullOrEmpty(searchContent))
				DeleteFilter (ConfigFilterType.Search);
			else
				AddFilter(ConfigFilterType.Search);
			if (_IsNewStr)
				OnFilter ();
		}

		public void DeleteFilter(ConfigFilterType t)
		{
			if (currentFilter != ConfigFilterType.None && t != ConfigFilterType.None)
			{
				currentFilter &= ConfigFilterType.All ^ t;
			}
		}

		public void ClearFilters()
		{
			currentFilter = ConfigFilterType.None;
			OnFilter ();
		}

		public void AddFilter(ConfigFilterType t)
		{
			currentFilter |= t;
		}

		public void OnFilter ()
		{
			for(int i=0; i<configList.Count; i++)
            {
                configList[i].OnFilter (currentFilter, searchContent);
			}
		}
		#endregion
    }
}
