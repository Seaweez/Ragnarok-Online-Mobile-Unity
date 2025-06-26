using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Ghost.Utils;

namespace Ghost.Extensions
{
	public static class StringExtensions {

		public static byte[] ToBytes(this string str, Encoding encoding)
		{
			return encoding.GetBytes(str);
		}
		
		public static int ToHashInt(this string str, Encoding encoding)
		{
			if (null == str)
			{
				return -1;
			}
			if (0 >= str.Length)
			{
				return 0;
			}
			return str.ToBytes(encoding).ToHashInt();
		}

		public static string[] SplitDigit(this string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return null;
			}

			var parts = new List<string>();

			bool findingDigit = true;
			int startIndex = 0;
			var strLen = str.Length;
			for (int i = 0; i < strLen; ++i)
			{
				if (findingDigit)
				{
					if (char.IsDigit(str[i]))
					{
						startIndex = i;
						findingDigit = false;
					}
				}
				else
				{
					if (!char.IsDigit(str[i]))
					{
						parts.Add(str.Substring(startIndex, i-startIndex));
						findingDigit = true;
					}
				}
			}
			if (!findingDigit && startIndex < strLen)
			{
				parts.Add(str.Substring(startIndex, strLen-startIndex));
			}

			return parts.ToArray();
		}

		public static int GetDBCCaseLength(this string str)
		{
			int len = 0;
			foreach (var c in str)
			{
				len += StringUtils.GetDBCCaseLength(c);
			}
			return len;
		}
		
	}
} // namespace Ghost.Extensions
