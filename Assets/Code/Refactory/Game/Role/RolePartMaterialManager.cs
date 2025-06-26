using UnityEngine;
using System.Collections.Generic;
using Ghost.Utility;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class RolePartMaterialInfo : IReuseableObject<Material, int>
	{
		public int maxCountInPool;
		public Material originMat;
		public List<Material> instanceUsedMatList;
		public List<Material> instancePool;

		public int refCount;

		public bool outlineEnable;
		public bool toonEnable;
		public int multiColorNumber;

		public void DisableOutline()
		{
			if (outlineEnable && null != originMat)
			{
				var shaderWithoutOutline = ShaderManager.Me.rolePart;
				originMat.shader = shaderWithoutOutline;
				if (null != instanceUsedMatList)
				{
					for (int i = 0; i < instanceUsedMatList.Count; ++i)
					{
						instanceUsedMatList[i].shader = shaderWithoutOutline;
					}
				}
				if (null != instancePool)
				{
					for (int i = 0; i < instancePool.Count; ++i)
					{
						instancePool[i].shader = shaderWithoutOutline;
					}
				}
			}
		}

		public void DisableToon()
		{
			if (toonEnable && null != originMat)
			{
				RolePart.SetToonEnable(originMat, false);
				if (null != instanceUsedMatList)
				{
					for (int i = 0; i < instanceUsedMatList.Count; ++i)
					{
						RolePart.SetToonEnable(instanceUsedMatList[i], false);
					}
				}
				if (null != instancePool)
				{
					for (int i = 0; i < instancePool.Count; ++i)
					{
						RolePart.SetToonEnable(instancePool[i], false);
					}
				}
			}
		}

		public void RestoreOutline()
		{
			if (outlineEnable && null != originMat)
			{
				var shaderOutline = null != ShaderManager.Me ? ShaderManager.Me.rolePartOutline : Shader.Find("RO/Role/PartOutline");
				originMat.shader = shaderOutline;
				if (null != instanceUsedMatList)
				{
					for (int i = 0; i < instanceUsedMatList.Count; ++i)
					{
						instanceUsedMatList[i].shader = shaderOutline;
					}
				}
				if (null != instancePool)
				{
					for (int i = 0; i < instancePool.Count; ++i)
					{
						instancePool[i].shader = shaderOutline;
					}
				}
			}
		}
		
		public void RestoreToon()
		{
			if (toonEnable && null != originMat)
			{
				RolePart.SetToonEnable(originMat, true);
				if (null != instanceUsedMatList)
				{
					for (int i = 0; i < instanceUsedMatList.Count; ++i)
					{
						RolePart.SetToonEnable(instanceUsedMatList[i], true);
					}
				}
				if (null != instancePool)
				{
					for (int i = 0; i < instancePool.Count; ++i)
					{
						RolePart.SetToonEnable(instancePool[i], true);
					}
				}
			}
		}

		public void SetMaskColor(Color maskColor)
		{
			if (null != originMat)
			{
				RolePart.SetMaskColor(originMat, maskColor);
				if (null != instanceUsedMatList)
				{
					for (int i = 0; i < instanceUsedMatList.Count; ++i)
					{
						RolePart.SetMaskColor(instanceUsedMatList[i], maskColor);
					}
				}
				if (null != instancePool)
				{
					for (int i = 0; i < instancePool.Count; ++i)
					{
						RolePart.SetMaskColor(instancePool[i], maskColor);
					}
				}
			}
		}

		public Material CreateInstance()
		{
			Material instance = null;
			if (null != instancePool && 0 < instancePool.Count)
			{
				instance = instancePool[instancePool.Count-1];
				instancePool.RemoveAt(instancePool.Count-1);
				// init instance
				RolePart.SetBlendMode(instance, RolePart.GetBlendMode(originMat));
				if (RolePart.IsChangeColorEnable(originMat))
				{
					RolePart.SetChangeColorEnable(instance, true);
					RolePart.SetChangeColor(instance, RolePart.GetChangeColor(originMat));
				}
				else
				{
					RolePart.SetChangeColorEnable(instance, false);
				}

				if (RolePart.IsToonEnable(originMat))
				{
					RolePart.SetToonEnable(instance, true);
					for (int i = 1; i < 4; ++i)
					{
						RolePart.SetToonLightColor(instance, RolePart.GetToonLightColor(originMat, i), i);
						RolePart.SetToonLightExposure(instance, RolePart.GetToonLightExposure(originMat, i), i);
					}
				}
				else
				{
					RolePart.SetToonEnable(instance, false);
				}

				RolePart.SetAlpha(instance, RolePart.GetAlpha(originMat));
			}
			else
			{
				instance =  Material.Instantiate<Material>(originMat);
			}

			if (null == instanceUsedMatList)
			{
				instanceUsedMatList = new List<Material>();
			}
			instanceUsedMatList.Add(instance);
			return instance;
		}

		public void DestroyInstance(Material instance)
		{
			instanceUsedMatList.Remove(instance);
			if (0 <= maxCountInPool)
			{
				var count = (null != instancePool) ? instancePool.Count : 0;
				if (count >= maxCountInPool)
				{
					Material.Destroy(instance);
					return;
				}
			}
			if (null == instancePool)
			{
				instancePool = new List<Material>();
			}
			instancePool.Add(instance);
		}

		#region IReuseableObjectBase
		public void Construct(Material mat, int count)
		{
			originMat = mat;
			maxCountInPool = count;
			refCount = 0;

			outlineEnable = (ShaderManager.Me.rolePartOutline == originMat.shader);
			toonEnable = RolePart.IsToonEnable(originMat);
			multiColorNumber = RolePart.GetMultiColorNumber(originMat);
		}
		public void Destruct()
		{
			RestoreOutline();
			RestoreToon();
			originMat = null;
			if (null != instanceUsedMatList)
			{
				instanceUsedMatList.Clear();
			}
			if (null != instancePool)
			{
				for (int i = 0; i < instancePool.Count; ++i)
				{
					Material.Destroy(instancePool[i]);
				}
				instancePool.Clear();
			}
			refCount = 0;
		}
		public bool reused{get;set;}
		
		public void Destroy()
		{
			Destruct();
		}
		#endregion IReuseableObjectBase
	}

	[System.Serializable]
	public class MultiColorInfo
	{
		public Color[] colors;
		public float speed = 1f;
		public Color curColor{get;private set;}

		private float progress = 0f;

		public void Update()
		{
			if (null == colors || 1 >= colors.Length)
			{
				return;
			}
			var i = (int)progress;
			if (colors.Length <= i)
			{
				progress -= i;
				i = 0;
			}
			var j = i+1;
			if (colors.Length == j)
			{
				j = 0;
			}
			curColor = Color.Lerp(colors[i], colors[j], progress-i);
			progress += speed * Time.deltaTime;
		}
	}

	[SLua.CustomLuaClassAttribute]
	public class RolePartMaterialManager : SingleTonGO<RolePartMaterialManager> 
	{
		#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
		private Dictionary<Material, Material> replaceMaterials = new Dictionary<Material, Material>(500);
		[SLua.DoNotToLuaAttribute]
		public Material ReplaceMaterial(Material m)
		{
			Material newM;
			if (!replaceMaterials.TryGetValue(m, out newM))
			{
				newM = Material.Instantiate<Material>(m);
				// test begin
//				if (!(null != newM.shader && newM.shader.name.StartsWith("RO/Role/")))
//				{
//					Texture texture = null;
//					if (null == newM.mainTexture)
//					{
//						texture = newM.GetTexture(string.Format("_CombineTex{0}", 1));
//					}
//					newM.shader = Shader.Find("RO/Role/PartOutline");
//					if (null != texture)
//					{
//						newM.mainTexture = texture;
//					}
//					RolePart.SetToonEnable(newM, true);
//				}
				// test end
				replaceMaterials[m] = newM;
			}
			return newM;
		}
		[SLua.DoNotToLuaAttribute]
		public void ReplaceSharedMaterials(SkinnedMeshRenderer[] smrs)
		{
			if (!smrs.IsNullOrEmpty())
			{
				for (int i = 0; i < smrs.Length; ++i)
				{
					var r = smrs[i];
					var mats = r.sharedMaterials;
					if (!mats.IsNullOrEmpty())
					{
						for (int j = 0; j < mats.Length; ++j)
						{
							var m = mats[j];
							if (null != m)
							{
								mats[j] = ReplaceMaterial(m);
							}
						}
						r.sharedMaterials = mats;
					}
				}
			}
		}
		[SLua.DoNotToLuaAttribute]
		public void ReplaceSharedMaterials(MeshRenderer[] mrs)
		{
			if (!mrs.IsNullOrEmpty())
			{
				for (int i = 0; i < mrs.Length; ++i)
				{
					var r = mrs[i];
					var mats = r.sharedMaterials;
					if (!mats.IsNullOrEmpty())
					{
						for (int j = 0; j < mats.Length; ++j)
						{
							var m = mats[j];
							if (null != m)
							{
								mats[j] = ReplaceMaterial(m);
							}
						}
						r.sharedMaterials = mats;
					}
				}
			}
		}
		#endif
		
		public static RolePartMaterialManager Instance{get{return Me;}}

		public bool enable = true;

		public float updateInterval = 0f;
		private float nextUpdateTime = 0f;

		public int materialInstanceMaxCountInPool = 3;
		public int materialListCapacity = 500;
		public int newMaterialListCapacity = 10;
		public int multiColorMaterialListCapacity = 10;
		public Vector3 toonLightDirection = new Vector3(0.4484f,-0.15643f,-0.88002f);

		public MultiColorInfo[] multiColors;

		public ObjectPool<RolePartMaterialInfo> infoPool{get;private set;}
		public List<RolePartMaterialInfo> materialList{get;private set;}
		public List<RolePartMaterialInfo> newMaterialList{get;private set;}
		public List<RolePartMaterialInfo> multiColorMaterialList{get;private set;}

		private Transform cameraTransform_;
		private Transform cameraTransform
		{
			get
			{
				if (null == cameraTransform_ && null != Camera.main)
				{
					cameraTransform_ = Camera.main.transform;
				}
				return cameraTransform_;
			}
		}
		private Vector3 prevCameraEuler = Vector3.zero;
		private Quaternion toonLightRotation = Quaternion.identity;

		private Material findingOriginMat;
		private bool FindInfoPredicate(RolePartMaterialInfo info)
		{
			return info.originMat == findingOriginMat;
		}

		private bool outlineEnable = true;
		private bool toonEnable = true;

		#region interface
		public void ClearInfoPool()
		{
			infoPool.Clear();
		}

		public int GetInfoCountInPool()
		{
			return infoPool.Count;
		}

		public void DisableOutline()
		{
			if (!outlineEnable)
			{
				return;
			}
			outlineEnable = false;
			for (int i = 0; i < materialList.Count; ++i)
			{
				materialList[i].DisableOutline();
			}
			for (int i = 0; i < newMaterialList.Count; ++i)
			{
				newMaterialList[i].DisableOutline();
			}
		}
		
		public void DisableToon()
		{
			if (!toonEnable)
			{
				return;
			}
			toonEnable = false;
			for (int i = 0; i < materialList.Count; ++i)
			{
				materialList[i].DisableToon();
			}
			for (int i = 0; i < newMaterialList.Count; ++i)
			{
				newMaterialList[i].DisableToon();
			}
		}
		
		public void RestoreOutline()
		{
			if (outlineEnable)
			{
				return;
			}
			outlineEnable = true;
			for (int i = 0; i < materialList.Count; ++i)
			{
				materialList[i].RestoreOutline();
			}
			for (int i = 0; i < newMaterialList.Count; ++i)
			{
				newMaterialList[i].RestoreOutline();
			}
		}
		
		public void RestoreToon()
		{
			if (toonEnable)
			{
				return;
			}
			toonEnable = true;
			for (int i = 0; i < materialList.Count; ++i)
			{
				materialList[i].RestoreToon();
			}
			for (int i = 0; i < newMaterialList.Count; ++i)
			{
				newMaterialList[i].RestoreToon();
			}
		}

		public RolePartMaterialInfo FindInfo(Material originMat)
		{
			findingOriginMat = originMat;
			var info = materialList.Find(FindInfoPredicate);
			findingOriginMat = null;
			return info;
		}

		public int FindInfoIndex(Material originMat)
		{
			findingOriginMat = originMat;
			var index = materialList.FindIndex(FindInfoPredicate);
			findingOriginMat = null;
			return index;
		}

		public bool RegisterMaterial(Material originMat)
		{
			var info = FindInfo(originMat);
			if (null == info)
			{
				info = DoRegisterMaterial(originMat);
				if (null == info)
				{
					return false;
				}
			}
			++info.refCount;
			return true;
		}

		private RolePartMaterialInfo DoRegisterMaterial(Material originMat)
		{
			if (null == originMat)
			{
				ROLogger.Log("<color=red>No Role Part Material: </color>");
				return null;
			}
			if (!(null != originMat.shader && originMat.shader.name.StartsWith("RO/Role/")))
			{
				ROLogger.LogFormat(originMat, "<color=red>Invalid Role Part Material: </color>{0}", originMat.name);
				return null;
			}
			var info = CreateInfo(originMat);
			materialList.Add(info);
			newMaterialList.Add(info);
			if (0 <= info.multiColorNumber)
			{
				multiColorMaterialList.Add(info);
			}
			return info;
		}

		public void UnregisterMaterial(Material originMat)
		{
			var index = FindInfoIndex(originMat);
			if (0 > index)
			{
				return;
			}
			var info = materialList[index];
			if (0 >= --info.refCount)
			{
				materialList.RemoveAt(index);
				newMaterialList.Remove(info);
				RecycleInfo(info);
			}
		}

		public Material CreateMaterialInstance(Material originMat)
		{
			var info = FindInfo(originMat);
			if (null == info)
			{
				info = DoRegisterMaterial(originMat);
				if (null == info)
				{
					return null;
				}
				++info.refCount;
			}
			return info.CreateInstance();
		}
		
		public void DestroyInstance(Material originMat, Material instanceMat)
		{
			if (null == instanceMat)
			{
				return;
			}
			var info = FindInfo(originMat);
			if (null != info)
			{
				info.DestroyInstance(instanceMat);
			}
			else
			{
				Material.Destroy(instanceMat);
			}
		}
		#endregion interface

		private RolePartMaterialInfo CreateInfo(Material originMat)
		{
			var info = infoPool.Create(originMat, materialInstanceMaxCountInPool);
			if (!outlineEnable)
			{
				info.DisableOutline();
			}
			if (!toonEnable)
			{
				info.DisableToon();
			}
			return info;
		}

		private void RecycleInfo(RolePartMaterialInfo info)
		{
			infoPool.Destroy(info);
		}

		private void DoUpdateMaterials(List<RolePartMaterialInfo> list)
		{
			if (enable && 0 < list.Count)
			{
				var dir = toonLightRotation * toonLightDirection;
				for (int i = list.Count-1; 0 <= i; --i)
				{
					var info = list[i];
					if (null != info.originMat)
					{
						info.originMat.SetVector("_LightDirection1", dir);
						if (!info.instanceUsedMatList.IsNullOrEmpty())
						{
							for (int j = 0; j < info.instanceUsedMatList.Count; ++j)
							{
								info.instanceUsedMatList[j].SetVector("_LightDirection1", dir);
							}
						}
					}
					else
					{
						list.RemoveAt(i);
						RecycleInfo(info);
					}
				}
			}
		}

		private void UpdateAllMaterials()
		{
			if (0 < materialList.Count)
			{
				DoUpdateMaterials(materialList);
				newMaterialList.Clear();
			}
		}

		private void UpdateNewMaterials()
		{
			if (0 < newMaterialList.Count)
			{
				DoUpdateMaterials(newMaterialList);
				newMaterialList.Clear();
			}
		}

		private void UpdateMultiColorMaterials()
		{
			if (multiColors.IsNullOrEmpty())
			{
				return;
			}
			for (int i = 0; i < multiColors.Length; ++i)
			{
				multiColors[i].Update();
			}

			if (0 < multiColorMaterialList.Count)
			{
				for (int i = 0; i < multiColorMaterialList.Count; ++i)
				{
					var matInfo = multiColorMaterialList[i];
					if (multiColors.CheckIndex(matInfo.multiColorNumber))
					{
						matInfo.SetMaskColor(multiColors[matInfo.multiColorNumber].curColor);
					}
				}
			}
		}

		private void UpdateLightDirection(Vector3 cameraEuler)
		{
			toonLightRotation = Quaternion.Euler(0, cameraEuler.y, 0);
		}

		private void UpdateCameraEuler()
		{
			if (null == cameraTransform)
			{
				return;
			}
			var cameraEuler = cameraTransform.eulerAngles;
			if (prevCameraEuler.y != cameraEuler.y)
			{
				prevCameraEuler = cameraEuler;
				UpdateLightDirection(cameraEuler);
				UpdateAllMaterials();
			}
			else
			{
				UpdateNewMaterials();
			}
		}
	
		#region behaviour
		protected override void Awake ()
		{
			base.Awake ();
			infoPool = new ObjectPool<RolePartMaterialInfo>();
			materialList = new List<RolePartMaterialInfo>(materialListCapacity);
			newMaterialList = new List<RolePartMaterialInfo>(newMaterialListCapacity);
			multiColorMaterialList = new List<RolePartMaterialInfo>(multiColorMaterialListCapacity);
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			ClearInfoPool();
			for (int i = 0; i < materialList.Count; ++i)
			{
				var info = materialList[i];
				info.Destroy();
			}
			materialList.Clear();
			newMaterialList.Clear();
			multiColorMaterialList.Clear();
		}

		void LateUpdate()
		{
			if (Time.time > nextUpdateTime)
			{
				nextUpdateTime = Time.time + updateInterval;
			}

			UpdateCameraEuler();
			UpdateMultiColorMaterials();
		}
		#endregion behaviour
	}
} // namespace RO
