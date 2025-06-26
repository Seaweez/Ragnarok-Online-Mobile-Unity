using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class RODebugInfo : SingleTonGO<RODebugInfo>
	{
		//      bool _Enable;
		private float f_Fps;
		private int i_Frames = 0;
		private float f_LastInterval;
		public  float f_UpdateInterval = 0.5f;
		public bool profilerToolEnable = false;

		private float upBps = 0;
		private float downBps = 0;
		private double lastUped = 0;
		private double lastDowned = 0;
		private float lastRefreshTime = 0;
		float luaVMRamTimeStamp = 0;
		int luaVM;
		int deltaLuaVM;
		const int luaVMsampleTime = 1;

		Rect[] rects;
		GUIStyle _style = new GUIStyle ();

		public static RODebugInfo Instance {
			get {
				return Me;
			}
		}

		public GameObject monoGameObject {
			get {
				return gameObject;
			}
		}

		protected override void Awake ()
		{
			rects = new Rect[] {
				new Rect (5, 5, Screen.width, Screen.height),
				new Rect (5, 20, Screen.width, Screen.height),
				new Rect (5, 35, Screen.width, Screen.height),
				new Rect (5, 50, Screen.width, Screen.height),
				new Rect (5, 65, Screen.width, Screen.height),
				new Rect (5, 80, Screen.width, Screen.height),
				new Rect (5, 95, Screen.width, Screen.height),
				new Rect (5, 110, Screen.width, Screen.height),
				new Rect (5, 125, Screen.width, Screen.height),
				new Rect (5, 140, Screen.width, Screen.height),
				new Rect (5, 155, Screen.width, Screen.height),
				new Rect (5, 170, Screen.width, Screen.height),
				new Rect (5, 185, Screen.width, Screen.height),
			};
			textColor = Color.green;
			enabled = false;
			base.Awake ();
		}

		public Color textColor {
			set {
				_style.normal.textColor = value;
			}
			get {
				return _style.normal.textColor;
			}
		}

		public bool Enable {
			set{ enabled = value; }
			get{ return enabled; }
		}

		#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
		public static bool GMEnable = true;
		#else
        public static bool GMEnable = false;
        #endif

		void OnGUI ()
		{
			float passedTime = Time.realtimeSinceStartup - luaVMRamTimeStamp;
			if (passedTime >= luaVMsampleTime) {
				deltaLuaVM = (MyLuaSrv.Instance.LuaStateMemory - luaVM) / ((int)passedTime);
				luaVM = MyLuaSrv.Instance.LuaStateMemory;
				luaVMRamTimeStamp += (int)passedTime;
			}
			DrawLabel (rects [0], "当前运行设备 :" + SystemInfo.deviceModel.ToString () + string.Format (" ({0}x{1})", LuaLuancher.originalScreenWidth, LuaLuancher.originalScreenHeight));
			DrawLabel (rects [1], "当前运行系统 :" + Application.platform.ToString ());
			DrawLabel (rects [2], "当前luaVM内存 :" + MyLuaSrv.Instance.LuaStateMemory.ToString () + "kb ,新增k/s:" + deltaLuaVM);
			DrawLabel (rects [3], "当前帧数Fps : " + f_Fps.ToString ("f2") + "/" + Application.targetFrameRate.ToString("f2"));

			if (null != Net.NetConnectionManager.Me) {
				DrawLabel (rects [4], string.Format ("上行速度 : {0}/s", FormatSize (upBps)));
				DrawLabel (rects [5], string.Format ("下行速度 : {0}/s", FormatSize (downBps)));
				DrawLabel (rects [6], string.Format ("上行总量 : {0}", FormatSize (Net.NetConnectionManager.Me.uploadByteLength)));
				DrawLabel (rects [7], string.Format ("下行总量 : {0}", FormatSize (Net.NetConnectionManager.Me.downloadbyteLength)));
			}

			DrawLabel (rects [8], string.Format ("MonoHeapSize : {0}", FormatSize (UnityEngine.Profiling.Profiler.GetMonoHeapSize ())));
			DrawLabel (rects [9], string.Format ("MonoUsedSize : {0}", FormatSize (UnityEngine.Profiling.Profiler.GetMonoUsedSize ())));
			DrawLabel (rects [10], string.Format ("TotalAllocatedMemory : {0}", FormatSize (UnityEngine.Profiling.Profiler.GetTotalAllocatedMemory ())));
			DrawLabel (rects [11], string.Format ("TotalReservedMemory : {0}", FormatSize (UnityEngine.Profiling.Profiler.GetTotalReservedMemory ())));
			DrawLabel (rects [12], string.Format ("TotalUnusedReservedMemory : {0}", FormatSize (UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemory ())));

			if (profilerToolEnable) {
				if (0 < ProfilerTool.infoList.Count) {
					var rect = rects [12];
					for (int i = 0; i < ProfilerTool.infoList.Count; ++i) {
						var profilerInfo = ProfilerTool.infoList [i];
						rect.y += 15;
						DrawLabel (rect, string.Format ("{0}: callTimes={1}, monoUsedTotal={2}, monoUsedMin={3}, monoUsedMax={4}", profilerInfo.tag, profilerInfo.callTimes, FormatSize (profilerInfo.monoUsedTotal), FormatSize (profilerInfo.monoUsedMin), FormatSize (profilerInfo.monoUsedMax)));
					}
				}
			}
		}

		void DrawLabel (Rect rect, string text)
		{
			GUI.Label (rect, text, _style);
		}

		void Start ()
		{
			if (null != Net.NetConnectionManager.Me) {
				RefreshNetInfo_2 ();
			}
		}

		void Update ()
		{
			DisplayFps ();
			DisplayNetInfo ();
		}

		void DisplayFps ()
		{
			++i_Frames;
			if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval) {
				f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

				i_Frames = 0;

				f_LastInterval = Time.realtimeSinceStartup;
			}
		}

		private static string FormatSize (double size)
		{
			if (size < 1024) {
				return string.Format ("{0}B", size);
			} else if (size < 1024 * 1024) {
				return string.Format ("{0:F2}KB", size / 1024);
			} else if (size < 1024 * 1024 * 1024) {
				return string.Format ("{0:F3}MB", size / (1024 * 1024));
			}
			return string.Format ("{0:F4}GB", size / (1024 * 1024 * 1024));
		}

		private void RefreshNetInfo_1 (float pastTime)
		{
			var uped = Net.NetConnectionManager.Me.uploadByteLength - lastUped;
			upBps = (float)(uped / pastTime);

			var downed = Net.NetConnectionManager.Me.downloadbyteLength - lastDowned;
			downBps = (float)(downed / pastTime);
		}

		private void RefreshNetInfo_2 ()
		{
			lastRefreshTime = Time.time;
			lastUped = Net.NetConnectionManager.Me.uploadByteLength;
			lastDowned = Net.NetConnectionManager.Me.downloadbyteLength;
		}

		private void DisplayNetInfo ()
		{
			if (null != Net.NetConnectionManager.Me) {
				var pastTime = Time.time - lastRefreshTime;
				if (1 <= pastTime) {
					RefreshNetInfo_1 (pastTime);
					RefreshNetInfo_2 ();
				}
			}
		}
	}
}
 // namespace RO
