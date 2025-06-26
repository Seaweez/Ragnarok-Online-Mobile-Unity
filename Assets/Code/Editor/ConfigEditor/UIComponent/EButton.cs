using UnityEngine;
using UnityEditor;
using System;

namespace EditorTool
{
    public class EButton : EComponentBase
    {
        public delegate void OnClickEvent(EComponentBase component, bool IsPress);
        private OnClickEvent OnClickEvt;
        public GUIContent content;

        private EComponentLayout m_eLayout = EComponentLayout.GUILayout;

        private bool m_bIsPress = false;
        private Action _DrawBtnAction;
        private Rect _BtnRect;

        public EButton(string name, OnClickEvent evt, string image = null)
        {
            Init(name, evt, image, EComponentLayout.GUILayout);
        }

        public EButton(string name, OnClickEvent evt, Rect rect, string image = null)
        {
            _BtnRect = rect;
            Init(name, evt, image, EComponentLayout.GUI);
        }

        private void Init(string name, OnClickEvent evt, string image, EComponentLayout layout)
        {
            SetOnClickEvent(evt);
            InitGUIContent(name, image);
            SetBtnLayout(layout);
        }

        private void InitGUIContent(string name, string image)
        {
            if(image == null)
                content = new GUIContent(name);
            else
                content = new GUIContent(name, EditorGUIUtility.IconContent(image).image);
        }

        private void SetOnClickEvent(OnClickEvent evt)
        {
            OnClickEvt = evt;
        }

        private void SetBtnLayout(EComponentLayout layout)
        {
            m_eLayout = layout;
            switch (m_eLayout)
            {
                case EComponentLayout.GUI:
                    _DrawBtnAction = DrawGUIBtn;
                    break;
                case EComponentLayout.GUILayout:
                default:
                    _DrawBtnAction = DrawGUILayoutBtn;
                    break;
            }
        }

        public override void Draw()
        {
            _DrawBtnAction();
        }

        public void DrawStyle(string style)
        {
            bool b = GUILayout.Button(content, GUI.skin.GetStyle(style));
            SetBtnState(b);
        }

        public void DrawStyle(GUIStyle style)
        {
            bool b = GUILayout.Button(content, style);
            SetBtnState(b);
        }

        public void DrawGUIStyle(string style)
        {
            bool b = GUI.Button(_BtnRect, content, GUI.skin.GetStyle(style));
            SetBtnState(b);
        }

        public void DrawGUIStyle(GUIStyle style)
        {
            bool b = GUI.Button(_BtnRect, content, style);
            SetBtnState(b);
        }

        private void DrawGUILayoutBtn()
        {
            bool b = GUILayout.Button(content, EditorStyles.toolbarButton);
            SetBtnState(b);
        }

        private void DrawGUIBtn()
        {
            bool b = GUI.Button(_BtnRect, content);
            SetBtnState(b);
        }

        private void SetBtnState(bool b)
        {
            if (b != m_bIsPress && OnClickEvt != null)
            {
                m_bIsPress = b;
                OnClickEvt(this, m_bIsPress);
            }
        }
    }
}
