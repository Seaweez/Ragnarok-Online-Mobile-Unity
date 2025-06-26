using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;
using RO.Config;
using RO;
using Ghost.Utils;

namespace EditorTool
{
	public class NeedZippedFileInfo
	{
		string _fileName;
		long _fileSize;

		public long fileSize {
			get {
				return _fileSize;
			}
		}

		public string fileName {
			get {
				return _fileName;
			}
		}

		public NeedZippedFileInfo (string fileName)
		{
			_fileName = PathUnity.Combine (Application.dataPath, fileName.Replace (AvarageSizeZips.buildBundlePath, ""));
			_fileSize = new FileInfo (_fileName).Length;
		}
	}

	public class GreedyAvarageZip
	{
		public static int index = 0;
		List<NeedZippedFileInfo> _files = new List<NeedZippedFileInfo> ();
		string _zipName;
		long _totalSize;

		public List<NeedZippedFileInfo> files {
			get {
				return _files;
			}
		}

		public long totalSize {
			get {
				return _totalSize;
			}
		}

		public string zipName {
			get {
				return _zipName;
			}
		}

		public GreedyAvarageZip (string path)
		{
			if (string.IsNullOrEmpty (path)) {
				path = Application.streamingAssetsPath;
			}
			_zipName = Path.Combine (path, string.Format ("assets{0}.zip", index++));
		}

		public void AddNeedZippedFile (NeedZippedFileInfo file)
		{
			if (_files.Contains (file) == false) {
				_totalSize += file.fileSize;
				_files.Add (file);
			}
		}

		public override string ToString ()
		{
			return string.Format ("[GreedyAvarageZip: files={0}, totalSize={1}, zipName={2}]", files.Count, ZipBundles.BytesToString (totalSize), zipName);
		}
	}

	public class AvarageSizeZips
	{
		const int CompressLevel = 9;
		public const string buildBundlePath = "Assets/";
		static string BundleDir = PathUnity.Combine (buildBundlePath + BundleLoaderStrategy.EditorRoot, ApplicationHelper.platformFolder);
		static string BundleDir2 = PathUnity.Combine (Application.dataPath, BundleLoaderStrategy.EditorRoot);
		static long _totalBundlesSize = 0;

		public static long totalBundlesSize (string path)
		{
//			if (_TotalBundleFiles == null || _totalBundlesSize == 0) {
			Debug.Log (TotalBundleFiles (path).Count);
//			}
			return _totalBundlesSize;
		}
		
		//获取总大小
		//获取所有需要压缩的文件
		static List<NeedZippedFileInfo> _TotalBundleFiles;

		static List<NeedZippedFileInfo> TotalBundleFiles (string path)
		{
//			if (_TotalBundleFiles == null) {
			_totalBundlesSize = 0;
			GreedyAvarageZip.index = 0;
			_TotalBundleFiles = new List<NeedZippedFileInfo> ();
			if (string.IsNullOrEmpty (path)) {
				path = BundleDir;
			}
			string[] files = Directory.GetFiles (path, "*", SearchOption.AllDirectories);
			string fileName = null;
			string extension = null;
			string rawFileName = null;
			string versionFile = ROPathConfig.VersionFileName;
			for (int i=0; i<files.Length; i++) {
				fileName = files [i];
				extension = Path.GetExtension (fileName);
				rawFileName = Path.GetFileName (fileName);
				if (rawFileName == versionFile) {
					continue;
				}
				if (string.IsNullOrEmpty (extension) || extension == ".unity3d" || extension == ".manifest" || extension == ".mp4" || extension == ".xml" || extension == ".txt") {
					NeedZippedFileInfo info = new NeedZippedFileInfo (fileName);
					_TotalBundleFiles.Add (info);
					_totalBundlesSize += info.fileSize;
				} else if (extension != ".DS_Store" && extension != ".meta") {
					Debug.Log (string.Format ("file:{0}, need not to be zipped..", fileName));
				}
			}
//			}
			return _TotalBundleFiles;
		}

		static int Compare (NeedZippedFileInfo x, NeedZippedFileInfo y)
		{
			//从大到小
			return y.fileSize.CompareTo (x.fileSize);
		}

		static int CompareZips (GreedyAvarageZip x, GreedyAvarageZip y)
		{
			//从大到小
			return x.totalSize.CompareTo (y.totalSize);
		}

