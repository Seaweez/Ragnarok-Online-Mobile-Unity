using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RO
{
	/// <summary>
	/// + Scroll View
	/// |- ROUITable
	/// |-- Item 1
	/// |-- Item 2
	/// |-- Item 3
	/// </summary>
	[SLua.CustomLuaClassAttribute]
	public class ROUITable : UITable {

		public delegate GameObject OnInitializeItem (int realIndex);
		public delegate void OnRemoveItem(GameObject obj);
		public OnInitializeItem onInitializeItem;
		public OnRemoveItem onRemoveItem;
		public bool	mIsMoveToFirst = false;

		UIScrollView mScroll;
		bool mHorizontal = false;
		List<int> mRealIndex = new List<int> ();	//record the index in panel to realindex
		float mOffsetTable = 0;
		List<Transform> mChildren = new List<Transform> ();

		int mMinIndex = 0;
		int mMaxIndex = 0;

		protected void OnMove (UIPanel panel) { WrapContent(); }

		protected override void Start ()
		{
			base.Start ();

			hideInactive = true; 

			CacheScrollView ();

			mPanel.onClipMove = OnMove;
		}

		protected bool CacheScrollView ()
		{
			mScroll = mPanel.GetComponent<UIScrollView>();
			if (mScroll == null) return false;
			if (mScroll.movement == UIScrollView.Movement.Horizontal) mHorizontal = true;
			else if (mScroll.movement == UIScrollView.Movement.Vertical) mHorizontal = false;
			else return false;
			return true;
		}

		public void WrapContent()
		{
			Vector3[] corners = GetPanelCorners (transform);
			Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);
			List<int> insertList = new List<int> ();
			List<GameObject> removeList = new List<GameObject> ();
			bool isInsertFirst = false;
							
			if (mHorizontal) {

				float extents = mPanel.GetViewSize().x * 0.5f;
				
				if(mChildren.Count >= 1)
				{
					if (direction == Direction.Down)
					{
						Transform child = mChildren[0];
						int realIndexFirst = mRealIndex[0];
						int realIndexEnd = mRealIndex[mChildren.Count - 1];
						float distance = child.localPosition.x - center.x;
						Bounds b = NGUIMath.CalculateRelativeWidgetBounds(child, !hideInactive);
						Vector3[] childCorners = GetPanelCorners (child);
						float border = extents * 4;
						float critical = extents - b.extents.x;
						float max = childCorners[2].x;
						
						if(distance > border)
						{
							removeList.Add(child.gameObject);
							mRealIndex.RemoveAt(0);
							
							Vector3[] lastChildCorners = GetPanelCorners ( mChildren[1] );
							float tempOffset = max - lastChildCorners[2].x;
							mOffsetTable += tempOffset;
						}
						else if(distance < critical)
						{
							if(realIndexFirst - 1 >= mMinIndex)
							{
								mRealIndex.Insert(0 , realIndexFirst - 1 );
								insertList.Add(realIndexFirst - 1);
								isInsertFirst = true;
							}
						}
						
						child = mChildren[mChildren.Count - 1];
						distance = child.localPosition.x - center.x;						
						b = NGUIMath.CalculateRelativeWidgetBounds(child, !hideInactive);
						childCorners = GetPanelCorners (child);
						critical = extents - b.extents.x;
						
						if(distance < -border)
						{
							removeList.Add(child.gameObject);
							mRealIndex.RemoveAt(mChildren.Count - 1);
						}
						else if(distance > -critical)
						{
							if(realIndexEnd + 1 <= mMaxIndex)
							{
								mRealIndex.Add( realIndexEnd + 1 );
								insertList.Add(realIndexEnd + 1);
							}
						}
					}	
				}

				ChangeTable(insertList,removeList);
				
				if (isInsertFirst) {
					Transform child = mChildren[0];
					Vector3[] childCorners = GetPanelCorners (child);
					float max = childCorners[2].y;
					
					Vector3[] lastChildCorners = GetPanelCorners ( mChildren[1] );
					float tempOffset = max - lastChildCorners[2].y;
					mOffsetTable -= tempOffset;
					
					ResetTable();
				}
			}
			else {

				float extents = mPanel.GetViewSize().y * 0.5f;

				if(mChildren.Count >= 1 && mRealIndex.Count >= mChildren.Count)
				{
					if (direction == Direction.Down)
					{
						Transform child = mChildren[0];
						int realIndexFirst = mRealIndex[0];
						int realIndexEnd = mRealIndex[mChildren.Count - 1];
						float distance = child.localPosition.y - center.y;
						Bounds b = NGUIMath.CalculateRelativeWidgetBounds(child, !hideInactive);
						Vector3[] childCorners = GetPanelCorners (child);
						float border = extents * 4;
//						float critical = extents - b.extents.y;
						float critical = extents + b.extents.y;
						float max = childCorners[2].y;

						if(realIndexFirst == mMinIndex)
						{
							if(distance >= extents)
								mIsMoveToFirst = false;
							else
								mIsMoveToFirst = true;
						}

						if(distance > border)
						{
							removeList.Add(child.gameObject);
							mRealIndex.RemoveAt(0);

							Vector3[] lastChildCorners = GetPanelCorners ( mChildren[1] );
							float tempOffset = max - lastChildCorners[2].y;
							mOffsetTable += tempOffset;
						}
						else if(distance < critical)
						{
							if(realIndexFirst - 1 >= mMinIndex)
							{
								mRealIndex.Insert(0 , realIndexFirst - 1 );
								insertList.Add(realIndexFirst - 1);
								isInsertFirst = true;
							}
						}

						child = mChildren[mChildren.Count - 1];
						distance = child.localPosition.y - center.y;						
						b = NGUIMath.CalculateRelativeWidgetBounds(child, !hideInactive);
						childCorners = GetPanelCorners (child);
//						critical = extents - b.extents.y;
						critical = extents + b.extents.y;

						if(distance < -border)
						{
							removeList.Add(child.gameObject);
							if(mRealIndex.Count >= mChildren.Count)
								mRealIndex.RemoveAt(mChildren.Count - 1);
							else
								mRealIndex.RemoveAt(mChildren.Count - 2);
						}
						else if(distance > -critical)
						{
							if(realIndexEnd + 1 <= mMaxIndex)
							{
								mRealIndex.Add( realIndexEnd + 1 );
								insertList.Add(realIndexEnd + 1);
							}
						}
					}
				}

				ChangeTable(insertList,removeList);

				if (isInsertFirst) {
					Transform child = mChildren[0];
					Vector3[] childCorners = GetPanelCorners (child);
					float max = childCorners[2].y;
					
					Vector3[] lastChildCorners = GetPanelCorners ( mChildren[1] );
					float tempOffset = max - lastChildCorners[2].y;
					mOffsetTable -= tempOffset;
					ResetTable();
				}
			}
		}

		public void ChangeTable(List<int> insert,List<GameObject> remove)
		{
			bool isChange = false;

			for (int i = 0; i < insert.Count; ++i) {			
				InitializeItem(insert[i]);
				isChange = true;
			}

			for (int i = 0; i < remove.Count; ++i) {
				RemoveItem(remove[i]);
				isChange = true;
			}
			
			if (isChange) {
				ResetTable();
			}
		}

		public void ResetTable()
		{
			transform.localPosition += new Vector3(0,mOffsetTable,0);
			mOffsetTable = 0.0f;
			Reposition ();
			mChildren.Clear ();
			mChildren = GetChildList ();
		}

		public bool IsTableInPanel()
		{
			Reposition ();

			Bounds b = NGUIMath.CalculateRelativeWidgetBounds(transform, !hideInactive);

			Vector3[] corners = GetPanelCorners (transform);
			
			if (mHorizontal) {
				float min = corners[0].x;
				float max = corners[2].x;

				if (direction == Direction.Down) {
					if(b.min.x < min - padding.x)
						return false;
				} 
				else if (direction == Direction.Up) {
					if(b.max.x > max + padding.x)
						return false;
				}
				
			} else {
				float min = corners[0].y;
				float max = corners[2].y;
				
				if (direction == Direction.Down) {
					if(b.min.y < min - padding.y)
						return false;
				} 
				else if (direction == Direction.Up) {
					if(b.max.y > max + padding.y)
						return false;
				}
			}
			return true;
		}

		protected GameObject InitializeItem (int realIndex)
		{
			GameObject obj = null;

			if (onInitializeItem != null) {
				obj = onInitializeItem (realIndex);
			}

			return obj;
		}

		protected void RemoveItem(GameObject obj)
		{
			if (onRemoveItem != null) {
				onRemoveItem (obj);
			}
		}

		public Vector3[] GetPanelCorners(Transform trans)
		{
			Vector3[] corners = mPanel.worldCorners;
			for (int i = 0; i < 4; ++i)
			{
				Vector3 v = corners[i];
				v = trans.InverseTransformPoint(v);
				corners[i] = v;
			}	

			return corners;
		}

		public void SetShowIndex(int maxIndex)
		{
			mRealIndex.Clear ();

			for (int i = 0; i < transform.childCount; ++i) {
				mRealIndex.Add(i);
			}

			mMinIndex = 0;
			mMaxIndex = maxIndex;

			mChildren.Clear ();
			mChildren = GetChildList();
		}

		public bool IsHorizontal
		{
			get
			{
				return mHorizontal;
			}
		}
	}
}
