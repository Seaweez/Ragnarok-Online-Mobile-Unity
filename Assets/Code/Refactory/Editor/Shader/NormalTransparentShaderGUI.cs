using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace UnityEditor
{
	internal class NormalTransparentShaderGUI : ShaderGUI 
	{
		private static class Styles
		{
			public static readonly string propertyFrameAnimation = "_FrameAnimation";
			public static readonly string propertyColumn = "_Column";
			public static readonly string propertyRow = "_Row";
			public static readonly string propertySpeed = "_Speed";

			public static readonly GUIContent columnText = new GUIContent("Column");
			public static readonly GUIContent rowText = new GUIContent("Row");
			public static readonly GUIContent speedText = new GUIContent("Speed");
		}

		private MaterialProperty frameAnimation = null;
		private MaterialProperty column = null;
		private MaterialProperty row = null;
		private MaterialProperty speed = null;

		public static void MaterialChanged(Material material)
		{
			
		}

		private void FindProperties (MaterialProperty[] props)
		{
			frameAnimation = FindProperty (Styles.propertyFrameAnimation, props);
			column = FindProperty (Styles.propertyColumn, props);
			row = FindProperty (Styles.propertyRow, props);
			speed = FindProperty (Styles.propertySpeed, props);
		}

		private void OnGUI_Int(MaterialEditor materialEditor, GUIContent text, MaterialProperty property)
		{
			EditorGUI.showMixedValue = property.hasMixedValue;
			var value = (int)property.floatValue;
			EditorGUI.BeginChangeCheck();
			value = EditorGUILayout.IntField(text, (int)property.floatValue);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(text.text);
				property.floatValue = value;
			}
			EditorGUI.showMixedValue = false;
		}

		private void OnGUI_Float(MaterialEditor materialEditor, GUIContent text, MaterialProperty property)
		{
			EditorGUI.showMixedValue = property.hasMixedValue;
			var value = property.floatValue;
			EditorGUI.BeginChangeCheck();
			value = EditorGUILayout.FloatField(text, property.floatValue);
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
			if (0f != frameAnimation.floatValue)
			{
				OnGUI_Int(materialEditor, Styles.columnText, column);
				OnGUI_Int(materialEditor, Styles.rowText, row);
				OnGUI_Float(materialEditor, Styles.speedText, speed);

				if (EditorGUI.EndChangeCheck())
				{
					foreach (var t in column.targets)
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
