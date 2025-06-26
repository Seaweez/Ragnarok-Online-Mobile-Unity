using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class UIScrollViewEx : UIScrollView
	{
		public GameObject BoundTarget;
		
		public override Bounds bounds {
			get {
				if (!mCalculatedBounds) {
					mCalculatedBounds = true;
					mTrans = transform;
					if (null != BoundTarget) {
						mBounds = NGUIMath.CalculateRelativeWidgetBounds (mTrans, BoundTarget.transform);
					} else {
						mBounds = NGUIMath.CalculateRelativeWidgetBounds (mTrans, mTrans);
					}
				}
				return mBounds;
			}
		}
	}
} // namespace RO
