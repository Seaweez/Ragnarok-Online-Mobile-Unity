using System;
using UnityEditor;
using System.Collections.Generic;

namespace EditorTool
{
	public class AtlasMakerConfigManager
	{
		class AtlasMakerConfig
		{
			public AtlasMakerConfig ()
			{
			}
		}

		public List<AtlasMakerConfigCell> configCellList = new List<AtlasMakerConfigCell>();

		const string coreDataPath = "Assets/Code/Editor/AtlasMaker/AtlasData/CoreDataManager.asset";
		static AtlasMakerConfigManager _Instance;
		public CoreDataManager dataManager = null;
		public static AtlasMakerConfigManager Instance
		{
			get
			{
				if (_Instance == null)
					_Instance = new AtlasMakerConfigManager();
				return _Instance;
			}
		}

		public AtlasMakerConfigManager ()
		{
			Init ();
		}

		public void Init()
		{
			LoadConfig ();
			ResetAllConfigCell ();
		}

		public void LoadConfig()
		{
			if(dataManager == null)
				dataManager = AssetDatabase.LoadAssetAtPath<CoreDataManager> (coreDataPath);
		}

		private void ResetAllConfigCell()
		{
			for(int i = 0; i < dataManager.atlasDataList.Count; i++)
			{
				if (i < configCellList.Count)
					configCellList [i].Reset (dataManager.atlasDataList [i]);
				else
					configCellList.Add (new AtlasMakerConfigCell (dataManager.atlasDataList [i]));
			}
			if (dataManager.atlasDataList.Count < configCellList.Count)
				configCellList.RemoveRange (dataManager.atlasDataList.Count, configCellList.Count - dataManager.atlasDataList.Count);
		}

		public void DrawAllTitle()
		{
			for(int i = 0; i < configCellList.Count; i++)
			{
				configCellList [i].DrawTitle ();
			}
		}

		public void DestroyCell(AtlasMakerConfigCell cell)
        {
            if (cell != null)
            {
                cell.data.Destroy();
                dataManager.RemoveData(cell.data);
                configCellList.Remove(cell);
            }
        }

        public AtlasMakerConfigCell AddNewConfig()
        {
            AtlasData d = new AtlasData();
            d.atlasName = "newAtlas";
            dataManager.atlasDataList.Add(d);
            AtlasMakerConfigCell cell = new AtlasMakerConfigCell(d);
            configCellList.Add(cell);
            dataManager.Save();
            return cell;
        }
	}
}

