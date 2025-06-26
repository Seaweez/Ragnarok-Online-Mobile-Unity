using UnityEngine;
using System.Collections.Generic;

namespace RO
{
    [SLua.CustomLuaClassAttribute]
    public enum TouchEventType
    {
        BEGIN,
        MOVE,
        HOLD,
        END
    }

    [SLua.CustomLuaClassAttribute]
    public class InputInfo
    {
        public static readonly InputInfo Global = new InputInfo();

        public TouchEventType touchEventType;
        public int pointerID;
        public Vector3 touchPoint;
        public bool beginOnUI;
        public bool overUI;
    }

    [SLua.CustomLuaClassAttribute]
    public enum PhotographMode
    {
        SELFIE,
        PHOTOGRAPHER,
        CAM3D,
    }

    [SLua.CustomLuaClassAttribute]
    public class Photograph
    {
        public bool running { get; private set; }
        public bool ready { get; private set; }

        private float originalZoom = 1;
        private float originalZoomMin = 1;
        private float originalZoomMax = 1;

        public CameraController cameraController = null;

        public Vector3 forceRotation = Vector3.zero;
        public bool useForceRotation = false;

        public PhotographMode mode { get; private set; }
        private int readyNumber = 0;

        private CameraController.Info GetSelfieCameraInfo()
        {
            var info = cameraController.photographInfo.CloneSelf();
            var angle = info.focus.rotation.eulerAngles;
            var y = angle.y + 180f;
            y = y - Mathf.Floor(y / 360f) * 360f;
            info.rotation.y = y;

            var role = info.focus.GetComponent<RoleComplete>();
            if (null != role)
            {
                var cp = role.GetCP(Config.RolePoint.CONNECT_HAIR);
                if (null != cp)
                {
                    info.focusOffset = cp.transform.position - role.transform.position;
                }
            }
            return info;
        }

        private CameraController.Info GetPhotographerCameraInfo()
        {
            var info = cameraController.photographInfo.CloneSelf();
            var angle = info.focus.rotation.eulerAngles;
            var y = angle.y;
            y = y - Mathf.Floor(y / 360f) * 360f;
            info.rotation.y = y;

            var role = info.focus.GetComponent<RoleComplete>();
            if (null != role)
            {
                var cp = role.GetCP(Config.RolePoint.CONNECT_HAIR);
                if (null != cp)
                {
                    info.focusOffset = cp.transform.position - role.transform.position;
                }
            }

            info.focusViewPort.z = 0;
            return info;
        }

        private CameraController.Info GetStandardMMORPGCameraInfo()
        {
            var info = cameraController.photographInfo.CloneSelf();
            var angle = info.focus.rotation.eulerAngles;
            var y = angle.y + 180f; // Adjusting angle to get the opposite direction
            y = y - Mathf.Floor(y / 360f) * 360f; // Normalizing the angle
            info.rotation.y = y;

            var role = info.focus.GetComponent<RoleComplete>();
            if (null != role)
            {
                var cp = role.GetCP(Config.RolePoint.CONNECT_BODY);
                if (null != cp)
                {
                    // Initially set focusOffset based on the connection point's position
                    info.focusOffset = cp.transform.position - role.transform.position;
                }
            }

            //info.fieldOfView = 23;
            info.focusViewPort.z = 23;

            return info;
        }



        private void ApplyMode(PhotographMode m)
        {
            if (!running)
            {
                return;
            }

            if (null == cameraController)
            {
                return;
            }

            mode = m;

            ready = false;
            ++readyNumber;

            CameraController.Info cameraInfo = null;
            switch (mode)
            {
                case PhotographMode.SELFIE:
                    cameraInfo = GetSelfieCameraInfo();
                    break;
                case PhotographMode.PHOTOGRAPHER:
                    cameraInfo = GetPhotographerCameraInfo();
                    break;
            }
            if (useForceRotation)
            {
                cameraInfo.rotation = forceRotation;
            }
            var readyNumberThisSession = readyNumber;
            cameraController.SmoothTo(
                cameraInfo, cameraController.photographSwitchDuration,
                delegate (CameraController cc) {
                    if (running && readyNumber == readyNumberThisSession)
                    {
                        ready = true;
                    }
                });
        }

