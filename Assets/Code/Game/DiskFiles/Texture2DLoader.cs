using UnityEngine;
using System.Collections;
using System.IO;

[SLua.CustomLuaClassAttribute]
public class Texture2DLoader
{
	private static byte[] m_buffer = new byte[4 * 1024 * 1024];

	public static Texture2D Load(string path)
	{
		if (m_buffer == null) {
			m_buffer = new byte[4 * 1024 * 1024];
		}
		int fileLength = 0;
		try
		{
			fileLength = FileHelper.LoadFile (path, m_buffer);
		}
		catch (IOException e) {
			Debug.LogWarning (e);
		}
		if (fileLength > 0) {
			Texture2D texture2D = new Texture2D (0, 0, TextureFormat.RGB24, false);
			bool loadSuccess = texture2D.LoadImage (m_buffer);
			if (loadSuccess) {
				return texture2D;
			}
		}
		return null;
	}
	
	public static void ClearBuffer()
	{
		m_buffer = null;
	}

	public static void SetBufferSize(int size)
	{
		m_buffer = new byte[size];
	}
}
