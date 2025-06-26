using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XCodeEditor;
using System;
using System.IO;
using System.Xml;

public class ROEntitlements
{
	string _filePath;
	XCPlist _plist;

	public ROEntitlements (string filePath)
	{
		_filePath = filePath;
		_plist = new XCPlist (filePath);
	}

	public void CreateFile ()
	{
		using (FileStream fs = File.Create (_filePath)) {
		}
	}

	public void Process (Hashtable hash)
	{
		if (hash == null)
			return;
		try {
			_plist.Process (hash);
		} catch (Exception ) {
			Dictionary<string, object> dict = new Dictionary<string, object> ();
			//可能是个空文件
			foreach (DictionaryEntry entry in hash) {
				_plist.AddPlistItems ((string)entry.Key, entry.Value, dict);
			}
			PlistCS.Plist.writeXml (dict, _filePath);
		}
	}

	public static void writeXml (object value, string path)
	{
		using (StreamWriter writer = new StreamWriter (path)) {
			writer.Write (PlistCS.Plist.writeXml (value, true));
		}
	}
}