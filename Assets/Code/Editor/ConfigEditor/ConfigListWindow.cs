using System;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    public class ConfigListWindow : EditorWindow
    {
        static ConfigListWindow window;
        #region ScrollView相关属性
        static float fRowHeight = 18.0f;
        static Vector2 m_ScrollPos = Vector2.zero;
        static float m_fLeftOffset = 5f;
		static float m_fRightOffset = 20f;
		static float m_fTopOffset = 20f;

        public static float fRowWidth
        {
            get
            {
                return window.position.width - 10;
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

        static int _VisibleRowNum = 0;
        public static int visibleRowNum
        {
            get
            {
                _VisibleRowNum = (int)Mathf.Ceil(window.position.height / fRowHeight);
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

		static IDraw _SelectedUI;
		public static IDraw selectedUI
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

        public ConfigListWindow()
        {
            window = this;
        }

        public static Rect GetTitleRect(int rowidx)
        {
			Rect r = new Rect(m_fLeftOffset, (rowidx - 1) * fRowHeight + m_fTopOffset, fRowWidth - m_fRightOffset, fRowHeight);
            return r;
        }

		void Init()
		{
			toolbar = InitToolbar ();
		}

		ETopToolBar toolbar;
		ESearchField _SearchField;
		private ETopToolBar InitToolbar()
		{
			ETopToolBar bar = new ETopToolBar();
			bar.InsertFlexibleSpace();
			_SearchField = new ESearchField (100, StartSearchEvt);
			bar.AddToDrawQueue(_SearchField);
			bar.InsertFlexibleSpace();
			return bar;
		}

		void StartSearchEvt(EComponentBase obj, string content)
		{
			ConfigManager.Instance.SetSearchContent (content);
		}

        void Update()
        {
            Repaint();
        }

        void OnGUI()
		{
			toolbar.Draw();
            if (ConfigEditorWindow.Instance.selectedUI != null)
                DrawContent();
		}

		void OnDisable()
		{
			window = null;
			selectedUI = null;
			m_ScrollPos = Vector3.zero;
		}

		void OnEnable()
		{
			ConfigEditorWindow.Instance.AddListener(OnKeyDownEvt);
			Init ();
		}

        //绘制左侧列表
        private void DrawContent()
        {
            SetAllRowNum();
            m_ScrollPos = GUI.BeginScrollView(contentRect, m_ScrollPos, viewRect);
            ConfigEditorWindow.Instance.selectedUI.DrawDetail();
            GUI.EndScrollView(true);
			UpdateInput ();
        }

        void UpdateInput()
        {
            if (Event.current.isKey && Event.current.type == EventType.KeyDown)
                OnKeyDownEvt(Event.current.keyCode);
        }

        private int m_nTotalRow;
        //设置行号的总入口
        int SetAllRowNum()
        {
            m_nTotalRow = 0;
            m_nTotalRow = ConfigEditorWindow.Instance.selectedUI.SetChildRowNum(m_nTotalRow);
            return m_nTotalRow;
        }

		void OnKeyDownEvt(KeyCode code)
		{
			if(EditorWindow.focusedWindow != this)
				return;
			switch(code)
            {
				case KeyCode.UpArrow:
					MoveSelected (-1);
                    break;
			case KeyCode.DownArrow:
					MoveSelected (1);
                    break;
                case KeyCode.LeftArrow:
                    _SelectedUI = null;
                    ConfigEditorWindow.Instance.Focus();
                    break;
            }
		}

		void MoveSelected(int step)
		{
            if (m_nTotalRow > 0)
            {
				int selectedIdx = _SelectedUI != null ? _SelectedUI.rownum : 0;
                selectedIdx += step;
                selectedIdx = selectedIdx < 1 ? 1 : selectedIdx;
                selectedIdx = selectedIdx < m_nTotalRow ? selectedIdx : m_nTotalRow;
                if (selectedIdx <= firstRowIdx) m_ScrollPos.y = fRowHeight * (selectedIdx - 1);
                int _FloorVisibleRowNum = Mathf.FloorToInt(window.position.height / fRowHeight);
                if (selectedIdx > firstRowIdx + _FloorVisibleRowNum - 2) m_ScrollPos.y = (selectedIdx - _FloorVisibleRowNum + 2) * fRowHeight;
				_SelectedUI = ConfigEditorWindow.Instance.selectedUI.GetChild(selectedIdx);
            }
		}

        public static void FocusListWindow()
        {
            if (window != null)
                window.MoveSelected(1);
			if(_SelectedUI != null)
                EditorWindow.FocusWindowIfItsOpen<ConfigListWindow>();
        }
    }
}
