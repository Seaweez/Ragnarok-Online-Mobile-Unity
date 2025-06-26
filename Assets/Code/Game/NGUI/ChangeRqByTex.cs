using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class ChangeRqByTex : UITexture
	{
		public bool excute;
		public int RenderQ;
		static Texture2D occupy;

		protected override void Awake ()
		{
			base.Awake ();

			foreach (Transform t in transform) {
				if (t != null) {
					AddChild (t.gameObject);
				}
			}
			onRender = m => changeRenderQ ();
		}

		protected override void OnStart ()
		{
			base.OnStart ();

			// hide texture
			if (null == mainTexture) {
				if (occupy == null) {
					occupy = new Texture2D (4, 4, TextureFormat.RGBA32, false);
					ImageConversion.EncodeToPNG (occupy);

					Color[] pixels = occupy.GetPixels ();
					for (int i = 0; i < pixels.Length; i++) {
						pixels [i].r = 1.0f / 255;
						pixels [i].g = 1.0f / 255;
						pixels [i].b = 1.0f / 255;
						pixels [i].a = 1.0f / 255;
					}
					occupy.SetPixels (pixels);
					occupy.Apply ();
				}
				mainTexture = occupy;
			}
			//width = 2;
			//height = 2;
		}

		void OnDestroy ()
		{
			RemoveFromPanel ();
			mainTexture = null;
		}

		protected override void OnEnable ()
		{
			base.OnEnable ();
			excute = false;
		}

		public void AddChild (GameObject c)
		{
			c.transform.SetParent (transform, false);
			excute = false;
		}

		protected override void OnUpdate ()
		{
			if (null != drawCall) {
				if (!excute) {
					changeRenderQ ();
					excute = true;
				}
			} 
			base.OnUpdate ();
		}

		void changeRenderQ ()
		{
			if (null != drawCall) {
				RenderQ = drawCall.renderQueue;
				GameObjectUtil.Instance.ChangeLayersRecursively (gameObject, LayerMask.LayerToName (gameObject.layer));
				Component[] cs = GameObjectUtil.Instance.GetAllComponentsInChildren (gameObject, typeof(Renderer));
				for (int k = 0; k < cs.Length; k++) {
					Renderer r = cs [k] as Renderer;
					if (null != r && null != r.sharedMaterial)
						r.sharedMaterial.renderQueue = RenderQ;
				}
			}
		}
	}
}
// namespace RO
