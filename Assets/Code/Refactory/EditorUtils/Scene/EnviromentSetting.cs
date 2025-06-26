using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Text;
using System.Collections.Generic;
using Ghost.Extensions;
using RO;

namespace EditorTool
{
	[System.Serializable]
	public class EnviromentLighting : ICloneable
	{
		public bool enable;

		public Color ambientEquatorColor;
		public Color ambientGroundColor;
		public float ambientIntensity;
		public Color ambientLight;
		public AmbientMode ambientMode = AmbientMode.Flat;
		public Color ambientSkyColor;
		public Cubemap customReflection;
		public DefaultReflectionMode defaultReflectionMode = DefaultReflectionMode.Skybox;
		public int defaultReflectionResolution;
		public int reflectionBounces;
		public float reflectionIntensity;

		public object Clone()
		{
			return MemberwiseClone();
		}
		
		public EnviromentLighting CloneSelf()
		{
			return MemberwiseClone() as EnviromentLighting;
		}

		public void Apply()
		{
			if (!enable)
			{
				return;
			}
			RenderSettings.ambientMode = ambientMode;
			if (AmbientMode.Skybox == ambientMode
			    || AmbientMode.Flat == ambientMode)
			{
				RenderSettings.ambientLight = ambientLight;
			}
			else if (AmbientMode.Trilight == ambientMode)
			{
				RenderSettings.ambientSkyColor = ambientSkyColor;
				RenderSettings.ambientEquatorColor = ambientEquatorColor;
				RenderSettings.ambientGroundColor = ambientGroundColor;
			}
			RenderSettings.ambientIntensity = ambientIntensity;
			RenderSettings.defaultReflectionMode = defaultReflectionMode;
			if (DefaultReflectionMode.Custom == defaultReflectionMode)
			{
				RenderSettings.customReflection = customReflection;
			}
			RenderSettings.defaultReflectionResolution = defaultReflectionResolution;
			RenderSettings.reflectionBounces = reflectionBounces;
			RenderSettings.reflectionIntensity = reflectionIntensity;
		}

		public static void Clear()
		{
			RenderSettings.ambientMode = AmbientMode.Flat;
			RenderSettings.skybox = null;
			RenderSettings.ambientLight = Color.white;
			RenderSettings.ambientIntensity = 1;

			RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
			RenderSettings.defaultReflectionResolution = 128;
			RenderSettings.reflectionBounces = 0;
			RenderSettings.reflectionIntensity = 0;
		}
	}

	[System.Serializable]
	public class EnviromentFog : ICloneable
	{
		public bool enable;

		public bool fog;
		public Color fogColor;
		public float fogDensity;
		public float fogEndDistance;
		public FogMode fogMode = FogMode.Linear;
		public float fogStartDistance;
		
		public object Clone()
		{
			return MemberwiseClone();
		}
		
		public EnviromentFog CloneSelf()
		{
			return MemberwiseClone() as EnviromentFog;
		}

		public void Apply()
		{
			if (!enable)
			{
				return;
			}
			RenderSettings.fog = fog;
			if (fog)
			{
				RenderSettings.fogColor = fogColor;
				RenderSettings.fogMode = fogMode;
				switch (fogMode)
				{
				case FogMode.Linear:
					RenderSettings.fogStartDistance = fogStartDistance;
					RenderSettings.fogEndDistance = fogEndDistance;
					break;
				case FogMode.Exponential:
				case FogMode.ExponentialSquared:
					RenderSettings.fogDensity = fogDensity;
					break;
				}
			}
		}

		public static void Clear()
		{
			RenderSettings.fog = false;
		}
	}

	[System.Serializable]
	public class EnviromentFlare : ICloneable
	{
		public bool enable;

		public float flareFadeSpeed;
		public float flareStrength;
		
		public object Clone()
		{
			return MemberwiseClone();
		}
		
		public EnviromentFlare CloneSelf()
		{
			return MemberwiseClone() as EnviromentFlare;
		}

		public void Apply()
		{
			if (!enable)
			{
				return;
			}
			RenderSettings.flareFadeSpeed = flareFadeSpeed;
			RenderSettings.flareStrength = flareStrength;
		}

		public static void Clear()
		{
			RenderSettings.flareFadeSpeed = 0;
			RenderSettings.flareStrength = 0;
		}
	}

