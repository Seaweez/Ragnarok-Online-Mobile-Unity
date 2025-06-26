using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Ghost.Utils;
using RO;
using RO.Config;

namespace EditorTool
{
    public static class CopyBundles
    {
        private static readonly string BundleDir = PathUnity.Combine("Assets/" + BundleLoaderStrategy.EditorRoot, ApplicationHelper.platformFolder);
        
        /// <summary>
        /// 开始拷贝AssetBundle
        /// </summary>
        public static void StartCopy()
        {
            var allBundleNames = TraverseAllBundleFiles();
            CopyAssetBundles(allBundleNames, BundleDir, "Assets/StreamingAssets");
        }

        /// <summary>
        /// 拷贝AssetBundle
        /// </summary>
        /// <param name="bundleNames">要拷贝的AssetBundle列表</param>
        /// <param name="srcPath">原路径</param>
        /// <param name="destPath">目标路径</param>
        public static void CopyAssetBundles(List<string> bundleNames, string srcPath, string destPath)
        {
            for (var i = 0; i < bundleNames.Count; ++i)
            {
                var bundleName = bundleNames[i];
                var src = Path.Combine(srcPath, bundleName);
                var dest = Path.Combine(destPath, bundleName);
                var destDir = Path.GetDirectoryName(dest);
                if (string.IsNullOrEmpty(destDir))
                {
                    Debug.LogError("Copy Bundles Error path = " + src);
                    break;
                }

                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }
                File.Copy(src, dest);
                
                EditorUtility.DisplayProgressBar("Copy AssetBundle To StreamingAssets", string.Format("Bundle Name: {0}", bundleName), (float)(i + 1) / bundleNames.Count);
            }
            
            EditorUtility.ClearProgressBar();
            
            Debug.LogError("Copy AssetBundle Count = " + bundleNames.Count);
        }
        
        private static List<string> TraverseAllBundleFiles(string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = BundleDir;
            }

            var allBundleNames = new List<string>();
            var filePaths = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                if (Path.GetFileName(filePath) == ROPathConfig.VersionFileName)
                {
                    continue;
                }

                var fileStandardPath = filePath.Replace("\\", "/");
                var fileExtension = Path.GetExtension(fileStandardPath);
                if (string.IsNullOrEmpty(fileExtension) || fileExtension.Equals(".unity3d") ||
                    fileExtension.Equals(".mp4") 	    || fileExtension.Equals(".xml") ||
                    fileExtension.Equals(".txt")        || fileExtension.Equals(".manifest"))
                {
                    allBundleNames.Add(fileStandardPath.Replace(BundleDir + "/", ""));   
                }
            }
            return allBundleNames;
        }
    }
}