        private void ApplyMode3D(PhotographMode m)
        {
            if (!running)
            {
                return;
            }

            if (null == cameraController)
            {
                return;
            }

            mode = m;

            ready = false;
            ++readyNumber;

            CameraController.Info cameraInfo = null;
            switch (mode)
            {
                case PhotographMode.CAM3D:
                    cameraInfo = GetStandardMMORPGCameraInfo();
                    break;

            }
            if (useForceRotation)
            {
                cameraInfo.rotation = forceRotation;
            }
            var readyNumberThisSession = readyNumber;
            cameraController.SmoothTo3D(
                cameraInfo, cameraController.photographSwitchDuration,
                delegate (CameraController cc) {
                    if (running && readyNumber == readyNumberThisSession)
                    {
                        ready = true;
                    }
                });

        }

        public void Enter(PhotographMode m)
        {
            if (running)
            {
                if (mode != m)
                {
                    ApplyMode(m);
                }
                return;
            }
            cameraController = CameraController.Me;
            if (null == cameraController)
            {
                return;
            }

            running = true;

            originalZoom = cameraController.zoom;
            originalZoomMin = cameraController.zoomMin;
            originalZoomMax = cameraController.zoomMax;
            cameraController.ResetCurrentInfoByZoom(1);
            cameraController.ResetZoomMinMax(cameraController.photographZoomMin, cameraController.photographZoomMax);

            ApplyMode(m);

            cameraController.beSingleton = false;
            LuaLuancher.Me.ignoreAreaTrigger = true;
        }

        public void Enter3D(PhotographMode m)
        {

            if (running)
            {
                if (mode != m)
                {
                    ApplyMode3D(m);
                }
                return;
            }
            cameraController = CameraController.Me;
            if (null == cameraController)
            {
                return;
            }

            running = true;

            cameraController.ResetCurrentInfoByZoom(cameraController.zoom);
            cameraController.ResetZoomMinMax(cameraController.zoomMinEx, cameraController.zoomMaxEx);

            ApplyMode3D(m);
            CameraPointManager.Me.enabled = false;
            cameraController.beSingleton = false;
            //  LuaLuancher.Me.ignoreAreaTrigger = true;
        }

        public void Exit()
        {
            if (!running)
            {
                return;
            }
            running = false;
            ready = false;
            useForceRotation = false;

            if (null != cameraController)
            {
                cameraController.forceSmoothDuration = cameraController.photographSwitchDuration;
                cameraController.forceSmoothDurationValid = true;

                cameraController.ResetCurrentInfoByZoom(originalZoom);
                cameraController.ResetZoomMinMax(originalZoomMin, originalZoomMax);
                cameraController.RestoreDefault(cameraController.photographSwitchDuration, delegate (CameraController cc) {
                    cc.delayForceSmoothDurationValid = false;
                });

                cameraController.beSingleton = true;
            }

            LuaLuancher.Me.ignoreAreaTrigger = false;
        }
    }

    [SLua.CustomLuaClassAttribute]
    public class InputManager : SingleTonGO<InputManager>
    {
        [SLua.CustomLuaClassAttribute]
        public enum Model
        {
            DEFAULT,
            PHOTOGRAPH,
            CAMERA3D,
        }

        public delegate void InputListener(InputInfo info);
        public InputListener extraInputListener = null;
        public InputListener extraInputListenerLater = null;

        public float DEFAULT_SCREEN_SIZE_INCH = 4;
        public float touchSenseInch = 0.1f;
        public float zoomOneInch = 1f;
        public float fovZoomOneInch = 10f;
        public float angleOneInch = 0.03f;

