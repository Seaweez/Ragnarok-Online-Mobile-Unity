using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ghost.Utils;
using Ghost.Extensions;
using RO;

namespace EditorTool
{
	public static class GameObjectHelper 
	{
		public static Transform[] GetPoints(GameObject obj, string prefix)
		{
			int maxIndex = 0;
			var indexes = new List<int>();
			
			var pattern = StringUtils.ConnectToString(prefix, @"\d+");
			var objs = obj.FindGameObjectsInChildren(delegate(GameObject testObj){
				var objName = testObj.name;
				if (!objName.StartsWith(prefix))
				{
					return false;
				}
				var match = Regex.Match(objName, pattern, RegexOptions.IgnoreCase);
				if (match.Success)
				{
					match = Regex.Match(match.Value, @"\d+", RegexOptions.RightToLeft);
					if (match.Success)
					{
						var index = int.Parse(match.Value);
						if (0 <= index && !indexes.Contains(index))
						{
							maxIndex = System.Math.Max(maxIndex, index);
							indexes.Add(index);
							return true;
						}
					}
				}
				return false;
			});
			
			if (0 < objs.Length)
			{
				var sortedObjs = new Transform[maxIndex+1];
				for (int i = 0; i < objs.Length; ++i)
				{
					sortedObjs[indexes[i]] = objs[i].transform;
				}
				return sortedObjs;
			}
			
			return null;
		}

		public static TransformWithID[] GetPointsWithID(GameObject obj, string prefix)
		{
			var infos = new List<TransformWithID>();
			var pattern = StringUtils.ConnectToString(prefix, @"\d+");
			obj.FindGameObjectsInChildren(delegate(GameObject testObj){
				var objName = testObj.name;
				if (!objName.StartsWith(prefix))
				{
					return false;
				}
				var match = Regex.Match(objName, pattern, RegexOptions.IgnoreCase);
				if (match.Success)
				{
					match = Regex.Match(match.Value, @"\d+", RegexOptions.RightToLeft);
					if (match.Success)
					{
						var ID = int.Parse(match.Value);
						infos.Add(new TransformWithID(ID, testObj.transform));
					}
				}
				return false;
			});
			if (0 < infos.Count)
			{
				return infos.ToArray();
			}
			return null;
		}

		public static T[] FindComponentsInChildren<T>(GameObject obj) where T:Component
		{
			var components = obj.FindComponentsInChildren<T>();
			if (0 < components.Length)
			{
				return components;
			}
			return null;
		}
	}
} // namespace EditorTool
