using UnityEngine;
using System.Collections.Generic;
using System.Net.Sockets;
using System;
using Ghost.Extensions;
using Ghost.Utility;

namespace Ghost
{
	public static class SocketHelper
	{
		public static void LoopSend(
			Socket socket, 
			byte[] data, 
			int offset, 
			int size, 
			SocketFlags flags = SocketFlags.None)
		{
			while (0 < size)
			{
				var ret = socket.Send(data, offset, size, flags);
				if (0 >= ret)
				{
					throw new System.IO.IOException(string.Format("Send return {0}", ret));
				}
				offset += ret;
				size -= ret;
			}
		}
		public static void LoopReceive(
			Socket socket, 
			byte[] buffer, 
			int offset, 
			int size, 
			SocketFlags flags = SocketFlags.None)
		{
			while (0 < size)
			{
				var ret = socket.Receive(buffer, offset, size, flags);
				if (0 >= ret)
				{
					throw new System.IO.IOException(string.Format("Receive return {0}", ret));
				}
				offset += ret;
				size -= ret;
			}
		}
	}

	[System.Serializable]
	[SLua.CustomLuaClassAttribute]
	public class TCPSessionInfo<_SendData, _RecvData> : IDisposable, Async.IProductOwner
	{
		[SLua.CustomLuaClassAttribute]
		public enum Phase
		{
			None,
			ConnectPost,
			Connected,
			ClosePost,
			Closed,
			Exception
		}

		public class OperateData_Connect
		{
			public string host;
			public int port;
		}
		public class OperateData_Disconnect
		{
		}
		public class OperateData_Send : Async.ProductBase<TCPSessionInfo<_SendData, _RecvData>>
		{
			public _SendData data;
		}
		private ObjectPool<OperateData_Send> sendDataPool = new ObjectPool<OperateData_Send>();

		#region IProductOwner
		public void DestroyProduct(IDisposable p)
		{
			var optData = p as OperateData_Send;
			#if DEBUG
			Debug.Assert(null != optData);
			#endif // DEBUG
			if (null != sendDataPool)
			{
				sendDataPool.Destroy(optData);
			}
			else
			{
				optData.Destroy();
			}
		}
		#endregion IProductOwner

		public TcpClient tcp{get;private set;}

		#region sync
		public Phase phase
		{
			get
			{
				return syncInfo.GetPhase();
			}
			set
			{
				syncInfo.SetPhase(value);
			}
		}
		public System.Exception exception
		{
			get
			{
				return syncInfo.GetException();
			}
			set
			{
				syncInfo.SetException(value);
			}
		}

		public class SyncInfo
		{
			private object syncLock = new object();
			private System.Exception excetion = null;
			private Phase phase = Phase.None;

			public System.Exception GetException()
			{
				lock (syncLock)
				{
					return excetion;
				}
			}
			public void SetException(System.Exception e)
			{
				lock (syncLock)
				{
					if (null != e)
					{
						if (null == excetion)
						{
							excetion = e;
							phase = Phase.Exception;
						}
					}
					else
					{
						excetion = null;
						phase = Phase.None;
					}
				}
			}

			public Phase GetPhase()
			{
				lock (syncLock)
				{
					return phase;
				}
			}
			public void SetPhase(Phase p)
			{
				lock (syncLock)
				{
					phase = p;
				}
			}
		}

		private SyncInfo syncInfo = new SyncInfo();

		public bool AllowConnect()
		{
			var p = phase;
			switch (p)
			{
			case Phase.None:
				return true;
			}
			return false;
		}
		public bool AllowDoConnect()
		{
			var p = phase;
			switch (p)
			{
			case Phase.ConnectPost:
				return true;
			}
			return false;
		}

		public bool AllowDisconnect()
		{
			var p = phase;
			switch (p)
			{
			case Phase.Connected:
				return true;
			}
			return false;
		}
		public bool AllowDoDisconnect()
		{
			var p = phase;
			switch (p)
			{
			case Phase.ClosePost:
				return true;
			}
			return false;
		}

		public bool AllowSend()
		{
			var p = phase;
			switch (p)
			{
			case Phase.Connected:
				return true;
			}
			return false;
		}

		public bool AllowReceive()
		{
			var p = phase;
			switch (p)
			{
			case Phase.Connected:
				return true;
			}
			return false;
		}
		#endregion sync

