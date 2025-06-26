using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public partial class RolePart
	{
		#region material
		[SLua.CustomLuaClassAttribute]
		public enum BlendMode
		{
			Opaque,
			Transparent
		}

		public static void CopyMaterialRunValues(Material newMat, Material oldMat)
		{
			if (null == newMat || null == oldMat)
			{
				return;
			}
			var renderMode = RolePart.GetBlendMode(oldMat);
			var alpha = RolePart.GetAlpha(oldMat);
			var changeColorEnable = RolePart.IsChangeColorEnable(oldMat);
			var changeColor = RolePart.GetChangeColor(oldMat);
			
			RolePart.SetBlendMode_ForRun(newMat, renderMode);
			RolePart.SetAlpha(newMat, alpha);
			RolePart.SetChangeColorEnable(newMat, changeColorEnable);
			RolePart.SetChangeColor(newMat, changeColor);
		}
		
		public static BlendMode GetBlendMode(Material material)
		{
			return (BlendMode)material.GetInt("_Mode");
		}

		public static void SetBlendMode_ForRun(Material material, BlendMode blendMode)
		{
			var oldBlendMode = GetBlendMode(material);
			if (oldBlendMode == blendMode)
			{
				return;
			}
			material.SetInt("_Mode", (int)blendMode);
			switch (blendMode)
			{
			case BlendMode.Opaque:
				SetBlendMode(material, UnityEngine.Rendering.BlendMode.One, UnityEngine.Rendering.BlendMode.Zero, -1);
				material.SetInt("_ZWrite", 1);
				material.DisableKeyword("_Alpha_ON");
				break;
			case BlendMode.Transparent:
				SetBlendMode(material, UnityEngine.Rendering.BlendMode.SrcAlpha, UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha, 3000);
//				material.SetInt("_ZWrite", 1);
				material.EnableKeyword("_Alpha_ON");
				break;
			}
		}
		
		public static void SetBlendMode(Material material, BlendMode blendMode)
		{
			material.SetInt("_Mode", (int)blendMode);
			switch (blendMode)
			{
			case BlendMode.Opaque:
				SetBlendMode(material, UnityEngine.Rendering.BlendMode.One, UnityEngine.Rendering.BlendMode.Zero, -1);
				material.SetInt("_ZWrite", 1);
				material.DisableKeyword("_Alpha_ON");
				break;
			case BlendMode.Transparent:
				SetBlendMode(material, UnityEngine.Rendering.BlendMode.SrcAlpha, UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha, 3000);
				material.EnableKeyword("_Alpha_ON");
				break;
			}
		}
		
		public static void SetBlendMode(Material material, UnityEngine.Rendering.BlendMode src, UnityEngine.Rendering.BlendMode dest, int renderQueue)
		{
			material.SetInt("_SrcBlend", (int)src);
			material.SetInt("_DstBlend", (int)dest);
			material.renderQueue = renderQueue;
		}
		
		public static void SetToonLightColor(Material material, Color color, int n)
		{
			material.SetColor(string.Format("_LightColor{0}", n), color);
		}
		public static Color GetToonLightColor(Material material, int n)
		{
			return material.GetColor(string.Format("_LightColor{0}", n));
		}
		public static void SetToonLightExposure(Material material, float exposure, int n)
		{
			material.SetFloat(string.Format("_Exposure{0}", n), exposure);
		}
		public static float GetToonLightExposure(Material material, int n)
		{
			return material.GetFloat(string.Format("_Exposure{0}", n));
		}
		
		public static void SetAlpha(Material material, float alpha)
		{
			material.SetFloat("_Alpha", alpha);
		}
		public static float GetAlpha(Material material)
		{
			return material.GetFloat("_Alpha");
		}
		
		public static void SetChangeColor(Material material, Color color)
		{
			material.SetColor("_ChangeColor", color);
		}
		public static Color GetChangeColor(Material material)
		{
			return material.GetColor("_ChangeColor");
		}

		public static void SetMaskColor(Material material, Color color)
		{
			material.SetColor("_MaskColor", color);
		}
		public static Color GetMaskColor(Material material)
		{
			return material.GetColor("_MaskColor");
		}

		public static void SetMultiColorNumber(Material material, int n)
		{
			material.SetInt("_MultiColorNumber", n);
		}
		public static int GetMultiColorNumber(Material material)
		{
			return material.GetInt("_MultiColorNumber");
		}
		
		public static void SetKeywordEnable(Material material, bool enable, string keyword)
		{
			if (enable)
			{
				material.EnableKeyword(keyword);
			}
			else
			{
				material.DisableKeyword(keyword);
			}
		}
		
		public static bool IsChangeColorEnable(Material material)
		{
			return material.IsKeywordEnabled("_ChangeColor_ON");
		}
		public static void SetChangeColorEnable(Material material, bool enable)
		{
			SetKeywordEnable(material, enable, "_ChangeColor_ON");
		}
		
		public static bool IsMaskColorEnable(Material material)
		{
			return material.IsKeywordEnabled("_MaskColor_ON");
		}
		public static void SetMaskColorEnable(Material material, bool enable)
		{
			SetKeywordEnable(material, enable, "_MaskColor_ON");
		}

        public static bool IsMask2ColorEnable(Material material)
        {
            return material.IsKeywordEnabled("_Mask2Color_ON");
        }
        public static void SetMask2ColorEnable(Material material, bool enable)
        {
            SetKeywordEnable(material, enable, "_Mask2Color_ON");
        }

        public static bool IsSATEnable(Material material)
        {
            return material.IsKeywordEnabled("_SAT_ON");
        }
        public static void SetSATEnable(Material material, bool enable)
        {
            SetKeywordEnable(material, enable, "_SAT_ON");
        }

        public static bool IsToonEnable(Material material)
		{
			return material.IsKeywordEnabled("_ToonLight_ON");
		}
		public static void SetToonEnable(Material material, bool enable)
		{
			SetKeywordEnable(material, enable, "_ToonLight_ON");
		}

		public static bool IsCutoutEnable(Material material)
		{
			return material.IsKeywordEnabled("_ALPHATEST_ON");
		}
		public static void SetCutoutEnable(Material material, bool enable)
		{
			SetKeywordEnable(material, enable, "_ALPHATEST_ON");
		}

		public static bool IsBlinkEnable(Material material)
		{
			return material.IsKeywordEnabled("_BLINK_ON");
		}
		public static void SetBlinkEnable(Material material, bool enable)
		{
			SetKeywordEnable(material, enable, "_BLINK_ON");
		}
		#endregion material

	}
} // namespace RO
