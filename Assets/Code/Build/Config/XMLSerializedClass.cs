using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace RO
{
	public class XMLSerializedClass<T> where T : class
	{
		static XmlSerializer serializer = new XmlSerializer (typeof(T));
	
		public static T CreateByFile (string file)
		{
			if (File.Exists (file)) {
				return CreateByStr (File.ReadAllText (file));
			}
			return null;
		}
		
		public static T CreateByStr (string content)
		{
			StringReader sr = new StringReader (content);
			T res = (T)serializer.Deserialize (sr);
			sr.Close ();
			sr.Dispose ();
			return res;
		}
		
		public virtual void SaveToFile (string path)
		{
			if (File.Exists (path))
				File.Delete (path);
			TextWriter writer = new StreamWriter (path);
			serializer.Serialize (writer, this);
			writer.Close ();
		}

	}
} // namespace RO
