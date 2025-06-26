using UnityEngine;
using System.Collections.Generic;
using RO;

/// <summary>
/// Attach this script to the container that has the objects to center on as its children.
/// </summary>
[SLua.CustomLuaClassAttribute]
public class MyUICenterOnChild : MonoBehaviour
{
	public delegate void OnCenterCallback (GameObject centeredObject);
	
	/// <summary>
	/// The strength of the spring.
	/// </summary>
	public float springStrength = 8f;
	
	/// <summary>
	/// If set to something above zero, it will be possible to move to the next page after dragging past the specified threshold.
	/// </summary>	
	public float nextPageThreshold = 0f;
	
	/// <summary>
	/// Callback to be triggered when the centering operation completes.
	/// </summary>	
	public SpringPanel.OnFinished onFinished;
	
	/// <summary>
	/// Callback triggered whenever the script begins centering on a new child object.
	/// </summary>
	public OnCenterCallback onCenter;
	
	UIScrollView mScrollView;
	GameObject mCenteredObject;
	
	/// <summary>
	/// Game object that the draggable panel is currently centered on.
	/// </summary>
	public GameObject centeredObject { get { return mCenteredObject; } }
	
	void Awake () 
	{
		mScrollView = NGUITools.FindInParents<UIScrollView>(gameObject);
		if (mScrollView == null)
		{
			RO.LoggerUnused.LogWarning(GetType() + " requires " + typeof(UIScrollView) + " on a parent object in order to work", this);
			enabled = false;
			return;
		}
	}

	/// <summary>
	/// Ensure that the threshold is always positive.
	/// </summary>
	void OnValidate () { nextPageThreshold = Mathf.Abs(nextPageThreshold); }
	
	/// <summary>
	/// Center the panel on the specified target.
	/// </summary>
	void CenterOn (Transform target, Vector3 panelCenter)
	{
		if (target != null && mScrollView != null && mScrollView.panel != null)
		{
			Transform panelTrans = mScrollView.panel.cachedTransform;
			mCenteredObject = target.gameObject;
			
			// Figure out the difference between the chosen child and the panel's center in local coordinates
			Vector3 cp = panelTrans.InverseTransformPoint(target.position);
			Vector3 cc = panelTrans.InverseTransformPoint(panelCenter);
			Vector3 localOffset = cp - cc;
			
			// Offset shouldn't occur if blocked
			if (!mScrollView.canMoveHorizontally) localOffset.x = 0f;
			if (!mScrollView.canMoveVertically) localOffset.y = 0f;
			localOffset.z = 0f;
			
			// Spring the panel to this calculated position
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				panelTrans.localPosition = panelTrans.localPosition - localOffset;
				
				Vector4 co = mScrollView.panel.clipOffset;
				co.x += localOffset.x;
				co.y += localOffset.y;
				mScrollView.panel.clipOffset = co;
			}
			else
				#endif
			{
				SpringPanel.Begin(mScrollView.panel.cachedGameObject,
				                  panelTrans.localPosition - localOffset, springStrength).onFinished = onFinished;
			}
		}
		else mCenteredObject = null;
		
		// Notify the listener
		if (onCenter != null) onCenter(mCenteredObject);
	}
	
	/// <summary>
	/// Center the panel on the specified target.
	/// </summary>
	public void CenterOn (Transform target)
	{
		if (mScrollView != null && mScrollView.panel != null)
		{
			Vector3[] corners = mScrollView.panel.worldCorners;
			Vector3 panelCenter = (corners[2] + corners[0]) * 0.5f;
			CenterOn(target, panelCenter);
		}
	}
}
