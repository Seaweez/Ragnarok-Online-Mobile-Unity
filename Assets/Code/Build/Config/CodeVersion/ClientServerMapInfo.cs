using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RO.Config
{
//	[SLua.CustomLuaClassAttribute]
	public class ClientServerMapInfo
	{
		[XmlAttribute("ClientCodeVersion")] 
		public int
			clientCodeVersion = -1;
		[XmlAttribute("MaxServerVersion")] 
		public string
			maxServerVersion;
		[XmlAttribute("Enabled")] 
		public bool
			enabled;

		public override string ToString ()
		{
			return string.Format ("[ClientServerMapInfo] clientCodeVersion:{0} maxServerVersion:{1}", clientCodeVersion, maxServerVersion);
		}
	}
} // namespace RO.Config
