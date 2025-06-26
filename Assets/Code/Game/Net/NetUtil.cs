using System.Text;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
namespace RO.Net
{
	[SLua.CustomLuaClassAttribute]
	public class NetUtil
	{
		#region test
		// zlib test
		public static byte[] GetZlibBytes()
		{
			var s = "120 156 99 100 103 160 54 0 0 2 228 0 9";
			var byteStrings = s.Split(' ');
			var bytes = new byte[byteStrings.Length];
			for (int i = 0, max = byteStrings.Length; i < max; i++) {
				bytes[i] = Convert.ToByte(byteStrings[i]);
			}
			return bytes;
		}
		#endregion

		// convert IP int to IP string
		public static String GetIP(string ip)
		{
			long a = long.Parse(ip);
			string sb = "";
			long b = (a >> 24) & 0xff;
			sb = "." + b;
			b = (a >> 16) & 0xff;
			sb = "." + b + sb;
			b = (a >> 8) & 0xff;
			sb = "." + b + sb;
			b = (a >> 0) & 0xff;
			sb = b + sb;
			return sb;
		}

		private static bool hasInit = false;

		// get new byte array
		// for lua
		public static byte[] GetNewBytes(int length = 0)
		{
			return new byte[length];
		}

		// append
		public static void SetByteByIndex(byte[] bytes, byte b, int index)
		{
			bytes[index] = b;
		}

		// get bytes length
		// for lua
		public static int GetBytesLength(byte[] bytes)
		{
			return bytes.Length;
		}

		// get bytes length
		// for lua
		public static long GetInt64ByHexString(string hex)
		{
			long result = (long)0;
			try{
				result = Convert.ToInt64(hex,16);
			}
			catch(Exception )
			{

			}
			finally{

			}
			return result;
		}

		// append
		public static byte[] Append(byte[] a, byte[] b)
		{
			try {
				byte[] c = new byte[a.Length + b.Length];
				for (int i = 0, max = a.Length; i < max; i++) {
					c[i] = a[i];
				}
				for (int i = a.Length, max = a.Length + b.Length; i < max; i++) {
					c[i] = b[i - a.Length];
				}
				return c;
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::Append Error: " + e.Message);
				return null;
			}
		}

		// get byte from bytes
		public static byte[] GetBytes(byte[] value, int start, int length)
		{
			try {
				byte[] result = new byte[length];
				for (int i = 0, max = length; i < max; i++) {
					result[i] = value[start + i];
				}
				return result;
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::GetBytes Error: " + e.Message);
				return new byte[length];
			}
		}

		#region bytes
		// bytes to string
		// just for log
		public static string BytesToString(byte[] value)
		{
			try {
				StringBuilder sb = new StringBuilder();
				for (int i = 0, max = value.Length; i < max; i++) {
					sb.Append(value[i] + " ");
				}
				return sb.ToString();
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::BytesToString Error: " + e.Message);
				return "";
			}
		}

		public static string BytesToStringByLen(byte[] value,int index = 0,int len =0)
		{
			len = len == 0? value.Length:len;
			try {
				StringBuilder sb = new StringBuilder();
				for (int i = index, max = len; i < max; i++) {
					sb.Append(value[i] + " ");
				}
				return sb.ToString();
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::BytesToString Error: " + e.Message);
				return "";
			}
		}

		// char
		// lua dont has char so save as string
		public static string BytesToChar(byte[] value)
		{
			try {
				return Convert.ToInt16(value[0]).ToString();
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::BytesToChar Error: " + e.Message);
				return "";
			}
		}

		// char
		// lua dont has char so save as string
		public static string BytesToChars(byte[] value)
		{
			try {
				return Encoding.UTF8.GetString(value);
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::BytesToChars Error: " + e.Message);
				return "";
			}
		}

		// bytes to int 
		public static int BytesToInt4(byte[] value, int offset = 0)
		{
			try {
//				byte[] bytes = new byte[] {
//					0,
//					0,
//					0,
//					0
//				};
//				for (int i = 0, max = value.Length; i < max; i++) {
//					bytes[i] = value[i];
//				}
				return BitConverter.ToInt32(value, offset);
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::BytesToInt4 Error: " + e.Message);
				return 0;
			}
		}

