using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[System.Serializable]
	public class CameraPointLinkInfo
	{
		public float angle = 0;

		public CameraPoint cp1 = null;
		public CameraPoint cp2 = null;
		public Vector3 expand = Vector3.zero;
		public bool weightOnX = false;
		public bool weightOnZ = false;

		public int priority = 0;
		
		public CameraPoint tempCP{get;private set;}
		
		public bool valid
		{
			get
			{
				return null != cp1 && !cp1.invalidate 
					&& null != cp2 && !cp2.invalidate 
					&& (weightOnX || weightOnZ);
			}
		}
		
		private void InitTempCP(Transform parent)
		{
			if (null == tempCP)
			{
//				Vector3 left,right,leftEnd,rightEnd;
//				RolePicker.GetRect(cp1.position, cp2.position, Quaternion.Euler(0,angle,0), out left,out right,out leftEnd,out rightEnd);
				
				tempCP = new GameObject("cp_link").AddComponent<CameraPoint>();
				tempCP.transform.parent = parent;
				tempCP.transform.position = (cp1.position+cp2.position) / 2;
				tempCP.transform.eulerAngles = new Vector3(0,angle,0);
				
				tempCP.type = AreaTrigger.Type.RECTANGE;
				tempCP.size = new Vector2(Mathf.Abs(cp1.position.x-cp2.position.x)+expand.x, Mathf.Abs(cp1.position.z-cp2.position.z)+expand.z);
				
				tempCP.info = new CameraController.Info();
				
				tempCP.focusViewPortValid = cp1.focusViewPortValid || cp2.focusViewPortValid;
				tempCP.rotationValid = cp1.rotationValid || cp2.rotationValid;

				tempCP.priority = priority;

				tempCP.forLink = true;
			}
		}
		
		public bool Check(Transform t, Transform parent = null)
		{
			if (!valid)
			{
				return false;
			}
			InitTempCP(parent);
			return tempCP.Check(t);
		}
		
		public void UpdateTempCameraPoint(CameraController.Info originalDefaultInfo, Transform t, Transform parent = null)
		{
			InitTempCP(parent);
			
			float lerpT = 0.5f;
			if (weightOnX && weightOnZ)
			{
				var d1 = Vector2.Distance(t.position.XZ (), cp1.position.XZ());
				var d2 = Vector2.Distance(t.position.XZ (), cp2.position.XZ());
				lerpT = d1/(d1+d2);
			}
			else if (weightOnX)
			{
				if (0 == angle)
				{
					lerpT = Mathf.Abs(t.position.x-cp1.position.x) / Mathf.Abs(cp2.position.x-cp1.position.x);
				}
				else
				{
					float d1;
					if (tempCP.TryGetDistanceInRectX1(t.position, out d1))
					{
						lerpT = d1 / tempCP.size.x;
					}
				}
			}
			else if (weightOnZ)
			{
				if (0 == angle)
				{
					lerpT = Mathf.Abs(t.position.z-cp1.position.z) / Mathf.Abs(cp2.position.z-cp1.position.z);
				}
				else
				{
					float d1;
					if (tempCP.TryGetDistanceInRectZ1(t.position, out d1))
					{
						lerpT = d1 / tempCP.size.y;
					}
				}
			}

			var i1 = cp1.info;
			var i2 = cp2.info;
			if (!(cp1.focusViewPortValid && cp1.rotationValid))
			{
				i1 = i1.CloneSelf();
				if (!cp1.focusViewPortValid)
				{
					i1.focus = originalDefaultInfo.focus;
					i1.focusOffset = originalDefaultInfo.focusOffset;
					i1.focusViewPort = originalDefaultInfo.focusViewPort;
				}
				if (!cp1.rotationValid)
				{
					i1.rotation = originalDefaultInfo.rotation;
				}
			}
			if (!(cp2.focusViewPortValid && cp2.rotationValid))
			{
				i2 = i2.CloneSelf();
				if (!cp2.focusViewPortValid)
				{
					i2.focus = originalDefaultInfo.focus;
					i2.focusOffset = originalDefaultInfo.focusOffset;
					i2.focusViewPort = originalDefaultInfo.focusViewPort;
				}
				if (!cp2.rotationValid)
				{
					i2.rotation = originalDefaultInfo.rotation;
				}
			}

			CameraController.Info.Lerp(i1, i2, tempCP.info, lerpT);
		}
	}
} // namespace RO
