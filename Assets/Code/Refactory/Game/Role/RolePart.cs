using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[System.Serializable]
	[SLua.CustomLuaClassAttribute]
	public class TransformWithID
	{
		public int ID;
		public Transform transform;
        

        public TransformWithID(int id, Transform t)
		{
			ID = id;
			transform = t;
		}
	}

	[SLua.CustomLuaClassAttribute]
	public partial class RolePart : MonoBehaviour 
	{
		public Transform[] eps;
		public Transform[] cps;
		public TransformWithID[] ecps;

		public Animator[] animators;
		public SkinnedMeshRenderer[] smrs;
		public MeshRenderer[] mrs;
        [Tooltip("UseOffest and Scale")]
        [SerializeField]
        private bool useOffset = false;
        public Vector3 offset = Vector3.zero; // ค่าเริ่มต้นของ offset
        public Vector3 scale = Vector3.one;
        //		public ParticleSystem[] pss;

        private bool _componentsDisable = false;

        void LateUpdate()
        {
            if (useOffset) // ตรวจสอบว่า checkbox ถูกเปิดหรือไม่
            {
                ApplyOffsetAndScale();
            }
        }
        public bool componentsDisable
		{
			get
			{
				return _componentsDisable;
			}
			set
			{
				if (value == componentsDisable)
				{
					return;
				}
				_componentsDisable = value;
				ApplyComponentsEnable();
			}
		}

        public void UpdateOffsetAndScale(Vector3 newOffset, Vector3 newScale)
        {
            // อัปเดตค่า offset และ scale ใหม่
            offset = newOffset;
            scale = newScale;

            // ปรับค่าการเปลี่ยนแปลงให้กับ Transform ทันที
            ApplyOffsetAndScale();
        }

        private void ApplyOffsetAndScale()
        {
            if (useOffset) // ตรวจสอบว่าค่า checkbox ถูกเลือกหรือไม่
            {
                // ปรับค่าการแปลตำแหน่ง (offset) และการปรับขนาด (scale) ให้กับ Transform ของ RolePart
                transform.localPosition = offset;
                transform.localScale = scale;
            }
        }

        private void ApplyComponentsEnable()
		{
			var enable = !componentsDisable;
			if (!animators.IsNullOrEmpty())
			{
				for (int i = 0; i < animators.Length; ++i)
				{
					animators[i].enabled = enable;
				}
			}
			if (!smrs.IsNullOrEmpty())
			{
				for (int i = 0; i < smrs.Length; ++i)
				{
					smrs[i].enabled = enable;
				}
			}
			if (!mrs.IsNullOrEmpty())
			{
				for (int i = 0; i < mrs.Length; ++i)
				{
					mrs[i].enabled = enable;
				}
			}
			if (!ecps.IsNullOrEmpty())
			{
				for (int i = 0; i < ecps.Length; ++i)
				{
					var t = ecps[i].transform;
					var childCount = t.childCount;
					if (0 < childCount)
					{
						for (int j = 0; j < childCount; ++j)
						{
							var child = t.GetChild(j);
							var effect = child.GetComponent<EffectHandle>();
							if (null != effect)
							{
								effect.componentsDisable = enable;
							}
						}
					}
				}
			}
		}

		private int _layer = 0;
		public int layer
		{
			get
			{
				return _layer;
			}
			set
			{
				if (value == layer)
				{
					return;
				}
				_layer = value;
				ApplyLayer();
			}
		}

		public virtual void ApplyLayer()
		{
			GameObjectUtil.Instance.ChangeLayersRecursively(gameObject, layer);
		}

		private static List<Transform> tempTransformList_1 = new List<Transform>();
		private static List<Transform> tempTransformList_2 = new List<Transform>();
		private static HashSet<Transform> tempHashSet = new HashSet<Transform>();
		public void ApplyLayerIgnoreCPs()
		{
			tempTransformList_1.Clear();
			tempTransformList_2.Clear();
			tempHashSet.Clear();
			if (!cps.IsNullOrEmpty())
			{
				for (int i = 0; i < cps.Length; ++i)
				{
					var t = cps[i];
					if (null != t)
					{
						tempHashSet.Add(t);
					}
				}
			}
			
			var parents = tempTransformList_1;
			parents.Add(transform);
			var nextParents = tempTransformList_2;
			
			while (0 < parents.Count)
			{
				for (int i = 0; i < parents.Count; ++i)
				{
					var parent = parents[i];
					parent.gameObject.layer = layer;
					if (tempHashSet.Contains(parent))
					{// ignore cp
						continue;
					}
					var childCount = parent.childCount;
					for (int j = 0; j < childCount; ++j)
					{
						nextParents.Add(parent.GetChild(j));
					}
				}
				parents.Clear();
				var temp = parents;
				parents = nextParents;
				nextParents = temp;
			}
			
			tempTransformList_1.Clear();
			tempTransformList_2.Clear();
			tempHashSet.Clear();
		}

		public Transform GetEP(int i)
		{
			if (null == eps || !eps.CheckIndex(i))
			{
				return null;
			}
			return eps[i];
		}
		public Transform GetCP(int i)
		{
			if (null == cps || !cps.CheckIndex(i))
			{
				return null;
			}
			return cps[i];
		}

		public void MoveEffectToTransform(Transform to)
		{
			var effectList = EffectHandle.tempEffectList;

			for (int i = 0; i < eps.Length; ++i)
			{
				var fromEP = eps[i];
				if (null != fromEP)
				{
					var childCount = fromEP.childCount;
					for (int j = 0; j < childCount; ++j)
					{
						var child = fromEP.GetChild(j);
						var effect = child.GetComponent<EffectHandle>();
						if (null != effect)
						{
							effectList.Add(effect);
						}
					}
				}
			}

			for (int i = 0; i < effectList.Count; ++i)
			{
				var effect = effectList[i];
				effect.transform.SetParent(to, false);
			}

			effectList.Clear();
		}

		public void MoveEffect(RolePart to)
		{
			for (int i = 0; i < eps.Length; ++i)
			{
				var fromEP = eps[i];
				if (null != fromEP)
				{
					var toEP = to.GetEP(i);
					if (null == toEP)
					{
						var asBody = this as RolePartBody;
						if (null != asBody)
						{
							toEP = asBody.owner.transform;
						}
					}
					if (null == toEP)
					{
						var roleComplete = gameObject.GetComponentInParent<RoleComplete>();
						if (null != roleComplete)
						{
							toEP = roleComplete.transform;
						}
					}

					var effectList = EffectHandle.tempEffectList;

					var childCount = fromEP.childCount;
					for (int j = 0; j < childCount; ++j)
					{
						var child = fromEP.GetChild(j);
						var effect = child.GetComponent<EffectHandle>();
						if (null != effect)
						{
							effectList.Add(effect);
						}
					}
					
					for (int j = 0; j < effectList.Count; ++j)
					{
						var effect = effectList[j];
						effect.transform.SetParent(toEP, false);
					}
					
					effectList.Clear();
				}
			}
		}

		public void SetActionSpeed(float speed)
		{
			if (animators.IsNullOrEmpty())
			{
				return;
			}
			for (int i = 0; i < animators.Length; ++i)
			{
				var a = animators[i];
				a.speed = speed;
			}
		}

		public virtual void PlayAction(int nameHash, int defaultNameHash, float speed, float normalizedTime)
		{
			if (animators.IsNullOrEmpty())
			{
				return;
			}
			for (int i = 0; i < animators.Length; ++i)
			{
				var a = animators[i];
				a.speed = speed;
				if (!a.HasState(0, nameHash))
				{
					if (a.HasState(0, defaultNameHash))
					{
						nameHash = defaultNameHash;
					}
					else
					{
						if (null != LuaLuancher.Me)
						{
							nameHash = LuaLuancher.Me.defaultActionNameHash;
							if (!a.HasState(0, nameHash))
							{
								continue;
							}
						}
						else
						{
							continue;
						}
					}
				}
				a.Play (nameHash, -1, normalizedTime);
			}
		}

		public void PlayAction(int nameHash, int defaultNameHash)
		{
			PlayAction(nameHash, defaultNameHash, 1f, 0f);
		}

		#region behaviour
		protected virtual void Awake()
		{
			Awake_Render();
		}

		protected virtual void Start()
		{
			if (!ecps.IsNullOrEmpty())
			{
				var dictionary = EffectDictionary.global;
				if (null != dictionary)
				{
					var loader = ResourceManager.Loader;
					for (int i = 0; i < ecps.Length; ++i)
					{
						var ecp = ecps[i];
						var effectInfo = dictionary.GetEffectInfo(ecp.ID);
						if (null != effectInfo)
						{
							var prefab = loader.SLoad<GameObject>(ResourcePathHelper.IDEffect(effectInfo.path));
							if (null != prefab)
							{
								var effect = GameObject.Instantiate(prefab) as GameObject;
								effect.name = prefab.name;
								effect.transform.ResetParent(ecp.transform);
								GameObjectUtil.Instance.ChangeLayersRecursively(effect, ecp.transform.gameObject.layer);
							}
						}
					}
				}
			}
		}

		protected virtual void OnDestroy()
		{
			OnDestroy_Render();
		}
		#endregion behaviour
	}
} // namespace RO
