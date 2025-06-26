using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Ghost.Utility;
using RO;

namespace Ghost
{
	public class ThreadMonitor : SingleTonGO<ThreadMonitor>
	{
		public float abortDelay = -1;

		public HashSet<Thread> activeThreads{get;private set;}
		public List<KeyValuePair<Thread, float>> abortThreads{get;private set;}

		public bool allowAbortDelay
		{
			get
			{
				return 0 <= abortDelay;
			}
		}

		public void Monit(Thread t)
		{
			activeThreads.Add(t);
		}

		public bool DelayAbort(Thread t)
		{
			if (!allowAbortDelay)
			{
				return false;
			}
			if (!activeThreads.Remove(t))
			{
				return false;
			}
			abortThreads.Add(new KeyValuePair<Thread, float>(t, Time.fixedTime+abortDelay));
			return true;
		}

		#region behaviour
		void Start()
		{
			activeThreads = new HashSet<Thread>();
			abortThreads = new List<KeyValuePair<Thread, float>>();
		}

		private List<KeyValuePair<Thread, float>> temp_inactiveThreas = new List<KeyValuePair<Thread, float>>();
		void FixedUpdate()
		{
			var fixedTime = Time.fixedTime;
			var inactiveThreads = temp_inactiveThreas;
			inactiveThreads.Clear();
			foreach (var t in activeThreads)
			{
				if (!t.IsAlive)
				{
					inactiveThreads.Add(new KeyValuePair<Thread, float>(t, fixedTime+abortDelay));
				}
			}

			if (0 < inactiveThreads.Count)
			{
				foreach (var key_value in inactiveThreads)
				{
					activeThreads.Remove(key_value.Key);
				}

				if (allowAbortDelay)
				{
					abortThreads.AddRange(inactiveThreads);
				}
			}

			if (0 < abortThreads.Count)
			{
				for (int i = abortThreads.Count-1; i >= 0; --i)
				{
					var key_value = abortThreads[i];
					if (fixedTime >= key_value.Value)
					{
						key_value.Key.Abort();
						abortThreads.RemoveAt(i);
					}
				}
			}
		}
		#endregion behaviour
	}
} // namespace Ghost
