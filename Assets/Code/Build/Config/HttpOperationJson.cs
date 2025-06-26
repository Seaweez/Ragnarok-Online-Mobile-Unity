using UnityEngine;
using LitJson;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Ghost.Utils;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class HttpOperationJson
	{
		public const string FILENAME = "HttpOperationJson";
		static HttpOperationJson _Instance;
		static HttpOperationJson _InstanceInResource;
		
		public static HttpOperationJson Instance {
			get {
				if (_Instance == null) {
					// load from persistentDataPath
					_Instance = ReadFromPersistentDataPath ();
					if (_Instance == null || _Instance.data == null) {
						_Instance = ReadFromResourceFolder ();
					}
				}
				return _Instance;
			}
		}

		public static void ResetInstance ()
		{
			_Instance = null;
		}

		string _rawString;

		public string rawString {
			get {
				return _rawString;
			}
		}
	
		[SLua.DoNotToLua]
		JsonData
			_data;

		[SLua.DoNotToLua]
		public JsonData data {
			get {
				return _data;
			}
		}

		public HttpOperationJson ()
		{
		}

		public HttpOperationJson (JsonData data)
		{
			_data = data;
		}

		public void ParseByText (string text)
		{
			_rawString = text;
			try {
				_data = JsonMapper.ToObject (text);
			} catch (System.Exception e) {
				RO.LoggerUnused.Log (e);
			}
		}

		public static HttpOperationJson CreateFromResource (string file)
		{
			if (string.IsNullOrEmpty (file)) {
				file = FILENAME;
			}
			TextAsset ta = Resources.Load (file) as TextAsset;
			if (ta != null) {
				return CreateFromText (ta.text);
			}
			return null;
		}

		public static HttpOperationJson CreateFromFile (string file)
		{
			if (File.Exists (file)) {
				return CreateFromText (File.ReadAllText (file));
			}
			return null;
		}

		public static HttpOperationJson CreateFromText (string text)
		{
			HttpOperationJson json = new HttpOperationJson ();
			json.ParseByText (text);
			return json;
		}

		public static HttpOperationJson ReadFromPersistentDataPath ()
		{
			string folder = PathUnity.Combine (Application.persistentDataPath, ApplicationHelper.platformFolder);
			string fileInpersistent = PathUnity.Combine (folder, FILENAME + ".txt");
			return CreateFromFile (fileInpersistent);
		}

		public static HttpOperationJson ReadFromResourceFolder ()
		{
			if (_InstanceInResource == null) {
				_InstanceInResource = CreateFromResource (null);
			}
			return _InstanceInResource;
		}

		public void SaveToFile (string path)
		{
			if (_data != null) {
				if (File.Exists (path))
					File.Delete (path);
				string text = _data.ToJson ();
				RO.LoggerUnused.Log (text);
				File.WriteAllText (path, text, Encoding.UTF8);
			}
		}

		public void SaveToPersistentDataPath ()
		{
			string folder = PathUnity.Combine (Application.persistentDataPath, ApplicationHelper.platformFolder);
			string fileInpersistent = PathUnity.Combine (folder, FILENAME + ".txt");
			SaveToFile (fileInpersistent);
		}
	}

	[SLua.CustomLuaClassAttribute]
	public enum HttpOperationJsonState
	{
		OK = 0,
		LackOfParams = 1,
		ErrorGetServerVersion = 2,
		ErrorGetUpdateInfo = 3,
		ErrorClientMuchNewer = 4,
	}
} // namespace RO
