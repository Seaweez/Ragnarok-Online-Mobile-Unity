using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EditorTool
{
	public static class EditorToolUtils 
	{
		public static string FormatName(string name, int ID)
		{
			var match = Regex.Match(name, @"_(\-?)\d+", RegexOptions.RightToLeft);
			if (match.Success)
			{
				if (match.Index+match.Length == name.Length)
				{
					name = name.Remove(match.Index);
				}
			}
			return string.Format("{0}_{1}", name, ID);
		}
	
	}
} // namespace EditorTool
