using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudFile
{
	public class TaskRecordCenter
	{
		private static int m_recordIDGenerator;
		public static int NewRecordID
		{
			get
			{
				return ++m_recordIDGenerator;
			}
		}

		private List<TaskRecord> m_taskRecords;

		public void Open()
		{
			if (m_taskRecords == null)
			{
				m_taskRecords = new List<TaskRecord>();
			}
		}

		public DownloadTaskRecord CreateTaskRecordForDownload(string url, string path, Dictionary<string, string> custom_headers)
		{
			DownloadTaskRecord record = new DownloadTaskRecord(NewRecordID, path, url, custom_headers);
			m_taskRecords.Add(record);
			return record;
		}

		public UploadTaskRecordForNormal CreateTaskRecordForNormalUpload(string path, string url, Dictionary<string, string> custom_headers)
		{
			UploadTaskRecordForNormal record = new UploadTaskRecordForNormal(NewRecordID, path, url, custom_headers);
			m_taskRecords.Add(record);
			return record;
		}

		public UploadTaskRecordForForm CreateTaskRecordForFormUpload(string path, string url, string policy, string authorization, Dictionary<string, string> custom_headers)
		{
			UploadTaskRecordForForm record = new UploadTaskRecordForForm (NewRecordID, path, url, policy, authorization, custom_headers);
			m_taskRecords.Add (record);
			return record;
		}

		public TaskRecord GetTaskRecord(int record_id)
		{
			if (m_taskRecords != null && m_taskRecords.Count > 0)
			{
				return m_taskRecords.Find(x => x.ID == record_id);
			}
			return null;
		}

		public void RemoveTaskRecord(int record_id)
		{
			if (m_taskRecords != null && m_taskRecords.Count > 0)
			{
				TaskRecord taskRecord = m_taskRecords.Find(x => x.ID == record_id);
				m_taskRecords.Remove(taskRecord);
			}
		}

		public void RemoveTaskRecord(TaskRecord task_record)
		{
			if (m_taskRecords != null && m_taskRecords.Count > 0)
			{
				m_taskRecords.Remove(task_record);
			}
		}

		public void SetTaskRecordState(int task_record_id, E_TaskState state)
		{
			if (m_taskRecords != null && m_taskRecords.Count > 0)
			{
				TaskRecord taskRecord = m_taskRecords.Find(x => x.ID == task_record_id);
				if (taskRecord != null)
				{
					taskRecord.State = state;
				}
			}
		}

		#region for what test case
		public int ExistDownloadRecord(string path)
		{
			if (m_taskRecords != null && m_taskRecords.Count > 0)
			{
				foreach (TaskRecord taskRecord in m_taskRecords) {
					if (taskRecord is DownloadTaskRecord) {
						DownloadTaskRecord downloadTaskRecord = taskRecord as DownloadTaskRecord;
						if (downloadTaskRecord.Path == path) {
							return downloadTaskRecord.ID;
						}
					}
				}
			}
			return 0;
		}
		public int ExistUploadRecord(string path)
		{
			if (m_taskRecords != null && m_taskRecords.Count > 0)
			{
				foreach (TaskRecord taskRecord in m_taskRecords) {
					if (taskRecord is UploadTaskRecordForNormal) {
						UploadTaskRecordForNormal uploadTaskRecordForNormal = taskRecord as UploadTaskRecordForNormal;
						if (uploadTaskRecordForNormal.Path == path) {
							return uploadTaskRecordForNormal.ID;
						}
					}
				}
			}
			return 0;
		}
		#endregion


		public void Close()
		{
			if (m_taskRecords != null)
			{
				m_taskRecords.Clear();
			}
		}
	}
}