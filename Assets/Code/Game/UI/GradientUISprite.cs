using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class GradientUISprite : UISprite
	{
		public enum GradientDirection
		{
			Horizontal,
			Vertical,
		}
		public bool isGradient = true;
		public GradientDirection dir = GradientDirection.Vertical;
		[HideInInspector]
		[SerializeField]
		protected Color mgradientTop = Color.white;
		[HideInInspector]
		[SerializeField]
		protected Color mgradientBottom = Color.black;

		public Color gradientTop {
			get{ return mgradientTop;}
			set {
				if (mgradientTop != value) {
					mgradientTop = value;
					mChanged = true;
				}
			}
		}

		public Color gradientBottom {
			get{ return mgradientBottom;}
			set {
				if (mgradientBottom != value) {
					mgradientBottom = value;
					mChanged = true;
				}
			}
		}

		public override void OnFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
		{
//			base.OnFill (verts, uvs, cols);
			Texture tex = mainTexture;
			if (tex == null)
				return;
			
			if (mSprite == null)
				mSprite = atlas.GetSprite (spriteName);
			if (mSprite == null)
				return;
			
			Rect outer = new Rect (mSprite.x, mSprite.y, mSprite.width, mSprite.height);
			Rect inner = new Rect (mSprite.x + mSprite.borderLeft, mSprite.y + mSprite.borderTop,
			                      mSprite.width - mSprite.borderLeft - mSprite.borderRight,
			                      mSprite.height - mSprite.borderBottom - mSprite.borderTop);
			
			outer = NGUIMath.ConvertToTexCoords (outer, tex.width, tex.height);
			inner = NGUIMath.ConvertToTexCoords (inner, tex.width, tex.height);
			
			int offset = verts.size;
			Fill (verts, uvs, cols, outer, inner);
			
			if (onPostFill != null)
				onPostFill (this, offset, verts, uvs, cols);
			if (isGradient) {
				GradientColor (cols);
			}
		}

		protected void GradientColor (BetterList<Color32> cols)
		{
			int count = cols.size;
			count /= 4;
			cols.Clear ();
			if (this.type == Type.Sliced) {
				float width = (float)this.width;
				float height = (float)this.height;
				float h = 0;
				Color l = gradientTop;
				Color r = gradientBottom;
				if (dir == GradientDirection.Vertical) {
					for (int x = 0; x < 3; ++x) {
						h = 0;
						for (int y = 0; y < 3; ++y) {
							if (centerType == AdvancedType.Invisible && x == 1 && y == 1)
								continue;
							int y2 = y + 1;
							l = Color.Lerp (gradientTop, gradientBottom, 1 - h / height);
							h += mTempPos [y2].y - mTempPos [y].y;
							r = Color.Lerp (gradientTop, gradientBottom, 1 - h / height);
							VAddColor (cols, r, l);
						}
					}
				} else {
					for (int x = 0; x < 3; ++x) {
						int x2 = x + 1;
						l = Color.Lerp (gradientTop, gradientBottom, h / width);
						h += mTempPos [x2].x - mTempPos [x].x;
						r = Color.Lerp (gradientTop, gradientBottom, h / width);
						for (int y = 0; y < 3; ++y) {
							if (centerType == AdvancedType.Invisible && x == 1 && y == 1)
								continue;
							HAddColor (cols, l, r);
						}
					}
				}
			} else {
				if (dir == GradientDirection.Vertical) {
					for (int i=0; i<count; i++) {
						VAddColor (cols, gradientTop, gradientBottom);
					}
				} else {
					for (int i=0; i<count; i++) {
						HAddColor (cols, gradientTop, gradientBottom);
					}
				}
			}
		}

		protected void HAddColor (BetterList<Color32> cols, Color left, Color right)
		{
			cols.Add (left);
			cols.Add (left);
			cols.Add (right);
			cols.Add (right);
		}

		protected void VAddColor (BetterList<Color32> cols, Color top, Color bottom)
		{
			cols.Add (bottom);
			cols.Add (top);
			cols.Add (top);
			cols.Add (bottom);
		}
	}
} // namespace RO
