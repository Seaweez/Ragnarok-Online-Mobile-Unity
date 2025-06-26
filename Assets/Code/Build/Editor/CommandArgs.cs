using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace EditorTool
{
	public class CommandArgs
	{
		public static List<string>GetCommandArgs ()
		{
			string[] args = System.Environment.GetCommandLineArgs ();
			List<string> rst = new List<string> ();
			string str = null;
			for (int i=0; i<args.Length; i++) {
				str = args [i];
				if (!str.StartsWith ("-") && i>=4) {
					if (!str.Contains (".") || i >= 5)
						rst.Add (str);
				}
			}
			return rst;
		}
	
	}
} // namespace EditorTool
