using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RO;
using Ghost.Utils;

namespace EditorTool
{
	[CustomEditor(typeof(EnviromentSetting))]
	public class EnviromentSettingEditor : Editor
	{
		private static int[] reflectionResolutions = {
			128,
			256,
			512,
			1024
		};
		private static string[] reflectionResolutionStrs;
		static EnviromentSettingEditor()
		{
			reflectionResolutionStrs = new string[reflectionResolutions.Length];
			for (int i = 0; i < reflectionResolutions.Length; ++i)
			{
				reflectionResolutionStrs[i] = reflectionResolutions[i].ToString();
			}
		}
		
		public void OnInspectorGUI_Lighting (EnviromentLighting setting)
		{
			if (null == setting)
			{
				return;
			}
			setting.enable = EditorGUILayout.BeginToggleGroup("Enable (Ambient Lighting)", setting.enable);
			
			setting.ambientMode = (AmbientMode)EditorGUILayout.EnumPopup("Ambient Source", setting.ambientMode);
			switch (setting.ambientMode)
			{
			case AmbientMode.Skybox:
			case AmbientMode.Flat:
				setting.ambientLight = EditorGUILayout.ColorField("Ambient Color", setting.ambientLight);
				break;
			case AmbientMode.Trilight:
				setting.ambientSkyColor = EditorGUILayout.ColorField("Sky Color", setting.ambientSkyColor);
				setting.ambientEquatorColor = EditorGUILayout.ColorField("Equator Color", setting.ambientEquatorColor);
				setting.ambientGroundColor = EditorGUILayout.ColorField("Ground Color", setting.ambientGroundColor);
				break;
			}
			
			setting.ambientIntensity = EditorGUILayout.Slider("Ambient Intensity", setting.ambientIntensity, 0, 8);
			
			setting.defaultReflectionMode = (DefaultReflectionMode)EditorGUILayout.EnumPopup("Reflection Source", setting.defaultReflectionMode);
			switch (setting.defaultReflectionMode)
			{
			case DefaultReflectionMode.Skybox:
				break;
			case DefaultReflectionMode.Custom:
				setting.customReflection = EditorGUILayout.ObjectField("Cubemap", setting.customReflection, typeof(Cubemap), false) as Cubemap;
				break;
			}
			setting.defaultReflectionResolution = EditorGUILayout.IntPopup("Resolution", setting.defaultReflectionResolution, reflectionResolutionStrs, reflectionResolutions);
			setting.reflectionIntensity = EditorGUILayout.Slider("Reflection Intensity", setting.reflectionIntensity, 0f, 1f);
			setting.reflectionBounces = EditorGUILayout.IntSlider("Reflection Bounces", setting.reflectionBounces, 1, 5);
			
			EditorGUILayout.EndToggleGroup();
		}

		public void OnInspectorGUI_Fog (EnviromentFog setting)
		{
			if (null == setting)
			{
				return;
			}
			setting.enable = EditorGUILayout.BeginToggleGroup("Enable (Fog Setting)", setting.enable);
			
			setting.fog = EditorGUILayout.BeginToggleGroup("Fog", setting.fog);
			setting.fogColor = EditorGUILayout.ColorField("Fog Color", setting.fogColor);
			setting.fogMode = FogMode.Linear;
//			setting.fogMode = (FogMode)EditorGUILayout.EnumPopup("Fog Mode", setting.fogMode);
//			switch (setting.fogMode)
//			{
//			case FogMode.Linear:
				setting.fogStartDistance = EditorGUILayout.FloatField("Start", setting.fogStartDistance);
				setting.fogEndDistance = EditorGUILayout.FloatField("End", setting.fogEndDistance);
//				break;
//			case FogMode.Exponential:
//			case FogMode.ExponentialSquared:
//				setting.fogDensity = EditorGUILayout.FloatField("Density", setting.fogDensity);
//				break;
//			}
			EditorGUILayout.EndToggleGroup();
			
			EditorGUILayout.EndToggleGroup();
		}

