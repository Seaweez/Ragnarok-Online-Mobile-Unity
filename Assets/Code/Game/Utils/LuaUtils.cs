using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Ghost.Extensions;
using LitJson;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public static class LuaUtils
	{
#if OBSOLETE
		public static List<RoleAgent> ToRoleList (SLua.LuaTable table)
		{
			if (null == table) {
				return null;
			}
			var list = new List<RoleAgent> ();
			foreach (var key_value in table) {
				var v = key_value.value as RoleAgent;
				if (null != v) {
					list.Add (v);
				}
			}
			return list;
		}

		public static void SortRoleList(List<RoleAgent> roles, System.Func<RoleAgent, RoleAgent, int> comparison)
		{
			roles.Sort(delegate(RoleAgent x, RoleAgent y) {
				return comparison(x, y);
			});
		}
#endif

		public static List<Color> CreateColorList ()
		{
			return new List<Color> ();
		}

		public static void AddColorToList (List<Color> list, Color c)
		{
			list.Add (c);
		}

		public static List<string> CreateStringList ()
		{
			return new List<string> ();
		}

		public static void SetRotation (GameObject obj, Vector3 sourcePosition, Vector3 targetPosition)
		{
			var transform = obj.transform;

			var dir = (targetPosition - sourcePosition).normalized;
			var lookAtPosition = transform.position + dir;
			lookAtPosition.y = transform.position.y;

			transform.LookAt (lookAtPosition);
		}

		public static bool RaycastTerrain (Vector3 screenPoint, out float x, out float y, out float z)
		{
			Ray ray = Camera.main.ScreenPointToRay (screenPoint);

			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, float.PositiveInfinity, LayerMask.GetMask (Config.Layer.TERRAIN.Key))) {
				x = hit.point.x;
				y = hit.point.y;
				z = hit.point.z;
				return true;
			}

			x = 0;
			y = 0;
			z = 0;
			return false;
		}

		public static bool RaycastAccessable (Vector3 screenPoint, out GameObject hitObj)
		{
			Ray ray = Camera.main.ScreenPointToRay (screenPoint);
			
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, float.PositiveInfinity, LayerMask.GetMask (Config.Layer.ACCESSABLE.Key))) {
				hitObj = hit.collider.gameObject;
				return true;
			}
			
			hitObj = null;
			return false;
		}

		public static string StrToUtf8 (string s)
		{
			return  System.Text.Encoding.UTF8.GetString (System.Text.Encoding.UTF8.GetBytes (s));
		}

		public static Quaternion GetLookRotation (Vector3 origin, Vector3 target)
		{
			target.y = origin.y;
			var direction = target - origin;
			var targetAngle = Mathf.Atan2 (direction.x, direction.z) * Mathf.Rad2Deg;
			return Quaternion.Euler (new Vector3 (0, targetAngle, 0));
		}

		public static bool StringEquals (string s1, string s2)
		{
			return string.Equals (s1, s2);
		}

		public static bool ColorEquals (Color c1, Color c2)
		{
			return Color.Equals (c1, c2);
		}

		public static bool AnimatorHelperEquals (AnimatorHelper a1, AnimatorHelper a2)
		{
			return AnimatorHelper.Equals (a1, a2);
		}

		public static string SubString (string beSubedStr, int startIndex, int length)
		{
			if (string.IsNullOrEmpty (beSubedStr))
				return null;
			return beSubedStr.Substring (startIndex, length);
		}

		public static string SubString (string beSubedStr, int startIndex)
		{
			return LuaUtils.SubString (beSubedStr, startIndex, beSubedStr.Length - startIndex);
		}

		public static Vector3 RandomInsideSphere (float radii)
		{
			return Random.insideUnitSphere * radii;
		}

		public static int StringLength (string str)
		{
			if (string.IsNullOrEmpty (str))
				return 0;
			return str.Length;
		}

		private static void JsonObjectToLua (JsonData data, WriterAdapter writer)
		{
			var keys = data.Keys;
			if (null != keys && 0 < keys.Count)
			{
				writer.WriteStructStart();
				foreach (var k in keys)
				{
					var element = data[k];
					if (null != element)
					{
						writer.WriteMemberName(k);
						JsonDataToLua(element, writer);
					}
				}
				writer.WriteStructEnd();
			}
		}

		private static void JsonArrayToLua (JsonData data, WriterAdapter writer)
		{
			var count = data.Count;
			if (0 < count)
			{
				writer.WriteArrayStart();
				for (int i = 0; i < count; ++i)
				{
					var element = data[i];
					if (null != element)
					{
						JsonDataToLua(element, writer);
					}
				}
				writer.WriteArrayEnd();
			}
		}

		private static void JsonDataToLua (JsonData data, WriterAdapter writer)
		{
			var jsonType = data.GetJsonType();
			switch (jsonType)
			{
			case JsonType.None:
				writer.WriteMemberValue("nil");
				break;
			case JsonType.Boolean:
				writer.WriteMemberValue(bool.Parse(data.ToString()));
				break;
			case JsonType.Double:
				writer.WriteMemberValue(double.Parse(data.ToString()));
				break;
			case JsonType.Int:
				writer.WriteMemberValue(int.Parse(data.ToString()));
				break;
			case JsonType.Long:
				writer.WriteMemberValue(long.Parse(data.ToString()));
				break;
			case JsonType.String:
				writer.WriteMemberValue(data.ToString());
				break;
			case JsonType.Object:
				JsonObjectToLua(data, writer);
				break;
			case JsonType.Array:
				JsonArrayToLua(data, writer);
				break;
			}
		}

		public static string JsonToLua (string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return null;
			}
			try
			{
				var data = JsonMapper.ToObject(str);
				if (null == data)
				{
					return null;
				}

				System.Text.StringBuilder sb = new System.Text.StringBuilder ();
				var writer =DataWriter.GetAdapter(new LuaWriter (sb), null);
				JsonDataToLua(data, writer);

				if (0 < sb.Length)
				{
					return sb.ToString();
				}
			}
			catch (System.Exception e)
			{
				BuglyAgent.PrintLog(LogSeverity.LogError, "{0}", e);
			}
			return null;
		}


		public static void PlayMovie (string filePath, bool canSkip)
		{
			#if UNITY_IOS || UNITY_ANDROID
			filePath = Path.Combine ("Videos", filePath);
			string persistentDataPath = Application.persistentDataPath + "/" + ApplicationHelper.platformFolder + "/";
			string fileName = Path.Combine (persistentDataPath, filePath);
			if (File.Exists (fileName) == false) {
				fileName = PathHelper.GetPathURL (Path.Combine (Application.streamingAssetsPath, filePath));
			} else {
				#if UNITY_IOS
				fileName = "file://" + fileName;
				#endif
			}
			Handheld.PlayFullScreenMovie (fileName, Color.black, canSkip ? FullScreenMovieControlMode.CancelOnInput : FullScreenMovieControlMode.Hidden);
			#endif
		}



		public static short HostToNetworkOrder_16(short v)
		{
			return System.Net.IPAddress.HostToNetworkOrder(v);
		}
		public static int HostToNetworkOrder_32(int v)
		{
			return System.Net.IPAddress.HostToNetworkOrder(v);
		}
		public static long HostToNetworkOrder_64(long v)
		{
			return System.Net.IPAddress.HostToNetworkOrder(v);
		}

		public static short NetworkToHostOrder_16(short v)
		{
			return System.Net.IPAddress.NetworkToHostOrder(v);
		}
		public static int NetworkToHostOrder_32(int v)
		{
			return System.Net.IPAddress.NetworkToHostOrder(v);
		}
		public static long NetworkToHostOrder_64(long v)
		{
			return System.Net.IPAddress.NetworkToHostOrder(v);
		}
	}

	[SLua.CustomLuaClassAttribute]
	public class ByteStream
	{
		private MemoryStream stream;

		public byte[] data
		{
			get
			{
				return stream.GetBuffer();
			}
		}

		public long dataLen
		{
			get
			{
				return stream.Length;
			}
		}

		public ByteStream()
		{
			stream = new MemoryStream();
		}

		public ByteStream(byte[] data)
		{
			stream = new MemoryStream(data);
		}

		public ByteStream(byte[] data, int index, int count)
		{
			stream = new MemoryStream(data, index, count);
		}

		public int WriteNetwork_8(byte v)
		{
			stream.WriteByte(v);
			return 1;
		}

		public int WriteNetwork_16(short v)
		{
			var bytes = System.BitConverter.GetBytes(LuaUtils.HostToNetworkOrder_16(v));
			stream.Write(bytes, 0, bytes.Length);
			return bytes.Length;
		}

		public int WriteNetwork_32(int v)
		{
			var bytes = System.BitConverter.GetBytes(LuaUtils.HostToNetworkOrder_32(v));
			stream.Write(bytes, 0, bytes.Length);
			return bytes.Length;
		}

		public int WriteNetwork_64(long v)
		{
			var bytes = System.BitConverter.GetBytes(LuaUtils.HostToNetworkOrder_64(v));
			stream.Write(bytes, 0, bytes.Length);
			return bytes.Length;
		}

		public int WriteNetwork_array_8(byte[] v)
		{
			int len = v.Length;
			var writtenLen = WriteNetwork_32(len);

			stream.Write(v, 0, v.Length);
			return writtenLen+v.Length;
		}

		public int WriteNetwork_array_16(short[] v)
		{
			int len = v.Length;
			var writtenLen = WriteNetwork_32(len);

			for (int i = 0; i < v.Length; ++i)
			{
				writtenLen += WriteNetwork_16(v[i]);
			}
			return writtenLen;
		}

		public int WriteNetwork_array_32(int[] v)
		{
			int len = v.Length;
			var writtenLen = WriteNetwork_32(len);
			
			for (int i = 0; i < v.Length; ++i)
			{
				writtenLen += WriteNetwork_32(v[i]);
			}
			return writtenLen;
		}
		
		public int WriteNetwork_array_64(long[] v)
		{
			int len = v.Length;
			var writtenLen = WriteNetwork_32(len);
			
			for (int i = 0; i < v.Length; ++i)
			{
				writtenLen += WriteNetwork_64(v[i]);
			}
			return writtenLen;
		}
		
		public int WriteNetwork_string(string v)
		{
			var bytes = System.Text.Encoding.UTF8.GetBytes(v);
			return WriteNetwork_array_8(bytes);
		}

		public byte ReadNetwork_8()
		{
			return (byte)stream.ReadByte();
		}

		public short ReadNetwork_16()
		{
			var data = new byte[2];
			stream.Read (data, 0, data.Length);
			return LuaUtils.NetworkToHostOrder_16(System.BitConverter.ToInt16(data, 0));
		}
		
		public int ReadNetwork_32()
		{
			var data = new byte[4];
			stream.Read (data, 0, data.Length);
			return LuaUtils.NetworkToHostOrder_32(System.BitConverter.ToInt32(data, 0));
		}
		
		public long ReadNetwork_64()
		{
			var data = new byte[8];
			stream.Read (data, 0, data.Length);
			return LuaUtils.NetworkToHostOrder_64(System.BitConverter.ToInt64(data, 0));
		}

		public byte[] ReadNetwork_array_8()
		{
			var len = ReadNetwork_32();
			if (0 < len)
			{
				var data = new byte[len];
				stream.Read (data, 0, data.Length);
				return data;
			}
			return null;
		}
		
		public short[] ReadNetwork_array_16()
		{
			var len = ReadNetwork_32();
			if (0 < len)
			{
				var list = new List<short>();
				for (int i = 0; i < len; ++i)
				{
					list.Add(ReadNetwork_16());
				}
				return list.ToArray();
			}
			return null;
		}
		
		public int[] ReadNetwork_array_32()
		{
			var len = ReadNetwork_32();
			if (0 < len)
			{
				var list = new List<int>();
				for (int i = 0; i < len; ++i)
				{
					list.Add(ReadNetwork_32());
				}
				return list.ToArray();
			}
			return null;
		}
		
		public long[] ReadNetwork_array_64()
		{
			var len = ReadNetwork_32();
			if (0 < len)
			{
				var list = new List<long>();
				for (int i = 0; i < len; ++i)
				{
					list.Add(ReadNetwork_64());
				}
				return list.ToArray();
			}
			return null;
		}

		public string ReadNetwork_string()
		{
			var bytes = ReadNetwork_array_8();
			if (!bytes.IsNullOrEmpty())
			{
				return System.Text.Encoding.UTF8.GetString(bytes);
			}
			return null;
		}
	}

} // namespace RO
