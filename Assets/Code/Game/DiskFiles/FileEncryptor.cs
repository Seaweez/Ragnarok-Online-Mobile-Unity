using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using UnityEngine;
using System;

public class FileEncryptor
{
	const string PLIST_KEY_FOR_ENCRYPTO_KEY = "encryptoKey";
	
	public static string Key
	{
		get
		{
			string key = PlayerPrefs.GetString(PLIST_KEY_FOR_ENCRYPTO_KEY);
			if (string.IsNullOrEmpty(key))
			{
				key = GenerateKey();
				PlayerPrefs.SetString(PLIST_KEY_FOR_ENCRYPTO_KEY, key);
			}
			return key;
		}
	}
	
	public static string GenerateKey()
	{
		DESCryptoServiceProvider desCrypto =(DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
		return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
	}

	public static void EncryptFile(string path)
	{
		if (FileHelper.ExistFile(path))
		{
			Stream streamOfFile = FileHelper.GetFileStream(path);
			if (streamOfFile != null)
			{
				string generatedFilePath = FileHelper.GetParentDirectoryPath(path) + '/' + FileHelper.GetFileNameFromPath(path);
				Stream streamOfGeneratedFile = FileHelper.GetFileStreamWithTruncateMode(generatedFilePath);
				if (streamOfGeneratedFile != null)
				{
					RijndaelManaged rmCrypto = new RijndaelManaged();
					UnicodeEncoding ue = new UnicodeEncoding();
					byte[] bytesKey = ue.GetBytes(Key);
					CryptoStream streamCrypto = null;
					try
					{

						streamCrypto = new CryptoStream(streamOfGeneratedFile, rmCrypto.CreateEncryptor(bytesKey, bytesKey), CryptoStreamMode.Write);
						int byteInStream;
						while ((byteInStream = streamOfFile.ReadByte()) != -1)
						{
							streamCrypto.WriteByte((byte)byteInStream);
						}
					}
					catch (Exception e)
					{
						throw e;
					}
					finally
					{
						if (streamCrypto != null)
						{
							streamCrypto.Close();
						}
						streamOfFile.Close();
						streamOfGeneratedFile.Close();
					}
				}
			}
		}
	}
}
