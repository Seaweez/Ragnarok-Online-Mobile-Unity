using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[RequireComponent(typeof(UITexture))]
	[SLua.CustomLuaClassAttribute]
	public class RenderQByUI : MonoBehaviour
	{
		public bool excute = false;
		public int RenderQ;
		UITexture pic;
		GameObject child;
		Renderer catchRender;

		[ContextMenu("TestExcute")]
		public void TestExcute ()
		{
			foreach (Transform t in transform) {
				AddChild (t.gameObject);
			}
		}

		public void AddChild (GameObject c)
		{
			child = c;
			c.transform.SetParent (transform, false);
		}

		void Awake ()
		{
			pic = GetComponent<UITexture> ();
			if (pic.mainTexture == null)
			{
				int width = pic.width; // หรือขนาดที่คุณต้องการ
				int height = pic.height; // หรือขนาดที่คุณต้องการ
				Texture2D occupy = new Texture2D(width, height);
				// ตั้งค่าพิกเซลหรือสีถ้าจำเป็น
				pic.mainTexture = occupy;
			}

			foreach (Transform t in transform) {
				if (t != null) {
					AddChild (t.gameObject);
				}
			}
		}

		void Update ()
		{
			if (pic != null && pic.drawCall != null) {
				if (catchRender == null || catchRender.material.renderQueue != pic.drawCall.renderQueue)
					changeRenderQ ();
				if (!excute) {
					changeRenderQ ();
					excute = true;
				}
			}
		}

		void changeRenderQ ()
		{
			int rq = pic.drawCall.renderQueue;
			RenderQ = rq;
			if (child != null) {
				GameObjectUtil.Instance.ChangeLayersRecursively (child, "UI");
				Component[] cs = GameObjectUtil.Instance.GetAllComponentsInChildren (child, typeof(Renderer));
				for (int k = 0; k<cs.Length; k++) {
					Renderer r = cs [k] as Renderer;
					r.material.renderQueue = rq;
					catchRender = r;
				}
			}
		}
	}
} // namespace RO
