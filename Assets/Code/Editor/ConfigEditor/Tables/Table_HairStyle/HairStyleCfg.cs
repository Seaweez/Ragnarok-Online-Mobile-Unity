using System;
using System.Collections.Generic;
using SLua;
using Ghost.Utils;
using System.IO;

namespace EditorTool
{
    public class HairStyleCfgCell : CfgCellBase<HairStyleCfg>
    {
        public HairStyleCfgCell() : base() { }
    }

    public class HairStyleCfg : CfgBase
	{
		//搜索时比对id ItemID
		static string[] _SearchFieldArr = new string[] { "id", "ItemID" };
		public override string[] searchFieldArr { get { return _SearchFieldArr; } }
		Dictionary<string, CfgEntry> _Map = new Dictionary<string, CfgEntry>(){};
		public override Dictionary<string, CfgEntry> map { get{ return _Map; } set { _Map = value; } }
		public override List<CfgEntry> data { get { return _Data; } set { _Data = value; } }
		public List<CfgEntry> _Data = new List<CfgEntry>()
		{
			new CfgEntry("id"),
            new CfgEntry("NameZh"),
            new CfgEntry("NameEn"),
            new CfgEntry("ItemID"),
			new CfgEntry("ModelDir"),
			new CfgEntry("ModelName"),
			new CfgEntry("Texture"),
			new CfgEntry("PublicAnime"),
			new CfgEntry("Icon"),
			new CfgEntry("HairFront"),
			new CfgEntry("HairBack"),
			new CfgEntry("HairAdornment"),
			new CfgEntry("DefaultColor"),
			new CfgEntry("PaintColor"),
			new CfgEntry("AvatarColor"),
			new CfgEntry("Sex"),
			new CfgEntry("IsPro"),
			new CfgEntry("OnSale")
		};

		string[] _HeadAtlasPathArr;
		string _HeadAtlasPath
		{
			get
			{
				if (_HeadAtlasPathArr == null || _HeadAtlasPathArr.Length < 1)
					CfgEditor_UIAtlasConfig.map.TryGetValue ("Head", out _HeadAtlasPathArr);
				if (_HeadAtlasPathArr.Length < 1)
					return null;
				else
					return _HeadAtlasPathArr [0];
			}
		}

		static string _ModelPath = "Assets/Art/Model/Role";
		static string _HairPath = "Assets/Resources/Role/Hair";
		private static string _ErrorFormat = "HairStyle表错误： ID = {0} , {1}";
		public override string errorFormat { get { return _ErrorFormat; } }
		private static string _WarnsFormat = "HairStyle表警告： ID = {0} , {1}";
		public override string warnsFormat { get { return _WarnsFormat; } }

		public HairStyleCfg () : base(){ }

		public override void Check()
		{
			ModelTexCheck ();
			SpriteCheck ("Icon", "hairStyle");
			HeadAtlasSpriteCheck ("HairFront");
			HeadAtlasSpriteCheck ("HairBack");
			HeadAtlasSpriteCheck ("HairAdornment");
			TryNotifyChanged();
		}

		public override void DeepCheck()
		{
			RolePartHairCheck ();
			TryNotifyChanged(false);
		}

		void ModelTexCheck()
		{
			CfgEntry texEntry = FindEntry ("Texture");
			CfgEntry dirEntry = FindEntry ("ModelDir");
			CfgEntry modelEntry = FindEntry ("ModelName");
			string srcFolder = PathUnity.Combine (_ModelPath, dirEntry.value);
            if (string.IsNullOrEmpty(dirEntry.value))
                AddWarning("ModelDir字段为空！", dirEntry);
            else if (Directory.Exists (srcFolder)) 
			{
				if (texEntry != null && !string.IsNullOrEmpty (texEntry.value))
				{
					string path = PathUnity.Combine (srcFolder, texEntry.value);
					path = Path.ChangeExtension (path, AssetChecker.materialExt);
					if(!AssetChecker.CheckAssetFile(path))
						AddError (string.Format(fileMissErrorFormat, path), texEntry);
				}
				if (modelEntry != null && !string.IsNullOrEmpty (modelEntry.value))
				{
					string err = AssetChecker.CheckAssetFileWithMultiExt (srcFolder, modelEntry.value, AssetChecker.modelExt);
					if (!string.IsNullOrEmpty(err))
						AddError (err, modelEntry);
				}
			}
			else
				AddWarning(string.Format ("{0}目录不存在", srcFolder), dirEntry);
		}

		void RolePartHairCheck()
		{
			Path.ChangeExtension (PathUnity.Combine(_HairPath, id.ToString()), AssetChecker.prefabExt);
		}

		private void HeadAtlasSpriteCheck(string spriteKey)
		{
			CfgEntry entry = FindEntry(spriteKey);
			if (!string.IsNullOrEmpty(entry.value))
			{
				bool checkResult = AssetChecker.CheckAtlasSpriteByPath(entry.value, _HeadAtlasPath);
				if (!checkResult)
				{
					string error = string.Format(AssetChecker.atlasSpriteMissErrorFormat, entry.key, entry.value, _HeadAtlasPath);
					AddError(error, entry);
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

