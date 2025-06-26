using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public class RandomArray<T> 
	{
		public T[] randomArray = null;
	
		public T RandomGet()
		{
			if (randomArray.IsNullOrEmpty())
			{
				return default(T);
			}
			return randomArray[Random.Range(0, randomArray.Length)];
		}
	}
} // namespace RO
