using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(PolygonSprite))]
	public class PolygonSpriteEditor :Editor
	{
		PolygonSprite _ui;
		bool _needRebuild = false;
		bool _dirty = false;

		void Reset ()
		{
			_needRebuild = false;
			_dirty = false;
		}

		void CheckDirty ()
		{
			if (_needRebuild) {
				_ui.ReBuildPolygon ();
			} else if (_dirty) {
			}
		}

		public override void OnInspectorGUI ()
		{
			Reset ();
			serializedObject.Update ();
			_ui = target as PolygonSprite;
			DrawMat ();
			int side = EditorGUILayout.IntSlider ("边数", _ui.SideNum, 3, 12);
			if (side != _ui.SideNum) {
				_ui.SideNum = side;
			}
			DrawMode ();
			DrawColor ();
			DrawLengths ();
			CheckDirty ();
			serializedObject.ApplyModifiedProperties ();
		}

		void DrawMat ()
		{
			NGUIEditorTools.DrawProperty ("材质", serializedObject, "mat", GUILayout.MinWidth (40f));
			NGUIEditorTools.DrawProperty ("着色器", serializedObject, "useShader", GUILayout.MinWidth (40f));
		}

		void DrawMode()
		{
			GUILayout.BeginHorizontal ();
			_ui.useVertexColor = EditorGUILayout.Toggle ("顶点色截断", _ui.useVertexColor);
			EditorGUI.BeginDisabledGroup (!_ui.useVertexColor);
			{
				GUILayout.EndHorizontal ();
				
				GUILayout.BeginHorizontal ();
				NGUIEditorTools.SetLabelWidth (50f);
				GUILayout.Space (30f);
				_ui.vertexColorLength = EditorGUILayout.FloatField("顶点色长度最大",_ui.vertexColorLength);
				//				NGUIEditorTools.DrawProperty ("外部颜色", serializedObject, "mgradientOutside", GUILayout.MinWidth (40f));
				NGUIEditorTools.SetLabelWidth (80f);
			}
			EditorGUI.EndDisabledGroup ();
			GUILayout.EndHorizontal ();
		}

		void DrawColor ()
		{
			GUILayout.BeginHorizontal ();
			_ui.isGradient = EditorGUILayout.Toggle ("渐变色", _ui.isGradient);
			EditorGUI.BeginDisabledGroup (!_ui.isGradient);
			{
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				NGUIEditorTools.SetLabelWidth (50f);
				GUILayout.Space (30f);
				_ui.gradientInside = EditorGUILayout.ColorField("内部颜色",_ui.gradientInside, GUILayout.MinWidth (40f));

//				NGUIEditorTools.DrawProperty ("内部颜色", serializedObject, "mgradientInside", GUILayout.MinWidth (40f));
				NGUIEditorTools.SetLabelWidth (80f);
				GUILayout.EndHorizontal ();
				
				GUILayout.BeginHorizontal ();
				NGUIEditorTools.SetLabelWidth (50f);
				GUILayout.Space (30f);
				_ui.gradientOutside = EditorGUILayout.ColorField("外部颜色",_ui.gradientOutside, GUILayout.MinWidth (40f));
//				NGUIEditorTools.DrawProperty ("外部颜色", serializedObject, "mgradientOutside", GUILayout.MinWidth (40f));
				NGUIEditorTools.SetLabelWidth (80f);
			}
			EditorGUI.EndDisabledGroup ();
			GUILayout.EndHorizontal ();
		}

		void DrawLengths ()
		{
			int sides = _ui.SideNum;
			for (int i=0; i<sides; i++) {
				float len = EditorGUILayout.FloatField ("第" + (i + 1) + "点", _ui.lengths [i]);
				if (len != _ui.lengths [i]) {
					_ui.SetLength (i, len);
				}
			}
		}
	}
} // namespace EditorTool
