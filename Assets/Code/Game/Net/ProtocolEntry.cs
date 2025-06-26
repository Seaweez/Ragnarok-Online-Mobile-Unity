using UnityEngine;
using System.Collections;
using RO.Net;
using System.Collections.Generic;
using System;

namespace RO.Net
{
	public class ProtocolEntry
	{
		public class GameReceive
		{
			public int id1;
			public int id2;
			public byte[] data;
		
			public GameReceive (int id1, int id2, byte[] data)
			{
				this.id1 = id1;
				this.id2 = id2;
				this.data = data;
			}
		}
	
		public class ProtocolInfo
		{
			public int flags = 0;
			public int bodyLength = 0;
			public byte[] body = null;
			public int byteLength = 0;
			public bool invalid = false;
			public int encryptLen = 0;
			public bool needEncrypt = false;
			public bool needCompress = false;
			public void Reset (int newFlags, int newBodyLength)
			{
				flags = newFlags;
				bodyLength = newBodyLength;
				needEncrypt = (newFlags & 0x02) == 2;
				needCompress = (newFlags & 0x01) == 1;
				if(needEncrypt)
				{
					if(newBodyLength%8 == 0)
					{
						encryptLen = newBodyLength;
					}
					else
					{
						encryptLen = 8 - newBodyLength%8 + newBodyLength;
					}
					if (null == body || body.Length < encryptLen) {
						body = new byte[encryptLen];
					}
				}
				else
				{
					if (null == body || body.Length < newBodyLength) {
						body = new byte[newBodyLength];
					}
					encryptLen = 0;
				}
				byteLength = 0;
				invalid = false;
			}
		
			public void DecompressBody ()
			{
				if (needCompress && byteLength >= bodyLength) {
//					body = NetZlib.Decompress (body, 0, bodyLength);
					var outBuffer = new byte[1];
					var decompressedLen = lzip.decompressBuffer(body, bodyLength, ref outBuffer, 0);
					if (0 < decompressedLen)
					{
						body = outBuffer;
						bodyLength = body.Length;
						byteLength = bodyLength;
						encryptLen = bodyLength;
					}
					else
					{
						invalid = true;
                        ROLogger.LogFormat("<color=red>decompress failed:</color> bodyLength={0}", bodyLength);

                        //BuglyAgent.PrintLog(LogSeverity.LogError, "decompress failed: bodyLength={0}", bodyLength);
                    }
                }
			}

			public void Decrypt ()
			{
				if (byteLength >= encryptLen) {
					var decryptData = NetUtil.Decrypt(body,encryptLen);
					if(null == decryptData)
					{
						invalid = true;
						//BuglyAgent.PrintLog(LogSeverity.LogError, "Decrypt failed: bodyLength={0}", bodyLength);
						int id1 = Convert.ToInt32 (Convert.ToChar (body [0]));
						int id2 = Convert.ToInt32 (Convert.ToChar (body [1]));
                        ROLogger.LogFormat("<color=red>Decrypt failed:</color> bodyLength={0} id1={1},id2={2}", bodyLength, id1, id2);
                    }
                    else
					{
						body = decryptData;					
					}
				}
			}
		
			private static Queue<ProtocolInfo> reuseableInfos = new Queue<ProtocolInfo> ();
		
			public static ProtocolInfo Create (int newFlags, int newBodyLength)
			{
				var info = (0 < reuseableInfos.Count) ? reuseableInfos.Dequeue () : new ProtocolInfo ();
				info.Reset (newFlags, newBodyLength);
				return info;
			}
		
			public static void Destroy (ProtocolInfo info)
			{
				if (!reuseableInfos.Contains (info)) {
					reuseableInfos.Enqueue (info);
				}
			}

			public List<ProtocolInfo> ProcessSubProtoc ()
			{
				var subPro = new List<ProtocolInfo>();
				parseSubProtoc(subPro,this.body,4);
				return subPro;
			}

