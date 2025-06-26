using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class MonoFileDirectoryHandler : MonoSingleton<MonoFileDirectoryHandler>
{
	public void WriteFile(string path, byte[] bytes, Action<bool, bool> on_complete)
	{
		if (!string.IsNullOrEmpty(path))
		{
			bool exist = FileDirectoryHandler.ExistFile(path);
			if (bytes != null && bytes.Length > 0)
			{
				StartCoroutine(IE_WriteFile(path, bytes, () => {
					if (on_complete != null)
					{
						on_complete(exist, true);
					}
				}, () => {
					if (on_complete != null)
					{
						on_complete(exist, false);
					}
				}));
			}
			else
			{
				if (on_complete != null)
				{
					on_complete(exist, false);
				}
			}
		}
		else
		{
			if (on_complete != null)
			{
				on_complete(false, false);
			}
		}
	}

	IEnumerator IE_WriteFile(string path, byte[] bytes, Action on_success, Action on_fail)
	{
		string absolutePath = FileDirectoryHandler.GetAbsolutePath(path);
		Stream stream = null;
		try
		{
			if (File.Exists(absolutePath))
			{
				stream = File.Open(absolutePath, FileMode.Truncate, FileAccess.Write, FileShare.None);
			}
			else
			{
				stream = File.Open(absolutePath, FileMode.Create, FileAccess.Write, FileShare.None);
			}
			stream.Write(bytes, 0, bytes.Length);
		}
		catch(Exception e)
		{
			if (on_fail != null)
			{
				on_fail();
			}
			RO.ROLogger.LogErrorFormat("IE_WriteFile failed, {0}", e);
		}
		finally
		{
			if (null != stream)
			{
				stream.Close();
				stream = null;
			}
			if (on_success != null)
			{
				on_success();
			}
		}
		yield return null;
	}
}
