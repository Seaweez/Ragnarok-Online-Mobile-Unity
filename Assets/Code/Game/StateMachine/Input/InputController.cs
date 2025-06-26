using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;
using Ghost.Utils;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public abstract class InputController : State
	{
		public class InputState
		{
			public delegate void TouchBeginListener(InputState state);
			public delegate void TouchMovedListener(InputState state);
			public delegate void TouchHoldListener(InputState state);
			public delegate void TouchEndListener(InputState state);

			public event TouchBeginListener touchBeginListener = null;
			public event TouchMovedListener touchMovedListener = null;
			public event TouchHoldListener touchHoldListener = null;
			public event TouchEndListener touchEndListener = null;

			private bool listening = false;

			public int pointerID
			{
				get
				{
					return helper.pointerID;
				}
			}

			public InputHelper helper{get;private set;}
			public bool touching{get;private set;}
			public Vector3 prevTouchPoint{get;private set;}
			public Vector3 touchBeginPoint
			{
				get
				{
					return helper.touchBeginPoint;
				}
			}
			public Vector3 touchPoint
			{
				get
				{
					return helper.touchPoint;
				}
			}

			public bool beginOnUI{get;set;}
			
			public InputState(InputHelper ih)
			{
				helper = ih;
			}
			~InputState()
			{
				if (listening && null != helper)
				{
					helper.touchListener -= OnTouch;
				}
			}

			private void StartListen()
			{
				if (listening)
				{
					return;
				}
				helper.touchListener += OnTouch;
				listening = true;
			}
			private void EndListen()
			{
				if (!listening)
				{
					return;
				}
				helper.touchListener -= OnTouch;
				listening = false;
			}
			
			private void OnTouch(int pID, Vector3 screenPoint)
			{
				if (!touching)
				{
					TouchBegin();
				}
				else if (GeometryUtils.PositionEqual(prevTouchPoint, screenPoint))
				{
					TouchHold();
				}
				else
				{
					TouchMoved();
				}
				prevTouchPoint = screenPoint;
			}
			
			private void TouchBegin()
			{
				touching = true;
				OnTouchBegin();
			}
			
			private void TouchMoved()
			{
				OnTouchMoved();
			}

			private void TouchHold()
			{
				OnTouchHold();
			}
			
			private void TouchEnd()
			{
				touching = false;
				OnTouchEnd();
			}
			
			private void OnTouchBegin()
			{
				if (null != touchBeginListener)
				{
					touchBeginListener(this);
				}
			}
			
			private void OnTouchMoved()
			{
				if (null != touchMovedListener)
				{
					touchMovedListener(this);
				}
			}

			private void OnTouchHold()
			{
				if (null != touchHoldListener)
				{
					touchHoldListener(this);
				}
			}
			
			private void OnTouchEnd()
			{
				if (null != touchEndListener)
				{
					touchEndListener(this);
				}
			}

			public void Enter()
			{
				StartListen();
				touching = false;
				helper.Reset();
			}
			
			public void Exit()
			{
				EndListen();
				touching = false;
			}
			
			public void Update()
			{
				helper.Update();
				if (touching && !helper.touching)
				{
					TouchEnd();
				}
			}
		}
		
		private InputState currentState{get;set;}
		private List<InputState> states{get; set;}

		#region UGUI
		private Canvas mainCanvas_ = null;
		public Canvas mainCavas
		{
			get
			{
				if (null == mainCanvas_)
				{
					var obj = GameObject.FindGameObjectWithTag(Config.Tag.MAIN_CANVAS);
					if (null != obj)
					{
						mainCanvas_ = obj.GetComponent<Canvas>();
					}
				}
				return mainCanvas_;
			}
		}
		#endregion UGUI

		public bool isOverUI
		{
			get
			{
				#region UGUI
				var cavas = mainCavas;
				if (null != cavas && InputHelper.PointOverUIObject(cavas, touchPoint))
				{
					return true;
				}
				#endregion UGUI
				
				#region NGUI
				if (UICamera.isOverUI)
				{
					return true;
				}
				#endregion NGUI

				return false;
			}
		}

		public bool beginOnUI
		{
			get
			{
				return null != currentState && currentState.beginOnUI;
			}
		}

		public int touchingCount
		{
			get
			{
				int count = 0;
				for (int i = 0; i < states.Count; ++i)
				{
					var state = states[i];
					if (state.touching)
					{
						++count;
					}
				}
				return count;
			}
		}

		public int pointerID
		{
			get
			{
				return null != currentState ? currentState.pointerID : -1;
			}
		}
		public Vector3 touchBeginPoint
		{
			get
			{
				return null != currentState ? currentState.touchBeginPoint : Vector3.zero;
			}
		}
		public Vector3 touchPoint
		{
			get
			{
				return null != currentState ? currentState.touchPoint : Vector3.zero;
			}
		}

		protected InputState GetState(int pointerID)
		{
			return (null != states && states.Count > pointerID) ? states[pointerID] : null;
		}

		public InputController(InputHelper[] ihs)
		{
			var stateList = new List<InputState>();
			for (int i = 0; i < ihs.Length; ++i)
			{
				var ih = ihs[i];
				var state = new InputState(ih);
				state.touchBeginListener += TouchBegin;
				state.touchMovedListener += TouchMoved;
				state.touchHoldListener += TouchHold;
				state.touchEndListener += TouchEnd;
				stateList.Add(state);
			}
			states = stateList;
		}

		private bool NotifyExtra(TouchEventType eventType)
		{
			if (null != InputManager.Me && null != InputManager.Me.extraInputListener && allowExtra())
			{
				var info = InputInfo.Global;
				info.touchEventType = eventType;
				info.pointerID = pointerID;
				info.touchPoint = touchPoint;
				info.beginOnUI = beginOnUI;
				info.overUI = isOverUI;
				InputManager.Me.extraInputListener(info);

				return true;
			}
			return false;
		}

		private void TouchBegin(InputState state)
		{
			if (exitPending)
			{
				return;
			}
			if (!running)
			{
				return;
			}
			currentState = state;

			currentState.beginOnUI = isOverUI;
			if (!NotifyExtra(TouchEventType.BEGIN))
			{
				OnTouchBegin();
			}
			currentState = null;
		}
		
		private void TouchMoved(InputState state)
		{
			if (exitPending)
			{
				return;
			}
			if (!running)
			{
				return;
			}
			currentState = state;
			if (!NotifyExtra(TouchEventType.MOVE))
			{
				OnTouchMoved();
			}
			currentState = null;
		}

		private void TouchHold(InputState state)
		{
			if (exitPending)
			{
				return;
			}
			if (!running)
			{
				return;
			}
			currentState = state;
			if (!NotifyExtra(TouchEventType.HOLD))
			{
				OnTouchHold();
			}
			currentState = null;
		}
		
		private void TouchEnd(InputState state)
		{
			if (exitPending)
			{
				return;
			}
			if (!running)
			{
				return;
			}
			currentState = state;
			if (!NotifyExtra(TouchEventType.END))
			{
				OnTouchEnd();
			}
			currentState = null;
		}

		protected virtual void OnTouchBegin()
		{
		}

		protected virtual void OnTouchMoved()
		{
		}

		protected virtual void OnTouchHold()
		{
		}

		protected virtual void OnTouchEnd()
		{
		}

		protected virtual bool allowExtra()
		{
			return true;
		}

		// override
		protected override bool DoEnter()
		{
			if (!base.DoEnter())
			{
				return false;
			}
			for (int i = 0; i < states.Count; ++i)
			{
				var state = states[i];
				state.Enter ();
			}
			return true;
		}

		protected override void DoExit()
		{
			base.DoExit();
			for (int i = 0; i < states.Count; ++i)
			{
				var state = states[i];
				state.Exit ();
			}
		}

		protected override void DoUpdate()
		{
			base.DoUpdate();
			for (int i = 0; i < states.Count; ++i)
			{
				var state = states[i];
				state.Update ();
			}
#if UNITY_EDITOR
            /*if (null != CameraController.Me)
			{
				var MOUSE_SCROLL_WHEEL = "Mouse ScrollWheel";
				if (0 > Input.GetAxis(MOUSE_SCROLL_WHEEL))
				{
					var fieldOfView = CameraController.Me.cameraFieldOfView-1;
					if (null != InputManager.Me)
					{
						fieldOfView = Mathf.Clamp(fieldOfView, InputManager.Me.cameraFieldOfViewMin, InputManager.Me.cameraFieldOfViewMax);
					}
					CameraController.Me.ResetFieldOfView(fieldOfView);
				}
				else if (0 < Input.GetAxis(MOUSE_SCROLL_WHEEL))
				{
					var fieldOfView = CameraController.Me.cameraFieldOfView+1;
					if (null != InputManager.Me)
					{
						fieldOfView = Mathf.Clamp(fieldOfView, InputManager.Me.cameraFieldOfViewMin, InputManager.Me.cameraFieldOfViewMax);
					}
					CameraController.Me.ResetFieldOfView(fieldOfView);
				}
			}*/
            if (null != CameraController.Me)
            {
                var MOUSE_SCROLL_WHEEL = "Mouse ScrollWheel";
                if (0 > Input.GetAxis(MOUSE_SCROLL_WHEEL))
                {
                    var fieldOfView = CameraController.Me.zoom-0.1f;
                    if (null != InputManager.Me)
                    {
                        fieldOfView = Mathf.Clamp(fieldOfView, CameraController.Me.zoomMinEx, CameraController.Me.zoomMaxEx);
                    }
                    //CameraController.Me.ResetFieldOfView(fieldOfView);
                }
                else if (0 < Input.GetAxis(MOUSE_SCROLL_WHEEL))
                {
                    var fieldOfView = CameraController.Me.zoom +0.1f;
                    if (null != InputManager.Me)
                    {
                        fieldOfView = Mathf.Clamp(fieldOfView, CameraController.Me.zoomMinEx, CameraController.Me.zoomMaxEx);
                    }
                   // CameraController.Me.ResetFieldOfView(fieldOfView);
                }
            }

#endif // UNITY_EDITOR
        }

	}
} // namespace RO
