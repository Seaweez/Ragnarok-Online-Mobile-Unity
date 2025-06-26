using UnityEngine;
using System.Collections;
[SLua.CustomLuaClassAttribute]
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Custom Scroll Bar")]
public class UICustomScrollBar : UIScrollBar
{

	public bool isDragging { get { return mPressed && mDragStarted; } }
	protected bool mDragStarted = false;
	protected bool mPressed = false;
	// Use this for initialization

	protected override void OnPressForeground (GameObject go, bool isPressed)
	{
		base.OnPressForeground(go,isPressed);
		mPressed = isPressed;
		mDragStarted = isPressed;
	}

	protected override void OnDragForeground (GameObject go, Vector2 delta)
	{
		base.OnDragForeground(go,delta);
		mDragStarted = true;
	}

}

