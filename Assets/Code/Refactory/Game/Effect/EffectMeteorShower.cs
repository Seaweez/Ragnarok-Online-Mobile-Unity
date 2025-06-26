using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public class EffectMeteorShower : EffectLogic 
	{
		public ResourceID resID;
		public MeteorShower prefab;

		public int count = 0;
		public float intervalMin = 0.3f;
		public float intervalMax = 0.3f;
		public Vector2 startPositionMin;
		public Vector2 startPositionMax;
		public Vector2 endPositionMin;
		public Vector2 endPositionMax;
		public float durationMin = 1f;
		public float durationMax = 1f;

		public Vector2 speedScaleFactor = new Vector2(0.1f, 3f);

		private List<MeteorShower> meteorShowers = new List<MeteorShower>();
		private int restCount = 0;
		private float nextTime = 0;

		private MeteorShower NewMeteorShower()
		{
			MeteorShower ms = null;
			if (null != resID)
			{
				var go = GameObjPool.Me.RGet(resID, Config.Pool.NAME_DEFAULT);
				if (null == go)
				{
					return null;
				}
				ms = go.GetComponent<MeteorShower>();
				if (null == ms)
				{
					GameObjPool.Me.RAdd (go, resID, Config.Pool.NAME_DEFAULT);
					return null;
				}
			}
			else if (null != prefab)
			{
				var go = GameObjPool.Me.Get(prefab.name, Config.Pool.NAME_DEFAULT); 
				if (null != go)
				{
					ms = go.GetComponent<MeteorShower>();
					if (null == ms)
					{
						GameObjPool.Me.Add (go, prefab.name, Config.Pool.NAME_DEFAULT);
						return null;
					}
				}
				else
				{
					ms = GameObject.Instantiate<MeteorShower>(prefab);
					if (null != ms)
					{
						ms.name = prefab.name;
					}
				}
			}
			if (null != ms)
			{
				ms.transform.ResetParent(transform);
			}
			return ms;
		}

		private void DeleteMeteorShower(MeteorShower ms)
		{
			if (null != GameObjPool.Me)
			{
				if (null != resID)
				{
					GameObjPool.Me.RAdd (ms.gameObject, resID, Config.Pool.NAME_DEFAULT);
					return;
				}
				else if (null != prefab)
				{
					GameObjPool.Me.Add (ms.gameObject, prefab.name, Config.Pool.NAME_DEFAULT);
					return;
				}
			}
			GameObject.Destroy(ms.gameObject);
		}

		private void Clear()
		{
			foreach (var ms in meteorShowers)
			{
				DeleteMeteorShower(ms);
			}
			meteorShowers.Clear();
		}

		private void TryNew()
		{
			if (0 < restCount && Time.time >= nextTime)
			{
				var ms = NewMeteorShower();
				if (null == ms)
				{
					return;
				}
				
				var duration = Random.Range(durationMin, durationMax);
				if (0 < duration)
				{
					var startPosition = startPositionMin + Random.insideUnitCircle.Multiply(startPositionMax-startPositionMin);
					var endPosition = endPositionMin + Random.insideUnitCircle.Multiply(endPositionMax-endPositionMin);
					
					var averageSpeed = (endPosition-startPosition) / duration;

					var startSpeed = (2*averageSpeed).Divide(Vector2.one+speedScaleFactor);
					var endSpeed = startSpeed.Multiply(speedScaleFactor);
					var acceleration = (endSpeed-startSpeed) / duration;
					
					ms.startPosition = startPosition;
					ms.startSpeed = startSpeed;
					ms.acceleration = acceleration;
					ms.duration = duration;

					ms.Launch();
					meteorShowers.Add (ms);
				}

				--restCount;
				nextTime = Time.time + Random.Range(intervalMin, intervalMax);
			}
		}

		protected override void Replay ()
		{
			base.Replay ();

			if (0 >= count)
			{
				return;
			}

			restCount = count;
			nextTime = Time.time;

			running = true;
		}

		void LateUpdate()
		{
			if (!running)
			{
				return;
			}

			TryNew();

			int msCount = meteorShowers.Count;
			for (int i = msCount-1; i >= 0; --i)
			{
				var ms = meteorShowers[i];
				if (!ms.running)
				{
					DeleteMeteorShower(ms);
					meteorShowers.RemoveAt(i);
				}
			}
			if (0 >= restCount && 0 >= meteorShowers.Count)
			{
				running = false;
			}
		}

		void OnDisable()
		{
			Clear();
		}
	}
} // namespace RO
