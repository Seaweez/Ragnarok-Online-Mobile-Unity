using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

[SLua.CustomLuaClassAttribute]
public class FileHelper
{
	public static bool ExistDirectory(string path)
	{
		return Directory.Exists(path);
	}

	public static void CreateDirectory(string path)
	{
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}

	public static void DeleteDirectory(string path)
	{
		Directory.Delete(path, true);
	}

	public static string[] GetChildrenPath(string path)
	{
		string[] childrenName = GetChildrenName(path);
		if (childrenName != null && childrenName.Length > 0)
		{
			string[] childrenPath = new string[childrenName.Length];
			for (int i = 0; i < childrenName.Length; i++)
			{
				childrenPath[i] = path + "/" + childrenName[i];
			}
			return childrenPath;
		}
		return null;
	}
	
	public static string[] GetChildrenName(string path)
	{
		DirectoryInfo di = new DirectoryInfo(path);
		if (di.Exists)
		{
			List<string> listNames = new List<string>();
			FileSystemInfo[] sFSI = di.GetFileSystemInfos();
			for (int i = 0; i < sFSI.Length; i++)
			{
				FileSystemInfo fsi = sFSI[i];
				listNames.Add(fsi.Name);
			}
			return listNames.ToArray();
		}
		return null;
	}

	public static Stream CreateFile(string path)
	{
		return File.Create(path);
	}

	public static bool ExistFile(string path)
	{
		return File.Exists(path);
	}

	public static void DeleteFile(string path)
	{
		File.Delete(path);
	}

	public static void WriteFile(string path, byte[] bytes)
	{
		if (bytes != null && bytes.Length > 0)
		{
			try
			{
				File.WriteAllBytes(path, bytes);
			}
			catch(IOException e)
			{
				throw e;
			}
		}
	}

