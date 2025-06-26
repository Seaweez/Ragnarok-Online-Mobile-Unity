using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;
using RO.Config;
using RO;

namespace EditorTool
{
	public class ZipBundles
	{
		const string buildBundlePath = "Assets/";

		public static void StartZip ()
		{
			long splitSize = 
				#if UNITY_IPHONE || UNITY_IOS
				130 * 1024 * 1024;
			#elif UNITY_ANDROID
				60*1024*1024;
			#else
				60 * 1024 * 1024;
			#endif
			
			List<GreedyAvarageZip> splits = AvarageSizeZips.StartZipBySize (splitSize);
			SaveZipNamesToXML (splits);
			SaveMD5Check (splits);
		}

		static void SaveZipNamesToXML (List<GreedyAvarageZip> splits)
		{
			if (config.names != null)
				config.names.Clear ();
			long totalzipSize = 0;
			long totalunzipSize = 0;
			int totalunzipfiles = 0;
			foreach (string f in Directory.GetFiles(Application.streamingAssetsPath, "*.zip")) {
				FileInfo info = new FileInfo (f);
				totalzipSize += info.Length;
				GreedyAvarageZip zipInfo = splits.Find ((v) => {
					return v.zipName == f;
				});
				config.AddInfo (Path.GetFileName (f), info.Length, zipInfo.totalSize);
				totalunzipSize += zipInfo.totalSize;
				totalunzipfiles += zipInfo.files.Count;
			}
			config.size = totalzipSize;
			config.unzipSize = totalunzipSize;
			config.unZipFilesCount = totalunzipfiles;
			config.SaveToFile (path);
		}

		[MenuItem ("AssetBundle/测试安装包资源MD5")]
		public static void SaveMD5Check ()
		{
			long splitSize = 
				#if UNITY_IPHONE || UNITY_IOS
				130 * 1024 * 1024;
			#elif UNITY_ANDROID
				60*1024*1024;
			#else
				60 * 1024 * 1024;
			#endif
			List<GreedyAvarageZip> splits = AvarageSizeZips.SplitZipBySize (splitSize);
			SaveMD5Check (splits);
		}

		static void SaveMD5Check (List<GreedyAvarageZip> splits)
		{
			GreedyAvarageZip split = null;
			string md5 = null;
			string filename = null;
			string rawfilename = null;
			string prefix = Ghost.Utils.PathUnity.Combine (Application.dataPath, BundleLoaderStrategy.EditorRoot);
			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch ();  
			sw.Start ();
			int fileCount = 0;
			string mainMainfestName = ApplicationHelper.platformFolder;
			ABZipMD5Infos info = new ABZipMD5Infos ("Install.zip");
			for (int i = 0; i < splits.Count; i++) {
				split = splits [i];
				if (split.files != null) {
					string extension = null;
					for (int k = 0; k < split.files.Count; k++) {
						filename = split.files [k].fileName;
						rawfilename = Path.GetFileNameWithoutExtension (filename);
						extension = Path.GetExtension (filename);
						if (extension == ".manifest" && !rawfilename.Contains (mainMainfestName)) {
							continue;
						}
						if (extension != ".unity3d" || filename.Contains ("script/")) {
							fileCount += 1;
							md5 = MyMD5.HashFile (filename);
							filename = filename.Replace (prefix, "");
							info.Add (filename, md5, split.files [k].fileSize);
						}
					}
				}
			}
			sw.Stop ();
			TimeSpan ts2 = sw.Elapsed;
			Debug.Log (string.Format ("md5耗时{0}秒,总共{1}个文件", ts2.TotalMilliseconds / 1000, fileCount));
			ExtractFilesCheck.SaveInstallABZipMD5Info (info);
			AssetDatabase.Refresh ();
			ABZipMD5Infos read = ExtractFilesCheck.ReadABZipMD5InfosInResource ();
			Debug.Log (read.md5Infos.Count);
		}

		static string path = Path.Combine (Application.dataPath, "Resources/InstallAssetZips.xml");
		static InstallAssetZips _config;

		static InstallAssetZips config {
			get {
				if (_config == null) {
					_config = InstallAssetZips.CreateByFile (path);
					if (_config == null) {
						_config = new InstallAssetZips ();
					}
				}
				return _config;
			}
		}

		public static String BytesToString (long byteCount)
		{
			string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
			if (byteCount == 0)
				return "0" + suf [0];
			long bytes = Math.Abs (byteCount);
			int place = Convert.ToInt32 (Math.Floor (Math.Log (bytes, 1024)));
			double num = Math.Round (bytes / Math.Pow (1024, place), 1);
			return (Math.Sign (byteCount) * num).ToString () + suf [place];
		}
	}
}
// namespace EditorTool


