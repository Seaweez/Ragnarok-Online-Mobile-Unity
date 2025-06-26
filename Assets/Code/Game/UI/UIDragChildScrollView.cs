using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class UIDragChildScrollView : MonoBehaviour
	{
		public UIScrollView childScrollView;
		UIScrollView parentScrollV;
		UIScrollView nowScrollV;

		void Start ()
		{
			if (childScrollView) {
				Transform parentT = childScrollView.transform.parent;
				if (parentT) {
					parentScrollV = NGUITools.FindInParents<UIScrollView> (parentT);
				}
			}
		}
		
		void OnPress (bool press)
		{
			if (!press && nowScrollV != null) {
				nowScrollV.Press (false);
				nowScrollV = null;
			}
		}

		void OnDrag (Vector2 delta)
		{
			if (nowScrollV == null) {
				if (null == parentScrollV) {
					nowScrollV = childScrollView;
				} else {
					if (Mathf.Abs (delta.x) < Mathf.Abs (delta.y)) {
						if (childScrollView.movement == UIScrollView.Movement.Vertical) {
							nowScrollV = childScrollView;
						} else if (parentScrollV.movement == UIScrollView.Movement.Vertical) {
							nowScrollV = parentScrollV;
						}
					} else {
						if (childScrollView.movement == UIScrollView.Movement.Horizontal) {
							nowScrollV = childScrollView;
						} else if (parentScrollV.movement == UIScrollView.Movement.Horizontal) {
							nowScrollV = parentScrollV;
						}
					}
					if (nowScrollV != null) {
						nowScrollV.Press (true);
					}
				}
			}
			if (nowScrollV != null) {
				nowScrollV.Drag ();
			}
		}

	}
} // namespace RO