		static void StartZip (List<GreedyAvarageZip> splitZips, string path)
		{
			if (splitZips != null) {
				System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch ();  
				sw.Start ();
				for (int i=0; i<splitZips.Count; i++) {
					ZipIt (splitZips [i], path);
				}
				sw.Stop ();  
				TimeSpan ts2 = sw.Elapsed;
				Debug.Log (string.Format ("压缩耗时{0}秒", ts2.TotalMilliseconds / 1000));
			}
			_TotalBundleFiles = null;
		}

		static void ZipIt (GreedyAvarageZip greedyAvarageZip, string path)
		{
			NeedZippedFileInfo file = null;
			if (string.IsNullOrEmpty (path)) {
				path = BundleDir2;
			}
			for (int i=0; i<greedyAvarageZip.files.Count; i++) {
				file = greedyAvarageZip.files [i];
				ZipUtil.compressFile (file.fileName, path, CompressLevel, greedyAvarageZip.zipName, null, true, false);
			}
		}

		//avarageSize 
		public static List<GreedyAvarageZip> StartZipBySize (long avarageSize, string path = null, string targetPath = null)
		{
			if (avarageSize > 0) {
				return StartZipByZipFileCount (Mathf.FloorToInt (totalBundlesSize (path) / avarageSize), path, targetPath);
			}
			return null;
		}

		public static List<GreedyAvarageZip> StartZipByZipFileCount (int count, string path = null, string targetPath = null)
		{
			if (string.IsNullOrEmpty (targetPath) && string.IsNullOrEmpty (path) == false) {
				targetPath = Directory.GetParent (path).FullName + "/";
			}
//			BundleDir = Path.Combine (buildBundlePath + BundleLoaderStrategy.EditorRoot, ApplicationHelper.platformFolder);
			List<GreedyAvarageZip> splitZips = Distrube (TotalBundleFiles (path), count, targetPath);
			StartZip (splitZips, targetPath);
			return splitZips;
		}

		public static List<GreedyAvarageZip> SplitZipBySize(long avarageSize, string path = null, string targetPath = null)
		{
			if (avarageSize > 0) {
				return SplitZipByCount (Mathf.FloorToInt (totalBundlesSize (path) / avarageSize), path, targetPath);
			}
			return null;
		}

		public static List<GreedyAvarageZip> SplitZipByCount(int count, string path = null, string targetPath = null)
		{
			if (string.IsNullOrEmpty (targetPath) && string.IsNullOrEmpty (path) == false) {
				targetPath = Directory.GetParent (path).FullName + "/";
			}
			//			BundleDir = Path.Combine (buildBundlePath + BundleLoaderStrategy.EditorRoot, ApplicationHelper.platformFolder);
			List<GreedyAvarageZip> splitZips = Distrube (TotalBundleFiles (path), count, targetPath);
			return splitZips;
		}

		/// <summary>
		/// 倒序贪婪
		/// </summary>
		/// <param name="list"></param>
		/// <param name="count"></param>
		static List<GreedyAvarageZip> Distrube (List<NeedZippedFileInfo> list, int count, string path = null)
		{
			if (list.Count < count) {
				Debug.LogError ("分配的list大小必须大于等于分组数");
				return null;
			}
			List<GreedyAvarageZip> sumlist = new List<GreedyAvarageZip> ();//保存分组的总和,方便根据其值进行从小到大排序
//			List<List<long>> slist = new List<List<long>> ();//保存分组的所有分配到的值

			//先将list按照从大到小排序
			list.Sort (Compare);
			
			int n = 0;
			int c = 0;
			for (int i = 0; i < list.Count; i += count) {//每次取count步长的值
				n++;
				c = 0;
				for (int j = i; j < n * count; j++) {//将count步长中的值从0开始一个个赋予每个分组
					if (j >= list.Count)
						break;					
					if (sumlist.Count < count) {
						GreedyAvarageZip greedyZip = new GreedyAvarageZip (path);
						sumlist.Add (greedyZip);
						greedyZip.AddNeedZippedFile (list [j]);
					} else {
						sumlist [c].AddNeedZippedFile (list [j]);
						c++;
					}
				}
				//sort
				sumlist.Sort (CompareZips);
				
			}
			for (int jj = 0; jj < sumlist.Count; jj++) {
				Debug.Log (sumlist [jj].ToString ());
			}
			return sumlist;
		}
	}
} // namespace EditorTool