		public void OnInspectorGUI_Flare (EnviromentFlare setting)
		{
			if (null == setting)
			{
				return;
			}
			setting.enable = EditorGUILayout.BeginToggleGroup("Enable (Flare)", setting.enable);
			
			setting.flareFadeSpeed = EditorGUILayout.FloatField("Flare Fade Speed", setting.flareFadeSpeed);
			setting.flareStrength = EditorGUILayout.Slider("Flare Strength", setting.flareStrength, 0f, 1f);
			
			EditorGUILayout.EndToggleGroup();
		}

		public void OnInspectorGUI_Skybox (EnviromentSkybox setting)
		{
			if (null == setting)
			{
				return;
			}
			setting.enable = EditorGUILayout.BeginToggleGroup("Enable (Skybox)", setting.enable);
			
			setting.type = (EnviromentSkybox.Type)EditorGUILayout.EnumPopup("Mode", setting.type);
			
			switch (setting.type)
			{
			case EnviromentSkybox.Type.SolidColor:
				setting.skyTint = EditorGUILayout.ColorField("Color", setting.skyTint);
				break;
			case EnviromentSkybox.Type.CubemapOnly:
				setting.cubemap = EditorGUILayout.ObjectField("Cubemap", setting.cubemap, typeof(Cubemap), false) as Cubemap;
				setting.cubemapAlpha = EditorGUILayout.ObjectField("Cubemap Alpha", setting.cubemapAlpha, typeof(Cubemap), false) as Cubemap;
				setting.cubemapRotation = EditorGUILayout.Slider("Cubemap Rotation", setting.cubemapRotation, 0, 360);
				setting.exposure = EditorGUILayout.Slider("Cubemap Exposure", setting.exposure, 0, 8);
				setting.cubemapTint = EditorGUILayout.ColorField("Cubemap Tint", setting.cubemapTint);
				break;
			case EnviromentSkybox.Type.Procedural:
				setting.sunSize = EditorGUILayout.Slider("Sun Size", setting.sunSize, 0, 1);
//				setting.sunRotation = EditorGUILayout.Vector3Field("Sun Rotation", setting.sunRotation);
//				setting.sunFlare = EditorGUILayout.ObjectField("Sun Flare", setting.sunFlare, typeof(Flare), false) as Flare;
				setting.atmoshpereThickness = EditorGUILayout.Slider("Atmoshpere Thickness", setting.atmoshpereThickness, 0, 5);
				setting.skyTint = EditorGUILayout.ColorField("Sky Tint", setting.skyTint);
				setting.ground = EditorGUILayout.ColorField("Ground", setting.ground);
				setting.exposure = EditorGUILayout.Slider("Exposure", setting.exposure, 0, 8);
				break;
			case EnviromentSkybox.Type.ProceduralEx:
				setting.sunSize = EditorGUILayout.Slider("Sun Size", setting.sunSize, 0, 1);
//				setting.sunRotation = EditorGUILayout.Vector3Field("Sun Rotation", setting.sunRotation);
//				setting.sunFlare = EditorGUILayout.ObjectField("Sun Flare", setting.sunFlare, typeof(Flare), false) as Flare;
				setting.atmoshpereThickness = EditorGUILayout.Slider("Atmoshpere Thickness", setting.atmoshpereThickness, 0, 5);
				setting.skyTint = EditorGUILayout.ColorField("Sky Tint", setting.skyTint);
				setting.ground = EditorGUILayout.ColorField("Ground", setting.ground);
				setting.exposure = EditorGUILayout.Slider("Exposure", setting.exposure, 0, 8);
				setting.cubemap = EditorGUILayout.ObjectField("Cubemap", setting.cubemap, typeof(Cubemap), false) as Cubemap;
				setting.cubemapAlpha = EditorGUILayout.ObjectField("Cubemap Alpha", setting.cubemapAlpha, typeof(Cubemap), false) as Cubemap;
				setting.cubemapRotation = EditorGUILayout.Slider("Cubemap Rotation", setting.cubemapRotation, 0, 360);
				setting.cubemapTint = EditorGUILayout.ColorField("Cubemap Tint", setting.cubemapTint);
				break;
			}
			
			EditorGUILayout.EndToggleGroup();
		}

//		public void OnInspectorGUI_SceneObject (EnviromentSceneObject setting)
//		{
//			if (null == setting)
//			{
//				return;
//			}
//			setting.enable = EditorGUILayout.BeginToggleGroup("Enable (SceneObject)", setting.enable);
//			
//			setting.lightColor = EditorGUILayout.ColorField("Light Color", setting.lightColor);
//			setting.lightScale = EditorGUILayout.Slider("Light Scale", setting.lightScale, 0, 3);
//			
//			EditorGUILayout.EndToggleGroup();
//		}

