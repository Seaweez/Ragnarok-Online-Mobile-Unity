//#define _FILE_ENCRYPTION_

using System.Collections;
using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
using RO;

[SLua.CustomLuaClassAttribute]
public class FileDirectoryHandler
{
	private static string m_sandBoxPath = Application.persistentDataPath;

	public static string GetAbsolutePath(string path)
	{
		if (string.IsNullOrEmpty (path)) {
			return m_sandBoxPath;
		}
		if (path.Contains (m_sandBoxPath)) {
			return path;
		}
		return m_sandBoxPath + "/" + path;
	}

	public static bool ExistDirectory(string path)
	{
		bool b = false;
		string absolutePath = GetAbsolutePath(path);
		if (absolutePath != null) {
			try
			{
				b = Directory.Exists (absolutePath);
			}
			catch (IOException e)
			{
				ROLogger.LogErrorFormat("ExistDirectory failed: {0}", e);
			}
		}
		return b;
	}

	public static bool CreateDirectory(string path)
	{
		bool b = false;
		if (!ExistDirectory (path)) {
			string absolutePath = GetAbsolutePath (path);
			if (absolutePath != null) {
				try
				{
					Directory.CreateDirectory(absolutePath);
					b = true;
				}
				catch (IOException e) {
					ROLogger.LogErrorFormat("CreateDirectory failed: {0}", e);
					b = false;
				}
			}
		}
		return b;
	}

	public static bool DeleteDirectory(string path)
	{
		bool b = false;
		if (ExistDirectory (path)) {
			string absolutePath = GetAbsolutePath (path);
			if (absolutePath != null) {
				try
				{
					Directory.Delete (absolutePath, true);
					b = true;
				}
				catch (IOException e) {
					ROLogger.LogErrorFormat("DeleteDirectory failed: {0}", e);
					b = false;
				}
			}
		}
		return b;
	}

	public static bool DeleteFile(string path)
	{
		bool b = false;
		if (ExistFile (path)) {
			string absolutePath = GetAbsolutePath (path);
			if (absolutePath != null) {
				try
				{
					File.Delete(absolutePath);
					b = true;
				}
				catch (IOException e) {
					ROLogger.LogErrorFormat("DeleteFile failed: {0}", e);
					b = false;
				}
			}
		}
		return b;
	}

	public static bool[] WriteFile(string path, byte[] bytes)
	{
		bool[] retValue = new bool[]{false, false};
		if (bytes != null && bytes.Length > 0)
		{
			bool exist = ExistFile(path);
			retValue[0] = exist;

			string absolutePath = GetAbsolutePath(path);
			if (absolutePath != null) {
				FileStream stream = null;
				try {
					stream = File.Open (absolutePath, FileMode.Create, FileAccess.Write, FileShare.None);
					stream.Write (bytes, 0, bytes.Length);
					retValue[1] = true;
				} catch (Exception e) {
					ROLogger.LogErrorFormat ("WriteFile failed: {0}", e);
					retValue [1] = false;
				} finally {
					if (null != stream) {
						stream.Close ();
						stream = null;
					}
				}
				#if _FILE_ENCRYPTION_
				EncryptFile(path);
				DeleteFile(path);
				#endif
			}
		}
		return retValue;
	}

	public static void WriteFile(string path, byte[] bytes, Action<bool, bool> on_complete)
	{
		MonoFileDirectoryHandler.ins.WriteFile(path, bytes, (x, y) => {
			if (y)
			{
#if _FILE_ENCRYPTION_
				EncryptFile(path);
				DeleteFile(path);
#endif
			}
			if (on_complete != null)
			{
				on_complete(x, y);
			}
		});
	}

//	public static void WriteFile(string path, byte[] bytes, Action<bool, bool> on_complete)
//	{
//		if (!string.IsNullOrEmpty(path))
//		{
//			if (bytes != null && bytes.Length > 0)
//			{
//				bool exist = FileDirectoryHandler.ExistFile(path);
//				string absolutePath = FileDirectoryHandler.GetAbsolutePath(path);
//				Stream stream = File.Open(absolutePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
//				try
//				{
//					bool done = false;
//					stream.BeginWrite(bytes, 0, bytes.Length, new AsyncCallback((x) => {
//						Stream _stream = (Stream)x.AsyncState;
//						_stream.Close();
//						done = true;
//					}), stream);
//					while (true)
//					{
//						if (done)
//						{
//							if (on_complete != null)
//								on_complete(exist, true);
//							break;
//						}
//					}
//				}
//				catch (Exception e)
//				{
//					Logger.LogWarning(e);
//				}
//				finally
//				{
//					stream.Close();
//					if (on_complete != null)
//						on_complete(exist, false);
//				}
//			}
//		}
//		else
//		{
//			if (on_complete != null)
//				on_complete(false, false);
//		}
//	}

