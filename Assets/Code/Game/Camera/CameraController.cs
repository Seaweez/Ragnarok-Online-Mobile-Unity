using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ghost.Attribute;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CameraController : SingleTonGO<CameraController>
	{
		public static CameraController Instance
		{
			get
			{
				return Me;
			}
		}
		
		public GameObject monoGameObject
		{
			get
			{
				return gameObject;
			}
		}

		public static CameraController singletonInstance{get;private set;}
		public System.Action<CameraController, bool> singletonChangedListener = null;
		public bool beSingleton
		{
			get
			{
				return this == Me;
			}
			set
			{
				if (value)
				{
					RegisterMe();
				}
				else
				{
					UnregisterMe();
				}
				if (null != singletonChangedListener)
				{
					singletonChangedListener(this, beSingleton);
				}
			}
		}

		[SLua.CustomLuaClassAttribute, System.Serializable]
		public class Info : System.ICloneable
		{
			public Transform focus = null;
			public Vector3 focusOffset = Vector3.zero;
			public Vector3 focusViewPort = new Vector3(0.5f, 0.4f, 23f);
			public Vector3 rotation = new Vector3(30f, -135f, 0f);
			public float fieldOfView = 0f;

           

            public Vector3 cameraPosition = Vector3.zero;
			public bool cameraPositionLocked = false;

			public bool fieldOfViewValid
			{
				get
				{
					return 0 < fieldOfView;
				}
			}

			public object Clone()
			{
				return MemberwiseClone();
			}
			
			public Info CloneSelf()
			{
				return Clone() as Info;
			}

			[SLua.DoNotToLuaAttribute]
			public static Info Lerp(Info i1, Info i2, float t)
			{
				var ret = new Info();
				Lerp(i1, i2, ret, t);
				return ret;
			}

			[SLua.DoNotToLuaAttribute]
			public static void Lerp(Info i1, Info i2, Info ret, float t)
			{
				if (i1.focus != i2.focus)
				{
					if (0.5f > t)
					{
						ret.focus = i1.focus;
						ret.focusOffset = i1.focusOffset;
						ret.focusViewPort = i1.focusViewPort;
					}
					else
					{
						ret.focus = i2.focus;
						ret.focusOffset = i2.focusOffset;
						ret.focusViewPort = i2.focusViewPort;
					}
				}
				else
				{
					ret.focus = i1.focus;
					ret.focusOffset = Vector3.Lerp(i1.focusOffset, i2.focusOffset, t);
					ret.focusViewPort = Vector3.Lerp(i1.focusViewPort, i2.focusViewPort, t);
				}

				ret.rotation = Vector3.Lerp(i1.rotation, i2.rotation, t);
				ret.fieldOfView = Mathf.Lerp(i1.fieldOfView, i2.fieldOfView, t);
			}
		}

		public float forceSmoothDuration = 0f;
		private int forceSmoothDurationValid_ = 0;
		public bool forceSmoothDurationValid
		{
			get
			{
				return 0 < forceSmoothDurationValid_;
			}
			set
			{
				if (value)
				{
					++forceSmoothDurationValid_;
				}
				else
				{
					--forceSmoothDurationValid_;
				}
			}
		}
		public bool delayForceSmoothDurationValid
		{
			set
			{
				StartCoroutine(DelaySetForceSmoothDurationValid(value));
			}
		}
		IEnumerator DelaySetForceSmoothDurationValid(bool value)
		{
			yield return new WaitForEndOfFrame();
			forceSmoothDurationValid = value;
		}
		[System.NonSerialized]
		public int disableSmoothRestore_ = 0;
		public bool disableSmoothRestore
		{
			get
			{
				return 0 < disableSmoothRestore_;
			}
			set
			{
				if (value)
				{
					++disableSmoothRestore_;
				}
				else
				{
					--disableSmoothRestore_;
				}
			}
		}

		public Vector3 cameraRotationEulerOffset = Vector3.zero;
		private Vector3 activeCameraRotationEuler
		{
			get
			{
				if (null != activeCamera)
				{
					return activeCamera.transform.rotation.eulerAngles-cameraRotationEulerOffset;
				}
				return Vector3.zero;
			}
			set
			{
				if (null != activeCamera)
				{
					activeCamera.transform.rotation = Quaternion.Euler(value+cameraRotationEulerOffset);
				}
			}
		}

		private Quaternion activeCameraRotation
		{
			get
			{
				if (Vector3.Equals(Vector3.zero, cameraRotationEulerOffset))
				{
					if (null != activeCamera)
					{
						return activeCamera.transform.rotation;
					}
				}
				return Quaternion.Euler(activeCameraRotationEuler);
			}
			set
			{
				activeCameraRotationEuler = value.eulerAngles;
			}
		}

		private Vector3 activeCameraPosition
		{
			get
			{
				if (null != activeCamera)
				{
					return activeCamera.transform.position-positionOffset;
				}
				return Vector3.zero;
			}
			set
			{
				if (null != activeCamera)
				{
					activeCamera.transform.position = value+positionOffset;
				}
			}
		}

		public Vector3 cameraPosition
		{
			get
			{
				if (null != currentInfo && currentInfo.cameraPositionLocked)
				{
					return currentInfo.cameraPosition;
				}
				return activeCameraPosition;
			}
		}

		public Quaternion cameraRotation
		{
			get
			{
				if (null != currentInfo)
				{
					return Quaternion.Euler(currentInfo.rotation);
				}
				return activeCameraRotation;
			}
		}

		public Vector3 cameraRotationEuler
		{
			get
			{
				if (null != currentInfo)
				{
					return currentInfo.rotation;
				}
				return activeCameraRotationEuler;
			}
		}

		public float cameraFieldOfView
		{
			get
			{
				if (null != currentInfo && 0 < currentInfo.fieldOfView)
				{
					return currentInfo.fieldOfView;
				}
				if (null != activeCamera)
				{
					return activeCamera.fieldOfView;
				}
				return 0f;
			}
		}

		public Transform focus
		{
			get
			{
				if (null != currentInfo)
				{
					return currentInfo.focus;
				}
				return null;
			}
			private set
			{
				if (null == currentInfo)
				{
					currentInfo = new Info();
				}
				currentInfo.focus = value;
				RefreshPuppetFocus();
			}
		}

		public Vector3 focusOffset
		{
			get
			{
				if (null != currentInfo)
				{
					return currentInfo.focusOffset;
				}
				return Vector3.zero;
			}
		}

		public Vector3 focusPosition
		{
			get
			{
				if (null != currentInfo)
				{
					return currentInfo.focus.position + currentInfo.focusOffset;
				}
				return Vector3.zero;
			}
		}

		public Vector3 focusViewPort
		{
			get
			{
				if (null != currentInfo)
				{
					return currentInfo.focusViewPort;
				}
				return Vector3.zero;
			}
		}

		public Vector3? cameraLockPosition
		{
			get
			{
				if (null != currentInfo && currentInfo.cameraPositionLocked)
				{
					return currentInfo.cameraPosition;
				}
				return null;
			}
		}

		[SerializeField, SetProperty("zoomMin")]
		private float zoomMin_ = 0.7f;
		public float zoomMin
		{
			get
			{
				return zoomMin_;
			}
			set
			{
				zoomMin_ = value;
				if (0 > zoomMin_)
				{
					zoomMin_ = 0;
				}
				else if (zoomMax < zoomMin_)
				{
					zoomMin_ = zoomMax;
				}
				if (zoomMinEx > zoomMin)
				{
					zoomMinEx = zoomMin;
				}
			}
		}
		[SerializeField, SetProperty("zoomMax")]
		private float zoomMax_ = 2.3f;
		public float zoomMax
		{
			get
			{
				return zoomMax_;
			}
			set
			{
				zoomMax_ = value;
				if (5 < zoomMax_)
				{
					zoomMax_ = 0;
				}
				else if (zoomMin > zoomMax_)
				{
					zoomMax_ = zoomMin;
				}
				if (zoomMaxEx < zoomMax)
				{
					zoomMaxEx = zoomMax;
				}
			}
		}
		
		[SerializeField, SetProperty("zoom")]
		private float zoom_ = 1.0f;
		public float zoom
		{
			get
			{
				return zoom_;
			}
			set
			{
				zoom_ = Mathf.Clamp(value, zoomMin, zoomMax);
				if (zoomMin > value)
				{
					var zoomEx = zoomMin-zoomMinEx;
					var x = zoomMin-value+1;
					var y = zoomMin-(zoomEx-zoomEx/x);
					zoom_ = y;
				}
				else if (zoomMax < value)
				{
					var zoomEx = zoomMaxEx-zoomMax;
					var x =  value-zoomMax+1;
					var y = zoomMax+(zoomEx-zoomEx/x);
					zoom_ = y;
				}
				else
				{
					zoom_ = value;
				}
				ApplyCurrentInfo();
			}
		}

		public float forceZoom
		{
			set
			{
				zoom_ = value;
				ApplyCurrentInfo();
			}
		}

		[SerializeField, SetProperty("zoomMinEx")]
		private float zoomMinEx_ = 0.7f;
		public float zoomMinEx
		{
			get
			{
				return zoomMinEx_;
			}
			set
			{
				zoomMinEx_ = Mathf.Clamp(value, 0, zoomMin);
			}
		}

		[SerializeField, SetProperty("zoomMaxEx")]
		private float zoomMaxEx_ = 2.3f;
		public float zoomMaxEx
		{
			get
			{
				return zoomMaxEx_;
			}
			set
			{
				zoomMaxEx_ = value;
				if (zoomMax > value)
				{
					zoomMaxEx_ = zoomMax;
				}
				else
				{
					zoomMaxEx_ = value;
				}
			}
		}

		[SerializeField, SetProperty("activeCamera")]
		private Camera camera_ = null;
		public Camera activeCamera
		{
			get
			{
				return camera_;
			}
			set
			{
				camera_ = value;
				ApplyCurrentInfo();
			}
		}
		private CameraExtraInfo[] cameras = null;
		private int updateCameras_ = 0;
		public bool updateCameras
		{
			get
			{
				return 0 < updateCameras_;
			}
			set
			{
				if (value)
				{
					++updateCameras_;
				}
				else
				{
					--updateCameras_;
				}
			}
		}

		public Vector3 targetFocusOffset
		{
			get
			{
				if (null != smoothFocusTo && smoothFocusTo.running)
				{
					return smoothFocusTo.targetFocusOffset;
				}
				return focusOffset;
			}
		}
		public Vector3 targetFocusViewPort
		{
			get
			{
				if (null != smoothFocusTo && smoothFocusTo.running)
				{
					return smoothFocusTo.targetFocusViewPort;
				}
				return focusViewPort;
			}
		}
		public Quaternion targetRotation
		{
			get
			{
				if (null != smoothRotateTo && smoothRotateTo.running)
				{
					return smoothRotateTo.targetRotation;
				}
				return cameraRotation;
			}
		}
		public Vector3 targetRotationEuler
		{
			get
			{
				if (null != smoothRotateTo && smoothRotateTo.running)
				{
					return smoothRotateTo.targetRotation.eulerAngles;
				}
				return cameraRotationEuler;
			}
		}
		public float targetFieldOfView
		{
			get
			{
				if (null != smoothFieldOfViewTo && smoothFieldOfViewTo.running)
				{
					return smoothFieldOfViewTo.targetFieldOfView;
				}
				return cameraFieldOfView;
			}
		}
		public float targetZoom
		{
			get
			{
				if (null != smoothZoomTo && smoothZoomTo.running)
				{
					return smoothZoomTo.targetZoom;
				}
				return zoom;
			}
		}

		public Info defaultInfo = null;
		private Info currentInfo_ = null;
		public Info currentInfo
		{
			get
			{
				return currentInfo_;
			}
			private set
			{
				if (value == currentInfo)
				{
					return;
				}
				currentInfo_ = value;
				RefreshPuppetFocus();
			}
		}
		public float focusObjYOffset = 0.3f;
		public Info photographInfo = null;
		public float photographSwitchDuration = 0.3f;
		public float photographZoomMin = 1;
		public float photographZoomMax = 1;

		public Vector3 positionOffset = Vector3.zero;

		public GameObject[] childrenPrefabs;

		public bool allowLowerThanFocus = false;
		public float smoothFocusToMaxDistance = 2;

		private Vector3 followOffset;
		private bool currentInfoDirty = false;

		private Dictionary<Transform, Transform> puppetFocus = null;
		private Transform currentPuppetSource = null;

		public void SetPuppetFocus(Transform source, Transform puppet, float duration)
		{
			if (null != puppet)
			{
				if (null == puppetFocus)
				{
					puppetFocus = new Dictionary<Transform, Transform>();
				}
				puppetFocus[source] = puppet;
				if (focusToRunning)
				{
					if (smoothFocusTo.focus == source)
					{
						FocusTo(source, smoothFocusTo.targetFocusOffset, smoothFocusTo.targetFocusViewPort, duration);
					}
				}
				else
				{
					if (null != currentInfo && currentInfo.focus == source)
					{
						FocusTo(source, currentInfo.focusOffset, currentInfo.focusViewPort, duration);
					}
				}
			}
			else
			{
				if (null != puppetFocus && puppetFocus.Remove(source))
				{
					if (focusToRunning)
					{
						if (currentPuppetSource == source)
						{
							FocusTo(source, smoothFocusTo.targetFocusOffset, smoothFocusTo.targetFocusViewPort, duration);
						}
					}
					else
					{
						if (currentPuppetSource == source)
						{
							FocusTo(source, currentInfo.focusOffset, currentInfo.focusViewPort, duration);
						}
					}
				}
			}
		}

		private void RefreshPuppetFocus()
		{
			if (null != currentInfo.focus)
			{
				var puppet = TryGetPuppetFocus(currentInfo.focus);
				if (currentInfo.focus != puppet)
				{
					currentPuppetSource = currentInfo.focus;
					currentInfo.focus = puppet;
				}
				else
				{
					currentPuppetSource = null;
				}
			}
			else
			{
				currentPuppetSource = null;
			}
		}
		
		public Transform TryGetPuppetFocus(Transform focus)
		{
			if (null != focus && null != puppetFocus)
			{
				Transform puppet;
				if (puppetFocus.TryGetValue(focus, out puppet))
				{
					return puppet;
				}
			}
			return focus;
		}

		public bool CameraRotationEquals(Quaternion rotation)
		{
			var angle = Quaternion.Angle(cameraRotation, rotation);
			return 0.1 >= angle;
		}

		public bool CameraRotationEquals(Vector3 rotation)
		{
			return CameraRotationEquals(Quaternion.Euler(rotation));
		}

		private void ResetCamera()
		{
			if (null == activeCamera)
			{
				activeCamera = GetComponent<Camera>();
				if (null == activeCamera)
				{
					activeCamera = Camera.main;
				}
			}
		}

		public void UpdateCameras()
		{
			if (null == activeCamera)
			{
				return;
			}
			if (cameras.IsNullOrEmpty())
			{
				return;
			}
			for (int i = 0; i < cameras.Length; ++i)
			{
				var c = cameras[i];
				if (null != c.camera)
				{
					c.camera.fieldOfView = (activeCamera.fieldOfView+c.fieldOfViewExtra)*c.fieldOfViewScale;
				}
			}
		}

		public void ApplyCurrentInfo()
		{
			currentInfoDirty = true;
		}

		private void DoApplyCurrentInfo()
		{
			if (null == activeCamera)
			{
				return;
			}
			if (null == currentInfo)
			{
				return;
			}
			
			activeCameraRotationEuler = currentInfo.rotation;
			if (0 < currentInfo.fieldOfView)
			{
				activeCamera.fieldOfView = currentInfo.fieldOfView;
				UpdateCameras();
			}
			
			if (null != currentInfo.focus)
			{
				var fvp = currentInfo.focusViewPort;
				var focusObjPosition = currentInfo.focus.position;
				var fo = currentInfo.focusOffset;

				var vp = ViewPortApplyZoom(fvp, zoom);
				var from = activeCamera.ViewportToWorldPoint(vp);
				var to = currentInfo.focus.TransformPoint(fo);
				to.x = (focusObjPosition.x+to.x)/2.0f;
				to.z = (focusObjPosition.z+to.z)/2.0f;

				if (0 < smoothFocusToMaxDistance && null != smoothFocusTo && smoothFocusTo.running && smoothFocusToMaxDistance < Vector3.Distance(from, to))
				{
					smoothFocusTo.End(this, true);

					fvp = currentInfo.focusViewPort;
					focusObjPosition = currentInfo.focus.position;
					fo = currentInfo.focusOffset;

					vp = ViewPortApplyZoom(fvp, zoom);
					from = activeCamera.ViewportToWorldPoint(vp);
					to = currentInfo.focus.TransformPoint(fo);
					to.x = (focusObjPosition.x+to.x)/2.0f;
					to.z = (focusObjPosition.z+to.z)/2.0f;
				}
				var newPosition = activeCameraPosition + (to-from);

				if (currentInfo.focusOffset.y > focusObjYOffset)
				{
					focusObjPosition.y += focusObjYOffset;
				}
				if (!allowLowerThanFocus && focusObjPosition.y <= to.y && focusObjPosition.y > newPosition.y)
				{
					var newForwardPosition = newPosition + (to-newPosition);
					var disableRange = Vector2.Distance(to.XZ(), focusObjPosition.XZ());
					var forwardDistance = Vector3.Distance(newPosition, newForwardPosition);


					if (disableRange < forwardDistance)
					{
						var offsetForward = 1-disableRange/forwardDistance;
						if (focusObjPosition.y < to.y)
						{
							offsetForward = Mathf.Clamp((focusObjPosition.y-newPosition.y)/(to.y-newPosition.y), 0, offsetForward);
						}
						newPosition += (to-newPosition)*offsetForward;
					}
				}

				followOffset = newPosition-to;
				
				if (null != AudioManager.Me)
				{
					AudioManager.Me.SetListenerFollow(currentInfo.focus);
				}
			}
		}

		public void SetDefault()
		{
			currentInfo = (null != defaultInfo) ? defaultInfo.CloneSelf() : null;
			ResetCamera();
			ApplyCurrentInfo();
		}

		public void SetInfo(Info info)
		{
			currentInfo = info.CloneSelf();
			ResetCamera();
			ApplyCurrentInfo();
		}

		#region resetter
		private void ResetCurrentInfo()
		{
			if (null != activeCamera)
			{
				if (null != currentInfo.focus)
				{
					currentInfo.focusViewPort = ViewPortUnapplyZoom(activeCamera.WorldToViewportPoint(focusPosition), zoom);
				}
				currentInfo.rotation = activeCameraRotationEuler;
			}
		}

		public void ResetCurrentInfoByFocus(Transform newFocus = null)
		{
			focus = newFocus;

			if (null == activeCamera)
			{
				ResetCamera();
			}
			ResetCurrentInfo();
			ApplyCurrentInfo();
		}
		public void ResetCurrentInfoByZoom(float newZoom = 1f)
		{
			if (zoom == newZoom)
			{
				return;
			}
			if (null == currentInfo)
			{
				ResetCurrentInfoByFocus();
			}
			var oldZoom = zoom;
			zoom_ = Mathf.Clamp(newZoom, zoomMin, zoomMax);
			currentInfo.focusViewPort.z *= (newZoom/oldZoom);
			ApplyCurrentInfo();
			InterruptSmoothZoom();
		}
		public void ResetZoomMinMax(float min, float max)
		{
			if (min > max)
			{
				return;
			}
			zoomMin_ = min;
			zoomMax_ = max;
			ResetCurrentInfoByZoom(Mathf.Clamp(zoom, zoomMin, zoomMax));
		}

		public void ResetFocusOffset(Vector3 focusOffset)
		{
			if (null == currentInfo)
			{
				ResetCurrentInfoByFocus();
			}
			if (!Vector3.Equals(currentInfo.focusOffset, focusOffset))
			{
				currentInfo.focusOffset = focusOffset;
				ApplyCurrentInfo();
			}
		}

		public void ResetFocusViewPort(Vector3 focusViewPort)
		{
			if (null == currentInfo)
			{
				ResetCurrentInfoByFocus();
			}
			if (!Vector3.Equals(currentInfo.focusViewPort, focusViewPort))
			{
				currentInfo.focusViewPort = focusViewPort;
				ApplyCurrentInfo();
			}
		}

		public void ResetRotation(Quaternion rotation)
		{
			if (null == currentInfo)
			{
				ResetCurrentInfoByFocus();
			}
			if (!CameraRotationEquals(rotation))
			{
				currentInfo.rotation = rotation.eulerAngles;
				ApplyCurrentInfo();
			}
		}

		public void ResetRotation(Vector3 rotation)
		{
			if (null == currentInfo)
			{
				ResetCurrentInfoByFocus();
			}
			if (!CameraRotationEquals(rotation))
			{
				currentInfo.rotation = rotation;
				ApplyCurrentInfo();
			}
		}

		public void ResetFieldOfView(float fieldOfView)
		{
			if (null == currentInfo)
			{
				ResetCurrentInfoByFocus();
			}
			if (cameraFieldOfView != fieldOfView)
			{
				currentInfo.fieldOfView = fieldOfView;
				ApplyCurrentInfo();
			}
		}

		public void ResetLockPosition(Vector3 p)
		{
			if (null == currentInfo)
			{
				ResetCurrentInfoByFocus();
			}
			currentInfo.cameraPositionLocked = true;
			if (!Vector3.Equals(cameraPosition, p))
			{
				currentInfo.cameraPosition = p;
			}
		}
		#endregion resetter

		public void ForceApplyCurrentInfo()
		{
			DoApplyCurrentInfo();
			currentInfoDirty = false;
		}

		public void UpdatePosition()
		{
			if (null != focus && null != activeCamera)
			{
				var lockPosition = cameraLockPosition;
				if (null != lockPosition)
				{
					activeCameraPosition = (Vector3)lockPosition;

					var p0 = activeCamera.transform.position;
					var p1 = activeCamera.ViewportToWorldPoint(focusViewPort);
					var p2 = focusPosition;

					var v1 = p1-p0;
					var v2 = p2-p0;
					var normal = Vector3.Cross(v1, v2);
					activeCamera.transform.RotateAround(p0, normal, Vector3.Angle(v1, v2));
					currentInfo.rotation = activeCamera.transform.eulerAngles;
					// bad way
//					activeCamera.transform.LookAt(focusPosition);
//					ResetCurrentInfo();
				}
				else
				{
					activeCameraPosition = focusPosition + followOffset;
				}
			}
		}

		public static Vector3 ViewPortUnapplyZoom(Vector3 viewPort, float zoom)
		{
			var vp = viewPort;
			if (0 < zoom)
			{
				vp.z *= zoom;
			}
			return vp;
		}

		public static Vector3 ViewPortApplyZoom(Vector3 viewPort, float zoom)
		{
			var vp = viewPort;
			if (0 < zoom)
			{
				var scale = 1.0f / zoom;
				vp.z *= scale;
			}
			return vp;
		}

		void Reset()
		{
			ResetCamera();
		}

		protected override void Awake ()
		{
			base.Awake ();
			singletonInstance = CameraController.Me;
			if (!childrenPrefabs.IsNullOrEmpty())
			{
				foreach (var prefab in childrenPrefabs)
				{
					var go = GameObject.Instantiate(prefab);
					go.name = prefab.name;
					go.transform.parent = transform;
					go.transform.localPosition = Vector3.zero;
					go.transform.localRotation = Quaternion.identity;
				}
			}
		}

		protected override void OnDestroy ()
		{
			if (this == singletonInstance)
			{
				singletonInstance = null;
			}
			base.OnDestroy ();
		}

		void Start()
		{
			if (null != activeCamera)
			{
				cameras = activeCamera.GetComponentsInChildren<CameraExtraInfo>();
			}
//			if (null == currentInfo)
//			{
//				SetDefault();
//			}
		}

		#region smooth
		private List<CameraSmooth> smoothes = new List<CameraSmooth>();
		public void ApplySmooth(CameraSmooth smooth)
		{
			if (smoothes.Contains(smooth))
			{
				return;
			}
			smoothes.Add (smooth);
		}
		void LateUpdate()
		{
			for (int i = smoothes.Count-1; i >= 0; --i)
			{
				var smooth = smoothes[i];
				smooth.Update(this);
				if (!smooth.running)
				{
					smoothes.RemoveAt(i);
					smooth.Destroy();
				}
			}

			if (currentInfoDirty)
			{
				DoApplyCurrentInfo();
				currentInfoDirty = false;
			}

			UpdatePosition();

			if (updateCameras)
			{
				UpdateCameras();
			}
		}

		private CameraSmoothFocusTo smoothFocusTo = null;
		private CameraSmoothRotateTo smoothRotateTo = null;
		private CameraSmoothZoomTo smoothZoomTo = null;
		private CameraSmoothFieldOfViewTo smoothFieldOfViewTo = null;
		private CameraSmoothPositionTo smoothPositionTo = null;

		public bool smoothToRunning
		{
			get
			{
				return focusToRunning || rotateToRunning || fieldOfViewToRunning || positionToRunning;
			}
		}

		public bool focusToRunning
		{
			get
			{
				return null != smoothFocusTo && smoothFocusTo.running;
			}
		}

		public bool rotateToRunning
		{
			get
			{
				return null != smoothRotateTo && smoothRotateTo.running;
			}
		}

		public bool fieldOfViewToRunning
		{
			get
			{
				return null != smoothFieldOfViewTo && smoothFieldOfViewTo.running;
			}
		}

		public bool positionToRunning
		{
			get
			{
				return null != smoothPositionTo && smoothPositionTo.running;
			}
		}

		public void FocusTo(
			Transform focus, 
			Vector3 targetFocusOffset,
			Vector3 targetFocusViewPort, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			if (null == smoothFocusTo)
			{
				smoothFocusTo = CameraSmoothFocusTo.Create(focus, targetFocusOffset, targetFocusViewPort, duration, listener);
			}
			else
			{
				smoothFocusTo.End(this);
				
				smoothFocusTo.focus = focus;
				smoothFocusTo.targetFocusOffset = targetFocusOffset;
				smoothFocusTo.targetFocusViewPort = targetFocusViewPort;
				smoothFocusTo.duration = duration;
				smoothFocusTo.listener = listener;
			}
			smoothFocusTo.Launch(this);
		}

		public void FocusTo(
			Transform focus, 
			Vector3 targetFocusViewPort, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			FocusTo(focus, Vector3.zero, targetFocusViewPort, duration, listener);
		}
		public void FocusTo(
			Vector3 targetFocusOffset,
			Vector3 targetFocusViewPort, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			FocusTo(null, targetFocusOffset, targetFocusViewPort, duration, listener);
		}
		public void FocusTo(
			Vector3 targetFocusViewPort, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			FocusTo(Vector3.zero, targetFocusViewPort, duration, listener);
		}
		public void FocusTo(
			Transform focus, 
			Vector3 targetFocusOffset, 
			Vector3 targetFocusViewPort)
		{
			if (focusToRunning)
			{
				smoothFocusTo.focus = focus;
				smoothFocusTo.targetFocusOffset = targetFocusOffset;
				smoothFocusTo.targetFocusViewPort = targetFocusViewPort;
			}
			else
			{
				if (null != focus)
				{
					ResetCurrentInfoByFocus(focus);
				}
				ResetFocusOffset(targetFocusOffset);
				ResetFocusViewPort(targetFocusViewPort);
			}
		}

		public void RotateTo(
			Quaternion targetRotation, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			if (null == smoothRotateTo)
			{
				smoothRotateTo = CameraSmoothRotateTo.Create(targetRotation, duration, listener);
			}
			else
			{
				smoothRotateTo.End(this);

				smoothRotateTo.targetRotation = targetRotation;
				smoothRotateTo.duration = duration;
				smoothRotateTo.listener = listener;
			}
			smoothRotateTo.Launch(this);
		}

		public void RotateTo(
			Vector3 targetRotation, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			RotateTo(Quaternion.Euler(targetRotation), duration, listener);
		}

		public void RotateTo(Quaternion targetRotation)
		{
			if (rotateToRunning)
			{
				smoothRotateTo.targetRotation = targetRotation;
			}
			else
			{
				ResetRotation(targetRotation);
			}
		}
		public void RotateTo(Vector3 targetRotation)
		{
			RotateTo(Quaternion.Euler(targetRotation));
		}
		public void FieldOfViewTo(
			float targetFieldOfView, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			if (0 >= targetFieldOfView)
			{
				return;
			}
			if (null == smoothFieldOfViewTo)
			{
				smoothFieldOfViewTo = CameraSmoothFieldOfViewTo.Create(targetFieldOfView, duration, listener);
			}
			else
			{
				smoothFieldOfViewTo.End(this);
				
				smoothFieldOfViewTo.targetFieldOfView = targetFieldOfView;
				smoothFieldOfViewTo.duration = duration;
				smoothFieldOfViewTo.listener = listener;
			}
			smoothFieldOfViewTo.Launch(this);
		}
		public void FieldOfViewTo(float targetFieldOfView)
		{
			if (0 >= targetFieldOfView)
			{
				return;
			}
			if (fieldOfViewToRunning)
			{
				smoothFieldOfViewTo.targetFieldOfView = targetFieldOfView;
			}
			else
			{
				ResetFieldOfView(targetFieldOfView);
			}
		}
		public void PositionTo(
			Vector3 targetPosition, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			if (null == smoothPositionTo)
			{
				smoothPositionTo = CameraSmoothPositionTo.Create(targetPosition, duration, listener);
			}
			else
			{
				smoothPositionTo.End(this);
				
				smoothPositionTo.targetPosition = targetPosition;
				smoothPositionTo.duration = duration;
				smoothPositionTo.listener = listener;
			}
			smoothPositionTo.Launch(this);
		}
		public void PositionTo(Vector3 targetPosition)
		{
			if (positionToRunning)
			{
				smoothPositionTo.targetPosition = targetPosition;
			}
			else
			{
				ResetLockPosition(targetPosition);
			}
		}

		public void SmoothTo(Info info, float duration, System.Action<CameraController> listener = null)
		{
			FocusTo(info.focus, info.focusOffset, info.focusViewPort, duration, listener);
			RotateTo(info.rotation, duration, listener);
			FieldOfViewTo(info.fieldOfView, duration, listener);
			if (info.cameraPositionLocked)
			{
				PositionTo(info.cameraPosition, duration, listener);
			}
			else
			{
				if (null != smoothPositionTo)
				{
					smoothPositionTo.End(this);
				}
				if (null != currentInfo)
				{
					currentInfo.cameraPositionLocked = false;
				}
			}
		}
		
		public void SmoothTo3D(Info info, float duration, System.Action<CameraController> listener = null)
		{
			FocusTo(info.focus, info.focusOffset, info.focusViewPort, duration, listener);
			FieldOfViewTo(info.fieldOfView, duration, listener);
			if (info.cameraPositionLocked)
			{
				PositionTo(info.cameraPosition, duration, listener);
			}
			else
			{
				if (null != smoothPositionTo)
				{
					smoothPositionTo.End(this);
				}
				if (null != currentInfo)
				{
					currentInfo.cameraPositionLocked = false;
				}
			}
		}

		public void SmoothTo(Info info)
		{
			FocusTo(info.focus, info.focusOffset, info.focusViewPort);
			RotateTo(info.rotation);
			FieldOfViewTo(info.fieldOfView);
			if (info.cameraPositionLocked)
			{
				PositionTo(info.cameraPosition);
			}
			else
			{
				if (null != smoothPositionTo)
				{
					smoothPositionTo.End(this);
				}
				currentInfo.cameraPositionLocked = false;
			}
		}

		public void InterruptSmoothTo()
		{
			if (null != smoothFocusTo)
			{
				smoothFocusTo.End(this, true);
			}
			if (null != smoothPositionTo)
			{
				smoothPositionTo.End(this, true);
			}
			if (null != smoothRotateTo)
			{
				smoothRotateTo.End(this, true);
			}
			if (null != smoothZoomTo)
			{
				smoothZoomTo.End(this, true);
			}
		}

		public void RestoreDefault(float duration, System.Action<CameraController> listener = null)
		{
			if (null == defaultInfo)
			{
				return;
			}
			if (disableSmoothRestore)
			{
				disableSmoothRestore = false;
				SetDefault();
				if (null != listener)
				{
					listener(this);
				}
			}
			else if (0 < duration)
			{
				SmoothTo(defaultInfo, duration, listener);
			}
			else
			{
				SetDefault();
				if (null != listener)
				{
					listener(this);
				}
			}
		}

		public void ZoomTo(
			float targetZoom, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			if (null == smoothZoomTo)
			{
				smoothZoomTo = CameraSmoothZoomTo.Create(targetZoom, duration, listener);
			}
			else
			{
				smoothZoomTo.End(this);
				
				smoothZoomTo.targetZoom = targetZoom;
				smoothZoomTo.duration = duration;
				smoothZoomTo.listener = listener;
			}
			smoothZoomTo.Launch(this);
		}

		public void RestoreZoom(float duration, System.Action<CameraController> listener = null)
		{
			if (zoomMin > zoom)
			{
				ZoomTo(zoomMin, duration, listener);
			}
			else if (zoomMax < zoom)
			{
				ZoomTo(zoomMax, duration, listener);
			}
		}

		public void InterruptSmoothZoom()
		{
			if (null != smoothZoomTo)
			{
				smoothZoomTo.End(this);
			}
		}
		#endregion smooth
	}

	[SLua.CustomLuaClassAttribute]
	public abstract class CameraSmooth
	{
		public System.Action<CameraController> listener = null;
       // public InputManager inputManager;
        public static T CreateT<T>(float duration, System.Action<CameraController> listener = null) where T:CameraSmooth
		{
			var obj = ObjectPool.getObject<T>();
			obj.Reset();
			obj.duration = duration;
			obj.listener = listener;
			return obj;
		}

		public void Destroy()
		{
			ObjectPool.addToPool(this);
		}

		protected struct State
		{
			public float duration;
			public float escaped;
			public float dampVelocity;

			public void Reset(float d = 0f)
			{
				duration = d;
				escaped = 0f;
				dampVelocity = 0f;
			}
		}

		protected State state;
		
		public float duration = 1f;
		public bool running{get;private set;}
		
		public void Reset()
		{
			running = false;
		}

		protected void Start(CameraController controller)
		{
			if (running)
			{
				return;
			}

			/*if (InputManager.Me.photographMode == PhotographMode.CAM3D)
			{
                InputManager.Me.photograph.Enter3D(PhotographMode.CAM3D);
			}*/
			if (controller.forceSmoothDurationValid)
			{
				duration = controller.forceSmoothDuration;
			}
			if (0 < duration)
			{
				running = true;
				state.Reset(duration);

				controller.ApplySmooth(this);
			}
			else
			{
				DoUpdate(controller, 1);
				NotifyFinished(controller);
			}
		}

		public void End(CameraController controller, bool set = false)
		{
			if (!running)
			{
				return;
			}
			if (set)
			{
				DoUpdate(controller, 1);
			}
			running = false;
			NotifyFinished(controller);
		}

		public void Update(CameraController controller)
		{
			if (!running)
			{
				return;
			}
			state.escaped = Mathf.SmoothDamp(
				state.escaped, 
				state.duration, 
				ref state.dampVelocity, 
				state.duration);
			var progress = state.escaped / state.duration;
			DoUpdate(controller, Mathf.Clamp01(progress));

			if (0.001 >= (state.duration-state.escaped))
			{
				End (controller);
			}
		}

		protected abstract void DoUpdate(CameraController controller, float progress);

		protected void NotifyFinished(CameraController controller)
		{
			if (null != listener)
			{
				listener(controller);
			}
		}
	}


} // namespace RO