		[System.Serializable]
		[SLua.CustomLuaClassAttribute]
		public class Setting : ICloneable
		{
			public int sendTimeout = 0;
			public int receiveTimeout = 0;
			public int sendBufferSize = 0;
			public int receiveBufferSize = 0;
			public bool blocking = true;

			public object Clone()
			{
				return MemberwiseClone();
			}

			public Setting CloneSelf()
			{
				return Clone() as Setting;
			}
		}
		public string host;
		public int port;
		public Setting setting;

		private void TCPSetting()
		{
			tcp.SendTimeout = setting.sendTimeout;
			tcp.ReceiveTimeout = setting.receiveTimeout;
			tcp.SendBufferSize = setting.sendBufferSize;
			tcp.ReceiveBufferSize = setting.receiveBufferSize;
			tcp.Client.Blocking = setting.blocking;
		}

		public TCPSessionInfo(TcpClient client, Setting s = null)
		{
			#if DEBUG
			Debug.Assert(null != client);
			#endif // DEBUG
			tcp = client;
			if (tcp.Connected)
			{
				phase = Phase.Connected;
				host = tcp.Client.RemoteEndPoint.ToString();
				port = 0;
			}
			if (null == s)
			{
				s = new Setting();
			}
			setting = s;
			TCPSetting();
		}

		#region IDispose
		public void Dispose()
		{
			sendDataPool.Dispose();
			sendDataPool = null;
			tcp.Close();
			phase = Phase.None;
		}
		#endregion IDispose

		public bool Connect(Async.Consumer asyncOperate)
		{
			if (!AllowConnect())
			{
				return false;
			}
			if (string.IsNullOrEmpty(host))
			{
				return false;
			}
			if (0 >= port)
			{
				return false;
			}

			TCPSetting();

			phase = Phase.ConnectPost;

			var optData = new OperateData_Connect();
			optData.host = host;
			optData.port = port;
			asyncOperate.PostProduct(optData);
			return true;
		}

		public bool Disconnect(Async.Consumer asyncOperate)
		{
			if (!AllowDisconnect())
			{
				return false;
			}
			phase = Phase.ClosePost;

			var optData = new OperateData_Disconnect();
			asyncOperate.PostProduct(optData);
			return true;
		}

		public bool Send(Async.Consumer asyncOperate, _SendData data)
		{
			if (!AllowSend())
			{
				return false;
			}
			var optData = sendDataPool.Create(this);
			optData.data = data;
			asyncOperate.PostProduct(optData);
			return true;
		}

		#region background
		public System.Action<Socket, _SendData> DoBkgSend;
		public System.Func<Socket, _RecvData> DoBkgReceive;

		public bool BkgConnect(OperateData_Connect optData)
		{
			if (!AllowDoConnect())
			{
				return false;
			}
			try
			{
				tcp.Client.Connect(optData.host, optData.port);
				phase = Phase.Connected;
			}
			catch (System.Exception e)
			{
				exception = e;
				return false;
			}
			return true;
		}

		public bool BkgDisconnect(OperateData_Disconnect optData)
		{
			if (!AllowDoDisconnect())
			{
				return false;
			}
			try
			{
				tcp.Client.Disconnect(true);
			}
			catch (System.Exception e)
			{
				exception = e;
				return false;
			}
			phase = Phase.Closed;
			return true;
		}

		public bool BkgSend(OperateData_Send optData)
		{
			#if DEBUG
			Debug.Assert(this == optData.owner);
			#endif // DEBUG
			if (!AllowSend())
			{
				return false;
			}

			#if DEBUG
			Debug.Assert(null != DoBkgSend);
			#endif // DEBUG
			try
			{
				DoBkgSend(tcp.Client, optData.data);
			}
			catch (System.Exception e)
			{
				exception = e;
				return false;
			}
			return true;
		}

		public _RecvData BkgReceive()
		{
			if (AllowReceive())
			{
				try
				{
					#if DEBUG
					Debug.Assert(null != DoBkgReceive);
					#endif // DEBUG
					return DoBkgReceive(tcp.Client);
				}
				catch (System.Exception e)
				{
					exception = e;
				}
			}
			return default(_RecvData);
		}
		#endregion background
	}
} // namespace Ghost
