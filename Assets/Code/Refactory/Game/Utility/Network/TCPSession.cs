using UnityEngine;
using System.Collections.Generic;
using System.Net.Sockets;
using Ghost.Extensions;

namespace Ghost
{
	[SLua.CustomLuaClassAttribute]
	public abstract class TCPSession<_SendData, _RecvData> : MonoBehaviour 
	{
		public class Info : TCPSessionInfo<_SendData, _RecvData>
		{
			public Info(TcpClient tcp, Setting s = null)
				: base(tcp, s)
			{
			}
		}

		[SerializeField]
		protected Info info_ = new Info(new TcpClient());
		public Info info
		{
			get
			{
				return info_;
			}
		}

		private Async.Producer asyncReceive = new Async.Producer();
		private Async.Consumer asyncOperate = new Async.Consumer();

		public bool Connect()
		{
			return info.Connect(asyncOperate);
		}

		public bool Disconnect()
		{
			return info.Disconnect(asyncOperate);
		}

		public bool Send(_SendData data)
		{
			return info.Send(asyncOperate, data);
		}

		#region background

		#region abstract
		protected abstract void DoBkgSend(Socket tcp, _SendData data);
		protected abstract _RecvData DoBkgReceive(Socket tcp);
		#endregion abstract

		#region virtual
		protected virtual List<object> DoBkgBatch(List<object> args)
		{
			return args;
		}
		#endregion virtual

		private void BkgBatchOperate(List<object> args)
		{
			args = DoBkgBatch(args);
			for (int i = 0; i < args.Count; ++i)
			{
				BkgOperate(args[i]);
			}
		}

		private void BkgOperate(object arg)
		{
			var optDataSend = arg as Info.OperateData_Send;
			if (null != optDataSend)
			{
				info.BkgSend(optDataSend);
				return;
			}
			var optDataConnect = arg as Info.OperateData_Connect;
			if (null != optDataConnect)
			{
				info.BkgConnect(optDataConnect);
				return;
			}
			var optDataDisconnect = arg as Info.OperateData_Disconnect;
			if (null != optDataDisconnect)
			{
				info.BkgDisconnect(optDataDisconnect);
				return;
			}
		}

		private object BkgReceive()
		{
			return info.BkgReceive();
		}
		#endregion background

		#region abstract
		protected abstract void DoHandleReceive(_RecvData data);
		#endregion abstract

		private void HandleReceive(object p)
		{
			DoHandleReceive((_RecvData)p);
		}

		#region behaviour
		protected virtual void Start()
		{
			info.DoBkgSend = DoBkgSend;
			info.DoBkgReceive = DoBkgReceive;

			asyncOperate.StartWork(BkgBatchOperate, BkgOperate);
		}

		protected virtual void FixedUpdate()
		{
			var p = info.phase;
			switch (p)
			{
			case Info.Phase.Closed:
				GameObject.Destroy(gameObject);
				break;
			case Info.Phase.Exception:
				asyncReceive.EndWork();
				asyncOperate.EndWork();
				break;
			case Info.Phase.ClosePost:
				asyncReceive.EndWork();
				break;
			case Info.Phase.Connected:
				if (!asyncReceive.working)
				{
					asyncReceive.StartWork(BkgReceive);
				}
				else
				{
					asyncReceive.ConsumeProducts(HandleReceive);
				}
				break;
			}
		}

		protected virtual void OnDestroy()
		{
			info.Dispose();
			asyncReceive.EndWork();
			asyncOperate.EndWork();
		}
		#endregion behaviour
	}
} // namespace Ghost
