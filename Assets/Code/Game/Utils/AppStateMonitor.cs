using UnityEngine;
using System.Collections;
using RO.Net;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class AppStateMonitor : SingleTonGO<AppStateMonitor>
	{
		public Action applicationQuitHandler;
		public Action<bool> applicationFocusHandler;
		public Action<bool> applicationPauseHandler;

		public static AppStateMonitor Instance {
			get{ return Me;}
		}

		protected override void Awake ()
		{
			base.Awake ();
		}
		
		public GameObject monoGameObject
		{
			get
			{
				return gameObject;
			}
		}

		void OnApplicationFocus (bool isFocus)
		{
			if (applicationFocusHandler != null)
				applicationFocusHandler (isFocus);			
		}

		void OnApplicationPause (bool isPause)
		{
			if (applicationPauseHandler != null)
				applicationPauseHandler (isPause);
		}

		void OnApplicationQuit ()
		{
			if (applicationQuitHandler != null)
				applicationQuitHandler ();
		}
	}
}