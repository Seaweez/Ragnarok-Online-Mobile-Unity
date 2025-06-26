using UnityEngine;
using System.Collections.Generic;
using Ghost.Attribute;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CameraSmoothZoomTo : CameraSmooth
	{
		public static CameraSmoothZoomTo Create(
			float targetZoom, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			var obj = CreateT<CameraSmoothZoomTo>(duration, listener);
			obj.targetZoom = targetZoom;
			return obj;
		}
		
		public float targetZoom;
		
		private float originalZoom;
		
		public bool Launch(CameraController controller)
		{
			if (null == controller)
			{
				return false;
			}

			if (controller.zoom == targetZoom)
			{
				return false;
			}
			
			originalZoom = controller.zoom;
			
			Start(controller);
			
			return true;
		}
		
		protected override void DoUpdate(CameraController controller, float progress)
		{
			controller.forceZoom = Mathf.Lerp(originalZoom, targetZoom, progress);
		}
	}
} // namespace RO