	public static byte[] LoadFile(string path)
	{
		byte[] retValue = null;
		if (ExistFile(path))
		{
#if _FILE_ENCRYPTION_
			DecryptFile(path);
#endif
			string absolutePath = GetAbsolutePath(path);
			if (absolutePath != null) {
				FileStream stream = null;
				try {
					stream = File.Open (absolutePath, FileMode.Open, FileAccess.Read, FileShare.Read);
					byte[] bytes = new byte[stream.Length];
					stream.Read (bytes, 0, bytes.Length);
					retValue = bytes;
				} catch (Exception e) {
					ROLogger.LogErrorFormat ("LoadFile failed: {0}", e);
				} finally {
					if (null != stream) {
						stream.Close ();
						stream = null;
					}
				}
				#if _FILE_ENCRYPTION_
				DeleteFile(path);
				#endif
			}
		}
		return retValue;
	}

	public static void LoadFile(string path, Action<FileStream> action)
	{
		if (ExistFile(path) && null != action)
		{
#if _FILE_ENCRYPTION_
			DecryptFile(path);
#endif
			string absolutePath = GetAbsolutePath (path);
			if (absolutePath != null) {
				FileStream stream = null;
				try {
					stream = File.Open (absolutePath, FileMode.Open, FileAccess.Read, FileShare.Read);
					action (stream);
				} catch (Exception e) {
					ROLogger.LogErrorFormat ("LoadFile failed: {0}", e);
				} finally {
					if (null != stream) {
						stream.Close ();
						stream = null;
					}
				}
				#if _FILE_ENCRYPTION_
				DeleteFile(path);
				#endif
			}
		}
	}

	public static bool ExistFile(string path)
	{
		string absolutePath = GetAbsolutePath(path);
		if (absolutePath != null) {
			return File.Exists (absolutePath);
		}
		return false;
	}

	public static string[] GetChildrenPath(string path)
	{
		string[] retValue = null;
		if (ExistDirectory (path)) {
			string absolutePath = GetAbsolutePath (path);
			if (absolutePath != null) {
				try
				{
					DirectoryInfo di = new DirectoryInfo (absolutePath);
					List<string> pathList = new List<string> ();
					FileSystemInfo[] sFileSystemInfo = di.GetFileSystemInfos ();
					for (int i = 0; i < sFileSystemInfo.Length; i++) {
						FileSystemInfo fileSystemInfo = sFileSystemInfo [i];
						pathList.Add (path + "/" + fileSystemInfo.Name);
					}
					retValue = pathList.ToArray ();
				}
				catch (IOException e) {
					ROLogger.LogErrorFormat ("GetChildrenPath failed: {0}", e);
				}
			}
		}
		return retValue;
	}

	public static string[] GetChildrenName(string path)
	{
		if (string.IsNullOrEmpty(path)) return null;
		string absolutePath = GetAbsolutePath(path);
		if (!FileDirectoryHandler.ExistDirectory(absolutePath)) return null;
		DirectoryInfo di = new DirectoryInfo(absolutePath);
		List<string> nameList = new List<string>();
		FileSystemInfo[] sFileSystemInfo = di.GetFileSystemInfos();
		for (int i = 0; i < sFileSystemInfo.Length; i++)
		{
			FileSystemInfo fileSystemInfo = sFileSystemInfo[i];
			nameList.Add(fileSystemInfo.Name);
		}
		return nameList.ToArray();
	}

	public static string GetParentDirectoryPath(string path)
	{
		if (string.IsNullOrEmpty(path)) return null;
		string[] pathSplitBySlash = path.Split('/');
		if (pathSplitBySlash.Length > 1)
		{
			string parentDirectoryPath = "";
			int count = pathSplitBySlash.Length - 1;
			for (int i = 0; i < count; i++)
			{
				string fileSystemNode = pathSplitBySlash[i];
				if (i == count - 1)
				{
					parentDirectoryPath += fileSystemNode;
				}
				else
				{
					parentDirectoryPath += fileSystemNode + "/";
				}
			}
			return parentDirectoryPath;
		}
		return null;
	}

	public static string GetDirectoryNameFromPath(string path)
	{
		if (string.IsNullOrEmpty(path)) return null;
		string[] pathSplitBySlash = path.Split('/');
		return pathSplitBySlash[pathSplitBySlash.Length - 1];
	}

	public static string GetFilePathNoExtension(string path)
	{
		string retValue = "";
		if (!string.IsNullOrEmpty(path))
		{
			string[] pathSplitBySlash = path.Split('/');
			string fileNameWithExtension = pathSplitBySlash[pathSplitBySlash.Length - 1];
			string[] fileNameWithExtensionSplitByDot = fileNameWithExtension.Split('.');
			string extension = fileNameWithExtensionSplitByDot[fileNameWithExtensionSplitByDot.Length - 1];
			retValue = path.Replace("." + extension, "");
		}
		return retValue;
	}

