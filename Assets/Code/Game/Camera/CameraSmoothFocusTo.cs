using UnityEngine;
using System.Collections.Generic;
using Ghost.Attribute;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CameraSmoothFocusTo : CameraSmooth
	{
		public static CameraSmoothFocusTo Create(
			Transform focus, 
			Vector3 targetFocusViewPort, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			var obj = CreateT<CameraSmoothFocusTo>(duration, listener);
			obj.focus = focus;
			obj.targetFocusViewPort = targetFocusViewPort;
			return obj;
		}

		public static CameraSmoothFocusTo Create(
			Transform focus, 
			Vector3 targetFocusOffset,
			Vector3 targetFocusViewPort, 
			float duration, 
			System.Action<CameraController> listener = null)
		{
			var obj = CreateT<CameraSmoothFocusTo>(duration, listener);
			obj.focus = focus;
			obj.targetFocusOffset = targetFocusOffset;
			obj.targetFocusViewPort = targetFocusViewPort;
			return obj;
		}
		
		public Transform focus = null;
		public Vector3 targetFocusOffset = Vector3.zero;
		public Vector3 targetFocusViewPort;

		private Vector3 originalFocusOffset;
		private Vector3 originalFocusViewPort;
		
		public bool Launch(CameraController controller)
		{
			if (null == controller)
			{
				return false;
			}

			if (null != focus)
			{
				controller.ResetCurrentInfoByFocus(focus);
			}

			originalFocusOffset = controller.focusOffset;
			originalFocusViewPort = controller.focusViewPort;
			if (Vector3.Equals(originalFocusViewPort, targetFocusViewPort))
			{
				return false;
			}
			
			Start(controller);
			
			return true;
		}
		
		protected override void DoUpdate(CameraController controller, float progress)
		{
			controller.ResetFocusOffset(Vector3.Lerp(originalFocusOffset, targetFocusOffset, progress));
			controller.ResetFocusViewPort(Vector3.Lerp(originalFocusViewPort, targetFocusViewPort, progress));
		}
	}
} // namespace RO
