using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System;

namespace RO
{
	public class ClientVersionJsonData
	{
		public class AssetsUpdateInfo
		{
			string _url;
			long _size;
			string _clientVersion;
			string _md5;

			public string clientVersion {
				get {
					return _clientVersion;
				}
			}

			public long size {
				get {
					return _size;
				}
			}

			public string url {
				get {
					return _url;
				}
			}

			public string md5 {
				get {
					return _md5;
				}
			}

			public AssetsUpdateInfo (string url, long size, string clientVersion, string md5)
			{
				_url = url;
				_size = size;
				_clientVersion = clientVersion;
				_md5 = md5;
			}
		}

		JsonData _jsonData;
		JsonData _data;

		public JsonData jsonData {
			get {
				return _jsonData;
			}
		}

		public JsonData data {
			get {
				return _data;
			}
		}

		public string GetValue (string key)
		{
			if (_data != null && _data.Keys.Contains (key)) {
				return _data [key].ToString ();
			}
			return null;
		}

		public string clientVersion {
			get {
				return GetValue ("client");
			}
		}

		public string serverVersion {
			get {
				return GetValue ("server");
			}
		}

		public bool forceUpdateApp {
			get {
				string res = GetValue ("force");
				if (string.IsNullOrEmpty (res)) {
					return false;
				} 
				//后端没有用true false….用1和0
				return res == "1";
			}
		}

		public int notifyCode {
			get {
				string res = GetValue ("notifycode");
				if (string.IsNullOrEmpty (res) == false) {
					int code = -1;
					if (int.TryParse (res, out code)) {
						return code;
					}
				}
				return -1;
			}
		}

		public string notify {
			get {
				return GetValue ("notify");
			}
		}

		public List<AssetsUpdateInfo> infos {
			get {
				List<AssetsUpdateInfo> res = null;
				if (_data != null && _data.Keys.Contains ("urls")) {
					JsonData urls = _data ["urls"];
					if (urls != null && urls.IsArray) {
						res = new List<AssetsUpdateInfo> ();
						for (int i = 0; i < urls.Count; i++) {
							JsonData jsonInfo = urls [i];
							string md5 = jsonInfo.Keys.Contains("md5") ? jsonInfo ["md5"].ToString () : null;
							AssetsUpdateInfo info = new AssetsUpdateInfo (jsonInfo ["url"].ToString (), long.Parse (jsonInfo ["size"].ToString ()), jsonInfo ["client"].ToString (), md5);
							res.Add (info);
						}
					}
				}
				return res;
			}
		}

		public long size {
			get {
				string zipSize = GetValue ("size");
				if (string.IsNullOrEmpty (zipSize) == false) {
					long res = 0;
					if (long.TryParse (zipSize, out res)) {
						return res;
					}
				}
				return 0;
			}
		}

		public string humanReadSize {
			get {
				return humanReadableByteCount (size);
			}
		}

		public ClientVersionJsonData (JsonData jsonData)
		{
			_jsonData = jsonData;
			_data = _jsonData ["data"];
		}

		public static string humanReadableByteCount (long bytes)
		{
			int unit = 1024;
			if (bytes < unit)
				return bytes + " B";
			int exp = (int)(Math.Log (bytes) / Math.Log (unit));
			return String.Format ("{0:F1} {1}B", bytes / Math.Pow (unit, exp), "KMGTPE" [exp - 1]);
		}

	}
}
 // namespace RO
