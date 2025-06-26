using UnityEngine;
using System.Collections;
using CloudFile;

public class CloudFileManagerHelper : Singleton<CloudFileManagerHelper> {
	#region for what test case
	public CloudFileManagerHelper()
	{
		CloudFileManager.Ins.Open ();
	}

	public int DownloadAsNew(string url, string path, bool md5_check, string[] custom_headers = null)
	{
		int taskRecordID = CloudFileManager.Ins._TaskRecordCenter.ExistDownloadRecord (path);
		if (taskRecordID > 0) {
			CloudFileManager.Ins.StopTask (taskRecordID);
		}
		return CloudFileManager.Ins.Download (url, path, md5_check, custom_headers);
	}

	public int DownloadAsNew(
		string url,
		string path,
		bool md5_check,
		CloudFileCallback.ProgressCallback progress_callback,
		CloudFileCallback.SuccessCallback success_callback,
		CloudFileCallback.ErrorCallback error_callback,
		string[] custom_headers = null)
	{
		int recordID = DownloadAsNew(url, path, md5_check, custom_headers);
		if (recordID > 0)
		{
			if (progress_callback != null || success_callback != null || error_callback != null)
			{
				CloudFileManager.Ins._CloudFileCallbacks.RegisterCallback(recordID, progress_callback, success_callback, error_callback);
			}
			return recordID;
		}
		return 0;
	}

	public int DownloadAsOld(string url, string path, bool md5_check, string[] custom_headers = null)
	{
		int taskRecordID = CloudFileManager.Ins._TaskRecordCenter.ExistDownloadRecord (path);
		if (taskRecordID > 0) {
			TaskRecord taskRecord = CloudFileManager.Ins._TaskRecordCenter.GetTaskRecord (taskRecordID);
			if (taskRecord.State != E_TaskState.Progress) {
				taskRecord.URL = url;
				taskRecord.SetCustomHeaders (custom_headers);
				CloudFileManager.Ins.RestartTask (taskRecordID);
			}
			return taskRecordID;
		} else {
			return CloudFileManager.Ins.Download (url, path, md5_check, custom_headers);
		}
	}

	public int DownloadAsOld(
		string url,
		string path,
		bool md5_check,
		CloudFileCallback.ProgressCallback progress_callback,
		CloudFileCallback.SuccessCallback success_callback,
		CloudFileCallback.ErrorCallback error_callback,
		string[] custom_headers = null)
	{
		int recordID = DownloadAsOld(url, path, md5_check, custom_headers);
		if (recordID > 0)
		{
			if (progress_callback != null || success_callback != null || error_callback != null)
			{
				CloudFileManager.Ins._CloudFileCallbacks.RegisterCallback(recordID, progress_callback, success_callback, error_callback);
			}
			return recordID;
		}
		return 0;
	}

	public int NormalUploadAsNew(string path, string url, string[] custom_headers = null)
	{
		int taskRecordID = CloudFileManager.Ins._TaskRecordCenter.ExistUploadRecord (path);
		if (taskRecordID > 0) {
			CloudFileManager.Ins.StopTask (taskRecordID);
		}
		return CloudFileManager.Ins.NormalUpload (path, url, custom_headers);
	}

	public int NormalUploadAsNew(
		string path,
		string url,
		CloudFileCallback.ProgressCallback progress_callback,
		CloudFileCallback.SuccessCallback success_callback,
		CloudFileCallback.ErrorCallback error_callback,
		string[] custom_headers = null)
	{
		int recordID = NormalUploadAsNew(path, url, custom_headers);
		if (recordID > 0)
		{
			if (progress_callback != null || success_callback != null || error_callback != null)
			{
				CloudFileManager.Ins._CloudFileCallbacks.RegisterCallback(recordID, progress_callback, success_callback, error_callback);
			}
			return recordID;
		}
		return 0;
	}

	public int NormalUploadAsOld(string path, string url, string[] custom_headers = null)
	{
		int taskRecordID = CloudFileManager.Ins._TaskRecordCenter.ExistUploadRecord (path);
		if (taskRecordID > 0) {
			TaskRecord taskRecord = CloudFileManager.Ins._TaskRecordCenter.GetTaskRecord (taskRecordID);
			if (taskRecord.State != E_TaskState.Progress) {
				taskRecord.URL = url;
				taskRecord.SetCustomHeaders (custom_headers);
				CloudFileManager.Ins.RestartTask (taskRecordID);
			}
			return taskRecordID;
		} else {
			return CloudFileManager.Ins.NormalUpload (path, url, custom_headers);
		}
	}

	public int NormalUploadAsOld(
		string path,
		string url,
		CloudFileCallback.ProgressCallback progress_callback,
		CloudFileCallback.SuccessCallback success_callback,
		CloudFileCallback.ErrorCallback error_callback,
		string[] custom_headers = null)
	{
		int recordID = NormalUploadAsOld(path, url, custom_headers);
		if (recordID > 0)
		{
			CloudFileManager.Ins._CloudFileCallbacks.UnregisterCallback (recordID);
			if (progress_callback != null || success_callback != null || error_callback != null)
			{
				CloudFileManager.Ins._CloudFileCallbacks.RegisterCallback(recordID, progress_callback, success_callback, error_callback);
			}
			return recordID;
		}
		return 0;
	}
	#endregion for what test case
}
