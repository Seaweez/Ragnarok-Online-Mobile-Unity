using RO;
using SLua;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace EditorTool
{
    public class NpcCfgCell : CfgCellBase<NpcCfg>
    {

        public NpcCfgCell() : base() { }

        public override void ReInitUI()
        {
            base.ReInitUI();
            new EButton("加载模型", OnLoadAvatar);
        }

        void OnLoadAvatar(EComponentBase btn, bool isPress)
        {
            if (isPress)
            {
                NpcTable manager = ConfigManager.Instance.GetTable("Npc") as NpcTable;
                manager.ShowAvatar(cfgdata);
            }
        }
    }

    public class NpcCfg : CfgBase
	{
		/**Table_Npc[1000]数据示例
        //[1000] = {id = 1000, NameZh = '读取时替代用npc', ShowName = 2, Guild = '', GuildEmblem = '', MapIcon = '', AtlasOpt = '', Position = '', Type = 'NPC', Race = 'DemiHuman', Nature = 'Neutral', Shape = 'M', Body = 1001, Gender = 1, BodyDefaultColor = 0, Hair = 1001, VisitVocal = '', EndVocal = '', Icon = 'Man', AdventureReward = _EmptyTable, Desc = '', MoveSpdRate = 1, SpawnSE = '', DefaultDialog = 96399, NpcFunction = _EmptyTable, RequireNpcFunction = _EmptyTable, FnIcon = '', FnDesc = '', IsVeer = 1, move = 0, AccessRange = 2, Emoji = 1, Action = 9, NoShowIcon = 1, }, 
         **/
		private static string _RolePartPath = "Assets/Resources/Role/{0}/{1}.prefab";
		private static string _ErrorFormat = "Npc表错误： ID = {0} , {1}";
		public override string errorFormat { get { return _ErrorFormat; } }
		private static string _WarnsFormat = "Npc表警告： ID = {0} , {1}";
		public override string warnsFormat { get { return _WarnsFormat; } }
		private static string _CfgMissErrorFormat = "{0}表中 id = {1} 配置缺失！";

		//搜索时匹配id和Name两个属性
		static string[] _SearchFieldArr = new string[] { "id", "NameZh", "NameEn" };
		public override string[] searchFieldArr { get { return _SearchFieldArr; } }

		public static Dictionary<string, string> partNameFloderNameMap = new Dictionary<string, string>()
		{
			{"Body","Body"},
			{"Hair","Hair"},
			{"LeftHand","Weapon"},
			{"RightHand","Weapon"},
			{"Head","Head"},
			{"Wing","Wing"},
			{"Face","Face"},
			{"Tail","Tail"},
			{"Eye","Eye"},
			{"Mount","Mount"}
		};

		#region 自定义每一条目的数据
		private int _HairColorIndex = 0;
		private int _EyeColorIndex = 0;
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
            new CfgEntry("ShowName"),
			new CfgEntry("Guild"),
			new CfgEntry("GuildEmblem"),
			new CfgEntry("MapIcon"),
			new CfgEntry("AtlasOpt"),
			new CfgEntry("Position"),
			new CfgEntry("Type"),
			new CfgEntry("Race"),
			new CfgEntry("Nature"),
			new CfgEntry("Shape"),
			new CfgEntry("Body"),
			new CfgEntry("Gender"),
			new CfgEntry("BodyDefaultColor"),
			new CfgEntry("Hair"),
			new CfgEntry("VisitVocal"),
			new CfgEntry("EndVocal"),
			new CfgEntry("Icon"),
			new CfgEntry("AdventureReward"),
			new CfgEntry("Desc"),
			new CfgEntry("MoveSpdRate"),
			new CfgEntry("SpawnSE"),
			new CfgEntry("DefaultDialog"),
			new CfgEntry("NpcFunction"),
			new CfgEntry("RequireNpcFunction"),
			new CfgEntry("FnIcon"),
			new CfgEntry("FnDesc"),
			new CfgEntry("IsVeer"),
			new CfgEntry("move"),
			new CfgEntry("AccessRange"),
			new CfgEntry("Emoji"),
			new CfgEntry("Action"),
			new CfgEntry("NoShowIcon"),
			new CfgEntry("LeftHand"),
			new CfgEntry("RightHand"),
			new CfgEntry("Head"),
			new CfgEntry("Wing"),
			new CfgEntry("Face"),
			new CfgEntry("Tail"),
			new CfgEntry("Eye"),
			new CfgEntry("Mount"),
			new CfgEntry("HeadDefaultColor"),
			new CfgEntry("EyeDefaultColor"),
			new CfgEntry("Mouth"),
			new CfgEntry("DisableWait"),
			new CfgEntry("DisablePlayshow")
		};
		#endregion

		private static Dictionary<string, string> _BindMap = new Dictionary<string, string>()
		{
			{"Body", "Table_Body" },
			{"Mount", "Table_Mount" },
		};

		public NpcCfg() : base() { }

		public override void Init(int k, LuaTable table)
		{
			base.Init (k, table);
			_HairColorIndex = LuaWorker.GetFieldInt(table, "HairColorIndex");
			_EyeColorIndex = LuaWorker.GetFieldInt(table, "EyeColorIndex");
			InitRoleParts();
		}

		#region 加载Avatar相关
		private static string _DefaultPartId = "0";
		private static string[] _PartsKeyArr = new string[] 
		{
			"Body", "Hair", "LeftWeapon", "RightWeapon", "Head", "Wing", "Face", "Tail", "Eye", "Mouth", "Mount", "Gender", "HairColorIndex", "EyeColorIndex", "SmoothDisplay"
		};
		private string[] _RoleParts = new string[11];

		private void InitRoleParts()
		{
			string v = string.Empty;
			for (int i = 0; i < _RoleParts.Length; i++)
			{
				v = GetValueBykey(_PartsKeyArr[i]);
				_RoleParts[i] = (string.IsNullOrEmpty(v)) ? _DefaultPartId : v;
			}
		}

		string m_sCompletePath = "Assets/Resources/RoleComplete.prefab";
		GameObject m_goComplete;
		List<RolePart> _Parts = new List<RolePart>();
		public void CreateAvatar()
		{
			m_goComplete = AssetDatabase.LoadAssetAtPath<GameObject>(m_sCompletePath);
			m_goComplete = GameObject.Instantiate<GameObject>(m_goComplete);
			Selection.activeGameObject = m_goComplete;
			m_goComplete.SetActive(true);
			RoleComplete m_cRoleComplete = m_goComplete.GetComponent<RoleComplete>();
			Selection.activeGameObject = m_goComplete;
			RolePart part;
			GameObject obj;
			for (int i = 0; i < _RoleParts.Length; i++)
			{
				if (_RoleParts[i] != _DefaultPartId)
				{
					string _PartName = _PartsKeyArr[i];
					if (partNameFloderNameMap.ContainsKey(_PartName))
						_PartName = partNameFloderNameMap[_PartName];
					string _PartPath = string.Format(_RolePartPath, _PartName, _RoleParts[i]);
					obj = AssetDatabase.LoadAssetAtPath<GameObject>(_PartPath);
					Selection.activeGameObject = obj;
					obj = GameObject.Instantiate<GameObject>(obj);
					obj.SetActive(true);
					part = obj.GetComponent<RolePart>();
					part.componentsDisable = false;
					_Parts.Add(part);
					m_cRoleComplete.SetPart(i, part, true);
				}
			}
			m_cRoleComplete.hairColorIndex = _HairColorIndex;
			m_cRoleComplete.eyeColorIndex = _EyeColorIndex;
		}

		public void DestroyAvatar()
		{
			for(int i = 0; i < _Parts.Count; i++)
			{
				if(_Parts[i] != null && _Parts[i].gameObject != null)
					GameObject.DestroyImmediate(_Parts[i].gameObject);
			}
			_Parts.Clear();
			GameObject.DestroyImmediate(m_goComplete);
			m_goComplete = null;
		}
		#endregion

		private string GetValueBykey(string key)
		{
			if (map.ContainsKey(key))
				return map [key].value;
			return string.Empty;
		}

		public override void Check()
		{
			PartsCheck();
			GuildIconCheck();
			MapIconCheck();
			HeadIconCheck();
			SceneTopIconCheck();
			AudioCheck("VisitVocal");
			AudioCheck("EndVocal");
			AudioCheck("SpawnSE");
			TryNotifyChanged();
		}

		public override void DeepCheck()
		{
			PartsCheck(true);
			TryNotifyChanged(false);
		}

		private void GuildIconCheck()
		{
			SpriteCheck("GuildEmblem", FindEntry("AtlasOpt").value);
		}

		private void MapIconCheck()
		{
			SpriteCheck("MapIcon", "Map");
		}

		private void HeadIconCheck()
		{
			CfgEntry entry = FindEntry("Icon");
			if(!string.IsNullOrEmpty(entry.value) && entry.value != "0")
				SpriteCheck(entry, "face");
		}

		private void SceneTopIconCheck()
		{
			SpriteCheck("FnIcon", "uiicon");
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

		private void AudioCheck(string entryKey)
        {
            CfgEntry entry = FindEntry(entryKey);
            string error = string.Empty;
            ScriptChecker.CheckAudioByCfg(ref error, entry.value);
            if (!string.IsNullOrEmpty(error))
                AddError(error, entry);
        }

		private void PartsCheck(bool isDeepCheck = false)
		{
			foreach (var item in partNameFloderNameMap)
			{
				CfgEntry entry = FindEntry(item.Key);
				//有绑定关系的会在被绑定的配置中检查资源，比如Mount
				if (entry.bindCfg != null)
					continue;
				if (string.IsNullOrEmpty(entry.value) || entry.value == "0")
					continue;

				string path = string.Format(_RolePartPath, item.Value, entry.value);
				CheckFileExist(entry, path);
				if (isDeepCheck)
					DeepCheckScript(entry, path);
			}
		}

		private void CheckFileExist(CfgEntry entry, string path)
		{
			if (!AssetChecker.CheckAssetFile(path))
			{
				string error = string.Format(fileMissErrorFormat, string.Format(_RolePartPath, partNameFloderNameMap[entry.key], entry.value));
				AddError(error, entry);
			}
		}

		private void DeepCheckScript(CfgEntry entry, string path)
		{
			GameObject go = AssetChecker.TryLoadAsset<GameObject>(path);
			if (go != null)
			{
				string error = string.Empty;
				if (entry.key == "Body")
					ScriptChecker.CheckRolePartBody(go, ref error);
				else
					ScriptChecker.CheckRolePart(go, ref error);
				if (!string.IsNullOrEmpty(error))
					AddError(error, entry);
			}
			else
			{
				string error = string.Format(fileMissErrorFormat, string.Format(_RolePartPath, partNameFloderNameMap[entry.key], entry.value));
				AddError(error, entry);
			}
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
	}
}