		public override void OnInspectorGUI ()
		{
			var setting = target as EnviromentSetting;

			setting.sun = EditorGUILayout.ObjectField("Skybox Sun", setting.sun, typeof(Light), true) as Light;

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.Separator();
			OnInspectorGUI_Lighting(setting.lighting);

			EditorGUILayout.Separator();
			OnInspectorGUI_Fog(setting.fog);

			EditorGUILayout.Separator();
			OnInspectorGUI_Flare(setting.flare);
			
			EditorGUILayout.Separator();
			OnInspectorGUI_Skybox(setting.skybox);
			
//			EditorGUILayout.Separator();
//			OnInspectorGUI_SceneObject(setting.sceneObject);

			var changed = EditorGUI.EndChangeCheck();

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.Separator();
			setting.ID = EditorGUILayout.IntField("ID", setting.ID);
			if (EditorGUI.EndChangeCheck())
			{
				setting.name = EditorToolUtils.FormatName(setting.name, setting.ID);
			}

			EditorGUILayout.Separator();
			if (Application.isPlaying)
			{
				if (GUILayout.Button("Apply") || changed)
				{
					setting.Apply();
				}

				EditorGUILayout.Separator();
				if (GUILayout.Button("Export"))
				{
					if (0 >= setting.ID)
					{
						EditorUtility.DisplayDialog("Invalid ID", setting.ID.ToString(), "OK");
					}
					else
					{
						var path = string.Format("Resources/Script/Refactory/Config/Enviroment/Enviroment_{0}.txt", setting.ID);
						if (ExportUtils.SaveAsset(path, Export(setting)))
						{
							var prefab = PrefabUtility.GetPrefabParent(setting);
							prefab = PrefabUtility.ReplacePrefab(setting.gameObject, prefab);
							EditorUtility.SetDirty(prefab);
							AssetDatabase.SaveAssets();
						}
					}
				}
			}
			else
			{
				if (GUILayout.Button("Clear Ambient Lighting"))
				{
					EnviromentSetting.ClearLighting();
				}
				if (GUILayout.Button("Clear Fog"))
				{
					EnviromentSetting.ClearFog();
				}
				if (GUILayout.Button("Clear Flare"))
				{
					EnviromentSetting.ClearFlare();
				}
				if (GUILayout.Button("Clear All"))
				{
					EnviromentSetting.ClearAll();
				}
			}
		}

