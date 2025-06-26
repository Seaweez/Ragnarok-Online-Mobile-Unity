using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RO.Config
{
	[SLua.CustomLuaClassAttribute]
	public class BuildBundleInfo
	{
		[XmlAttribute("SVN_Start")] 
		public int
			startV = -1;
		[XmlAttribute("SVN_End")] 
		public int
			endV;
		[XmlAttribute("ResVersion")] 
		public int
			version;
		[XmlAttribute("ServerVersion")] 
		public string
			serverVersion;
		[XmlAttribute("ForceUpdateApp")] 
		public bool
			forceUpdateApp;

		public override string ToString ()
		{
			return string.Format ("[BuildBundleInfo] startV:{0} endV:{1} version:{2} serverVersion:{3} forceUpdateApp:{4}", startV, endV, version, serverVersion, forceUpdateApp);
		}
	}
} // namespace RO.Config
