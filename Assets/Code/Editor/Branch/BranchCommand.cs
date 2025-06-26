using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ghost.Extensions;
using Ghost.Utils;

namespace EditorTool
{
	public static class BranchCommand 
	{
		public static string GetRootPath()
		{
			var rootPath = Application.dataPath;
			for (int i = 0; i < 4; ++i)
			{
				rootPath = Path.GetDirectoryName(rootPath);
			}
			if (rootPath.EndsWith("client-branches"))
			{
				rootPath = Path.GetDirectoryName(rootPath);
			}
			return PathUnity.Combine(rootPath, "");
		}

		public static string GetDevPath()
		{
			return Path.Combine(GetRootPath(), "client-trunk");
		}

		public static string GetStudioPath()
		{
			return Path.Combine(GetRootPath(), "client-branches/Studio");
		}

		public static string GetTFPath()
		{
			return Path.Combine(GetRootPath(), "client-branches/TF_New");
		}

		public static string GetReleasePath()
		{
			return Path.Combine(GetRootPath(), "client-branches/Release_New");
		}

		public static string GetOverseaPath()
		{
			return Path.Combine(GetRootPath(), "client-branches/Oversea");
		}

		public static string GetOverseaKRPath()
		{
			return Path.Combine(GetRootPath(), "client-branches/OverseaKR");
		}

		public static string GetOverseaRCPath()
		{
			return Path.Combine(GetRootPath(), "client-branches/Oversea_rc");
		}

		public static string GetOversea2017Path()
		{
			return Path.Combine(GetRootPath(), "client-branches/Oversea-2017");
		}

		public static string GetOverseaWWPath()
		{
			return Path.Combine(GetRootPath(), "client-branches/Oversea-WW");
		}

		public static string GetAssetPath(string branchPath)
		{
			return Path.Combine(branchPath, "client-refactory/Develop/Assets");
		}

		public static void CopyAssets(string srcAssetPath, string destAssetPath, string[] paths)
		{
			int fileCount = 0;
			int i = 0;
			System.Action<FileInfo> callback = delegate(FileInfo file){
				EditorUtility.DisplayProgressBar("Dev-->Studio", string.Format("{0}/{1} {2}", ++i, fileCount, file.Name), (float)i/fileCount);
			};

			for (int j = 0; j < paths.Length; ++j)
			{
				var path = Path.Combine(destAssetPath, paths[j]);
				EditorUtility.DisplayProgressBar("Dev-->Studio, Delete Assets", string.Format("{0}/{1} deleting", j+1, paths.Length), (float)j/paths.Length);
				if (Directory.Exists(path))
				{
					Directory.Delete(path, true);
				}
			}

			fileCount = 0;
			for (int j = 0; j < paths.Length; ++j)
			{
				var path = Path.Combine(srcAssetPath, paths[j]);
				EditorUtility.DisplayProgressBar("Dev-->Studio, Calculate Assets File Count", string.Format("{0}/{1} file count: {2}", j+1, paths.Length, fileCount), (float)j/paths.Length);
				var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
				if (null != files)
				{
					fileCount += files.Length;
				}
			}

			i = 0;
			for (int j = 0; j < paths.Length; ++j)
			{
				var srcPath = Path.Combine(srcAssetPath, paths[j]);
				var desPath = Path.Combine(destAssetPath, paths[j]);
				FileSystemUtils.CopyDirectory(srcPath, desPath, null, callback);
			}
		}

		public static void CopyProjectFiles(string srcAssetPath, string destAssetPath)
		{
			var files = Directory.GetFiles(destAssetPath, "*", SearchOption.TopDirectoryOnly);
			if (!files.IsNullOrEmpty())
			{
				for (int j = 0; j < files.Length; ++j)
				{
					File.Delete(files[j]);
					EditorUtility.DisplayProgressBar("Dev-->Studio, Delete Project", string.Format("{0}/{1} deleting", j+1, files.Length), (float)j/files.Length);
				}
			}

			files = Directory.GetFiles(srcAssetPath, "*", SearchOption.TopDirectoryOnly);
			if (!files.IsNullOrEmpty())
			{
				for (int j = 0; j < files.Length; ++j)
				{
					var file = new FileInfo(files[j]);
					var targetPath = Path.Combine(destAssetPath, file.Name);
					FileSystemUtils.CopyFile(file, targetPath);
					EditorUtility.DisplayProgressBar("Dev-->Studio", string.Format("{0}/{1} {2}", ++j, files.Length, file.Name), (float)j/files.Length);
				}
			}
		}

