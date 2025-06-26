using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Ghost.Utils;

namespace RO
{
	sealed public class RoamInputController : InputController 
	{
		public float speed = 0.1f;
		public Transform target;

		private Vector3 centerPoint{get;set;}
		private Vector3 direction{get;set;}
		private bool moving{get;set;}

		public RoamInputController(InputHelper[] ihs)
			: base(ihs)
		{

		}

		protected override void DoUpdate ()
		{
			base.DoUpdate ();

			if (moving && null != target)
			{
				var moveDistance = speed * Time.deltaTime;
				var movePosition = target.position + direction*moveDistance;
				target.position = movePosition;
			}
		}

		protected override void OnTouchBegin ()
		{
			if (0 != pointerID)
			{
				return;
			}

			centerPoint = touchPoint;
			moving = false;
		}

		protected override void OnTouchMoved()
		{
			if (0 != pointerID)
			{
				return;
			}
			var dir = touchPoint-centerPoint;
			dir.z = dir.y;
			dir.y = 0;
			dir = Quaternion.Euler(new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0)) * dir;
			
			direction = dir;

			moving = true;
		}

		protected override void OnTouchEnd ()
		{
			moving = false;
		}

		protected override bool allowExtra()
		{
			return 0 < pointerID;
		}
	
	}
} // namespace RO
