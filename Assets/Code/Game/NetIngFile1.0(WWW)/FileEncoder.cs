using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using RO;

public static class FileEncoder
{
	public static byte[] FileToBytes(string file_path)
	{
		bool fileIsExist = File.Exists(file_path);
		if (!fileIsExist)
		{
			RO.LoggerUnused.LogWarning(file_path + " no exist.");
			return null;
		}
		byte[] bytes = File.ReadAllBytes(file_path);
		return bytes;
	}
}
