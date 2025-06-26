using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public class WrapSliderValue : MonoBehaviour
{
	public UIScrollBar bar;
	public float showHideSpeed = 1;
	public bool isAlwaysShow = false;
	UIPanel panel;
	UIWrapContent wrapContent;
	UIScrollView scrollView;
	UIScrollView.Movement dir = UIScrollView.Movement.Horizontal;
	Vector2 panelOffset = Vector2.zero;
	Vector3 startPos;
	float minValue = 0;
	float maxValue = 0;
	bool canExcute = false;

	void Awake ()
	{
		canExcute = bar != null;

		panel = GetComponent<UIPanel> ();
		scrollView = GetComponent<UIScrollView> ();
		wrapContent = GetComponentInChildren<UIWrapContent> ();
		panelOffset = panel.clipOffset;
		dir = scrollView.movement;
		startPos = transform.localPosition;

		Calculation_TotalValue ();
	}

	void Calculation_TotalValue ()
	{
		float offsetl = dir == UIScrollView.Movement.Horizontal ? panelOffset.x : panelOffset.y;
		float sizel = dir == UIScrollView.Movement.Horizontal ? panel.GetViewSize ().x : panel.GetViewSize ().y;
		sizel = (System.Convert.ToInt32 (sizel / wrapContent.itemSize) - 1) * wrapContent.itemSize;
		minValue = wrapContent.minIndex * wrapContent.itemSize - offsetl;
		maxValue = wrapContent.maxIndex * wrapContent.itemSize + offsetl - sizel;
//		bar.barSize = sizel / maxValue * 1.0f;
	}

	float Calculation_BarValue ()
	{
		float l = dir == UIScrollView.Movement.Horizontal ? panel.clipOffset.x : panel.clipOffset.y;
		return Mathf.InverseLerp (minValue, maxValue, l);
	}

	void LateUpdate ()
	{
		if (canExcute) {
			float nValue = Calculation_BarValue ();
			if (dir == UIScrollView.Movement.Vertical)
				nValue = 1 - nValue;
			bar.value = nValue;
			if (!isAlwaysShow)
				changeAlpha ();
		}
	}

	void changeAlpha ()
	{
		float alpha = bar.alpha;
		float delta = RealTime.deltaTime;
		bool sholdMove = scrollView.isDragging;
		alpha += sholdMove ? delta * 10 * showHideSpeed : -delta * 3 * showHideSpeed;
		alpha = Mathf.Clamp01 (alpha);
		if (bar.alpha != alpha)
			bar.alpha = alpha;
	}

	public void ResetPosition ()
	{
		panel.clipOffset = panelOffset;
		transform.localPosition = startPos;
		scrollView.ResetPosition ();

		wrapContent.mFirstTime = true;
		wrapContent.WrapContent ();

	}

}
