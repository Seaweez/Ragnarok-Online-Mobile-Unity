using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using RO;
using System;
using RO.Config;

namespace EditorTool
{
	public static class ArchiveUtil
	{
#if ARCHIVE_AB
		[MenuItem ("AssetBundle/Archive/Copy AssetBundle To StreamAssets")]
#else
		[MenuItem ("AssetBundle/Archive/Zip AssetBundle To StreamAssets")]
#endif
		public static void ZipBundlesAndMoveToStream()
		{
            var sw = new System.Diagnostics.Stopwatch();  
			sw.Start ();
			
			//删除原来的StreamingAssets目录
			DeleteStreamingAssets();
			
#if ARCHIVE_AB
			//将所有AssetBundle拷贝到StreamingAssets目录下
			CopyBundles.StartCopy();
#else
			//压缩AssetBundle到StreamingAssets目录下
			ZipBundles.StartZip();
#endif
			
			//写入版本信息文件
			string versionFile = Path.GetFileName(PathHelper.GetVersionConfigPath(false, BuildBundleEnvInfo.Env));
			string targetPath = Path.Combine(Application.dataPath + "/Resources", versionFile);
			if (File.Exists(targetPath))
			{
				File.Delete(targetPath);
			}
			File.Copy(PathHelper.GetVersionConfigPath(true, BuildBundleEnvInfo.Env), targetPath);
			
			//刷新AssetData
			AssetDatabase.Refresh ();
			
			sw.Stop();
			Debug.Log(string.Format("压缩耗时{0}秒", sw.Elapsed.TotalMilliseconds / 1000));
		}

		[MenuItem("AssetBundle/Archive/Clear Zip")]
		public static void ZipClear()
		{
			DirectoryInfo folder = new DirectoryInfo(Application.streamingAssetsPath);
			folder.Delete();
		}

		[MenuItem ("Assets/自动拆分压缩选中文件(夹)")]
		public static void SplitZipSelect ()
		{
			UnityEngine.Object select = Selection.activeObject;
			if (select != null) {
				string path = AssetDatabase.GetAssetPath (select);
				AvarageSizeZips.StartZipBySize(200 * 1024 * 1024, path);
				AssetDatabase.Refresh ();
			}
		}

		[MenuItem ("Assets/zip压缩选中文件(夹）")]
		public static void ZipSelect ()
		{
			UnityEngine.Object select = Selection.activeObject;
			if (select != null) {
				System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch ();  
				sw.Start ();
				string path = "/" + AssetDatabase.GetAssetPath (select);
				path = path.Replace ("/Assets/", "");
				path = Path.Combine (Application.dataPath, path);
				string targetZip = ROPathConfig.TrimExtension (path) + ".zip";
				Debug.Log (path);
				Debug.Log (targetZip);
				ZipUtil.compressDir (path, 5, targetZip, new string[]{ ".meta" }, true);
				AssetDatabase.Refresh ();
				sw.Stop ();  
				TimeSpan ts2 = sw.Elapsed;
				Debug.Log (string.Format ("压缩耗时{0}秒", ts2.TotalMilliseconds / 1000));
			}
		}

		[MenuItem ("Assets/zip压缩多个选中文件(夹）")]
		public static void CompressSelects ()
		{
			UnityEngine.Object[] selects = Selection.objects;
			string outputPath = Path.Combine (Application.dataPath, BundleLoaderStrategy.EditorRoot);
			string targetZip = EditorUtility.SaveFilePanelInProject ("Save As",
				                   "xxx_yyy.zip", "zip", "Save zip as...", NGUISettings.currentPath);
			for (int i = 0; i < selects.Length; i++) {
				string path = "/" + AssetDatabase.GetAssetPath (selects [i]);
				path = path.Replace ("/Assets/", "");
				path = Path.Combine (Application.dataPath, path);
				string extension = Path.GetExtension (path);
				if (string.IsNullOrEmpty (extension))
					ZipUtil.compressDir (path, 5, targetZip, new string[]{ ".meta" }, true, false);
				else
					ZipUtil.compressFile (path, outputPath, 5, targetZip, new string[]{ ".meta" }, true, false);
				AssetDatabase.Refresh ();
			}
		}

