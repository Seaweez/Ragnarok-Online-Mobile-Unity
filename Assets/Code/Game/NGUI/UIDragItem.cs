//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using System;
using RO;

[SLua.CustomLuaClassAttribute]
public class UIDragItem : MonoBehaviour
{
	static Vector2 empty = new Vector2 ();
	public UISprite icon;
	[SLua.CustomLuaClassAttribute]
	public enum DragDropType
	{
		Empty,
		Target,
		Source,
		Both
	}
	public DragDropType type;
	public bool cancelScrollWhenDrag = true;
	public Func<object,object> OnReplace;
	public Action<object> OnDropEmpty;
	public Action<object> OnStart;
	public Action<object> OnCursor;
	public Func<object> GetObserved;
	public object data;
	static object mDraggedItem;
	static bool mIsDragging = false;
	bool _DragEnable = true;
	public UIScrollView scrollView;
	UIScrollView mScroll;
	public bool dropEmpty;

	public bool DragEnable {
		set{ _DragEnable = value;}
		get{ return _DragEnable;}
	}

	/// <summary>
	/// This function should return the item observed by this UI class.
	/// </summary>

	virtual protected object observedItem { 
		get {
			if (GetObserved != null)
				return GetObserved ();
			return null;
		} 
	}

	/// <summary>
	/// Replace the observed item with the specified value. Should return the item that was replaced.
	/// </summary>

	virtual protected object Replace (object item)
	{
		if (OnReplace != null) {
			return OnReplace (item);
		}
		return null;
	}

	void Start ()
	{
		FindScrollView ();
	}

	/// <summary>
	/// Allow to move objects around via drag & drop.
	/// </summary>

	void OnClick ()
	{
		if (mDraggedItem != null) {
			OnDrop (null);
		} else if (data != null) {
			mDraggedItem = Replace (null);
			UpdateCursor ();
		}
	}

	void StartDrag ()
	{
		mIsDragging = true;
		if (OnStart != null)
			OnStart (this);
		SetScrollViewCanDrag ();
	}

	void EndDrag ()
	{
		mIsDragging = false;
		SetScrollViewCanDrag ();
	}

	public void StopDrag ()
	{
		mDraggedItem = null;
//		EndDrag ();
		UICursorWithTween.Clear ();
	}

	public virtual void ManualStartDrag ()
	{
		OnDrag (empty);
	}

	public virtual void ManualEndDrag ()
	{
		OnDragEnd ();
	}

	/// <summary>
	/// Start dragging the item.
	/// </summary>

	protected virtual void OnDrag (Vector2 delta)
	{
		if (_DragEnable && mIsDragging == false && data != null && type > DragDropType.Target) {
			if (UICamera.currentTouch != null)
				UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
//			mDraggedItem = Replace (null);
			mDraggedItem = data;
			StartDrag ();
			UpdateCursor ();
		}
	}

	/// <summary>
	/// Stop dragging the item.
	/// </summary>

	protected virtual void OnDrop (GameObject go)
	{
//		print ("OnDrop " + this.gameObject.name);
		if (go != null) {
			UIDragItem source = go.GetComponent<UIDragItem> ();
			if (source != null) {
				source.dropEmpty = false;
			}
		}
		if (type != DragDropType.Source)
			Replace (mDraggedItem);
		EndDrag ();
		mDraggedItem = null;
		UpdateCursor ();
	}

	protected virtual void OnDragEnd ()
	{
		if (_DragEnable && UICamera.currentTouch != null && UICamera.currentTouch.dragged != UICamera.currentTouch.current)
			dropEmpty = true;
//		print ("OnDragEnd " + this.gameObject.name);
//		mDraggedItem = null;
		EndDrag ();
		UICursorWithTween.Clear ();
	}

	/// <summary>
	/// Set the cursor to the icon of the item being dragged.
	/// </summary>

	protected virtual void UpdateCursor ()
	{
		if (mDraggedItem != null) {
			if (OnCursor != null)
				OnCursor (this);
			else
				UICursorWithTween.Set (icon.atlas, icon.spriteName);
		} else {
//			print ("Clear");
			UICursorWithTween.Clear ();
		}
	}

	void FindScrollView ()
	{
		// If the scroll view is on a parent, don't try to remember it (as we want it to be dynamic in case of re-parenting)
		UIScrollView sv = NGUITools.FindInParents<UIScrollView> (transform);
		
		if (scrollView == null || (sv != scrollView)) {
			scrollView = sv;
		}
		mScroll = scrollView;
	}

	void SetScrollViewCanDrag ()
	{
		if (mScroll != null && cancelScrollWhenDrag)
			mScroll.enabled = !mIsDragging;
	}

	protected virtual void LateUpdate ()
	{
		if (dropEmpty == true) {
			dropEmpty = false;
			if (OnDropEmpty != null)
				OnDropEmpty (this);
		}
	}
	/// <summary>
	/// Keep an eye on the item and update the icon when it changes.
	/// </summary>
}
