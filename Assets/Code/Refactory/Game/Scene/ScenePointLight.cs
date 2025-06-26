using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;
using Ghost.Utils;

namespace RO
{
	public class ScenePointLight : MonoBehaviour 
	{
		public Renderer[] renderers;

		public Color color = Color.white;
		public float intensity = 1;

		private Dictionary<Material, Material> orig_inst = new Dictionary<Material, Material>();
		private Dictionary<Material, Material> inst_orig = new Dictionary<Material, Material>();

		private bool _usingInstance = false;
		public bool usingInstance
		{
			get
			{
				return _usingInstance;
			}
			set
			{
				if (value == usingInstance)
				{
					return;
				}
				_usingInstance = value;
				if (usingInstance)
				{
					UseInistanceMaterials();
				}
				else
				{
					UseOriginalMaterials();
				}
			}
		}

		public bool on
		{
			get
			{
				return 0 < intensity;
			}
		}

		private void CreateInistanceMaterials()
		{
			var position = transform.position;
			var shaderManager = ShaderManager.Me;
			for (int i = 0; i < renderers.Length; ++i)
			{
				var r = renderers[i];
				if (null != r)
				{
					var mats = r.sharedMaterials;
					if (!mats.IsNullOrEmpty())
					{
						for (int j = 0; j < mats.Length; ++j)
						{
							var mat = mats[j];
							if (null != mat && !orig_inst.ContainsKey(mat))
							{
								var shader = mat.shader;
								if (shaderManager.sceneObjectLit == shader || 
								    shaderManager.sceneObjectLitWind == shader)
								{
									var instMat = Material.Instantiate<Material>(mat);
									instMat.EnableKeyword("_PointLight_ON");
									instMat.SetColor("_PointLightColor", color);
									instMat.SetFloat("_PointLightIntensity", intensity);
									instMat.SetVector("_PointLightPos", position);
									orig_inst.Add(mat, instMat);
									inst_orig.Add(instMat, mat);
								}
							}
						}
					}
				}
			}
		}
		private void DestroyInistanceMaterials()
		{
			UseOriginalMaterials();
			foreach (var key_value in inst_orig)
			{
				Material.Destroy(key_value.Key);
			}
			orig_inst.Clear();
			inst_orig.Clear();
		}

		private void UseOriginalMaterials()
		{
			for (int i = 0; i < renderers.Length; ++i)
			{
				var r = renderers[i];
				if (null != r)
				{
					var mats = r.sharedMaterials;
					if (!mats.IsNullOrEmpty())
					{
						var changed = false;
						for (int j = 0; j < mats.Length; ++j)
						{
							var mat = mats[j];
							Material origMat;
							if (null != mat && inst_orig.TryGetValue(mat, out origMat))
							{
								changed = true;
								mats[j] = origMat;
							}
						}
						if (changed)
						{
							r.sharedMaterials = mats;
						}
					}
				}
			}
		}
		private void UseInistanceMaterials()
		{
			for (int i = 0; i < renderers.Length; ++i)
			{
				var r = renderers[i];
				if (null != r)
				{
					var mats = r.sharedMaterials;
					if (!mats.IsNullOrEmpty())
					{
						var changed = false;
						for (int j = 0; j < mats.Length; ++j)
						{
							var mat = mats[j];
							Material instMat;
							if (null != mat && orig_inst.TryGetValue(mat, out instMat))
							{
								changed = true;
								mats[j] = instMat;
							}
						}
						if (changed)
						{
							r.sharedMaterials = mats;
						}
					}
				}
			}
		}

		public void Refresh()
		{
			usingInstance = on;
			if (usingInstance)
			{
				var position = transform.position;
				foreach (var key_value in inst_orig)
				{
					var instMat = key_value.Key;
					instMat.SetColor("_PointLightColor", color);
					instMat.SetFloat("_PointLightIntensity", intensity);
					instMat.SetVector("_PointLightPos", position);
				}
			}
		}

		#region behaviour
		void Start()
		{
			if (renderers.IsNullOrEmpty())
			{
				return;
			}
			CreateInistanceMaterials();
		}

		void OnDestroy()
		{
			DestroyInistanceMaterials();
		}

#if UNITY_EDITOR
		void Update()
		{
			Refresh();
		}
#endif
		#endregion behaviour
	}
} // namespace RO
