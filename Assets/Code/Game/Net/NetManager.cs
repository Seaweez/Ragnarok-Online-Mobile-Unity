using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;
using SLua;

namespace RO.Net
{
	public class NetManagerHelper : LuaObject
	{
		public static int HeadLength;
		public static int IDLength;
		public static MemoryStream stream;
		public static byte[] compressBuffer;
		public static int NonceHeadLen;

		static NetManagerHelper()
		{
			HeadLength = 3;
			IDLength = 2;
			NonceHeadLen = 2;
			stream = new MemoryStream();
			compressBuffer = new byte[HeadLength+IDLength];
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		public static int GameSend(IntPtr l)
		{
			try 
			{
				// online
				if (null != NetConnectionManager.Me) 
				{
					Int32 a1;
					LuaObject.checkType(l,1,out a1);
					Int32 a2;
					LuaObject.checkType(l,2,out a2);

					var pID1 = Convert.ToUInt32(a1);
					var pID2 = Convert.ToUInt32(a2);

					var compress = false;
					int dataLen;
					int nonceLen;
					if(NetConnectionManager.Me.EnableNonce)
					{
						if(LuaDLL.lua_isstring(l,4))
						{
							IntPtr nonceStr = LuaDLL.luaS_tolstring32(l, 3, out nonceLen);
							IntPtr str = LuaDLL.luaS_tolstring32(l, 4, out dataLen);
							if (IntPtr.Zero != str)
							{
								if (NetConnectionManager.Me.maxZibNum < dataLen + nonceLen) 
								{
									compress = true;
									stream.SetLength(IDLength+dataLen+NonceHeadLen+nonceLen);
									var buffer = stream.GetBuffer();
									buffer [0] = Convert.ToByte (pID1);
									buffer [1] = Convert.ToByte (pID2);
									buffer [2] = Convert.ToByte ((nonceLen & 0xff));
									buffer [3] = Convert.ToByte ((nonceLen >> 8) & 0xff);
									Marshal.Copy(nonceStr, stream.GetBuffer(), IDLength + NonceHeadLen, nonceLen);
									Marshal.Copy(str, stream.GetBuffer(), IDLength+NonceHeadLen+nonceLen, dataLen);
								}
								else
								{
									stream.SetLength(HeadLength+IDLength+dataLen+NonceHeadLen+nonceLen);
									var buffer = stream.GetBuffer();
									buffer [3] = Convert.ToByte (pID1);
									buffer [4] = Convert.ToByte (pID2);
									buffer [5] = Convert.ToByte ((nonceLen & 0xff));
									buffer [6] = Convert.ToByte ((nonceLen >> 8) & 0xff);
									Marshal.Copy(nonceStr, stream.GetBuffer(), HeadLength+IDLength + NonceHeadLen, nonceLen);
									Marshal.Copy(str, stream.GetBuffer(), HeadLength+IDLength +NonceHeadLen+nonceLen, dataLen);
								}
							}
							else
							{
								dataLen = 0;
								stream.SetLength(HeadLength+IDLength);
							}
						}
						else
						{
							dataLen = 0;
							stream.SetLength(HeadLength+IDLength);
						}
					}
					else
					{
						if(LuaDLL.lua_isstring(l,3))
						{
							IntPtr str = LuaDLL.luaS_tolstring32(l, 3, out dataLen);
							if (IntPtr.Zero != str)
							{
								if (NetConnectionManager.Me.maxZibNum < dataLen) 
								{
									compress = true;
									stream.SetLength(IDLength+dataLen);
									var buffer = stream.GetBuffer();
									buffer [0] = Convert.ToByte (pID1);
									buffer [1] = Convert.ToByte (pID2);
									Marshal.Copy(str, stream.GetBuffer(), IDLength, dataLen);
								}
								else
								{
									stream.SetLength(HeadLength+IDLength+dataLen);
									var buffer = stream.GetBuffer();
									buffer [3] = Convert.ToByte (pID1);
									buffer [4] = Convert.ToByte (pID2);
									Marshal.Copy(str, stream.GetBuffer(), HeadLength+IDLength, dataLen);
								}
							}
							else
							{
								dataLen = 0;
								stream.SetLength(HeadLength+IDLength);
							}
						}
						else
						{
							dataLen = 0;
							stream.SetLength(HeadLength+IDLength);
						}
					}


					var bytes = stream.GetBuffer();
					var bytesLen = (int)stream.Length;

					var flag = 0;
					if (compress) 
					{
						var compressedLen = lzip.compressBuffer (
							bytes, 
							bytesLen, 
							ref compressBuffer, 
							HeadLength,
							NetConnectionManager.Me.compressLevel);

						if (0 < compressedLen) 
						{
							bytesLen = HeadLength+compressedLen;
							flag = 1;
						}
						else
						{
							for (int i = 0; i < bytesLen; ++i)
							{
								compressBuffer[i+HeadLength] = bytes[i];
							}
							bytesLen += HeadLength;
						}
						bytes = compressBuffer;
					}
					var bodyLen = bytesLen-HeadLength;
					byte[] sendData = null;
					if(null != NetConnectionManager.Me && NetConnectionManager.Me.EnableEncrypt)
					{
						bytes = NetUtil.Encrypt(bytes,HeadLength,bodyLen);
						if(null == bytes)
						{
							return error(l,"Encrypt error!");
						}
						var encryLen = bytes.Length;
						sendData = new byte[encryLen+HeadLength];

						flag = flag +2;
						sendData [0] = Convert.ToByte (flag);
						sendData [1] = Convert.ToByte ((bodyLen & 0xff));
						sendData [2] = Convert.ToByte ((bodyLen >> 8) & 0xff);

						for (int i = 0; i < encryLen; ++i)
						{
							sendData[i+3] = bytes[i];
						}
					}
					else
					{
						bytes [0] = Convert.ToByte (flag);
						bytes [1] = Convert.ToByte ((bodyLen & 0xff));
						bytes [2] = Convert.ToByte ((bodyLen >> 8) & 0xff);

						sendData = new byte[bytesLen]; 
						for (int i = 0; i < bytesLen; ++i)
						{
							sendData[i] = bytes[i];
						}
					}

					NetConnectionManager.Me.DoGameSend(pID1, pID2, sendData);

					if(NetConnectionManager.Me.EnableSelfResolve)
					{
						NetConnectionManager.Me.GameHandleReceive(sendData,sendData.Length);
					}
				}
				// offline
				else {
					RO.LoggerUnused.LogError ("NetManager::GameSend Error: network not connected");
					// nothing to do now
				}

				pushValue(l,true);
				return 1;
			}
			catch(Exception e) 
			{
				return error(l,e);
			}
		}
		
		public static void reg(IntPtr l)
		{
			getTypeTable(l, "RO.Net.NetManagerHelper");
			addMember(l, GameSend, false);
			createTypeMetatable(l, null, typeof(NetManagerHelper));
		}
	}

