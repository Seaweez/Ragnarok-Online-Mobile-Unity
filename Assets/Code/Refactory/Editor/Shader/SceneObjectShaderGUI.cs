using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace UnityEditor
{
	internal class SceneObjectShaderGUI : ShaderGUI 
	{
		private static class Styles
		{
			public static readonly string propertyMode = "_Mode";
			public static readonly string propertyMainTex = "_MainTex";
			public static readonly string propertyColor = "_Color";
			public static readonly string propertyCutoff = "_Cutoff";

			public static readonly string propertyWindDirection = "_WindDirection";
			public static readonly string propertyWindTimeScale = "_WindTimeScale";

			public static readonly string propertyCutX = "_CutX";
			public static readonly string propertyCutY = "_CutY";

			public static readonly string renderingMode = "Rendering Mode";
			public static readonly string[] blendNames = System.Enum.GetNames (typeof (ShaderManager.SceneObjectBlendMode));

			public static readonly GUIContent albedoText = new GUIContent("Albedo", "Albedo (RGB) and Transparency (A)");
			public static readonly GUIContent alphaCutoffText = new GUIContent("Alpha Cutoff", "Threshold for alpha cutoff");

			public static readonly GUIContent windText = new GUIContent("Wind", "Wind Settings");
			public static readonly GUIContent windDirectionText = new GUIContent("Wind Direction", "Wind Direction");
			public static readonly GUIContent windTimeScaleText = new GUIContent("Wind Time Scale", "Wind Time Scale");

			public static readonly string cutXText = "Cut X";
			public static readonly string cutYText = "Cut Y";
		}

		private MaterialProperty blendMode = null;
		private MaterialProperty albedoMap = null;
		private MaterialProperty albedoColor = null;
		private MaterialProperty alphaCutoff = null;
		private MaterialProperty windDirection = null;
		private MaterialProperty windTimeScale = null;
		private MaterialProperty cutX = null;
		private MaterialProperty cutY = null;

		public static void MaterialChanged(Material material)
		{
			ShaderManager.HandleSceneObjectMaterial(material);
		}

		private void FindProperties (MaterialProperty[] props)
		{
			blendMode = FindProperty (Styles.propertyMode, props);
			albedoMap = FindProperty (Styles.propertyMainTex, props);
			albedoColor = FindProperty (Styles.propertyColor, props);
			alphaCutoff = FindProperty (Styles.propertyCutoff, props);
			windDirection = FindProperty (Styles.propertyWindDirection, props, false);
			windTimeScale = FindProperty (Styles.propertyWindTimeScale, props, false);
			cutX = FindProperty (Styles.propertyCutX, props);
			cutY = FindProperty (Styles.propertyCutY, props);
		}

		private void BlendModePopup(MaterialEditor materialEditor)
		{
			EditorGUI.showMixedValue = blendMode.hasMixedValue;
			var mode = (ShaderManager.SceneObjectBlendMode)blendMode.floatValue;
			
			EditorGUI.BeginChangeCheck();
			mode = (ShaderManager.SceneObjectBlendMode)EditorGUILayout.Popup(Styles.renderingMode, (int)mode, Styles.blendNames);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo("Rendering Mode");
				blendMode.floatValue = (float)mode;
			}
			
			EditorGUI.showMixedValue = false;
		}

		void AlbedoArea(MaterialEditor materialEditor)
		{
			var material = materialEditor.target as Material;
			materialEditor.TexturePropertySingleLine(Styles.albedoText, albedoMap, albedoColor);
			if (((ShaderManager.SceneObjectBlendMode)material.GetFloat(Styles.propertyMode) == ShaderManager.SceneObjectBlendMode.Cutout))
			{
				materialEditor.ShaderProperty(alphaCutoff, Styles.alphaCutoffText.text, MaterialEditor.kMiniTextureFieldLabelIndentLevel+1);
			}
		}

		void WindArea(MaterialEditor materialEditor)
		{
//			var material = materialEditor.target as Material;
			EditorGUILayout.LabelField(Styles.windText);
			materialEditor.VectorProperty(windDirection, Styles.windDirectionText.text);
			materialEditor.FloatProperty(windTimeScale, Styles.windTimeScaleText.text);
		}

		void CutArea(MaterialEditor materialEditor)
		{
			var cutEnableCount = 0;
			var cutDisableCount = 0;
			foreach (var t in blendMode.targets)
			{
				var mat = t as Material;
				var test = mat.IsKeywordEnabled("_CUT_ON");
				if (test)
				{
					++cutEnableCount;
				}
				else
				{
					++cutDisableCount;
				}
			}

			var enable = false;
			#region cut
			EditorGUILayout.Separator();

			EditorGUI.showMixedValue = 0 < cutEnableCount && 0 < cutDisableCount;
			EditorGUI.BeginChangeCheck();
			enable = EditorGUILayout.BeginToggleGroup("Enable(Cut)", 0<cutEnableCount);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo("Enable(Cut)");
				foreach (var t in blendMode.targets)
				{
					var mat = t as Material;
					if (enable)
					{
						mat.EnableKeyword("_CUT_ON");
					}
					else
					{
						mat.DisableKeyword("_CUT_ON");
					}
				}
			}
			EditorGUI.showMixedValue = false;

			EditorGUI.showMixedValue = cutX.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			var value = materialEditor.FloatProperty(cutX, Styles.cutXText);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.cutXText);
				cutX.floatValue = value;
			}
			EditorGUI.showMixedValue = false;

			EditorGUI.showMixedValue = cutY.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			value = materialEditor.FloatProperty(cutY, Styles.cutYText);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.cutYText);
				cutY.floatValue = value;
			}
			EditorGUI.showMixedValue = false;

			EditorGUILayout.EndToggleGroup();
			#endregion cut
		}

		public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] props)
		{
			base.OnGUI(materialEditor, props);
			EditorGUI.BeginChangeCheck();
			FindProperties (props);
			BlendModePopup(materialEditor);
			EditorGUILayout.Separator();
			AlbedoArea(materialEditor);
			if (null != windDirection && null != windTimeScale)
			{
				WindArea(materialEditor);
			}
			if (EditorGUI.EndChangeCheck())
			{
				foreach (var t in blendMode.targets)
				{
					MaterialChanged(t as Material);
				}
			}

			EditorGUILayout.Separator();
			CutArea(materialEditor);

		}

		public override void AssignNewShaderToMaterial (Material material, Shader oldShader, Shader newShader)
		{
			base.AssignNewShaderToMaterial (material, oldShader, newShader);
			MaterialChanged(material);
		}

	}
} // namespace UnityEditor
