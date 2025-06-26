using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

namespace RO
{
	[ExecuteInEditMode]
	public class RenderSettingsSetter : MonoBehaviour 
	{
		public Color ambientEquatorColor;
		public Color ambientGroundColor;
		public float ambientIntensity;
		public Color ambientLight;
		public AmbientMode ambientMode;
		public SphericalHarmonicsL2 ambientProbe;
		public Color ambientSkyColor;
		public Cubemap customReflection;
		public DefaultReflectionMode defaultReflectionMode;
		public int defaultReflectionResolution;
		public float flareFadeSpeed;
		public float flareStrength;
		public bool fog;
		public Color fogColor;
		public float fogDensity;
		public float fogEndDistance;
		public FogMode fogMode;
		public float fogStartDistance;
		public float haloStrength;
		public int reflectionBounces;
		public float reflectionIntensity;
		public Material skybox;

		public bool update = false;

		public void Get()
		{
			skybox = RenderSettings.skybox;
			ambientMode = RenderSettings.ambientMode;
			ambientLight = RenderSettings.ambientLight;
			ambientSkyColor = RenderSettings.ambientSkyColor;
			ambientEquatorColor = RenderSettings.ambientEquatorColor;
			ambientGroundColor = RenderSettings.ambientGroundColor;
			ambientIntensity = RenderSettings.ambientIntensity;
			defaultReflectionMode = RenderSettings.defaultReflectionMode;
			customReflection = (Cubemap)RenderSettings.customReflection;
			defaultReflectionResolution = RenderSettings.defaultReflectionResolution;
			reflectionBounces = RenderSettings.reflectionBounces;
			reflectionIntensity = RenderSettings.reflectionIntensity;
			haloStrength = RenderSettings.haloStrength;
			flareFadeSpeed = RenderSettings.flareFadeSpeed;
			flareStrength = RenderSettings.flareStrength;
			fog = RenderSettings.fog;
			fogColor = RenderSettings.fogColor;
			fogMode = RenderSettings.fogMode;
			fogStartDistance = RenderSettings.fogStartDistance;
			fogEndDistance = RenderSettings.fogEndDistance;
			fogDensity = RenderSettings.fogDensity;

			ambientProbe = RenderSettings.ambientProbe;
		}

		public void Set()
		{
			RenderSettings.skybox = skybox;

			RenderSettings.ambientMode = ambientMode;
			switch (ambientMode)
			{
			case AmbientMode.Skybox:
			case AmbientMode.Flat:
				RenderSettings.ambientLight = ambientLight;
				break;
			case AmbientMode.Trilight:
				RenderSettings.ambientSkyColor = ambientSkyColor;
				RenderSettings.ambientEquatorColor = ambientEquatorColor;
				RenderSettings.ambientGroundColor = ambientGroundColor;
				break;
			}

			RenderSettings.ambientIntensity = ambientIntensity;
			
			RenderSettings.defaultReflectionMode = defaultReflectionMode;
			switch (defaultReflectionMode)
			{
			case DefaultReflectionMode.Skybox:
				break;
			case DefaultReflectionMode.Custom:
				RenderSettings.customReflection = customReflection;
				break;
			}
			RenderSettings.defaultReflectionResolution = defaultReflectionResolution;
			RenderSettings.reflectionBounces = reflectionBounces;
			RenderSettings.reflectionIntensity = reflectionIntensity;

			RenderSettings.haloStrength = haloStrength;

			RenderSettings.flareFadeSpeed = flareFadeSpeed;
			RenderSettings.flareStrength = flareStrength;

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

			RenderSettings.ambientProbe = ambientProbe;
		}

		void Awake()
		{
			Set();
		}

		void Update()
		{
			if (!update)
			{
				return;
			}
			Set();
		}
	}
} // namespace RO
