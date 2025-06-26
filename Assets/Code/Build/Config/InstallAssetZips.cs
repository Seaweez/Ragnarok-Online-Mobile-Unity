using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RO
{
	public class InstallAssetZips : XMLSerializedClass<InstallAssetZips>
	{
		[XmlAttribute ("ZipName")]
		public string
			zipName;
		[XmlAttribute ("ZipSize")]
		public long
			size;
		[XmlAttribute ("UnZipSize")]
		public long
			unzipSize;
		[XmlAttribute ("UnZipFilesCount")]
		public long
			unZipFilesCount;
		public List<InstallAssetZips> names;
		static InstallAssetZips _Instance;

		public static InstallAssetZips Instance {
			get {
				if (_Instance == null) {
					TextAsset ta = Resources.Load ("InstallAssetZips") as TextAsset;
					if (ta != null) {
						_Instance = InstallAssetZips.CreateByStr (ta.text);
					}
				}
				return _Instance;
			}
		}

		public void AddInfo (string zipName, long size, long upZipSize)
		{
			if (names == null) {
				names = new List<InstallAssetZips> ();
			}
			InstallAssetZips zip = names.Find ((v) => {
				return v.zipName == zipName;
			});
			if (zip == null) {
				zip = new InstallAssetZips ();
				zip.zipName = zipName;
				names.Add (zip);
			}
			zip.size = size;
			zip.unzipSize = upZipSize;
		}
	}
}
 // namespace RO
