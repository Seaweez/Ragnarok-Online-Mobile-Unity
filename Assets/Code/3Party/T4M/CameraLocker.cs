using UnityEngine;
using System.Collections.Generic;
using Ghost.Attribute;
using Ghost.Extensions;

public class CameraLocker : MonoBehaviour {

	private Transform follow = null;
	private Vector3 followOffset = Vector3.zero;
	private Transform selfTransform = null;

	[SerializeField, SetProperty("focus")]
	private Transform focus_ = null;
	public Transform focus
	{
		get
		{
			return focus_;
		}
		set
		{
			focus_ = value;
			follow = focus;
			AdjustForFocus();
		}
	}
	
	[SerializeField, SetProperty("focusViewPort")]
	private Vector3 focusViewPort_ = new Vector3(0.5f, 0.5f, 16.0f);
	public Vector3 focusViewPort
	{
		get
		{
			return focusViewPort_;
		}
		set
		{
			focusViewPort_ = value;
			AdjustForFocus();
		}
	}
	
	[SerializeField, SetProperty("zoomMin")]
	private float zoomMin_ = 0.7f;
	public float zoomMin
	{
		get
		{
			return zoomMin_;
		}
		set
		{
			zoomMin_ = value;
			if (0 > zoomMin_)
			{
				zoomMin_ = 0;
			}
			else if (zoomMax < zoomMin_)
			{
				zoomMin_ = zoomMax;
			}
		}
	}
	[SerializeField, SetProperty("zoomMax")]
	private float zoomMax_ = 2.3f;
	public float zoomMax
	{
		get
		{
			return zoomMax_;
		}
		set
		{
			zoomMax_ = value;
			if (5 < zoomMax_)
			{
				zoomMax_ = 0;
			}
			else if (zoomMin > zoomMax_)
			{
				zoomMax_ = zoomMin;
			}
		}
	}
	
	[SerializeField, SetProperty("zoom")]
	private float zoom_ = 1.0f;
	public float zoom
	{
		get
		{
			return zoom_;
		}
		set
		{
			zoom_ = Mathf.Clamp(value, zoomMinEx, zoomMaxEx);
			AdjustForFocus();
		}
	}
	
	[SerializeField, SetProperty("zoomMinEx")]
	private float zoomMinEx_;
	public float zoomMinEx
	{
		get
		{
			return zoomMinEx_;
		}
		set
		{
			zoomMinEx_ = Mathf.Clamp(value, 0, zoomMin);
		}
	}
	[SerializeField, SetProperty("zoomMaxEx")]
	private float zoomMaxEx_;
	public float zoomMaxEx
	{
		get
		{
			return zoomMaxEx_;
		}
		set
		{
			zoomMaxEx_ = value;
			if (zoomMaxEx_ < zoomMax)
			{
				zoomMaxEx_ = zoomMax;
			}
		}
	}
	public float adjustZoomDuration = 0.5f;
	private float adjustZoomVelocity = 0f;
	[SerializeField, SetProperty("autoAdjustZoom")]
	private bool autoAdjustZoom_ = false;
	public bool autoAdjustZoom
	{
		get
		{
			return autoAdjustZoom_;
		}
		set
		{
			autoAdjustZoom_ = value;
			if (!autoAdjustZoom)
			{
				adjustZoomVelocity = 0f;
			}
		}
	}
	private void AdjustZoom(float targetZoom)
	{
		zoom = Mathf.SmoothDamp(zoom, targetZoom, ref adjustZoomVelocity, adjustZoomDuration);
		if (Mathf.Abs(targetZoom-zoom) < 0.001)
		{
			zoom = targetZoom;
		}
	}

	private Camera ctrlCamera{get;set;}
	
	public CameraLocker()
	{
		zoomMinEx_ = zoomMin;
		zoomMaxEx_ = zoomMax;
	}
	
	private void AdjustForFocus()
	{
		if (null != ctrlCamera && null != focus)
		{
			var vp = focusViewPort;
			if (0 < zoom)
			{
				var scale = 1.0f / zoom;
				vp.z *= scale;
			}
			var from = ctrlCamera.ViewportToWorldPoint(vp);
			var to = focus.position;
			var newPosition = ctrlCamera.transform.position + (to-from);
			followOffset = newPosition-to;
		}
	}
	
	// behavior
	void Awake()
	{
		if (null == ctrlCamera)
		{
			ctrlCamera = GetComponent<Camera>();
		}
		selfTransform = transform;
		follow = focus;
		AdjustForFocus();
	}
	
	void LateUpdate()
	{
		if (null != follow)
		{
			selfTransform.position = follow.position + followOffset;
		}
		if (autoAdjustZoom)
		{
			if (zoomMin > zoom)
			{
				AdjustZoom(zoomMin);
			}
			else if (zoomMax < zoom)
			{
				AdjustZoom(zoomMax);
			}
		}
	}
}