	public static bool AppendBytesToFile(string path, byte[] bytes)
	{
		bool retValue = false;
		if (!string.IsNullOrEmpty(path))
		{
			if (bytes != null && bytes.Length > 0)
			{
				ExistFile(path);
				string absolutePath = GetAbsolutePath(path);
				FileStream stream = null;
				BinaryWriter bw = null;
				try
				{
					stream = File.Open(absolutePath, FileMode.Append, FileAccess.Write, FileShare.None);
					bw = new BinaryWriter(stream);
					bw.Write(bytes.Length);
					bw.Write(bytes);
					retValue = true;
				}
				catch (Exception e)
				{
					ROLogger.LogErrorFormat("AppendBytesToFile failed: {0}", e);
					retValue = false;
				}
				finally
				{
					if (null != stream)
					{
						stream.Close();
						stream = null;
					}
					if (null != bw)
					{
						bw.Close();
						bw = null;
					}
				}
			}
		}
		return retValue;
	}

	private const string ENCRYPT_KEY = @"myKey123";
	public static void EncryptFile(string source_file_path)
	{
		string sourceFileAbsolutePath = GetAbsolutePath(source_file_path);
		string targetFileAbsolutePath = GetAbsolutePath(GetFilePathNoExtension(source_file_path));
		DoEncryptFile(sourceFileAbsolutePath, targetFileAbsolutePath, ENCRYPT_KEY);
	}

	public static void DecryptFile(string source_file_path)
	{
		string sourceFilePathNoExtension = GetFilePathNoExtension(source_file_path);
		string sourceFileAbsolutePath = GetAbsolutePath(sourceFilePathNoExtension);
		string targetFileAbsolutePath = GetAbsolutePath(source_file_path);
		DoDecryptFile(sourceFileAbsolutePath, targetFileAbsolutePath, ENCRYPT_KEY);
	}

	public static void DoEncryptFile(string source_file_path, string target_file_path, string key)
	{
		FileStream fsCrypt = null;
		CryptoStream cs = null;
		FileStream fsIn = null;
		try
		{
			string password = ENCRYPT_KEY;
			UnicodeEncoding UE = new UnicodeEncoding();
			byte[] bytesKey = UE.GetBytes(password);
			string cryptFile = target_file_path;
			fsCrypt = new FileStream(cryptFile, FileMode.Create, FileAccess.Write, FileShare.None);
			RijndaelManaged RMCrypto = new RijndaelManaged();
			cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(bytesKey, bytesKey), CryptoStreamMode.Write);
			fsIn = new FileStream(source_file_path, FileMode.Open, FileAccess.Read, FileShare.Read);
			int data;
			while ((data = fsIn.ReadByte()) != -1) // TODO why read byte??
				cs.WriteByte((byte)data);
		}
		catch
		{
			RO.LoggerUnused.LogError("Encryption failed!");
		}
		finally
		{
			if (null != fsCrypt)
			{
				fsCrypt.Close();
				fsCrypt = null;
			}
			if (null != cs)
			{
				cs.Close();
				cs = null;
			}
			if (null != fsIn)
			{
				fsIn.Close();
				fsIn = null;
			}
		}
	}
	
	public static void DoDecryptFile(string source_file_path, string target_file_path, string key)
	{
		FileStream fsCrypt = null;
		CryptoStream cs = null;
		FileStream fsOut = null;
		try
		{
			string password = ENCRYPT_KEY;
			UnicodeEncoding UE = new UnicodeEncoding();
			byte[] bytesKey = UE.GetBytes(password);
			fsCrypt = new FileStream(source_file_path, FileMode.Open, FileAccess.Read, FileShare.Read);
			RijndaelManaged RMCrypto = new RijndaelManaged();
			cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(bytesKey, bytesKey), CryptoStreamMode.Read);
			fsOut = new FileStream(target_file_path, FileMode.Create, FileAccess.Write, FileShare.None);
			int data;
			while ((data = cs.ReadByte()) != -1) // TODO why read byte??
				fsOut.WriteByte((byte)data);
		}
		catch
		{
			RO.LoggerUnused.LogError("Decryption failed!");
		}
		finally
		{
			if (null != fsCrypt)
			{
				fsCrypt.Close();
				fsCrypt = null;
			}
			if (null != cs)
			{
				cs.Close();
				cs = null;
			}
			if (null != fsOut)
			{
				fsOut.Close();
				fsOut = null;
			}
		}
	}

	public static string GenerateEncryptKey()
	{
		DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
		return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
	}

	public static string GetFileNameFromPath(string path)
	{
		string retValue = "";
		if (!string.IsNullOrEmpty(path))
		{
			string[] pathSplitBySlash = path.Split('/');
			retValue = pathSplitBySlash[pathSplitBySlash.Length - 1];
		}
		return retValue;
	}

	public static string GetFileExtensionFromName(string file_name)
	{
		string retValue = "";
		if (!string.IsNullOrEmpty(file_name))
		{
			string[] fileNameSplitBySlash = file_name.Split('.');
			retValue = fileNameSplitBySlash[fileNameSplitBySlash.Length - 1];
		}
		return retValue;
	}
}