		public void Export_Lighting(EnviromentLighting info, WriterAdapter writer)
		{
			if (!info.enable)
			{
				return;
			}
			
			writer.WriteMemberName("lighting");
			writer.WriteStructStart();
			
			writer.WriteMemberName("ambientMode");
			ExportUtils.ExportEnum(writer, info.ambientMode);
			
			switch (info.ambientMode)
			{
			case AmbientMode.Skybox:
			case AmbientMode.Flat:
				writer.WriteMemberName("ambientLight");
				ExportUtils.ExportColor(writer, info.ambientLight);
				break;
			case AmbientMode.Trilight:
				writer.WriteMemberName("ambientSkyColor");
				ExportUtils.ExportColor(writer, info.ambientSkyColor);
				
				writer.WriteMemberName("ambientEquatorColor");
				ExportUtils.ExportColor(writer, info.ambientEquatorColor);
				
				writer.WriteMemberName("ambientGroundColor");
				ExportUtils.ExportColor(writer, info.ambientGroundColor);
				break;
			}

			writer.WriteMemberName("ambientIntensity");
			writer.WriteMemberValue(info.ambientIntensity);
			
			writer.WriteMemberName("defaultReflectionMode");
			ExportUtils.ExportEnum(writer, info.defaultReflectionMode);
			
			switch (info.defaultReflectionMode)
			{
			case DefaultReflectionMode.Skybox:
				break;
			case DefaultReflectionMode.Custom:
				if (null != info.customReflection)
				{
					writer.WriteMemberName("customReflection");
					ExportUtils.ExportAsset(writer, info.customReflection);
				}
				break;
			}
			
			writer.WriteMemberName("defaultReflectionResolution");
			writer.WriteMemberValue(info.defaultReflectionResolution);
			
			writer.WriteMemberName("reflectionBounces");
			writer.WriteMemberValue(info.reflectionBounces);
			
			writer.WriteMemberName("reflectionIntensity");
			writer.WriteMemberValue(info.reflectionIntensity);
			
			writer.WriteStructEnd();
		}

		public void Export_Fog(EnviromentFog info, WriterAdapter writer)
		{
			
			if (!info.enable)
			{
				return;
			}
			
			writer.WriteMemberName("fog");
			writer.WriteStructStart();
			
			writer.WriteMemberName("fog");
			ExportUtils.ExportBoolean(writer, info.fog);
			if (info.fog)
			{
				writer.WriteMemberName("fogColor");
				ExportUtils.ExportColor(writer, info.fogColor);
				
				writer.WriteMemberName("fogMode");
				ExportUtils.ExportEnum(writer, info.fogMode);
				switch (info.fogMode)
				{
				case FogMode.Linear:
					writer.WriteMemberName("fogStartDistance");
					writer.WriteMemberValue(info.fogStartDistance);
					writer.WriteMemberName("fogEndDistance");
					writer.WriteMemberValue(info.fogEndDistance);
					break;
				case FogMode.Exponential:
				case FogMode.ExponentialSquared:
					writer.WriteMemberName("fogDensity");
					writer.WriteMemberValue(info.fogDensity);
					break;
				}
			}
			
			writer.WriteStructEnd();
		}
		
		public void Export_Flare(EnviromentFlare info, WriterAdapter writer)
		{
			if (!info.enable)
			{
				return;
			}
			
			writer.WriteMemberName("flare");
			writer.WriteStructStart();
			
			writer.WriteMemberName("flareFadeSpeed");
			writer.WriteMemberValue(info.flareFadeSpeed);
			
			writer.WriteMemberName("flareStrength");
			writer.WriteMemberValue(info.flareStrength);
			
			writer.WriteStructEnd();
		}
		
		public void DoExportCubemap(EnviromentSkybox info, WriterAdapter writer, Light sun, bool useExposure)
		{
			if (null != info.cubemap)
			{
				writer.WriteMemberName("cubemap");
				ExportUtils.ExportAsset(writer, info.cubemap);

				if (null != info.cubemapAlpha)
				{
					writer.WriteMemberName("cubemapAlpha");
					ExportUtils.ExportAsset(writer, info.cubemapAlpha);
				}

				writer.WriteMemberName("cubemapRotation");
				writer.WriteMemberValue(info.cubemapRotation);
				
				writer.WriteMemberName("cubemapTint");
				ExportUtils.ExportColor(writer, info.cubemapTint);

				if (useExposure)
				{
					writer.WriteMemberName("exposure");
					writer.WriteMemberValue(info.exposure);
				}
			}
		}
		
