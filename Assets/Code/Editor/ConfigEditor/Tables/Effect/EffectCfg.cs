using System.Collections.Generic;
using SLua;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    public class EffectCfgCell : CfgCellBase<EffectCfg>
    {
        public EffectCfgCell() : base() { }
    }

    public class EffectCfg : CfgBase
	{
		private static string _ErrorFormat = "Effect表错误： ID = {0} , {1}";
		public override string errorFormat { get { return _ErrorFormat; } }
		private static string _WarnsFormat = "Effect表警告： ID = {0} , {1}";
		public override string warnsFormat { get { return _WarnsFormat; } }
		//搜索时比对id Name
		static string[] _SearchFieldArr = new string[] { "Name", "Path" };
		public override string[] searchFieldArr { get { return _SearchFieldArr; } }
		Dictionary<string, CfgEntry> _Map = new Dictionary<string, CfgEntry>(){};
		public override Dictionary<string, CfgEntry> map { get{ return _Map; } set { _Map = value; } }
		public override List<CfgEntry> data { get { return _Data; } set { _Data = value; } }
		List<CfgEntry> _Data = new List<CfgEntry>()
		{
			new CfgEntry("Name"),
			new CfgEntry("Path")
		};

		public EffectCfg () : base() { }

		public override void Init(int k, LuaTable table)
		{

		}

		public void SpecialInit(int k, string name, string path)
		{
			key = k;
			id = name;
            data[0].value = name;
            data[1].value = path;
			for(int i=0; i < data.Count; i++)
			{
				data [i].parent = this;
				data [i].SetShowString ();
				map.Add (data [i].key, data [i]);
			}
			FindEntry ("Path").SetCallback (OnClickPath);
		}

		public override void Check()
		{
			TryNotifyChanged();
		}

		public override void DeepCheck()
		{
			CheckEffectHandle ();
			TryNotifyChanged(false);
		}

		private void CheckEffectHandle()
		{
			string e = string.Empty;
			CfgEntry entry = FindEntry ("Path");
			ScriptChecker.CheckEffecthandle (ref e, entry.value);
			if (!string.IsNullOrEmpty (e))
				AddError (e, entry);
		}

		void OnClickPath(EComponentBase c)
		{
			Object obj = AssetDatabase.LoadMainAssetAtPath(FindEntry ("Path").value);
			Selection.activeObject = obj;
			EditorGUIUtility.PingObject(obj);
		}
	}
}

