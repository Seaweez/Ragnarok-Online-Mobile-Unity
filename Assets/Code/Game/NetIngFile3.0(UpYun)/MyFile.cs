using UnityEngine;
using System.Collections;
using System.IO;
using UpYunLibrary;
using System;
using System.Collections.Generic;
using RO;

[SLua.CustomLuaClassAttribute]
public class MyFile : IDisposable
{
	private string m_path;
	public string Path
	{
		get
		{
			return m_path;
		}
	}
	public int Size
	{
		get
		{
			FileInfo fi = new FileInfo(m_path);
			return (int)fi.Length;
		}
	}
	private string m_fileMD5;
	public string FileMD5
	{
		get {return m_fileMD5;}
	}
	private const int UNIT_BLOCK_SIZE = 1024 * 1024;
	private int m_blockCount = -1;
	public int BlockCount
	{
		get
		{
			int size = Size;
			if (m_blockCount == -1)
			{
				if (size > UNIT_BLOCK_SIZE)
				{
					m_blockCount = size / UNIT_BLOCK_SIZE;
					int remainder = size % UNIT_BLOCK_SIZE;
					if (remainder > 0)
					{
						m_blockCount++;
					}
				}
				else
				{
					m_blockCount = 1;
				}
			}
			return m_blockCount;
		}
	}
	private FileStream m_fileStream;

	public MyFile(string path)
	{
		this.m_path = path;
		this.m_fileMD5 = MyMD5.HashFile(this.m_path);
	}

	~MyFile()
	{
		Dispose();
	}

	public byte[][] GetBlocks()
	{
		int blockCount;
		int[] blocksSize = null;
		int[] blocksStartPos = null;
		int fileSize = Size;
		if (fileSize > UNIT_BLOCK_SIZE)
		{
			int unitBlockSize = UNIT_BLOCK_SIZE;
			blockCount = fileSize / unitBlockSize;
			int remainder = fileSize % unitBlockSize;
			if (remainder > 0)
			{
				blockCount++;
				blocksSize = new int[blockCount];
				blocksStartPos = new int[blockCount];
				int startPos = 0;
				for (int i = 0; i < blocksSize.Length; i++)
				{
					blocksStartPos[i] = startPos;
					
					int size = (i == blocksSize.Length - 1) ? remainder : unitBlockSize;
					blocksSize[i] = size;
					startPos += size;
				}
			}
			else
			{
				blocksSize = new int[blockCount];
				blocksStartPos = new int[blockCount];
				int startPos = 0;
				for (int i = 0; i < blocksSize.Length; i++)
				{
					blocksStartPos[i] = startPos;
					blocksSize[i] = unitBlockSize;
					startPos += unitBlockSize;
				}
			}
		}
		else
		{
			blockCount = 1;
			blocksSize = new int[]{fileSize};
			blocksStartPos = new int[]{0};
		}
		if (blocksSize != null && blocksSize.Length > 0 && blocksStartPos != null && blocksStartPos.Length > 0)
		{
			List<byte[]> listBytes = new List<byte[]>();
			m_fileStream = File.OpenRead(m_path);
			for (int i = 0; i < blocksSize.Length; i++)
			{
				int blockSize = blocksSize[i];
				int blockStartPos = blocksStartPos[i];
				byte[] bytes = ReadBlock(blockStartPos, blockStartPos + blockSize - 1);
				listBytes.Add(bytes);
			}
			m_fileStream.Close();
			m_fileStream = null;
			return listBytes.ToArray();
		}
		return null;
	}
	
	public byte[] GetBlock(int index)
	{
		byte[][] blocks = GetBlocks();
		if (blocks != null && blocks.Length > 0)
		{
			return blocks[index];
		}
		return null;
	}

	private byte[] ReadBlock(int start_pos, int end_pos, int unit_length = 8192)
	{
		start_pos = Mathf.Max(start_pos, 0);
		end_pos = Mathf.Max(end_pos, 0);
		int length = end_pos - start_pos + 1;
		if (length <= 0) return null;
		byte[] bytes = new byte[length];
		try
		{
			int indicatorForBytes = 0;
			while (start_pos < end_pos)
			{
				if (start_pos + unit_length > end_pos)
				{
					unit_length = end_pos - start_pos + 1;
				}
				m_fileStream.Seek(start_pos, SeekOrigin.Begin);
				int readLength = m_fileStream.Read(bytes, indicatorForBytes, unit_length);
				start_pos += unit_length;
				indicatorForBytes += readLength;
			}
		}
		catch (Exception e)
		{
			RO.LoggerUnused.LogWarning(e);
		}
		return bytes;
	}

	public void BeginWrite()
	{

	}

	public void Write(byte[] bytes)
	{

	}

	public void EndWrite()
	{

	}

	private void CloseFileStream()
	{
		if (m_fileStream != null)
		{
			m_fileStream.Close();
			m_fileStream = null;
		}
	}

	public void Dispose()
	{
		CloseFileStream();
	}
}