		public void DoExportProcedural(EnviromentSkybox info, WriterAdapter writer, Light sun)
		{
			if (null != sun)
			{
				writer.WriteMemberName("sunColor");
				ExportUtils.ExportColor(writer, sun.color);
				
				writer.WriteMemberName("sunIntensity");
				writer.WriteMemberValue(sun.intensity);
				
				writer.WriteMemberName("sunBounceIntensity");
				writer.WriteMemberValue(sun.bounceIntensity);
				
				writer.WriteMemberName("sunRotation");
				ExportUtils.ExportVector(writer, sun.transform.eulerAngles);
				
				if (null != sun.flare)
				{
					writer.WriteMemberName("sunFlare");
					ExportUtils.ExportAsset(writer, sun.flare);
				}
				
				writer.WriteMemberName("sunSize");
				writer.WriteMemberValue(info.sunSize);
			}
			writer.WriteMemberName("atmoshpereThickness");
			writer.WriteMemberValue(info.atmoshpereThickness);
			
			writer.WriteMemberName("skyTint");
			ExportUtils.ExportColor(writer, info.skyTint);
			
			writer.WriteMemberName("ground");
			ExportUtils.ExportColor(writer, info.ground);
			
			writer.WriteMemberName("exposure");
			writer.WriteMemberValue(info.exposure);
		}
		
		public void Export_Skybox(EnviromentSkybox info, WriterAdapter writer, Light sun)
		{
			if (!info.enable)
			{
				return;
			}
			
			writer.WriteMemberName("skybox");
			writer.WriteStructStart();
			
			writer.WriteMemberName("type");
			ExportUtils.ExportEnum(writer, info.type);
			
			if (EnviromentSkybox.Type.SolidColor == info.type)
			{
				writer.WriteMemberName("skyTint");
				ExportUtils.ExportColor(writer, info.skyTint);
			}
			else
			{
				switch (info.type)
				{
				case EnviromentSkybox.Type.CubemapOnly:
					DoExportCubemap(info, writer, sun, true);
					break;
				case EnviromentSkybox.Type.Procedural:
					DoExportProcedural(info, writer, sun);
					break;
				case EnviromentSkybox.Type.ProceduralEx:
					DoExportProcedural(info, writer, sun);
					DoExportCubemap(info, writer, sun, false);
					break;
				}
			}
			
			writer.WriteStructEnd();
		}

//		public void Export_SceneObject(EnviromentSceneObject info, WriterAdapter writer)
//		{
//			if (!info.enable)
//			{
//				return;
//			}
//			
//			writer.WriteMemberName("sceneObject");
//			writer.WriteStructStart();
//			
//			writer.WriteMemberName("lightColor");
//			ExportUtils.ExportColor(writer, info.lightColor);
//			
//			writer.WriteMemberName("lightScale");
//			writer.WriteMemberValue(info.lightScale);
//			
//			writer.WriteStructEnd();
//		}

		public string Export(EnviromentSetting info)
		{
			var name = string.Format("Enviroment_{0}", info.ID);
			StringBuilder sb = new StringBuilder ();
			var writer = DataWriter.GetAdapter(new LuaWriter (sb), name, true);
			
			writer.WriteStructStart();
			
			if (null != info.lighting)
			{
				Export_Lighting(info.lighting, writer);
			}
			if (null != info.fog)
			{
				Export_Fog(info.fog, writer);
			}
			if (null != info.flare)
			{
				Export_Flare(info.flare, writer);
			}
			if (null != info.skybox)
			{
				Export_Skybox(info.skybox, writer, info.sun);
			}
//			if (null != info.sceneObject)
//			{
//				Export_SceneObject(info.sceneObject, writer);
//			}
			
			writer.WriteStructEnd();
			
			sb.AppendLine();
			sb.AppendFormat("return {0}", name);
			return sb.ToString();
		}
	}

} // namespace EditorTool
