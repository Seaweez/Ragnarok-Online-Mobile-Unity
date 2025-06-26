using System;
using UnityEngine;

namespace EditorTool
{
    public class ESearchField : EComponentBase
    {
        public string id;
        public delegate void StartSearchEvent(EComponentBase component, string content);
        private int width;
        private string content = string.Empty;
        private EButton cancelBtn;
        private const string _FieldSkinStr = "ToolbarSeachTextField";
        private const string _CancelBtnSkinStr = "ToolbarSeachCancelButton";
        private StartSearchEvent _SearchEvt;
		private KeyCode _LastKey;
		private float _KeydownTime;

        private bool m_bIsFocused
        {
            get
            {
                return GUI.GetNameOfFocusedControl() == id;
            }
        }

        public ESearchField(int w, StartSearchEvent evt)
        {
            width = w;
            _SearchEvt = evt;
            id = this.GetHashCode().ToString();
            cancelBtn = new EButton(string.Empty, OnClickCancelBtn);
        }

        public override void Draw()
        {
            GUI.SetNextControlName(id);
            DrawTextField();
			cancelBtn.DrawStyle(_CancelBtnSkinStr);
			if (m_bIsFocused && Event.current.isKey)
			{
				if(Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
				{
					NotifySearch();
					GUI.FocusControl(null);
				}
				else
					RefreshKeyDown(Event.current.keyCode);
			}
			if(_LastKey != KeyCode.None && Time.realtimeSinceStartup - _KeydownTime > 0.001f)
				NotifySearch ();
        }

        void DrawTextField()
        {
            content = GUILayout.TextField(content, GUI.skin.FindStyle(_FieldSkinStr), GUILayout.Width(width));
        }

        void OnClickCancelBtn(EComponentBase btn, bool IsPress)
        {
            content = string.Empty;
            _SearchEvt(this, content);
            GUI.FocusControl(null);
        }

		void RefreshKeyDown(KeyCode key)
		{
			_LastKey = key;
			_KeydownTime = Time.realtimeSinceStartup;
		}

		void NotifySearch()
		{
			_LastKey = KeyCode.None;
			_SearchEvt(this, content);
		}
    }
}
