using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using Ghost.Extensions;
using Ghost.Utils;

namespace EditorTool
{
	public class AssetInfo : ScriptableObject
	{
		private static Dictionary<string, AssetInfo> assetInfos = new Dictionary<string, AssetInfo>();
		public static AssetInfo GetInstance(string name)
		{
			AssetInfo info;
			if (!assetInfos.TryGetValue(name, out info))
			{
				var path = string.Format("Assets/Art/AssetInfo_{0}.asset", name);
				info = AssetDatabase.LoadAssetAtPath<AssetInfo>(path);
				if (null == info)
				{
					info = ScriptableObject.CreateInstance<AssetInfo>();
					AssetDatabase.CreateAsset (info, path);
				}
				assetInfos.Add(name, info);
			}
			return info;
		}

		private SHA1 sha1 = SHA1.Create();
		private string ComputeHash(string filePath)
		{
			FileStream stream = null;
			try
			{
				stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
				if (stream.CanRead)
				{
					var bytes = sha1.ComputeHash(stream);
					if (!bytes.IsNullOrEmpty())
					{
						var sb = new StringBuilder();  
						foreach (var b in bytes)
						{  
							sb.Append(b.ToString("x2"));  
						}
						return sb.ToString();
					}
				}
			}
			catch (IOException)
			{

			}
			finally
			{
				if (null != stream)
				{
					stream.Close();
				}
			}
			return null;
		}

		public static bool IsModelFBX(string path)
		{
			var fileName = Path.GetFileName(path);
			return string.Equals("fbx", Path.GetExtension(fileName).TrimStart('.').ToLower());
		}

		private static string AssetPathStart = "Assets/";
		private static string GetFilePath(string path)
		{
			if (path.StartsWith(AssetPathStart))
			{
				path = path.Substring(AssetPathStart.Length);
			}
			return PathUnity.Combine(Application.dataPath, path);
		}

		[System.Serializable]
		public class Info
		{
			public string path;
			public GameObject asset;
			public string hash;

			public string newHash{get;set;}
			public bool handled{get;set;}
		}

		public Info[] infos;

		private Dictionary<string, Info> infoMap = new Dictionary<string, Info>();

		public bool running{get;private set;}

