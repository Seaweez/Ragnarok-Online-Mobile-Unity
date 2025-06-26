using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class FileIOHelper
	{
		public static void AppendBytes (string path, byte[] datas, bool writeLength)
		{
			FileStream fs = null;
			if (File.Exists (path))
				fs = new FileStream (path, FileMode.Append);
			else
				fs = new FileStream (path, FileMode.OpenOrCreate);
			if (writeLength) {
				BinaryWriter bw = new BinaryWriter (fs);
				bw.Write (datas.Length);
				bw.Write (datas);
				bw.Close ();
			} else
				fs.Write (datas, 0, datas.Length);
			fs.Close ();
		}
	
		public static byte[][] OpenRead (string path, bool readLength)
		{
			if (File.Exists (path)) {
				List<byte[]> caches = new List<byte[]>();
				FileStream fs = File.OpenRead (path);
				BinaryReader br = new BinaryReader (fs);
				int length = 0;
				byte[] datas = null;
				while (br.BaseStream.Position < br.BaseStream.Length) {
					length = br.ReadInt32 ();
					datas = br.ReadBytes(length);
					caches.Add(datas);
				}
				return caches.ToArray();
			}
			return null;
		}

		public static byte[][] ReadBytes (string path)
		{
			byte[][] bytes = null;
			FileStream fs = null;
			FileDirectoryHandler.LoadFile(path, (x) => {
				fs = x;
				if (fs != null)
				{
					List<byte[]> caches = new List<byte[]>();
					BinaryReader br = new BinaryReader (fs);
					int length = 0;
					byte[] datas = null;
					try
					{
						while (br.BaseStream.Position < br.BaseStream.Length) {
							length = br.ReadInt32 ();
							if (length < 0)
							{
								RO.LoggerUnused.Log("FileIOHelper ReadBytes : length < 0");
								break;
							}
							datas = br.ReadBytes(length);
							caches.Add(datas);
						}
					}
					catch(EndOfStreamException eof)
					{
						RO.LoggerUnused.Log("FileIOHelper ReadBytes " + eof.Message.ToString());
					}
					bytes = caches.ToArray();
				}
			});
			return bytes;
		}
	}
} // namespace RO
