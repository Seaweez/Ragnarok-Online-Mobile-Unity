using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Ghost.Extensions;

namespace Ghost.Utility
{
	// for ObjectPool 
	public interface IReuseableObjectBase
	{
		void Destruct();
		bool reused{get;set;}
		
		void Destroy();
	}

	public interface IReuseableObject : IReuseableObjectBase
	{
		void Construct();
	}

	public interface IReuseableObject<P1> : IReuseableObjectBase
	{
		void Construct(P1 p1);
	}

	public interface IReuseableObject<P1, P2> : IReuseableObjectBase
	{
		void Construct(P1 p1, P2 p2);
	}

	public interface IReuseableObject<P1, P2, P3> : IReuseableObjectBase
	{
		void Construct(P1 p1, P2 p2, P3 p3);
	}

	public interface IReuseableObject<P1, P2, P3, P4> : IReuseableObjectBase
	{
		void Construct(P1 p1, P2 p2, P3 p3, P4 p4);
	}

	public class ReuseableList<T> : IReuseableObject
	{
		public List<T> list{get;private set;}

		public ReuseableList()
		{
			list = new List<T>();
		}

		#region IReuseableObject
		public void Construct()
		{
		}
		public void Destruct()
		{
			for (int i = 0;i < list.Count; ++i)
			{
				var obj = list[i];
				var dispose = obj as IDisposable;
				if (null != dispose)
				{
					dispose.Dispose();
				}
			}
			list.Clear();
		}
		public bool reused{get;set;}

		public void Destroy()
		{
			Destruct();
		}
		#endregion IReuseableObject
	}

	public class ObjectPool<T> : IDisposable where T:IReuseableObjectBase, new()
	{
		public static readonly ObjectPool<T> Singleton = new ObjectPool<T>();

		private Stack<T> pool = new Stack<T>();

		public int Count
		{
			get
			{
				return pool.Count;
			}
		}

		private T Pop()
		{
			T obj;
			if (0 == pool.Count)
			{
				obj = new T();
			}
			else
			{
				obj = pool.Pop();
			}
			return obj;
		}

		private void Push(T obj)
		{
			if (obj.reused)
			{
				return;
			}
			pool.Push(obj);
		}

		public T Create()
		{
			var obj = Pop();
			(obj as IReuseableObject).Construct();
			obj.reused = true;
			return obj;
		}

		public T Create<P1>(P1 p1)
		{
			var obj = Pop();
			(obj as IReuseableObject<P1>).Construct(p1);
			obj.reused = true;
			return obj;
		}

		public T Create<P1, P2>(P1 p1, P2 p2)
		{
			var obj = Pop();
			(obj as IReuseableObject<P1, P2>).Construct(p1, p2);
			obj.reused = true;
			return obj;
		}

		public T Create<P1, P2, P3>(P1 p1, P2 p2, P3 p3)
		{
			var obj = Pop();
			(obj as IReuseableObject<P1, P2, P3>).Construct(p1, p2, p3);
			obj.reused = true;
			return obj;
		}

		public T Create<P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4)
		{
			var obj = Pop();
			(obj as IReuseableObject<P1, P2, P3, P4>).Construct(p1, p2, p3, p4);
			obj.reused = true;
			return obj;
		}

		public void Destroy(T obj)
		{
			Push(obj);
			obj.Destruct();
			obj.reused = false;
		}

		#region safe
		[MethodImpl(MethodImplOptions.Synchronized)]
		private T SafePop()
		{
			return Pop();
		}
		
		[MethodImpl(MethodImplOptions.Synchronized)]
		private void SafePush(T obj)
		{
			Push (obj);
		}

		public T SafeCreate()
		{
			var obj = SafePop();
			(obj as IReuseableObject).Construct();
			obj.reused = true;
			return obj;
		}

		public T SafeCreate<P1>(P1 p1)
		{
			var obj = SafePop();
			(obj as IReuseableObject<P1>).Construct(p1);
			obj.reused = true;
			return obj;
		}
		
		public T SafeCreate<P1, P2>(P1 p1, P2 p2)
		{
			var obj = SafePop();
			(obj as IReuseableObject<P1, P2>).Construct(p1, p2);
			obj.reused = true;
			return obj;
		}

		public T SafeCreate<P1, P2, P3>(P1 p1, P2 p2, P3 p3)
		{
			var obj = SafePop();
			(obj as IReuseableObject<P1, P2, P3>).Construct(p1, p2, p3);
			obj.reused = true;
			return obj;
		}
		
		public T SafeCreate<P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4)
		{
			var obj = SafePop();
			(obj as IReuseableObject<P1, P2, P3, P4>).Construct(p1, p2, p3, p4);
			obj.reused = true;
			return obj;
		}
		
		public void SafeDestroy(T obj)
		{
			SafePush(obj);
			obj.Destruct();
			obj.reused = false;
		}
		#endregion safe

		public void Clear(int restCount = 0)
		{
			var destroyCount = pool.Count-restCount;
			for (int i = 0; i < destroyCount; ++i)
			{
				var obj = pool.Pop();
				obj.Destroy();
			}
		}

		#region IDisposable
		public void Dispose() 
		{
			Clear(0);
		}
		#endregion IDisposable

	}
} // namespace Ghost.Utility
