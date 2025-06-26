using UnityEngine;
using System.Collections.Generic;
using Ghost.Utils;
using Ghost.Attribute;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class Bus : MonoBehaviour 
	{
		private const string HUNG_POINT_PREFIX = "HP_";

		[SerializeField, SetProperty("ID")]
		private long ID_ = 0;
		public long ID
		{
			get
			{
				return ID_;
			}
			set
			{
				ID_ = value;
				gameObject.name = string.Format("bus_{0}", ID);
			}
		}

		[SLua.CustomLuaClassAttribute]
		public struct ProgressInfo
		{
			public Vector3 position;
			public float progress;

			[SLua.DoNotToLuaAttribute]
			public static readonly ProgressInfo Global = new ProgressInfo();
		}

		public Animator animator = null;
		public Carrier carrier = null;
		private Carrier[] carriers = null;

		public System.Action<ProgressInfo> progressListener = null;
		public System.Action<ProgressInfo> arrivedListener = null;
		bool _cloned;

		public int mainCarrierIndex = 1;
		public int defaultCarrierID = 0;

		public Vector3 busPosition
		{
			get
			{
				return null != carrier ? carrier.transform.position : transform.position;
			}
		}

		public void ActionEventPlayAudioEffect(int index)
		{
			if (null == carrier)
			{
				return;
			}
			carrier.PlayAudioEffect(index);
		}

		public void ActionEventPlayAction(string name)
		{
			if (null == carrier)
			{
				return;
			}
			carrier.PlayAction(name);
		}

		public void ActionEventNotify()
		{
			if (null != progressListener)
			{
				var info = ProgressInfo.Global;
				info.position = busPosition;
				info.progress = Mathf.Clamp01(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
				progressListener(info);
			}
		}

		public void ActionEventArrived()
		{
			if (null != arrivedListener)
			{
				var info = ProgressInfo.Global;
				info.position = busPosition;
				info.progress = 1;
				arrivedListener(info);
			}
		}

		public int GetSeatCount()
		{
			return null != carrier ? carrier.GetSeatCount() : 0;
		}

		public GameObject GetSeat(int seat)
		{
			return null != carrier ? carrier.GetSeat(seat) : null;
		}

#if OBSOLETE
		public bool GetOn(int seat, RoleAgent role)
		{
			return null != carrier && carrier.GetOn(seat, role);
		}

		public bool GetOff(RoleAgent role, Vector3 position)
		{
			return null != carrier && carrier.GetOff(role, position);
		}
#endif

		public int GetSeatCount(int carrierIndex)
		{
			var c = GetCarrier(carrierIndex);
			return null != c ? c.GetSeatCount() : 0;
		}
		
		public GameObject GetSeat(int carrierIndex, int seat)
		{
			var c = GetCarrier(carrierIndex);
			return null != c ? c.GetSeat(seat) : null;
		}

#if OBSOLETE
		public bool GetOn(int carrierIndex, int seat, RoleAgent role)
		{
			var c = GetCarrier(carrierIndex);
			return null != c && c.GetOn(seat, role);
		}
		
		public bool GetOff(int carrierIndex, RoleAgent role, Vector3 position)
		{
			var c = GetCarrier(carrierIndex);
			return null != c && c.GetOff(role, position);
		}
#endif

		public void Wait()
		{
			if (null != animator)
			{
				animator.Play ("buswait", -1, 0);
			}
		}

		public bool GO(int line, float progress = 0)
		{
			if (null == animator)
			{
				animator = GetComponent<Animator>();
				if (null == animator)
				{
					return false;
				}
			}
			var stateNameHash = Animator.StringToHash(string.Format("line{0}", line));
			if (!animator.HasState(0, stateNameHash))
			{
				return false;
			}
			animator.Play(stateNameHash, -1, progress);
			return true;
		}

		public void End()
		{
			if (null != animator)
			{
				animator.Play ("buswait", -1, 0);
			}
		}

		public Bus Copy()
		{
			var newBus = GameObject.Instantiate<Bus>(this);
			newBus.ClearPassengers();
			newBus._cloned = true;
			newBus.transform.parent = (transform.parent);
			newBus.animator = newBus.gameObject.GetComponent<Animator>();
			newBus.transform.localPosition = transform.localPosition;
			newBus.transform.localRotation = transform.localRotation;
			newBus.Wait ();
			return newBus;
		}

		public void RestoreDefaultCarriers()
		{
			SResetAllCarriers(ResourcePathHelper.IDCarrier(defaultCarrierID));
		}

		public void ClearPassengers()
		{
			if (null != carrier)
			{
				carrier.ClearPassengers();
			}
		}

		public void RefreshCarriers()
		{
			if (hungPoints.IsNullOrEmpty())
			{
				if (null == carrier)
				{
					carrier = GetComponentInChildren<Carrier>();
				}
			}
			else
			{
				carriers = new Carrier[hungPoints.Length];
				for (int i = 0; i < carriers.Length; ++i)
				{
					var hp = hungPoints[i];
					var c = null != hp ? hp.GetComponentInChildren<Carrier>() : null;
					carriers[i] = c;
					if (null == carrier && null != c)
					{
						carrier = c;
					}
				}
			}
		}

		public void ResetAllCarriers(ResourceID resID)
		{
			var prefab = ResourceManager.Loader.Load<Carrier>(resID);
			if (null == prefab)
			{
				return;
			}

			if (carriers.IsNullOrEmpty())
			{
				ExchangeCarrier(carrier, prefab, mainCarrierIndex);
			}
			else
			{
				for (int i = 0; i < carriers.Length; ++i)
				{
					var oldC = carriers[i];
					carriers[i] = ExchangeCarrier(oldC, prefab, i);
				}
			}
		}

		public void SResetAllCarriers(string resPath)
		{
			var prefab = ResourceManager.Loader.SLoad<Carrier>(resPath);
			if (null == prefab)
			{
				return;
			}
			
			if (carriers.IsNullOrEmpty())
			{
				ExchangeCarrier(carrier, prefab, mainCarrierIndex);
			}
			else
			{
				for (int i = 0; i < carriers.Length; ++i)
				{
					var oldC = carriers[i];
					carriers[i] = ExchangeCarrier(oldC, prefab, i);
				}
			}
		}

		private Carrier ExchangeCarrier(Carrier oldC, Carrier prefab, int index)
		{
			Transform carrierParent = null;
			if (null == oldC)
			{
				var go = GetHungPoint(index);
				if (null == go)
				{
					return null;
				}
				carrierParent = go.transform;
			}
			var newC = GameObject.Instantiate<Carrier>(prefab);
			newC.name = prefab.name;

			if (null != oldC)
			{
				newC.transform.parent = oldC.transform.parent;
				newC.transform.localPosition = oldC.transform.localPosition;
				newC.transform.localRotation = oldC.transform.localRotation;
				newC.transform.localScale = oldC.transform.localScale;
				GameObject.Destroy(oldC.gameObject);
			}
			else
			{
				newC.transform.ResetParent(carrierParent);
			}
			if (mainCarrierIndex == index)
			{
				carrier = newC;
			}
			return newC;
		}

		public Carrier GetCarrier(int index)
		{
			if (carriers.IsNullOrEmpty() || !carriers.CheckIndex(index))
			{
				return null;
			}
			return carriers[index];
		}

		#region HP
		public static GameObject[] GetHungPoints(GameObject obj)
		{
			return PointSubject.GetPoints(obj, HUNG_POINT_PREFIX);
		}
		
		[SerializeField, HideInInspector]
		private GameObject[] hungPoints_ = null;
		public GameObject[] hungPoints
		{
			get
			{
				return hungPoints_;
			}
		}
		public GameObject GetHungPoint(int index)
		{
			if (!hungPoints_.CheckIndex(index))
			{
				return null;
			}
			return hungPoints_[index];
		}
		public void RefreshHungPoints()
		{
			hungPoints_ = GetHungPoints(gameObject);
		}
		#endregion HP

		private Vector3 originCameraRotationOffset = Vector3.zero;
		private CameraController cameraController_ = null;
		private CameraController cameraController
		{
			get
			{
				return cameraController_;
			}
			set
			{
				if (value == cameraController)
				{
					return;
				}
				if (null != cameraController)
				{
					cameraController.cameraRotationEulerOffset = originCameraRotationOffset;
				}
				cameraController_ = value;
				if (null != cameraController)
				{
					originCameraRotationOffset = cameraController.cameraRotationEulerOffset;
				}
			}
		}
		public void CaptureCameraBegin(CameraController cc)
		{
			cameraController = cc;
		}
		public void CaptureCameraEnd()
		{
			cameraController = null;
		}

		void Reset()
		{
			RefreshHungPoints();
		}

		void Start()
		{
			if (!_cloned && !(null != BusManager.Me && BusManager.Me.Add(this)))
			{
				GameObject.Destroy(gameObject);
			}
			else
			{
				RefreshHungPoints();
				if (null == animator)
				{
					animator = GetComponent<Animator>();
				}
				RefreshCarriers();
				if (!_cloned)
				{
					RestoreDefaultCarriers();
				}
			}
		}

		void Update()
		{
			if (null != cameraController && null != carrier)
			{
				cameraController.cameraRotationEulerOffset = carrier.transform.eulerAngles;
				cameraController.ApplyCurrentInfo();
			}
		}

		void OnDestroy()
		{
			if (null != BusManager.Me && !_cloned)
			{
				BusManager.Me.Remove(this);
			}
		}
	
	}
} // namespace RO