		[MenuItem("RO/Branch/Dev-->Studio/Artist And Designer")]
		static void Dev2Studio_ArtistAndDesigner()
		{
			var devPath = GetDevPath();
			var devAssetPath = GetAssetPath(devPath);
			Path.Combine(devPath, "Cehua");
			Path.Combine(devPath, "client-export");

			var studioPath = GetStudioPath();
			var studioAssetPath = GetAssetPath(studioPath);
			Path.Combine(studioPath, "Cehua");
			Path.Combine(studioPath, "client-export");

			try
			{
				#region root
				var paths = new string[]{
					"Cehua",
					"client-export",
				};
				CopyAssets(devPath, studioPath, paths);
				#endregion root

				#region assets
				paths = new string[]{
					"Art",
					"DevelopEffect",
					"DevelopScene",
					"Engine",
					"Gizmos",
					"Scene",
					"StreamingAssets",
					"Resources/GUI/atlas",
					"Resources/GUI/pic",
					"Resources/NPC",
					"Resources/Role",
					"Resources/Skill",
					"Resources/Public/Audio",
					"Resources/Public/BusCarrier",
					"Resources/Public/Effect",
					"Resources/Public/Emoji",
					"Resources/Public/Item",
					"Resources/Public/Material",
					"Resources/Public/SpineEffect",
				};
				CopyAssets(devAssetPath, studioAssetPath, paths);
				#endregion assets
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}
		}

		[MenuItem("RO/Branch/Dev-->Studio/Programmer")]
		static void Dev2Studio_Programmer()
		{
			var devPath = GetDevPath();
			var devAssetPath = GetAssetPath(devPath);
			var devDevelopPath = Path.Combine(devPath, "client-refactory/Develop");

			var studioPath = GetStudioPath();
			var studioAssetPath = GetAssetPath(studioPath);
			var studioDevelopPath =  Path.Combine(studioPath, "client-refactory/Develop");

			try
			{
				#region Project
				CopyProjectFiles(devDevelopPath, studioDevelopPath);
				var paths = new string[]{
					"iOSSDK",
					"ProjectSettings",
				};
				CopyAssets(devDevelopPath, studioDevelopPath, paths);
				#endregion Project

				#region assets
				paths = new string[]{
					"Code",
					"DevelopTest",
					"NewInstall",
					"Plugins",
					"Resources/AssetManageConfig",
					"Resources/GUI/v1",
					"Resources/Json",
					"Resources/Script",
					"Resources/Slua",
					"Resources/Public/Shader",
				};
				CopyAssets(devAssetPath, studioAssetPath, paths);
				#endregion assets
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}
		}

		[MenuItem("RO/Branch/Dev-->Studio/Programmer(HotUpdate Only)")]
		static void Dev2Studio_ProgrammerHotUpdateOnly()
		{
			var devPath = GetDevPath();
			var devAssetPath = GetAssetPath(devPath);

			var studioPath = GetStudioPath();
			var studioAssetPath = GetAssetPath(studioPath);

			try
			{
				#region assets
				var paths = new string[]{
					"NewInstall",
					"Resources/AssetManageConfig",
					"Resources/GUI/v1",
					"Resources/Json",
					"Resources/Script",
					"Resources/Slua",
					"Resources/Public/Shader",
				};
				CopyAssets(devAssetPath, studioAssetPath, paths);
				#endregion assets
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}
		}

