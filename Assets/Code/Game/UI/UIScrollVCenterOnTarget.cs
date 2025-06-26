using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class UIScrollVCenterOnTarget : MonoBehaviour
	{
		public Transform target;
		public Transform centerTarget;
		UIPanel mPanel;
		Transform mTrans;
		Bounds mBound, checkBound;
		Vector3 catchPos;
		
		void Start ()
		{
			mPanel = gameObject.GetComponent<UIPanel> ();
			mTrans = mPanel.transform;
		}
		
		public void RestrictWithinBounds ()
		{
			if (target != null) {
				Vector3 cp = mTrans.InverseTransformPoint (target.position);
				Vector3[] corners = mPanel.worldCorners;
				Vector3 panelCenter = (corners [2] + corners [0]) * 0.5f;
				Vector3 cc = mTrans.InverseTransformPoint (panelCenter);

				Vector3 localOffset = cp - cc;
				centerTarget.localPosition = centerTarget.localPosition - localOffset;
			}
		}
		
		void FixedUpdate ()
		{
			if (catchPos != target.position) {
				RestrictWithinBounds ();
				catchPos = target.position;
			}
		}
	
	}
} // namespace RO
