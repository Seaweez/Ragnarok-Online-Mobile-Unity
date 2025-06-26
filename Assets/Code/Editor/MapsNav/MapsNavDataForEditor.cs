using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlRoot("AllMapsNavData")]
public class AllMapsNavData
{
	[XmlRoot("MapsNavDataForEditor")]
	public class MapsNavDataForEditor
	{
		[XmlElement("originMapID")]
		public int originMapID;
		[XmlElement("originMapName")]
		public string originMapName;
		[XmlElement("targetMapID")]
		public int targetMapID;
		[XmlElement("targetMapName")]
		public string targetMapName;
		[XmlElement("path")]
		public string path;
	}

	[XmlArray("data")]
	public MapsNavDataForEditor[] data;

	public void Serialize(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(AllMapsNavData));
		TextWriter tw = null;
		try
		{
			tw = new StreamWriter(path);
			serializer.Serialize(tw, this);
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

	public static AllMapsNavData Deserialize(string path)
	{
		AllMapsNavData retValue = null;
		FileStream fs = null;
		try
		{
			fs = new FileStream(path, FileMode.Open);
			bool fileIsValid = fs.Length > 0;
			if (fileIsValid)
			{
				XmlSerializer serializer = new XmlSerializer(typeof(AllMapsNavData));
				retValue = (AllMapsNavData)serializer.Deserialize(fs);
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