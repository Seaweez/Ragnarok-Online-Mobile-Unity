using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using System.Text;
using System;

namespace RO
{
	public class ExtractFilesCheck
	{
		const string INSTALL_ABZIPS_FILE_NAME_PREFIX = "InstallABZipMD5Info";
		public const string DOWNLOAD_ABZIPS_FILE_NAME_PREFIX = "AssetMD5Verify.txt";

		public static void SaveInstallABZipMD5Info (ABZipMD5Infos info)
		{
			string fileName = INSTALL_ABZIPS_FILE_NAME_PREFIX + ".txt";
			string path = Path.Combine (Path.Combine (Application.dataPath, "Resources"), fileName);
			if (File.Exists (path)) {
				File.Delete (path);
			}
			JsonData data = new JsonData ();
			data ["ZipFileName"] = info.zipName;
			JsonData md5s = new JsonData ();
			data ["Infos"] = md5s;
			MD5Info md5Info;
			for (int i = 0; i < info.md5Infos.Count; i++) {
				md5Info = info.md5Infos [i];
				JsonData subInfo = new JsonData ();
				subInfo ["FileName"] = md5Info.fileName;
				subInfo ["MD5"] = md5Info.md5;
				subInfo ["Size"] = md5Info.size;
				md5s.Add (subInfo);
			}
			string text = data.ToJson ();
			File.WriteAllText (path, text, Encoding.UTF8);
		}

		public static ABZipMD5Infos ReadABZipMD5InfosInResource ()
		{
			ABZipMD5Infos info = null;
			string fileName = INSTALL_ABZIPS_FILE_NAME_PREFIX;
			fileName = Path.GetFileNameWithoutExtension (fileName);
			TextAsset ta = Resources.Load<TextAsset> (fileName);
			if (ta != null && ta.text != null) {
				int res = 0;
				info = ParseZipMD5InfosByStr (ta.text, ref res);
			}
			return info;
		}

		/**
		 * res:  1 success
		 * 	-1 file content is empty
		 * 	-2 parse error
		 **/
		public static ABZipMD5Infos ParseZipMD5InfosByFile (string filePath, ref int res)
		{
			ABZipMD5Infos info = null;
			if (File.Exists (filePath)) {
				try {
					string content = File.ReadAllText (filePath);
					info = ParseZipMD5InfosByStr (content, ref res);
				} catch (Exception ) {
					res = -2;
					info = null;
				}
			} else {
				res = 1;
				info = null;
			}
			return info;
		}

		/**
		 * res:  1 success
		 * 	-1 content empty
		 * 	-2 parse error
		 **/
		public static ABZipMD5Infos ParseZipMD5InfosByStr (string content, ref int res)
		{
			ABZipMD5Infos info = null;
			if (string.IsNullOrEmpty (content)) {
				res = -1;
				return info;
			}
			try {
				JsonData data = JsonMapper.ToObject (content);
				string filename = data ["ZipFileName"].ToString ();
				JsonData md5s = data ["Infos"] as JsonData;
				if (md5s.IsArray) {
					info = new ABZipMD5Infos (filename);
					for (int k = 0; k < md5s.Count; k++) {
						JsonData subInfo = md5s [k];
						info.Add (subInfo ["FileName"].ToString (), subInfo ["MD5"].ToString (), long.Parse (subInfo ["Size"].ToString ()));
					}
				}
				res = 1;
			} catch (Exception ) {
				res = -2;
				info = null;
			}
			return info;
		}
	}
}

public class ABZipMD5Infos
{
	public string zipName { get; private set; }

	List<MD5Info> _md5Infos = new List<MD5Info> ();

	public List<MD5Info> md5Infos {
		get {
			return _md5Infos;
		}
	}

	public ABZipMD5Infos (string zipName)
	{
		this.zipName = zipName;
	}

	public void Add (string filename, string md5, long size)
	{
		MD5Info info = new MD5Info ();
		info.fileName = filename;
		info.md5 = md5;
		info.size = size;
		_md5Infos.Add (info);
	}
}

public struct MD5Info
{
	public string fileName;
	public string md5;
	public long size;
}