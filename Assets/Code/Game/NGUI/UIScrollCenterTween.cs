using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[RequireComponent(typeof(UICenterOnChild))]
	[SLua.CustomLuaClassAttribute]
	public class UIScrollCenterTween : MonoBehaviour
	{
		public float deltaAddScale = 0.4f;
		public float deltaDistance;
		public int startIndex = 0;
		UIPanel clipPanel;
		UICenterOnChild centerOnChild;
		Transform panelParent;
		Vector3 midPos = Vector3.zero;
		bool isSetStart = false;

		void Awake ()
		{
			clipPanel = NGUITools.FindInParents<UIPanel> (gameObject);
			centerOnChild = gameObject.GetComponent<UICenterOnChild> ();
			panelParent = clipPanel.transform.parent;
			midPos = panelParent.InverseTransformPoint (clipPanel.transform.position);
			clipPanel.onClipMove = tweenChild;
		}

		void tweenChild (UIPanel panel)
		{
			for (int i=0; i<transform.childCount; i++) {
				Transform child = transform.GetChild (i);
				Vector3 childlocalPos = panelParent.InverseTransformPoint (child.position);
				float deltaM = Vector3.Distance (childlocalPos, midPos);
				float lerp = 1 - Mathf.InverseLerp (0, deltaDistance, deltaM);
				float scale = deltaAddScale * Mathf.Abs (lerp) + 1;
				child.localScale = new Vector3 (scale, scale, scale);
			}
		}

		void OnEnable ()
		{
			isSetStart = false;
		}

		void Update ()
		{
			if (!isSetStart) {
				isSetStart = true;
				setStartIndex (startIndex);
			}
		}
		
		public void SetStartIndex (int index)
		{
			startIndex = index;
			isSetStart = false;
		}
		
		void setStartIndex (int index)
		{
			// start index
			if (transform.childCount > 0) {
//				UIScrollView scrollView = clipPanel.GetComponent<UIScrollView> ();
//				if (scrollView.movement == UIScrollView.Movement.Horizontal) {
//					scrollView.MoveRelative (new Vector3 (-startIndex * deltaDistance, 0, 0));
//				} else if (scrollView.movement == UIScrollView.Movement.Vertical) {
//					scrollView.MoveRelative (new Vector3 (0, startIndex * deltaDistance, 0));
//				}
				Transform trans = transform.GetChild (index);
				centerOnChild.CenterOn (trans);
			}
			tweenChild (clipPanel);
		}
	}
} // namespace RO