			public void parseSubProtoc(List<ProtocolInfo> list,byte[] data,int offset)
			{
				if(offset<data.Length && data.Length - offset > 2)
				{
					var bodyLength = Convert.ToInt32 (NetUtil.BytesToUInt2 (data, offset));
					if(bodyLength <= data.Length - offset - 2)
					{	
						var info = ProtocolInfo.Create (0, bodyLength);
						info.body = new byte[bodyLength];
						Array.Copy(data,offset+2,info.body,0,bodyLength);
						list.Add(info);
						parseSubProtoc(list,data,offset+bodyLength +2);
					}
				}
			}
		}
	
		public class ProtocolBytes
		{

			private int receiveCount = 0;
			private const int headByteLength = 3;
			private byte[] bytes = new byte[headByteLength];
			private int byteLength = 0;
			private ProtocolInfo info = null;
		
			private bool headValid {
				get {
					return byteLength >= headByteLength;
				}
			}

			public void clearData()
			{
				if(null != info)
				{
					ProtocolEntry.ProtocolInfo.Destroy (info);
					info = null;
				}
				byteLength = 0;
				receiveCount = 0;
			}
		
			private bool bodyValid {
				get {
					if(info.needEncrypt)
					{
						return null != info && info.byteLength >= info.encryptLen;
					}
					else
					{
						return null != info && info.byteLength >= info.bodyLength;
					}

				}
			}
		
			private ProtocolInfo GetProtocolInfo ()
			{
				if (bodyValid) {
					var oldInfo = info;
					byteLength = 0;
					info = null;
					return oldInfo;
				}
				return null;
			}
		
			private int ParseHead (byte[] rawBytes, int validLength, int offset)
			{
				if (offset < validLength && !headValid) {
					var readLength = Math.Min (headByteLength - byteLength, validLength - offset);
					Array.Copy (rawBytes, offset, bytes, byteLength, readLength);
					byteLength += readLength;
				
					if (headValid) {
						var flags = Convert.ToInt32 (Convert.ToChar (bytes [0]));
						var bodyLength = Convert.ToInt32 (NetUtil.BytesToUInt2 (bytes, 1));
						info = ProtocolInfo.Create (flags, bodyLength);
					}
					return readLength;
				}
				return 0;
			}
		
			private int ParseBody (byte[] rawBytes, int validLength, int offset)
			{
				if (offset < validLength && null != info) {
					var readLength = 0;
					if(info.needEncrypt)
					{
						readLength = Math.Min (info.encryptLen - info.byteLength, validLength - offset);
						Array.Copy (rawBytes, offset, info.body, info.byteLength, readLength);
						info.byteLength += readLength;
					}
					else
					{
						readLength = Math.Min (info.bodyLength - info.byteLength, validLength - offset);
						Array.Copy (rawBytes, offset, info.body, info.byteLength, readLength);
						info.byteLength += readLength;
					}

				
					if (bodyValid) {
						if(info.needEncrypt)
						{							
							info.Decrypt();
						}
						info.DecompressBody ();
					}
					return readLength;
				}
				return 0;
			}
		
			private void DoParse (List<ProtocolInfo> protocolList, byte[] rawBytes, int validLength, int offset)
			{
				if (offset >= validLength) {
					return;
				}
				if (!headValid) {
					offset += ParseHead (rawBytes, validLength, offset);
				
					if (!headValid) {
						return;
					}
				
					if (offset >= validLength) {
						return;
					}
				}
			
				if (!bodyValid) {
					offset += ParseBody (rawBytes, validLength, offset);
				
					if (!bodyValid) {
						return;
					}
				}
			
				protocolList.Add (GetProtocolInfo ());
				if (offset < validLength) {
					DoParse (protocolList, rawBytes, validLength, offset);
				}
			}
		
			public List<ProtocolInfo> Parse (byte[] rawBytes, int validLength, int offset)
			{
				receiveCount = receiveCount+1;
//				NetUtil.WriteFile(String.Format("ReciveBefore_{0}.txt",receiveCount),rawBytes,validLength);
				List<ProtocolInfo> protocolList = new List<ProtocolInfo> ();
				DoParse (protocolList, rawBytes, validLength, offset);
				return protocolList;
			}
		}
	}
}