        public Vector2 cameraRotationMin = new Vector2(10, -180);
        public Vector2 cameraRotationMax = new Vector2(80, 180);
        public int gyroDelay = 10;
        private Vector3 targetRotation;

        public float cameraFieldOfViewMin = 18;
        public float cameraFieldOfViewMax = 63;

        public int clickGroundEffectID = 3;
        public int lockedTargetEffectID = 4;
        public Color lockedTargetEffectColor = Color.green;
        public Color lockedTargetEffectEnemyColor = Color.red;
        private bool isJoystickActive = false;
        private bool isCameraControlActive = false;

        public SpriteFade joystickDrawer = null;
        public Model model = Model.DEFAULT;
        public PhotographMode photographMode = PhotographMode.SELFIE;
        public Rect forceJoystickArea;
        public Rect Rotate;

        public int disableMove = 0;
        public int disableZoom = 0;
        public int disableLock = 0;

        private int _disable = 0;

        public float clickRoleRange = 1f;

        public float serverMoveDelay = 0.3f;
        public float searchTargetRange = 20f;

        private Photograph photograph_ = new Photograph();
        public Photograph photograph
        {
            get
            {
                return photograph_;
            }
        }

        private InputHelper[] helpers { get; set; }

        private StateMachine<InputController> controllerStateMachine { get; set; }
        private StateMachine<InputController> gyroControllerStateMachine { get; set; }
        public DefaultInputController defaultController { get; private set; }
        public JoystickInputController joystickController { get; private set; }
        public ZoomInputController zoomController { get; private set; }
        public CameraInputController cameraController { get; private set; }
        public CameraGyroInputController cameraGyroController { get; private set; }
        public CameraFieldOfViewInputController cameraFieldOfViewController { get; private set; }

        private InputController nextController { get; set; }

        public static InputManager Instance
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

        public bool disable
        {
            get
            {
                return 0 < _disable;
            }
            set
            {
                if (value)
                {
                    if (!disable)
                    {
                        if (null != controllerStateMachine.currentState && controllerStateMachine.currentState.running)
                        {
                            controllerStateMachine.ForceSwitch(null);
                        }
                    }
                    ++_disable;
                }
                else
                {
                    --_disable;
                }
            }
        }

        public bool IsCurrentExtraInputListener(InputListener listener)
        {
            return extraInputListener == listener || extraInputListenerLater == listener;
        }

        public void SetExtraInputListener(InputListener listener, bool immdiately = true)
        {
            if (immdiately)
                extraInputListener = listener;
            else
                extraInputListenerLater = listener;

        }

        public bool ClearExtraInputListener(InputListener listener)
        {
            if (!IsCurrentExtraInputListener(listener))
            {
                return false;
            }
            extraInputListener = null;
            extraInputListenerLater = null;
            return true;
        }

        public void Interrupt()
        {
            if (null != controllerStateMachine.currentState && controllerStateMachine.currentState.running)
            {
                controllerStateMachine.currentState.DelayExit();
            }
        }

