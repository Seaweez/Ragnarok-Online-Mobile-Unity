using System;
using System.Collections.Generic;
using SLua;
using Ghost.Utils;
using System.IO;

namespace EditorTool
{
    public class AssesoriesCfgCell : CfgCellBase<AssesoriesCfg>
    {
        public AssesoriesCfgCell() : base() { }
    }

    public class AssesoriesCfg  : CfgBase
	{
		private static string _AtlasNames = string.Empty;
		private static string _AtlasFloderPath = "Assets/Resources/GUI/atlas/preferb/";
		private static List<string> _AtlasList = null;
		private static string _ErrorFormat = "Assesories表错误： ID = {0} , {1}";
		public override string errorFormat { get { return _ErrorFormat; } }
		private static string _WarnsFormat = "Assesories表警告： ID = {0} , {1}";
		public override string warnsFormat { get { return _WarnsFormat; } }

		static string[] _SearchFieldArr = new string[] { "id" };
		public override string[] searchFieldArr { get { return _SearchFieldArr; } }

		Dictionary<string, CfgEntry> _Map = new Dictionary<string, CfgEntry>(){};
		public override Dictionary<string, CfgEntry> map { get{ return _Map; } set { _Map = value; } }

		public override List<CfgEntry> data { get { return _Data; } set { _Data = value; } }
		List<CfgEntry> _Data = new List<CfgEntry>
		{
			new CfgEntry("id"),
            new CfgEntry("NameZh"),
            new CfgEntry("Back"),
			new CfgEntry("Front")
		};

		public AssesoriesCfg () : base() { nameArr = new string[] { "NameZh" }; }

        public override void Check()
		{
			SpriteCheck ("Back");
			SpriteCheck ("Front");
			TryNotifyChanged();
			_AtlasList = null;
		}

		public override void DeepCheck()
		{
			TryNotifyChanged(false);
		}

		private void SpriteCheck(string spriteKey)
		{
			CfgEntry entry = FindEntry(spriteKey);
			if (!string.IsNullOrEmpty(entry.value))
			{
				if (_AtlasList == null)
					CollectAtlasPath ();
				bool checkResult = false;
				for (int i = 0; i < _AtlasList.Count; i++) 
				{
					checkResult = AssetChecker.CheckAtlasSpriteByPath (entry.value, _AtlasList [i]);
					if (checkResult)
						return;
				}
				if(!checkResult)
				{
					string error = string.Format (AssetChecker.atlasSpriteMissErrorFormat, entry.key, entry.value, _AtlasNames);
					AddError (error, entry);
				}
			}
		}

		private void CollectAtlasPath()
		{
			_AtlasNames = string.Empty;
			_AtlasList = new List<string> ();
			string[] _Arr = Directory.GetFiles (_AtlasFloderPath);
			if(_Arr != null)
			{
				for(int i=0; i < _Arr.Length; i++)
				{
					if (_Arr [i].EndsWith (".prefab") && (_Arr [i].Contains ("Assesories_") || _Arr [i].Contains ("Aface_"))) 
					{
						_AtlasNames += Path.GetFileNameWithoutExtension(_Arr[i]) + " ";
						_AtlasList.Add (_Arr [i].Replace (".prefab", ""));
					}
				}
			}
		}
	}
}

