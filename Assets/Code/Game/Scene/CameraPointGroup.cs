using UnityEngine;
using System.Collections.Generic;
using Ghost.Attribute;
using Ghost.Extensions;

namespace RO
{
	[System.Serializable]
	public class CameraPointGroupInfo
	{
		public bool enable = true;

		public CameraPoint[] cameraPoints;

		public void ResetValidity()
		{
			foreach (var cp in cameraPoints)
			{
				cp.gameObject.SetActive(enable);
			}
		}
	}

	public class CameraPointGroup : MonoBehaviour
	{
		public CameraPointGroupInfo[] groups;

		public void SetValidity(int index, bool valid)
		{
			if (null == groups || !groups.CheckIndex(index))
			{
				return;
			}
			var g = groups[index];
			g.enable = valid;
			g.ResetValidity();
		}

		public void ResetValidity()
		{
			foreach (var g in groups)
			{
				g.ResetValidity();
			}
		}

		void Awake ()
		{
			ResetValidity();
		}

		void Start()
		{
			if (!(null != CameraPointManager.Me && CameraPointManager.Me.SetGroup(this)))
			{
				GameObject.Destroy(gameObject);
			}
		}
		
		void OnDestroy()
		{
			if (null != CameraPointManager.Me)
			{
				CameraPointManager.Me.ClearGroup(this);
			}
		}

	}
} // namespace RO
