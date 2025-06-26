using System;
using System.Collections.Generic;
using SLua;
using Ghost.Utils;
using System.IO;

namespace EditorTool
{
    public class EquipCfgCell : CfgCellBase<EquipCfg>
    {
        public EquipCfgCell() : base() { }
    }

    public class EquipCfg : CfgBase 
	{
		static string _AssesoriesType = "8";
		static string _ModelPath = "Assets/Art/Model/Role";
		private static string _ErrorFormat = "Equip表错误： ID = {0} , {1}";
		public override string errorFormat { get { return _ErrorFormat; } }
		private static string _WarnsFormat = "Equip表警告： ID = {0} , {1}";
		public override string warnsFormat { get { return _WarnsFormat; } }
		//搜索时比对id Name
		static string[] _SearchFieldArr = new string[] { "id", "NameZh", "NameEn" };
		public override string[] searchFieldArr { get { return _SearchFieldArr; } }
		Dictionary<string, CfgEntry> _Map = new Dictionary<string, CfgEntry>(){};
		public override Dictionary<string, CfgEntry> map { get{ return _Map; } set { _Map = value; } }
		public override List<CfgEntry> data { get { return _Data; } set { _Data = value; } }
		List<CfgEntry> _Data = new List<CfgEntry>()
		{
			new CfgEntry("id"),
            new CfgEntry("NameZh"),
            new CfgEntry("NameEn"),
            new CfgEntry("VID"),
			new CfgEntry("HairID"),
			new CfgEntry("CanEquip"),
			new CfgEntry("Type"),
			new CfgEntry("EquipType"),
			new CfgEntry("CardSlot"),
			new CfgEntry("SuitID"),
			new CfgEntry("SuitRefineAttr"),
			new CfgEntry("Effect"),
			new CfgEntry("EffectAdd"),
			new CfgEntry("UniqueEffect"),
			new CfgEntry("RefineEffect"),
			new CfgEntry("ForbidFuncBit"),
			new CfgEntry("display"),
			new CfgEntry("ModelDir"),
			new CfgEntry("Model"),
			new CfgEntry("Texture"),
			new CfgEntry("Desc"),
			new CfgEntry("SE_attack"),
			new CfgEntry("SE_fire"),
			new CfgEntry("SE_hit"),
			new CfgEntry("SubstituteID"),
			new CfgEntry("DecomposeID"),
			new CfgEntry("DecomposeNum"),
		};

		public EquipCfg () : base() { }

		public override void Check()
		{
			TypeCheck ();
			ModelTexCheck ();
			AnimationCheck ();
			AudioCheck ("SE_attack");
			AudioCheck ("SE_fire");
			AudioCheck ("SE_hit");
			EquipTypeCheck();
			TryNotifyChanged();
		}

		public override void DeepCheck()
		{
			TryNotifyChanged(false);
		}

		void TypeCheck()
		{
			CfgEntry typeEntry = FindEntry ("Type");
			if (string.IsNullOrEmpty (typeEntry.value))
				AddWarning ("Type值为空！", typeEntry);
		}

		void ModelTexCheck()
		{
			CfgEntry texEntry = FindEntry ("Texture");
			CfgEntry dirEntry = FindEntry ("ModelDir");
			CfgEntry modelEntry = FindEntry ("Model");
			string srcFolder = PathUnity.Combine (_ModelPath, dirEntry.value);
            if (string.IsNullOrEmpty(dirEntry.value))
                AddWarning("ModelDir字段为空！", dirEntry);
            else if (Directory.Exists (srcFolder)) 
			{
				if (!string.IsNullOrEmpty (texEntry.value))
				{
					string path = PathUnity.Combine (srcFolder, texEntry.value);
					path = Path.ChangeExtension (path, AssetChecker.materialExt);
					if(!AssetChecker.CheckAssetFile(path))
						AddError (string.Format(AssetChecker.fileMissErrorFormat, path), texEntry);
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

		void AnimationCheck()
		{
			CfgEntry dirEntry = FindEntry ("ModelDir");
            if (string.IsNullOrEmpty(dirEntry.value)) return;
			string srcFolder = PathUnity.Combine (_ModelPath, dirEntry.value);
			CfgEntry entry = FindEntry ("Animation");
			if (entry != null && !string.IsNullOrEmpty (entry.value))
			{
				string path = PathUnity.Combine (srcFolder, entry.value);
				path = Path.ChangeExtension (path, AssetChecker.aniExt);
				if(!AssetChecker.CheckAssetFile(path))
					AddError (string.Format(AssetChecker.fileMissErrorFormat, path), entry);
			}
		}

		private void AudioCheck(string entryKey)
		{
			CfgEntry entry = FindEntry(entryKey);
            string error = string.Empty;
            ScriptChecker.CheckAudioByCfg(ref error, entry.value);
            if(!string.IsNullOrEmpty(error))
			    AddError(error, entry);
		}

		private void EquipTypeCheck()
		{
			CfgEntry entry = FindEntry("EquipType");
			if(entry.value == _AssesoriesType)
			{
				ITableBase table = ConfigManager.Instance.GetTable ("Table_Assesories");
				CfgBase cfg = table.FindCfgByKey(id);
				if(cfg == null)
					AddWarning(string.Format("id = {0} 的头饰在Table_Assesories中未找到", id), entry);
			}
		}
	}
}

