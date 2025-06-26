using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	sealed public class ZoomInputController : InputController 
	{
		public float zoomOneDistance = 300f; // pixel

		private Vector3 p1;
		private Vector3 p2;
		private float prevDistance = -2f;
		public CameraController cameraController = null;

		public ZoomInputController(InputHelper[] ihs)
			: base(ihs)
		{

		}

		protected override bool DoAllowInterruptedBy(State other)
		{
			if (null != (other as DefaultInputController)
			    || null != (other as JoystickInputController))
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
			cameraController.InterruptSmoothZoom();
			p1 = Vector3.zero;
			p2 = Vector3.zero;
			prevDistance = -2f;
			return true;
		}

		protected override void DoExit ()
		{
			base.DoExit ();
			if (null != cameraController)
			{
				cameraController.RestoreZoom(0.5f);
			}
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
				InputManager.Me.SwitchToJoystick(touchPoint);
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
			cameraController.zoom += (newDistance-prevDistance) / zoomOneDistance;
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

        public void CheckMouseScroll()
        {
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
            if (cameraController != null && scrollAmount != 0)
            {
                // คำนวณการเปลี่ยนแปลงการซูม และปรับตามค่า zoomOneDistance สำหรับความเร็วการซูม
                // คุณอาจต้องการปรับค่านี้เพื่อให้เหมาะสมกับการซูมในเกมของคุณ
                cameraController.zoom += scrollAmount * zoomOneDistance / 300f; // ตัวอย่างการปรับใช้งาน
            }
        }

    }
} // namespace RO