	/**
	 * Manager online connections and offline logic
	 */ 
	[SLua.CustomLuaClassAttribute]
	public class NetManager
	{	
		public static int SubProtoc_Id1 = 99;
		public static int SubProtoc_Id2 = 1;
		public static SDictionary<uint,uint> writeFileDt = new SDictionary<uint,uint >();

		#region game 
		// connect
		public static void ConnLoginServer (string ip, int port,Action<int,int> handle, object timeout = null)
		{
			// online
			if (NetConnectionManager.Me != null) {
				NetLog.Log ("NetManager::ConnLoginServer:ip:" + ip + " port:" + port);
				NetConnectionManager.Me.ConnLoginServer (ip, port, handle);

			}
			// offline
			else {
				RO.LoggerUnused.LogError ("NetManager::GameConn Error: network not connected");
				// nothing to do now
			}
		}

		// login
		public static void ConnGameServer (string ip, int port,Action<int,int> handle)
		{
			// online
			if (NetConnectionManager.Instance != null) {
				NetLog.Log ("NetManager::ConnGameServer ip: " + ip + " port: " + port);
				NetConnectionManager.Me.ConnGameServer (ip, port,handle);
			}
			// offline
			else {
				RO.LoggerUnused.LogError ("NetManager::GameLogin Error: network not connected");
				// nothing to do now
			}
		}

//		// dis connect
//		public static void GameDisConnect (Action handle = null)
//		{
//			// online
//			if (NetConnectionManager.Me != null) {
//				NetConnectionManager.Me.GameDisConnect (handle);
//			}
//			// offline
//			else {
//				Logger.LogError ("NetManager::GameDisConnect Error: network not connected");
//				// nothing to do now
//			}
//		}

		// close
		public static void GameClose ()
		{
			// online
			if (NetConnectionManager.Me != null) {
				NetConnectionManager.Me.GameClose ();
			}
			// offline
			else {
				RO.LoggerUnused.LogError ("NetManager::GameClose Error: network not connected");
				// nothing to do now
			}
		}

		// disconnect
		public static void GameDisConnect ()
		{
			// online
			if (NetConnectionManager.Me != null) {
				NetConnectionManager.Me.DisConnect ();
			}
			// offline
			else {
				RO.LoggerUnused.LogError ("NetManager::GameClose Error: network not connected");
				// nothing to do now
			}
		}

		// set game update
		public static void GameSetUpdate (Action func)
		{
			// online
			if (NetConnectionManager.Me != null) {
				NetConnectionManager.Me.GameUpdate = func;
			}
			// offline
			else {
				RO.LoggerUnused.LogError ("NetManager::GameSetUpdate Error: network not connected");
				// nothing to do now
			}
		}

		public static void SetSocketSendCallBack (Action<NetProtocolID> callBack)
		{
			if (NetConnectionManager.Me != null) {
				NetConnectionManager.Me.SetSendCallBack (callBack);
			}
		}

		public static void AddSendCallBackProtocolID (uint id1, uint id2)
		{
			if (NetConnectionManager.Me != null) {
				NetConnectionManager.Me.AddSendCallBackProtocolID (id1, id2);
			}
		}

		public static void RegistWriteFileProtocolID (uint id1, uint id2)
		{
			NetManager.AddSendCallBackProtocolID(id1,id2);
			writeFileDt.Add(id1,id2);
		}

		// is receiving
		public static bool GameIsReceiving ()
		{
			// online
			if (NetConnectionManager.Me != null) {
				return NetConnectionManager.Me.GameIsReceiving;
			}
			// offline
			else {
				RO.LoggerUnused.LogError ("NetManager::GameSend Error: network not connected");
				// nothing to do now
				return false;
			}	
		}

		// receive
		// id1 大消息号
		// id2 小消息号
		// only for offline
		private static void GameReceive (byte[] bytes)
		{
			// nothing todo now
		}
		#endregion
	}
}