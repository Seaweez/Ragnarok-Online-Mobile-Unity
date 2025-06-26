using UnityEngine;
using RO;
using System.Diagnostics;
using System;

namespace EditorTool {
	/**
	 * call outside command helper (such as shell,bat)
	 */ 
	public class CommandHelper 
	{
		/// <summary>
		/// Excute the specified command and argument.
		/// </summary>
		/// <param name="argument">Argument.</param>
		public static void ExcutePython(string argument)
		{
			UnityEngine.Debug.Log("excute python:"+argument);
			if(Application.platform == RuntimePlatform.WindowsEditor)
			{
				ExcuteExternalCommandNoLog("python",argument);
			}
			else 
			{
				ExcuteExternalCommand("python",argument);
			}

		}
		public static void ExcuteLua(string argument)
		{
			UnityEngine.Debug.Log("excute lua:"+argument);
			if(Application.platform == RuntimePlatform.WindowsEditor)
			{
				ExcuteExternalCommandNoLog("lua",argument);
			}
			else 
			{
				ExcuteExternalCommand("lua",argument);
			}
			
		}


		public static void ExcuteExternalCommand(string command,string argument)
		{
			ProcessStartInfo start = new ProcessStartInfo(command,argument);
			start.CreateNoWindow = true;
			start.UseShellExecute = false;
			start.RedirectStandardOutput = true;
			start.RedirectStandardError = true;
			start.RedirectStandardInput = true;
			start.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
			start.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;	
			Process ps = new Process();
			ps.StartInfo = start;
			ps.Start();
			ps.WaitForExit();
			UnityEngine.Debug.Log(ps.StandardError.ReadToEnd());
			UnityEngine.Debug.Log(ps.StandardOutput.ReadToEnd());	
			ps.Close();
		}

		public static void ExcuteExternalCommand2(string command,string argument)
		{
			ProcessStartInfo start = new ProcessStartInfo(command,argument);
			start.CreateNoWindow = true;
			start.UseShellExecute = false;
			start.RedirectStandardOutput = true;
			start.RedirectStandardError = true;
			start.RedirectStandardInput = true;
			start.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
			start.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;	
			Process ps = Process.Start(start);
			ps.WaitForExit();			
			UnityEngine.Debug.Log(ps.StandardError.ReadToEnd());
			UnityEngine.Debug.Log(ps.StandardOutput.ReadToEnd());
			ps.Close();
		}

		public static void ExcuteExternalCommandNoLog(string command,string argument)
		{
			Process ps = Process.Start(command,argument);
			ps.WaitForExit();
			ps.Close();
		}
	}
}