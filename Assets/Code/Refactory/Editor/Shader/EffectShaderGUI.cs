using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace UnityEditor
{
	internal class EffectShaderGUI : ShaderGUI 
	{
		public enum BlendMode
		{
			Additive,
			AlphaBlend
		}
		private static class Styles
		{
			public static readonly string propertyMode = "_Mode";
			public static readonly string propertyZWrite = "_ZWrite1";
			public static readonly string propertyCullMode = "_Cull";
			public static readonly string propertyMainTex = "_MainTex";
			public static readonly string propertyColor = "_TintColor";
			public static readonly string propertyMaskTex = "_MaskTex";
			public static readonly string propertyMainTexUVSpeed = "_MainTexUVSpeed";
			public static readonly string propertyMaskTexUVSpeed = "_MaskTexUVSpeed";

			public static readonly string renderingMode = "Rendering Mode";
			public static readonly string[] blendNames = System.Enum.GetNames (typeof (BlendMode));
			public static readonly string zwriteOn = "ZWrite";
			public static readonly string cullMode = "Cull";
			public static readonly string[] cullModeNames = System.Enum.GetNames (typeof (UnityEngine.Rendering.CullMode));
		
			public static readonly string maskTex = "Mask Tex";

			public static readonly string mainTexUVSpeed = "Main Tex UV Speed";
			public static readonly string maskTexUVSpeed = "Mask Tex UV Speed";
		}

		private MaterialProperty blendMode = null;
		private MaterialProperty zwriteOn = null;
		private MaterialProperty cullMode = null;
		private MaterialProperty maskTex = null;
		private MaterialProperty mainTexUVSpeed = null;
		private MaterialProperty maskTexUVSpeed = null;

		public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
		{
			switch (blendMode)
			{
			case BlendMode.Additive:
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
				material.EnableKeyword ("_ADDITIVE_ON");
				break;
			case BlendMode.AlphaBlend:
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				material.DisableKeyword ("_ADDITIVE_ON");
				break;
			}
		}
		public static void SetupMaterialWithZWrite(Material material, bool zwriteOn)
		{
			material.SetInt(Styles.propertyZWrite, (zwriteOn ? 1 : 0));
		}
		public static void SetupMaterialWithCullMode(Material material, UnityEngine.Rendering.CullMode cullMode)
		{
			material.SetInt(Styles.propertyCullMode, (int)cullMode);
		}
		public static void MaterialChanged(Material material)
		{
			SetupMaterialWithBlendMode(material, (BlendMode)material.GetFloat(Styles.propertyMode));
		}

		private void FindProperties (MaterialProperty[] props)
		{
			blendMode = FindProperty (Styles.propertyMode, props);
			zwriteOn = FindProperty (Styles.propertyZWrite, props);
			cullMode = FindProperty (Styles.propertyCullMode, props);
			maskTex = FindProperty (Styles.propertyMaskTex, props);
			mainTexUVSpeed = FindProperty (Styles.propertyMainTexUVSpeed, props);
			maskTexUVSpeed = FindProperty (Styles.propertyMaskTexUVSpeed, props);
		}

		private void BlendModePopup(MaterialEditor materialEditor)
		{
			EditorGUI.showMixedValue = blendMode.hasMixedValue;
			var mode = (BlendMode)blendMode.floatValue;
			
			EditorGUI.BeginChangeCheck();
			mode = (BlendMode)EditorGUILayout.Popup(Styles.renderingMode, (int)mode, Styles.blendNames);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.renderingMode);
				blendMode.floatValue = (float)mode;
			}
			
			EditorGUI.showMixedValue = false;
		
		}
		private void ZWriteSet(MaterialEditor materialEditor)
		{
			EditorGUI.showMixedValue = zwriteOn.hasMixedValue;
			var mode = (1 == zwriteOn.floatValue);
			
			EditorGUI.BeginChangeCheck();
			mode = EditorGUILayout.ToggleLeft(Styles.zwriteOn, mode);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.zwriteOn);
				zwriteOn.floatValue = mode ? 1f : 0f;
			}
			
			EditorGUI.showMixedValue = false;
		}
		private void CullModePopup(MaterialEditor materialEditor)
		{
			EditorGUI.showMixedValue = cullMode.hasMixedValue;
			var mode = (UnityEngine.Rendering.CullMode)cullMode.floatValue;
			
			EditorGUI.BeginChangeCheck();
			mode = (UnityEngine.Rendering.CullMode)EditorGUILayout.Popup(Styles.cullMode, (int)mode, Styles.cullModeNames);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.cullMode);
				cullMode.floatValue = (float)mode;
			}
			
			EditorGUI.showMixedValue = false;
		}
		private void MaskSet(MaterialEditor materialEditor)
		{
			var maskEnableCount = 0;
			var maskDisableCount = 0;
			foreach (var t in maskTex.targets)
			{
				var material = (t as Material);
				var test = material.IsKeywordEnabled ("_MASK_ON");
				if (test)
				{
					++maskEnableCount;
				}
				else
				{
					++maskDisableCount;
				}
			}

			EditorGUI.showMixedValue = 0 < maskEnableCount && 0 < maskDisableCount;
			EditorGUI.BeginChangeCheck();
			var enable = EditorGUILayout.BeginToggleGroup("Enable(Mask)", 0<maskEnableCount);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo("Enable(Mask)");
				foreach (var t in maskTex.targets)
				{
					var material = (t as Material);
					if (enable)
					{
						material.EnableKeyword("_MASK_ON");
					}
					else
					{
						material.DisableKeyword("_MASK_ON");
					}
				}
			}
			EditorGUI.showMixedValue = false;

			EditorGUI.showMixedValue = maskTex.hasMixedValue;
			materialEditor.TextureProperty(maskTex, Styles.maskTex);
			EditorGUI.showMixedValue = false;

			EditorGUILayout.EndToggleGroup();
		}

		private void UVAnimationSet(MaterialEditor materialEditor)
		{
			var enableCount = 0;
			var disableCount = 0;
			foreach (var t in mainTexUVSpeed.targets)
			{
				var material = (t as Material);
				var test = material.IsKeywordEnabled ("_UV_ANIMATION_ON");
				if (test)
				{
					++enableCount;
				}
				else
				{
					++disableCount;
				}
			}

			EditorGUI.showMixedValue = 0 < enableCount && 0 < disableCount;
			EditorGUI.BeginChangeCheck();
			var enable = EditorGUILayout.BeginToggleGroup("Enable(UV Animation)", 0<enableCount);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo("Enable(UV Animation)");
				foreach (var t in mainTexUVSpeed.targets)
				{
					var material = (t as Material);
					if (enable)
					{
						material.EnableKeyword("_UV_ANIMATION_ON");
					}
					else
					{
						material.DisableKeyword("_UV_ANIMATION_ON");
					}
				}
			}
			EditorGUI.showMixedValue = false;

			EditorGUI.showMixedValue = mainTexUVSpeed.hasMixedValue;
			materialEditor.VectorProperty(mainTexUVSpeed, Styles.mainTexUVSpeed);
			materialEditor.VectorProperty(maskTexUVSpeed, Styles.maskTexUVSpeed);
			EditorGUI.showMixedValue = false;

			EditorGUILayout.EndToggleGroup();
		}

		public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] props)
		{
			EditorGUI.BeginChangeCheck();
			FindProperties (props);
			BlendModePopup(materialEditor);
			ZWriteSet(materialEditor);
			CullModePopup(materialEditor);
			EditorGUILayout.Separator();
			base.OnGUI(materialEditor, props);

			EditorGUILayout.Separator();
			MaskSet(materialEditor);

			EditorGUILayout.Separator();
			UVAnimationSet(materialEditor);


			if (EditorGUI.EndChangeCheck())
			{
				foreach (var t in blendMode.targets)
				{
					MaterialChanged(t as Material);
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
