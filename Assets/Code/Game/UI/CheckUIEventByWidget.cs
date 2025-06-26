using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	[RequireComponent(typeof(UIWidget))]
	public class CheckUIEventByWidget : MonoBehaviour
	{
		public UICamera.VectorDelegate onDrag;
		public UICamera.VoidDelegate onClick;
		Bounds size;
		Transform point;
		Camera uiCamera;
		Vector3 delta;
		bool clickIn;

		void Start ()
		{
			uiCamera = NGUITools.FindCameraForLayer (gameObject.layer);
			if (point == null) {
				GameObject g = new GameObject ();
				point = g.transform;
			}
			point.transform.SetParent (transform, false);
			size = NGUIMath.CalculateRelativeWidgetBounds (transform);
		}

		void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {
				point.position = uiCamera.ScreenToWorldPoint (Input.mousePosition);
				if (size.Contains (point.localPosition)) {
					delta = point.localPosition;
					clickIn = true;
				}
			} else if (clickIn && Input.GetMouseButtonUp (0)) {
				clickIn = false;
				point.position = uiCamera.ScreenToWorldPoint (Input.mousePosition);
				delta = point.transform.localPosition - delta;
				if (onDrag != null) {
					onDrag (gameObject, (Vector2)delta);
				}
			}
		}
	}
} // namespace RO
