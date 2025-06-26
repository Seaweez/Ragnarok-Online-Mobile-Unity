using System.Collections.Generic;
using UnityEngine;

namespace EditorTool
{
    public class EyeCfgCell : CfgCellBase<EyeCfg>
    {
        public EyeCfgCell() : base() { }
    }

    public class EyeCfg : CfgBase
	{
		static string _ModelPath = "Assets/Resources/Role/Eye/{0}.prefab";
		private static string _ErrorFormat = "Equip表错误： ID = {0} , {1}";
		public override string errorFormat { get { return _ErrorFormat; } }
		private static string _WarnsFormat = "Equip表警告： ID = {0} , {1}";
		public override string warnsFormat { get { return _WarnsFormat; } }
		//搜索时比对id Name
		static string[] _SearchFieldArr = new string[] { "id" };
		public override string[] searchFieldArr { get { return _SearchFieldArr; } }
		Dictionary<string, CfgEntry> _Map = new Dictionary<string, CfgEntry>(){};
		public override Dictionary<string, CfgEntry> map { get{ return _Map; } set { _Map = value; } }
		public override List<CfgEntry> data { get { return _Data; } set { _Data = value; } }
		List<CfgEntry> _Data = new List<CfgEntry>()
		{
			new CfgEntry("id"),
            new CfgEntry("NameZh"),
            new CfgEntry("NameEn"),
            new CfgEntry("DefaultColor")
		};

		public EyeCfg () : base() { }

		public override void Check()
		{
			CheckRolePartHair ();
			TryNotifyChanged();
		}

		public override void DeepCheck()
		{
			TryNotifyChanged(false);
		}

		private void CheckRolePartHair(bool isDeepCheck = false)
		{
				CfgEntry entry = FindEntry("id");
				//有绑定关系的会在被绑定的配置中检查资源，比如Mount
				if (entry.bindCfg != null)
					return;
				if (string.IsNullOrEmpty(entry.value))
					return;

				string path = string.Format(_ModelPath, entry.value);
//				CheckFileExist(entry, path);
//				if (isDeepCheck)
				DeepCheckRolePartHair(entry, path);
		}

		private void DeepCheckRolePartHair(CfgEntry entry, string path)
		{
			GameObject go = AssetChecker.TryLoadAsset<GameObject>(path);
			if (go != null)
			{
				string error = string.Empty;
				ScriptChecker.CheckRolePartHair(go, ref error);
				if (!string.IsNullOrEmpty(error))
					AddError(error, entry);
			}
			else
			{
				string error = string.Format(fileMissErrorFormat, path);
				AddError(error, entry);
			}
		}
	}
}