	[System.Serializable]
	public class EnviromentSkybox : ICloneable
	{
		public bool enable;

		public enum Type
		{
			SolidColor,
			CubemapOnly,
			Procedural,
			ProceduralEx
		}
		public Type type = Type.SolidColor;

		public float sunSize;
		public float atmoshpereThickness;
		public Color skyTint;
		public Color ground;
		public float exposure;

		public Cubemap cubemap = null;
		public Cubemap cubemapAlpha = null;
		public float cubemapRotation = 0;
		public Color cubemapTint = Color.white;
		
		public object Clone()
		{
			return MemberwiseClone();
		}
		
		public EnviromentSkybox CloneSelf()
		{
			return MemberwiseClone() as EnviromentSkybox;
		}

		public void Apply(Camera skyboxCamera, Material skyboxMaterial)
		{
			if (!enable)
			{
				return;
			}
			if (Type.SolidColor == type)
			{
				skyboxCamera.clearFlags = CameraClearFlags.SolidColor;
				skyboxCamera.backgroundColor = skyTint;
			}
			else
			{
				skyboxCamera.clearFlags = CameraClearFlags.Skybox;

				var shaderManager = ShaderManager.Me;
				switch (type)
				{
				case Type.CubemapOnly:
					if (skyboxMaterial.shader != shaderManager.skyboxCubemap)
					{
						skyboxMaterial.shader = shaderManager.skyboxCubemap;
					}
					skyboxMaterial.SetTexture("_Tex", cubemap);
					skyboxMaterial.SetTexture("_TexAlpha", cubemapAlpha);
					skyboxMaterial.SetFloat("_Rotation", cubemapRotation);
					skyboxMaterial.SetColor("_Tint", cubemapTint);
					skyboxMaterial.SetFloat("_Exposure", exposure);
					if (null != cubemapAlpha)
					{
						skyboxMaterial.EnableKeyword("USETEXALPHA_ON");
					}
					else
					{
						skyboxMaterial.DisableKeyword("USETEXALPHA_ON");
					}
					break;
				case Type.Procedural:
					if (skyboxMaterial.shader != shaderManager.skyboxProcedural)
					{
						skyboxMaterial.shader = shaderManager.skyboxProcedural;
					}
					skyboxMaterial.SetFloat("_SunSize", sunSize);
					skyboxMaterial.SetFloat("_AtmosphereThickness", atmoshpereThickness);
					skyboxMaterial.SetColor("_SkyTint", skyTint);
					skyboxMaterial.SetColor("_GroundColor", ground);
					skyboxMaterial.SetFloat("_Exposure", exposure);
					break;
				case Type.ProceduralEx:
					if (skyboxMaterial.shader != shaderManager.skyboxProceduralEx)
					{
						skyboxMaterial.shader = shaderManager.skyboxProceduralEx;
					}
					skyboxMaterial.SetFloat("_SunSize", sunSize);
					skyboxMaterial.SetFloat("_AtmosphereThickness", atmoshpereThickness);
					skyboxMaterial.SetColor("_SkyTint", skyTint);
					skyboxMaterial.SetColor("_GroundColor", ground);
					skyboxMaterial.SetFloat("_Exposure", exposure);

					skyboxMaterial.SetTexture("_Tex", cubemap);
					skyboxMaterial.SetTexture("_TexAlpha", cubemapAlpha);
					skyboxMaterial.SetFloat("_Rotation", cubemapRotation);
					skyboxMaterial.SetColor("_TexTint", cubemapTint);
					if (null != cubemapAlpha)
					{
						skyboxMaterial.EnableKeyword("USETEXALPHA_ON");
					}
					else
					{
						skyboxMaterial.DisableKeyword("USETEXALPHA_ON");
					}
					break;
				}
			}
		}

		public static void Clear()
		{

		}
	}

//	[System.Serializable]
//	public class EnviromentSceneObject : ICloneable
//	{
//		public bool enable;
//
//		public Color lightColor = Color.white;
//		public float lightScale = 1;
//		
//		public object Clone()
//		{
//			return MemberwiseClone();
//		}
//		
//		public EnviromentSceneObject CloneSelf()
//		{
//			return MemberwiseClone() as EnviromentSceneObject;
//		}
//
//		public void Apply(Material[] sceneObjectMaterials)
//		{
//			if (!enable)
//			{
//				return;
//			}
//			for (int i = 0; i < sceneObjectMaterials.Length; ++i)
//			{
//				var mat = sceneObjectMaterials[i];
//				mat.SetColor("_LightColor", lightColor);
//				mat.SetFloat("_LightScale", lightScale);
//			}
//		}
//
//		public static void Clear()
//		{
//			
//		}
//	}

