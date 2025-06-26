using UnityEngine;
using System.Collections.Generic;
using RO;
using System;
using System.Collections;
using System.Threading;
using SLua;
using LitJson;
using System.Net;
using System.Net.Sockets;

namespace RO.Net
{
	/**
	 * Network manager
	 * Controller all connections
	 */ 
	[SLua.CustomLuaClassAttribute]	
	public class NetConnectionManager : SingleTonGO<NetConnectionManager>
	{
		public static NetConnectionManager Instance {
			get {
				return Me;
			}
		}

		public class QueryIpWraper{
			public string ip;
			public bool complete = false;
		}

		public bool EnableLog = true;
		public bool EnableEncrypt = false;
		public bool EnableNonce = false;

		public bool EnableSelfResolve = false;

		private NetLogger _logger = new NetLogger ();
		public int maxZibNum = 1024;
		public int compressLevel = 9;
		public  int connectTimeOut = 5000;
		public  int recvMaxLen = 5120;
		public  double uploadByteLength = 0f;
		public  double downloadbyteLength = 0f;

//		#region game connection
		// data received from game server 
		// for lua 
		// because of lua function can not be called in a not main thread

//		private long _gameSendNum = 0;
		
		// the lua function receive
		private LuaFunction _gameRecvFunc;
		
		// the max num of every frame deal with
		private JsonData _gameConfig = null;
		private JsonData _gameConfigPriority = null;
		private SDictionary<uint,SDictionary<uint,NetProtocolID>> _sendCallBackID = new SDictionary<uint, SDictionary<uint, NetProtocolID>> ();
		// current receive datas
		private object _gameReceivesLocker = new object ();
		private List<ProtocolEntry.GameReceive> _gameReceives = new List<ProtocolEntry.GameReceive> ();
		private Queue<List<ProtocolEntry.GameReceive>> receiveQueue = new Queue<List<ProtocolEntry.GameReceive>> ();

		private void GameAddReceives (ProtocolEntry.GameReceive receive)
		{
			//设定消息优先级（收到某消息后，发现配置中有高优先级，则插入到处理列表最前面）
			for (int i = 0, max = this._gameConfigPriority.Count; i < max; i++) {			
				var priority = this._gameConfigPriority [i];
				var id1 = (int)priority ["id1"];
				var id2 = (int)priority ["id2"];
				if (receive.id1 == id1 && receive.id2 == id2) {
					lock (_gameReceivesLocker) {
						this._gameReceives.Insert (0, receive);
					}
					return;
				}
			}

			lock (_gameReceivesLocker) {
				this._gameReceives.Add (receive);
			}
		}
		
		public bool GameIsReceiving {
			get {
				if (!this.GameServerConnection.IsConnected ())
					return false;
				
				return this._gameReceives.Count > 0;
			}
		}

		protected override void Awake ()
		{
			base.Awake ();
			GameServerConnection = new NetConnectionTCP (recvMaxLen);
		}
		
		// update function for lua
		private Action _gameUpdate;

		public Action GameUpdate {
			get { return this._gameUpdate;}
			set { this._gameUpdate = value;}
		}
		
		// game server connection
		private NetConnectionTCP GameServerConnection ;
		private NetConnectionTCP LoginServerConnection;
		private NetConnectionTCP curConnect;
		
		// protocl parser
		private ProtocolEntry.ProtocolBytes protocolBytes = new ProtocolEntry.ProtocolBytes ();
//		#endregion
		
		// Start
		void Start ()
		{
			// game handles
			this.GameServerConnection.Handler.HandleReceive = this.GameHandleReceive;
//			this.LoginServerConnection.Handler.HandleReceive = this.GameHandleReceive;
			
			this._gameRecvFunc = MyLuaSrv.Instance.GetFunction ("NetProtocol.Receive");
			this._gameConfig = NetJsonUtil.GetJsonObj ("Json/Net/NetConfig");
			this._gameConfigPriority = this._gameConfig ["priority"];
		}
		
		public void Restart ()
		{
			this.GameClose ();
			this._gameRecvFunc = MyLuaSrv.Instance.GetFunction ("NetProtocol.Receive");
		}
		
