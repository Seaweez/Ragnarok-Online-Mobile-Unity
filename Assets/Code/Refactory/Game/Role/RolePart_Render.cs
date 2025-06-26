using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	interface MaterialHandler
	{
		void Handle(Material m, int i, int j);
	}

	class MaterialHandler_CreateInstance : MaterialHandler
	{
		public RolePartMaterialManager matManager;
		public Material[][] instanceMaterials;
		public Material[][] tempInstanceMaterials;

		public void Handle(Material m, int i, int j)
		{
			instanceMaterials[i][j] = matManager.CreateMaterialInstance(m);
			if (null != tempInstanceMaterials[i] && null == tempInstanceMaterials[i][j])
			{
				tempInstanceMaterials[i][j] = instanceMaterials[i][j];
			}
		}
	}

	class MaterialHandler_DestroyOrigin : MaterialHandler
	{
		public RolePartMaterialManager matManager;
		
		public void Handle(Material m, int i, int j)
		{
			if (null != m)
			{
				matManager.UnregisterMaterial(m);
			}
		}
	}

	class MaterialHandler_DestroyInstance : MaterialHandler
	{
		public RolePartMaterialManager matManager;
		public Material[][] originMaterials;
		public Material[][] instanceMaterials;
		
		public void Handle(Material m, int i, int j)
		{
			if (null != m)
			{
				if (null != matManager)
				{
					matManager.DestroyInstance(originMaterials[i][j], m);
				}
				else
				{
					Material.Destroy(m);
				}
				instanceMaterials[i][j] = null;
			}
		}
	}

	class MaterialHandler_SetBelendMode : MaterialHandler
	{
		public RolePart.BlendMode blendMode;
		
		public void Handle(Material m, int i, int j)
		{
			if (null != m)
			{
				RolePart.SetBlendMode_ForRun(m, blendMode);
			}
		}
	}

	class MaterialHandler_SetToonLightColor : MaterialHandler
	{
		public Color color;
		public int n;

		public void Handle(Material m, int i, int j)
		{
			if (null != m)
			{
				RolePart.SetToonLightColor(m, color, n);
			}
		}
	}

	class MaterialHandler_SetToonLightExposure : MaterialHandler
	{
		public float exposure;
		public int n;
		
		public void Handle(Material m, int i, int j)
		{
			if (null != m)
			{
				RolePart.SetToonLightExposure(m, exposure, n);
			}
		}
	}

	class MaterialHandler_SetChangeColor : MaterialHandler
	{
		public Color color;
		
		public void Handle(Material m, int i, int j)
		{
			if (null != m)
			{
				RolePart.SetChangeColor(m, color);
			}
		}
	}
	
	class MaterialHandler_SetAlpha : MaterialHandler
	{
		public float alpha;
		
		public void Handle(Material m, int i, int j)
		{
			if (null != m)
			{
				RolePart.SetAlpha(m, alpha);
			}
		}
	}

	class MaterialHandler_SetKeywordEnable : MaterialHandler
	{
		public string key;
		public bool enable;
		
		public void Handle(Material m, int i, int j)
		{
			if (null != m)
			{
				RolePart.SetKeywordEnable(m, enable, key);
			}
		}
	}

	public partial class RolePart
	{
		public SkinnedMeshRenderer mainSMR;
		public MeshRenderer mainMR;
		public Material[] materials;

		[System.NonSerialized]
		public Material[][] smrOriginMaterials;
		[System.NonSerialized]
		public Material[][] smrInstanceMaterials;
		private Material[][] tempSMRInstanceMaterials;
		
		[System.NonSerialized]
		public Material[][] mrOriginMaterials;
		[System.NonSerialized]
		public Material[][] mrInstanceMaterials;
		private Material[][] tempMRInstanceMaterials;
		
		public bool usingInstanceMaterials{get;private set;}

		static void HandleMaterials(Material[][] materials, MaterialHandler handler)
		{
			if (!materials.IsNullOrEmpty())
			{
				for (int i = 0; i < materials.Length; ++i)
				{
					var mats = materials[i];
					if (!mats.IsNullOrEmpty())
					{
						for (int j = 0; j < mats.Length; ++j)
						{
							var m = mats[j];
							handler.Handle(m, i, j);
						}
					}
				}
			}
		}

		static void HandleMaterials(Material[][] materials1, Material[][] materials2, MaterialHandler handler)
		{
			HandleMaterials(materials1, handler);
			HandleMaterials(materials2, handler);
		}

		static MaterialHandler_CreateInstance matHandler_CreateInstance = new MaterialHandler_CreateInstance();
		static MaterialHandler_DestroyOrigin matHandler_DestroyOrigin = new MaterialHandler_DestroyOrigin();
		static MaterialHandler_DestroyInstance matHandler_DestroyInstance = new MaterialHandler_DestroyInstance();
		static MaterialHandler_SetBelendMode matHandler_SetBlendMode = new MaterialHandler_SetBelendMode();
		static MaterialHandler_SetToonLightColor matHandler_SetToonLightColor = new MaterialHandler_SetToonLightColor();
		static MaterialHandler_SetToonLightExposure matHandler_SetToonLightExposure = new MaterialHandler_SetToonLightExposure();
		static MaterialHandler_SetChangeColor matHandler_SetChangeColor = new MaterialHandler_SetChangeColor();
		static MaterialHandler_SetAlpha matHandler_SetAlpha = new MaterialHandler_SetAlpha();
		static MaterialHandler_SetKeywordEnable matHandler_SetKeywordEnable = new MaterialHandler_SetKeywordEnable();

		private void CreateInstanceMaterials()
		{
			if (null == RolePartMaterialManager.Me)
			{
				return;
			}
			matHandler_CreateInstance.matManager = RolePartMaterialManager.Me;

			matHandler_CreateInstance.instanceMaterials = smrInstanceMaterials;
			matHandler_CreateInstance.tempInstanceMaterials = tempSMRInstanceMaterials;
			HandleMaterials(smrOriginMaterials, matHandler_CreateInstance);

			matHandler_CreateInstance.instanceMaterials = mrInstanceMaterials;
			matHandler_CreateInstance.tempInstanceMaterials = tempMRInstanceMaterials;
			HandleMaterials(mrOriginMaterials, matHandler_CreateInstance);
		}
		
		protected virtual void DestroyOriginMaterials()
		{
			if (null == RolePartMaterialManager.Me)
			{
				return;
			}
			matHandler_DestroyOrigin.matManager = RolePartMaterialManager.Me;
			HandleMaterials(smrOriginMaterials, mrOriginMaterials, matHandler_DestroyOrigin);

			var matManager = RolePartMaterialManager.Me;
			for (int i = 0; i < materials.Length; ++i)
			{
				var m = materials[i];
				if (null != m)
				{
					matManager.UnregisterMaterial(m);
				}
			}
		}
		
		private void DestroyInstanceMaterials()
		{
			matHandler_DestroyInstance.matManager = RolePartMaterialManager.Me;
			
			matHandler_DestroyInstance.originMaterials = smrOriginMaterials;
			matHandler_DestroyInstance.instanceMaterials = smrInstanceMaterials;
			HandleMaterials(smrInstanceMaterials, matHandler_DestroyInstance);
			
			matHandler_DestroyInstance.originMaterials = mrOriginMaterials;
			matHandler_DestroyInstance.instanceMaterials = mrInstanceMaterials;
			HandleMaterials(mrInstanceMaterials, matHandler_DestroyInstance);
		}

		public bool SwitchColor(int i)
		{
			if (null == materials || !materials.CheckIndex(i))
			{
				return false;
			}
			var mat = materials[i];
			if (null != mainSMR)
			{
				ReplaceOriginMaterial(mat, mainSMR, 0);
			}
			else if (null != mainMR)
			{
				ReplaceOriginMaterial(mat, mainMR, 0);
			}
			return true;
		}

		public void SetBlendMode(BlendMode blendMode)
		{
			matHandler_SetBlendMode.blendMode = blendMode;
			HandleMaterials(smrInstanceMaterials, mrInstanceMaterials, matHandler_SetBlendMode);
		}

		public void SetChangeColorEnable(bool enable)
		{
			matHandler_SetKeywordEnable.key = "_ChangeColor_ON";
			matHandler_SetKeywordEnable.enable = enable;
			HandleMaterials(smrInstanceMaterials, mrInstanceMaterials, matHandler_SetKeywordEnable);
		}
		public void SetMaskColorEnable(bool enable)
		{
			matHandler_SetKeywordEnable.key = "_MaskColor_ON";
			matHandler_SetKeywordEnable.enable = enable;
			HandleMaterials(smrInstanceMaterials, mrInstanceMaterials, matHandler_SetKeywordEnable);
		}

        public void SetMask2ColorEnable(bool enable)
        {
            matHandler_SetKeywordEnable.key    = "_Mask2Color_ON";
            matHandler_SetKeywordEnable.enable = enable;
            HandleMaterials(smrInstanceMaterials, mrInstanceMaterials, matHandler_SetKeywordEnable);
        }

        public void SetSATEnable(bool enable)
        {
            matHandler_SetKeywordEnable.key = "_SAT_ON";
            matHandler_SetKeywordEnable.enable = enable;
            HandleMaterials(smrInstanceMaterials, mrInstanceMaterials, matHandler_SetKeywordEnable);
        }

        public void SetToonEnable(bool enable)
		{
			matHandler_SetKeywordEnable.key = "_ToonLight_ON";
			matHandler_SetKeywordEnable.enable = enable;
			HandleMaterials(smrInstanceMaterials, mrInstanceMaterials, matHandler_SetKeywordEnable);
		}
        public void SetToonLightColor(Color color, int n)
		{
			if (!usingInstanceMaterials)
			{
				return;
			}
			matHandler_SetToonLightColor.color = color;
			matHandler_SetToonLightColor.n = n;
			HandleMaterials(smrInstanceMaterials, mrInstanceMaterials, matHandler_SetToonLightColor);
		}
		public void SetToonLightExposure(float exposure, int n)
		{
			if (!usingInstanceMaterials)
			{
				return;
			}
			matHandler_SetToonLightExposure.exposure = exposure;
			matHandler_SetToonLightExposure.n = n;
			HandleMaterials(smrInstanceMaterials, mrInstanceMaterials, matHandler_SetToonLightExposure);
		}
		public void SetAlpha(float alpha)
		{
			if (!usingInstanceMaterials)
			{
				return;
			}
			matHandler_SetAlpha.alpha = alpha;
			HandleMaterials(smrInstanceMaterials, mrInstanceMaterials, matHandler_SetAlpha);
		}
		public void SetChangeColor(Color color)
		{
			if (!usingInstanceMaterials)
			{
				return;
			}
			matHandler_SetChangeColor.color = color;
			HandleMaterials(smrInstanceMaterials, mrInstanceMaterials, matHandler_SetChangeColor);
		}
		
		public void UseOriginMaterials()
		{
			if (!usingInstanceMaterials)
			{
				return;
			}
			usingInstanceMaterials = false;
			
			if (!smrs.IsNullOrEmpty())
			{
				for (int i = 0; i < smrs.Length; ++i)
				{
					var r = smrs[i];
					r.sharedMaterials = smrOriginMaterials[i];
				}
			}
			
			if (!mrs.IsNullOrEmpty())
			{
				for (int i = 0; i < mrs.Length; ++i)
				{
					var r = mrs[i];
					r.sharedMaterials = mrOriginMaterials[i];
				}
			}
			DestroyInstanceMaterials();
		}
		
		public void UseInstanceMaterials()
		{
			if (usingInstanceMaterials)
			{
				return;
			}
			usingInstanceMaterials = true;
			
			CreateInstanceMaterials();
			if (!smrs.IsNullOrEmpty())
			{
				for (int i = 0; i < smrs.Length; ++i)
				{
					var r = smrs[i];
					if (null != tempSMRInstanceMaterials[i])
					{
						r.sharedMaterials = tempSMRInstanceMaterials[i];
					}
					else
					{
						r.sharedMaterials = smrInstanceMaterials[i];
					}
				}
			}
			
			if (!mrs.IsNullOrEmpty())
			{
				for (int i = 0; i < mrs.Length; ++i)
				{
					var r = mrs[i];
					if (null != tempMRInstanceMaterials[i])
					{
						r.sharedMaterials = tempMRInstanceMaterials[i];
					}
					else
					{
						r.sharedMaterials = mrInstanceMaterials[i];
					}
				}
			}
		}
		
		public void ReplaceOriginMaterial(Material mat, SkinnedMeshRenderer smr, int index)
		{
			if (!smrs.IsNullOrEmpty())
			{
				for (int i = 0; i < smrs.Length; ++i)
				{
					var r = smrs[i];
					if (r == smr)
					{
						var matManager = RolePartMaterialManager.Me;
						if (null != matManager)
						{
							if (usingInstanceMaterials)
							{
								var oldInstanceMat = smrInstanceMaterials[i][index];
								var newInstanceMat = matManager.CreateMaterialInstance(mat);
								CopyMaterialRunValues(newInstanceMat, oldInstanceMat);

								matManager.DestroyInstance(smrOriginMaterials[i][index], oldInstanceMat);
								smrInstanceMaterials[i][index] = newInstanceMat;
								if (null != newInstanceMat && null != tempSMRInstanceMaterials[i])
								{
									tempSMRInstanceMaterials[i][index] = newInstanceMat;
								}
							}
						}
						smrOriginMaterials[i][index] = mat;
						
						if (usingInstanceMaterials)
						{
							r.sharedMaterials = smrInstanceMaterials[i];
						}
						else
						{
							r.sharedMaterials = smrOriginMaterials[i];
						}
						return;
					}
				}
			}
		}
		
		public void ReplaceOriginMaterial(Material mat, MeshRenderer mr, int index)
		{
			if (!mrs.IsNullOrEmpty())
			{
				for (int i = 0; i < mrs.Length; ++i)
				{
					var r = mrs[i];
					if (r == mr)
					{
						var matManager = RolePartMaterialManager.Me;
						if (null != matManager)
						{
							if (usingInstanceMaterials)
							{
								var oldInstanceMat = mrInstanceMaterials[i][index];
								var newInstanceMat = matManager.CreateMaterialInstance(mat);
								CopyMaterialRunValues(newInstanceMat, oldInstanceMat);

								matManager.DestroyInstance(mrOriginMaterials[i][index], oldInstanceMat);
								mrInstanceMaterials[i][index] = newInstanceMat;
								if (null != newInstanceMat && null != tempMRInstanceMaterials[i])
								{
									tempMRInstanceMaterials[i][index] = newInstanceMat;
								}
							}
						}
						mrOriginMaterials[i][index] = mat;
						
						if (usingInstanceMaterials)
						{
							r.sharedMaterials = mrInstanceMaterials[i];
						}
						else
						{
							r.sharedMaterials = mrOriginMaterials[i];
						}
						return;
					}
				}
			}
		}

		#region behaviour
		private void Awake_Render()
		{
			var matManager = RolePartMaterialManager.Me;
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
			matManager.ReplaceSharedMaterials(smrs);
			matManager.ReplaceSharedMaterials(mrs);
			if (!materials.IsNullOrEmpty())
			{
				for (int i = 0; i < materials.Length; ++i)
				{
					var m = materials[i];
					if (null != m)
					{
						materials[i] = matManager.ReplaceMaterial(m);
					}
				}
			}
			#endif

			if (!smrs.IsNullOrEmpty())
			{
				smrOriginMaterials = new Material[smrs.Length][];
				smrInstanceMaterials = new Material[smrs.Length][];
				tempSMRInstanceMaterials = new Material[smrs.Length][];
				for (int i = 0; i < smrs.Length; ++i)
				{
					var r = smrs[i];
					var mats = r.sharedMaterials;
					smrOriginMaterials[i] = mats;
					
					if (!mats.IsNullOrEmpty())
					{
						var instanceMats = new Material[mats.Length];
						smrInstanceMaterials[i] = instanceMats;

						var tempInstanceMats = tempSMRInstanceMaterials[i];
						// register
						for (int j = 0; j < mats.Length; ++j)
						{
							var m = mats[j];
							if (null != m)
							{
								if (!matManager.RegisterMaterial(m))
								{
									if (null == tempInstanceMats)
									{
										tempInstanceMats = new Material[mats.Length];
										tempSMRInstanceMaterials[i] = tempInstanceMats;
									}
									tempInstanceMats[j] = m;
								}
							}
						}
					}
				}
			}
			
			if (!mrs.IsNullOrEmpty())
			{
				mrOriginMaterials = new Material[mrs.Length][];
				mrInstanceMaterials = new Material[mrs.Length][];
				tempMRInstanceMaterials = new Material[mrs.Length][];
				for (int i = 0; i < mrs.Length; ++i)
				{
					var r = mrs[i];
					var mats = r.sharedMaterials;
					mrOriginMaterials[i] = mats;
					if (!mats.IsNullOrEmpty())
					{
						var instanceMats = new Material[mats.Length];
						mrInstanceMaterials[i] = instanceMats;

						var tempInstanceMats = tempMRInstanceMaterials[i];
						// register
						for (int j = 0; j < mats.Length; ++j)
						{
							var m = mats[j];
							if (null != m)
							{
								if (!matManager.RegisterMaterial(m))
								{
									if (null == tempInstanceMats)
									{
										tempInstanceMats = new Material[mats.Length];
										tempMRInstanceMaterials[i] = tempInstanceMats;
									}
									tempInstanceMats[j] = m;
								}
							}
						}
					}
				}
			}

			if (!materials.IsNullOrEmpty())
			{
				for (int i = 0; i < materials.Length; ++i)
				{
					var m = materials[i];
					if (null != m)
					{
						matManager.RegisterMaterial(m);
					}
				}
			}
		}
		
		private void OnDestroy_Render()
		{
			DestroyOriginMaterials();
			DestroyInstanceMaterials();
			
			smrInstanceMaterials = null;
			mrInstanceMaterials = null;
			tempSMRInstanceMaterials = null;
			tempMRInstanceMaterials = null;
		}
		#endregion behaviour
	
	}
} // namespace RO
