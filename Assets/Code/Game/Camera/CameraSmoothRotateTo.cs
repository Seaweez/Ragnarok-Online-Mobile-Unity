using UnityEngine;
using System.Collections.Generic;
using Ghost.Attribute;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CameraSmoothRotateTo : CameraSmooth
	{
		public static CameraSmoothRotateTo Create(
			Quaternion targetRotation, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			var obj = CreateT<CameraSmoothRotateTo>(duration, listener);
			obj.targetRotation = targetRotation;
			return obj;
		}
		
		public Quaternion targetRotation;
		
		private Quaternion originalRotation;
		
		public bool Launch(CameraController controller)
		{
			if (null == controller)
			{
				return false;
			}

			if (controller.CameraRotationEquals(targetRotation))
			{
				return false;
			}
			
			originalRotation = controller.cameraRotation;
			
			Start(controller);
			
			return true;
		}
		
		protected override void DoUpdate(CameraController controller, float progress)
		{
			controller.ResetRotation(Quaternion.Lerp(originalRotation, targetRotation, progress));
		}
	}
} // namespace RO
