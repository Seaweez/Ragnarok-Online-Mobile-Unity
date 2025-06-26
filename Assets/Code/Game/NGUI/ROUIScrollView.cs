using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	/**
     * Extend from NGUI UI Scroll View
     * Add some custom special function
     */ 
	[SLua.CustomLuaClassAttribute]
	public class ROUIScrollView : UIScrollView
	{
		// enable stop check
		public bool _stopCheckEnable = false;
		
		public OnDragNotification OnDragStarted {
			get {
				return base.onDragStarted;
			}
			set {
				base.onDragStarted = value;
			}
		}
		
		public OnDragNotification OnMomentumMove {
			get {
				return base.onMomentumMove;
			}
			set {
				base.onMomentumMove = value;
			}
		}
		
		// event
		public Action<float, float> OnPulling;
		public SpringPanel.OnFinished OnBackToStop;
		public SpringPanel.OnFinished OnStop;
		public SpringPanel.OnFinished OnRevertFinished;
		public float BackStrength = 13;
		public Transform MainTarget;
		public Transform RefreshTarget;
		
		public override Bounds bounds {
			get {
				if (!mCalculatedBounds) {
					mCalculatedBounds = true;
					mTrans = transform;
					if (null != MainTarget) {
						mBounds = NGUIMath.CalculateRelativeWidgetBounds (mTrans, MainTarget);
					} else {
						mBounds = NGUIMath.CalculateRelativeWidgetBounds (mTrans, mTrans);
					}
				}
				return mBounds;
			}
		}

		// pre drag effect
		public DragEffect PreDragEffect = DragEffect.None;
		
		// is stop
		private bool _stop = false;
		private float stopOffset;

		// default 2
		public float StopOffset {
			get {
				if (0 == stopOffset && null != RefreshTarget) {
					Bounds refreshBound = NGUIMath.CalculateRelativeWidgetBounds (mTrans, RefreshTarget);
					stopOffset = refreshBound.size.y * 2;
				}
				return stopOffset;
			}
			set {
				stopOffset = value;
			}
		}
		
		// should move
		protected override bool shouldMove {
			get {
				if (this._stop)
					return false;
				return base.shouldMove;
			}
		}

		public override void MoveRelative (UnityEngine.Vector3 relative)
		{
			if (!_stop) {
				if (null != OnPulling) {
					Vector3 constraint = mPanel.CalculateConstrainOffset (bounds.min, bounds.max);
					if (!canMoveHorizontally)
						constraint.x = 0f;
					if (!canMoveVertically)
						constraint.y = 0f;
					
					if (constraint.sqrMagnitude > 0.1f && constraint.y > 0) {
						OnPulling (constraint.magnitude, StopOffset);
					}
				}

				base.MoveRelative (relative);
			}
		}
		
		public override bool RestrictWithinBounds (bool instant, bool horizontal, bool vertical)
		{
			if (mPanel == null)
				return false;
			
			Bounds b = bounds;
			Vector3 constraint = mPanel.CalculateConstrainOffset (b.min, b.max);
			
			if (!horizontal)
				constraint.x = 0f;
			if (!vertical)
				constraint.y = 0f;
			
			if (constraint.sqrMagnitude > 0.1f) {
				if (!instant && dragEffect == DragEffect.MomentumAndSpring) {
					// Spring back into place
					Vector3 pos = mTrans.localPosition + constraint;
					pos.x = Mathf.Round (pos.x);
					pos.y = Mathf.Round (pos.y);
					SpringPanel.Begin (mPanel.gameObject, pos, 13f).strength = 8f;
					
					if (this.CheckStop (constraint)) {
						this.Stop ();
					}
				} else {
					// Jump back into place
					MoveRelative (constraint);
					
					// Clear the momentum in the constrained direction
					if (Mathf.Abs (constraint.x) > 0.01f)
						mMomentum.x = 0;
					if (Mathf.Abs (constraint.y) > 0.01f)
						mMomentum.y = 0;
					if (Mathf.Abs (constraint.z) > 0.01f)
						mMomentum.z = 0;
					mScroll = 0f;
				}
				return true;
			}
			return false;
		}
		
		private void Stop ()
		{
			this.dragEffect = DragEffect.None;
			
			this._stop = true;
			
			Bounds b = bounds;
			if (null != RefreshTarget) {
				Bounds refreshBound = NGUIMath.CalculateRelativeWidgetBounds (mTrans, RefreshTarget);
				b.Encapsulate (refreshBound);
			}
			Vector3 constraint = mPanel.CalculateConstrainOffset (b.min, b.max);
			Vector3 pos = mTrans.localPosition + constraint;
			SpringPanel.Begin (mPanel.gameObject, pos, BackStrength).onFinished = OnStop;
			if (null != OnBackToStop) {
				OnBackToStop ();
			}
		}
		
		[ContextMenu("Revert")]
		public void Revert ()
		{
			this.dragEffect = this.catchDragEffect;
			
			this._stop = false;

			Bounds b = bounds;
			Vector3 constraint = mPanel.CalculateConstrainOffset (b.min, b.max);
			Vector3 pos = mTrans.localPosition + constraint;
			pos.x = Mathf.Round (pos.x);
			pos.y = Mathf.Round (pos.y);
			SpringPanel.Begin (mPanel.gameObject, pos, BackStrength).onFinished = OnRevertFinished;
		}

		public override void ResetPosition ()
		{
			Revert ();
			base.ResetPosition ();
		}
		
		private bool CheckStop (Vector3 pos)
		{
			// check whether over stop offset
			if (!(!this.mPressed
				&& (this.centerOnChild == null)
				&& this._stopCheckEnable
				&& this.IsEnablePanelSpring ())) {
				return false;
			}
			return pos.y > StopOffset;
		}
		
		private bool IsEnablePanelSpring ()
		{
			SpringPanel sp = this.mPanel.gameObject.GetComponent<SpringPanel> ();
			if (sp != null)
				return sp.enabled;
			return false;
		}     
	}
}
