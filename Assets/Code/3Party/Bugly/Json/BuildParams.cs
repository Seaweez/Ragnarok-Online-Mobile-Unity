using UnityEngine;
using System.Collections;
using LitJson;
using RO;
using System.IO;
using System.Text;
using Ghost.Utils;

public class BuildParams
{
	//Add Push
	public const string JPUSH_KEY = "JPush_Params";
	public const string NEED_JPUSH = "NeedJPush";
	public const string JPUSH_APP_KEY = "JPushAppKey";
	public const string JPUSH_IS_RELEASE = "JPushIsRelease";

	public static string FILENAME {
		get { 
			return "BuildParams";
		}
	}

	static BuildParams _Instance;
	static BuildParams _InstanceInResource;

	public static BuildParams Instance {
		get {
			if (_Instance == null) {
				// load from persistentDataPath
				_Instance = (BuildParams)ReadFromPersistentDataPath (FILENAME);
				if (_Instance == null || _Instance.data == null) {
					_Instance = (BuildParams)ReadFromResourceFolder (FILENAME);
				}
			}
			return _Instance;
		}
	}

	public static void ResetInstance ()
	{
		_Instance = null;
	}

	#region JPush

	public bool Get_JPush_Enable {
		get { 
			if (data != null) {
				JsonData jpushparams = data [JPUSH_KEY];
				if (jpushparams != null) {
					bool res = false;
					if (bool.TryParse (jpushparams [NEED_JPUSH].ToString (), out res)) {
						return res;
					}
				}
			}
			return false;
		}
	}

	public string Get_JPush_Appkey {
		get { 
			if (data != null) {
				JsonData jpushparams = data [JPUSH_KEY];
				if (jpushparams != null) {
					return jpushparams [JPUSH_APP_KEY].ToString ();
				}
			}
			return null;
		}
	}

	public bool Get_JPush_IsRelease {
		get { 
			if (data != null) {
				JsonData jpushparams = data [JPUSH_KEY];
				if (jpushparams != null) {
					bool res = false;
					if (bool.TryParse (jpushparams [JPUSH_IS_RELEASE].ToString (), out res)) {
						return res;
					}
				}
			}
			return false;
		}
	}

	#endregion


	string _rawString;

	public string rawString {
		get {
			return _rawString;
		}
	}

	JsonData
		_data;

	public JsonData data {
		get {
			return _data;
		}
	}

	public BuildParams ()
	{
	}

	public BuildParams (JsonData data)
	{
		_data = data;
	}

	public void ParseByText (string text)
	{
		_rawString = text;
		try {
			_data = JsonMapper.ToObject (text);
		} catch (System.Exception ) {
		}
	}

	public static BuildParams CreateFromResource (string file)
	{
		TextAsset ta = Resources.Load (file) as TextAsset;
		if (ta != null) {
			return CreateFromText (ta.text);
		}
		return null;
	}

	public static BuildParams CreateFromFile (string file)
	{
		if (File.Exists (file)) {
			return CreateFromText (File.ReadAllText (file));
		}
		return null;
	}

	public static BuildParams CreateFromText (string text)
	{
		BuildParams json = new BuildParams ();
		json.ParseByText (text);
		return json;
	}

	public static BuildParams ReadFromPersistentDataPath (string fileName)
	{
		string folder = PathUnity.Combine (Application.persistentDataPath, ApplicationHelper.platformFolder);
		string fileInpersistent = PathUnity.Combine (folder, fileName + ".txt");
		return CreateFromFile (fileInpersistent);
	}

	public static BuildParams ReadFromResourceFolder (string fileName)
	{
		return CreateFromResource (fileName);
	}

	public void SaveToFile (string path)
	{
		if (_data != null) {
			if (File.Exists (path))
				File.Delete (path);
			string text = _data.ToJson ();
			File.WriteAllText (path, text, Encoding.UTF8);
		}
	}

	public void SaveToPersistentDataPath (string path)
	{
		string folder = PathUnity.Combine (Application.persistentDataPath, ApplicationHelper.platformFolder);
		string fileInpersistent = PathUnity.Combine (folder, path + ".txt");
		SaveToFile (fileInpersistent);
	}
}
