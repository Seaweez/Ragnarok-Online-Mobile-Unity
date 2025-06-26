using System.Collections.Generic;
using System.Text;
using Ghost.Utils;

namespace Ghost.Extensions
{
	public static class CollectionExtensions {

		// array
		public static bool IsNullOrEmpty<T>(this T[] array)
		{
			return null == array || 0 >= array.Length;
		}

		public static string DumpString<T>(this T[] array)
		{
			if (array.IsNullOrEmpty())
			{
				return "[]";
			}
			return StringUtils.ConnectToString('[', StringUtils.ArrayToStringWithSeparator(", ", array), ']');
		}

		public static bool CheckIndex<T>(this T[] array, int index)
		{
			return 0 <= index && array.Length > index;
		}

		public static int ToHashInt(this byte[] bytes)
		{
			return ByteUtils.HashToInt(bytes);
		}

		// collection
		public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
		{
			return null == collection || 0 >= collection.Count;
		}

		public static bool CheckIndex<T>(this ICollection<T> collection, int index)
		{
			return 0 <= index && collection.Count > index;
		}

		// enumrable
		public static T[] ToArray<T>(this IEnumerable<T> enumerable)
		{
			if (null == enumerable)
			{
				return null;
			}
			var list = new List<T>();
			var enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				list.Add(enumerator.Current);
			}
			return list.ToArray();
		}

		public static string DumpString<T>(this IEnumerable<T> enumerable)
		{
			return enumerable.ToArray().DumpString();
		}

		// list
		public static List<T> ToUnique<T>(this List<T> list)
		{
			if (1 > list.Count) 
			{
				return list;
			}
			list.ToArray();
			list.Sort ();
			List<T> uniqueList = new List<T> ();
			foreach (T obj in list) 
			{
				if (0 >= uniqueList.Count || !object.Equals (uniqueList[uniqueList.Count-1], obj)) 
				{
					uniqueList.Add (obj);
				}
			}
			return uniqueList;
		}

		public static List<T> ToNotUnique<T>(this List<T> list)
		{
			if (1 > list.Count) 
			{
				return list;
			}
			list.ToArray();
			list.Sort ();
			List<T> uniqueList = new List<T> ();
			List<T> notUniqueList = new List<T> ();
			foreach (T obj in list) 
			{
				if (0 >= uniqueList.Count || !object.Equals (uniqueList[uniqueList.Count-1], obj)) 
				{
					uniqueList.Add (obj);
				}
				else if (0 >= notUniqueList.Count || !object.Equals (notUniqueList[notUniqueList.Count-1], obj))
				{
					notUniqueList.Add (obj);
				}
			}
			return notUniqueList;
		}

		public static void MakeUnique<T>(this List<T> list)
		{
			var uniqueList = list.ToUnique();
			if (null != uniqueList)
			{
				list.Clear();
				list.AddRange(uniqueList);
			}
		}
		
	}
} // namespace Ghost.Extensions
