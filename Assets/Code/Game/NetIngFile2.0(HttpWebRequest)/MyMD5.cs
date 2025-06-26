using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

[SLua.CustomLuaClassAttribute]
public class MyMD5
{
	public static string HashFile(string path)
	{
		string retValue = "";
		if (File.Exists(path))
		{
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			byte[] bytes = md5.ComputeHash(fs);
			fs.Close();
			string str = System.BitConverter.ToString(bytes);
			str = str.Replace("-", "");
			retValue = str.ToLower();
		}
		return retValue;
	}

	public static string HashBytes(byte[] bytes)
	{
		string retValue = "";
		if (bytes != null && bytes.Length > 0)
		{
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] bytesHashResult = md5.ComputeHash(bytes);
			string strHashResult = BitConverter.ToString(bytesHashResult);
			strHashResult = strHashResult.Replace("-", "");
			retValue = strHashResult.ToLower();
		}
		return retValue;
	}

	public static string HashString(string str)
	{
		string retValue = "";
		if (!string.IsNullOrEmpty(str))
		{
			MD5 m = new MD5CryptoServiceProvider();
			byte[] bytes = m.ComputeHash(Encoding.UTF8.GetBytes(str));
			string strHashResult = BitConverter.ToString(bytes);
			strHashResult = strHashResult.Replace("-", "");
			retValue = strHashResult.ToLower();
		}
		return retValue;
	}
}
