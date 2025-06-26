using UnityEngine;
using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
namespace RO.Net
{
	[SLua.CustomLuaClassAttribute]
	public class FunctionLoginDnsResolve{

		[SLua.CustomLuaClassAttribute]
		public enum DnsResolveState {
			None,
			Finish,
			Resolving,
			Failure,
		};

		public class DnsResolveResult
		{
			public string domain;
			public string ip;
			public int delay;
			public DnsResolveState state;
			public string errorMessage;
			public int errorCode;
			public DnsResolveResult()
			{
				state = DnsResolveState.None;
			}
		}

		public class DnsResolve
		{
			public DnsResolveResult result;
			public FunctionLoginDnsResolve fldsr;
			public DnsResolve(string domain,FunctionLoginDnsResolve fldsr)
			{
				result = new DnsResolveResult();
				result.domain = domain;
				this.fldsr = fldsr;
			}
			public void startResolve()
			{
				result.state = DnsResolveState.Resolving;
				IPAddress ipAddress;	
				bool isIp = IPAddress.TryParse(result.domain,out ipAddress);
				if(!isIp)
				{
					try{
						var start = DateTime.Now;
						IPHostEntry ipHost = Dns.GetHostEntry(result.domain);
						var end = DateTime.Now;
						result.delay = (int)(end - start).TotalMilliseconds;
						ipAddress = (ipHost != null?ipHost.AddressList[0]:null);
						if(null != ipAddress)
						{
							result.ip = ipAddress.ToString();
							result.state = DnsResolveState.Finish;
						}
						else
						{
							result.state = DnsResolveState.Failure;
							result.errorMessage = "Dns.GetHostEntry resolve None;";
						}
					}
					catch(SocketException e)
					{
						result.state = DnsResolveState.Failure;
						result.errorMessage = e.Message;
						result.errorCode = e.ErrorCode;
						NetLog.Log (string.Format("Dns.GetHostEntry error! msg:{0}",e.Message));
					}
					catch(Exception e)
					{
						result.state = DnsResolveState.Failure;
						NetLog.Log (string.Format("Dns.GetHostEntry error! msg:{0}",e.Message));
						result.errorMessage = e.Message;
					}
				}
				else
				{
					result.ip = result.domain;
					result.state = DnsResolveState.Finish;
				}
				fldsr.completeCount = fldsr.completeCount + 1;
			}
		}


		public List<DnsResolve> snsResolves = new List<DnsResolve>();
		public Action<DnsResolve[]> callback = null;


		private int completeCount = 0;

		public float passedTime = 0;
		private float startTime = 0;
		private int overTime = 5;

		public  FunctionLoginDnsResolve(int overTime)
		{
			this.overTime = overTime;		
		}

		public bool isComplete{
			get{
				if(completeCount >= snsResolves.Count)
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

		public void addDomainName(string domain)
		{
			if(true != string.IsNullOrEmpty(domain))
			{
				var sct = new DnsResolve(domain,this);
				snsResolves.Add(sct);
			}			
		}

		public void setCallback(Action<DnsResolve[]>callback)
		{
			this.callback = callback;
		}

		public void tryStartResolve()
		{
			
			foreach(var srl in snsResolves)
			{	
				ThreadPool.QueueUserWorkItem(new WaitCallback(startResolve),srl);
			}
			this.startTime = Time.realtimeSinceStartup;
			FunctionLoginMono.Instance.startResolve(this);
		}

		private void startResolve(object obj)
		{	
			DnsResolve srl = obj as DnsResolve;
			srl.startResolve();
		}

		public void finishRequest()
		{
			if(null != this.callback && null !=snsResolves)
			{
				this.callback(snsResolves.ToArray());
			}
			else
			{
				NetLog.Log ("DnsResolveQuest's callback or domainList is null!");
			}
		}

		public void updatePassedTime()
		{
			var now = Time.realtimeSinceStartup;
			this.passedTime = now - startTime;
		}
	}
}