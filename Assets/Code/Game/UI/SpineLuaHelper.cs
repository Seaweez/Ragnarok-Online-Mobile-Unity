using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class SpineLuaHelper
	{
		static public void PlayDefalutAnim (SkeletonAnimation s, Action end)
		{
			PlayAnim (s, s.AnimationName, end);
		}

		static public void PlayAnim (SkeletonAnimation s, string animname, Action end)
		{
			Spine.TrackEntry t = s.state.SetAnimation (0, animname, s.loop);
			if (end != null)
				t.End += (a,b) => end ();
		}

	}
} // namespace RO
