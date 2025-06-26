//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// If you don't have or don't wish to create an atlas, you can simply use this script to draw a texture.
/// Keep in mind though that this will create an extra draw call with each UITexture present, so it's
/// best to use it only for backgrounds or temporary visible widgets.
/// </summary>

[ExecuteInEditMode]
[SLua.CustomLuaClassAttribute]
[AddComponentMenu("NGUI/UI/NGUI Texture Ex")]
public class UITextureEx : UITexture
{
	public enum TypeEx
	{
		Fitted,
	}
	public TypeEx typeEx = TypeEx.Fitted;
	public Vector2 anchor = new Vector2(0.5f, 0.5f);

	private void SetFittedUVRect(Texture tex)
	{
		Vector2 size = new Vector2(tex.width, tex.height);
		
		var u = new Rect(0,0,1,1);
		
		var vAspect = localSize.y/localSize.x;
		var uAspect = size.y/size.x;
		if (vAspect > uAspect)
		{
			u.width = size.y/vAspect / size.x;
			u.x = (1-u.width) * Mathf.Clamp01(anchor.x);
		}
		else if (vAspect < uAspect)
		{
			u.height = size.x*vAspect / size.y;
			u.y = (1-u.height) * Mathf.Clamp01(anchor.y);
		}
		
		uvRect = u;
	}

	public override void OnFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture tex = mainTexture;
		if (tex == null) return;

		switch (typeEx)
		{
		case TypeEx.Fitted:
			SetFittedUVRect(tex);
			break;
		}

		base.OnFill(verts, uvs, cols);
	}
}


