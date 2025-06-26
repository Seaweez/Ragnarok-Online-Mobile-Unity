using UnityEngine;
using System.Collections.Generic;

//-----------------------------------------------------------------------------
// Copyright 2015-2016 RenderHeads Ltd.  All rights reserverd.
//-----------------------------------------------------------------------------

namespace RenderHeads.Media.AVProVideo
{
	[AddComponentMenu("AVPro Video/Apply To Delegate")]
//	[SLua.CustomLuaClassAttribute]
	public class ApplyToDelegate : MonoBehaviour
	{
		// texture, scale, offset
		public System.Action<Texture, Vector2, Vector2> receiver;
		public MediaPlayer _media;
		public Texture2D _defaultTexture;

		void Update ()
		{
			bool applied = false;
			if (_media != null && _media.TextureProducer != null) {
				Texture texture = _media.TextureProducer.GetTexture ();
				if (texture != null) {
					ApplyMapping (texture, _media.TextureProducer.RequiresVerticalFlip ());
					applied = true;
				}
			}

			if (!applied) {
				ApplyMapping (_defaultTexture, false);
			}
		}
		
		private void ApplyMapping (Texture texture, bool requiresYFlip)
		{
			if (receiver != null) {
				Vector2 scale = Vector2.one;
				Vector2 offset = Vector2.zero;
				if (requiresYFlip) {
					scale = new Vector2 (1.0f, -1.0f);
					offset = new Vector3 (0.0f, 1.0f);
				}

				receiver (texture, scale, offset);
			}
		}

		void OnEnable ()
		{
			Update ();
		}
		
		void OnDisable ()
		{
			ApplyMapping (_defaultTexture, false);
		}

	}
}