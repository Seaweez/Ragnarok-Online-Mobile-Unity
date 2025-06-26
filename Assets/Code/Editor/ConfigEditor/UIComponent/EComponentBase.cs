using UnityEngine;
using UnityEditor;

namespace EditorTool
{
    public class EComponentBase
	{
		public delegate void VoidDelegate (EComponentBase c);
		public VoidDelegate onClick;
		public void OnClick ()		{ if (onClick != null) onClick(this); }
        public enum EComponentLayout
        {
            GUI,
            GUILayout
        }

        virtual public void Draw()
        {
        }
    }
}
