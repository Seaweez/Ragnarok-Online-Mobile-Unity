using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	/**
     * 扩展NGUI的滚动视图。可有当前progress的回调
     */ 
	[SLua.CustomLuaClassAttribute]
	public class ScrollViewWithProgress : UIScrollView
	{
		public class StateRange
		{
			public float min;
			public float max;

			public bool IsInRange (float value)
			{
				return min <= value && max >= value;
			}
		}

		public delegate void OnProgressNotification (float progress,int custom);
		//进度回调
		public OnProgressNotification onProgress;
		//进度状态
		public OnProgressNotification onProgressStateChange;

		public float progress{ get; private set; }

		public int state{ get; private set; }

		protected StateRange _currentRange;
		protected SDictionary<int,StateRange> _states = new SDictionary<int, StateRange> ();

		public void AddStateRange (int state, float min, float max, bool needCheckRightNow = false)
		{
			StateRange range = _states [state];
			if (range == null) {
				range = new StateRange ();
				_states [state] = range;
			}
			range.min = min;
			range.max = max;
			if (needCheckRightNow)
				CheckState ();
		}

		public void RemoveStateRange(int state)
		{
			StateRange range = _states [state];
			if (range != null && range == _currentRange) {
				_currentRange = null;
			}
			_states.Remove (state);
		}

		public void CheckState ()
		{
			if (_currentRange == null || _currentRange.IsInRange (progress) == false) {
				foreach (KeyValuePair<int,StateRange> kvp in _states) {
					if (kvp.Value.IsInRange (progress)) {
						if (state != kvp.Key) {
							state = kvp.Key;
							_currentRange = kvp.Value;
							if (onProgressStateChange != null)
								onProgressStateChange (progress, state);
							break;
						}
					}
				}
			}
		}

		public override void UpdateScrollbars (bool recalculateBounds)
		{
			if (mPanel == null)
				return;

			if (recalculateBounds) {
				mCalculatedBounds = false;
				mShouldMove = shouldMove;
			}
			
			Bounds b = bounds;
			Vector2 bmin = b.min;
			Vector2 bmax = b.max;

			if (movement == Movement.Horizontal && bmax.x > bmin.x) {
				Vector4 clip = mPanel.finalClipRegion;
				int intViewSize = Mathf.RoundToInt (clip.z);
				if ((intViewSize & 1) != 0)
					intViewSize -= 1;
				float halfViewSize = intViewSize * 0.5f;
				halfViewSize = Mathf.Round (halfViewSize);
				
				if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
					halfViewSize -= mPanel.clipSoftness.x;
				
				float contentSize = bmax.x - bmin.x;
				float viewSize = halfViewSize * 2f;
				float contentMin = bmin.x;
				float contentMax = bmax.x;
				float viewMin = clip.x - halfViewSize;
				float viewMax = clip.x + halfViewSize;
				
				contentMin = viewMin - contentMin;
				contentMax = contentMax - viewMax;
				
				UpdateScrollbars (contentMin, contentMax, contentSize, viewSize, false);
			}
			
			if (movement == Movement.Vertical && bmax.y > bmin.y) {
				Vector4 clip = mPanel.finalClipRegion;
				int intViewSize = Mathf.RoundToInt (clip.w);
				if ((intViewSize & 1) != 0)
					intViewSize -= 1;
				float halfViewSize = intViewSize * 0.5f;
				halfViewSize = Mathf.Round (halfViewSize);
				
				if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
					halfViewSize -= mPanel.clipSoftness.y;
				
				float contentSize = bmax.y - bmin.y;
				float viewSize = halfViewSize * 2f;
				float contentMin = bmin.y;
				float contentMax = bmax.y;
				float viewMin = clip.y - halfViewSize;
				float viewMax = clip.y + halfViewSize;
				
				contentMin = viewMin - contentMin;
				contentMax = contentMax - viewMax;
				
				UpdateScrollbars (contentMin, contentMax, contentSize, viewSize, true);
			}
		}

		protected void UpdateScrollbars (float contentMin, float contentMax, float contentSize, float viewSize, bool inverted)
		{
			mIgnoreCallbacks = true;
			{
				float contentPadding;
				float tempProgress;
				if (viewSize < contentSize) {
					contentMin = Mathf.Clamp01 (contentMin / contentSize);
					contentMax = Mathf.Clamp01 (contentMax / contentSize);
					
					contentPadding = contentMin + contentMax;
					tempProgress = inverted ? ((contentPadding > 0.001f) ? 1f - contentMin / contentPadding : 0f) :
						((contentPadding > 0.001f) ? contentMin / contentPadding : 1f);
				} else {
					contentMin = Mathf.Clamp01 (-contentMin / contentSize);
					contentMax = Mathf.Clamp01 (-contentMax / contentSize);
					
					contentPadding = contentMin + contentMax;
					tempProgress = inverted ? ((contentPadding > 0.001f) ? 1f - contentMin / contentPadding : 0f) :
						((contentPadding > 0.001f) ? contentMin / contentPadding : 1f);
					
					if (contentSize > 0) {
						contentMin = Mathf.Clamp01 (contentMin / contentSize);
						contentMax = Mathf.Clamp01 (contentMax / contentSize);
						contentPadding = contentMin + contentMax;
					}
				}
				if (tempProgress != progress) {
					progress = tempProgress;
					if (onProgress != null)
						onProgress (progress, 0);
					CheckState ();
				}
			}
			mIgnoreCallbacks = false;
		}
	}
} // namespace RO
