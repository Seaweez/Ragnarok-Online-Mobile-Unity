using System.Text;
using Ghost.Extensions;
using System.Text.RegularExpressions;

namespace Ghost.Utils
{
	public static class StringUtils
	{
		private static StringBuilder builder = new StringBuilder();

		public static string ArrayToString<T> (T[] objs)
		{
			if (objs.IsNullOrEmpty ()) {
				return string.Empty;
			}
			builder.Length = 0;
			for (int i = 0; i < objs.Length; ++i) {
				builder.Append (objs [i]);
			}
			return builder.ToString ();
		}

		public static string ConnectToString (params object[] objs)
		{
			return ArrayToString (objs);
		}

		public static string ArrayToStringWithSeparator<T> (string separator, T[] objs)
		{
			if (objs.IsNullOrEmpty ()) {
				return string.Empty;
			}
			builder.Length = 0;
			builder.Append (objs [0]);
			for (int i = 1; i < objs.Length; ++i) {
				builder.Append (separator).Append (objs [i]);
			}
			return builder.ToString ();
		}

		public static string ConnectToStringWithSeparator (string separator, params object[] objs)
		{
			return ArrayToStringWithSeparator (separator, objs);
		}

		public static int GetDBCCaseLength (char c)
		{
			var bytes = Encoding.Unicode.GetBytes (new char[]{c});
			return (1 < bytes.Length && 0 != bytes[1]) ? 2 : 1;
		}

		public static bool IsEmojiCharacter (char c)
		{
			//反向合法字符
			Regex reg = new Regex ("[^A-Za-z\\d\\u4E00-\\u9FA5\\p{P}‘’“”]+$");
			byte[] bc = Encoding.UTF8.GetBytes (new char[]{c});
			return reg.IsMatch (Encoding.UTF8.GetString (bc));
		}

		public static bool IsPunctuationCharacter (char c)
		{
			return char.IsPunctuation (c);
		}

		public static bool IsSymbolCharacter (char c)
		{
			return char.IsSymbol (c);
		}

		public static bool IsSeparatorCharacter (char c)
		{
			return char.IsSeparator (c);
		}

		public static string[] Split (string str, char[] separators)
		{
			if (string.IsNullOrEmpty (str))
			{
				return null;
			}
			else
			{
				return str.Split (separators);
			}
		}

	}
} // namespace Ghost.Utils
