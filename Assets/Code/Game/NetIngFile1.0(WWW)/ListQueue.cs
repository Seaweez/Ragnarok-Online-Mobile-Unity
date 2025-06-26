using UnityEngine;
using System.Collections;

public class ListQueue<T> : System.Collections.Generic.List<T>
{
	public void Enqueue(T item)
	{
		base.Add(item);
	}
	
	public T Dequeue()
	{
		var t = base[0]; 
		base.RemoveAt(0);
		return t;
	}
	
	public T Peek()
	{
		return base[0];
	}
}