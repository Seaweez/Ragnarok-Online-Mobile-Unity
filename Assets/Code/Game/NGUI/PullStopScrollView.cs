using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
    /*
     * Extend from NGUI UI Scroll View
     * Add some custom special function
     */
    [SLua.CustomLuaClassAttribute]
    public class PullStopScrollView : UIScrollView
    {
        public bool _stopCheckEnable = false;

        public OnDragNotification OnDragStarted
        {
            get
            {
                return base.onDragStarted;
            }
            set
            {
                base.onDragStarted = value;
            }
        }

        public OnDragNotification OnMomentumMove
        {
            get
            {
                return base.onMomentumMove;
            }
            set
            {
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

        public Bounds refreshBound
        {
            get
            {
                return NGUIMath.CalculateRelativeWidgetBounds(mTrans, RefreshTarget);
            }
        }

        public Bounds mainBound
        {
            get
            {
                return NGUIMath.CalculateRelativeWidgetBounds(mTrans, MainTarget);
            }
        }

        public override Bounds bounds
        {
            get
            {
                Bounds rbound = NGUIMath.CalculateAbsoluteWidgetBounds(RefreshTarget);
                if (rbound.center.y < mPanel.worldCorners[1].y)
                {
                    return base.bounds;
                }
                return mainBound;
            }
        }

        // pre drag effect
        public DragEffect PreDragEffect = DragEffect.None;

        private bool spring_lock = false;

        void SpringFinished()
        {
            spring_lock = false;
        }

        public override bool RestrictWithinBounds(bool instant, bool horizontal, bool vertical)
        {
            if (mPanel == null)
            {
                return false;
            }

            if (spring_lock)
            {
                return false;
            }

            Bounds b = bounds;
            Vector3 constraint = mPanel.CalculateConstrainOffset(b.min, b.max);

            if (!horizontal)
                constraint.x = 0f;
            if (!vertical)
                constraint.y = 0f;

            if (constraint.sqrMagnitude > 0.1f)
            {
                if (!instant && dragEffect == DragEffect.MomentumAndSpring)
                {
                    // Spring back into place
                    Vector3 pos = mTrans.localPosition + constraint;
                    pos.x = Mathf.Round(pos.x);
                    pos.y = Mathf.Round(pos.y);

                    SpringPanel sp = SpringPanel.Begin(mPanel.gameObject, pos, BackStrength);
                    sp.onFinished = SpringFinished;
                    spring_lock = true;
                }
                else
                {
                    // Jump back into place
                    MoveRelative(constraint);

                    // Clear the momentum in the constrained direction
                    if (Mathf.Abs(constraint.x) > 0.01f)
                        mMomentum.x = 0;
                    if (Mathf.Abs(constraint.y) > 0.01f)
                        mMomentum.y = 0;
                    if (Mathf.Abs(constraint.z) > 0.01f)
                        mMomentum.z = 0;
                    mScroll = 0f;
                }
                return true;
            }
            return false;
        }

        [ContextMenu("Revert")]
        public void Revert()
        {
            Bounds b = mainBound;
            Vector3 constraint = mPanel.CalculateConstrainOffset(b.min, b.max);
            Vector3 pos = mTrans.localPosition + constraint;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            SpringPanel.Begin(mPanel.gameObject, pos, BackStrength).onFinished = OnRevertFinished;
        }

        [ContextMenu("BackToStop")]
        public void BackToStop()
        {
            Bounds b = base.bounds;
            Vector3 constraint = mPanel.CalculateConstrainOffset(b.min, b.max);
            Vector3 pos = mTrans.localPosition + constraint;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            SpringPanel.Begin(mPanel.gameObject, pos, BackStrength).onFinished = OnBackToStop;
        }

        public override void Press(bool pressed)
        {
            if (spring_lock)
            {
                spring_lock = false;
            }
            base.Press(pressed);
        }

        public override void ResetPosition()
        {
            base.ResetPosition();
        }
    }
}
