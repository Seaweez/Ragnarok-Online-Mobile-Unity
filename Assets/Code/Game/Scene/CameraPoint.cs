using UnityEngine;
using System.Collections.Generic;
using Ghost.Utils;
using Ghost.Extensions;
using Ghost.Attribute;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CameraPoint : AreaTrigger 
	{
		[SerializeField, SetProperty("ID")]
		private int ID_ = 0;
		public int ID
		{
			get
			{
				return ID_;
			}
			set
			{
				ID_ = value;
				RefreshName();
			}
		}
		public bool IDValid
		{
			get
			{
				return 0 != ID;
			}
		}

		public CameraController.Info info;
		public float duration = 1f;

		public bool focusViewPortValid = false;
		public bool rotationValid = false;

		public bool surround = false;

		public int priority = 0;

		public bool forLink{get;set;}

		protected void RefreshName()
		{
			if (0 != ID)
			{
				gameObject.name = string.Format("cp_{0}", ID);
			}
		}
		
		void Reset()
		{
			RefreshName();
		}

		void Start()
		{
			if (!(null != CameraPointManager.Me && (forLink || CameraPointManager.Me.Add(this))))
			{
				GameObject.Destroy(gameObject);
			}
		}

		void OnDestroy()
		{
			if (null != CameraPointManager.Me)
			{
				CameraPointManager.Me.Remove(this);
			}
		}

	}

} // namespace RO
