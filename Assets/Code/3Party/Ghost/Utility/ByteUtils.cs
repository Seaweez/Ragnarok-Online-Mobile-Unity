using UnityEngine;
using System.Collections.Generic;

namespace Ghost.Utils
{
	public static class ByteUtils {
		
		public static int HashToInt(byte[] data)
		{
			if (null == data)
			{
				return -1;
			}
			if (0 >= data.Length)
			{
				return 0;
			}
			const int M = 0x5bd1e995;
			const int R = 24;
			int len = data.Length;
			int h = len;
			
			for (int i = 0; len >= 4; i += 4, len -= 4)
			{
				int k = System.BitConverter.ToInt32(data, i);
				k *= M;
				k ^= k >> R;
				k *= M;
				h *= M;
				h ^= k;
			}
			if (2 < len)
			{
				h ^= data[2] << 16;
			}
			if (1 < len)
			{
				h ^= data[1] << 8;
			}
			if (0 < len)
			{
				h ^= data[0];
				h *= M;
			}
			
			h ^= h >> 13;
			h *= M;
			h ^= h >> 15;
			return h;
		}
		
	}
} // namespace Ghost.Utils
