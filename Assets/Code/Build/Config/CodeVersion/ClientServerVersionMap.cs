using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace RO.Config
{
	public class ClientServerVersionMap:XMLSerializedClass<ClientServerVersionMap>
	{
		public List<ClientServerMapInfo> infos = new List<ClientServerMapInfo> ();
		[XmlIgnoreAttribute]
		public SDictionary<int, ClientServerMapInfo>
			versionMap = new SDictionary<int, ClientServerMapInfo> ();
		bool _isInited = false;

		public int InfosCount {
			get {
				return infos.Count;
			}
		}

		public void Init ()
		{
			if (!_isInited) {
				_isInited = true;
				ClientServerMapInfo info = null;
				for (int i = 0; i < infos.Count; i++) {
					info = infos [i];
					versionMap [info.clientCodeVersion] = info;
				}
			}
		}

		void _AddInfo (int clientCode, string maxServerVersion,bool enabled)
		{
			ClientServerMapInfo info = new ClientServerMapInfo ();
			info.clientCodeVersion = clientCode;
			info.maxServerVersion = maxServerVersion;
			info.enabled = enabled;
			infos.Add (info);
			versionMap [info.clientCodeVersion] = info;
		}

		public void ModifyInfo (int clientCode, string maxServerVersion,bool enabled)
		{
			Init ();
			ClientServerMapInfo info = versionMap [clientCode];
			if (info == null) {
				_AddInfo (clientCode, maxServerVersion,enabled);
			} else {
				if (string.IsNullOrEmpty (maxServerVersion) == false && maxServerVersion != "0") {
					info.maxServerVersion = maxServerVersion;
				}
				info.enabled = enabled;
			}
		}
	}
}
// namespace RO.Config
