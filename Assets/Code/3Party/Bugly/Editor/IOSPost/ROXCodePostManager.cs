using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace UnityEditor.XCodeEditor
{
	public interface IROXCodePostProcess
	{
		void Process ();

		void Release ();
	}

	public class ROXCodePostManager
	{
		XCProject _project;
		string _builtPath;
		List<IROXCodePostProcess> _processes;

		public ROXCodePostManager (string builtPath)
		{
			//得到xcode工程的路径
			_builtPath = Path.GetFullPath (builtPath);

			// Create a new project object from build target
			_project = new XCProject (_builtPath);

			_processes = new List<IROXCodePostProcess> () {
				new RORequiredXCodePost (_project),
				new ROOptionalXCodePost (_project)
			};
		}

		public void ProcessAll ()
		{
			IROXCodePostProcess p;
			for (int i = 0; i < _processes.Count; i++) {
				p = _processes [i];
				Debug.LogFormat ("<color=yellow>Process:{0} start</color>",p.GetType().ToString());
				p.Process ();
				p.Release ();
			}
			_project.Save ();
		}
	}
}