		// Update
		public int updateIntervalMillis = 20;
		public int updateIntervalLimitMillis = 33;
		private System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
		private int elapsedMillis = 0;
//		private int totalCount = 0;
//		private long totalMillis = 0;
		void Update ()
		{
			int updateCount = 1;
			if (stopWatch.IsRunning)
			{
				stopWatch.Stop();
//				totalMillis += stopWatch.ElapsedMilliseconds;
				elapsedMillis += (int)Math.Min(stopWatch.ElapsedMilliseconds, updateIntervalLimitMillis);
				updateCount = (int)(elapsedMillis / updateIntervalMillis);
//				totalCount += updateCount;
//				Debug.LogFormat("<color=white>NetConnectionManager.Update: </color>{0}, {1}, {2}, {3}, {4}FPS",
//				                updateCount, Time.time, elapsedMillis, stopWatch.ElapsedMilliseconds, (int)(totalCount/(totalMillis/1000.0)));
				elapsedMillis -= updateCount*updateIntervalMillis;
			}

			for (int i = 0; i < updateCount; ++i)
			{
				UpdateGame();
			}

			stopWatch.Reset();
			stopWatch.Start();
		}
		
//		void FixedUpdate ()
//		{
//			this.UpdateGame ();
//		}
		
		// Destroy
		protected override void OnDestroy ()
		{
			this.GameClose ();
			base.OnDestroy ();
		}

		// gui 
		void OnGUI ()
		{
			//			GUI.Label(new Rect(50, 50, 100, 30), "up : " + (this._gameSendNum / 1024.0f).ToString("f3") + "kb");
			//			GUI.Label(new Rect(150, 50, 100, 30), "down : " + (this._gameRecvNum / 1024.0f).ToString("f3") + "kb");
		}
		
//		#region game connection
		// update
		private void UpdateGame ()
		{
			if (this._gameUpdate != null)
				this._gameUpdate ();
			
			// receive
			if (this._gameRecvFunc != null) {
				List<ProtocolEntry.GameReceive> receives = null;
				lock (_gameReceivesLocker) {
					receives = this._gameReceives;
					if (null == receives || 0 == receives.Count) {
						return;
					}
					if (0 == receiveQueue.Count) {
						this._gameReceives = new List<ProtocolEntry.GameReceive> ();
					} else {
						this._gameReceives = receiveQueue.Dequeue ();
					}
				}

				foreach (var receive in receives) {			
					if (this._gameRecvFunc.Ref < 1) {
						this._gameRecvFunc = MyLuaSrv.Instance.GetFunction ("NetProtocol.Receive");
					}
					if (this._gameRecvFunc.Ref > 0) {
                        //						this._gameRecvFunc.call (
                        //							receive.id1, 
                        //							receive.id2, 
                        //							receive.data
                        //						);

                            MyLuaSrv.Instance.CallReceiveProtol(
                            this._gameRecvFunc,
                            receive.id1,
                            receive.id2,
                            receive.data,
                            receive.data.Length);

					}
				}

				receives.Clear ();
				receiveQueue.Enqueue (receives);
				
				TryHandleSend ();
			}
		}

		// connect
		public void ConnLoginServer (string ip, int port, Action<int,int> handle)
		{
			int connectTo = connectTimeOut;
			var result = this.LoginServerConnection.Conn (ip, port, connectTo);
			if (result) {
				Action<int> connLoginServerHandler = (int state) => {
					curConnect = this.LoginServerConnection;
					if (handle != null) {
						handle (state, curConnect.NetDelay);
					}
				};
				StartCoroutine (ConnCallback (this.LoginServerConnection, connLoginServerHandler));
			}
		}

		public void ConnGameServer (string ip, int port, Action<int,int> handle)
		{

			int connectTo = connectTimeOut;
			IPAddress ipAddress;	
			bool isIp = IPAddress.TryParse(ip,out ipAddress);
			if(!isIp)
			{
				Action<string> getIpHandler = (string resolveIp) => {
					var result = this.GameServerConnection.Conn (resolveIp, port, connectTo);
					if (result) {
						Action<int> closeLoginServerHandler = (int state) => {
							curConnect = this.GameServerConnection;
							handle (state, curConnect.NetDelay);
						};
						StartCoroutine (ConnCallback (this.GameServerConnection, closeLoginServerHandler));
					}
				};
				QueryIpWraper wraper = new QueryIpWraper();
				wraper.ip = ip;
				wraper.complete = false;
				ThreadPool.QueueUserWorkItem(new WaitCallback(StartGetIP), wraper);

				StartCoroutine (GetIPCallback (wraper,getIpHandler));
			}
			else
			{
				var result = this.GameServerConnection.Conn (ip, port, connectTo);
				if (result) {
					Action<int> closeLoginServerHandler = (int state) => {
						//				if (this.LoginServerConnection != null) {
						//					this.LoginServerConnection.DisConnect ();
						//				}
						curConnect = this.GameServerConnection;
						handle (state, curConnect.NetDelay);
					};
					StartCoroutine (ConnCallback (this.GameServerConnection, closeLoginServerHandler));
				}
			}
		}

