using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace EditorTool
{
	public class AtlasMakerWindow : EditorWindow
	{
		static Vector2 m_ScrollPos = Vector2.zero;
		static AtlasInspectorWindow _InspectorWindow = null;

		public static AtlasMakerWindow window = null;
		public AtlasMakerWindow ()
		{
			window = this;
		}

		[MenuItem("NGUI/图集打包窗口")]
		public static void Open()
		{
			ShowWindow ();
		}

		static void ShowWindow()
		{
			window = GetWindow<AtlasMakerWindow> ();
			window.titleContent.text = "图集列表";
			window.position.size.Set (1000f, 800f);
		}

		void Init()
		{
			InitDocker ();
			InitAltasConfig ();
		}

		void OnGUI()
		{
			m_ScrollPos = EditorGUILayout.BeginScrollView (m_ScrollPos);
			AtlasMakerConfigManager.Instance.DrawAllTitle ();
            EditorGUILayout.EndScrollView ();
			if (GUILayout.Button("添加新配置", EditorStyles.toolbarButton))
                AddNewConfig();
        }

		private void InitDocker()
		{
			if(_InspectorWindow == null)
			{
				_InspectorWindow = GetWindow<AtlasInspectorWindow> ("打包配置");
				Rect r = new Rect (0f, 0f, 200f, position.height);
				EDocker.Dock (window, _InspectorWindow, r, 1);
			}
		}

		private void InitAltasConfig()
		{
//			List<AtlasData> dataList = AtlasMakerConfigManager.Instance.dataManager.atlasDataList;
//			List<AtlasCoreData> list = AtlasMakerConfigManager.Instance.dataManager.atlasCoreDataList;
//			dataList.Clear ();
//			for(int i = 0; i < list.Count; i++)
//			{
//				AtlasData data = new AtlasData ();
//				data.tempEntry = list [i].tempEntry;
//				data.resourcePathList = new List<string> ();
//				data.resourcePathList.Add(list [i].resourcePath);
//				data.atlasName = list [i].atlasName;
//				data.iconWidth = list [i].iconWidth;
//				data.iconHeight = list [i].iconHeight;
//				data.atlasPath = list [i].atlasPath;
//				data.targetPfbPath = list [i].entryPfbPath;
//				data.targetMatPath = list [i].entryMatPath;
//				data.targetTexPath = list [i].entryTexPath;
//				data.iosFormat = list [i].iosFormat;
//				data.androidFormat = list [i].androidFormat;
//				data.maxSize = list [i].maxSize;
//				data.forceSquare = list [i].forceSquare;
//				dataList.Add (data);
//			}
//			EditorUtility.SetDirty (AtlasMakerConfigManager.Instance.dataManager);
		}

        private void AddNewConfig()
        {
            AtlasMakerConfigCell cell = AtlasMakerConfigManager.Instance.AddNewConfig();
            AtlasInspectorWindow.selected.cell = cell;
        }

        void Update()
		{
			Repaint ();
		}

		void OnDisable()
		{
			window = null;
		}

		void OnEnable()
		{
			ShowWindow ();
			Init ();
		}
	}
}

