using UnityEngine;
using System.Collections.Generic;
using Ghost.Utility;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class TransformFollowInfo : IReuseableObject<Transform, Transform, Vector3, int>
	{
		public Transform transform;
		public Transform target;
		public Vector3 offset;
		public int followModes;
		bool _followPosition;
		bool _followRotation;
		bool _followScale;

		public System.Action<Transform, object> lostCallback = null;
		public object lostCallbackArg = null;

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
			if (null == target)
			{
				return false;
			}
			if (_followPosition) {
				transform.position = target.position + offset;
			}
			if (_followRotation) {
				transform.rotation = target.rotation;
			}
			if (_followScale) {
				transform.localScale = target.localScale;
			}
			return true;
		}

		#region IReuseableObjectBase
		public void Construct(Transform t, Transform target, Vector3 o, int e)
		{
			this.transform = t;
			this.target = target;
			this.offset = o;
			this.followModes = e;

			_followPosition = (this.followModes & TransformFollowManager.FOLLOW_POS) > 0;
			_followRotation = (this.followModes & TransformFollowManager.FOLLOW_ROT) > 0;
			_followScale = (this.followModes & TransformFollowManager.FOLLOW_SCALE) > 0;
			
			lostCallback = null;
			lostCallbackArg = null;
		}
		public void Destruct()
		{
			transform = null;
			target = null;
			lostCallback = null;
			lostCallbackArg = null;
		}
		public bool reused{get;set;}
		
		public void Destroy()
		{
			Destruct();
		}
		#endregion IReuseableObjectBase
	}

	[SLua.CustomLuaClassAttribute]
	public class TransformFollowManager : SingleTonGO<TransformFollowManager>
	{
		public const int FOLLOW_POS = 1;
		public const int FOLLOW_ROT = 2;
		public const int FOLLOW_SCALE = 4;
		public static TransformFollowManager Instance{get{return Me;}}
		public float updateInterval = 0f;
		private float nextUpdateTime = 0f;

		private Vector3 zeroVector3 = Vector3.zero;
		
		public int infoListCapacity = 500;

		public ObjectPool<TransformFollowInfo> infoPool{get;private set;}
		public List<TransformFollowInfo> infoList{get;private set;}

		private Transform findingTransform;
		private bool FindInfoPredicate(TransformFollowInfo info)
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
		
		public TransformFollowInfo FindInfo(Transform t)
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
			Transform target, 
			Vector3 offset, 
			int mode,
			System.Action<Transform, object> lostCallback,
			object lostCallbackArg)
		{
			var info = FindInfo(t);
			if (null == info)
			{
				info = CreateInfo(t, target, offset, mode);
				infoList.Add(info);
			}
			else
			{
				info.target = target;
				info.offset = offset;
			}
			info.lostCallback = lostCallback;
			info.lostCallbackArg = lostCallbackArg;
		}

		public void RegisterFollowPos(
			Transform t, 
			Transform target, 
			Vector3 offset, 
			System.Action<Transform, object> lostCallback,
			object lostCallbackArg)
		{
			RegisterFollow (t,target,offset,FOLLOW_POS,lostCallback,lostCallback);
		}

		public void RegisterFollowRotation(
			Transform t, 
			Transform target, 
			System.Action<Transform, object> lostCallback,
			object lostCallbackArg)
		{
			RegisterFollow (t,target,zeroVector3,FOLLOW_ROT,lostCallback,lostCallback);
		}

		public void RegisterFollowScale(
			Transform t, 
			Transform target, 
			System.Action<Transform, object> lostCallback,
			object lostCallbackArg)
		{
			RegisterFollow (t,target,zeroVector3,FOLLOW_SCALE,lostCallback,lostCallback);
		}

		public void RegisterFollowAll(
			Transform t, 
			Transform target, 
			Vector3 offset, 
			System.Action<Transform, object> lostCallback,
			object lostCallbackArg)
		{
			RegisterFollow (t,target,offset,FOLLOW_POS | FOLLOW_SCALE | FOLLOW_ROT,lostCallback,lostCallback);
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
		private TransformFollowInfo CreateInfo(Transform t, Transform target, Vector3 offset, int mode)
		{
			return infoPool.Create(t, target, offset, mode);
		}
		
		private void RecycleInfo(TransformFollowInfo info)
		{
			infoPool.Destroy(info);
		}

		#region behaviour
		protected override void Awake ()
		{
			base.Awake ();
			infoPool = new ObjectPool<TransformFollowInfo>();
			infoList = new List<TransformFollowInfo>(infoListCapacity);
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
