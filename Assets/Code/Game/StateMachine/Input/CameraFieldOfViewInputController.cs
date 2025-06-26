using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	sealed public class CameraFieldOfViewInputController : InputController 
	{
		public float zoomOneDistance = 300f; // pixel
		public float fieldOfViewMin = 18;
		public float fieldOfViewMax = 63;
		public CameraController cameraController = null;

		private Vector3 p1;
		private Vector3 p2;
		private float prevDistance = -2f;

		public CameraFieldOfViewInputController(InputHelper[] ihs)
			: base(ihs)
		{

		}

		protected override bool DoAllowInterruptedBy(State other)
		{
			if (null != (other as DefaultInputController)
			    || null != (other as CameraInputController)
			    || null != (other as CameraGyroInputController))
			{
				return true;
			}
			return base.DoAllowInterruptedBy(other);
		}

		protected override bool DoEnter()
		{
			if (!base.DoEnter())
			{
				return false;
			}
			if (null == cameraController)
			{
				return false;
			}
			return true;
		}

		protected override void OnTouchBegin()
		{
			if (0 == pointerID)
			{
				p1 = touchPoint;
				if (0 > prevDistance)
				{
					prevDistance += 1;
				}
			}
			else if (1 == pointerID)
			{
				p2 = touchPoint;
				if (0 > prevDistance)
				{
					prevDistance += 1;
				}
			}
			if (0 <= prevDistance)
			{
				prevDistance = Vector2.Distance(p1, p2);
			}
		}

		protected override void OnTouchMoved()
		{
			if (1 >= touchingCount)
			{
				DelayExit();
				InputManager.Me.SwitchToCameraControl(touchPoint);
				return;
			}

			if (null == cameraController)
			{
				Exit();
				return;
			}

			bool init = false;
			if (0 > prevDistance)
			{
				init = true;
			}
			if (0 == pointerID)
			{
				p1 = touchPoint;
				if (0 > prevDistance)
				{
					prevDistance += 1;
				}
			}
			else if (1 == pointerID)
			{
				p2 = touchPoint;
				if (0 > prevDistance)
				{
					prevDistance += 1;
				}
			}
			else
			{
				return;
			}

			if (0 > prevDistance)
			{
				return;
			}
			else if (init)
			{
				prevDistance = Vector2.Distance(p1, p2);
				return;
			}

			var newDistance = Vector2.Distance(p1, p2);
			var fieldOfView = cameraController.cameraFieldOfView+(newDistance-prevDistance)/zoomOneDistance;
			fieldOfView = Mathf.Clamp(fieldOfView, fieldOfViewMin, fieldOfViewMax);
			cameraController.ResetFieldOfView(fieldOfView);
			prevDistance = newDistance;
		}

		protected override void OnTouchEnd()
		{
			if (null == cameraController
			    || 0 >= touchingCount)
			{
				DelayExit();
				return;
			}
		}

		protected override bool allowExtra()
		{
			return 1 < pointerID;
		}
	
	}
} // namespace RO
