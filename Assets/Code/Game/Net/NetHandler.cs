using System;

namespace RO.Net
{
	public class NetHandler
	{
		public delegate void Handle(byte[] bytes, int length);

//		// connect
//		private Action _handleConn = null;
//		public Action HandleConn {
//			get {return this._handleConn;}
//			set {
//				if (this._handleConn == null)
//					this._handleConn = value;
//			}
//		}
//
//		// disconnect
//		private Action _handleDisConnect = null;
//		public Action HandleDisConnect {
//			get {return this._handleDisConnect;}
//			set {
//				if (this._handleDisConnect == null)
//					this._handleDisConnect = value;
//			}
//		}

		// send
		private Action<NetProtocolID> _handleSend = null;
		public Action<NetProtocolID> HandleSend {
			get {return this._handleSend;}
			set {this._handleSend = value;}
		}

		// receive
		private Handle _handleReceive = null;
		public Handle HandleReceive {
			get {return this._handleReceive;}
			set {
				if (this._handleReceive == null)
					this._handleReceive = value;
			}
		}

		// ctor
		public NetHandler()
		{
		}
	}
}