        public void SwitchToJoystick(Vector3 centerPointOnScreen)
        {
            var holdEnable = false;
            if (Model.PHOTOGRAPH == model)
            {
                bool forceJoystick = false;
                if (0 >= disableMove && 0 < forceJoystickArea.width && 0 < forceJoystickArea.height)
                {
                    if (null != CameraController.singletonInstance)
                    {
                        Vector2 vp = CameraController.singletonInstance.activeCamera.ScreenToViewportPoint(centerPointOnScreen);
                        forceJoystick = forceJoystickArea.Contains(vp);
                    }
                }
                if (!forceJoystick)
                {
                    SwitchToCameraControl(centerPointOnScreen);
                    return;
                }
                if (PhotographMode.PHOTOGRAPHER == photograph.mode)
                {
                    holdEnable = true;
                }
            }

            if (Model.DEFAULT == model)
            {
                bool forceJoystick = false;
                forceJoystickArea.width = 0.45f;
                forceJoystickArea.height = 0.8f;
                if (0 < forceJoystickArea.width && 0 < forceJoystickArea.height)
                {
                    if (null != CameraController.singletonInstance)
                    {
                        Vector2 vp = CameraController.singletonInstance.activeCamera.ScreenToViewportPoint(centerPointOnScreen);
                        forceJoystick = forceJoystickArea.Contains(vp);
                    }
                }
                if (!forceJoystick)
                {
                    //  SwitchToCameraControl(centerPointOnScreen);
                    return;
                }
                // SwitchToCameraControl(centerPointOnScreen);



            }

            if (Model.CAMERA3D == model)
            {
                bool forceJoystick = false;
                if (0 >= disableMove && 0 < forceJoystickArea.width && 0 < forceJoystickArea.height)
                {
                    if (null != CameraController.singletonInstance)
                    {
                        Vector2 vp = CameraController.singletonInstance.activeCamera.ScreenToViewportPoint(centerPointOnScreen);
                        forceJoystick = forceJoystickArea.Contains(vp);
                    }
                }
                if (!forceJoystick)
                {
                    SwitchToCameraControl(centerPointOnScreen);
                    return;
                }
                if (PhotographMode.CAM3D == photograph.mode)
                {
                    holdEnable = true;
                }
            }

            if (0 < disableMove)
            {
                return;
            }
            joystickController.holdEnable = holdEnable;
            joystickController.centerPoint = centerPointOnScreen;
            nextController = joystickController;
        }

        public void SwitchToDefault()
        {
#if OBSOLETE
			defaultController.flagResourceID = ResourceIDHelper.IDEffectCommon(clickGroundEffectID);
#endif
            nextController = defaultController;
        }

        public void SwitchToZoom()
        {
            if (Model.PHOTOGRAPH == model)
            {
                SwitchToCameraFieldOfViewControl();
                return;
            }

            /*if (Model.CAMERA3D == model)
            {
               SwitchToCameraFieldOfViewControl();
                return;
            }*/
            if (0 < disableZoom)
            {
                return;
            }
            zoomController.cameraController = CameraController.Me;
            nextController = zoomController;
        }

        public void SwitchToCameraControl(Vector3 centerPointOnScreen)
        {
            cameraController.centerPoint = centerPointOnScreen;
            cameraController.cameraController = photograph.cameraController;
            nextController = cameraController;
        }

        public void SwitchToCameraFieldOfViewControl()
        {
            cameraFieldOfViewController.cameraController = photograph.cameraController;
            nextController = cameraFieldOfViewController;
        }

        public void StartCameraGyroController()
        {
            cameraGyroController.cameraController = photograph.cameraController;
            gyroControllerStateMachine.ForceSwitch(cameraGyroController);
        }

        public void EndCameraGyroController()
        {
            gyroControllerStateMachine.ForceSwitch(null);
        }

#if OBSOLETE
        #region simulator
		public void ClickTerrain(Vector3 position)
		{
			defaultController.ClickTerrain (NavMeshAdjustY.Adjust(position));
		}
        #endregion simulator
#endif

        private void OnControllerEndUpdating()
        {
            // restore
            //			switch (model)
            //			{
            //			case Model.DEFAULT:
            SwitchToDefault();
            //				break;
            //			case Model.PHOTOGRAPH:
            //				SwitchToCameraGyroController();
            //				break;
            //			}
        }