		[MenuItem ("Assets/追加内容到文件尾部")]
		public static void AppendTextToFileEnd ()
		{
			UnityEngine.Object select = Selection.activeObject;
			if (select != null) {
				string path = "/" + AssetDatabase.GetAssetPath (select);
				path = path.Replace ("/Assets/", "");
				path = Path.Combine (Application.dataPath, path);
//				TestFileEnd.WriteVerifyToFileEnd (path);
			}
		}

		[MenuItem ("Assets/Test追加内容到文件尾部")]
		public static void TestAppendTextToFileEnd ()
		{
			UnityEngine.Object select = Selection.activeObject;
			if (select != null) {
				string path = "/" + AssetDatabase.GetAssetPath (select);
				path = path.Replace ("/Assets/", "");
				path = Path.Combine (Application.dataPath, path);
//				Debug.Log (TestFileEnd.HasVerify (path));
			}
		}

		[MenuItem ("Assets/测试文件的CRC16")]
		public static void PrintSelectFileCRC ()
		{
			UnityEngine.Object select = Selection.activeObject;
			if (select != null) {
				string path = "/" + AssetDatabase.GetAssetPath (select);
				path = path.Replace ("/Assets/", "");
				path = Path.Combine (Application.dataPath, path);
			}
		}

		[MenuItem ("Assets/测试文件的md5")]
		public static void PrintSelectFileMD5 ()
		{
			UnityEngine.Object select = Selection.activeObject;
			if (select != null) {
				string path = "/" + AssetDatabase.GetAssetPath (select);
				path = path.Replace ("/Assets/", "");
				path = Path.Combine (Application.dataPath, path);
				Debug.Log (MyMD5.HashFile (path));
			}
		}

		[MenuItem ("Assets/查看文件信息")]
		public static void GetFileInfo ()
		{
			UnityEngine.Object select = Selection.activeObject;
			if (select != null) {
				string path = "/" + AssetDatabase.GetAssetPath (select);
				path = path.Replace ("/Assets/", "");
				path = Path.Combine (Application.dataPath, path);
				FileInfo fi = new FileInfo (path);
				Debug.Log (fi.Length);
			}
		}


		[MenuItem ("AssetBundle/unity3d尾部追加")]
		public static void FileEndAppend ()
		{
		}

		[MenuItem ("Assets/查看压缩包信息")]
		public static void GetZipInfo ()
		{
			UnityEngine.Object select = Selection.activeObject;
			if (select != null) {
				string path = "/" + AssetDatabase.GetAssetPath (select);
				path = path.Replace ("/Assets/", "");
				path = Path.Combine (Application.dataPath, path);
				string outputPath = Path.Combine (Application.dataPath, BundleLoaderStrategy.EditorRoot);
				long res = lzip.getFileInfo (path, outputPath);
				if (res < 0) {
					Debug.LogError (res);
				} else {
					Debug.LogFormat ("解压后大小:{0}", res);
					int count = lzip.ninfo.Count;
					if (count == lzip.uinfo.Count) {
						for (int i = 0; i < count; i++) {
							Debug.LogFormat ("{0}  size:{1}", lzip.ninfo [i], lzip.uinfo [i]);
						}
					}
				}
				AssetDatabase.Refresh ();
			}
		}

		/// <summary>
		/// 删除StreamingAssets目录
		/// </summary>
		public static void DeleteStreamingAssets()
		{
			Action deleteTempFolder = () =>
			{//删除临时路径
				var folderPath = Application.streamingAssetsPath + "Temp";
				if (Directory.Exists(folderPath))
				{
					var folder = new DirectoryInfo(folderPath);
					folder.Delete(true);
				}
			};

			deleteTempFolder();

			if (Directory.Exists(Application.streamingAssetsPath))
			{
				var folder = new DirectoryInfo(Application.streamingAssetsPath);
				foreach (var video in folder.GetDirectories("Videos"))
				{
					video.Delete(true);
				}

				try
				{
					folder.MoveTo(Application.streamingAssetsPath + "Temp");
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					folder.Delete(true);
				}
				
			}
			Directory.CreateDirectory(Application.streamingAssetsPath);

			deleteTempFolder();
		}
	}
}

// namespace EditorTool