	public static void AppendBytes(string path, byte[] bytes)
	{
		if (bytes != null && bytes.Length > 0)
		{
			Stream stream = null;
			try
			{
				if (ExistFile(path))
				{
					stream = File.Open(path, FileMode.Append, FileAccess.Write, FileShare.None);
				}
				else
				{
					stream = CreateFile(path);
				}
				stream.Write(bytes, 0, bytes.Length);
			}
			catch (IOException e)
			{
				throw e;
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
		}
	}

	public static void AppendBytes(string path, byte[] bytes, int count)
	{
		if (bytes != null)
		{
			if (bytes.Length > count)
			{
				byte[] newBytes = new byte[count];
				for (int i = 0; i < newBytes.Length; i++)
				{
					newBytes[i] = bytes[i];
				}
				AppendBytes(path, newBytes);
			}
			else
			{
				AppendBytes(path, bytes);
			}
		}
	}

	public static void WriteFileAsync(string path, byte[] bytes, Action on_complete)
	{
		if (bytes != null && bytes.Length > 0)
		{
			Stream stream = null;
			try
			{
				stream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None);
				stream.BeginWrite(bytes, 0, bytes.Length, (x) => {
					stream.EndWrite(x);
					if (on_complete != null)
					{
						on_complete();
					}
				}, null);
			}
			catch(IOException e)
			{
				throw e;
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
		}
	}

	public static byte[] LoadFile(string path)
	{
		if (ExistFile(path))
		{
			byte[] bytes = null;
			try
			{
				bytes = File.ReadAllBytes(path);
			}
			catch(IOException e)
			{
				throw e;
			}
			return bytes;
		}
		return null;
	}

	public static int LoadFile(string path, byte[] bytes)
	{
		int retValue = 0;
		if (ExistFile (path)) {
			if (bytes != null) {
				Stream stream = null;
				try
				{
					stream = GetFileStream (path);
					if (bytes.Length >= stream.Length) {
						int streamLength = (int)stream.Length;
						stream.Read (bytes, 0, streamLength);
						retValue = streamLength;
					}
				}
				catch (IOException e) {
					throw e;
				}
				finally {
					stream.Close ();
				}
			} else {
				try
				{
					bytes = LoadFile (path);
					retValue = bytes.Length;
				}
				catch (IOException e) {
					throw e;
				}
			}
		}
		return retValue;
	}

	public static void LoadFileAsync(string path, Action<byte[]> on_complete)
	{
		Stream stream = GetFileStream(path);
		if (stream != null && stream.CanRead)
		{
			byte[] bytes = new byte[stream.Length];
			try
			{
				stream.BeginRead(bytes, 0, bytes.Length, (x) => {
					stream.EndRead(x);
					stream.Close();
					if (on_complete != null)
					{
						on_complete(bytes);
					}
				}, null);
			}
			catch (IOException e)
			{
				stream.Close();
				throw e;
			}
		}
	}

	public static Stream GetFileStream(string path)
	{
		Stream stream = null;
		try
		{
			stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
		}
		catch (IOException e)
		{
			throw e;
		}
		return stream;
	}

	public static Stream GetFileStreamWithTruncateMode(string path)
	{
		Stream stream = null;
		if (ExistFile(path))
		{
			try
			{
				stream = File.Open(path, FileMode.Truncate, FileAccess.ReadWrite, FileShare.None);
			}
			catch (IOException e)
			{
				throw e;
			}
		}
		else
		{
			stream = CreateFile(path);
		}
		return stream;
	}

	public static byte[] LoadFileBlock(string path, int start_pos, int length)
	{
		if (ExistFile(path))
		{
			if (length > 0)
			{
				byte[] bytes = LoadFile(path);
				if (bytes != null && bytes.Length > 0)
				{
					byte[] bytesBlock = new byte[length];
					int startPos = Math.Max(0, start_pos);
					Array.Copy(bytes, startPos, bytesBlock, 0, length);
					return bytesBlock;
				}
			}
		}
		return null;
	}

	public static string GetFileNameFromPath(string path)
	{
		int index = path.LastIndexOf('/');
		if (index >= 0 && index < path.Length - 1)
		{
			string nameWithExtension = path.Substring(index + 1);
			int indexOfDot = nameWithExtension.LastIndexOf('.');
			if (indexOfDot > 0)
			{
				return nameWithExtension.Substring(0, indexOfDot);
			}
		}
		return null;
	}

	public static string GetFileExtensionFromPath(string path)
	{
		int index = path.LastIndexOf('/');
		if (index >= 0 && index < path.Length - 1)
		{
			string nameWithExtension = path.Substring(index + 1);
			int indexOfDot = nameWithExtension.LastIndexOf('.');
			if (indexOfDot >= 0 && indexOfDot < nameWithExtension.Length - 1)
			{
				return nameWithExtension.Substring(indexOfDot + 1);
			}
		}
		return null;
	}

	public static string GetFileExtensionFromName(string name)
	{
		int indexOfDot = name.LastIndexOf('.');
		if (indexOfDot >= 0 && indexOfDot < name.Length - 1)
		{
			return name.Substring(indexOfDot + 1);
		}
		return null;
	}

	public static string GetParentDirectoryPath(string path)
	{
		int index = path.LastIndexOf('/');
		if (index > 0)
		{
			return path.Substring(0, index);
		}
		return null;
	}

	public static string GetFileFormat(string path)
	{
		byte[] bytes = LoadFile(path);
		return GetFileFormat(bytes);
	}

	public static string GetFileFormat(byte[] bytes)
	{
		if (bytes != null && bytes.Length > 0)
		{
			int indicator = 0;
			bool isTxt = true;
			while (indicator < bytes.Length)
			{
				if (bytes[indicator] == 0)
				{
					isTxt = false;
				}
				indicator++;
			}
			if (isTxt)
			{
				return "txt";
			}
			else
			{
				byte firstByte = bytes[0];
				byte secondByte = bytes[1];
				if (firstByte == 255 && secondByte == 216)
				{
					return "jpg";
				}
				else if (firstByte == 137 && secondByte == 80)
				{
					return "png";
				}
			}
		}
		return null;
	}

	public static long GetFileSize(string path)
	{
		FileInfo fi = new FileInfo(path);
		return fi.Length;
	}

	/// <summary>
	/// Only NTFS.
	/// </summary>
	public static void EncryptFile(string path)
	{
		File.Encrypt(path);
	}

	/// <summary>
	/// Only NTFS.
	/// </summary>
	public static void DecryptFile(string path)
	{
		File.Decrypt(path);
	}

	public static void MoveFile(string source_path, string destination_path)
	{
		File.Move(source_path, destination_path);
	}
}
