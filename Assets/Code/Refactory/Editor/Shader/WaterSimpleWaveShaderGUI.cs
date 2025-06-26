using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace UnityEditor
{
	internal class WaterSimpleWaveShaderGUI : ShaderGUI 
	{
		private static class Styles
		{
			public static readonly string propertySmoothTransparent = "_SmoothTransparen";
			public static readonly string propertyTransparentDistance = "_TransparentDistance";

			public static readonly GUIContent transparentDistanceText = new GUIContent("Transparent Distance");
		}

		private MaterialProperty smoothTransparent = null;
		private MaterialProperty transparentDistance = null;

		public static void MaterialChanged(Material material)
		{
			
		}

		private void FindProperties (MaterialProperty[] props)
		{
			smoothTransparent = FindProperty (Styles.propertySmoothTransparent, props);
			transparentDistance = FindProperty (Styles.propertyTransparentDistance, props);
		}

		private void OnGUI_Slider(MaterialEditor materialEditor, GUIContent text, MaterialProperty property, float left, float right)
		{
			EditorGUI.showMixedValue = property.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			var value = EditorGUILayout.Slider(text, property.floatValue, left, right);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(text.text);
				property.floatValue = value;
			}
			EditorGUI.showMixedValue = false;
		}

		public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] props)
		{
			base.OnGUI(materialEditor, props);
			EditorGUI.BeginChangeCheck();
			FindProperties (props);
			if (0f != smoothTransparent.floatValue)
			{
//				OnGUI_Slider(materialEditor, Styles.transparentDistanceText, transparentDistance, 0, 2);
				materialEditor.ShaderProperty(transparentDistance, Styles.transparentDistanceText.text);

				if (EditorGUI.EndChangeCheck())
				{
					foreach (var t in smoothTransparent.targets)
					{
						MaterialChanged(t as Material);
					}
				}
			}
		}

		public override void AssignNewShaderToMaterial (Material material, Shader oldShader, Shader newShader)
		{
			base.AssignNewShaderToMaterial (material, oldShader, newShader);
			MaterialChanged(material);
		}

	}
} // namespace UnityEditor
