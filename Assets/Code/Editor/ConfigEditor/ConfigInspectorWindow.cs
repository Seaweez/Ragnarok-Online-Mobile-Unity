using System;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    public class ConfigInspectorWindow : EditorWindow
	{
		Vector3 m_ScrollPos = Vector3.zero;

        public ConfigInspectorWindow()
        {
        }

		void Update()
		{
			Repaint ();
		}

		void OnGUI()
		{
			if (ConfigListWindow.selectedUI != null)
			{
				m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos);
				ConfigListWindow.selectedUI.DrawDetail ();
				EditorGUILayout.EndScrollView();
			}
		}

		void OnDisable()
		{
			m_ScrollPos = Vector3.zero;
		}
	}
}