#region 测试
/*using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RO.Config;
using RO;
using Ghost.Utils;
using ProBuilder2.Common;
using UnityEditor;

namespace EditorTool
{
	public class NeedZippedFileInfo
	{
		string _fileName;
		long _fileSize;

		public long fileSize {
			get {
				return _fileSize;
			}
		}

		public string fileName {
			get {
				return _fileName;
			}
		}

		public NeedZippedFileInfo (string fileName)
		{
			_fileName = PathUnity.Combine (Application.dataPath, fileName.Replace (AvarageSizeZips.BuildBundlePath, ""));
			_fileSize = new FileInfo (_fileName).Length;
		}
	}

	public class GreedyAvarageZip
	{
		public static int index = 0;
		List<NeedZippedFileInfo> _files = new List<NeedZippedFileInfo> ();
		string _zipName;
		long _totalSize;

		public List<NeedZippedFileInfo> files {
			get {
				return _files;
			}
		}

		public long totalSize {
			get {
				return _totalSize;
			}
		}

		public string zipName {
			get {
				return _zipName;
			}
		}

		public GreedyAvarageZip (string path)
		{
			if (string.IsNullOrEmpty (path)) {
				path = Application.streamingAssetsPath;
			}
			_zipName = Path.Combine (path, string.Format ("assets{0}.zip", index++));
		}

		public GreedyAvarageZip(string path, string zipName)
		{
			if (string.IsNullOrEmpty (path)) {
				path = Application.streamingAssetsPath;
			}
			_zipName = Path.Combine(path, string.Format("{0}.zip", zipName));
		}

		public void AddNeedZippedFile (NeedZippedFileInfo file)
		{
			if (_files.Contains (file) == false) {
				_totalSize += file.fileSize;
				_files.Add (file);
			}
		}

		public override string ToString ()
		{
			return string.Format ("[GreedyAvarageZip: files={0}, totalSize={1}, zipName={2}]", files.Count, ZipBundles.BytesToString (totalSize), zipName);
		}
	}

	public static class AvarageSizeZips
	{
		public const string BuildBundlePath = "Assets/";
		
		private const int CompressLevel = 9;
        private const int SceneDepZipStartIndex = 0;
		
		private static readonly string BundleDir = PathUnity.Combine(BuildBundlePath + BundleLoaderStrategy.EditorRoot, ApplicationHelper.platformFolder);
		private static readonly string BundleDir2 = PathUnity.Combine(Application.dataPath, BundleLoaderStrategy.EditorRoot);
		private static readonly string BundleDir3 = PathUnity.Combine(PathUnity.Combine(Application.dataPath, BundleLoaderStrategy.EditorRoot), ApplicationHelper.platformFolder);
		private static readonly string BundleSceneDir = Path.Combine(BundleDir, "scene/");
		
		/// <summary>
		/// 普通的场景名称
		/// </summary>
        private static readonly List<string> NormalSceneNames = new List<string> { "characterchoose", "characterselect", "gamestart", "loadscene", "prontera", "prt_1" };

		/// <summary>
		/// 普通的AssetBundle的路径列表
		/// </summary>
		private static List<string> m_NormalBundleNames;
		
		/// <summary>
		/// 额外场景的AssetBundle总大小
		/// </summary>
		private static long m_ExtraSceneBundlesSize;

		/// <summary>
		/// 额外场景的AssetBundle信息列表
		/// </summary>
		private static List<NeedZippedFileInfo> m_ExtraSceneBundles;
		
		/// <summary>
		/// 额外场景依赖的AssetBundle总大小
		/// </summary>
		private static long m_ExtraSceneDepBundlesSize;

		/// <summary>
		/// 额外场景依赖的AssetBundle信息列表
		/// </summary>
		private static List<NeedZippedFileInfo> m_ExtraSceneDepBundles;

		/// <summary>
		/// 根据每个包的大小压缩文件
		/// </summary>
		/// <param name="averageSize">每个包的大小</param>
		/// <param name="path">路径</param>
		/// <param name="targetPath">目标路径</param>
		/// <returns>压缩包</returns>
		public static List<GreedyAvarageZip> StartByZipFileSize (long averageSize, string path = null, string targetPath = null)
		{
			if (averageSize > 0)
			{
				//遍历Bundle文件
				TraverseAllBundleFiles(path);
				
				//将普通AssetBundle拷贝到StreamingAssets目录下面
				CopyBundles.CopyAssetBundles(m_NormalBundleNames, BundleDir, "Assets/StreamingAssets");
				
				//压缩
				var zips = new List<GreedyAvarageZip>();
				GreedyAvarageZip.index = SceneDepZipStartIndex;
				CompressSceneDependenciesZip(zips, (int) (m_ExtraSceneDepBundlesSize / averageSize), path, targetPath);
				//GreedyAvarageZip.index = SceneZipStartIndex;
				CompressSceneZip(zips, (int) (m_ExtraSceneBundlesSize / averageSize), path, targetPath);
				
				//清除进度条
				EditorUtility.ClearProgressBar();
				
				return zips;
			}
			return null;
		}
		
		/// <summary>
		/// 根据压缩包大小拆分AssetBundle
		/// </summary>
		/// <param name="averageSize">每个包的大小</param>
		/// <param name="path">路径</param>
		/// <param name="targetPath">目标路径</param>
		/// <returns>压缩包</returns>
		public static List<GreedyAvarageZip> SplitByZipFileSize(long averageSize, string path = null, string targetPath = null)
		{
			if (averageSize > 0)
			{
				if (string.IsNullOrEmpty(targetPath) && !string.IsNullOrEmpty(path))
				{
					targetPath = Directory.GetParent(path).FullName + "/";
				}
				
				//遍历Bundle文件
				TraverseAllBundleFiles(path);
				
				var splitSceneDepZips = Distrube(m_ExtraSceneDepBundles, (int) (m_ExtraSceneDepBundlesSize / averageSize), targetPath);
				var splitSceneZips = DistrubeScene(m_ExtraSceneBundles, targetPath);
				if (splitSceneDepZips == null)
				{
					return splitSceneZips;
				}

				if (splitSceneZips != null)
				{
					splitSceneDepZips.AddRange(splitSceneZips);
				}
				return splitSceneDepZips;
			}
			return null;
		}

		/// <summary>
		/// b遍历所有bundle文件
		/// </summary>
		private static void TraverseAllBundleFiles(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				path = BundleDir;
			}
			
			m_NormalBundleNames = new List<string>();
			m_ExtraSceneBundlesSize = 0;
			m_ExtraSceneBundles = new List<NeedZippedFileInfo>();
			m_ExtraSceneDepBundlesSize = 0;
			m_ExtraSceneDepBundles = new List<NeedZippedFileInfo>();
			
			var manifestBundle = AssetBundle.LoadFromFile(Path.Combine(BundleDir3, ApplicationHelper.platformFolder));
			var manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
			manifestBundle.Unload(false);

			var extraSceneBundleNames = new List<string>();
			var normalSceneDepBundleNames = new List<string>();
			var filePaths = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
			foreach (var filePath in filePaths)
			{
				if (Path.GetFileName(filePath) == ROPathConfig.VersionFileName)
				{
					continue;
				}

				var fileExtension = Path.GetExtension(filePath);
				if (string.IsNullOrEmpty(fileExtension) || fileExtension.Equals(".unity3d") ||
				    fileExtension.Equals(".manifest") 	|| fileExtension.Equals(".mp4") ||
				    fileExtension.Equals(".xml") 		|| fileExtension.Equals(".txt"))
				{
					var bundleName = filePath.Replace(BundleDir + "/", "");
					if (filePath.Contains(BundleSceneDir))
					{//场景文件
						if (NormalSceneNames.Contains(ParseSceneFolderName(bundleName)))
						{//普通场景
							m_NormalBundleNames.Add(bundleName);
							if (!string.IsNullOrEmpty(fileExtension) && fileExtension.Equals(".unity3d"))
							{
								var dependencies = manifest.GetAllDependencies(bundleName);
								normalSceneDepBundleNames.AddRange(dependencies);
							}
						}
						else
						{//额外场景
							extraSceneBundleNames.Add(bundleName);
						}
					}
					else
					{//非场景文件
						m_NormalBundleNames.Add(bundleName);
					}
				}
			}

			var extraSceneDepBundleNames = new List<string>();
			foreach (var bundleName in extraSceneBundleNames)
			{
				var zipInfo = new NeedZippedFileInfo(Path.Combine(BundleDir, bundleName));
				m_ExtraSceneBundles.Add(zipInfo);
				m_ExtraSceneBundlesSize += zipInfo.fileSize;
				
				var dependencies = manifest.GetAllDependencies(bundleName);
				foreach (var dependency in dependencies)
				{
					if (normalSceneDepBundleNames.Contains(dependency) || extraSceneDepBundleNames.Contains(dependency))
					{
						continue;
					}
					extraSceneDepBundleNames.Add(dependency);
					
					var dependencyManifest = dependency + ".manifest";
					m_NormalBundleNames.Remove(dependency);
					m_NormalBundleNames.Remove(dependencyManifest);

					var dependencyZipInfo = new NeedZippedFileInfo(Path.Combine(BundleDir, dependency));
					m_ExtraSceneDepBundles.Add(dependencyZipInfo);
					m_ExtraSceneDepBundlesSize += dependencyZipInfo.fileSize;
						
					var dependencyManifestZipInfo = new NeedZippedFileInfo(Path.Combine(BundleDir, dependencyManifest));
					m_ExtraSceneDepBundles.Add(dependencyManifestZipInfo);
					m_ExtraSceneDepBundlesSize += dependencyManifestZipInfo.fileSize;
				}
			}
		}

		/// <summary>
		/// 解析场景文件夹名称
		/// </summary>
		private static string ParseSceneFolderName(string path, bool isFullPath = false)
		{
			if (isFullPath)
			{
				var sceneSubPath = path.Replace(BundleDir3 + "/scene/", "");
				return sceneSubPath.Substring(0, sceneSubPath.IndexOf('/', 0));
			}
			else
			{
				var sceneSubPath = string.Format("_{0}", path).Replace("_scene/", string.Empty);
				return sceneSubPath.Substring(0, sceneSubPath.IndexOf('/', 0));
			}
		}

		/// <summary>
		/// 压缩场景文件
		/// </summary>
		private static void CompressSceneZip(List<GreedyAvarageZip> finalZips, string path = null, string targetPath = null)
		{
			if (string.IsNullOrEmpty(targetPath) && !string.IsNullOrEmpty(path))
			{
				targetPath = Directory.GetParent(path).FullName + "/";
			}
			
			var splitZips = DistrubeScene(m_ExtraSceneBundles, targetPath);
			if (splitZips != null)
			{
				CompressZips(splitZips, targetPath);
				finalZips.AddRange(splitZips);
			}
			
			m_ExtraSceneBundles = null;
		}

        private static void CompressSceneZip(List<GreedyAvarageZip> finalZips, int count, string path = null, string targetPath = null)
        {
            if (string.IsNullOrEmpty(targetPath) && !string.IsNullOrEmpty(path))
            {
                targetPath = Directory.GetParent(path).FullName + "/";
            }

            var splitZips = Distrube(m_ExtraSceneBundles, count == 0 ? 1 : count, targetPath);
            if (splitZips != null)
            {
                CompressZips(splitZips, targetPath);
                finalZips.AddRange(splitZips);
            }

            m_ExtraSceneBundles = null;
        }
		
		/// <summary>
		/// 压缩场景依赖文件
		/// </summary>
		private static void CompressSceneDependenciesZip(List<GreedyAvarageZip> finalZips, int count, string path = null, string targetPath = null)
		{
			if (string.IsNullOrEmpty(targetPath) && !string.IsNullOrEmpty(path))
			{
				targetPath = Directory.GetParent(path).FullName + "/";
			}
			
			var splitZips = Distrube(m_ExtraSceneDepBundles, count == 0 ? 1 : count, targetPath);
			if (splitZips != null)
			{
				CompressZips(splitZips, targetPath);
				finalZips.AddRange(splitZips);
			}
			
			m_ExtraSceneDepBundles = null;
		}
		
		/// <summary>
		/// 压缩所有Zips
		/// </summary>
		private static void CompressZips (List<GreedyAvarageZip> splitZips, string path)
		{
			if (splitZips != null)
			{
				foreach (var zip in splitZips)
				{
					CompressZip (zip, path);
				}
			}
		}

		/// <summary>
		/// 压缩单独的Zip
		/// </summary>
		private static void CompressZip (GreedyAvarageZip greedyAverageZip, string path)
		{
			if (string.IsNullOrEmpty (path))
			{
				path = BundleDir2;
			}

			for (var i = 0; i < greedyAverageZip.files.Count; i++)
			{
				var file = greedyAverageZip.files[i];
				ZipUtil.compressFile (file.fileName, path, CompressLevel, greedyAverageZip.zipName, null, true, false);
				
				EditorUtility.DisplayProgressBar("Zip AssetBundle To StreamingAssets", string.Format("Bundle Name: {0}", file.fileName), (float)(i + 1) / greedyAverageZip.files.Count);
			}
		}

		/// <summary>
		/// 倒序贪婪
		/// </summary>
		private static List<GreedyAvarageZip> Distrube(List<NeedZippedFileInfo> zipFileInfos, int count, string path = null)
		{
			if (zipFileInfos.Count < count)
			{
				return null;
			}
			
			//先将list按照从大到小排序
			zipFileInfos.Sort(Compare);
			
			//保存分组的总和,方便根据其值进行从小到大排序
			var sumList = new List<GreedyAvarageZip>();
			for (var index = 0; index < count; index++)
			{
				sumList.Add(new GreedyAvarageZip(path));
			}
			
			//拆分Zip包
			for (var i = 0; i < zipFileInfos.Count; i += count)
			{
				for (var j = 0; j < count; ++j)
				{
					var index = i + j;
					if (index < zipFileInfos.Count)
					{
						var greedyZip = sumList[j];
						greedyZip.AddNeedZippedFile(zipFileInfos[index]);
						continue;
					}
					break;
				}
			}
			sumList.Sort(CompareZips);
			return sumList;
		}

		private static List<GreedyAvarageZip> DistrubeScene(List<NeedZippedFileInfo> zipFileInfos, string path = null)
		{
			if (zipFileInfos.Count == 0)
			{
				return null;
			}
			
			//先将list按照从大到小排序
			zipFileInfos.Sort(Compare);

			//提取manifest包
			var manifestZipInfos = new Dictionary<string, NeedZippedFileInfo>();
			for (var index = zipFileInfos.Count - 1; index >= 0; --index)
			{
				var zipInfo = zipFileInfos[index];
				if (zipInfo.fileName.EndsWith(".unity3d.manifest", StringComparison.Ordinal))
				{
					manifestZipInfos.Add(zipInfo.fileName, zipInfo);
					zipFileInfos.RemoveAt(index);
				}
			}
			
			//拆分Zip包
			var greedyZipInfos = new Dictionary<string, GreedyAvarageZip>();
			foreach (var sceneZipInfo in zipFileInfos)
			{
				var subPath1 = sceneZipInfo.fileName.Replace(".unity3d", "");
				var subPath2 = subPath1.Substring(0, subPath1.LastIndexOf("/", StringComparison.Ordinal));
				var sceneFolderName = subPath2.Substring(subPath2.LastIndexOf("/", StringComparison.Ordinal));
				
				GreedyAvarageZip greedyZipInfo;
				if (!greedyZipInfos.TryGetValue(sceneFolderName, out greedyZipInfo))
				{
					var zipName = subPath1.Substring(subPath1.LastIndexOf("/", StringComparison.Ordinal) + 1);
					greedyZipInfo = new GreedyAvarageZip(path, zipName);
					greedyZipInfos.Add(sceneFolderName, greedyZipInfo);
				}
				greedyZipInfo.AddNeedZippedFile(sceneZipInfo);
				greedyZipInfo.AddNeedZippedFile(manifestZipInfos[sceneZipInfo.fileName + ".manifest"]);
			}
			
			//保存分组的总和,方便根据其值进行从小到大排序
			var sumList = new List<GreedyAvarageZip>();
			foreach (var pair in greedyZipInfos)
			{
				sumList.Add(pair.Value);
			}
			sumList.Sort(CompareZips);
			return sumList;
		}
		
		/// <summary>
		/// 从大到小排序函数
		/// </summary>
		private static int Compare(NeedZippedFileInfo x, NeedZippedFileInfo y)
		{
			return y.fileSize.CompareTo(x.fileSize);
		}

		/// <summary>
		/// 从大到小排序函数
		/// </summary>
		private static int CompareZips (GreedyAvarageZip x, GreedyAvarageZip y)
		{
			return x.totalSize.CompareTo(y.totalSize);
		}
	}
} // namespace EditorTool*/
#endregion
