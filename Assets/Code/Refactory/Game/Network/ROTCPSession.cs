using UnityEngine;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;
using Ghost.Extensions;
using Ghost.Utility;
using Ghost;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class ROTCPSession : TCPSession<IROProtol, IROProtol>
	{
		private Info.Phase prevPhase = Info.Phase.None;

		#region sync
		private ObjectPool<ReuseableList<int>> listPool;
		private object sendedIDsLocker = new object();
		private ReuseableList<int> sendedIDs;

		public void NotifySended(int sendID)
		{
			lock (sendedIDsLocker)
			{
				sendedIDs.list.Add(sendID);
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public ReuseableList<int> GetSendIDs()
		{
			lock (sendedIDsLocker)
			{
				if (0 >= sendedIDs.list.Count)
				{
					return null;
				}
			}
			var newSendedIDs = listPool.SafeCreate();
			lock (sendedIDsLocker)
			{
				var ret = sendedIDs;
				sendedIDs = newSendedIDs;
				return ret;
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void ReuseSendIDs(ReuseableList<int> p)
		{
			lock (sendedIDsLocker)
			{
				if (p == sendedIDs)
				{
					return;
				}
			}
			listPool.SafeDestroy(p);
		}
		#endregion sync

		#region override
		protected override List<object> DoBkgBatch (List<object> args)
		{
			// TODO Combine TCPSessionInfo.OperateData
			return args;
		}

		protected override void DoBkgSend (Socket tcp, IROProtol p)
		{
			#if DEBUG
			Debug.Assert(null != p);
			#endif

			if (ROProtolFactory.uncompressLimitSize < p.GetDataLength())
			{
				p.Compress();
			}

			SocketHelper.LoopSend(tcp, p.GetData(), 0, p.GetDataLength());
			NotifySended(p.GetID ());
		}

		protected override IROProtol DoBkgReceive (Socket tcp)
		{
			var p = ROProtolFactory.CreateProtol_PB(ROProtolFactory.receiveBufferCapacity, 0);

			p.SetDataLength(ROProtol_PB.HeadLength);
			var buffer = p.GetData();
			var bufferLength = p.GetDataLength();
			SocketHelper.LoopReceive(tcp, buffer, 0, bufferLength);

			int flag, dataLength;
			ROProtol_PB.UnserializeHead(buffer, 0, out flag, out dataLength);
			if (0 < dataLength)
			{
				p.SetDataLength(ROProtol_PB.HeadLength+dataLength);
				buffer = p.GetData();
				SocketHelper.LoopReceive(tcp, buffer, ROProtol_PB.HeadLength, dataLength);
				p.Unserialize();
				if (ROProtol_PB.Flag_Compressed == flag)
				{
					p.Decompress();
				}
			}
			return p;
		}

		protected override void DoHandleReceive (IROProtol p)
		{
			// TODO Callback Received
		}
		#endregion override

		#region behaviour
		protected virtual void Awake()
		{
			listPool = new ObjectPool<ReuseableList<int>>();
			sendedIDs = listPool.Create();
		}

		protected override void FixedUpdate ()
		{
			base.FixedUpdate ();
			var p = GetSendIDs();
			if (null != p)
			{
//				for (int i = 0; i < p.list.Count; ++i)
//				{
//					Logger.LogFormat("<color=green>Sended: </color>\n{0}", p.list[i]);
//				}
				// TODO Callback Sended
				ReuseSendIDs(p);
			}

			var curPhase = info.phase;
			if (prevPhase != curPhase)
			{
				RO.LoggerUnused.LogFormat(gameObject, "<color=green>Phase Changed: </color>\n{0} -> {1}", prevPhase, curPhase);
				var e = info.exception;
				if (null != e)
				{
					RO.LoggerUnused.LogFormat(gameObject, "<color=red>Exception: </color>\n{0}", e);
					// TODO Callback Exception
				}

				prevPhase = curPhase;
			}
		}
		#endregion behaviour
	}
} // namespace Ghost.Sample
