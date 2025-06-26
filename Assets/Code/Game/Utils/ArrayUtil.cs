using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrayUtil
{
	public static T[] Intersection<T>(T[] arr1, T[] arr2)
	{
		if (arr1 != null && arr1.Length > 0 && arr2 != null && arr2.Length > 0)
		{
			Dictionary<T, T> dict = new Dictionary<T, T>();
			foreach (T t in arr1)
			{
				dict.Add(t, t);
			}
			List<T> list = new List<T>();
			foreach (T t in arr2)
			{
				if (dict.ContainsKey(t))
				{
					list.Add(t);
				}
			}
			return list.ToArray();
		}
		return null;
	}

	public static T[] Complementary<T>(T[] arr1, T[] arr2)
	{
		if (arr1 != null && arr1.Length > 0)
		{
			Dictionary<T, T> dict = new Dictionary<T, T>();
			if (arr2 != null && arr2.Length > 0)
			{
				foreach (T t in arr2)
				{
					dict.Add(t, t);
				}
			}
			if (dict.Count > 0)
			{
				List<T> list = new List<T>();
				foreach (T t in arr1)
				{
					if (!dict.ContainsKey(t))
					{
						list.Add(t);
					}
				}
				return list.ToArray();
			}
			return arr1;
		}
		return null;
	}
}
