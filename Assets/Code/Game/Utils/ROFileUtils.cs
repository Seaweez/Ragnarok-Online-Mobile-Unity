using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public static class ROFileUtils
	{
		public static bool FileExists (string path)
		{
			return File.Exists (path);
		}
	
		public static void FileDelete (string file)
		{
			if (File.Exists (file))
				File.Delete (file);
		}

		public static string ReadAllText(string path)
		{
			if (File.Exists (path))
				return File.ReadAllText (path);
			return null;
		}

		public static bool DirectoryExists (string path)
		{
			return Directory.Exists (path);
		}

		public static void DirectoryDelete (string file, bool recursive)
		{
			if (Directory.Exists (file))
				Directory.Delete (file, recursive);
		}
	}
} // namespace RO
