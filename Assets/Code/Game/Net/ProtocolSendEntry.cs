using UnityEngine;
using System.Collections;

namespace RO
{
	public class ProtocolSendEntry
	{
		public byte[] bytes {set;get;}
		public NetProtocolID pId{set;get;}


		public ProtocolSendEntry(byte[] data,NetProtocolID pId)
		{
			this.bytes = data;
			this.pId = pId;
		}
	}
}


