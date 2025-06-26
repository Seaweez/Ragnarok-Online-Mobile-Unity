using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Ghost.Utils;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	sealed public class CameraInputController : InputController 
	{
		public float angleOneDistance = 10f; // pixel
		public Vector2 rotationMin;
		public Vector2 rotationMax;

		public Vector3 centerPoint{get;set;}

		private Vector3 originalCameraRotation;
		private Vector3 cameraRotationMin;
		private Vector3 cameraRotationMax;

		public bool enable = false;
		public bool noClamp = false;
		public CameraController cameraController = null;

		private bool inited = false;

		public CameraInputController(InputHelper[] ihs)
			: base(ihs)
		{
		
		}

		protected override bool DoAllowInterruptedBy(State other)
		{
			if (null != (other as DefaultInputController)
			    || null != (other as CameraFieldOfViewInputController)
			    || null != (other as CameraGyroInputController))
			{
				return true;
			}
			return base.DoAllowInterruptedBy(other);
		}

		protected override bool DoEnter ()
		{
			if (!base.DoEnter ())
			{
				return false;
			}
			if (null == cameraController)
			{
				return false;
			}

			inited = false;
			return true;
		}

		protected override void DoUpdate()
		{
			base.DoUpdate();
			if (0 >= touchingCount)
			{
				Exit();
			}
			else
			{
				if (!inited && enable)
				{
					inited = true;
					originalCameraRotation = cameraController.cameraRotationEuler;
					cameraRotationMin.x = rotationMin.x;
					cameraRotationMax.x = rotationMax.x;
					
					cameraRotationMin.y = originalCameraRotation.y+rotationMin.y;
					cameraRotationMax.y = originalCameraRotation.y+rotationMax.y;
					
					cameraRotationMin.z = originalCameraRotation.z;
					cameraRotationMax.z = originalCameraRotation.z;
				}
			}
		}

		protected override void OnTouchBegin ()
		{
			if (0 == pointerID)
			{
				return;
			}

			if (isOverUI)
			{
				return;
			}
		}

		protected override void OnTouchMoved()
		{
			if (0 != pointerID)
			{
				return;
			}

			if (!inited)
			{
				return;
			}

			if (null == cameraController)
			{
				return;
			}

			var cameraAngleDelta = new Vector3(
				-(touchPoint.y-centerPoint.y) / angleOneDistance, 
				(touchPoint.x-centerPoint.x) / angleOneDistance,
				0);
			var cameraRotation = originalCameraRotation + cameraAngleDelta;
			if (!noClamp)
			{
				var rx = GeometryUtils.UniformAngle180(cameraRotation.x);
				rx = Mathf.Clamp(rx, cameraRotationMin.x, cameraRotationMax.x);
				cameraRotation.x = GeometryUtils.UniformAngle(rx);
//				cameraRotation.y = Mathf.Clamp(cameraRotation.y, cameraRotationMin.y, cameraRotationMax.y);
			}

			cameraController.ResetRotation(cameraRotation);
		}

		protected override void OnTouchEnd()
		{
			if (0 < touchingCount)
			{
				return;
			}
			DelayExit();
		}

		protected override bool allowExtra()
		{
			return 0 < pointerID;
		}
	
	}
} // namespace RO
