using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace RO
{
	public static class AESSecurity
	{
		public static bool DebugLog = false;
		#region 秘钥对
		
		private static byte[] saltBytes = new byte[]{
			88,68,75,111,110,103,109,105,110,103,115,117,122,104,105,116,97,105,99,104,97
		};
		private static byte[] pWDBytes = new byte[]{
			88,68,107,109,115,117,122,104,105,122,117,105,99,104,97
		};
		
		#endregion
		
		#region aes管理器
		
		private static AesManaged _aesManaged;
		
		public static AesManaged aesManaged {
			get {
				if (_aesManaged == null) {
					string pWDString = System.Text.Encoding.UTF8.GetString (EncryptKeyBytes (pWDBytes));
					//提供高级加密标准 (AES) 对称算法的托管实现。
					_aesManaged = new AesManaged ();
					//通过使用基于 System.Security.Cryptography.HMACSHA1 的伪随机数生成器，实现基于密码的密钥派生功能 (PBKDF2)。
					Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes (pWDString, EncryptKeyBytes (saltBytes));
					/*
				            * AesManaged.BlockSize - 加密操作的块大小（单位：bit）
				            * AesManaged.LegalBlockSizes - 对称算法支持的块大小（单位：bit）
				            * AesManaged.KeySize - 对称算法的密钥大小（单位：bit）
				            * AesManaged.LegalKeySizes - 对称算法支持的密钥大小（单位：bit）
				            * AesManaged.Key - 对称算法的密钥
				            * AesManaged.IV - 对称算法的密钥大小
				            * Rfc2898DeriveBytes.GetBytes(int 需要生成的伪随机密钥字节数) - 生成密钥
				            */
					// 获取或设置加密操作的块大小（以位为单位）。
					_aesManaged.BlockSize = _aesManaged.LegalBlockSizes [0].MaxSize;
					//获取或设置用于对称算法的密钥大小（以位为单位）。
					_aesManaged.KeySize = _aesManaged.LegalKeySizes [0].MaxSize;
					//获取或设置用于对称算法的密钥。
					_aesManaged.Key = rfc.GetBytes (_aesManaged.KeySize / 8);
					//获取或设置用于对称算法的初始化向量 (IV)。
					_aesManaged.IV = rfc.GetBytes (_aesManaged.BlockSize / 8);
				}
				return _aesManaged;
			}
		}
		
		static byte[] EncryptKeyBytes (byte[] datas)
		{
			byte[] res = new byte[datas.Length];
			Buffer.BlockCopy (datas, 0, res, 0, datas.Length);
			for (int i=0; i<res.Length; i++) {
				res [i] = (byte)(res [i] << 1);
			}
			return res;
		}
		
		#endregion
		
		
		#region 加/解密算法
		
		/// <summary>
		/// 解密
		/// </summary>
		/// <param name="sSource">需要解密的内容</param>
		/// <returns></returns>
		public static byte[] DecryptString (string strSource, bool header = true)
		{
			byte[] encryptBytes = System.Text.Encoding.UTF8.GetBytes (strSource);
			return DecryptBytes (encryptBytes, header);
		}
		
		public static byte[] DecryptBytes (byte[] encryptBytes, bool header = true)
		{
			if (header && FileHeaderUtil.CheckHeaderIsEncrypt (encryptBytes) == false) {
				Debug.Log ("未被加密");
				return null;
			}
			// 用当前的 Key 属性和初始化向量 IV 创建对称解密器对象
			ICryptoTransform decryptTransform = aesManaged.CreateDecryptor ();
			//			// 解密后的输出流
			//			MemoryStream decryptStream = new MemoryStream ();
			//			
			//			// 将解密后的目标流（decryptStream）与解密转换（decryptTransform）相连接
			//			CryptoStream decryptor = new CryptoStream (
			//				decryptStream, decryptTransform, CryptoStreamMode.Write);
			//			encryptBytes = header ? FileHeaderUtil.RemoveEncryptHeader (encryptBytes) : encryptBytes;
			//			// 将一个字节序列写入当前 CryptoStream （完成解密的过程）
			//			decryptor.Write (encryptBytes, 0, encryptBytes.Length);
			//			decryptor.Close ();
			//			byte[] res = decryptStream.ToArray ();
			//			decryptStream.Dispose ();
			//			// 将解密后所得到的流转换为字符串
			//			return res;
			// 开辟一块内存流，存储密文  
			byte[] res = null;
			//			Profiler.BeginSample ("Memory");
			using (MemoryStream Memory = new MemoryStream (encryptBytes)) {
				
				if (header) {
					FileHeaderUtil.RemoveEncryptHeaderStream<MemoryStream> (Memory);
				}
				// 把内存流对象包装成加密流对象  
				res = decryptTransform.TransformFinalBlock (encryptBytes, Convert.ToInt32 (Memory.Position), encryptBytes.Length - Convert.ToInt32 (Memory.Position));
				//				using (CryptoStream Decryptor = new CryptoStream (Memory,  
				//			                                                 decryptTransform,  
				//			                                          CryptoStreamMode.Read)) {
				//					// 明文存储区  
				//					Profiler.BeginSample ("originalMemory");
				//					using (MemoryStream originalMemory = new MemoryStream()) {  
				//						Byte[] Buffer = new Byte[1024*1024];  
				//						Int32 readBytes = 0;  
				//						Profiler.BeginSample ("readBytes");
				//						while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0) {  
				//							originalMemory.Write (Buffer, 0, readBytes);  
				//						}  
				//						Profiler.EndSample ();
				//						res = originalMemory.ToArray ();  
				//					}  
				//					Profiler.EndSample ();
				//				}
				//				br.Close ();
			}
			//			Profiler.EndSample ();
			return res;
		}

		public static byte[] DecryptFile (string path)
		{
			byte[] res = null;
			using (FileStream fs = new FileStream(path,FileMode.Open)) {
				BinaryReader br = FileHeaderUtil.RemoveEncryptHeaderStream<FileStream> (fs);
				// 用当前的 Key 属性和初始化向量 IV 创建对称解密器对象
				ICryptoTransform decryptTransform = aesManaged.CreateDecryptor ();
				using (CryptoStream Decryptor = new CryptoStream (fs,  
							                                                 decryptTransform,  
							                                          CryptoStreamMode.Read)) {
					Byte[] buffer = new Byte[1024 * 1024];  
					Int32 readBytes = 0;  
					using(MemoryStream ms = new MemoryStream(Convert.ToInt32(fs.Length) + aesManaged.BlockSize))
					{
						while ((readBytes = Decryptor.Read(buffer, 0, buffer.Length)) > 0) {  
							ms.Write (buffer, 0, readBytes);  
						}
						res = ms.ToArray();
					}
				}
				br.Close ();
			}
			return res;
		}
		
		/// <summary>
		/// 加密
		/// </summary>
		/// <param name="sSource">需要加密的内容</param>
		/// <returns></returns>
		public static byte[] EncryptString (string strSource, bool header = true)
		{
			byte[] data = UTF8Encoding.UTF8.GetBytes (strSource);
			return EncryptBytes (data, header);
		}
		
		public static byte[] EncryptBytes (byte[] data, bool header = true)
		{
			if (DebugLog) {
				Debug.Log (string.Format ("加密前，字节流长度{0}", data.Length));
			}
			if (header && FileHeaderUtil.CheckHeaderIsEncrypt (data)) {
				Debug.Log ("已经加密过了");
				return null;
			}
			// 用当前的 Key 属性和初始化向量 IV 创建对称加密器对象
			ICryptoTransform encryptTransform = aesManaged.CreateEncryptor ();
			
			// 加密后的输出流
			MemoryStream encryptStream = new MemoryStream ();
			// 将加密后的目标流（encryptStream）与加密转换（encryptTransform）相连接
			CryptoStream encryptor = new CryptoStream
				(encryptStream, encryptTransform, CryptoStreamMode.Write);
			
			// 将一个字节序列写入当前 CryptoStream （完成加密的过程）
			encryptor.Write (data, 0, data.Length);
			encryptor.Close ();
			byte[] res = header ? FileHeaderUtil.AddEncryptHeader (encryptStream) : encryptStream.ToArray ();
			if (DebugLog) {
				Debug.Log (string.Format ("加密后，字节流长度{0}", res.Length));
			}
			encryptStream.Dispose ();
			return res;
		}
		
		#endregion
		
		public static byte[] DecryptBytes (AssetEncryptMode mode, byte[] datas)
		{
			switch (mode) {
			case AssetEncryptMode.Encryption1:
				return DecryptBytes (datas);
			}
			return null;
		}


	}
} // namespace RO

