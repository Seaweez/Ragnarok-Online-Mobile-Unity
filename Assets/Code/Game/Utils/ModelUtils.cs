using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public static class ModelUtils 
	{
		public static readonly Quaternion threeDMaxImportedModelRotation = Quaternion.Euler(new Vector3(-90, 0, 0));

		public static bool IsThreeDMaxImportedModel(Transform transform)
		{
			var localRotation = transform.localRotation;

			var test = 0.01f;
			return test > Mathf.Abs(threeDMaxImportedModelRotation.x-localRotation.x) 
					&& test > Mathf.Abs(threeDMaxImportedModelRotation.y-localRotation.y) 
					&& test > Mathf.Abs(threeDMaxImportedModelRotation.z-localRotation.z) 
					&& test > Mathf.Abs(threeDMaxImportedModelRotation.w-localRotation.w);
		}

		public static void AdjustSize(GameObject obj, float size)
		{
			if (!AdjustProjectorSize(obj, size))
			{
				AdjustSpriteSize(obj, size);
			}
		}

		public static void AdjustProjectorSize(Projector proj, float projectorHeight, float size)
		{
			proj.fieldOfView = (Mathf.Atan2(size/2.0f, projectorHeight)*180f/Mathf.PI) * 2;
		}

		public static bool AdjustProjectorSize(GameObject obj, float size)
		{
			var proj = obj.GetComponentInChildren<Projector>();
			if (null == proj)
			{
				return false;
			}
			AdjustProjectorSize(proj, proj.transform.position.y-obj.transform.position.y, size);
			return true;
		}

		public static void AdjustProjector(GameObject obj, Projector proj, float projectorHeight, float sizeScale = 1f)
		{
			if (null == proj)
			{
				return;
			}
			float lossyScaleY = 1;
			if (null == obj)
			{
				proj.transform.localPosition = new Vector3(0, projectorHeight, 0);
				proj.fieldOfView = 30;
			}
			else
			{
				var smr = obj.GetComponentInChildrenTopLevel<SkinnedMeshRenderer>();
				if (null == smr)
				{
					return;
				}
				lossyScaleY = obj.transform.lossyScale.y;
				var localBounds = smr.localBounds;
				if (IsThreeDMaxImportedModel(smr.transform))
				{
					var oldCenter = localBounds.center;
					localBounds.center = new Vector3(oldCenter.y, oldCenter.x, oldCenter.z);
					
					var oldSize = localBounds.size;
					localBounds.size = new Vector3(oldSize.y, oldSize.x, oldSize.z);
				}
				proj.transform.localPosition = new Vector3(localBounds.center.x, projectorHeight, localBounds.center.z);
				var size = new Vector2(localBounds.size.x, localBounds.size.z).magnitude;
				size *= sizeScale;
				AdjustProjectorSize(proj, projectorHeight, size);
			}
			proj.farClipPlane = proj.transform.localPosition.y * lossyScaleY * 2 + 1;
		}

		public static void AdjustSpriteSize(SpriteRenderer spriteRenderer, float size)
		{
			var spriteBounds = spriteRenderer.sprite.bounds;
			var spriteSize = Mathf.Max(spriteBounds.size.x, spriteBounds.size.y);
			spriteRenderer.transform.localScale = Vector3.one * (size/spriteSize);
		}

		public static bool AdjustSpriteSize(GameObject obj, float size)
		{
			var spriteRenderer = obj.GetComponentInChildren<SpriteRenderer>();
			if (null == spriteRenderer)
			{
				return false;
			}
			AdjustSpriteSize(spriteRenderer, size);
			return true;
		}

		public static void AdjustSprite(GameObject obj, SpriteRenderer spriteRenderer, float sizeScale = 1f)
		{
			if (null == spriteRenderer)
			{
				return;
			}
			if (null != obj)
			{
				var smr = obj.GetComponentInChildrenTopLevel<SkinnedMeshRenderer>();
				if (null == smr)
				{
					return;
				}
				var localBounds = smr.localBounds;
				if (IsThreeDMaxImportedModel(smr.transform))
				{
					var oldCenter = localBounds.center;
					localBounds.center = new Vector3(oldCenter.y, oldCenter.x, oldCenter.z);

					var oldSize = localBounds.size;
					localBounds.size = new Vector3(oldSize.y, oldSize.x, oldSize.z);
				}

				spriteRenderer.transform.localPosition = new Vector3(localBounds.center.x, 0, localBounds.center.z);
				var size = new Vector2(localBounds.size.x, localBounds.size.z).magnitude;
				size *= sizeScale;

				AdjustSpriteSize(spriteRenderer, size);
			}
		}
	
	}
} // namespace RO