		// bytes to uint2
		public static uint BytesToUInt2(byte[] value, int offset = 0)
		{
			try {
				return BitConverter.ToUInt16(value, offset);
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::BytesToUint2 Error: " + e.Message);
				return 0;
			}
		}

		// bytes to uint4
		public static uint BytesToUInt4(byte[] value)
		{
			try {
				return BitConverter.ToUInt32(value, 0);
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::BytesToUInt4 Error: " + e.Message);
				return 0;
			}
		}

		// long long
		// lua number is 32-bit so we must save long long as string
		public static string BytesToUInt8(byte[] value)
		{
			try {
				return BitConverter.ToUInt64(value, 0).ToString();
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::BytesToUInt8 Error: " + e.Message);
				return "";
			}
		}
		#endregion

		#region uint
		// uint to 1 byte array
		public static byte[] UintTo1Bytes(uint value)
		{
			try {
				return new byte[] {
					Convert.ToByte(value)
				};
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::UintTo1Bytes Error: " + e.Message);
				return new byte[1];
			}
		}

		// uint to 2 byte array
		public static byte[] UintTo2Bytes(uint value)
		{
			try {
				return new byte[] {
					Convert.ToByte((value & 0xff)),
					Convert.ToByte((value >> 8) & 0xff)
				};
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::UintTo2Bytes Error: " + e.Message);
				return new byte[2];
			}
		}

		// uint to 4 byte array
		public static byte[] UintTo4Bytes(uint value)
		{
			try {
				return new byte[] {
					Convert.ToByte((value & 0xff)),
					Convert.ToByte((value >> 8) & 0xff),
					Convert.ToByte((value >> 16) & 0xff),
					Convert.ToByte((value >> 24) & 0xff)
				};
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::UintTo4Bytes Error: " + e.Message);
				return null;
			}
		}

		// uint to 8 byte array
		// long long
		// lua number is 32-bit so we must save long long as string
		public static byte[] UintTo8Bytes(string value)
		{
			try {
				ulong v = Convert.ToUInt64(value);

				return new byte[] {
					Convert.ToByte((v & 0xff)),
					Convert.ToByte((v >> 8) & 0xff),
					Convert.ToByte((v >> 16) & 0xff),
					Convert.ToByte((v >> 24) & 0xff),
					Convert.ToByte((v >> 32) & 0xff),
					Convert.ToByte((v >> 40) & 0xff),
					Convert.ToByte((v >> 48) & 0xff),
					Convert.ToByte((v >> 56) & 0xff)
				};
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::UintTo8Bytes Error: " + e.Message);
				return new byte[8];
			}
		}
		#endregion

		#region int
		// int to 1 byte array
		public static byte[] IntTo1Bytes(int value)
		{
			try {
				return new byte[] {
					(byte)(value)
				};
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::IntTo1Bytes Error: " + e.Message);
				return new byte[1];
			}
		}

		// int to 2 byte array
		public static byte[] IntTo2Bytes(int value)
		{
			try {
				return new byte[] {
					Convert.ToByte((value & 0xff)),
					Convert.ToByte((value >> 8) & 0xff)
				};
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::IntTo2Bytes Error: " + e.Message);
				return new byte[2];
			}
		}

		// int to 4 byte array
		public static byte[] IntTo4Bytes(int value)
		{
			try {
				return new byte[] {
					Convert.ToByte((value & 0xff)),
					Convert.ToByte((value >> 8) & 0xff),
					Convert.ToByte((value >> 16) & 0xff),
					Convert.ToByte((value >> 24) & 0xff),
				};
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::IntTo4Bytes Error: " + e.Message);
				return new byte[4];
			}
		}
		#endregion

		#region char
		// char to 1byte array 
		public static byte[] CharTo1Bytes(string value)
		{
			try {
				return new byte[] {
					Convert.ToByte(Convert.ToChar(Convert.ToInt16(value)))
				};
			}
			catch (Exception e) {
				if (value != null && value.Length > 0)
				{
					NetLog.LogE("NetUtil::CharTo1Bytes Error: " + e.Message);
				}
				return new byte[1];
			}
		}

		// char to byte array
		public static byte[] CharsToBytes(string value, uint length)
		{
			try {				
				var result = new char[length];
				if (value != null) {
					value.CopyTo(0, result, 0, value.Length);
				}
				var bytes = new byte[length];
				for (int i = 0; i < length; i++)
				{
					bytes[i] = Convert.ToByte(result[i]);
				}
				return bytes;
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::CharsToBytes Error: " + e.Message);
				return new byte[length];
			}
		}
		#endregion

