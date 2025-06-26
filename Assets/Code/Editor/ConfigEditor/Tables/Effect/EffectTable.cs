using System.IO;
using System.Collections.Generic;

namespace EditorTool
{
	/// <summary>
	/// 虚拟的Table，Effect的虚拟集合，物理上并不存在此配置表。
	/// </summary>
	public class EffectTable : TableBase<EffectCfgCell, EffectCfg>
	{
		static string _PfbExt = ".prefab";
		static List<string> _EffectFolder = new List<string>
		{
			"Assets/Resources/public/Effect"
		};
		static List<string> _FileList = new List<string>();
		public EffectTable (string name) : base(name) { }

		public override void InitData()
		{
			_FileList.Clear();
			_FileList = CollectEffectFile ();
			InitCfgList();
		}

		List<string> CollectEffectFile()
		{
			List<string> fs = ConfigEditorUtil.CollectAllFolders(_EffectFolder, _PfbExt);
			return fs;
		}

		public override void InitCfgList()
		{
            cfgList.Clear();
            for (int i=0; i < _FileList.Count; i++)
			{
				EffectCfg cfg = new EffectCfg ();
				cfg.SpecialInit(i, Path.GetFileNameWithoutExtension(_FileList[i]), _FileList[i]);
				cfgList.Add (cfg);
			}
		}
	}
}

