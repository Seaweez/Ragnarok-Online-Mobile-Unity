using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class ShaderManager : SingleTonGO<ShaderManager> 
	{
		public static ShaderManager Instance{get{return Me;}}

		public Shader sceneObjectLit;
		public Shader sceneObjectUnlit;
		public Shader sceneObjectLitWind;
		public Shader sceneObjectUnlitWind;

		public Shader T4M_2;
		public Shader T4M_3;
		public Shader T4M_4;
		public Shader T4M_5;
		public Shader T4M_6;

		public Shader effectTransparent;
		public Shader effectTransparentAlwaysFront;

		public Shader skyboxCubemap;
		public Shader skyboxProcedural;
		public Shader skyboxProceduralEx;

		public Shader rolePart;
		public Shader rolePartOutline;

		[System.NonSerialized]
		private Shader[] sceneObjectShaders_ = null;
		public Shader[] sceneObjectShaders
		{
			get
			{
				if (sceneObjectShaders_.IsNullOrEmpty())
				{
					sceneObjectShaders_ = new Shader[]{
						sceneObjectLit,
						sceneObjectUnlit,
						sceneObjectLitWind,
						sceneObjectUnlitWind
					};
				}
				return sceneObjectShaders_;
			}
		}

		[System.NonSerialized]
		private Shader[] T4MShaders_ = null;
		public Shader[] T4MShaders
		{
			get
			{
				if (T4MShaders_.IsNullOrEmpty())
				{
					T4MShaders_ = new Shader[]{
						T4M_2,
						T4M_3,
						T4M_4,
						T4M_5,
						T4M_6
					};
				}
				return T4MShaders_;
			}
		}

		[System.NonSerialized]
		private Shader[] effectShaders_ = null;
		public Shader[] effectShaders
		{
			get
			{
				if (effectShaders_.IsNullOrEmpty())
				{
					effectShaders_ = new Shader[]{
						effectTransparent,
						effectTransparentAlwaysFront
					};
				}
				return effectShaders_;
			}
		}

		[System.NonSerialized]
		private Shader[] rolePartShaders_ = null;
		public Shader[] rolePartShaders
		{
			get
			{
				if (rolePartShaders_.IsNullOrEmpty())
				{
					rolePartShaders_ = new Shader[]{
						rolePart,
						rolePartOutline
					};
				}
				return rolePartShaders_;
			}
		}

		public enum SceneObjectBlendMode
		{
			Opaque,
			Cutout,
			Transparent,
			TransparentZWrite
		}
		public static void HandleSceneObjectMaterial(Material material)
		{
			if (!material.HasProperty("_Mode"))
			{
				return;
			}
			var blendMode = (SceneObjectBlendMode)material.GetFloat("_Mode");
			switch (blendMode)
			{
			case SceneObjectBlendMode.Opaque:
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
				material.SetInt("_ZWrite", 1);
				material.DisableKeyword("_ALPHATEST_ON");
//				material.DisableKeyword("_ALPHABLEND_ON");
//				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = -1;
				break;
			case SceneObjectBlendMode.Cutout:
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
				material.SetInt("_ZWrite", 1);
				material.EnableKeyword("_ALPHATEST_ON");
//				material.DisableKeyword("_ALPHABLEND_ON");
//				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 2450;
				break;
			case SceneObjectBlendMode.Transparent:
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
//				material.DisableKeyword("_ALPHABLEND_ON");
//				material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 3000;
				break;
			case SceneObjectBlendMode.TransparentZWrite:
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				material.SetInt("_ZWrite", 1);
				material.DisableKeyword("_ALPHATEST_ON");
//				material.DisableKeyword("_ALPHABLEND_ON");
//				material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 3000;
				break;
			}
		}
	
	}
} // namespace RO
