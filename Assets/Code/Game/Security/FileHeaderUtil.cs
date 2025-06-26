using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

namespace RO
{
	public class FileHeaderUtil
	{
		const byte ENCRYPT_HEADER_FLAG = 2;
		const string ENCRYPT_HEADER = "ENCRYPT_HEADER";
		
		public static bool CheckHeaderIsEncrypt (byte[] datas)
		{
			if (datas != null) {
				return CheckHeaderIsEncrypt (new MemoryStream (datas));
			}
			return false;
		}
		
		public static bool CheckHeaderIsEncrypt (MemoryStream ms)
		{
			bool res = false;
			if (ms != null) {
				BinaryReader br = new BinaryReader (ms);
				byte flag = br.ReadByte ();
				if (flag == ENCRYPT_HEADER_FLAG) {
					string info = br.ReadString ();
					if (info == ENCRYPT_HEADER) {
						res = true;
					}
				}
				br.Close ();
			}
			return res;
		}
		
		public static byte[] AddEncryptHeader (byte[] datas)
		{
			if (datas != null) {
				return AddEncryptHeader (new MemoryStream (datas));
			}
			return null;
		}
		
		public static byte[] AddEncryptHeader (MemoryStream ms)
		{
			if (ms != null) {
				MemoryStream headms = new MemoryStream ();
				BinaryWriter bw = new BinaryWriter (headms);
				//				bw.Seek (0, SeekOrigin.Begin);
				bw.Write (ENCRYPT_HEADER_FLAG);
				bw.Write (ENCRYPT_HEADER);
				//				bw.Write(ms.GetBuffer());
				bw.Flush ();
				byte[] headerBytes = headms.ToArray ();
				byte[] bodyBytes = ms.ToArray ();
				byte[] res = new byte[headerBytes.Length + bodyBytes.Length];
				Buffer.BlockCopy (headerBytes, 0, res, 0, headerBytes.Length);
				Buffer.BlockCopy (bodyBytes, 0, res, headerBytes.Length, bodyBytes.Length);
				//				byte[] res = headms.ToArray ();
				headms.Close ();
				return res;
			}
			return null;
		}
		
		public static byte[] RemoveEncryptHeader (byte[] datas)
		{
			if (datas != null) {
				return RemoveEncryptHeader (new MemoryStream (datas));
			}
			return null;
		}
		
		public static byte[] RemoveEncryptHeader (MemoryStream ms)
		{
			byte[] res = null;
			if (ms != null) {
				BinaryReader br = new BinaryReader (ms);
				byte flag = br.ReadByte ();
				if (flag == ENCRYPT_HEADER_FLAG) {
					string info = br.ReadString ();
					if (info == ENCRYPT_HEADER) {
						res = br.ReadBytes (System.Convert.ToInt32 (ms.Length - ms.Position));
					}
				}
				//				br.Close();
			}
			return res;
		}
		
		public static BinaryReader RemoveEncryptHeaderStream<T> (T s) where T:Stream
		{
			if (s != null) {
				BinaryReader br = new BinaryReader (s);
				byte flag = br.ReadByte ();
				if (flag == ENCRYPT_HEADER_FLAG) {
					string info = br.ReadString ();
					if (info == ENCRYPT_HEADER) {
						return br;
					}
				}
				br.Close();
			}
			return null;
		}

//		public static
	}
} // namespace RO
