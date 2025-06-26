using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(RenderSettingsSetter))]
	public class RenderSettingsSetterEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			var setter = target as RenderSettingsSetter;
			
			setter.skybox = EditorGUILayout.ObjectField("Skybox", setter.skybox, typeof(Material), true) as Material;

			setter.ambientMode = (AmbientMode)EditorGUILayout.EnumPopup("Ambient Source", setter.ambientMode);
			switch (setter.ambientMode)
			{
			case AmbientMode.Skybox:
			case AmbientMode.Flat:
				setter.ambientLight = EditorGUILayout.ColorField("Ambient Color", setter.ambientLight);
				break;
			case AmbientMode.Trilight:
				setter.ambientSkyColor = EditorGUILayout.ColorField("Sky Color", setter.ambientSkyColor);
				setter.ambientEquatorColor = EditorGUILayout.ColorField("Equator Color", setter.ambientEquatorColor);
				setter.ambientGroundColor = EditorGUILayout.ColorField("Ground Color", setter.ambientGroundColor);
				break;
			}
			
			setter.ambientIntensity = EditorGUILayout.FloatField("Ambient Intensity", setter.ambientIntensity);
			
			setter.defaultReflectionMode = (DefaultReflectionMode)EditorGUILayout.EnumPopup("Reflection Source", setter.defaultReflectionMode);
			switch (setter.defaultReflectionMode)
			{
			case DefaultReflectionMode.Skybox:
				break;
			case DefaultReflectionMode.Custom:
				setter.customReflection = EditorGUILayout.ObjectField("Cubemap", setter.customReflection, typeof(Cubemap), true) as Cubemap;
				break;
			}
			setter.defaultReflectionResolution = EditorGUILayout.IntField("Resolution", setter.defaultReflectionResolution);
			setter.reflectionIntensity = EditorGUILayout.Slider("Reflection Intensity", setter.reflectionIntensity, 0f, 1f);
			setter.reflectionBounces = EditorGUILayout.IntSlider("Reflection Bounces", setter.reflectionBounces, 1, 5);
			
			setter.haloStrength = EditorGUILayout.Slider("Halo Strength", setter.haloStrength, 0f, 1f);
			
			setter.flareFadeSpeed = EditorGUILayout.FloatField("Flare Fade Speed", setter.flareFadeSpeed);
			setter.flareStrength = EditorGUILayout.Slider("Flare Strength", setter.flareStrength, 0f, 1f);
			
			setter.fog = EditorGUILayout.ToggleLeft("Fog", setter.fog);
			if (setter.fog)
			{
				setter.fogColor = EditorGUILayout.ColorField("Fog Color", setter.fogColor);
				setter.fogMode = (FogMode)EditorGUILayout.EnumPopup("Fog Mode", setter.fogMode);
				switch (setter.fogMode)
				{
				case FogMode.Linear:
					setter.fogStartDistance = EditorGUILayout.FloatField("Start", setter.fogStartDistance);
					setter.fogEndDistance = EditorGUILayout.FloatField("End", setter.fogEndDistance);
					break;
				case FogMode.Exponential:
				case FogMode.ExponentialSquared:
					setter.fogDensity = EditorGUILayout.FloatField("Density", setter.fogDensity);
					break;
				}
			}
			
			EditorGUILayout.Separator();
			setter.update = EditorGUILayout.ToggleLeft("Update", setter.update);

			EditorGUILayout.Separator();
			if (GUILayout.Button("Read"))
			{
				setter.Get();
			}

			EditorGUILayout.Separator();
			if (GUILayout.Button("Write"))
			{
				setter.Set();
			}
		}
	}
} // namespace EditorTool
