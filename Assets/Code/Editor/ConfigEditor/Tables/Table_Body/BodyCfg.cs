using RO;
using SLua;
using System.IO;
using Ghost.Utils;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace EditorTool
{
    public class BodyCfgCell : CfgCellBase<BodyCfg>
    {
        public BodyCfgCell() : base() { }
    }

    public class BodyCfg : CfgBase
	{
		/**Table_Body[1]数据示例
         [1] = {id = 1, Texture = 'Novice_M', AvatarBody = 'Body_Novice_M', ShowWeaponType = 1, DefaultColor = 1, PaintColor = {0x0ef15a66,0xf10e0e66,0x3e4c5266,0xdfe1c966,0x930ef166,0xffe50066,0x1452c366,0x873a1466,0x00000000}},
         **/ 
		public static int titleIndex = 0;
		public static int[] shortKeyIdx = new int[] { 2 };
		public static int[] detailKeyIdx = new int[] { 1, 3, 4, 5 };
		private static string _ErrorFormat = "Body表错误： ID = {0} , {1}";
		public override string errorFormat { get { return _ErrorFormat; } }
		private static string _WarnsFormat = "Body表警告： ID = {0} , {1}";
		public override string warnsFormat { get { return _WarnsFormat; } }
		private static string _EntryMissErrorFormat = "\"{0}\"字段缺失！";
		private static string _EntryNullValueErrorFormat = "\"{0}\"字段为空！";
		private static string _BodyPartPath = "Assets/Resources/Role/Body/{0}.prefab";
		private static string _AvatarBodyAtlasPath = "Assets/Art/Public/Texture/GUI/New/Atlas/Head/Head.prefab";
		private static string _MissMainRendererWarning = "Texture为空，且Body上没有MainRenderer!";

		private static Dictionary<string, string> _BindMap = new Dictionary<string, string>()
		{

		};

        private string _SrcFolder = string.Empty;
        private string srcFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_SrcFolder))
                    _SrcFolder = PathUnity.Combine(AssetChecker.rolePath, GetValueBykey("ModelDir"));
                return _SrcFolder;
            }
        }

		static string[] _SearchFieldArr = new string[] { "id", "Texture" };
		public override string[] searchFieldArr { get { return _SearchFieldArr; } }
		Dictionary<string, CfgEntry> _Map = new Dictionary<string, CfgEntry>(){};
		public override Dictionary<string, CfgEntry> map { get{ return _Map; } set { _Map = value; } }
		public override List<CfgEntry> data { get { return _Data; } set { _Data = value; } }
		List<CfgEntry> _Data = new List<CfgEntry>()
		{
			new CfgEntry("id"),
            new CfgEntry("NameZh"),
            new CfgEntry("NameEn"),
            new CfgEntry("Texture"),
			new CfgEntry("AvatarBody"),
			new CfgEntry("ShowWeaponType"),
			new CfgEntry("DefaultColor"),
			new CfgEntry("PaintColor"),
			new CfgEntry("ModelDir"),
			new CfgEntry("ModelName"),
			new CfgEntry("EPs"),
			new CfgEntry("CPs"),
			new CfgEntry("AnimationClip")
		};

		public BodyCfg() : base() { }

		public override void Check()
		{
			ModelTexCheck ();
			SpriteCheck("AvatarBody", _AvatarBodyAtlasPath);
			BodyPartCheck (false);
			TryNotifyChanged();
		}

		public override void DeepCheck()
		{
			BodyPartCheck (true);
			AddEPCPInfo ();
			TryNotifyChanged(false);
		}

		void AddEPCPInfo()
		{
			CfgEntry ep = FindEntry ("EPs");
			CfgEntry cp = FindEntry ("CPs");
			CfgEntry anim = FindEntry ("AnimationClip");
			CfgEntry entry = FindEntry("id");
			if (string.IsNullOrEmpty(entry.value) || entry.value == "0")
				return;

			string path = string.Format(_BodyPartPath, id);
			GameObject go = AssetChecker.TryLoadAsset<GameObject>(path);
			RolePartBody body = null;
			if (go != null)
				body = go.GetComponent<RolePartBody> ();
			if (body != null)
			{
				string str = "\n";
				if(body.eps.Length > 0)
				{
					for(int i = 1; i < body.eps.Length; i++)
					{
						if(body.eps[i] != null)
							str += body.eps[i].name + "\n";
						else
							str += string.Format("EP_{0} Miss\n", i);
					}
					ep.value = str;
					ep.SetShowString ();
				}
				else
					AddWarning("EP点不存在！", ep);

				str = "\n";
				for(int i = 1; i < body.cps.Length; i++)
				{
					if(body.cps[i] != null)
						str += body.cps[i].name + "\n";
					else
						str += string.Format("CP_{0} Miss\n", i);
				}
				cp.value = str;
				cp.SetShowString ();

				str = "\n";
				for(int i = 0; i < body.animators.Length; i++)
				{
					RuntimeAnimatorController controller = body.animators [i].runtimeAnimatorController;
					if (null != controller)
					{
						for(int k = 0; k < controller.animationClips.Length; k++)
						{
							if (controller.animationClips [k] != null)
								str += controller.animationClips[k].name + "\n";
							else
								str += "空状态\n";
						}
					}
					else
					{
						str += "动画控制器为空\n";
					}
				}
				anim.value = str;
				anim.SetShowString ();
			}
		}

		void ModelTexCheck()
		{
			CfgEntry texEntry = FindEntry ("Texture");
			CfgEntry dirEntry = FindEntry ("ModelDir");
			CfgEntry modelEntry = FindEntry ("ModelName");
            if (string.IsNullOrEmpty(dirEntry.value))
                AddWarning("ModelDir字段为空！", dirEntry);
            else if (Directory.Exists (srcFolder)) 
			{
				bool m_bHasMR = HasMainRenderer ();
				if (!string.IsNullOrEmpty (texEntry.value)) 
				{
					string path = PathUnity.Combine (srcFolder, texEntry.value);
					path = Path.ChangeExtension (path, AssetChecker.materialExt);
					PathUnity.Combine (srcFolder, modelEntry.value);
					if (!AssetChecker.CheckAssetFile (path))
						AddError (string.Format (AssetChecker.fileMissErrorFormat, path), texEntry);
				} 
				else if (!m_bHasMR)
					AddWarning (_MissMainRendererWarning, texEntry);
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

		private void BodyPartCheck(bool isDeepCheck = false)
		{
			CfgEntry dirEntry = FindEntry ("ModelDir");
			if (!string.IsNullOrEmpty(dirEntry.value) && Directory.Exists (srcFolder)) 
			{
				//有材质球贴图目录才有prefab
				CfgEntry entry = FindEntry("id");
				if (string.IsNullOrEmpty(entry.value) || entry.value == "0")
					return;

				string path = string.Format(_BodyPartPath, id);
				CheckFileExist(entry, path);
				if (isDeepCheck)
					DeepCheckScript(entry, path);
			}
		}

		private void CheckFileExist(CfgEntry entry, string path)
		{
			if (!AssetChecker.CheckAssetFile(path))
			{
				string error = string.Format(fileMissErrorFormat, string.Format(_BodyPartPath, entry.value));
				AddError(error, entry);
			}
		}

		private bool HasMainRenderer()
		{
			CfgEntry dirEntry = FindEntry ("ModelDir");
			CfgEntry modelEntry = FindEntry ("ModelName");
			if (modelEntry != null && !string.IsNullOrEmpty (modelEntry.value)) 
			{
				RolePartBody body = Resources.Load<RolePartBody> (PathUnity.Combine(dirEntry.value, modelEntry.value));
				if (body != null)
					return body.mainMR != null || body.mainSMR != null;
				else
					return false;
			}
			return false;
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
				string error = string.Format(fileMissErrorFormat, string.Format(_BodyPartPath, entry.value));
				AddError(error, entry);
			}
		}

		//need optimize checkatlas is exist
		private void SpriteCheck(string spriteKey, string atlasPath)
		{
			CfgEntry entry = FindEntry(spriteKey);
			if (!string.IsNullOrEmpty(entry.value))
			{
				UIAtlas atlas = AssetChecker.TryLoadAsset<UIAtlas>(atlasPath);
				if (atlas != null) 
				{
					bool checkResult = AssetChecker.CheckAtlasSprite(atlas, entry.value);
					if (!checkResult)
					{
						string error = string.Format(AssetChecker.atlasSpriteMissErrorFormat, spriteKey, entry.value, _AvatarBodyAtlasPath);
						AddError (error, entry);
					}
				}
				else
				{
					string error = string.Format (fileMissErrorFormat, atlasPath);
					AddError (error, entry);
				}
			}
		}

		private void AssetCheck(string key, string extension)
		{
			CfgEntry entry = FindEntry(key);
			string error = string.Empty;
			if (!entry.bIsInited)
			{
				error = string.Format(_EntryMissErrorFormat, key);
			}
			else if (string.IsNullOrEmpty(entry.value))
			{
				error = string.Format(_EntryNullValueErrorFormat, key);
			}
			else
            {
                string assetpath = Path.ChangeExtension(PathUnity.Combine(srcFolder, entry.value), extension);
				if (!File.Exists(assetpath))
					error = string.Format(fileMissErrorFormat, assetpath);
			}

			if (!string.IsNullOrEmpty(error))
			{
				AddError (error, entry);
				//Debug.LogError(error);
			}
		}

		public string GetValueBykey(string key)
		{
			if (map.ContainsKey(key))
				return map [key].value;
			return string.Empty;
		}

		public override void InitBind()
		{
			foreach (var item in _BindMap)
			{
				CfgEntry entry = FindEntry(item.Key);
				ITableBase table = ConfigManager.Instance.GetTable(item.Value);
				if (!string.IsNullOrEmpty(entry.value))
				{
					CfgBase cfg = table.FindCfgByKey(entry.value);
					if (cfg != null)
						cfg.AddObserver(entry);
					else
						Debug.LogError("cfg is null !");
				}
			}
		}
	}
}

