using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace RO
{
	public class AssetManageConfig
	{
		bool _isInited;
		public int cachePoolMaxNum = 30;
		public List<AssetManageInfo> infos = new List<AssetManageInfo> ();
		[XmlIgnoreAttribute]
		public SDictionary<string,AssetManageInfo>
			maps = new SDictionary<string, AssetManageInfo> ();
		static XmlSerializer serializer = new XmlSerializer (typeof(AssetManageConfig));
	
		public static AssetManageConfig CreateByFile (string file)
		{
			if (File.Exists (file)) {
				return CreateByStr (File.ReadAllText (file));
			}
			return null;
		}

		public static AssetManageConfig CreateByStr (string content)
		{
			StringReader sr = new StringReader (content);
			AssetManageConfig res = (AssetManageConfig)serializer.Deserialize (sr);
			sr.Close ();
			sr.Dispose ();
			return res;
		}

		public void SaveToFile (string path)
		{
			if (File.Exists (path))
				File.Delete (path);
			TextWriter writer = new StreamWriter (path);
			serializer.Serialize (writer, this);
			writer.Close ();
		}

		public void Init ()
		{
			if (!_isInited) {
				_isInited = true;
				AssetManageInfo info = null;
				for (int i=0; i<infos.Count; i++) {
					info = infos [i];
					maps [info.path] = info;
				}
			}
		}

		public AssetManageInfo AddInfo (int id, string path, AssetManageMode assetMode, int assetLRUCount, AssetManageMode bundleMode, int bundleLRUCount, AssetEncryptMode encryption)
		{
			Init ();
			AssetManageInfo info = maps [path];
			if (info == null) {
				info = new AssetManageInfo ();
				infos.Add (info);
				maps [path] = info;
			}
			info.ResetInfo (id, path, assetMode, assetLRUCount, bundleMode, bundleLRUCount, encryption);
			return info;
		}

		public AssetManageInfo GetInfo (string key)
		{
			AssetManageInfo info = null;
			for (int i=0; i<infos.Count; i++) {
				info = infos [i];
				AssetManageInfo found = CheckInfo (key, info);
				if (found != null) {
					return found;
				}
			}
			return AssetManageInfo.Default;
		}

		AssetManageInfo CheckInfo (string key, AssetManageInfo info)
		{
			if (key.Contains (info.path.ToLower ())) {
				AssetManageInfo son = InnerGetInfo (key, info);
				if (son == null)
					return info;
				return son;
			}
			return null;
		}

		AssetManageInfo InnerGetInfo (string key, AssetManageInfo info)
		{
			AssetManageInfo sub = null;
			if (info.subs != null) {
				for (int i=0; i<info.subs.Count; i++) {
					sub = info.subs [i];
					AssetManageInfo found = CheckInfo (key, sub);
					if (found != null) {
						return found;
					}
				}
			}
			return null;
		}
	}
} // namespace RO
