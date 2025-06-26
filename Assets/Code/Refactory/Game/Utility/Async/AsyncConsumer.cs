using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Ghost.Extensions;
using Ghost.Utility;

namespace Ghost.Async
{
	public interface IProductOwner
	{
		void DestroyProduct(IDisposable p);
	}

	public class ProductBase<_Owner> : IReuseableObject<_Owner>, IDisposable 
		where _Owner:IProductOwner
	{
		public _Owner owner;

		#region IReuseableObject
		public void Construct(_Owner p1)
		{
			owner = p1;
		}
		public void Destruct()
		{
		}
		public bool reused{get;set;}

		public void Destroy()
		{
			Destruct();
		}
		#endregion IReuseableObject

		public void Dispose()
		{
			owner.DestroyProduct(this);
		}
	}

	public sealed class Consumer : ProducerConsumerBase
	{
		class ConsumerContext : Context
		{
			public System.Action<List<object>> bkgBatchProc{get;private set;}
			public System.Action<object> bkgProc{get;private set;}
			private ConditionVariable condition;

			public ConsumerContext(System.Action<List<object>> bbp, System.Action<object> bp)
				: base()
			{
				bkgBatchProc = bbp;
				bkgProc = bp;
				condition = new ConditionVariable();
			}

			public void Wait()
			{
				condition.Wait();
			}

			public void Sign()
			{	
				condition.Sign();
			}

			public void SignAll()
			{	
				condition.SignAll();
			}

			#region override
			public override void Close()
			{
				base.Close();
				Sign();
			}
			#endregion override
		}
		private ConsumerContext context;

		public bool StartWork(System.Action<List<object>> backgroundBatchProc, System.Action<object> backgroundProc)
		{
			if (null == backgroundBatchProc && null == backgroundProc)
			{
				return false;
			}
			context = new ConsumerContext(backgroundBatchProc, backgroundProc);
			if (!base.StartWork(BkgProc, context))
			{
				context = null;
				return false;
			}
			return true;
		}

		#region override
		protected override void DoEnd ()
		{
			base.DoEnd ();

			context.Close();
			context = null;
		}
		#endregion override

		public bool PostProduct(object p)
		{
			if (!working)
			{
				return false;
			}
			context.PostProduct(p);
			context.Sign();
			return true;
		}

		#region background
		private static void BkgProc(object param)
		{
			var context = param as ConsumerContext;
			if (null == context)
			{
				return;
			}
			try
			{
				while (!context.closed)
				{
					var p = context.GetProductContainer();
					if (null != p && 0 < p.list.Count)
					{
						if (null != context.bkgBatchProc)
						{
							context.bkgBatchProc(p.list);
						}
						else
						{
							for (int i = 0; i < p.list.Count; ++i)
							{
								context.bkgProc(p.list[i]);
							}
						}
						context.ReuseProductContainer(p);
					}
					else
					{
						context.Wait();
					}
				}
			}
			catch (ThreadInterruptedException)
			{

			}
			finally
			{
				context.Dispose();
			}
		}
		#endregion background

	}
} // namespace Ghost.Async
