using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Ghost.Extensions;

namespace Ghost.EditorTool
{
	public class CheckAssetDependenceLevel {
		
		private static void DoCheckDependenceLevelEachFolder (bool deep)
		{
			var directories = new List<string>();

			var selectedAssets = Selection.GetFiltered (typeof(Object), SelectionMode.Assets);
			foreach (Object obj in selectedAssets) 
			{
				var directory = AssetDatabase.GetAssetPath(obj);
				if (File.Exists(directory))
				{
					directory = Path.GetDirectoryName(directory);
				}
				directories.Add(directory);
			}

			directories.MakeUnique();

			if (deep)
			{
				var subDirectories = new List<string>();
				foreach (var directory in directories)
				{
					subDirectories.AddRange(Directory.GetDirectories(directory, "*", SearchOption.AllDirectories));
				}
				directories.AddRange(subDirectories);
				directories.MakeUnique();
			}

			try
			{
				float totalCount = directories.Count;
				float checkedCount = 0;
				foreach (var directory in directories)
				{
					EditorUtility.DisplayProgressBar(string.Format("CheckDependenceLevel: {0}/{1}", checkedCount, totalCount), string.Format("Checking: {0}", directory), checkedCount/totalCount);
					var level = AssetDependenceLevel.GetAssetLevel(null, directory);
					if (AssetDependenceLevel.IsBadAssetLevel(level))
					{
						continue;
					}
					var dependDirectories = AssetManager.GetAssetDirectoryDependenceDirectories(directory);
					
					foreach (var dependDirectory in dependDirectories)
					{
						var dependDirectoryLevel = AssetDependenceLevel.GetAssetLevel(null, dependDirectory);
						if (dependDirectoryLevel >= level)
						{
							Debug.LogError(string.Format("{0} level: {1}, bad depends on:\n{2} level: {3}", directory, level, dependDirectory, dependDirectoryLevel));
						}
					}
					++checkedCount;
				}
			}
			catch (System.Exception e)
			{
				throw e;
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}
		}

		[MenuItem("Assets/CheckDependenceLevel/DependenceLevelEachFolder")]
		static void CheckDependenceLevelEachFolder () { DoCheckDependenceLevelEachFolder (false); }
		
		[MenuItem("Assets/CheckDependenceLevel/DependenceLevelEachFolder(Deep)")]
		static void CheckDependenceLevelEachFolderDeep () { DoCheckDependenceLevelEachFolder (true); }
		
	}
} // namespace Ghost.EditorTool
