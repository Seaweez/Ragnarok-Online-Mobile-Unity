using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using zlib;

namespace RO.Net
{
	public class NetZlib
	{
		/// <summary>
		/// 复制流
		/// </summary>
		/// <param name="input">原始流</param>
		/// <param name="output">目标流</param>
		private static void CopyStream(System.IO.Stream input, System.IO.Stream output)
		{
			byte[] buffer = new byte[1024];
			int len;
			while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
			{
				output.Write(buffer, 0, len);
			}
			output.Flush();
		}

		/// <summary>
		/// 解压缩流
		/// </summary>
		/// <param name="sourceStream">需要被解压缩的流</param>
		/// <returns>解压后的流</returns>
		private static Stream DecompressStream(Stream sourceStream)
		{
			MemoryStream outStream = new MemoryStream();
			ZOutputStream outZStream = new ZOutputStream(outStream);
			CopyStream(sourceStream, outZStream);
			outZStream.finish();
			return outStream;
		}

		private static Stream CompressStream(Stream sourceStream)
		{
			MemoryStream outStream = new MemoryStream();
			ZOutputStream outZStream = new ZOutputStream(outStream,zlibConst.Z_DEFAULT_COMPRESSION);
			CopyStream(sourceStream, outZStream);
			outZStream.finish();
			return outStream;
		}

		/// <summary>
		/// 压缩流
		/// </summary>
		/// <param name="sourceStream">需要被压缩的流</param>
		public static  byte[] Compress(byte[] bytes, int offset = 0, int length = -1)
		{
			MemoryStream inputStream = new MemoryStream(bytes, offset, (0 < length) ? length : bytes.Length-offset);
			Stream outputStream = CompressStream(inputStream);
			byte[] outputBytes = new byte[outputStream.Length];
			outputStream.Position = 0;
			outputStream.Read(outputBytes, 0, outputBytes.Length);
			outputStream.Close();
			inputStream.Close();
			return outputBytes;
		}


		/// <summary>
		/// 解压缩字节数组
		/// </summary>
		/// <param name="sourceByte">需要被解压缩的字节数组</param>
		/// <returns>解压后的字节数组</returns>
		public static byte[] Decompress(byte[] bytes, int offset = 0, int length = -1)
		{
			MemoryStream inputStream = new MemoryStream(bytes, offset, (0 < length) ? length : bytes.Length-offset);
			Stream outputStream = DecompressStream(inputStream);
			byte[] outputBytes = new byte[outputStream.Length];
			outputStream.Position = 0;
			outputStream.Read(outputBytes, 0, outputBytes.Length);
			outputStream.Close();
			inputStream.Close();
			return outputBytes;
		}
	}
}

