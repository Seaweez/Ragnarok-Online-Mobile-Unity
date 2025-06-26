using UnityEngine;
using System.Collections.Generic;
using Ghost.Attribute;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CameraSmoothPositionTo : CameraSmooth
	{
		public static CameraSmoothPositionTo Create(
			Vector3 targetPosition, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			var obj = CreateT<CameraSmoothPositionTo>(duration, listener);
			obj.targetPosition = targetPosition;
			return obj;
		}
		
		public Vector3 targetPosition;
		
		private Vector3 originalPosition;
		
		public bool Launch(CameraController controller)
		{
			if (null == controller)
			{
				return false;
			}

			if (Vector3.Equals(controller.cameraPosition, targetPosition))
			{
				return false;
			}
			
			originalPosition = controller.cameraPosition;
			
			Start(controller);
			
			return true;
		}
		
		protected override void DoUpdate(CameraController controller, float progress)
		{
			controller.ResetLockPosition(Vector3.Lerp(originalPosition, targetPosition, progress));
		}
	}
} // namespace RO
