using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace RO.Config
{
	[SLua.CustomLuaClassAttribute]
	public static class ROPathConfig
	{
		public const string VERSIONFILE = "Versions.xml";
		public const string STREAMZIP = "assets.zip";
	
		public static string VersionFileName {
			get {
				return ApplicationHelper.platformFolder + "_Versions.xml";
			}
		}

		public static string StreamVersionFileName {
			get {
				string filePath = Application.streamingAssetsPath;
				return Path.Combine (filePath, STREAMZIP);
			}
		}

		public static string PersistentDirectory {
			get {
				return ApplicationHelper.persistentDataPath;
			}
		}

		public static string TrimExtension(string path)
		{
			string extension = Path.GetExtension(path);
			if (string.IsNullOrEmpty (extension) == false)
				return path.Replace (extension,"");
			return path;
		}
	}
} // namespace RO.Config
