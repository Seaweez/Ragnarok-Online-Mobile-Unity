using UnityEngine;
using UnityEditor;
using System.IO;

namespace EditorTool
{
	public class AtlasMakerConfigCell
	{
		public AtlasData data;

        static Color32 _SelectedColor = new Color32(62, 85, 150, 255);
        static GUIStyle _TitleStyle = new GUIStyle();
		private ELabel _TitleLabel = null;
		public AtlasMakerConfigCell (AtlasData d)
		{
			InitStyle ();
			Reset (d);
		}

		public void Reset(AtlasData d)
		{
			data = d;
			InitTitleLabel ();
		}

		private void InitStyle()
		{
			_TitleStyle.alignment = TextAnchor.MiddleLeft;
			_TitleStyle.normal.textColor = Color.white;
			_TitleStyle.fontSize = 12;
		}

		private void InitTitleLabel()
		{
			if (_TitleLabel == null)
				_TitleLabel = new ELabel ();
			_TitleLabel.content.text = data.atlasName;
		}

		public void DrawTitle()
		{
			Rect r = EditorGUILayout.GetControlRect();
			if (r.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
				AtlasInspectorWindow.selected.cell = this;
			if(this == AtlasInspectorWindow.selected.cell)
				EditorGUI.DrawRect(new Rect(r), _SelectedColor);
			_TitleLabel.content.text = data.atlasName;
			_TitleLabel.DrawGUI (r, _TitleStyle);
		}
	}
}

