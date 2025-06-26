using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using RO;
using System;
using EditorTool;
using Ghost.Utils;
using System.Threading;

public static class AutoBuildAssetBundles
{
	const string buildBundlePath = "Assets/";

    [MenuItem("AssetBundle/Pure packaging (without scanning resources)")]
    public static void BuildAssetBundles()
    {
        Debug.Log("Start pure packaging...");

        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();

        // Check output path
        string outputPath = Path.Combine(buildBundlePath + BundleLoaderStrategy.EditorRoot, ApplicationHelper.platformFolder);
        Debug.Log("Output Path: " + outputPath);
        if (!Directory.Exists(outputPath))
        {
            Debug.Log("Creating directory: " + outputPath);
            Directory.CreateDirectory(outputPath);
        }

        try
        {
            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);
            Debug.Log("Asset bundles built successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error building asset bundles: " + ex.Message);
        }
        if (!Directory.Exists (outputPath))
			Directory.CreateDirectory (outputPath);
#if ARCHIVE_AB
		BuildPipeline.BuildAssetBundles (outputPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
#else
		BuildPipeline.BuildAssetBundles (outputPath, BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);
#endif
		RemoveAllUnusedBundles ();
		PrintCount ();
		// encrypt some bundles
		 //EncryptFiles ();
		// AppendVerifyToFileEnd
         //AppendVerify();
		
		stopWatch.Stop();
        Debug.Log(string.Format("Pure packaging completed in {0} seconds", stopWatch.Elapsed.TotalMilliseconds / 1000));
    }

	public static void RemoveAllUnusedBundles ()
	{
		string[] unusedBundles = AssetDatabase.GetUnusedAssetBundleNames ();
		string fileName = null;
		string dir = Path.Combine (Application.dataPath + "/" + BundleLoaderStrategy.EditorRoot, ApplicationHelper.platformFolder);
		string directory = null;
		SDictionary<string,string> removedFileDir = new SDictionary<string, string> ();
		for (int i = 0; i < unusedBundles.Length; i++) {
			fileName = Path.Combine (dir, unusedBundles [i]);
			//				Debug.Log (fileName);
			directory = Path.GetDirectoryName (fileName);
			removedFileDir [directory] = directory;
			DeleteFile (fileName);
			DeleteFile (fileName + ".meta");
			DeleteFile (fileName + ".manifest");
			DeleteFile (fileName + ".manifest.meta");
		}
		foreach (KeyValuePair<string,string> kvp in removedFileDir) {
			DeleteEmptyDirectory (kvp.Value);
		}
		removedFileDir.Clear ();
		removedFileDir = null;
		AssetDatabase.RemoveUnusedAssetBundleNames ();
	}

	static void EncryptFiles ()
	{
		AssetManageConfig config = AssetManageConfig.CreateByFile (EditorTool.AssetManagerConfigEditor.filePath);
		if (config != null) {
			foreach (AssetManageInfo info in config.infos) {
				RecurselyCheckConfig (info);
			}
		}
	}

	static void RecurselyCheckConfig (AssetManageInfo info)
	{
		if (info != null) {
			if (info.encryption != AssetEncryptMode.None) {
				Action<string> encryptFunc = null;
				switch (info.encryption) {
				case AssetEncryptMode.Encryption1:
					encryptFunc = EncryptEditor.AESEncrypt;
					break;
				}
				string bundleRoot = Path.Combine (buildBundlePath + BundleLoaderStrategy.EditorRoot, ApplicationHelper.platformFolder);
				string bundlesPath = Path.Combine (bundleRoot, info.path.ToLower ());
				if (encryptFunc != null) {
					// try filename
					encryptFunc (bundlesPath + ".unity3d");
					if (Directory.Exists (bundlesPath)) {
						foreach (string f in Directory.GetFiles(bundlesPath, "*", SearchOption.AllDirectories)) {
							if (Path.GetExtension (f) == ".unity3d") {
								encryptFunc (f);
							}
						}
					}
				}
			}
		}
		if (info.subs != null) {
			foreach (AssetManageInfo i in info.subs) {
				RecurselyCheckConfig (i);
			}
		}
	}

	public static void AppendVerify()
	{

	}

	public static void DeleteFile (string bundleName)
	{
		if (File.Exists (bundleName)) {
			File.Delete (bundleName);
		}
	}
		
	public static void DeleteEmptyDirectory (string directory)
	{
		if (Directory.Exists (directory)) {
			string[] files = Directory.GetFiles (directory, "*", SearchOption.AllDirectories);
			if (files.Length == 0) {
				Directory.Delete (directory);
			}
		}
	}

	[MenuItem("AssetBundle/Number of output bundles")]
	static void PrintCount ()
	{
		string outputPath = Path.Combine (Application.dataPath + "/" + BundleLoaderStrategy.EditorRoot, ApplicationHelper.platformFolder);
		outputPath = Path.Combine (outputPath, ApplicationHelper.platformFolder);
		Debug.Log (outputPath);
		AssetBundle ab = AssetBundle.LoadFromFile (outputPath);
		AssetBundleManifest am = ab.LoadAsset<AssetBundleManifest> ("AssetBundleManifest");
		string[] all = am.GetAllAssetBundles ();
		Debug.Log (string.Format ("一共有{0}个assetbundle包", all.Length));
		string[] arts = Array.FindAll (all, (p) => {
			return p.StartsWith ("art/");
		});
		Debug.Log (string.Format ("一共有{0}个art assetbundle包", arts.Length));
		ab.Unload (true);

	}

	[MenuItem("AssetBundle/Check Incompatible Shaders")]
	public static void CheckIncompatibleShaders()
	{
		// Create a dictionary to store shader names and associated materials
		Dictionary<string, List<string>> shaderMaterials = new Dictionary<string, List<string>>();

		// Get all the materials in the game
		Material[] materials = Resources.FindObjectsOfTypeAll<Material>();

		// Loop through each material
		foreach (Material mat in materials)
		{
			if (mat.shader != null)
			{
				string shaderName = mat.shader.name;

				// Check if the shader uses any deprecated or incompatible features
				// Note: Replace this condition with your specific checks
				if (shaderName.Contains("Deprecated") || shaderName.Contains("Incompatible"))
				{
					if (!shaderMaterials.ContainsKey(shaderName))
					{
						shaderMaterials[shaderName] = new List<string>();
					}
					shaderMaterials[shaderName].Add(mat.name);
				}
			}
		}

		// Display the incompatible shaders and the materials that use them in the console
		foreach (KeyValuePair<string, List<string>> kvp in shaderMaterials)
		{
			Debug.Log("Incompatible Shader: " + kvp.Key);
			foreach (string materialName in kvp.Value)
			{
				Debug.Log("-- Material using this shader: " + materialName);
			}
		}
	}
}
