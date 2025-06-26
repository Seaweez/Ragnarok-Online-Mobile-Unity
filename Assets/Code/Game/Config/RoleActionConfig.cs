using UnityEngine;
using System.Collections.Generic;
using Ghost.Utils;

namespace RO.Config
{
	[SLua.CustomLuaClassAttribute]
	public static class RoleAction
	{
		public const float DESIGHN_MOVE_SPEED = 3.5f;
		public const float DESIGHN_ROTATION_SPEED = 360f*2;
		public const float DESIGHN_QUICK_ATTACK_SPEED = 3.0f;

		public const string MOUNT_PREFIX = "ride_";

		public const string WAIT = "wait";
		public const string WALK = "walk";
		public const string SIT_DOWN = "sit_down";
		public const string HIT = "hit";
		public const string DIE = "die";
		public const string ATTACK_WAIT = "attack_wait";
		public const string PLAY_SHOW = "playshow";
		public const string ATTACK = "attack";
		public const string QUICK_ATTACK = "quick_attack";
		public const string SKILL_READY = "skill_ready";
		public const string READING = "reading";
		public const string USE_SKILL = "use_skill";
		public const string USE_SKILL_2 = "use_skill2";
		public const string USE_MAGIC = "use_magic";
		public const string VICTORY = "victory";

		#region event
		public const int EVENT_FIRE = 1;
		public const int EVENT_DEAD = 2;
		#endregion event

		private static Dictionary<string, string> mountedAction_ = new Dictionary<string, string>();
		public static string GetMountedAction(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			string mountedName;
			if (!mountedAction_.TryGetValue(name, out mountedName))
			{
				mountedName = StringUtils.ConnectToString(MOUNT_PREFIX, name);
				mountedAction_.Add(name, mountedName);
			}
			return mountedName;
		}
	}

} // namespace RO
