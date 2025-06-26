using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class UISkeletonAnimation : SkeletonAnimation
	{
		Renderer _cachedRenderer;
		int _renderQ = 3000;

		void OnDestroy()
		{
			Destroy (mesh1);
			Destroy (mesh2);
		}

		public override void Reset ()
		{
			base.Reset ();
			_cachedRenderer = GetComponent<Renderer> ();
		}
	
		public override void Update ()
		{
			base.Update ();
		}

		public override void LateUpdate ()
		{
			if (_cachedRenderer != null) {
				if (_cachedRenderer.sharedMaterial != null)
					_renderQ = _cachedRenderer.sharedMaterial.renderQueue;
				else if (Application.isPlaying && _cachedRenderer.material != null)
					_renderQ = _cachedRenderer.material.renderQueue;
			}
			base.LateUpdate ();
			if (_cachedRenderer == null)
				_cachedRenderer = GetComponent<Renderer> ();
			if (Application.isPlaying)
				_cachedRenderer.material.renderQueue = _renderQ;
		}
	}
} // namespace RO
