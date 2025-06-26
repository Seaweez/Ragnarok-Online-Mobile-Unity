using UnityEngine;
using UnityEditor;
using LitJson;
using System.Collections.Generic;
using RO;
using Ghost.Utils;

namespace EditorTool
{
	public static class HttpOperationJsonEditor
	{
		public static void CmdSetHttpJson ()
		{
			List<string> args = CommandArgs.GetCommandArgs ();
			if (args.Count > 1) {
				int urlCount = 3;
				int i = 0;
				List<string> urls = null;
				for (i = 0; i < urlCount; i++) {
					string url = args [i];
					if (string.IsNullOrEmpty (url) == false) {
						if (urls == null) {
							urls = new List<string> ();
						}
						urls.Add (url);
					}
				}
				List<string> elements = new List<string> ();
				for (i=urlCount; i<args.Count; i++) {
					elements.Add (args [i]);
				}
				SetHttpJson (urls, elements);
			}
		}
	
		public static void SetHttpJson (List<string> urls, List<string> elements)
		{
			HttpOperationJson old = HttpOperationJson.Instance;
			JsonData data = new JsonData ();
			if (urls != null && urls.Count > 0) {
				JsonData urlsData = new JsonData ();
				data ["urls"] = urlsData;
				foreach (string url in urls) {
					if (string.IsNullOrEmpty (url) == false && url.Contains ("http")) {
						urlsData.Add (url);
					}
				}
			} else if (old != null) {
				//use old
				data ["urls"] = old.data ["urls"];
			}

			if (elements != null && elements.Count > 0) {
				JsonData elementDatas = new JsonData ();
				data ["elements"] = elementDatas;
				foreach (string element in elements) {
					if (string.IsNullOrEmpty (element) == false && element.Contains (":")) {
						string[] keypair = element.Split (':');
						if (keypair.Length == 2) {
							string key = keypair [0];
							string value = keypair [1];
							elementDatas [key] = value;
						} else {
						}
					}
				}
			}
			HttpOperationJson json = new HttpOperationJson (data);
			string path = PathUnity.Combine (Application.dataPath, string.Format ("Resources/{0}.txt", HttpOperationJson.FILENAME));
			json.SaveToFile (path);
			AssetDatabase.Refresh ();
		}

		[MenuItem( "AssetBundle/测试httpjson动态数据")]
		public static void Test ()
		{
			string url1 = "https://rebirth-nft.com:5556";
			string url2 = "https://rebirth-nft.com:5556";
			List<string> urls = new List<string> ();
			urls.Add (url1);
			urls.Add (url2);
			
			List<string> elements = new List<string> ();
			elements.Add ("plat:1");
			SetHttpJson (urls, elements);
		}
	}
} // namespace EditorTool