		#region optimize version
//		public GameObject[] reimportedAssets;
//
//		private Dictionary<string, Info> changedInfoMap = new Dictionary<string, Info>();
//		private Dictionary<string, Info> handledInfoMap = new Dictionary<string, Info>();
//
//		public void AddReimportedAsset(string path)
//		{
//			if (infos.IsNullOrEmpty())
//			{
//				return;
//			}
//
//			var info = ArrayUtility.Find(infos, delegate(Info obj) {
//				return string.Equals(obj.path, path);
//			});
//
//			if (!reimportedAssets.IsNullOrEmpty())
//			{
//				if (ArrayUtility.Contains(reimportedAssets, info.asset))
//				{
//					return;
//				}
//			}
//
//			if (null != info)
//			{
//				ArrayUtility.Add(ref reimportedAssets, info.asset);
//			}
//		}
//
//		public void Begin()
//		{
//			if (running)
//			{
//				return;
//			}
//			infoMap.Clear();
//			changedInfoMap.Clear();
//			handledInfoMap.Clear();
//
//			if (!infos.IsNullOrEmpty())
//			{
//				foreach (var info in infos)
//				{
//					if (null != info && null != info.asset)
//					{
//						infoMap[info.path] = info;
//					}
//				}
//			}
//
//			if (!reimportedAssets.IsNullOrEmpty())
//			{
//				foreach (var asset in reimportedAssets)
//				{
//					if (null != asset)
//					{
//						var path = AssetDatabase.GetAssetPath(asset);
//						var filePath = GetFilePath(path);
//						var hash = ComputeHash(filePath);
//
//						var info = infoMap[path];
//						if (null != info)
//						{
//							if (string.Equals(info.hash, hash))
//							{
//								continue;
//							}
//						}
//						else
//						{
//							info = new Info();
//						}
//						info.path = path;
//						info.asset = asset;
//						info.hash = hash;
//
//						changedInfoMap[path] = info;
//					}
//				}
//			}
//			running = true;
//		}
//
//		// asset may be set null
//		// return asset changed
//		public bool GetAsset(string path, out GameObject asset)
//		{
//			if (!running)
//			{
//				asset = null;
//				return false;
//			}
//			Info info;
//			if (changedInfoMap.TryGetValue(path, out info))
//			{
//				asset = info.asset;
//				return true;
//			}
//
//			if (infoMap.TryGetValue(path, out info))
//			{
//				asset = info.asset;
//				if (null != asset)
//				{
//					if (IsModelFBX(path))
//					{
//						return false;
//					}
//					var filePath = GetFilePath(path);
//					var hash = ComputeHash(filePath);
//					if (string.Equals(info.hash, hash))
//					{
//						return false;
//					}
//					info.hash = hash;
//				}
//			}
//
//			asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
//			if (null != asset)
//			{
//				info = new Info();
//				info.path = path;
//				info.asset = asset;
//				changedInfoMap[path] = info;
//			}
//			return true;
//		}
//
//		public void SetAssetHandled(string path)
//		{
//			if (!running)
//			{
//				return;
//			}
//			Info info;
//			if (changedInfoMap.TryGetValue(path, out info))
//			{
//				handledInfoMap[path] = info;
//			}
//		}
//
//		public void End()
//		{
//			if (!running)
//			{
//				return;
//			}
//			foreach (var key_value in changedInfoMap)
//			{
//				var path = key_value.Key;
//				var info = key_value.Value;
//				if (string.IsNullOrEmpty(info.hash))
//				{
//					var filePath = GetFilePath(path);
//					info.hash = ComputeHash(filePath);
//				}
//				infoMap[path] = info;
//			}
//			infos = infoMap.Values.ToArray();
//			foreach (var path in handledInfoMap.Keys)
//			{
//				changedInfoMap.Remove(path);
//			}
//			ArrayUtility.Clear(ref reimportedAssets);
//			foreach (var info in changedInfoMap.Values)
//			{
//				ArrayUtility.Add(ref reimportedAssets, info.asset);
//			}
//
//			infoMap.Clear();
//			changedInfoMap.Clear();
//			handledInfoMap.Clear();
//			running = false;
//		}
		#endregion optimize version

		#region safe version

		public void Begin()
		{
			if (running)
			{
				return;
			}
			infoMap.Clear();

			if (!infos.IsNullOrEmpty())
			{
				foreach (var info in infos)
				{
					info.asset = AssetDatabase.LoadAssetAtPath<GameObject>(info.path);
					if (null != info.asset)
					{
						info.newHash = null;
						info.handled = false;
						infoMap[info.path] = info;
					}
				}
			}

			running = true;
		}

		// asset may be set null
		// return asset changed
		public bool GetAsset(string path, out GameObject asset)
		{
			if (!running)
			{
				asset = null;
				return false;
			}

			Info info;
			if (infoMap.TryGetValue(path, out info))
			{
				if (string.IsNullOrEmpty(info.newHash))
				{
					var filePath = GetFilePath(path);
					info.newHash = ComputeHash(filePath);
				}

				if (string.Equals(info.hash, info.newHash))
				{
					asset = info.asset;
					return false;
				}
			}

			asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
			return true;
		}

		public void SetAssetHandled(string path, GameObject asset)
		{
			if (!running)
			{
				return;
			}
			Info info;
			if (!infoMap.TryGetValue(path, out info))
			{
				info = new Info();
				info.path = path;

				var filePath = GetFilePath(path);
				info.newHash = ComputeHash(filePath);

				infoMap[path] = info;
			}
			info.asset = asset;
//			info.hash = info.newHash;
			info.handled = true;
		}

		public void End()
		{
			if (!running)
			{
				return;
			}
			infos = infoMap.Values.ToArray();
			foreach (var info in infos)
			{
				if (info.handled)
				{
					info.hash = info.newHash;
					info.handled = false;
				}
				else
				{
					info.newHash = null;
				}
			}

			infoMap.Clear();
			running = false;

			EditorUtility.SetDirty(this);
		}
		#endregion safe version
	}
} // namespace EditorTool
