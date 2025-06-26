using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;
using RO;

namespace EditorTool
{
	public class RoleActionPlayer : MonoBehaviour
	{
		public Animator animator = null;
		public Transform[] effectPoints = null;
		
		void Start()
		{
			if (effectPoints.IsNullOrEmpty())
			{
				effectPoints = GameObjectHelper.GetPoints(gameObject, "EP_");
			}
			if (null == animator)
			{
				animator = GetComponent<Animator>();
			}
		}
	}
} // namespace EditorTool