		[MenuItem("RO/Branch/Dev-->Studio/All")]
		static void Dev2Studio_All()
		{
			var devPath = GetDevPath();
			var devDevelopPath = Path.Combine(devPath, "client-refactory/Develop");

			var studioPath = GetStudioPath();
			var studioDevelopPath =  Path.Combine(studioPath, "client-refactory/Develop");

			try
			{
				CopyProjectFiles(devDevelopPath, studioDevelopPath);
				var paths = new string[]{
					"Cehua",
					"client-export",
					"client-refactory/Develop/Assets",
					"client-refactory/Develop/iOSSDK",
					"client-refactory/Develop/ProjectSettings",
				};
				CopyAssets(devPath, studioPath, paths);
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}

		}

		public static void CopyAssetWithMeta(string srcAssetPath, string destAssetPath, string assetPath)
		{
			if (assetPath.StartsWith("Assets/"))
			{
				assetPath = assetPath.Substring(7);
			}
			var destPath = PathUnity.Combine(destAssetPath, assetPath);
			if (!File.Exists(destPath))
			{
				var destFolderPath = Path.GetDirectoryName(destPath);
				if (!Directory.Exists(destFolderPath))
				{
					Directory.CreateDirectory(destFolderPath);
				}
			}
			var srcPath = PathUnity.Combine(srcAssetPath, assetPath);
			File.Copy(srcPath, destPath, true);
			Debug.LogFormat("<color=green>\tCopy File: </color>{0}\n<color=green>\tTo: </color>{1}", srcPath, destPath);
			// meta
			srcPath += ".meta";
			destPath += ".meta";
			File.Copy(srcPath, destPath, true);
			Debug.LogFormat("<color=green>\tCopy File: </color>{0}\n<color=green>\tTo: </color>{1}", srcPath, destPath);
		}

		public static void DelAssetWithMeta(string srcAssetPath, string destAssetPath, string assetPath)
		{
			if (assetPath.StartsWith("Assets/"))
			{
				assetPath = assetPath.Substring(7);
			}
			var destPath = PathUnity.Combine(destAssetPath, assetPath);
			if (File.Exists(destPath))
			{
				File.Delete(destPath);
			}
			var srcPath = PathUnity.Combine(srcAssetPath, assetPath);
		
			destPath += ".meta";
			if (File.Exists(destPath))
			{
				File.Delete(destPath);
			}
		}

		public static void DoAsset<T>(Object selectionObj, System.Action<T, string> doFunc, string typeName, string[] extensions = null) where T:Object
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			if (!extensions.IsNullOrEmpty())
			{
				if (!ArrayUtility.Contains(extensions, Path.GetExtension(path)))
				{
					return;
				}
			}
			if (AssetDatabase.IsValidFolder(path))
			{
				var filterString = "t:"+typeName;
				var guids = AssetDatabase.FindAssets(filterString, new string[]{path}); 

				if (guids.IsNullOrEmpty())
				{
					return;
				}

				var totalCount = guids.Length;
				try
				{
					int i = 0;
					foreach (var guid in guids)
					{
						var objPath = AssetDatabase.GUIDToAssetPath(guid);
						if (!extensions.IsNullOrEmpty())
						{
							if (!ArrayUtility.Contains(extensions, Path.GetExtension(objPath)))
							{
								continue;
							}
						}
						var obj = AssetDatabase.LoadAssetAtPath<T>(objPath);

						++i;
						EditorUtility.DisplayProgressBar(
							string.Format("Proccessing Folder: {0}", path), 
							string.Format("{0}/{1}, {2}", i, totalCount, obj.name), 
							(float)i/totalCount);
						doFunc(obj, objPath);
					}
				}
				finally
				{
					EditorUtility.ClearProgressBar();
				}

			}
			else
			{
				doFunc(selectionObj as T, path);
			}
		}

