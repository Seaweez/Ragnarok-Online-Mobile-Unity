using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace UnityEditor
{
	internal class RolePartShaderGUI : ShaderGUI 
	{
		private static class Styles
		{
			public static readonly string propertyMode = "_Mode";
			public static readonly string renderingMode = "Rendering Mode";
			public static readonly string[] blendNames = System.Enum.GetNames (typeof (RolePart.BlendMode));
			
			public static readonly string propertyZWrite = "_ZWrite";
			public static readonly string zwriteOn = "ZWrite";

			public static readonly string propertyAlpha = "_Alpha";
			public static readonly string alpha = "Alpha";

			public static readonly string propertyChangeColor = "_ChangeColor";
			public static readonly string changeColor = "LerpColor";

			public static readonly string propertyMaskColor = "_MaskColor";
			public static readonly string maskColor = "MaskColor";
			public static readonly string propertyMaskColorThreshold = "_MaskColorThreshold";
			public static readonly string maskColorThreshold = "MaskColorThreshold";

            public static readonly string propertyMask2Color = "_Mask2Color";
            public static readonly string mask2Color = "Mask2Color";

            public static readonly string propertySatValue = "_SatValue";
            public static readonly string SatValue         = "SatValue";


            public static readonly string propertyLightColor1 = "_LightColor1";
			public static readonly string lightColor1 = "ToonColor1";

			public static readonly string propertyLightExposure1 = "_Exposure1";
			public static readonly string lightExposure1 = "ToonExposure1";

			public static readonly string propertyLightColor2 = "_LightColor3";
			public static readonly string lightColor2 = "ToonColor2";

			public static readonly string propertyLightExposure2 = "_Exposure3";
			public static readonly string lightExposure2 = "ToonExposure2";

			public static readonly string propertyToonEffect = "_ToonEffect";
			public static readonly string toonEffect = "ToonEffect";

			public static readonly string propertyToonStep = "_ToonSteps";
			public static readonly string toonStep = "ToonSteps";

			public static readonly string propertyMultiColorNumber = "_MultiColorNumber";
			public static readonly string multiColorNumber = "MultiColorNumber";

			public static readonly string propertySubTex = "_SubTex";
			public static readonly string subTex = "Sub Albedo";

			public static readonly string propertyBlinkSpeed = "_BlinkSpeed";
			public static readonly string blinkSpeed = "Blink Speed";

			public static readonly string propertyBlinkMin = "_BlinkMin";
			public static readonly string blinkMin = "Blink Min";

			public static readonly string propertyBlinkMax = "_BlinkMax";
			public static readonly string blinkMax = "Blink Max";
		}
		
		private MaterialProperty blendMode = null;
		private MaterialProperty zwriteOn = null;
		private MaterialProperty alpha = null;
		private MaterialProperty changeColor = null;
		private MaterialProperty maskColor = null;
        private MaterialProperty mask2Color = null;
        private MaterialProperty SatValue = null;
        private MaterialProperty maskColorThreshold = null;
		private MaterialProperty lightColor1 = null;
		private MaterialProperty lightExposure1 = null;
		private MaterialProperty lightColor2 = null;
		private MaterialProperty lightExposure2 = null;
		private MaterialProperty toonEffect = null;
		private MaterialProperty toonStep = null;
		private MaterialProperty multiColorNumber = null;

		private MaterialProperty subTex = null;
		private MaterialProperty blinkSpeed = null;
		private MaterialProperty blinkMin = null;
		private MaterialProperty blinkMax = null;

		public static void MaterialChanged(Material material)
		{
			RolePart.SetBlendMode(material, RolePart.GetBlendMode(material));
		}
		
		private void FindProperties (MaterialProperty[] props)
		{
			blendMode = FindProperty (Styles.propertyMode, props);
			zwriteOn = FindProperty (Styles.propertyZWrite, props);
			alpha = FindProperty (Styles.propertyAlpha, props);
			changeColor = FindProperty (Styles.propertyChangeColor, props);
			maskColor = FindProperty (Styles.propertyMaskColor, props);
			maskColorThreshold = FindProperty(Styles.propertyMaskColorThreshold, props);
            mask2Color = FindProperty(Styles.propertyMask2Color, props);
            SatValue = FindProperty(Styles.propertySatValue, props);
            lightColor1 = FindProperty (Styles.propertyLightColor1, props);
			lightExposure1 = FindProperty (Styles.propertyLightExposure1, props);
			lightColor2 = FindProperty (Styles.propertyLightColor2, props);
			lightExposure2 = FindProperty (Styles.propertyLightExposure2, props);
			toonEffect = FindProperty (Styles.propertyToonEffect, props);
			toonStep = FindProperty (Styles.propertyToonStep, props);

			multiColorNumber = FindProperty (Styles.propertyMultiColorNumber, props);

			subTex = FindProperty (Styles.propertySubTex, props);
			blinkSpeed = FindProperty (Styles.propertyBlinkSpeed, props);
			blinkMin = FindProperty (Styles.propertyBlinkMin, props);
			blinkMax = FindProperty (Styles.propertyBlinkMax, props);
		}
		
		private void BlendModePopup(MaterialEditor materialEditor)
		{
			EditorGUI.showMixedValue = blendMode.hasMixedValue;
			var mode = (RolePart.BlendMode)blendMode.floatValue;
			
			EditorGUI.BeginChangeCheck();
			mode = (RolePart.BlendMode)EditorGUILayout.Popup(Styles.renderingMode, (int)mode, Styles.blendNames);
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

		private void OnGUI_Custom(MaterialEditor materialEditor)
		{
			var mode = (RolePart.BlendMode)blendMode.floatValue;
			if (RolePart.BlendMode.Transparent == mode)
			{
				ZWriteSet(materialEditor);
				if (Application.isPlaying)
				{
					EditorGUI.showMixedValue = alpha.hasMixedValue;
					
					EditorGUI.BeginChangeCheck();
					var a = EditorGUILayout.Slider(Styles.alpha, alpha.floatValue, 0f, 1f);
					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo(Styles.alpha);
						alpha.floatValue = a;
					}
					
					EditorGUI.showMixedValue = false;
				}
			}
			
			var cutoutEnableCount = 0;
			var cutoutDisableCount = 0;
			var changeColorEnableCount = 0;
			var changeColorDisableCount = 0;
			var maskColorEnableCount = 0;
            var mask2ColorEnableCount = 0;
            var SATEnableCount = 0;
            var maskColorDisableCount = 0;
            var mask2ColorDisableCount = 0;
            var SATDisableCount = 0;
            var toonEnableCount = 0;
			var toonDisableCount = 0;
			foreach (var t in blendMode.targets)
			{
				var test = RolePart.IsCutoutEnable(t as Material);
				if (test)
				{
					++cutoutEnableCount;
				}
				else
				{
					++cutoutDisableCount;
				}

				test = RolePart.IsChangeColorEnable(t as Material);
				if (test)
				{
					++changeColorEnableCount;
				}
				else
				{
					++changeColorDisableCount;
				}

				test = RolePart.IsMaskColorEnable(t as Material);
				if (test)
				{
					++maskColorEnableCount;
				}
				else
				{
					++maskColorDisableCount;
				}

                test = RolePart.IsMask2ColorEnable(t as Material);
                if (test)
                {
                    ++mask2ColorEnableCount;
                }
                else
                {
                    ++mask2ColorDisableCount;
                }

                test = RolePart.IsSATEnable(t as Material);
                if (test)
                {
                    ++SATEnableCount;
                }
                else
                {
                    ++SATDisableCount;
                }


                test = RolePart.IsToonEnable(t as Material);
				if (test)
				{
					++toonEnableCount;
				}
				else
				{
					++toonDisableCount;
				}
			}

			var enable = false;
			var color = Color.white;
			#region cutout
			EditorGUILayout.Separator();
			
			EditorGUI.showMixedValue = 0 < cutoutEnableCount && 0 < cutoutDisableCount;
			EditorGUI.BeginChangeCheck();
			enable = EditorGUILayout.BeginToggleGroup("Enable(Cutout)", 0<cutoutEnableCount);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo("Enable(Cutout)");
				foreach (var t in blendMode.targets)
				{
					RolePart.SetCutoutEnable(t as Material, enable);
				}
			}
			EditorGUI.showMixedValue = false;

			EditorGUILayout.EndToggleGroup();
			#endregion cutout

			#region change color
			if (Application.isPlaying)
			{
				EditorGUILayout.Separator();

				EditorGUI.showMixedValue = 0 < changeColorEnableCount && 0 < changeColorDisableCount;
				EditorGUI.BeginChangeCheck();
				enable = EditorGUILayout.BeginToggleGroup("Enable(ChangeColor)", 0<changeColorEnableCount);
				if (EditorGUI.EndChangeCheck())
				{
					materialEditor.RegisterPropertyChangeUndo("Enable(ChangeColor)");
					foreach (var t in blendMode.targets)
					{
						RolePart.SetChangeColorEnable(t as Material, enable);
					}
				}
				EditorGUI.showMixedValue = false;

				EditorGUI.showMixedValue = changeColor.hasMixedValue;
				EditorGUI.BeginChangeCheck();
				color = EditorGUILayout.ColorField(Styles.changeColor, changeColor.colorValue);
				if (EditorGUI.EndChangeCheck())
				{
					materialEditor.RegisterPropertyChangeUndo(Styles.changeColor);
					changeColor.colorValue = color;
				}
				EditorGUI.showMixedValue = false;

				EditorGUILayout.EndToggleGroup();
			}
			#endregion change color

			#region mask color
			EditorGUILayout.Separator();
			
			EditorGUI.showMixedValue = 0 < maskColorEnableCount && 0 < maskColorDisableCount;
			EditorGUI.BeginChangeCheck();
			enable = EditorGUILayout.BeginToggleGroup("Enable(MaskColor)", 0<maskColorEnableCount);

			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo("Enable(MaskColor)");
				foreach (var t in blendMode.targets)
				{
					RolePart.SetMaskColorEnable(t as Material, enable);
				}
			}

			EditorGUI.showMixedValue = false;
			EditorGUI.showMixedValue = maskColor.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			color = EditorGUILayout.ColorField(Styles.maskColor, maskColor.colorValue);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.maskColor);
				maskColor.colorValue = color;
			}
			EditorGUI.showMixedValue = false;

			EditorGUI.showMixedValue = maskColorThreshold.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			var v = EditorGUILayout.Slider(Styles.maskColorThreshold, maskColorThreshold.floatValue, 0, 1);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.maskColorThreshold);
				maskColorThreshold.floatValue = v;
			}
			EditorGUI.showMixedValue = false;

			#region multi color
			EditorGUI.showMixedValue = multiColorNumber.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			multiColorNumber.floatValue = EditorGUILayout.IntField(Styles.multiColorNumber, (int)multiColorNumber.floatValue);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.multiColorNumber);
			}
			EditorGUI.showMixedValue = false;
            #endregion multi color

            #region mask 2color
            EditorGUI.showMixedValue = 0 < mask2ColorEnableCount && 0 < mask2ColorDisableCount;
            EditorGUI.BeginChangeCheck();
            enable = EditorGUILayout.BeginToggleGroup("Enable(Mask2Color)", 0 < mask2ColorEnableCount);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo("Enable(Mask2Color)");
                foreach (var t in blendMode.targets)
                {
                    RolePart.SetMask2ColorEnable(t as Material, enable);
                }
            }

            EditorGUI.showMixedValue = false;
            EditorGUI.showMixedValue = maskColor.hasMixedValue;
            EditorGUI.BeginChangeCheck();
            color = EditorGUILayout.ColorField(Styles.mask2Color, mask2Color.colorValue);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(Styles.maskColor);
                mask2Color.colorValue = color;
            }
            EditorGUI.showMixedValue = false;

            EditorGUILayout.EndToggleGroup();
            #endregion
            EditorGUILayout.EndToggleGroup();
            #endregion mask color

            #region Sat
            EditorGUILayout.Separator();
            EditorGUI.showMixedValue = 0 < SATEnableCount && 0 < SATDisableCount;
            EditorGUI.BeginChangeCheck();
            enable = EditorGUILayout.BeginToggleGroup("Enable(SAT)", 0 < SATEnableCount);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo("Enable(SAT)");
                foreach (var t in blendMode.targets)
                {
                    RolePart.SetSATEnable(t as Material, enable);
                }
            }

            EditorGUI.showMixedValue = SatValue.hasMixedValue;
            EditorGUI.BeginChangeCheck();
            v = EditorGUILayout.Slider(Styles.SatValue, SatValue.floatValue, 0, 2);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(Styles.SatValue);
                SatValue.floatValue = v;
            }

            EditorGUILayout.EndToggleGroup();
            #endregion Sat


            #region toon
            EditorGUILayout.Separator();
            EditorGUI.showMixedValue = 0 < toonEnableCount && 0 < toonDisableCount;
			EditorGUI.BeginChangeCheck();
			enable = EditorGUILayout.BeginToggleGroup("Enable(Toon)", 0<toonEnableCount);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo("Enable(Toon)");
				foreach (var t in blendMode.targets)
				{
					RolePart.SetToonEnable(t as Material, enable);
				}
			}
			EditorGUI.showMixedValue = false;

			#region light color 1
			EditorGUI.showMixedValue = lightColor1.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			color = EditorGUILayout.ColorField(Styles.lightColor1, lightColor1.colorValue);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.lightColor1);
				lightColor1.colorValue = color;
			}
			EditorGUI.showMixedValue = false;

			EditorGUI.showMixedValue = lightExposure1.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			v = EditorGUILayout.Slider(Styles.lightExposure1, lightExposure1.floatValue, -3, 3);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.lightExposure1);
				lightExposure1.floatValue = v;
			}
			EditorGUI.showMixedValue = false;
			#endregion light color 1

			#region light color 2
			EditorGUI.showMixedValue = lightColor2.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			color = EditorGUILayout.ColorField(Styles.lightColor2, lightColor2.colorValue);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.lightColor2);
				lightColor2.colorValue = color;
			}
			EditorGUI.showMixedValue = false;
			
			EditorGUI.showMixedValue = lightExposure2.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			v = EditorGUILayout.Slider(Styles.lightExposure2, lightExposure2.floatValue, -3, 3);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.lightExposure2);
				lightExposure2.floatValue = v;
			}
			EditorGUI.showMixedValue = false;
			#endregion light color 2

			#region toon effect
			EditorGUI.showMixedValue = toonEffect.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			v = EditorGUILayout.Slider(Styles.toonEffect, toonEffect.floatValue, 0, 1);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.toonEffect);
				toonEffect.floatValue = v;
			}
			EditorGUI.showMixedValue = false;
			
			EditorGUI.showMixedValue = toonStep.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			v = EditorGUILayout.IntField(Styles.toonStep, (int)toonStep.floatValue);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.toonStep);
				toonStep.floatValue = v;
			}
			EditorGUI.showMixedValue = false;
			#endregion toon effect
			
			EditorGUILayout.EndToggleGroup();
			#endregion toon
		}

		private void OnGUI_Blink(MaterialEditor materialEditor)
		{
			var blinkEnableCount = 0;
			var blinkDisableCount = 0;
			foreach (var t in subTex.targets)
			{
				var test = RolePart.IsBlinkEnable(t as Material);
				if (test)
				{
					++blinkEnableCount;
				}
				else
				{
					--blinkDisableCount;
				}
			}

			var enable = false;
			EditorGUILayout.Separator();

			EditorGUI.showMixedValue = 0 < blinkEnableCount && 0 < blinkDisableCount;
			EditorGUI.BeginChangeCheck();
			enable = EditorGUILayout.BeginToggleGroup("Enable(Blink)", 0<blinkEnableCount);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo("Enable(Blink)");
				foreach (var t in blendMode.targets)
				{
					RolePart.SetBlinkEnable(t as Material, enable);
				}
			}
			EditorGUI.showMixedValue = false;

			EditorGUI.showMixedValue = subTex.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			var tex = materialEditor.TextureProperty(subTex, Styles.subTex);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.subTex);
				subTex.textureValue = tex;
			}
			EditorGUI.showMixedValue = false;

			EditorGUI.showMixedValue = blinkSpeed.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			var v = materialEditor.RangeProperty(blinkSpeed, Styles.blinkSpeed);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.blinkSpeed);
				blinkSpeed.floatValue = v;
			}
			EditorGUI.showMixedValue = false;

			EditorGUI.showMixedValue = blinkMin.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			v = materialEditor.RangeProperty(blinkMin, Styles.blinkMin);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.blinkMin);
				blinkMin.floatValue = v;
			}
			EditorGUI.showMixedValue = false;

			EditorGUI.showMixedValue = blinkMax.hasMixedValue;
			EditorGUI.BeginChangeCheck();
			v = materialEditor.RangeProperty(blinkMax, Styles.blinkMax);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(Styles.blinkMax);
				blinkMax.floatValue = v;
			}
			EditorGUI.showMixedValue = false;

			EditorGUILayout.EndToggleGroup();
		}
		
		public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] props)
		{
			FindProperties (props);

			EditorGUI.BeginChangeCheck();

			BlendModePopup(materialEditor);

			EditorGUILayout.Separator();
			OnGUI_Custom(materialEditor);

			EditorGUILayout.Separator();
			base.OnGUI(materialEditor, props);

			EditorGUILayout.Separator();
			OnGUI_Blink(materialEditor);

			var log = "";
			if (null != RolePartMaterialManager.Me)
			{
				var testMat = materialEditor.target as Material;
				var info = RolePartMaterialManager.Me.materialList.Find(delegate(RolePartMaterialInfo obj) {
					return obj.originMat == testMat
						|| (null != obj.instanceUsedMatList && null != obj.instanceUsedMatList.Find(delegate(Material m) {
							return testMat == m;
						}));
				});
				if (null != info)
				{
					log = info.refCount.ToString();
				}
				else
				{
					log = "No Info";
				}
			}
			else
			{
				log = "No Manager";
			}
			EditorGUILayout.Separator();
			EditorGUILayout.LabelField(string.Format("Ref Count: {0}", log));

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
