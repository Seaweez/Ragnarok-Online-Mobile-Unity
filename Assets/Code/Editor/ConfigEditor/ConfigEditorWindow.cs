using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using System.IO;
using System;

namespace EditorTool
{
    public class ConfigEditorWindow : EditorWindow
    {
        public Texture _ErrorIcon;
        public Texture _WarnIcon;
        ETopToolBar toolbar;
		EToggle _ErrorFilterToggle;
		EToggle _WarnFilterToggle;
		bool m_bInited = false;
		private int m_nTotalRow;
		ConfigListWindow _clWindow = null;
		ConfigInspectorWindow _ciWindow = null;
		static Vector2 _WindowSize = new Vector2 (1100f, 800f);

		private static ConfigEditorWindow m_Instance = null;
		public static ConfigEditorWindow Instance
		{
			get
			{
				if (m_Instance == null)
				{
					m_Instance = FindFirstInstance();
					if (m_Instance == null)
                    {
                        m_Instance = GetWindow<ConfigEditorWindow>();
                        m_Instance._ErrorIcon = EditorGUIUtility.FindTexture("console.erroricon.sml");
                        m_Instance._WarnIcon = EditorGUIUtility.FindTexture("console.warnicon.sml");
                    }
						
				}
				return m_Instance;
			}
		}

        IDraw _SelectedUI;
        public IDraw selectedUI
        {
            get
            {
                return _SelectedUI;
            }
            set
            {
                if (_SelectedUI != value)
                    _SelectedUI = value;
            }
        }

		public delegate void KeyDownEvent(KeyCode code);
		public KeyDownEvent keyDownEvt;

        #region ScrollView相关属性
        static float fRowHeight = 18.0f;
        static Vector2 m_ScrollPos = Vector2.zero;
        static float m_fLeftOffset = 5f;
        static float m_fTopOffset = 20f;

        public static float fRowWidth
        {
            get
            {
				return Instance.position.width - 10;
            }
        }

        Rect _ContentRect = new Rect(m_fLeftOffset, m_fTopOffset, 0, 0);
        Rect contentRect
        {
            get
            {
                _ContentRect.width = position.width - m_fLeftOffset;
                _ContentRect.height = position.height - m_fTopOffset;
                return _ContentRect;
            }
        }
        Rect _ViewRect = new Rect(m_fLeftOffset, m_fTopOffset, 0, 0);
        Rect viewRect
        {
            get
            {
                _ViewRect.width = position.width;
                _ViewRect.height = m_nTotalRow * fRowHeight;
                return _ViewRect;
            }
        }

        int _VisibleRowNum = 0;
        public int visibleRowNum
        {
            get
            {
                _VisibleRowNum = (int)Mathf.Ceil(position.height / fRowHeight);
                return _VisibleRowNum;
            }
        }

        static int _FirstRowIdx = 0;
        public static int firstRowIdx
        {
            get
            {
                _FirstRowIdx = (int)Mathf.Floor(m_ScrollPos.y / fRowHeight);
                return _FirstRowIdx;
            }
        }
        #endregion

		public ConfigEditorWindow() { }

        [MenuItem("Window/配置编辑器/配置编辑器窗口")]
        public static void OpenWindow()
        {
			Instance.Show (true);
        }

		void ShowWindow()
		{
			Instance.titleContent.text = "Table表";
//			Instance.OpenScene (_ScenePath);
			Rect r = Instance.position;
			r.size = _WindowSize;
			Instance.position = r;
		}

        void Init()
        {
			ShowWindow ();
			toolbar = InitToolbar();
			InitDocker ();
            EditorGUIUtility.FindTexture("TextAsset Icon");
			m_bInited = true;
        }

        void Update()
        {
			if (!m_bInited)
				Init ();
            Repaint();
        }

        void OnGUI()
		{
			if (!m_bInited)
				return;
			UpdateInput ();
            toolbar.Draw();
            DrawContent();
        }

		/**
		 -------------------------------------------------
		|           |             |                       |
		|           |             |                       |
		|           |             |                       |
		|           |             |                       |
		|           |             |                       |
		|           |             |                       |
		|           |             |                       |
		|--- 250 ---|---- 300 ----|--------- 550 ---------|
		|           |             |                       |
		|           |             |                       |
		|           |             |                       |
		|           |             |                       |
		|           |             |                       |
		|           |             |                       |
		|           |             |                       |
		 -------------------------------------------------
		 250 = position.width - 300 - 550
		**/
        private void InitDocker()
        {
			if (_clWindow == null) 
			{
				_clWindow = GetWindow<ConfigListWindow> ("配置列表");
				Rect rect = new Rect (0f, 0f, 300f, position.height);
				EDocker.Dock(Instance, _clWindow, rect, 1);
			}
			if(_ciWindow == null)
			{
				_ciWindow = GetWindow<ConfigInspectorWindow>("详情");
				Rect rect = new Rect (0f, 0f, 550f, position.height);
				EDocker.Dock(Instance, _ciWindow, rect, 2);
			}

        }

        public static Rect GetTitleRect(int rowidx)
        {
            Rect r = new Rect(m_fLeftOffset, (rowidx - 1) * fRowHeight + m_fTopOffset, fRowWidth, fRowHeight);
            return r;
        }

		public static ConfigEditorWindow FindFirstInstance()
		{
			var windows = (ConfigEditorWindow[])Resources.FindObjectsOfTypeAll(typeof(ConfigEditorWindow));
			if (windows.Length == 0)
				return null;
			return windows[0];
		}

