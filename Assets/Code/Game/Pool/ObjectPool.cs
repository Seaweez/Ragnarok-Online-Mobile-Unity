using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;
using RO;

public sealed class ObjectPool : BaseClass
{
	private static readonly int MAX_SIZE = 1000;
	private static Dictionary<Type,Queue> pools = new Dictionary<Type, Queue> ();
	public static Action<object> AddToPoolHandler;

	private static Queue getPool (Type type)
	{
		return pools.ContainsKey (type) ? pools [type] : pools [type] = new Queue ();
	}
	
	private static Queue getPool <T> ()
	{
		Type type = typeof(T);
		return pools.ContainsKey (type) ? pools [type] : pools [type] = new Queue ();
	}
	
	public static T getObject<T> (params object[] args)
	{
		//Type type = typeof(T);
		Queue pool = getPool<T> ();
		if (pool.Count > 0) {
//			print ("objPool getObject Dequeue-------->" + type);
			return (T)pool.Dequeue ();
		} else {
			return getNewClass<T> (args);
		}
	}
	
	public static bool hasObject<T> ()
	{
		//Type type = typeof(T);
		Queue pool = getPool<T> ();
		return pool.Count > 0;
	}

	public static object getObject (Type type, params object[] args)
	{
		Queue pool = getPool (type);
		if (pool.Count > 0) {
//			print ("objPool getObject Dequeue-------->" + type);
			return pool.Dequeue ();
		} else {
			return getNewClass (type, args);
		}
	}

	public static void addToPool (object obj)
	{
		if (obj == null)
			return;
		
		Type type = obj.GetType ();
		Queue pool = getPool (type);
		if (pool.Contains (obj) == false) {
			if (pool.Count >= MAX_SIZE) {
				pool.Dequeue ();
				RO.LoggerUnused.Log ("object pool-->" + type.ToString () + "has more than " + MAX_SIZE + " ! so shift the first");
			}
			if (AddToPoolHandler != null)
				AddToPoolHandler (obj);
			pool.Enqueue (obj);
		}
//		print ("objPool addToPool-------->" + obj);
	}
	
	private static object getNewClass (Type classRef, params object[] args)
	{
		if (args != null && args.Length > 0) {
			switch (args.Length) {
			case 1:
				return Activator.CreateInstance (classRef, args [0]);
			case 2:
				return Activator.CreateInstance (classRef, args [0], args [1]);
			case 3:
				return Activator.CreateInstance (classRef, args [0], args [1], args [2]);
			case 4:
				return Activator.CreateInstance (classRef, args [0], args [1], args [2], args [3]);
			case 5:
				return Activator.CreateInstance (classRef, args [0], args [1], args [2], args [3], args [4]);
			default:
				break;
			}
		}
		return Activator.CreateInstance (classRef);
	}

	private static T getNewClass <T> (params object[] args)
	{
		Type classRef = typeof(T);
		if (args != null && args.Length > 0) {
			switch (args.Length) {
			case 1:
				return (T)Activator.CreateInstance (classRef, args [0]);
			case 2:
				return (T)Activator.CreateInstance (classRef, args [0], args [1]);
			case 3:
				return (T)Activator.CreateInstance (classRef, args [0], args [1], args [2]);
			case 4:
				return (T)Activator.CreateInstance (classRef, args [0], args [1], args [2], args [3]);
			case 5:
				return (T)Activator.CreateInstance (classRef, args [0], args [1], args [2], args [3], args [4]);
			default:
				break;
			}
		}
		return (T)Activator.CreateInstance (classRef);
	}
}
