using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System;

public class MyXmlSerializer
{
	/// <summary>
	/// serialize object to disk with format xml.
	/// </summary>
	public static void Serialize<T>(T t, string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(T));
		TextWriter tw = null;
		try
		{
			tw = new StreamWriter(FileDirectoryHandler.GetAbsolutePath(path));
			serializer.Serialize(tw, t);
		}
		catch (Exception e)
		{
			Debug.LogWarning (e);
			throw e;
		}
		finally
		{
			if (tw != null) {
				tw.Close ();
			}
		}
	}

	public static T Deserialize<T>(string path)
	{
		T retValue = default(T);
		FileStream fs = null;
		try
		{
			fs = new FileStream(FileDirectoryHandler.GetAbsolutePath(path), FileMode.Open);
			bool fileIsValid = fs.Length > 0;
			if (fileIsValid)
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				retValue = (T)serializer.Deserialize(fs);
			}
		}
		catch (Exception e) {
			Debug.LogWarning (e);
			throw e;
		}
		finally
		{
			if (fs != null) {
				fs.Close ();
			}
		}
		return retValue;
	}
}