		public static void DoPrefabs(System.Action<GameObject, string> doFunc)
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				Debug.Log("<color=yellow>No Select Assets</color>");
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoAsset<GameObject>(obj, doFunc, "Prefab");
			}
			AssetDatabase.SaveAssets();
			Debug.LogFormat("<color=green>[DoPrefabs Finished]</color>");
		}

		public static string[] ScriptsExtensions = {".cs",".txt", ".shader",".cginc"};
		public static void DoScripts(System.Action<TextAsset, string> doFunc)
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				Debug.Log("<color=yellow>No Select Assets</color>");
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoAsset<TextAsset>(obj, doFunc, "TextAsset", ScriptsExtensions);
			}
			AssetDatabase.SaveAssets();
			Debug.LogFormat("<color=green>[DoCodes Finished]</color>");
		}

		public static void DoNoDependences(System.Action<Object, string> doFunc)
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				Debug.Log("<color=yellow>No Select Assets</color>");
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoAsset<Object>(obj, doFunc, "");
			}
			AssetDatabase.SaveAssets();
			Debug.LogFormat("<color=green>[DoCodes Finished]</color>");
		}

		static bool ToBranch_Prefabs_Valid()
		{
			var objs = Selection.objects;
			if (objs.IsNullOrEmpty())
			{
				return false;
			}

			foreach (var obj in objs)
			{
				AssetDatabase.GetAssetPath(obj);

				if (typeof(GameObject) != obj.GetType())
				{
					return false;
				}
			}

			return true;
		}

		[MenuItem("Assets/Branch/CopyTo Studio(Prefabs)", true)]
		static bool ToStudio_Prefabs_Valid()
		{
			return ToBranch_Prefabs_Valid();
		}
		[MenuItem("Assets/Branch/CopyTo TF(Prefabs)", true)]
		static bool ToTF_Prefabs_Valid()
		{
			return ToBranch_Prefabs_Valid();
		}
		[MenuItem("Assets/Branch/CopyTo Release(Prefabs)", true)]
		static bool ToRelease_Prefabs_Valid()
		{
			return ToBranch_Prefabs_Valid();
		}
		[MenuItem("Assets/Branch/CopyTo Oversea(Prefabs)", true)]
		static bool ToOversea_Prefabs_Valid()
		{
			return ToBranch_Prefabs_Valid();
		}
		[MenuItem("Assets/Branch/CopyTo Oversea-KR(Prefabs)", true)]
		static bool ToOverseaKR_Prefabs_Valid()
		{
			return ToBranch_Prefabs_Valid();
		}

		[MenuItem("Assets/Branch/CopyTo Oversea-RC(Prefabs)",true)]
		static bool ToOverseaRC_Prefabs_Valid()
		{
			return ToBranch_Prefabs_Valid();
		}

		[MenuItem("Assets/Branch/CopyTo Oversea-2017(Prefabs)",true)]
		static bool ToOversea2017_Prefabs_Valid()
		{
			return ToBranch_Prefabs_Valid();
		}

		[MenuItem("Assets/Branch/CopyTo Oversea-WW(Prefabs)",true)]
		static bool ToOverseaWW_Prefabs_Valid()
		{
			return ToBranch_Prefabs_Valid();
		}

		public static void ToBranch_Prefabs(string branchPath, string branchName)
		{
			var srcAssetPath = Application.dataPath;
			var branchAssetPath = GetAssetPath(branchPath);
			if (srcAssetPath == branchAssetPath)
			{
				Debug.LogFormat("<color=yellow>You'r working copy is {0}!!!</color>", branchName);
				return;
			}

			var sb = new System.Text.StringBuilder(500);
			DoPrefabs(delegate(GameObject obj, string path) {
				var depends = AssetDatabase.GetDependencies(new string[]{path});

				sb.Length = 0;
				sb.AppendLine("Prefab:");
				sb.AppendLine(path);

				if (!depends.IsNullOrEmpty())
				{
					sb.AppendLine();
					sb.AppendLine("Depends:");
					foreach (var depPath in depends)
					{
						if (!depPath.StartsWith("Assets/Art/") && !depPath.StartsWith("Assets/DevelopScene/Terrains")
							|| string.Equals(depPath, path))
						{
							continue;
						}
						sb.AppendLine(depPath);
					}
				}

				if (EditorUtility.DisplayDialog(string.Format("CopyTo {0}(Prefabs)", branchName), sb.ToString(), "OK", "Cancel"))
				{
					Debug.LogFormat(obj, "<color=green>-----To{0}----->: </color>{1}", branchName, path);
					if (!depends.IsNullOrEmpty())
					{
						foreach (var depPath in depends)
						{
							if (!depPath.StartsWith("Assets/Art/") && !depPath.StartsWith("Assets/DevelopScene/Terrains")
								|| string.Equals(depPath, path))
							{
								Debug.LogFormat(obj, "<color=white>\tIgnore: </color>{0}\n",  depPath);
								continue;
							}
							CopyAssetWithMeta(srcAssetPath, branchAssetPath, depPath);
						}
					}
					CopyAssetWithMeta(srcAssetPath, branchAssetPath, path);
					Debug.LogFormat("<color=green>-----To{0}-----< </color>", branchName);
				}
			});
		}

		public static void DelBranch_Prefabs(string branchPath, string branchName)
		{
			var srcAssetPath = Application.dataPath;
			var branchAssetPath = GetAssetPath(branchPath);
			if (srcAssetPath == branchAssetPath)
			{
				return;
			}

			DoPrefabs(delegate(GameObject obj, string path) {
				var depends = AssetDatabase.GetDependencies(new string[]{path});
				if (!depends.IsNullOrEmpty())
				{
					foreach (var depPath in depends)
					{
						if (!depPath.StartsWith("Assets/Art/") && !depPath.StartsWith("Assets/DevelopScene/Terrains")
							|| string.Equals(depPath, path))
						{
							continue;
						}
						DelAssetWithMeta(srcAssetPath, branchAssetPath, depPath);
					}
				}
				DelAssetWithMeta(srcAssetPath, branchAssetPath, path);
			});
		}

		[MenuItem("Assets/Branch/CopyTo Studio(Prefabs)")]
		public static void ToStudio_Prefabs()
		{
			ToBranch_Prefabs(GetStudioPath(), "Studio");
		}

		[MenuItem("Assets/Branch/CopyTo TF(Prefabs)")]
		public static void ToTF_Prefabs()
		{
			ToBranch_Prefabs(GetTFPath(), "TF");
		}

		[MenuItem("Assets/Branch/CopyTo Release(Prefabs)")]
		public static void ToRelease_Prefabs()
		{
			ToBranch_Prefabs(GetReleasePath(), "Release");
		}

		[MenuItem("Assets/Branch/CopyTo Oversea(Prefabs)")]
		public static void ToOversea_Prefabs()
		{
			ToBranch_Prefabs(GetOverseaPath(), "Oversea");
		}

		[MenuItem("Assets/Branch/CopyTo Oversea-KR(Prefabs)")]
		static void ToOverseaKR_Prefabs()
		{
			ToBranch_Prefabs(GetOverseaKRPath(), "Oversea-KR");
		}

		[MenuItem("Assets/Branch/CopyTo Oversea-RC(Prefabs)")]
		public static void ToOverseaRC_Prefabs()
		{
			ToBranch_Prefabs(GetOverseaRCPath(), "Oversea-RC");
		}

		[MenuItem("Assets/Branch/CopyTo Oversea-2017(Prefabs)")]
		static void ToOversea2017_Prefabs()
		{
			ToBranch_Prefabs(GetOversea2017Path(), "Oversea-2017");
		}

		[MenuItem("Assets/Branch/CopyTo Oversea-WW(Prefabs)")]
		static void ToOverseaWW_Prefabs()
		{
			ToBranch_Prefabs(GetOverseaWWPath(), "Oversea-WW");
		}

		static bool ToBranch_NoDependences_Valid()
		{
			var objs = Selection.objects;
			if (objs.IsNullOrEmpty())
			{
				return false;
			}

			foreach (var obj in objs)
			{
				AssetDatabase.GetAssetPath(obj);
				if (typeof(GameObject) == obj.GetType())
				{
					return false;
				}
			}

			return true;
		}

		[MenuItem("Assets/Branch/CopyTo Studio(无依赖)", true)]
		static bool ToStudio_NoDependences_Valid()
		{
			return ToBranch_NoDependences_Valid();
		}

		[MenuItem("Assets/Branch/CopyTo TF(无依赖)", true)]
		static bool ToTF_NoDependences_Valid()
		{
			return ToBranch_NoDependences_Valid();
		}

		[MenuItem("Assets/Branch/CopyTo Release(无依赖)", true)]
		static bool ToRelease_NoDependences_Valid()
		{
			return ToBranch_NoDependences_Valid();
		}

		[MenuItem("Assets/Branch/CopyTo Oversea(无依赖)", true)]
		static bool ToOversea_NoDependences_Valid()
		{
			return ToBranch_NoDependences_Valid();
		}

		[MenuItem("Assets/Branch/CopyTo Oversea-KR(无依赖)", true)]
		static bool ToOverseaKR_NoDependences_Valid()
		{
			return ToBranch_NoDependences_Valid();
		}

		[MenuItem("Assets/Branch/CopyTo Oversea-RC(无依赖)", true)]
		static bool ToOverseaRC_NoDependences_Valid()
		{
			return ToBranch_NoDependences_Valid();
		}

		[MenuItem("Assets/Branch/CopyTo Oversea-2017(无依赖)", true)]
		static bool ToOversea2017_NoDependences_Valid()
		{
			return ToBranch_NoDependences_Valid();
		}
		[MenuItem("Assets/Branch/CopyTo Oversea-WW(无依赖)", true)]
		static bool ToOverseaWW_NoDependences_Valid()
		{
			return ToBranch_NoDependences_Valid();
		}

		static void ToBranch_NoDependences(string branchPath, string branchName)
		{
			var srcAssetPath = Application.dataPath;
			var studioAssetPath = GetAssetPath(branchPath);
			if (srcAssetPath == studioAssetPath)
			{
				Debug.LogFormat("<color=yellow>You'r working copy is {0}!!!</color>", branchName);
				return;
			}

			var sb = new System.Text.StringBuilder(500);
			DoNoDependences(delegate(Object obj, string path) {
				sb.Length = 0;
				sb.AppendLine("资源无依赖拷贝:");
				sb.AppendLine(path);

				if (EditorUtility.DisplayDialog(string.Format("CopyTo {0}(无依赖)", branchName), sb.ToString(), "OK", "Cancel"))
				{
					Debug.LogFormat(obj, "<color=green>-----To{0}----->: </color>{1}", branchName, path);
					CopyAssetWithMeta(srcAssetPath, studioAssetPath, path);
					Debug.LogFormat("<color=green>-----To{0}-----< </color>", branchName);
				}
			});
		}

		[MenuItem("Assets/Branch/CopyTo Studio(无依赖)")]
		static void ToStudio_NoDependences()
		{
			ToBranch_NoDependences(GetStudioPath(), "Studio");
		}
		[MenuItem("Assets/Branch/CopyTo TF(无依赖)")]
		static void ToTF_NoDependences()
		{
			ToBranch_NoDependences(GetTFPath(), "TF");
		}
		[MenuItem("Assets/Branch/CopyTo Release(无依赖)")]
		static void ToRelease_NoDependences()
		{
			ToBranch_NoDependences(GetReleasePath(), "Release");
		}
		[MenuItem("Assets/Branch/CopyTo Oversea(无依赖)")]
		static void ToOversea_NoDependences()
		{
			ToBranch_NoDependences(GetOverseaPath(), "Oversea");
		}
		[MenuItem("Assets/Branch/CopyTo Oversea-KR(无依赖)")]
		static void ToOverseaKR_NoDependences()
		{
			ToBranch_NoDependences(GetOverseaKRPath(), "Oversea-KR");
		}

		[MenuItem("Assets/Branch/CopyTo Oversea-RC(无依赖)")]
		static void ToOverseaRC_NoDependences()
		{
			ToBranch_NoDependences(GetOverseaRCPath(), "Oversea_rc");
		}

		[MenuItem("Assets/Branch/CopyTo Oversea-2017(无依赖)")]
		static void ToOversea2017_NoDependences()
		{
			ToBranch_NoDependences(GetOversea2017Path(), "Oversea-2017");
		}

		[MenuItem("Assets/Branch/CopyTo Oversea-WW(无依赖)")]
		static void ToOverseaWW_NoDependences()
		{
			ToBranch_NoDependences(GetOverseaWWPath(), "Oversea-WW");
		}

	}
} // namespace EditorTool
