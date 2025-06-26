using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Ghost.Extensions;

namespace Ghost.EditorTool
{
	public class AssetManager {
		
		public static List<string> GetAssetDirectoryDependenceDirectories(string directory)
		{
			if (!Directory.Exists(directory))
			{
				Debug.LogWarning(string.Format("{0}\nnot exists", directory));
				return null;
			}
			
			var files = Directory.GetFiles(directory);
			var depends = AssetDatabase.GetDependencies(files);
			if (depends.IsNullOrEmpty())
			{
				return null;
			}
			
			var dependDirectories = new List<string>();
			foreach (var depend in depends)
			{
				var dependDirectory = Path.GetDirectoryName(depend);
				if (string.Equals(dependDirectory, directory))
				{
					continue;
				}
				dependDirectories.Add(dependDirectory);
			}
			
			dependDirectories.MakeUnique();
			
			return dependDirectories;
		}
		
	}
} // namespace Ghost.EditorTool
