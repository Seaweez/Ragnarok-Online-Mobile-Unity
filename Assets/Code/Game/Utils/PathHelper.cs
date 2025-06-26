using UnityEngine;
using System.Collections.Generic;
using Ghost.Config;
using Ghost.Utils;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public static class PathHelper
	{
		public static readonly string LOCAL_URL_PREFIX = PathConfig.LOCAL_URL_PREFIX;

		public static string GetPathURL (string path)
		{
			return StringUtils.ConnectToString (LOCAL_URL_PREFIX, path);
		}

		public static string GetSavePath (string subPath)
		{
			return PathUnity.Combine (Application.persistentDataPath, subPath);
		}

		public static string GetVersionConfigPath (bool absolutePath, string env = "")
		{
			string fileName = PathUnity.Combine (env, ApplicationHelper.platformFolder + "_Versions.xml");
			if (absolutePath)
				return PathUnity.Combine (Application.dataPath, BundleLoaderStrategy.EditorRoot) + fileName;
			else
				return PathUnity.Combine ("Assets/", BundleLoaderStrategy.EditorRoot) + fileName;
		}
	}
} // namespace RO
