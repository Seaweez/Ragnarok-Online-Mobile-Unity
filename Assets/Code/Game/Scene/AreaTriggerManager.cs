using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class AreaTriggerManager<T, AT> : SingleTonGO<T> where AT:AreaTrigger where T : SingleTonGO<T>, new()
	{
		protected AT currentTrigger = null;

		protected List<AT> triggers = new List<AT>();

		public bool Add(AT at)
		{
			if (!DoAdd(at))
			{
				return false;
			}
			triggers.Add(at);
//			at.transform.parent = transform;
			return true;
		}
		
		public void Remove(AT at)
		{
			DoRemove(at);
			if (triggers.Remove(at))
			{
//				at.transform.parent = null;
			}
		}

		protected virtual bool DoAdd(AT at)
		{
			return !triggers.Contains(at);
		}
		
		protected virtual void DoRemove(AT at)
		{
		}

		protected virtual void OnTriggerChanged(AT oldTrigger, AT newTrigger)
		{

		}

		protected virtual AT DoCheck(Transform t)
		{
			for (int i = 0; i < triggers.Count; ++i)
			{
				var at = triggers[i];
				if (at.Check(t))
				{
					return at;
				}
			}
			return null;
		}

		protected void SetTrigger(AT newTrigger, Transform t)
		{
			var oldTrigger = currentTrigger;
			if (oldTrigger == newTrigger)
			{
				return;
			}

			currentTrigger = newTrigger;

			if (null != oldTrigger)
			{
#if DEBUG_DRAW
				oldTrigger.ddPlayerIn = false;
#endif // DEBUG_DRAW
				oldTrigger.OnRoleExit(t);
			}

			if (null != newTrigger)
			{
#if DEBUG_DRAW
				newTrigger.ddPlayerIn = true;
#endif // DEBUG_DRAW
				newTrigger.OnRoleEnter(t);
			}

			OnTriggerChanged(oldTrigger, newTrigger);
		}
		
		private void Check()
		{
			var config = LuaLuancher.Me;
			if (null == config)
			{
				SetTrigger(null, null);
				return;
			}
			
			if (config.ignoreAreaTrigger)
			{
				SetTrigger(null, null);
				return;
			}

			var myself = config.myself;
			if (null == myself)
			{
				SetTrigger(null, null);
				return;
			}

			SetTrigger(DoCheck(myself.transform), myself.transform);
		}

		protected virtual void LateUpdate()
		{
			Check();
		}
	
	}
} // namespace RO
