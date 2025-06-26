using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class RoleCompleteHelper : MonoBehaviour 
	{
		public RoleComplete role;
		public int body = 0;
		public int hair = 0;
		public int leftWeapon = 0;
		public int rightWeapon = 0;
		public int head = 0;
		public int wing = 0;
		public int face = 0;
		public int tail = 0;
		public int eye = 0;
		public int mouth = 0;
		public int mount = 0;

		private int body_ = 0;
		private int hair_ = 0;
		private int leftWeapon_ = 0;
		private int rightWeapon_ = 0;
		private int head_ = 0;
		private int wing_ = 0;
		private int face_ = 0;
		private int tail_ = 0;
		private int eye_ = 0;
		private int mouth_ = 0;
		private int mount_ = 0;

		#region behaviour
		void Reset()
		{
			role = GetComponent<RoleComplete>();
		}

		void Awake()
		{
			if (null == role)
			{
				role = GetComponent<RoleComplete>();
			}
		}

		void Start()
		{
			Apply();
		}
		#endregion behaviour

		public void Apply()
		{
			TryChangePart(body, ref body_, 0, "Body");
			TryChangePart(hair, ref hair_, 1, "Hair");
			TryChangePart(leftWeapon, ref leftWeapon_, 2, "Weapon");
			TryChangePart(rightWeapon, ref rightWeapon_, 3, "Weapon");
			TryChangePart(head, ref head_, 4, "Head");
			TryChangePart(wing, ref wing_, 5, "Wing");
			TryChangePart(face, ref face_, 6, "Face");
			TryChangePart(tail, ref tail_, 7, "Tail");
			TryChangePart(eye, ref eye_, 8, "Eye");
			TryChangePart(mouth, ref mouth_, 9, "Mouth");
			TryChangePart(mount, ref mount_, 10, "Mount");
		}

		private void TryChangePart(int part, ref int part_, int i, string folder)
		{
			if (part == part_)
			{
				return;
			}
			part_ = part;

			var oldPart = role.parts[i];
			if (null != oldPart)
			{
				GameObject.Destroy(oldPart.gameObject);
			}

			var path = string.Format("Role/{0}/{1}", folder, part);
			var prefab = Resources.Load<RolePart>(path);
			if (null != prefab)
			{
				var newPart = Object.Instantiate<RolePart>(prefab);
				newPart.name = prefab.name;
				role.SetPart(i, newPart, true);
			}
		}
	}
} // namespace RO
