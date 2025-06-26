using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class NetProtocolID
	{
		uint _id1;
		uint _id2;

		public uint id1 {
			get {
				return _id1;
			}
		}

		public uint id2 {
			get {
				return _id2;
			}
		}

		public NetProtocolID (uint id1,uint id2)
		{
			this._id1 = id1;
			this._id2 = id2;
		}
	}
} // namespace RO
