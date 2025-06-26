using UnityEngine;
using System.Collections.Generic;
using Ghost.Attribute;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CameraSmoothFieldOfViewTo : CameraSmooth
	{
		public static CameraSmoothFieldOfViewTo Create(
			float targetFieldOfView, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			var obj = CreateT<CameraSmoothFieldOfViewTo>(duration, listener);
			obj.targetFieldOfView = targetFieldOfView;
			return obj;
		}
		
		public float targetFieldOfView;
		
		private float originalFieldOfView;
		
		public bool Launch(CameraController controller)
		{
			if (null == controller)
			{
				return false;
			}

			if (controller.cameraFieldOfView == targetFieldOfView)
			{
				return false;
			}
			
			originalFieldOfView = controller.cameraFieldOfView;
			
			Start(controller);
			
			return true;
		}
		
		protected override void DoUpdate(CameraController controller, float progress)
		{
			controller.ResetFieldOfView(Mathf.Lerp(originalFieldOfView, targetFieldOfView, progress));
		}
	}
} // namespace RO
