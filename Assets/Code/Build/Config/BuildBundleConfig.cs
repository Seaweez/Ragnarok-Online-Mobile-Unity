using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace RO.Config
{
	[SLua.CustomLuaClassAttribute]
	public class BuildBundleConfig
	{
		public int currentVersion = 1;
		public int maxDirectUpgradeVers = 5;
		public string appInstallUrl = "";
		public List<List<BuildBundleInfo>> infos = new List<List<BuildBundleInfo>> ();
		static XmlSerializer serializer = new XmlSerializer (typeof(BuildBundleConfig));
		[XmlIgnoreAttribute]
		public SDictionary<int, BuildBundleInfo>
			versionMap = new SDictionary<int, BuildBundleInfo> ();
		bool _isInited = false;

		public static BuildBundleConfig CreateByFile (string file)
		{
			if (File.Exists (file)) {
				return CreateByStr (File.ReadAllText (file));
			}
			return null;
		}

		public static BuildBundleConfig CreateByStr (string content)
		{
			StringReader sr = new StringReader (content);
			BuildBundleConfig res = (BuildBundleConfig)serializer.Deserialize (sr);
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

		public int InfosCount {
			get {
				return infos.Count;
			}
		}

		public int GetSubInfosCount (int i)
		{
			return infos [i].Count;
		}

		public void Init ()
		{
			if (!_isInited) {
				_isInited = true;
				BuildBundleInfo info = null;
				List<BuildBundleInfo> infoList = null;
				for (int i=0; i<infos.Count; i++) {
					infoList = infos [i];
					for (int j=0; j<infoList.Count; j++) {
						info = infoList [j];
						versionMap [info.version] = info;
					}
				}
			}
		}

		public int LastInfoNextVersion {
			get {
				BuildBundleInfo last = LastInfo;
				if (last != null)
					return last.version + 1;
				return 1;
			}
		}

		public BuildBundleInfo LastInfo {
			get {
				if (infos.Count > 0) {
					List<BuildBundleInfo> subInfos = infos [infos.Count - 1];
					if (subInfos.Count > 0) {
						return subInfos [subInfos.Count - 1];
					}
				}
				return null;
			}
		}

		public void AddInfo (int start, int end, int version, string serverVersion, bool forceUpdate)
		{
			Init ();
			if (versionMap [version] == null) {
				if (start > end) {
					RO.LoggerUnused.LogError ("svn拉版本，怎么能Start比end大呢");
					return;
				}
				if(LastInfo!=null && LastInfo.version >= version)
				{
					RO.LoggerUnused.LogError ("跟我扯犊子呢，怎么新的资源包版本号比以前小啊");
					return;
				}
				BuildBundleInfo info = new BuildBundleInfo ();
				info.startV = start;
				info.endV = end;
				info.version = version;
				info.serverVersion = serverVersion;
				info.forceUpdateApp = forceUpdate;
				if (forceUpdate) {
					// new list
					List<BuildBundleInfo> newAppVersions = CreateNewSubInfos ();
					newAppVersions.Add (info);
				} else {
					List<BuildBundleInfo> lastList = GetLastInfoList;
					if (lastList != null) {
						if (lastList.Count > 0) {
							BuildBundleInfo last = lastList [lastList.Count - 1];
							if (last.endV > start) {
								return;
							}
						}
						lastList.Add (info);
					}
				}
				versionMap [version] = info;
			}
		}

		List<BuildBundleInfo> GetLastInfoList {
			get {
				List<BuildBundleInfo> subInfos = null;
				bool needNewSub = true;
				if (infos.Count > 0) {
					subInfos = infos [infos.Count - 1];
					if (GetNeedDownLoadBundleInfo (subInfos) < maxDirectUpgradeVers) {
						needNewSub = false;
					}
				}
				if (needNewSub)
					subInfos = CreateNewSubInfos ();
				return subInfos;
			}
		}

		int GetNeedDownLoadBundleInfo (List<BuildBundleInfo> subInfos)
		{
			int num = 0;
			if (subInfos != null) {
				foreach (BuildBundleInfo info in subInfos) {
					if (info.forceUpdateApp == false)
						num++;
				}
			}
			return num;
		}

		List<BuildBundleInfo> CreateNewSubInfos ()
		{
			List<BuildBundleInfo> subInfos = new List<BuildBundleInfo> ();
			this.infos.Add (subInfos);
			return subInfos;
		}

		//根据当前版本号，返回需要更新的包信息
		public List<BuildBundleInfo> GetNeedDownLoads (int now)
		{
			if (now < currentVersion) {
				return GetNeedDownLoads (now, currentVersion);
			}
			return null;
		}

		public List<BuildBundleInfo> GetNeedDownLoads (int start, int end)
		{
			List<BuildBundleInfo> downloads = null;
			BuildBundleInfo download = null;
			if (start < end) {
				for (int i=0; i<infos.Count; i++) {
					download = SpawnDownload (start, end, infos [i]);
					if (download != null) {
						downloads = downloads != null ? downloads : new List<BuildBundleInfo> ();
						downloads.Add (download);
						if (download.forceUpdateApp) {
							return new List<BuildBundleInfo> (){download};
						}
					}
				}
			}
			return downloads;
		}

		BuildBundleInfo SpawnDownload (int start, int end, List<BuildBundleInfo> infos)
		{
			BuildBundleInfo download = null;
			bool needForceUpdate = false;
			List<BuildBundleInfo> fits = infos.FindAll ((i) => {
				if (i.version > start && i.version <= end) {
					if (i.forceUpdateApp)
						needForceUpdate = true;
					return true;
				}
				return false;
			});
			if (needForceUpdate) {
				download = new BuildBundleInfo ();
				download.forceUpdateApp = true;
				return download;
			}
			if (fits.Count > 0) {
				download = new BuildBundleInfo ();
				download.startV = fits [0].startV;
				download.endV = fits [fits.Count - 1].endV;
				download.version = fits [fits.Count - 1].version;
				return download;
			}
			return download;
		}
	}
} // namespace RO.Config
