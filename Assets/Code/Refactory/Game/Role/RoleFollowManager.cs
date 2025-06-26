using UnityEngine;
using System.Collections.Generic;
using Ghost.Utility;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class RoleFollowInfo : IReuseableObject<Transform, RoleComplete, Vector3, int>
	{
		public Transform transform;
		public RoleComplete role;
		public Vector3 offset;
		public Vector3 tempOffset;
		public int epID;

		public System.Action<Transform, object> lostCallback = null;
		public object lostCallbackArg = null;

		public bool cpMode = false;

		public void CallbackLost()
		{
			if (null != lostCallback)
			{
				lostCallback(transform, lostCallbackArg);
			}
		}

		public bool Update()
		{
			if (null == transform)
			{
				return false;
			}
			if (null == role || null == role.transform)
			{
				return false;
			}

			if (cpMode)
			{
				Transform followTransform = null;
				if (0 < epID)
				{
					followTransform = role.GetCP(epID);
				}
				if (null == followTransform)
				{
					followTransform = role.transform;
				}
				transform.position = followTransform.position;
				transform.rotation = followTransform.rotation;
				transform.localScale = followTransform.lossyScale;
			}
			else
			{
				if (0 < epID)
				{
					var epTransform = role.GetEP(epID);
					if (null != epTransform)
					{
						transform.position = epTransform.position + offset;
					}
					else
					{
						transform.position = role.transform.position + tempOffset;
					}
				}
				else
				{
					transform.position = role.transform.position + offset;
				}
			}
			return true;
		}

		#region IReuseableObjectBase
		public void Construct(Transform t, RoleComplete r, Vector3 o, int e)
		{
			transform = t;
			role = r;
			offset = o;
			epID = e;

			lostCallback = null;
			lostCallbackArg = null;

			cpMode = false;
		}
		public void Destruct()
		{
			transform = null;
			role = null;
		}
		public bool reused{get;set;}
		
		public void Destroy()
		{
			Destruct();
		}
		#endregion IReuseableObjectBase
	}

	[SLua.CustomLuaClassAttribute]
	public class RoleFollowManager : SingleTonGO<RoleFollowManager>  
	{
		public static RoleFollowManager Instance{get{return Me;}}

		public float updateInterval = 0f;
		private float nextUpdateTime = 0f;

		public int infoListCapacity = 500;

		public ObjectPool<RoleFollowInfo> infoPool{get;private set;}
		public List<RoleFollowInfo> infoList{get;private set;}

		private Transform findingTransform;
		private bool FindInfoPredicate(RoleFollowInfo info)
		{
			return info.transform == findingTransform;
		}

		#region interface
		public void ClearInfoPool()
		{
			infoPool.Clear();
		}
		
		public int GetInfoCountInPool()
		{
			return infoPool.Count;
		}
		
		public RoleFollowInfo FindInfo(Transform t)
		{
			findingTransform = t;
			var info = infoList.Find(FindInfoPredicate);
			findingTransform = null;
			return info;
		}

		public int FindInfoIndex(Transform t)
		{
			findingTransform = t;
			var index = infoList.FindIndex(FindInfoPredicate);
			findingTransform = null;
			return index;
		}
		
		public void RegisterFollow(
			Transform t, 
			RoleComplete role, 
			Vector3 offset, 
			Vector3 tempOffset,
			int epID,
			System.Action<Transform, object> lostCallback,
			object lostCallbackArg)
		{
			var info = FindInfo(t);
			if (null == info)
			{
				info = CreateInfo(t, role, offset, epID);
				infoList.Add(info);
			}
			else
			{
				info.role = role;
				info.offset = offset;
				info.epID = epID;
			}
			info.tempOffset = tempOffset;
			info.lostCallback = lostCallback;
			info.lostCallbackArg = lostCallbackArg;
			info.cpMode = false;
		}

		public void RegisterFollowCP(
			Transform t, 
			RoleComplete role, 
			int cpID,
			System.Action<Transform, object> lostCallback,
			object lostCallbackArg)
		{
			var info = FindInfo(t);
			if (null == info)
			{
				info = CreateInfo(t, role, Vector3.zero, cpID);
				infoList.Add(info);
			}
			else
			{
				info.role = role;
				info.offset = Vector3.zero;
				info.epID = cpID;
			}
			info.lostCallback = lostCallback;
			info.lostCallbackArg = lostCallbackArg;
			info.cpMode = true;
		}
		
		public void UnregisterFollow(Transform originTrans)
		{
			var index = FindInfoIndex(originTrans);
			if (0 > index)
			{
				return;
			}
			var info = infoList[index];
			infoList.RemoveAt(index);
			RecycleInfo(info);
		}

		#endregion interface
		private RoleFollowInfo CreateInfo(Transform t, RoleComplete role, Vector3 offset, int epID)
		{
			return infoPool.Create(t, role, offset, epID);
		}
		
		private void RecycleInfo(RoleFollowInfo info)
		{
			infoPool.Destroy(info);
		}

		#region behaviour
		protected override void Awake ()
		{
			base.Awake ();
			infoPool = new ObjectPool<RoleFollowInfo>();
			infoList = new List<RoleFollowInfo>(infoListCapacity);
		}
		
		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			ClearInfoPool();
			for (int i = 0; i < infoList.Count; ++i)
			{
				var info = infoList[i];
				info.Destroy();
			}
			infoList.Clear();
		}

		void LateUpdate()
		{
			if (Time.time > nextUpdateTime)
			{
				nextUpdateTime = Time.time + updateInterval;
			}

			for (int i = infoList.Count-1; 0 <= i ; --i)
			{
				var info = infoList[i];
				if (!info.Update())
				{
					infoList.RemoveAt(i);
					info.CallbackLost();
					RecycleInfo(info);
				}
			}
		}
		#endregion behaviour
	}
} // namespace RO
