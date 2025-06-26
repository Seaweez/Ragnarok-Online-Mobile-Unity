using UnityEngine;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace RO.Net
{
	/**
	 * TCP socket
	 * This connection is realized by socket tcp
	 */ 
	public class NetConnectionTCP : NetConnection
	{
		
		class ThreadContext
		{
			public bool exsit = false;

			public ThreadContext (bool exsit)
			{
				this.exsit = exsit;
			}
		}

		private int sendCount = 0;
		private Socket _socket = null;
		private Thread _thread = null;
		private Thread sendThread = null;
		private byte[] _bytes;
		private string ip = null;
		private int port;
		
		public int NetDelay = -1;
		
		private int startConnectTimeMill = -1;
		
		private ThreadContext recvContext = new ThreadContext (false);
		private ThreadContext sendContext = new ThreadContext (false);
		private ManualResetEvent sendtDone = new ManualResetEvent (false);
		
		private object sendDataLock = new object ();
		
		public List<NetProtocolID> sendDones = new List<NetProtocolID> ();
		//		public List<NetProtocolID> sendDones {
		//			get {
		//				return _sendDones;
		//			}
		//		}
		
		private int connectTimeOut;
		private int recvMaxLen;
		
		private List<ProtocolSendEntry> toSends = new List<ProtocolSendEntry> ();
		
		// ctor
		public NetConnectionTCP (int recvMaxLen)
		{
			this._name = "TCP";
			this.recvMaxLen = recvMaxLen;
			// 512b 
			this._bytes = new byte[this.recvMaxLen];
		}
		
		// init socket
		private void InitSocket (AddressFamily AddressFamily)
		{
			DisConnect ();
			this._socket = new Socket (
				AddressFamily,
				SocketType.Stream,
				ProtocolType.Tcp
			);
			
			this.state = (int)NetState.Disconnect;
		}

		private void InitRecThread ()
		{
			if (this.state == (int)NetState.Connect) {
				this._thread = new Thread (this.Recv);
				this._thread.IsBackground = true;
				recvContext.exsit = false;
				this._thread.Start (recvContext);
			}
		}

		private void stopRecThread ()
		{
			try {
				if (this._thread != null) {
					this.recvContext.exsit = true;
					this._thread.Interrupt ();
				}
			} catch (Exception e) {
				ROLogger.Log ("Exception:stopRecThread!"+e);
			} finally {
				this._thread = null;
			}
			
		}

		private void InitSendThread ()
		{
			if (this.state == (int)NetState.Connect) {
				this.sendThread = new Thread (this.sendData);
				this.sendThread.IsBackground = true;
				sendContext.exsit = false;
				this.sendThread.Start (sendContext);
			}	
		}

		private void stopSendThread ()
		{
			
			try {
				if (sendThread != null) {
					sendContext.exsit = true;
					sendThread.Interrupt ();
					//					sendThread.Abort("abort send Thread!");
				}
			} catch (Exception e) {
				ROLogger.Log ("Exception:stopSendThread!"+e);
			} finally {
				sendThread = null;
				toSends.Clear ();
			}
		}

		private void sendData (object obj)
		{
			try {
				var context = obj as ThreadContext;
				ProtocolSendEntry entry = null;
				while (!context.exsit) {
					entry = null;
					sendtDone.WaitOne ();
//					Debug.Log ("sendData");
					lock (sendDataLock) {
						if (toSends.Count > 0) {
							entry = toSends [0];
							toSends.RemoveAt (0);
						}
						if (toSends.Count == 0) {
//							Debug.Log ("没消息可发送，线程阻塞掉");
							sendtDone.Reset ();					
						}
					}

					if (entry != null) {
						try {
							byte[] bts;
							NetProtocolID pID = null;
							bts = entry.bytes;
							pID = entry.pId;
							
							this._logger.LogByteDatarintDataAndLen ("send", entry.bytes.Length, entry.bytes);
							if (pID != null) {
								foreach (uint key in NetManager.writeFileDt.Keys) {
									if (key == pID.id1 && NetManager.writeFileDt [key] == pID.id2) {
										sendCount = sendCount + 1;
										var fileName = string.Format ("sendFile_{0}.txt", sendCount);
										NetUtil.WriteFile (fileName, entry.bytes, entry.bytes.Length);
									}
								}
								
							}
//							Debug.Log ("发送消息");
							this.state = (int)NetState.Sending;
							Ghost.SocketHelper.LoopSend (this._socket, bts, 0, bts.Length);
							if (pID != null) {
								sendDones.Add (pID);
							}
							this.state = (int)NetState.Connect;
						} catch (Exception e) {
							ROLogger.LogWarningFormat ("NetConnectionTCP sendData error:{0}", e.Message);
							sendtDone.Set ();
						}
					}
				}
			} catch (ThreadInterruptedException e) {
				ROLogger.LogWarningFormat ("NetConnectionTCP sendData ThreadInterruptedException:{0}", e.Message);
			} finally {
			}
			
		}
		
		// is connected
		// only use to check whether the socket is init success
		// can not use to determine whether connected server
		public override bool IsConnected ()
		{
			if (this._socket == null)
				return false;
			
			return this._socket.Connected;
		}
		
		// conn
		public bool Conn (string ip, int port, int timeout)
		{
			bool hasConn = false;
			if (this.IsConnected ()) {
				ROLogger.LogFormat (" Conn(string ip, int port) is connected!state:{0}", this.state);
				return hasConn;
			}
			
			if (this.state == (int)NetState.Connecting) {
				ROLogger.Log (this._name + " Conn(string ip, int port) is connecting");
				return hasConn;
			}
			
			try {
				
				IPAddress ipAddress;	
				IPAddress.TryParse (ip, out ipAddress);
				IPEndPoint ipep = new IPEndPoint (
					                  ipAddress,
					                  port
				                  );
				
				this._logger.LogConn (this._name, " Conn(string ip, int port) ip: " + ip + " port: " + port);
				
				
				this.InitSocket (ipAddress.AddressFamily);
				this._logger.Log (ipAddress.AddressFamily.ToString ());
				this.state = (int)NetState.Connecting;
				var result = this._socket.BeginConnect (
					             ipep,
					             new AsyncCallback (this.ConnectCallback),
					             this._socket
				             );
				this.connectTimeOut = timeout;
				
				startConnectTimeMill = DateTime.Now.Millisecond;
				ThreadPool.RegisterWaitForSingleObject (result.AsyncWaitHandle, new WaitOrTimerCallback (TimeoutCallback), this._socket, connectTimeOut, true);
				
				this.ip = ip;
				this.port = port;
				hasConn = true;
			} catch (SocketException e) {
				ROLogger.LogWarningFormat (" SocketException Conn(string ip, int port) ip:{0},port:{1},msg:{2}", ip, port, e.Message);
				this.state = e.ErrorCode;				
			} catch (Exception e) {
				ROLogger.LogWarningFormat (" Exception Conn(string ip, int port) ip:{0},port:{1},msg:{2}", ip, port, e.Message);
				this.state = (int)NetState.Error;				
			}
			return hasConn;
		}
		
		// re conn
		public void ReConn ()
		{
			if (this.IsConnected ())
				return;
			
			if (this.ip != null && this.port != 0)
				this.Conn (this.ip, (int)this.port, this.connectTimeOut);
		}

		public void setSocket (Socket socket)
		{
			this.DisConnect ();
			this._socket = socket;
			this.state = this._socket.Connected ? (int)NetState.Connect : (int)NetState.ConnectFailure;
			int nowMill = DateTime.Now.Millisecond;
			this.NetDelay = nowMill - startConnectTimeMill;
			this._logger.Log (string.Format (" ConnectCallback state:{0},startConnectTimeMill:{1},now:{2} ", this.state.ToString (), startConnectTimeMill, nowMill));
			InitRecThread ();
			InitSendThread ();
		}
		
		// dis connect
		public void DisConnect ()
		{
			
			if (_socket == null) {
				ROLogger.Log (this._name + " DisConnect() socket is null");
				return;
			}
			
			if (this.state == (int)NetState.Disconnecting) {
				ROLogger.Log (this._name + " DisConnect() is Disconnecting");
				return;
			}
			
			stopRecThread ();
			stopSendThread ();
			try {
				this._socket.Close ();
				this._socket = null;
				this.state = (int)NetState.Disconnect;
			} catch (SocketException e) {				
				ROLogger.Log ("DisConnect,SocketException:" + e.Message);
				this.state = e.ErrorCode;
			} catch (Exception e) {
				ROLogger.Log ("DisConnect,Exception:" + e.Message);
				this.state = (int)NetState.Error;
			}
		}
		
		// send
		public void Send (byte[] bytes, NetProtocolID pID)
		{
			if (!this.IsConnected ()) {
				ROLogger.Log ("Send(byte[] bytes,NetProtocolID pID) is unconnected ");
				this._logger.LogUnConn (this._name);
				return;
			}
			
			//			if (this.state == (int) NetState.Sending) {
			//		this._logger.Log(this._name + " Send(byte[] bytes,NetProtocolID pID) is Sending");
			////				this._logger.LogUnConn(this._name);
			//				return;
			//			}
			ProtocolSendEntry entry = new ProtocolSendEntry (bytes, pID);
			lock (sendDataLock) {
				this.toSends.Add (entry);				
			}			  
//			Debug.Log ("有消息要派发，线程取消阻塞");
			sendtDone.Set ();
		}
		
		// receive
		private void Recv (object obj)
		{  
			try {
				var context = obj as ThreadContext;
				while (!context.exsit) {  
					// if disconnected then quit thread
					// then we should reconnect
					if (!this.IsConnected ()) {
						ROLogger.Log ("Recv() is unconnected ");
						break;
					}
					
					var length = -1;
					try {
						length = this._socket.Receive (this._bytes);
					} catch (SocketException e) {
						this._logger.LogReceiveError (this._name, e.Message);
						if (!this.IsConnected ()) {
							this.state = e.ErrorCode;
							break;
						}
					} catch (Exception e) {
						this._logger.LogReceiveError (this._name, e.Message);
						if (!this.IsConnected ()) {
							this.state = (int)NetState.Error;
							break;
						}
					}
					
					if (!(length > 0))
						break;
					
					// handle receive bytes
					if (this._handler.HandleReceive != null)
						this._handler.HandleReceive (this._bytes, length);
				}
			} catch (ThreadInterruptedException e) {
				Console.WriteLine (e.ToString ());
			} finally {							
				this.DisConnect ();
			}
		}

		#region callback functions

		// connect callback
		private void ConnectCallback (IAsyncResult iar)
		{
			if (iar.IsCompleted) {
				
				// start receiving
				try {
					this._socket.EndConnect (iar);					
					this.state = this._socket.Connected ? (int)NetState.Connect : (int)NetState.ConnectFailure;
					int nowMill = DateTime.Now.Millisecond;
					this.NetDelay = nowMill - startConnectTimeMill;
					ROLogger.LogFormat ("ConnectCallback state:{0},startConnectTimeMill:{1},now:{2} ", this.state.ToString (), startConnectTimeMill, nowMill);
					InitRecThread ();
					InitSendThread ();
				} catch (SocketException e) {
					ROLogger.LogWarningFormat ("ConnectCallback SocketException msg:{0}", e.Message);
					this.state = e.ErrorCode;
				} catch (Exception e) {
					ROLogger.LogWarningFormat ("ConnectCallback Exception msg:{0}", e.Message);
					this.state = (int)NetState.ConnectFailure;
				}				
			}
		}

		private void TimeoutCallback (object state, bool timedOut)
		{ 
			if (timedOut) {
				DisConnect ();
				this.state = (int)NetState.Timeout;
			}
		}
		
		// send callback
		private void SendCallback (IAsyncResult iar)
		{
			try {
				if (iar.IsCompleted) {

					if (iar.AsyncState != null) {
						NetProtocolID t = iar.AsyncState as NetProtocolID;
						sendDones.Add (t);
					}
					this.state = (int)NetState.Connect;
					this._socket.EndSend (iar);
					this._logger.LogSendSuccess (this._name + " callback");
					sendtDone.Set ();
				}
			} catch (Exception e) {
				ROLogger.LogWarningFormat ("SendCallback Exception msg:{0}", e.Message);
				sendtDone.Set ();
			} finally {
				
			}

		}

		#endregion
	}
}
