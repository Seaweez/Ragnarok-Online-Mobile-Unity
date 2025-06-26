using UnityEngine;
using System.Collections;

public static class AlgorithmForBubbleSort
{
	public static void BubbleSort(float[] s)
	{
		if (s != null && s.Length > 0)
		{
			for (int i = 0; i < s.Length - 1; ++i)
			{
				for (int j = 1; j < s.Length - i; ++j)
				{
					if (s[j - 1] > s[j])
					{
						s[j - 1] = -s[j - 1] + s[j];
						s[j] = -s[j - 1] + s[j];
						s[j - 1] = s[j - 1] + s[j];
					}
				}
			}
		}
	}
}
