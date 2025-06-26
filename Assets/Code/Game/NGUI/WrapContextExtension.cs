using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIWrapContent))]
public class WrapContextExtension : MonoBehaviour
{
	UIWrapContent mWrap;
	UIProgressBar msbar;
	float pct;

	// Use this for initialization
	void Awake ()
	{
		mWrap = gameObject.GetComponent<UIWrapContent> ();
		pct = (mWrap.maxIndex - mWrap.minIndex) / gameObject.transform.childCount;

		UIScrollView msview = NGUITools.FindInParents<UIScrollView> (gameObject);
		msbar = msview.verticalScrollBar == null ? msview.horizontalScrollBar : msview.verticalScrollBar;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		msbar.value = msbar.value / pct * 1.0f;
	}
}