	public class EnviromentSetting : MonoBehaviour
	{
		public int ID;
		public Light sun;

		public EnviromentLighting lighting;
		public EnviromentFog fog;
		public EnviromentFlare flare;
		public EnviromentSkybox skybox;
//		public EnviromentSceneObject sceneObject;

		private Camera skyboxCamera = null;
//		private Material[] sceneObjectMaterials = null;
		private Material skyboxMaterial = null;

		public void ResetSkyboxCamera()
		{
			float minDepth = int.MaxValue;
			var cameras = Camera.allCameras;
			for (int i = 0; i < cameras.Length; ++i)
			{
				var c = cameras[i];
				if (c.depth < minDepth)
				{
					minDepth = c.depth;
					skyboxCamera = c;
				}
			}
		}
		
//		public void ResetSceneObjectsMaterial()
//		{
//			if (null == ShaderManager.Me)
//			{
//				new GameObject("ShaderManager", typeof(ShaderManager));
//			}
//			var shaderNames = new List<string> ();
//			foreach (Shader s in ShaderManager.Me.sceneObjectShaders)
//			{
//				shaderNames.Add (s.name);
//			}
//			foreach (Shader s in ShaderManager.Me.T4MShaders)
//			{
//				shaderNames.Add (s.name);
//			}
//			
//			if (0 >= shaderNames.Count) 
//			{
//				return;
//			}
//			
//			var materials = new List<Material> ();
//			
//			var objs = UnityEngine.Object.FindObjectsOfType<Renderer> ();
//			foreach (var obj in objs) 
//			{
//				if (obj.sharedMaterials.IsNullOrEmpty ()) 
//				{
//					continue;
//				}
//				for (int i = 0; i < obj.sharedMaterials.Length; ++i) 
//				{
//					var m = obj.sharedMaterials [i];
//					if (null == m) {
//						Logger.LogFormat ("<color=red>[Material is NULL]: </color>{0}", obj.name);
//						continue;
//					}
//					if (shaderNames.Contains(m.shader.name))
//					{
//						m = obj.materials[i];
//						ShaderManager.HandleSceneObjectMaterial(m);
//						materials.Add(m);
//					}
//				}
//			}
//			sceneObjectMaterials = materials.ToArray();
//		}

		public static void ClearLighting()
		{
			EnviromentLighting.Clear();
		}

		public static void ClearFog()
		{
			EnviromentFog.Clear();
		}

		public static void ClearFlare()
		{
			EnviromentFlare.Clear();
		}

		public static void ClearAll()
		{
			EnviromentLighting.Clear();
//			EnviromentFog.Clear();
			EnviromentFlare.Clear();
		}

		public void Apply()
		{
			if (null != lighting)
			{
				lighting.Apply();
			}
			if (null != fog)
			{
				fog.Apply();
			}
			if (null != flare)
			{
				flare.Apply();
			}
			if (null != skybox && null != skyboxCamera)
			{
				var skyboxRender = skyboxCamera.GetComponent<Skybox>();
				if (null == skyboxRender)
				{
					skyboxRender = skyboxCamera.gameObject.AddComponent<Skybox>();
				}
				if (null == skyboxMaterial)
				{
					skyboxMaterial = new Material(Shader.Find("RO/Clear"));
				}
				skyboxRender.material = skyboxMaterial;
				skybox.Apply(skyboxCamera, skyboxMaterial);
			}
//			if (null != sceneObject)
//			{
//				sceneObject.Apply(sceneObjectMaterials);
//			}
		}

		void Start()
		{
			ResetSkyboxCamera();
//			ResetSceneObjectsMaterial();
		}

		void OnDestroy()
		{
			if (null != skyboxMaterial)
			{
				Material.Destroy(skyboxMaterial);
				skyboxMaterial = null;
			}
		}
	}
} // namespace EditorTool