		#region toFile
		// char to 1byte array 
		public static void WriteFile(String fileName,byte[] bytes,int lenght)
		{
			DirectoryInfo dir = new DirectoryInfo("ByteFile");
			if(!dir.Exists)
			{
				dir.Create();
			}else if(!hasInit)
			{
				hasInit = true;
				FileInfo[] infos = dir.GetFiles();
				foreach(var info in infos)
				{
					info.Delete();
				}
			}
			fileName = String.Format("ByteFile/{0}",fileName);
			try {
				StringBuilder sb = new StringBuilder();
				for (int i = 0, max = lenght; i < max; i++) {
					sb.Append(bytes[i] + " ");
				}
				FileStream fs = new FileStream(fileName,FileMode.Create);
				StreamWriter sw = new StreamWriter(fs,Encoding.Default);
				sw.Write(sb.ToString());
				sw.Close();
				fs.Close();
			}
			catch (Exception e) {
				NetLog.LogE("NetUtil::WriteFile Error: " + e.Message);
			}
		}

		public static bool IsIP(string ip)
		{
			return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
		}
		
		public static bool IsIPSect(string ip)
		{
			return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
		}

		public static string getSha1(string nonceStr) 
		{
			SHA1 shai = new SHA1CryptoServiceProvider();
			ASCIIEncoding enc = new ASCIIEncoding();
			byte[] dataToHash = enc.GetBytes(nonceStr);
			dataToHash = shai.ComputeHash(dataToHash);

			StringBuilder sBuilder = new StringBuilder();
			for (int i = 0; i < dataToHash.Length; i++)
			{
				sBuilder.Append(dataToHash[i].ToString("x2"));
			}

			var str = sBuilder.ToString();
			shai.Clear();
			return str;
			// Return the hexadecimal string.

//			var str = BitConverter.ToString(dataToHash);
//			str = str.Replace("-", "");
//			str = str.ToLower();
//			shai.Clear();
//			return str;
			//			return dataToHash.ToString();
		}

		private static byte[] DesSecKey = new byte[]{
			95,27,5,20,131,4,8,88
		};

		public static byte[] Decrypt(byte[] encryptedData,int length)  
		{
			try
			{
				MemoryStream msDecrypt = new MemoryStream(encryptedData,0,length);
				DESCryptoServiceProvider des = new DESCryptoServiceProvider();
				des.Mode = CipherMode.ECB;
				des.Padding = PaddingMode.Zeros;
				ICryptoTransform ict = des.CreateDecryptor(DesSecKey, DesSecKey);
				CryptoStream csDecrypt = new CryptoStream(msDecrypt, ict, CryptoStreamMode.Read);

				// Create buffer to hold the decrypted data.
				byte[] fromEncrypt = new byte[length];

				csDecrypt.Read(fromEncrypt, 0, length);

				csDecrypt.Close();
				msDecrypt.Close();

				return fromEncrypt;
			}
			catch(CryptographicException e)
			{
				ROLogger.LogErrorFormat("Decrypt:A Cryptographic error occurred: {0}", e.Message);
				return null;
			} 
		}

		public static byte[] Encrypt(byte[] encryptData,int offset,int length)  
		{  
			try
			{
				// Create a MemoryStream.

				byte[] temp = new byte[length];
				Buffer.BlockCopy (encryptData, offset, temp, 0, length);
				MemoryStream mStream = new MemoryStream();
				DESCryptoServiceProvider des = new DESCryptoServiceProvider();
				des.Mode = CipherMode.ECB;
				des.Padding = PaddingMode.Zeros;

				ICryptoTransform ict = des.CreateEncryptor(DesSecKey, DesSecKey);
				CryptoStream cStream = new CryptoStream(mStream, ict,CryptoStreamMode.Write);
				 
//				cStream.Write(encryptData, offset, length);
				cStream.Write(temp, 0, length);
				cStream.FlushFinalBlock();

				byte[] ret = mStream.ToArray();

				cStream.Close();
				mStream.Close();

				return ret;
			}
			catch(CryptographicException e)
			{
				ROLogger.LogErrorFormat("Encrypt:A Cryptographic error occurred: {0}", e.Message);
				return null;
			}

		}

		#endregion
	}
}