		public IEnumerator GetIPCallback (QueryIpWraper wraper, Action<string> callback)
		{

			while (!wraper.complete)
			{
				yield return 0;
			}
			callback(wraper.ip);
		}

		public void StartGetIP (object obj)
		{
			QueryIpWraper wrap = obj as QueryIpWraper;
			IPHostEntry ipHost = Dns.GetHostEntry(wrap.ip);
			var ipAddress = ipHost.AddressList[0];
			wrap.ip = ipAddress.ToString();
			wrap.complete = true;
		}

		public IEnumerator ConnCallback (NetConnection connect, Action<int> callback)
		{
			while ((int)NetState.Connecting == connect.state)
			{
				yield return 0;
			}
			callback(connect.state);
		}
		
		// close
		public void GameClose ()
		{
			this._gameRecvFunc = null;
			this._gameUpdate = null;
			this.DisConnect ();
		}

		public void DisConnect ()
		{
			this._gameReceives.Clear ();
			this.GameServerConnection.DisConnect ();
			protocolBytes.clearData();
		}

		public void setSocket (Socket socket)
		{
			DisConnect();
			this.GameServerConnection.setSocket (socket);
			curConnect = this.GameServerConnection;
		}

		public bool IsConnected ()
		{
			if(this.GameServerConnection != null)
				return this.GameServerConnection.IsConnected ();
			else
				return false;
		}

		void TryHandleSend ()
		{
			if (this.GameServerConnection.Handler.HandleSend != null) {
				for (int i=0; i<this.GameServerConnection.sendDones.Count; i++)
					this.GameServerConnection.Handler.HandleSend (this.GameServerConnection.sendDones [i]);
			}
			if (this.GameServerConnection.sendDones.Count > 0)
				this.GameServerConnection.sendDones.Clear ();
		}

		public void SetSendCallBack (Action<NetProtocolID> callBack)
		{
			this.GameServerConnection.Handler.HandleSend = callBack;
		}

		public void AddSendCallBackProtocolID (uint id1, uint id2)
		{
			SDictionary<uint,NetProtocolID> map = _sendCallBackID [id1];
			if (map == null) {
				map = new SDictionary<uint, NetProtocolID> ();
				_sendCallBackID [id1] = map;
			}
			if (map [id2] == null)
				map [id2] = new NetProtocolID (id1, id2);
		}

		NetProtocolID TryGetCareID (uint id1, uint id2)
		{
			SDictionary<uint,NetProtocolID> map = _sendCallBackID [id1];
			if (map != null)
				return map [id2];
			return null;
		}

		public void DoGameSend (uint id1, uint id2, byte[] bytes)
		{
			this.curConnect.Send (bytes, TryGetCareID (id1, id2));
		}
		
		// handle receive
		// id1 大消息号
		// id2 小消息号
		public void GameHandleReceive (byte[] bytes, int length)
		{
			downloadbyteLength += length;

			this._logger.LogByteDatarintDataAndLen ("Receive", length, bytes);
			var protocolList = protocolBytes.Parse (bytes, length, 0);
			AddGameReceive (protocolList);
		}

		private void AddGameReceive (List<ProtocolEntry.ProtocolInfo>protocolList)
		{
			if (null != protocolList && 0 < protocolList.Count) {
				// parse
				foreach (var protocolInfo in protocolList) {
					
					if (2 > protocolInfo.bodyLength) {
						continue;
					}
					int id1 = Convert.ToInt32 (Convert.ToChar (protocolInfo.body [0]));
					int id2 = Convert.ToInt32 (Convert.ToChar (protocolInfo.body [1]));
					byte[] data = null;
					if (2 < protocolInfo.bodyLength) {
						if (id1 == NetManager.SubProtoc_Id1 && id2 == NetManager.SubProtoc_Id2) {
							var subList = protocolInfo.ProcessSubProtoc ();
							AddGameReceive (subList);
							ProtocolEntry.ProtocolInfo.Destroy (protocolInfo);
						} else {
							if(false == protocolInfo.invalid)
							{
								data = new byte[protocolInfo.bodyLength - 2];
								Array.Copy (protocolInfo.body, 2, data, 0, data.Length);
								this.GameAddReceives (new ProtocolEntry.GameReceive (id1, id2, data));
							}
							ProtocolEntry.ProtocolInfo.Destroy (protocolInfo);
						}
						
					} else {
						data = new byte[0];
						this.GameAddReceives (new ProtocolEntry.GameReceive (id1, id2, data));
						ProtocolEntry.ProtocolInfo.Destroy (protocolInfo);
					}
				}
			}
		}
	}
}
