using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    public class ELabel : EComponentBase
    {
        private const float _WordWidth = 12f;
		public GUIContent content;

        float _Width;
        public float width { get{ return _Width; } }

		public ELabel(string src = "", float w = 0f, VoidDelegate d = null)
        {
            content = new GUIContent(src);
			_Width = w > 0 && !string.IsNullOrEmpty(src) ? w : src.Length * _WordWidth;
			if (d != null)
				onClick += d;
        }

		public ELabel(GUIContent con, float w = 100.0f, VoidDelegate d = null)
        {
            content = con;
			_Width = w;
			if (d != null)
				onClick += d;
        }

        public void SetLabelWidth(float w)
        {
            _Width = w;
        }

        public void SetTooltip(string tooltip)
        {
            if (content != null)
                content.tooltip = tooltip;
        }

        public override void Draw()
		{
			GUILayout.Label(content);
			CheckMouseClick ();
        }

		public void Draw(GUIStyle style)
		{
			GUILayout.Label (content, style);
			CheckMouseClick ();
		}

		public void DrawGUI(Rect r, GUIStyle style)
		{
			CheckMouseClick (r);
			GUI.Label (r, content, style);
		}

        public void DrawColorGUI(Rect r, Color col)
		{
			CheckMouseClick (r);
            GUI.contentColor = col;
            GUI.Label(r, content);
			GUI.contentColor = Color.white;
        }

		public void AddOnClickListener(VoidDelegate vd)
		{
			if(vd != null)
				onClick += vd;
		}

		void CheckMouseClick()
		{
			Rect r = GUILayoutUtility.GetLastRect ();
			CheckMouseClick(r);
		}

		void CheckMouseClick(Rect r)
		{
			if (r.Contains (Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
				OnClick ();
		}
    }
}
