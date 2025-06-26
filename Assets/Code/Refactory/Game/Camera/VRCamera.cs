using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class VRCamera : MonoBehaviour 
	{
		public Camera leftCamera;
		public Camera rightCamera;

		public Skybox leftSkybox;
		public Skybox rightSkybox;
	
		public float offset = 0.06f;
		private bool _enable = false;
		public bool enable
		{
			get
			{
				return _enable;
			}
			set
			{
				if (value == enable)
				{
					return;
				}
				_enable = value;
				if (enable)
				{
					if (null != rightCamera)
					{
						rightCamera.gameObject.SetActive(true);
					}
					else
					{
						if (null != leftCamera)
						{
							rightCamera = new GameObject("VR_Camera", typeof(Camera)).GetComponent<Camera>();
							rightCamera.transform.SetParent(leftCamera.transform, false);
							var flareLayer = leftCamera.GetComponent<FlareLayer>();
							if (null != flareLayer && flareLayer.enabled)
							{
								rightCamera.gameObject.AddComponent<FlareLayer>();
							}
						}
					}
					rightCamera.transform.localPosition = new Vector3(offset, 0, 0);
					rightCamera.transform.localRotation = Quaternion.identity;
					rightCamera.transform.localScale = Vector3.one;
					if (null != leftCamera && null != rightCamera)
					{
						var rect = leftCamera.rect;
						rect.width = 0.5f;
						leftCamera.rect = rect;
						
						rect = rightCamera.rect;
						rect.x = 0.5f;
						rect.width = 0.5f;
						rightCamera.rect = rect;
					}
				}
				else
				{
					if (null != rightCamera)
					{
						rightCamera.gameObject.SetActive(false);
					}
					var rect = leftCamera.rect;
					rect.width = 1f;
					leftCamera.rect = rect;
				}
			}
		}

		void LateUpdate()
		{
			if (!enable)
			{
				return;
			}
			if (null != leftCamera && null != rightCamera)
			{
				rightCamera.clearFlags = leftCamera.clearFlags;
				rightCamera.cullingMask = leftCamera.cullingMask;
				rightCamera.fieldOfView = leftCamera.fieldOfView;
				rightCamera.depth = leftCamera.depth;
				rightCamera.nearClipPlane = leftCamera.nearClipPlane;
				rightCamera.farClipPlane = leftCamera.farClipPlane;
				rightCamera.backgroundColor = leftCamera.backgroundColor;
			}

			if (null != leftSkybox && null != rightSkybox)
			{
				rightSkybox.material = leftSkybox.material;
			}
		}
	}
} // namespace RO
