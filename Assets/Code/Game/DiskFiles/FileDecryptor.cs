using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;

public class FileDecryptor
{
	public static void DecryptFile(string path)
	{
		if (FileHelper.ExistFile(path))
		{
			Stream streamOfFile = FileHelper.GetFileStream(path);
			if (streamOfFile != null)
			{
				string tempExtension = "unknown";
				string tempPathOfGeneratedFile = path + '.' + tempExtension;
				Stream streamOfGeneratedFile = FileHelper.GetFileStreamWithTruncateMode(tempPathOfGeneratedFile);
				if (streamOfGeneratedFile != null)
				{
					UnicodeEncoding ue = new UnicodeEncoding();
					byte[] bytesKey = ue.GetBytes(FileEncryptor.Key);
					RijndaelManaged rmCrypto = new RijndaelManaged();
					CryptoStream streamCrypto = null;
					try
					{
						streamCrypto = new CryptoStream(streamOfFile, rmCrypto.CreateDecryptor(bytesKey, bytesKey), CryptoStreamMode.Read);
						int byteInStream;
						while ((byteInStream = streamCrypto.ReadByte()) != -1)
						{
							streamOfGeneratedFile.WriteByte((byte)byteInStream);
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
					string fileFormat = FileHelper.GetFileFormat(tempPathOfGeneratedFile);
					string pathOfGeneratedFile = tempPathOfGeneratedFile.Replace(tempExtension, fileFormat);
					if (FileHelper.ExistFile(pathOfGeneratedFile))
					{
						FileHelper.DeleteFile(pathOfGeneratedFile);
					}
					FileHelper.MoveFile(tempPathOfGeneratedFile, pathOfGeneratedFile);
				}
			}
		}
	}
}