        //绘制左侧列表
        private void DrawContent()
        {
            SetAllRowNum();
			m_ScrollPos = GUI.BeginScrollView(contentRect, m_ScrollPos, viewRect, false, true);
            for(int i=0; i<ConfigManager.Instance.configList.Count; i++)
            {
                ITableBase table = ConfigManager.Instance.configList[i];
                if (table != null)
                    table.DrawTitle(firstRowIdx, visibleRowNum);
            }
            GUI.EndScrollView(true);
        }

        //设置行号的总入口
        int SetAllRowNum()
        {
            m_nTotalRow = 0;
			for(int i=0; i<ConfigManager.Instance.configList.Count; i++)
			{
				ITableBase table = ConfigManager.Instance.configList[i];
                if (table != null)
                    m_nTotalRow = table.SetRowNum(m_nTotalRow);
            }
            return m_nTotalRow;
        }

		void OnEnable()
		{
			AddListener (OnKeyDownEvt);
		}

		void OnDisable()
		{
			m_bInited = false;
			m_Instance = null;
			RemoveListener (OnKeyDownEvt);
			ConfigManager.Instance.Clear ();
			Resources.UnloadUnusedAssets ();
			GC.Collect ();
		}

        #region toolbar
        private ETopToolBar InitToolbar()
		{
			_WarnFilterToggle = new EToggle("0", _WarnIcon, OnFilterWarnEvt);
			_ErrorFilterToggle = new EToggle("0", _ErrorIcon, OnFilterErrorEvt);
            ETopToolBar bar = new ETopToolBar();
            bar.AddToDrawQueue(new EButton("加载配置", OnLoadBtn));
            bar.AddToDrawQueue(new EButton("深度检查", OnDeepCheckBtn));
            bar.InsertFlexibleSpace();
			bar.AddToDrawQueue(_WarnFilterToggle);
            bar.AddToDrawQueue(_ErrorFilterToggle);
			ConfigManager.Instance.ClearFilters ();
			RefreshToggleState ();
            return bar;
        }

        void OnLoadBtn(EComponentBase btn, bool IsPress)
        {
            if (IsPress)
            {
                ConfigManager.Instance.InitCfgManager();
				RefreshErrorWarnLable();
				AssetChecker.ClearAssetCache ();
            }
        }

        void OnDeepCheckBtn(EComponentBase btn, bool isPress)
        {
            if (isPress)
                ConfigManager.Instance.DeepCheckAllTable();
			RefreshErrorWarnLable();
        }

		void RefreshToggleState()
		{
			_WarnFilterToggle.SetToggleState ((ConfigManager.Instance.currentFilter & ConfigFilterType.Warning) > 0);
			_ErrorFilterToggle.SetToggleState ((ConfigManager.Instance.currentFilter & ConfigFilterType.Error) > 0);
		}

        void RefreshErrorWarnLable()
        {
			int errornum = 0, warnnum = 0;
			ConfigManager.Instance.StatisticErrorNum(ref errornum, ref warnnum);
			_ErrorFilterToggle.SetContent(errornum.ToString());
			_WarnFilterToggle.SetContent(warnnum.ToString());
        }

        void OnFilterErrorEvt(EComponentBase obj, bool isFilter)
		{
			if (isFilter)
				ConfigManager.Instance.AddFilter (ConfigFilterType.Error);
			else
				ConfigManager.Instance.DeleteFilter(ConfigFilterType.Error);
			ConfigManager.Instance.OnFilter ();
		}

		void OnFilterWarnEvt(EComponentBase obj, bool isFilter)
		{
			if (isFilter)
				ConfigManager.Instance.AddFilter (ConfigFilterType.Warning);
			else
				ConfigManager.Instance.DeleteFilter(ConfigFilterType.Warning);
			ConfigManager.Instance.OnFilter ();
		}

        #endregion

		void UpdateInput()
		{
			if(Event.current.isKey && Event.current.type == EventType.KeyDown)
				keyDownEvt(Event.current.keyCode);
		}

		public void AddListener(KeyDownEvent evt)
		{
			keyDownEvt += evt;
		}

		public void RemoveListener(KeyDownEvent evt)
		{
			keyDownEvt -= evt;
		}

		void OnKeyDownEvt(KeyCode code)
		{
			if(EditorWindow.focusedWindow != this)
				return;
			switch(code)
            {
                case KeyCode.UpArrow:
					MoveSelected(-1);
                    break;
                case KeyCode.DownArrow:
					MoveSelected(1);
                    break;
                case KeyCode.RightArrow:
				case KeyCode.Return:
				case KeyCode.KeypadEnter:
                    ConfigListWindow.FocusListWindow();
					break;
            }
		}

		void MoveSelected(int step)
		{
            int selectedIdx = _SelectedUI != null ? _SelectedUI.rownum - 1 : -1;
            selectedIdx += step;
			selectedIdx = selectedIdx < 0 ? 0 : selectedIdx;
			selectedIdx = selectedIdx < ConfigManager.Instance.configList.Count ? selectedIdx : ConfigManager.Instance.configList.Count - 1;
            if (selectedIdx <= firstRowIdx) m_ScrollPos.y = fRowHeight * (selectedIdx - 1);
            int _FloorVisibleRowNum = Mathf.FloorToInt(Instance.position.height / fRowHeight);
            if (selectedIdx > firstRowIdx + _FloorVisibleRowNum - 2) m_ScrollPos.y = (selectedIdx - _FloorVisibleRowNum + 2) * fRowHeight;
            _SelectedUI = ConfigManager.Instance.configList[selectedIdx].tableIDraw;
		}
    }
}