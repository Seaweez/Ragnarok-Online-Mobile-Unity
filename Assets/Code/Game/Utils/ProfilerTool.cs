using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class ProfilerInfo 
	{
		public string tag;
		public uint callTimes = 0;
		public long monoUsedTotal = 0;
		public long monoUsedMax = 0;
		public long monoUsedMin = uint.MaxValue;
	}

	[SLua.CustomLuaClassAttribute]
	public static class ProfilerTool 
	{
		public static bool enable = false;

		public static List<ProfilerInfo> infoList{get;private set;}
		public static Dictionary<string, ProfilerInfo> infoMap{get;private set;}

		public static ProfilerInfo currentInfo
		{
			get
			{
				return 0 < sampleInfos.Count ? sampleInfos.Peek().pInfo : null;
			}
		}

		public struct SampleInfo
		{
			public ProfilerInfo pInfo;
			public long monoUsedBegin;
		}
		private static Stack<SampleInfo> sampleInfos = new Stack<SampleInfo>();

		static ProfilerTool()
		{
			infoList = new List<ProfilerInfo>();
			infoMap = new Dictionary<string, ProfilerInfo>();
		}

		public static void BeginSample(string tag, Object obj = null)
		{
			if (!enable || string.IsNullOrEmpty(tag))
			{
				return;
			}
			ProfilerInfo info;
			if (!infoMap.TryGetValue(tag, out info))
			{
				info = new ProfilerInfo();
				info.tag = tag;
				infoList.Add(info);
				infoMap.Add(tag, info);
			}

			++info.callTimes;
			var sInfo = new SampleInfo();
			sInfo.pInfo = info;
			sInfo.monoUsedBegin = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong();
			sampleInfos.Push(sInfo);

			if (null != obj)
			{
				UnityEngine.Profiling.Profiler.BeginSample(tag, obj);
			}
			else
			{
				UnityEngine.Profiling.Profiler.BeginSample(tag);
			}
		}
	
		public static void EndSample()
		{
			if (!enable)
			{
				return;
			}
			if (0 >= sampleInfos.Count)
			{
				return;
			}
			UnityEngine.Profiling.Profiler.EndSample();

			var smapleInfo = sampleInfos.Pop();
			var monoUsedEnd = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong();
			if (smapleInfo.monoUsedBegin < monoUsedEnd)
			{
				var currentMonoUsed = monoUsedEnd-smapleInfo.monoUsedBegin;
				var pInfo = smapleInfo.pInfo;
				pInfo.monoUsedTotal += currentMonoUsed;
				if (currentMonoUsed > pInfo.monoUsedMax)
				{
					pInfo.monoUsedMax = currentMonoUsed;
				}
				if (currentMonoUsed < pInfo.monoUsedMin)
				{
					pInfo.monoUsedMin = currentMonoUsed;
				}
			}
		}
	}
} // namespace RO
