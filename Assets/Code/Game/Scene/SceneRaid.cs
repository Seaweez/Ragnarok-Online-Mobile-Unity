using UnityEngine;
using System.Collections.Generic;
using Ghost.Attribute;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class SceneRaid : MonoBehaviour 
	{
		public const int INVALID_ID = -1;

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
				if (0 > value)
				{
					return;
				}
				ID_ = value;
				RefreshName();
			}
		}

		[HideInInspector]
		public bool cameraInfoEnable = false;
		[HideInInspector]
		public CameraController.Info cameraInfo = null;

		private void RefreshName()
		{
			gameObject.name = string.Format("Raid_{0}", ID);
		}

		public List<long> GetInvalidNPCUniqueIDs()
		{
			var nps = GetComponentsInChildren<RaidNPCPoint>();
			if (!nps.IsNullOrEmpty())
			{
				var UniqueIDs = new List<long>();

				foreach (var np in nps)
				{
					UniqueIDs.Add(np.UniqueID);
				}

				return UniqueIDs.ToNotUnique();
			}

			return null;
		}

		void Reset()
		{
			RefreshName();
		}

		void Awake()
		{
#if OBSOLETE
			var player = Player.Me;
			if (ID != player.activeRaidID)
			{
				GameObject.DestroyImmediate(gameObject);
			}
#endif
		}
	}
} // namespace RO