        public void ResetParams()
        {
            defaultController.clickRoleRange = clickRoleRange;
            var dpi = Screen.dpi;
            if (0 == dpi)
            {
                dpi = new Vector2(Screen.width, Screen.height).magnitude / DEFAULT_SCREEN_SIZE_INCH;
            }
            var touchSensePixels = touchSenseInch * dpi;
            var zoomOnePixels = zoomOneInch * dpi;
            var fovZoomOnePixels = fovZoomOneInch * dpi;
            var angleOnePixels = angleOneInch * dpi;

            foreach (var helper in helpers)
            {
                helper.touchSenseMin = touchSensePixels;
            }

            zoomController.zoomOneDistance = zoomOnePixels;

            cameraController.angleOneDistance = angleOnePixels;
            cameraController.rotationMin = cameraRotationMin;
            cameraController.rotationMax = cameraRotationMax;

            cameraGyroController.gyroDelay = gyroDelay;
            cameraGyroController.rotationMin = cameraRotationMin;
            cameraGyroController.rotationMax = cameraRotationMax;

            cameraFieldOfViewController.zoomOneDistance = fovZoomOnePixels;
            cameraFieldOfViewController.fieldOfViewMin = cameraFieldOfViewMin;
            cameraFieldOfViewController.fieldOfViewMax = cameraFieldOfViewMax;
        }

        protected override void Awake()
        {
            base.Awake();
            helpers = new InputHelper[] { new InputHelper(0), new InputHelper(1) };
            controllerStateMachine = new StateMachine<InputController>();
            gyroControllerStateMachine = new StateMachine<InputController>();
            defaultController = new DefaultInputController(helpers);
            joystickController = new JoystickInputController(helpers, joystickDrawer);
            zoomController = new ZoomInputController(helpers);
            cameraController = new CameraInputController(helpers);
            cameraGyroController = new CameraGyroInputController(helpers);
            cameraFieldOfViewController = new CameraFieldOfViewInputController(helpers);

            ResetParams();
        }

        void Start()
        {
        }

