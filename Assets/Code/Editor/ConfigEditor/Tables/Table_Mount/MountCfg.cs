using System;
using System.Collections.Generic;
using SLua;
using UnityEngine;

namespace EditorTool
{
    public class MountCfgCell : CfgCellBase<MountCfg>
    {
        public MountCfgCell() : base() { }
    }

    public class MountCfg : CfgBase
	{
		//搜索时只比对id
		static string[] _SearchFieldArr = new string[] { "id" };
		public override string[] searchFieldArr { get { return _SearchFieldArr; } }
		Dictionary<string, CfgEntry> _Map = new Dictionary<string, CfgEntry>(){};
		public override Dictionary<string, CfgEntry> map { get{ return _Map; } set { _Map = value; } }
		public override List<CfgEntry> data { get { return _Data; } set { _Data = value; } }
		public List<CfgEntry> _Data = new List<CfgEntry>()
		{
			new CfgEntry("id"),
            new CfgEntry("NameZh"),
            new CfgEntry("NameEn")
        };

		private static string _MountPartPath = "Assets/Resources/Role/Mount/{0}.prefab";
		private static string _ErrorFormat = "Mount表错误： ID = {0} , {1}";
		public override string errorFormat { get { return _ErrorFormat; } }
		private static string _WarnsFormat = "Mount表警告： ID = {0} , {1}";
		public override string warnsFormat { get { return _WarnsFormat; } }

		public MountCfg () : base(){ }

		public override void Check()
		{
			MountCheck();
			TryNotifyChanged();
		}

		public override void DeepCheck()
		{
			MountCheck(true);
			TryNotifyChanged(false);
		}

		private void MountCheck(bool isDeepCheck = false)
		{
			CfgEntry entry = FindEntry("id");
			if (string.IsNullOrEmpty(entry.value) || entry.value == "0")
				return;

			string path = string.Format(_MountPartPath, id);
			CheckFileExist(entry, path);
			if (isDeepCheck)
				DeepCheckScript(entry, path);
		}

		private void CheckFileExist(CfgEntry entry, string path)
		{
			if (!AssetChecker.CheckAssetFile(path))
			{
				string error = string.Format(fileMissErrorFormat, string.Format(_MountPartPath, entry.value));
				AddError(error, entry);
			}
		}

		private void DeepCheckScript(CfgEntry entry, string path)
		{
			GameObject go = AssetChecker.TryLoadAsset<GameObject>(path);
			if (go != null)
			{
				string error = string.Empty;
				ScriptChecker.CheckRolePart(go, ref error);
				if (!string.IsNullOrEmpty(error))
					AddError(error, entry);
			}
			else
			{
				string error = string.Format(fileMissErrorFormat, string.Format(_MountPartPath, entry.value));
				AddError(error, entry);
			}
		}
	}
}

