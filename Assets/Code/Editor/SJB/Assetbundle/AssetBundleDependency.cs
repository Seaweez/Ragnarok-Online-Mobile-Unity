using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace EditorTool
{
	public class AssetBundleDependency
	{
		public static string[] GetRawDependencies (string[] pathes)
		{
			return AssetDatabase.GetDependencies (pathes);
		}
	
		public static string[] GetRawDependencies (List<string> pathes)
		{
			return GetRawDependencies (pathes.ToArray ());
		}

		public static string[] GetRawDependencies (string path)
		{
			return GetRawDependencies (new string[]{path});
		}

		public static string[] GetDependenciesWithoutExtensions (List<string> pathes, List<string> extensions, bool includeSelf)
		{
			List<string> rawDependencies = new List<string> (GetRawDependencies (pathes));
			if (!includeSelf) {
				pathes.ForEach ((p) => {
					rawDependencies.Remove (p);
				});
			}
			if (extensions != null) {
				string path = "";
				string extension = "";
				for (int i=rawDependencies.Count-1; i>=0; i--) {
					path = rawDependencies [i];
					extension = Path.GetExtension (path);
					if (extensions.Contains (extension))
						rawDependencies.RemoveAt (i);
				}
			}
			return rawDependencies != null ? rawDependencies.ToArray () : null;
		}
	}
} // namespace EditorTool
