using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Ghost.Config;

namespace RO
{
	public class InputHelper 
	{
		public static bool PointOverUIObject(Canvas canvas, Vector2 screenPosition) 
		{
			PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			eventDataCurrentPosition.position = screenPosition;
			
			GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
			List<RaycastResult> results = new List<RaycastResult>();
			uiRaycaster.Raycast(eventDataCurrentPosition, results);
			return results.Count > 0;
		}
		
		public static bool PointOverUIObject() 
		{
			PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
			return results.Count > 0;
		}

//		public static bool RayOverUICollider(Ray ray, out RaycastHit hit, params string[] layers) 
//		{
//			var layerList = new List<string>(layers);
//			layerList.Add(Config.Layer.UI);
//			if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerList.ToArray()))
//			{
//				if (LayerMask.NameToLayer(Config.Layer.UI) == hit.collider.gameObject.layer)
//				{
//					return true;
//				}
//			}
//			return false;
//		}

		private static bool GetInputSinglePoint(out Vector3 point, int pID = 0)
		{
			if (Input.touchSupported)
			{
				if (pID < Input.touchCount)
				{
					var touch = Input.GetTouch(pID);
					if (TouchPhase.Began == touch.phase
					    || TouchPhase.Moved == touch.phase
					    || TouchPhase.Stationary == touch.phase)
					{
						point = touch.position;
						return true;
					}
				}
			}
			else if (0 == pID)
			{
				if(Input.GetMouseButton(InputConfig.MOUSE_BUTTON_INDEX_LEFT)
				   || Input.GetMouseButton(InputConfig.MOUSE_BUTTON_INDEX_LEFT))
				{
					point = Input.mousePosition;
					return true;
				}
			}
			point = Vector3.zero;
			return false;
		}

		public delegate void TouchListener(int pID, Vector3 touchPoint);
		public event TouchListener touchListener;

		public float touchSenseMin = 50; // pixels
		public int pointerID{get; private set;}
		
		private Vector3 touchBeginPoint_ = Vector3.zero;
		private Vector3 touchPoint_ = Vector3.zero;
		public Vector3 touchBeginPoint
		{
			get
			{
				return touchBeginPoint_;
			}
			private set
			{
				touchBeginPoint_ = value;
			}
		}
		public Vector3 touchPoint
		{
			get
			{
				return touchPoint_;
			}
			private set
			{
				touchPoint_ = value;
			}
		}
		public bool touching{get; private set;}

		public InputHelper(int pID = 0)
		{
			pointerID = pID;
		}

		private void NotifyTouchListener()
		{
			if (null != touchListener)
			{
				touchListener(pointerID, touchPoint_);
			}
		}

		public void Reset()
		{
			if (touching)
			{
				Vector3 inputPoint;
				if (GetInputSinglePoint(out inputPoint, pointerID))
				{
					touchBeginPoint = inputPoint;
					touchPoint = inputPoint;
					NotifyTouchListener();
				}
				else
				{
					touching = false;
				}
			}
		}

		public void Update()
		{
			Vector3 inputPoint;
			if (GetInputSinglePoint(out inputPoint, pointerID))
			{
				if (!touching)
				{
					touching = true;
					touchBeginPoint = inputPoint;
					touchPoint = inputPoint;
				}
				else if (touchSenseMin < Vector3.Distance(touchPoint, inputPoint))
				{
					touchPoint = inputPoint;
				}
				NotifyTouchListener();
			}
			else
			{
				touching = false;
			}
		}
	
	}
} // namespace RO
