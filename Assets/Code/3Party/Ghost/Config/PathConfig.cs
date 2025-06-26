using UnityEngine;
using System.IO;

namespace Ghost.Config
{
	public static class PathConfig{

		public static readonly char[] SEPARATORS;

		public static readonly string DIRECTORY_ASSETS;
		public static readonly string DIRECTORY_STREAMING_ASSETS;
		public static readonly string DIRECTORY_PERSISTENT_ASSETS;
		public const string DIRECTORY_RESOURCES = "Resources";
		public const string DIRECTORY_STANDARD_ASSETS = "Standard Assets";
		public const string DIRECTORY_MATERIALS = "Materials";

		public static readonly string LOCAL_URL_PREFIX;

		public const string EXTENSION_SCENE = "unity";
		public const string EXTENSION_PREFAB = "prefab";
		public const string EXTENSION_FBX = "fbx";
		public const string EXTENSION_ASSET = "asset";
		public const string EXTENSION_ASSET_BUNDLE = "assetbundle";
		public const string EXTENSION_SCENE_PACKAGE = "unity3d";
		public const string EXTENSION_MATERIAL = "mat";
		public const string EXTENSION_ANIMATION = "anim";
		public const string EXTENSION_JSON = "json";
		public const string EXTENSION_LIGHT_MAP = "exr";

		public const string FILE_NAV_MESH = "NavMesh.asset";

		static PathConfig()
		{
			SEPARATORS = new char[]{
				Path.AltDirectorySeparatorChar, 
				Path.DirectorySeparatorChar, 
				Path.PathSeparator, 
				Path.VolumeSeparatorChar
			};
			DIRECTORY_ASSETS = Path.GetFileName(Application.dataPath.TrimEnd(SEPARATORS));
			DIRECTORY_STREAMING_ASSETS = Path.GetFileName(Application.streamingAssetsPath.TrimEnd(SEPARATORS));
			DIRECTORY_PERSISTENT_ASSETS = Path.GetFileName(Application.persistentDataPath.TrimEnd(SEPARATORS));

			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				LOCAL_URL_PREFIX = "jar:file://";
				break;
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.IPhonePlayer:
				LOCAL_URL_PREFIX = "file://";
				break;
			default:
				LOCAL_URL_PREFIX = "";
				break;
			}
		}

	}
} // namespace Ghost.Config
