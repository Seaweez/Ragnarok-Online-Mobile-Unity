using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudFile
{
	public class CloudFileLoaderFactory
	{
		private List<CloudFileLoader> m_loaders;

		public void Open()
		{
			if (m_loaders == null)
			{
				m_loaders = new List<CloudFileLoader>();
			}
		}

		public CloudFileDownloader CreateCloudFileDownloader(DownloadTaskRecord record)
		{
			CloudFileDownloader downloader = new CloudFileDownloader(record);
			m_loaders.Add(downloader);
			return downloader;
		}

		public CloudFileNormalUploader CreateCloudFileUploaderForNormal(UploadTaskRecordForNormal record, byte[] file_bytes)
		{
			CloudFileNormalUploader uploader = new CloudFileNormalUploader(record, file_bytes);
			m_loaders.Add(uploader);
			return uploader;
		}

		public CloudFileFormUploader CreateCloudFileUploaderForForm(UploadTaskRecordForForm record, byte[] file_bytes)
		{
			CloudFileFormUploader uploader = new CloudFileFormUploader(record, file_bytes);
			m_loaders.Add(uploader);
			return uploader;
		}

		public void StopLoader(int task_record_id)
		{
			if (m_loaders != null && m_loaders.Count > 0)
			{
				CloudFileLoader loader = m_loaders.Find(x => x.TaskRecordID == task_record_id);
				if (loader != null)
				{
					loader.Stop();
				}
			}
		}

		public void RestartLoader(int task_record_id)
		{
			if (m_loaders != null && m_loaders.Count > 0)
			{
				CloudFileLoader loader = m_loaders.Find(x => x.TaskRecordID == task_record_id);
				if (loader != null)
				{
					loader.Restart();
				}
			}
		}

		public void Close()
		{
			if (m_loaders != null && m_loaders.Count > 0)
			{
				for (int i = 0; i < m_loaders.Count; ++i)
				{
					CloudFileLoader loader = m_loaders[i];
					loader.Close();
				}
				m_loaders.Clear();
			}
		}
	}
}