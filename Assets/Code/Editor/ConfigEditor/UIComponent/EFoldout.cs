using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    public class EFoldout : EComponentBase
    {
        public delegate void OnChooseEvent(EComponentBase component, bool isChoose);
        private OnChooseEvent OnChooseEvt;
        public delegate void OnFoldoutEvent(EComponentBase component, bool isFoldout);
        private OnFoldoutEvent OnFoldoutEvt;
        private GUIContent content;
        private bool bIsFoldout = false;
        private bool bIsChoose = false;
        public EFoldout(string src, OnFoldoutEvent foldoutEvt, OnChooseEvent chooseEvt)
        {
            content = new GUIContent(src);
            OnFoldoutEvt = foldoutEvt;
            OnChooseEvt = chooseEvt;
        }

        public EFoldout(GUIContent c, OnFoldoutEvent foldoutEvt, OnChooseEvent chooseEvt)
        {
            content = c;
            OnFoldoutEvt = foldoutEvt;
            OnChooseEvt = chooseEvt;
        }

        public void DrawFoldout(Rect r)
        {
            bool b = EditorGUI.Foldout(r, bIsFoldout, content, EditorStyles.foldout);
            if (b != bIsFoldout)
            {
                bIsFoldout = b;
                OnFoldoutEvt(this, bIsFoldout);
            }
        }

        public void SetChoose(bool b)
        {
            bIsChoose = b;
            OnChooseEvt(this, bIsChoose);
        }
    }
}
