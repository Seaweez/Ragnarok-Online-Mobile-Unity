using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class RolePartBody : RolePartBodyMount 
	{
		public LocalBounds localBounds;

		public new BoxCollider collider;

		public Animator mainAnimator;

		public RoleComplete owner;

		#region override
		public override void ApplyLayer ()
		{
			ApplyLayerIgnoreCPs();
			if (0 == layer && null != collider)
			{
				collider.gameObject.layer = Config.Layer.ACCESSABLE.Value;
			}
		}

		public override void PlayAction(int nameHash, int defaultNameHash, float speed, float normalizedTime)
		{
			base.PlayAction(nameHash, defaultNameHash, speed, normalizedTime);
			Invoke("UpdateCollider", (null != LuaLuancher.Me ? LuaLuancher.Me.roleColliderUpdateDelay : 0.1f));
		}
		#endregion override

		public bool hasBounds
		{
			get
			{
				return (null != localBounds) || (null != mainSMR) || (null != mainMR);
			}
		}

		public bool HasAction(int nameHash)
		{
			return null != mainAnimator && mainAnimator.HasState(0, nameHash);
		}

		private static Vector3 LimitColliderSize(Vector3 size)
		{
			if (null != LuaLuancher.Me)
			{
				var minSize = LuaLuancher.Me.roleColliderMinSize;
				var maxSize = LuaLuancher.Me.roleColliderMaxSize;
				size.x = Mathf.Clamp(size.x, minSize.x, maxSize.x);
				size.y = Mathf.Clamp(size.y, minSize.y, maxSize.y);
				size.z = Mathf.Clamp(size.z, minSize.z, maxSize.z);
			}
			return size;
		}
		public void AdjustCollider(BoxCollider c)
		{
			if (null != localBounds)
			{
				c.center = localBounds.bounds.center;
				c.size = localBounds.bounds.size;
			}
			else if (null != mainSMR)
			{
				c.center = transform.InverseTransformPoint(mainSMR.bounds.center);
				var size = LimitColliderSize(mainSMR.bounds.size).Divide(transform.lossyScale);
				c.size = size;
			}
			else if (null != mainMR)
			{
				c.center = transform.InverseTransformPoint(mainMR.bounds.center);
				var size = LimitColliderSize(mainMR.bounds.size).Divide(transform.lossyScale);
				c.size = size;
			}
		}

		public void AdjustShadow(SpriteRenderer shadow)
		{
			if (null != collider)
			{
				AdjustCollider(collider);

				var spriteBounds = shadow.sprite.bounds;
				var spriteSize = Mathf.Max(spriteBounds.size.x, spriteBounds.size.y);

				var size = collider.size.XZ ().magnitude;

				shadow.transform.localScale = Vector3.one * (size/spriteSize);
			}
			else
			{
				shadow.transform.localScale = Vector3.one;
			}
		}

		public void AdjustProjector(Projector projector, float projectorHeight, float sizeScale)
		{
			if (null != collider)
			{
				var size = collider.size.XZ ().magnitude;
				size *= sizeScale;
				projector.fieldOfView = (Mathf.Atan2(size/2.0f, projectorHeight)*180f/Mathf.PI) * 2;
			}
			else
			{
				projector.fieldOfView = 30;
			}
		}

		public void UpdateCollider()
		{
			if (null != collider)
			{
				AdjustCollider(collider);
			}
		}

		#region action event
		[SLua.DoNotToLuaAttribute]
		public void ActionEventFire()
		{
			if (null != owner)
			{
				owner.ActionEventFire();
			}
		}

        [SLua.DoNotToLuaAttribute]
        public void ActionEventInterrupt()
        {
            if (null != owner)
            {
                owner.ActionEventInterrupt();
            }
        }

        [SLua.DoNotToLuaAttribute]
		public void ActionEventDead()
		{
			if (null != owner)
			{
				owner.ActionEventDead();
			}
		}
		#endregion action event

		#region behaviour
//		void Update()
//		{
//			UpdateCollider();
//		}
		#endregion behaviour
	}
} // namespace RO
