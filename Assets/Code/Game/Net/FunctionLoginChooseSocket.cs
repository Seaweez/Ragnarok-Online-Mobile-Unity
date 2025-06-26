using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
namespace RO.Net
{
	[SLua.CustomLuaClassAttribute]
	public class FunctionLoginChooseSocket{
		[SLua.CustomLuaClassAttribute]
		public enum SocketConnectState {
			None,
			Finish,
			Connecting,
			Failure,
		};

		public class SocketConnectResult
		{
			public string ip;
			public int delay;
			public SocketConnectState state;
			public string errorMessage;
			public int errorCode;
			public int port;
			public string originDomain;
			public Socket socket;
			public SocketConnectResult()
			{
				state = SocketConnectState.None;
			}

			public void dispose()
			{
				if(null != socket)
				{
					try{
						if(socket.Connected)
						{
							socket.Shutdown(SocketShutdown.Both);
							socket.Close();
						}
					}
					catch(Exception e)
					{
						ROLogger.Log ("unkown error occur!"+e);
					}
					finally
					{
						socket = null;
					}
				}
			}
		}

		public class SocketConnect
		{
			public SocketConnectResult result;
			private FunctionLoginChooseSocket flcs;

			public SocketConnect(string ip,int port,string originDomain,FunctionLoginChooseSocket flcs)
			{
				result = new SocketConnectResult();
				result.ip = ip;
				result.port = port;
				result.originDomain = originDomain;
				this.flcs = flcs;
			}
			public void startConnect()
			{			
				result.state = SocketConnectState.Connecting;
				IPAddress ipAddress;
				bool isIp = IPAddress.TryParse(result.ip,out ipAddress);
				if(!isIp)
				{
					ROLogger.Log ("SocketConnect the address is not a valid ip!");
					result.state = SocketConnectState.Failure;
					result.errorMessage = string.Format("SocketConnect the address is not a valid ip ip:{0},port:{1}",result.ip,result.port);
				}
				else
				{
					Socket socket = null;
					try{
						socket = new Socket(
							ipAddress.AddressFamily,
							SocketType.Stream,
							ProtocolType.Tcp
						);

						var start = DateTime.Now;
						socket.Connect(result.ip,result.port);
						var end = DateTime.Now;
						result.delay = (int)(end - start).TotalMilliseconds;

						if (!socket.Connected)
						{
							result.state = SocketConnectState.Failure;
							result.errorMessage = string.Format("SocketConnect socket.Connected is false ! ip:{0},port:{1},failure Msg:{2}",result.ip,result.port,"socket.Connected is false!");
						}
						else
						{
							result.state = SocketConnectState.Finish;
							result.socket = socket;
						}
					}
					catch(SocketException e)
					{
						result.state = SocketConnectState.Failure;
						result.errorCode = e.ErrorCode;
						result.errorMessage = string.Format("SocketConnect Connect occur SocketException ip:{0},port:{1},failure Msg:{2},errorCode:{3}",result.ip,result.port,e.Message,e.ErrorCode);
					}
					catch(Exception e)
					{
						result.state = SocketConnectState.Failure;
						result.errorMessage = string.Format("SocketConnect Connect failure! ip:{0},port:{1},failure Msg:{2}",result.ip,result.port,e.Message);
					}

					finally{
						if(null != socket && SocketConnectState.Finish != result.state)
						{
							try{
								if(socket.Connected)
								{
									socket.Shutdown(SocketShutdown.Both);
									socket.Close();
									socket = null;
								}
							}
							catch(Exception e)
							{
								ROLogger.Log ("unkown error occur!"+e);
							}
						}
					}
				}

				this.flcs.completeCount = this.flcs.completeCount +1;
			}

		}


		public List<SocketConnect> socketConnects = new List<SocketConnect>();
		public Action<SocketConnect[]> callback = null;


		private int completeCount = 0;
		public float passedTime = 0;
		private float startTime = 0;
		private int overTime = 5;

		public  FunctionLoginChooseSocket(int overTime)
		{
			this.overTime = overTime;		
		}

		public bool isComplete{
			get{
				if(completeCount >= socketConnects.Count)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		public bool isOverTime{
			get{
				if(passedTime >= overTime)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		public void addIpAndPort(string ip,int port,string originDomain)
		{
			if(true != string.IsNullOrEmpty(ip) && 0 != port)
			{
				var sct = new SocketConnect(ip,port,originDomain,this);
				socketConnects.Add(sct);
			}			
		}

		public void setCallback(Action<SocketConnect[]>callback)
		{
			this.callback = callback;
		}
		public void tryStartConnect()
		{
			foreach(var sct in socketConnects)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(startConnect),sct);
			}
			this.startTime = Time.realtimeSinceStartup;
			FunctionLoginMono.Instance.startConnect(this);
		}

		private void startConnect(object obj)
		{
			SocketConnect sct = obj as SocketConnect;
			sct.startConnect();
		}

		public void finishTest()
		{
			if(null != this.callback && socketConnects.Count > 0)
			{
				this.callback(socketConnects.ToArray());
			}
			else
			{
				ROLogger.Log ("SocketConnectTest's callback or socketConnects is null!");
			}
		}

		public void updatePassedTime()
		{
			var now = Time.realtimeSinceStartup;
			this.passedTime = now - startTime;
		}

	}
}