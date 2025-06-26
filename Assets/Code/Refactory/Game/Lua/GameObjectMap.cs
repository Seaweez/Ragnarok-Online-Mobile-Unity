using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;
using Ghost.Utility;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class GameObjectInfo
	{
		[SLua.CustomLuaClassAttribute]
		public class Info : IReuseableObject
		{
			public int type;
			public long key;
			public Transform transform = null;

			public float cullingRadius = 0;

			public bool wasVisible = true;
			public bool isVisible = true;
			public int previousDistance = 0;
			public int currentDistance = 0;

			#region IReuseableObjectBase
			public void Construct()
			{
			}
			public void Destruct()
			{
				transform = null;
			}
			public bool reused{get;set;}
			
			public void Destroy()
			{
				Destruct();
			}
			#endregion IReuseableObjectBase
		}

		public Dictionary<long, Info> map{get;private set;}

		public GameObjectInfo()
		{
			map = new Dictionary<long, Info>();
		}

		public void Add_Transform(int type, long key, Transform t, float radius)
		{
			Info info;
			if (!map.TryGetValue(key, out info))
			{
				info = GameObjectMap.Me.CreateInfo(type, key);
				map.Add(key, info);
			}
			info.transform = t;
			info.cullingRadius = radius;
		}
		public bool Remove(long key)
		{
			Info info;
			if (!map.TryGetValue(key, out info))
			{
				return false;
			}
			GameObjectMap.Me.RecycleInfo(info);
			return map.Remove(key);
		}
		public void Clear()
		{
			if (0 >= map.Count)
			{
				return;
			}
			foreach (var key_value in map)
			{
				GameObjectMap.Me.RecycleInfo(key_value.Value);
			}
			map.Clear();
		}
		public Info Get(long key)
		{
			Info info;
			if (!map.TryGetValue(key, out info))
			{
				return null;
			}
			return info;
		}
	}

	[SLua.CustomLuaClassAttribute]
	public partial class GameObjectMap : SingleTonGO<GameObjectMap> 
	{
		public float[] cullingGroupBoundingDistances;
		public int[] cullingGroupTypes;
		public int[] cullingGroupOnlySetActiveTypes;
		public int cullingGroupSphereCount = 500;
		public Transform cullingGroupReference = null;

		private CullingGroup cullingGroup;
		private BoundingSphere[] cullingGroupSpheres;
		private GameObjectInfo.Info[] cullingGroupInfos;
		private List<GameObjectInfo.Info> changedStates = new List<GameObjectInfo.Info>();

		public ObjectPool<GameObjectInfo.Info> infoPool{get;private set;}
		private Dictionary<int, GameObjectInfo> map = new Dictionary<int, GameObjectInfo>();

		public void ClearInfoPool()
		{
			infoPool.Clear();
		}
		
		public int GetInfoCountInPool()
		{
			return infoPool.Count;
		}

		public GameObjectInfo.Info CreateInfo(int type, long key)
		{
			var info = infoPool.Create();
			info.type = type;
			info.key = key;
			return info;
		}
		
		public void RecycleInfo(GameObjectInfo.Info info)
		{
			infoPool.Destroy(info);
			changedStates.Remove(info);
		}

		public void SetCullingGroupCamera(Camera c)
		{
			cullingGroup.targetCamera = c;
		}

		public void ClearCullingGroupCamera(Camera c)
		{
			if (cullingGroup.targetCamera == c)
			{
				cullingGroup.targetCamera = null;
			}
		}

		public bool Register_Transform(int type, long key, Transform t)
		{
			return Register_TransformForCulling(type, key, t, 0);
		}

		public bool Register_TransformForCulling(int type, long key, Transform t, float radius)
		{
			if (null == t)
			{
				return false;
			}
			GameObjectInfo info;
			if (!map.TryGetValue(type, out info))
			{
				info = new GameObjectInfo();
				map.Add(type, info);
			}
			info.Add_Transform(type, key, t, radius);
			return true;
		}

		public bool Unregister(int type, long key)
		{
			GameObjectInfo info;
			if (!map.TryGetValue(type, out info))
			{
				return false;
			}
			return info.Remove(key);
		}

		public void Clear(int type)
		{
			GameObjectInfo info;
			if (!map.TryGetValue(type, out info))
			{
				return;
			}
			info.Clear();
		}

		public void ClearAll()
		{
			foreach (var key_value in map)
			{
				key_value.Value.Clear();
			}
		}

		public Transform Get_Transform(int type, long key)
		{
			GameObjectInfo info;
			if (!map.TryGetValue(type, out info))
			{
				return null;
			}
			return info.Get(key).transform;
		}

		public GameObjectInfo.Info Get_Info(int type, long key)
		{
			GameObjectInfo info;
			if (!map.TryGetValue(type, out info))
			{
				return null;
			}
			return info.Get(key);
		}

		public List<GameObjectInfo.Info> GetChangedInfo()
		{
			return changedStates;
		}

		private void SphereStateChanged (CullingGroupEvent sphere)
		{
			if (!cullingGroupInfos.CheckIndex(sphere.index))
			{
				return;
			}
			var info = cullingGroupInfos[sphere.index];
			if (null == info)
			{
				return;
			}

			info.isVisible = sphere.isVisible;
			info.currentDistance = sphere.currentDistance;
			if (!cullingGroupOnlySetActiveTypes.IsNullOrEmpty())
			{
				for (int i = 0; i < cullingGroupOnlySetActiveTypes.Length; ++i)
				{
					if (cullingGroupOnlySetActiveTypes[i] == info.type)
					{
						if (info.wasVisible != info.isVisible)
						{
							info.transform.gameObject.SetActive(info.isVisible);
							info.wasVisible = info.isVisible;
						}
						info.previousDistance = info.currentDistance;
						return;
					}
				}
			}
			if (info.wasVisible != info.isVisible || info.previousDistance != info.currentDistance)
			{
				if (!changedStates.Contains(info))
				{
					changedStates.Add(info);
				}
			}
//			Debug.LogFormat("<color=white>SphereStateChanged: </color>index={0}, wasVisible={1}, isVisible={2}, previousDistance={3}, currentDistance={4}", 
//			                sphere.index, sphere.wasVisible, sphere.isVisible, sphere.previousDistance, sphere.currentDistance);
		}

		private List<long> invalidKeys = new List<long>();
		public void UpdateCullingGroup()
		{
			if (cullingGroupTypes.IsNullOrEmpty())
			{
				cullingGroup.SetBoundingSphereCount(0);
				return;
			}
			var sphereCountLimit = cullingGroupSpheres.Length;
			if (0 >= sphereCountLimit)
			{
				cullingGroup.SetBoundingSphereCount(0);
				return;
			}
			int sphereCount = 0;
			
			for (int i = 0; i < cullingGroupTypes.Length; ++i)
			{
				var type = cullingGroupTypes[i];
				GameObjectInfo info;
				if (map.TryGetValue(type, out info) && 0 < info.map.Count)
				{
					foreach (var key_value in info.map)
					{
						var t = key_value.Value.transform;
						if (null == t)
						{
							invalidKeys.Add(key_value.Key);
							continue;
						}
						
						var sphereIndex = sphereCount++;
						cullingGroupSpheres[sphereIndex].position = t.position;
						cullingGroupSpheres[sphereIndex].radius = key_value.Value.cullingRadius;
						
						cullingGroupInfos[sphereIndex] = key_value.Value;
						if (sphereCountLimit == sphereCount)
						{
							break;
						}
					}
					
					if (0 < invalidKeys.Count)
					{
						for (int j = 0; j < invalidKeys.Count; ++j)
						{
							info.Remove(invalidKeys[j]);
						}
						invalidKeys.Clear();
					}
					
					if (sphereCountLimit == sphereCount)
					{
						break;
					}
				}
			}
			
			for (int i = sphereCount; i < cullingGroupInfos.Length; ++i)
			{
				if (null == cullingGroupInfos[i])
				{
					break;
				}
				cullingGroupInfos[i] = null;
			}
			
			if (null != cullingGroupReference)
			{
				cullingGroup.SetDistanceReferencePoint(cullingGroupReference);
			}
			else if (null != cullingGroup.targetCamera)
			{
				cullingGroup.SetDistanceReferencePoint(cullingGroup.targetCamera.transform);
			}
			cullingGroup.SetBoundingSphereCount(sphereCount);
		}

		#region behaviour
		protected override void Awake ()
		{
			base.Awake ();
			infoPool = new ObjectPool<GameObjectInfo.Info>();
			cullingGroup = new CullingGroup();
		}

		void Start()
		{
			cullingGroup.SetBoundingDistances(cullingGroupBoundingDistances);
			cullingGroupSpheres = new BoundingSphere[cullingGroupSphereCount];
			cullingGroup.SetBoundingSpheres(cullingGroupSpheres);
			cullingGroup.onStateChanged = SphereStateChanged;
			
			cullingGroupInfos = new GameObjectInfo.Info[cullingGroupSphereCount];
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			ClearInfoPool();
			cullingGroup.Dispose();
		}

#if DEBUG_DRAW
		[SLua.DoNotToLuaAttribute]
		public int currentSphereCount;
		void OnDrawGizmos()
		{
			if (Application.isPlaying)
			{
				var sphereCountLimit = cullingGroupSpheres.Length;
				currentSphereCount = 0;

				for (int i = 0; i < cullingGroupTypes.Length; ++i)
				{
					var type = cullingGroupTypes[i];
					GameObjectInfo info;
					if (map.TryGetValue(type, out info) && 0 < info.map.Count)
					{
						foreach (var key_value in info.map)
						{
							if (sphereCountLimit == currentSphereCount)
							{
								Gizmos.color = Color.gray;
							}
							else
							{
								Gizmos.color = key_value.Value.isVisible ? Color.red : Color.green;
							}
							Gizmos.DrawWireSphere(key_value.Value.transform.position, key_value.Value.cullingRadius);
							++currentSphereCount;
						}
					}
				}
			}
		}
#endif // DEBUG_DRAW
		#endregion behaviour
	}
} // namespace RO
