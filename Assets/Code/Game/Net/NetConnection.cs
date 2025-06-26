using UnityEngine;
using System.Collections;

namespace RO.Net
{
	/**
	 * Network base class of all connections
	 */ 
	public class NetConnection
	{
		// name of this connection
		protected string _name = "NET";
		public string Name {
			set {this._name = value;}
		}

		public int state {
			set;get;
		}

		// logger
		protected NetLogger _logger = new NetLogger();
		public NetLogger Logger {
			get {return this._logger;}
		}

		// handler
		protected NetHandler _handler = new NetHandler();
		public NetHandler Handler {
			get {return this._handler;}
		}

		// ctor
		public NetConnection()
		{
		}

		// is connected
		public virtual bool IsConnected()
		{
			return false;
		}
	}
}