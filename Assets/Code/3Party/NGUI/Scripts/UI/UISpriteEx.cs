//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Sprite is a textured element in the UI hierarchy.
/// </summary>

[ExecuteInEditMode]
[SLua.CustomLuaClassAttribute]
[AddComponentMenu("NGUI/UI/NGUI Sprite Ex")]
public class UISpriteEx : UISprite
{
	public enum TypeEx
	{
		Fitted,
	}
	public TypeEx typeEx = TypeEx.Fitted;
	public Vector2 anchor = new Vector2(0.5f, 0.5f);

	Color32 drawingColor
	{
		get
		{
			Color colF = color;
			colF.a = finalAlpha;
			if (premultipliedAlpha) colF = NGUITools.ApplyPMA(colF);
			
			if (QualitySettings.activeColorSpace == ColorSpace.Linear)
			{
				colF.r = Mathf.GammaToLinearSpace(colF.r);
				colF.g = Mathf.GammaToLinearSpace(colF.g);
				colF.b = Mathf.GammaToLinearSpace(colF.b);
			}
			return colF;
		}
	}

	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 offset = pivotOffset;
			
			float x0 = -offset.x * mWidth;
			float y0 = -offset.y * mHeight;
			float x1 = x0 + mWidth;
			float y1 = y0 + mHeight;
			
			return new Vector4(
				mDrawRegion.x == 0f ? x0 : Mathf.Lerp(x0, x1, mDrawRegion.x),
				mDrawRegion.y == 0f ? y0 : Mathf.Lerp(y0, y1, mDrawRegion.y),
				mDrawRegion.z == 1f ? x1 : Mathf.Lerp(x0, x1, mDrawRegion.z),
				mDrawRegion.w == 1f ? y1 : Mathf.Lerp(y0, y1, mDrawRegion.w));
		}
	}

	Vector4 drawingUVs
	{
		get
		{
			return new Vector4(mInnerUV.xMin, mInnerUV.yMin, mInnerUV.xMax, mInnerUV.yMax);
		}
	}

	[System.NonSerialized] Rect mInnerUV = new Rect();

	public override void OnFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture tex = mainTexture;
		if (tex == null) return;

		if (mSprite == null) mSprite = atlas.GetSprite(spriteName);
		if (mSprite == null) return;

		Rect outer = new Rect(mSprite.x, mSprite.y, mSprite.width, mSprite.height);
		Rect inner = new Rect(mSprite.x + mSprite.borderLeft, mSprite.y + mSprite.borderTop,
			mSprite.width - mSprite.borderLeft - mSprite.borderRight,
			mSprite.height - mSprite.borderBottom - mSprite.borderTop);

		outer = NGUIMath.ConvertToTexCoords(outer, tex.width, tex.height);
		inner = NGUIMath.ConvertToTexCoords(inner, tex.width, tex.height);

		int offset = verts.size;
		FillEx(verts, uvs, cols, outer, inner);

		if (onPostFill != null)
			onPostFill(this, offset, verts, uvs, cols);
	}

	protected void FillEx (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, Rect outer, Rect inner)
	{
		mInnerUV = inner;

		switch (typeEx)
		{
		case TypeEx.Fitted:
			FittedFill(verts, uvs, cols);
			break;
		default:
			break;
		}
	}

	void FittedFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture tex = mainTexture;
		if (tex == null) return;
		
		Vector2 size = new Vector2(mInnerUV.width * tex.width, mInnerUV.height * tex.height);
		size *= pixelSize;
		if (tex == null || size.x < 2f || size.y < 2f) return;
		
		Color32 c = drawingColor;
		Vector4 v = drawingDimensions;
		Vector4 u = drawingUVs;
		
		var uWidth = Mathf.Abs(u.z-u.x);
		var uHeight = Mathf.Abs(u.w-u.y);
		
		var vAspect = localSize.y/localSize.x;
		var uAspect = size.y/size.x;
		if (vAspect > uAspect)
		{
			var newUWidth = uHeight/vAspect;
			u.x += (uWidth-newUWidth) * Mathf.Clamp01(anchor.x);
			u.z = u.x + newUWidth;
		}
		else if (vAspect < uAspect)
		{
			var newUHeight = uWidth*vAspect;
			u.y += (uHeight-newUHeight) * Mathf.Clamp01(anchor.y);
			u.w = u.y + newUHeight;
		}
		
		verts.Add(new Vector3(v.x, v.y));
		verts.Add(new Vector3(v.x, v.w));
		verts.Add(new Vector3(v.z, v.w));
		verts.Add(new Vector3(v.z, v.y));
		
		uvs.Add(new Vector2(u.x, u.y));
		uvs.Add(new Vector2(u.x, u.w));
		uvs.Add(new Vector2(u.z, u.w));
		uvs.Add(new Vector2(u.z, u.y));
		
		cols.Add(c);
		cols.Add(c);
		cols.Add(c);
		cols.Add(c);
	}
}