#region 测试
/*using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;
using RO;

namespace EditorTool
{
	public static class ZipBundles
	{
		/// <summary>
		/// 压缩包大小
		/// </summary>
#if UNITY_IPHONE || UNITY_IOS
		const long ZipSplitSize = 130 * 1024 * 1024;
#elif UNITY_ANDROID
		const long ZipSplitSize = 60 * 1024 * 1024;
#else
		const long ZipSplitSize = 60 * 1024 * 1024;
#endif

		/// <summary>
		/// 开始压缩
		/// </summary>
		public static void StartZip ()
		{
			//压缩新zips
			var splits = AvarageSizeZips.StartByZipFileSize(ZipSplitSize);
			SaveZipNamesToXML(splits);
			SaveMD5Check(splits);
		}

		static void SaveZipNamesToXML (List<GreedyAvarageZip> splits)
		{
			if (config.names != null)
			{
				config.names.Clear ();
			}
			
			long totalzipSize = 0;
			long totalunzipSize = 0;
			int totalunzipfiles = 0;
			foreach (string f in Directory.GetFiles(Application.streamingAssetsPath, "*.zip"))
			{
				FileInfo info = new FileInfo (f);
				totalzipSize += info.Length;
				GreedyAvarageZip zipInfo = splits.Find ((v) => v.zipName == f);
				config.AddInfo (Path.GetFileName (f), info.Length, zipInfo.totalSize);
				totalunzipSize += zipInfo.totalSize;
				totalunzipfiles += zipInfo.files.Count;
			}
			config.size = totalzipSize;
			config.unzipSize = totalunzipSize;
			config.unZipFilesCount = totalunzipfiles;
			config.SaveToFile (path);
		}

		[MenuItem ("AssetBundle/测试安装包资源MD5")]
		public static void SaveMD5Check ()
		{
			List<GreedyAvarageZip> splits = AvarageSizeZips.SplitByZipFileSize(ZipSplitSize);
			SaveMD5Check(splits);
		}
		
		static void SaveMD5Check (List<GreedyAvarageZip> splits)
		{
			GreedyAvarageZip split = null;
			string md5 = null;
			string filename = null;
			string rawfilename = null;
			string prefix = Ghost.Utils.PathUnity.Combine (Application.dataPath, BundleLoaderStrategy.EditorRoot);
			int fileCount = 0;
			string mainMainfestName = ApplicationHelper.platformFolder;
			ABZipMD5Infos info = new ABZipMD5Infos ("Install.zip");
			for (int i = 0; i < splits.Count; i++) {
				split = splits [i];
				if (split.files != null) {
					string extension = null;
					for (int k = 0; k < split.files.Count; k++) {
						filename = split.files [k].fileName;
						rawfilename = Path.GetFileNameWithoutExtension (filename);
						extension = Path.GetExtension (filename);
						if (extension == ".manifest" && !rawfilename.Contains (mainMainfestName)) {
							continue;
						}
						if (extension != ".unity3d" || filename.Contains ("script/")) {
							fileCount += 1;
							md5 = MyMD5.HashFile (filename);
							filename = filename.Replace (prefix, "");
							info.Add (filename, md5, split.files [k].fileSize);
						}
					}
				}
			}

			ExtractFilesCheck.SaveInstallABZipMD5Info (info);
			AssetDatabase.Refresh ();
			ABZipMD5Infos read = ExtractFilesCheck.ReadABZipMD5InfosInResource ();
		}

		static string path = Path.Combine (Application.dataPath, "Resources/InstallAssetZips.xml");
		static InstallAssetZips _config;

		static InstallAssetZips config {
			get {
				if (_config == null) {
					_config = InstallAssetZips.CreateByFile (path);
					if (_config == null) {
						_config = new InstallAssetZips ();
					}
				}
				return _config;
			}
		}

		public static String BytesToString (long byteCount)
		{
			string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
			if (byteCount == 0)
				return "0" + suf [0];
			long bytes = Math.Abs (byteCount);
			int place = Convert.ToInt32 (Math.Floor (Math.Log (bytes, 1024)));
			double num = Math.Round (bytes / Math.Pow (1024, place), 1);
			return (Math.Sign (byteCount) * num).ToString () + suf [place];
		}
	}
}
// namespace EditorTool*/
#endregion
