using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudFile
{
	[SLua.CustomLuaClassAttribute]
	public class CloudFileManager : Singleton<CloudFileManager>
	{
		public static CloudFileManager Ins
		{
			get
			{
				return ins;
			}
		}

		private TaskRecordCenter m_taskRecordCenter;
		public TaskRecordCenter _TaskRecordCenter
		{
			get
			{
				return m_taskRecordCenter;
			}
		}
		private CloudFileLoaderFactory m_cloudFileLoaderFactory;

		private CloudFileCallbacks m_cloudFileCallbacks;
		public CloudFileCallbacks _CloudFileCallbacks
		{
			get
			{
				return m_cloudFileCallbacks;
			}
		}

		public void Open()
		{
			if (m_taskRecordCenter == null)
			{
				m_taskRecordCenter = new TaskRecordCenter();
				m_taskRecordCenter.Open();
			}
			if (m_cloudFileLoaderFactory == null)
			{
				m_cloudFileLoaderFactory = new CloudFileLoaderFactory();
				m_cloudFileLoaderFactory.Open();
			}
			m_cloudFileCallbacks = new CloudFileCallbacks();
		}

		/// <param name="url">must be upyun url</param>
		public int Download(string url, string path, bool md5_check, string[] custom_headers = null)
		{
			string parentdirectoryPath = FileHelper.GetParentDirectoryPath(path);
			if (!FileHelper.ExistDirectory(parentdirectoryPath))
			{
				FileHelper.CreateDirectory (parentdirectoryPath);
			}
			if (FileHelper.ExistFile (path)) {
				FileHelper.DeleteFile (path);
			}
			DownloadTaskRecord downloadTaskRecord = m_taskRecordCenter.CreateTaskRecordForDownload (url, path, ArrayToDictionary(custom_headers));
			if (downloadTaskRecord.IsValid ()) {
				CloudFileDownloader cloudFileDownloader = m_cloudFileLoaderFactory.CreateCloudFileDownloader (downloadTaskRecord);
				cloudFileDownloader.BMD5Check = md5_check;
				cloudFileDownloader.Start ();
				return downloadTaskRecord.ID;
			}
			m_taskRecordCenter.RemoveTaskRecord (downloadTaskRecord);
			return 0;
		}

		public int Download(
			string url,
			string path,
			bool md5_check,
			CloudFileCallback.ProgressCallback progress_callback,
			CloudFileCallback.SuccessCallback success_callback,
			CloudFileCallback.ErrorCallback error_callback,
			string[] custom_headers = null)
		{
			int recordID = Download(url, path, md5_check, custom_headers);
			if (recordID > 0)
			{
				if (progress_callback != null || success_callback != null || error_callback != null)
				{
					m_cloudFileCallbacks.RegisterCallback(recordID, progress_callback, success_callback, error_callback);
				}
				return recordID;
			}
			return 0;
		}

		/// <param name="url">must be upyun url</param>
		public int NormalUpload(string path, string url, string[] custom_headers = null)
		{
			if (FileHelper.ExistFile(path))
			{
				byte[] fileBytes = FileHelper.LoadFile(path);
				UploadTaskRecordForNormal uploadTaskRecordForNormal = m_taskRecordCenter.CreateTaskRecordForNormalUpload(path, url, ArrayToDictionary(custom_headers));
				if (uploadTaskRecordForNormal.IsValid())
				{
					CloudFileNormalUploader cloudFileNormalUploader = m_cloudFileLoaderFactory.CreateCloudFileUploaderForNormal(uploadTaskRecordForNormal, fileBytes);
					cloudFileNormalUploader.Start();
					return uploadTaskRecordForNormal.ID;
				}
				m_taskRecordCenter.RemoveTaskRecord(uploadTaskRecordForNormal);
			}
			return 0;
		}

		public int NormalUpload(
			string path,
			string url,
			CloudFileCallback.ProgressCallback progress_callback,
			CloudFileCallback.SuccessCallback success_callback,
			CloudFileCallback.ErrorCallback error_callback,
			string[] custom_headers = null)
		{
			int recordID = NormalUpload(path, url, custom_headers);
			if (recordID > 0)
			{
				if (progress_callback != null || success_callback != null || error_callback != null)
				{
					m_cloudFileCallbacks.RegisterCallback(recordID, progress_callback, success_callback, error_callback);
				}
				return recordID;
			}
			return 0;
		}

		/// <param name="url">must be upyun url</param>
		public int FormUpload(string path, string url, string policy, string authorization, string[] custom_headers = null)
		{
			if (FileHelper.ExistFile(path))
			{
				byte[] fileBytes = FileHelper.LoadFile(path);
				UploadTaskRecordForForm uploadTaskRecordForForm = m_taskRecordCenter.CreateTaskRecordForFormUpload(path, url, policy, authorization, ArrayToDictionary(custom_headers));
				if (uploadTaskRecordForForm.IsValid())
				{
					CloudFileFormUploader cloudFileFormUploader = m_cloudFileLoaderFactory.CreateCloudFileUploaderForForm(uploadTaskRecordForForm, fileBytes);
					cloudFileFormUploader.Start();
					return uploadTaskRecordForForm.ID;
				}
				m_taskRecordCenter.RemoveTaskRecord(uploadTaskRecordForForm);
			}
			return 0;
		}

		public int FormUpload(
			string path,
			string url,
			string policy,
			string authorization,
			CloudFileCallback.ProgressCallback progress_callback,
			CloudFileCallback.SuccessCallback success_callback,
			CloudFileCallback.ErrorCallback error_callback,
			string[] custom_headers = null)
		{
			int recordID = FormUpload(path, url, policy, authorization, custom_headers);
			if (recordID > 0)
			{
				if (progress_callback != null || success_callback != null || error_callback != null)
				{
					m_cloudFileCallbacks.RegisterCallback(recordID, progress_callback, success_callback, error_callback);
				}
				return recordID;
			}
			return 0;
		}

		public void StopTask(int task_record_id)
		{
			if (task_record_id > 0)
			{
				m_taskRecordCenter.SetTaskRecordState(task_record_id, E_TaskState.Interrupt);
				m_cloudFileLoaderFactory.StopLoader(task_record_id);
			}
		}

		public void RestartTask(int task_record_id)
		{
			if (task_record_id > 0)
			{
				TaskRecord taskRecord = m_taskRecordCenter.GetTaskRecord (task_record_id);
				if (taskRecord is DownloadTaskRecord) {
					DownloadTaskRecord downloadTaskRecord = taskRecord as DownloadTaskRecord;
					if (FileHelper.ExistFile (downloadTaskRecord.Path)) {
						FileHelper.DeleteFile (downloadTaskRecord.Path);
					}
				}
				m_taskRecordCenter.SetTaskRecordState(task_record_id, E_TaskState.Progress);
				m_cloudFileLoaderFactory.RestartLoader(task_record_id);
			}
		}

		public void Close()
		{
			if (m_taskRecordCenter != null)
			{
				m_taskRecordCenter.Close();
			}
			if (m_cloudFileLoaderFactory != null)
			{
				m_cloudFileLoaderFactory.Close();
			}
		}

		public static Dictionary<string, string> ArrayToDictionary(string[] arr)
		{
			if (arr != null && arr.Length > 0) {
				Dictionary<string, string> retValue = new Dictionary<string, string> ();
				int i = 0;
				while (i < arr.Length) {
					string value = null;
					if (i + 1 < arr.Length) {
						value = arr [i + 1];
					}
					retValue.Add (arr [i], value);
					i = i + 2;
				}
				return retValue;
			}
			return null;
		}
	}
}