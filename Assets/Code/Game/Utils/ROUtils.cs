using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public struct DialogOption
	{
		public int id;
		public string text;
	}

	[SLua.CustomLuaClassAttribute]
	public static class ROUtils
	{
		public static bool IsAtRightOnScreen (Vector3 worldP1, Vector3 worldP2)
		{
			if (Camera.main != null) {
				var p1 = Camera.main.WorldToScreenPoint (worldP1);
				var p2 = Camera.main.WorldToScreenPoint (worldP2);
				return p2.x > p1.x;
			}
			return false;
		}

		public static DialogOption[] AnalyzeDialogOptionConfig (string optionStr)
		{
			List<DialogOption> result = new List<DialogOption> ();
			Regex reg = new Regex (@"\{[^\{\}]+\}");
			MatchCollection m1 = reg.Matches (optionStr);
			for (int i = 0; i<m1.Count; i++) {
				Regex reg2 = new Regex (@"\,\d+\}");
				string textStr = reg2.Split (m1 [i].Value) [0];
				string idStr = reg2.Match (m1 [i].Value).Value;
				if (textStr.Length > 1 && idStr.Length > 1) {
					textStr = textStr.Substring (1);
					idStr = idStr.Substring (1);
					idStr = idStr.Remove (idStr.Length - 1);

					DialogOption dialogoption = new DialogOption ();
					dialogoption.id = System.Convert.ToInt32 (idStr);
					dialogoption.text = textStr;
					result.Add (dialogoption);
				} else {
					RO.LoggerUnused.Log (string.Format ("Option Config is wrong!__%s", optionStr.ToString ()));
				}
			}
			return result.ToArray ();
		}
	}
} // namespace RO