        void Update()
        {
            if (disable)
            {
                return;
            }
            if (null == Camera.main)
            {
                return;
            }

            if (zoomController != null)
            {
                zoomController.CheckMouseScroll();
            }

            defaultController.disableMove = 0 < disableMove;
            defaultController.disableZoom = 0 < disableZoom;
            defaultController.disableLock = 0 < disableLock;

            if (null != nextController)
            {
                controllerStateMachine.TrySwitch(nextController);
                nextController = null;
            }
            controllerStateMachine.Update();
            gyroControllerStateMachine.Update();
            if (null == controllerStateMachine.currentState || !controllerStateMachine.currentState.running)
            {
                OnControllerEndUpdating();
            }



            switch (model)
            {
                case Model.DEFAULT:
                    photograph.Exit();
                    if (cameraGyroController.running)
                    {
                        EndCameraGyroController();
                    }
                    Rect joystickArea = new Rect(0, 0, Screen.width * 0.5f, Screen.height);
                    Rect cameraArea = new Rect(Screen.width * 0.5f, 0, Screen.width * 0.5f, Screen.height);
                    // กำหนดพื้นที่จอยสติ๊กทางด้านซ้ายของหน้าจอ
                    if (CameraPointManager.Me != null)
                    {
                        CameraPoint[] cameraPoints = FindObjectsOfType<CameraPoint>();
                        foreach (var cameraPoint in cameraPoints)
                        {
                            cameraPoint.gameObject.SetActive(false); // ปิดการทำงานของ CameraPoint (clones)
                        }
                    }

                    // ตรวจสอบการคลิกเมาส์
                    // ให้หมุนมุมกล้องได้เฉพาะในโหมด CAMERA3D และใช้คลิกซ้ายลาก
                    if (model == Model.CAMERA3D)
                    {
                        if (model == Model.CAMERA3D && Input.GetMouseButton(0)) // ตรวจสอบการคลิกซ้าย
                        {
                            Vector2 inputPosition = Input.mousePosition;
                    
                            if (CameraController.Instance != null) // ตรวจสอบว่า CameraController ไม่เป็น null
                            {
                                float rotationSpeed = 10.0f; // กำหนดความเร็วการหมุน
                                float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed;
                                float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
                    
                                // หมุนกล้องในแนวแกน Y (ซ้าย-ขวา) และ X (บน-ล่าง)
                                Vector3 newRotation = CameraController.Instance.cameraRotationEuler + new Vector3(-verticalRotation, horizontalRotation, 0);
                                CameraController.Instance.RotateTo(newRotation, 0.1f); // หมุนกล้องแบบ Smooth
                            }
                        }
                    }


                    // ตรวจสอบการสัมผัสหน้าจอ
                    if (Input.touchCount > 0)
                    {
                        // Loop through each touch to handle multiple touches
                        for (int i = 0; i < Input.touchCount; i++)
                        {
                            Touch touch = Input.GetTouch(i);
                            Vector2 inputPosition = touch.position;

                            if (cameraArea.Contains(inputPosition))
                            {
                                isCameraControlActive = true;
                                isJoystickActive = false;

                                // ควบคุมการหมุนกล้อง
                                if (touch.phase == TouchPhase.Moved)
                                {
                                    if (CameraController.Instance != null)
                                    {
                                        float rotationSpeed = 5.0f;
                                        float horizontalRotation = touch.deltaPosition.x * rotationSpeed;
                                        targetRotation = CameraController.Instance.cameraRotationEuler + new Vector3(0, horizontalRotation, 0);
                                        Vector3 smoothRotation = Vector3.Lerp(CameraController.Instance.cameraRotationEuler, targetRotation, Time.deltaTime * 5.0f);
                                        CameraController.Instance.RotateTo(smoothRotation, 0.1f);
                                    }
                                }
                            }

                            // รีเซ็ตการควบคุมเมื่อการสัมผัสจบลง
                            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                            {
                                isJoystickActive = false;
                                isCameraControlActive = false;
                            }
                        }
                    }


                    break;
                case Model.PHOTOGRAPH:
                    photograph.Enter(photographMode);
                    cameraController.enable = photograph.ready;
                    cameraController.noClamp = (PhotographMode.PHOTOGRAPHER == photograph.mode);
                    cameraGyroController.enable = photograph.ready;
                    cameraGyroController.noClamp = (PhotographMode.PHOTOGRAPHER == photograph.mode);
                    if (PhotographMode.PHOTOGRAPHER == photograph.mode)
                    {
                        if (!cameraGyroController.running)
                        {
                            StartCameraGyroController();
                        }
                    }
                    else
                    {
                        if (cameraGyroController.running)
                        {
                            EndCameraGyroController();
                        }
                    }
                    break;
                case Model.CAMERA3D:
                    photographMode = PhotographMode.CAM3D;
                    photograph.Enter3D(photographMode);
                    cameraController.enable = photograph.ready;
                    disableZoom = 0;
                    /* cameraController.noClamp = (PhotographMode.PHOTOGRAPHER == photograph.mode);
                     cameraGyroController.enable = photograph.ready;
                     cameraGyroController.noClamp = (PhotographMode.PHOTOGRAPHER == photograph.mode);*///
                    break;

            }
        }

        void LateUpdate()
        {
            if (extraInputListenerLater != null)
            {
                extraInputListener = extraInputListenerLater;
                extraInputListenerLater = null;
            }
        }



#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
        void OnGUI()
        {
            if (null != cameraGyroController && cameraGyroController.running)
            {
                cameraGyroController.OnGUI();
            }
            //			GUILayout.Label("Orientation: " + Screen.orientation);
            //			GUILayout.Label("Gyro.attitude: " + Input.gyro.attitude);
            //			GUILayout.Label("Gyro.attitude(Euler): " + Input.gyro.attitude.eulerAngles);
            //			GUILayout.Label("Gyro.base rotation(Euler): " + cameraGyroController.baseGyroEuler);
            //			GUILayout.Label("Gyro.world rotation: " + GyroUtils.GetWorldRotation());
            //			GUILayout.Label("Gyro.world rotation(Euler): " + GyroUtils.GetWorldRotation().eulerAngles);
            //			GUILayout.Label("Gyro.gravity: " + Input.gyro.gravity);
        }
#endif

    }
} // namespace RO
