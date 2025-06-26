using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public class NoneScrollBarScrollView : UIScrollView
{
	public float curMovmentX { set;get;}
	public float curMovmentY { set;get;}

	public override void UpdatePosition ()
	{
		if (mPanel == null) mPanel = GetComponent<UIPanel>();
		
		DisableSpring();
		Bounds b = bounds;
		if (b.min.x == b.max.x || b.min.y == b.max.y) return;
		
		Vector4 clip = mPanel.finalClipRegion;
		if(clip.w > b.size.y)
			return;
		float hx = clip.z * 0.5f;
		float hy = clip.w * 0.5f;
		float left = b.min.x + hx;
		float right = b.max.x - hx;
		float bottom = b.min.y + hy;
		float top = b.max.y - hy;

		if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			left -= mPanel.clipSoftness.x;
			right += mPanel.clipSoftness.x;
			bottom -= mPanel.clipSoftness.y;
			top += mPanel.clipSoftness.y;
		}

		Vector4 cr = mPanel.baseClipRegion;
		float tmp1 = Mathf.Clamp(clip.x+cr.x,left,right) ;
		float tmp2 = Mathf.Clamp(clip.y+cr.y,bottom,top);
	
		
		Vector3 pos = mTrans.localPosition;		
		// Update the position

		if (canMoveHorizontally) pos.x += clip.x+cr.x - tmp1;
		if (canMoveVertically) pos.y += clip.y+cr.y - tmp2;

		mTrans.localPosition = pos;

		
		if (canMoveHorizontally) clip.x = tmp1;
		if (canMoveVertically) clip.y = tmp2;
		
		// Update the clipping offset
		mPanel.clipOffset = new Vector2(clip.x - cr.x, clip.y - cr.y);	
	}

	public override void SetDragAmount (float x, float y, bool updateScrollbars)
	{
		base.SetDragAmount(x,y,updateScrollbars);
		curMovmentX = x;
		curMovmentY = y;
	}
}

