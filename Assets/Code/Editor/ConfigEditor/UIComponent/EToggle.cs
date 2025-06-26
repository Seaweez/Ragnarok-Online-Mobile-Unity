using UnityEngine;
using UnityEditor;
using System;

namespace EditorTool
{
    public class EToggle : EComponentBase
    {
        public delegate void OnToggleEvent(EComponentBase toggle, bool isToggle);
        OnToggleEvent _OnToggleEvt;

        public GUIContent content;

        private bool m_bIsToggle = false;

        public EToggle(string c, OnToggleEvent evt)
        {
            content = new GUIContent(c);
            SetOnToggleEvent(evt);
        }

        public EToggle(string c, Texture img, OnToggleEvent evt)
        {
            content = new GUIContent(c, img);
            SetOnToggleEvent(evt);
        }

        public void SetOnToggleEvent(OnToggleEvent evt)
        {
            _OnToggleEvt = evt;
        }

        public override void Draw()
        {
            bool b = GUILayout.Toggle(m_bIsToggle, content, EditorStyles.toolbarButton);
            SetToggleState(b);
        }

        public void SetContent(string c)
        {
            content.text = c;
        }

        public void SetToggleState(bool b)
        {
            if(b != m_bIsToggle && _OnToggleEvt != null)
            {
                m_bIsToggle = b;
                _OnToggleEvt(this, m_bIsToggle);
            }
        }
    }
}
