using UnityEngine;
using System.Diagnostics;
using UnityEditor;

namespace EditorTool
{
	public class AutoUpdate
	{
		public AutoUpdate() { }

		[MenuItem("Tools/AutoUpdate")]
		static void StartAutoUpdate()
		{
			Auto();
		}

		static void SvnUpdate()
		{
			EditorUtility.DisplayProgressBar("Svn更新中", "请不要关闭Unity", 1f);
			ExeCommandWindow("svn", "revert . -R");
			ExeCommandWindow("svn", "up --accept tf");
			EditorUtility.ClearProgressBar();
		}

		static void RefreshUnity()
		{
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
		}

		public static void ImmediateUpdate()
		{
			SvnUpdate();
			RefreshUnity();
		}

		static void Auto()
		{
			ImmediateUpdate();
			CountDownWindow.Start(Auto);
		}

		static void ExeCommandWindow(string command,string argument)
		{
			ProcessStartInfo start = new ProcessStartInfo(command,argument);
			start.CreateNoWindow = false;
			start.UseShellExecute = true;
			Process ps = Process.Start (start);
			ps.WaitForExit();
			ps.Close();
		}
	}
}
