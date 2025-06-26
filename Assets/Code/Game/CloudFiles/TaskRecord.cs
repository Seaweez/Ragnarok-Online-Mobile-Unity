using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace CloudFile
{
	public class TaskRecord
	{
		private int m_id;
		public int ID
		{
			get
			{
				return m_id;
			}
		}
		private string m_url;
		public string URL
		{
			get
			{
				return m_url;
			}
			set {
				m_url = value;
			}
		}
		public Dictionary<string, string> m_customHeaders;
		public void SetCustomHeaders(string[] custom_headers)
		{
			m_customHeaders = CloudFileManager.ArrayToDictionary (custom_headers);
		}
		public Dictionary<string, string> GetCustomHeaders()
		{
			return m_customHeaders;
		}
		private E_TaskState m_state;
		public E_TaskState State
		{
			get
			{
				return m_state;
			}
			set
			{
				m_state = value;
			}
		}
//		private TaskError m_error;
//		public TaskError Error
//		{
//			get
//			{
//				return m_error;
//			}
//			set
//			{
//				m_error = value;
//			}
//		}
		private float m_progress;
		public float Progress
		{
			get
			{
				return m_progress;
			}
			set
			{
				m_progress = value;
			}
		}

		public TaskRecord(int id, string url, Dictionary<string, string> custom_headers)
		{
			m_id = id;
			m_url = url;
			m_customHeaders = custom_headers;
		}

		public virtual bool IsValid()
		{
			return ID > 0 && State != E_TaskState.Progress;
		}
	}

	public class DownloadTaskRecord : TaskRecord
	{
		private string m_path;
		public string Path
		{
			get
			{
				return m_path;
			}
		}

		public DownloadTaskRecord(int id, string path, string url, Dictionary<string, string> custom_headers) : base(id, url, custom_headers)
		{
			m_path = path;
		}
	}

	public class UploadTaskRecord : TaskRecord
	{
		public UploadTaskRecord(int id, string url, Dictionary<string, string> custom_headers) : base(id, url, custom_headers)
		{

		}
	}

	public class UploadTaskRecordForNormal : UploadTaskRecord
	{
		private string m_path;
		public string Path
		{
			get
			{
				return m_path;
			}
		}

		public string FileExtension
		{
			get
			{
				return FileHelper.GetFileExtensionFromPath(Path);
			}
		}
		
		public UploadTaskRecordForNormal(int id, string path, string url, Dictionary<string, string> custom_headers) : base(id, url, custom_headers)
		{
			m_path = path;
		}

		public override bool IsValid ()
		{
			if (!base.IsValid())
			{
				return false;
			}
			if (!FileHelper.ExistFile(Path))
			{
				return false;
			}
			return true;
		}
	}

	public class UploadTaskRecordForForm : UploadTaskRecord
	{
		private string m_path;
		public string Path
		{
			get
			{
				return m_path;
			}
		}
		public string FileName
		{
			get
			{
				return FileHelper.GetFileNameFromPath(Path);
			}
		}

		private string m_policy;
		public string Policy
		{
			get
			{
				return m_policy;
			}
		}

		private string m_authorization;
		public string Authorization
		{
			get
			{
				return m_authorization;
			}
		}

		public UploadTaskRecordForForm(int id, string path, string url, string policy, string authorization, Dictionary<string, string> custom_headers) : base(id, url, custom_headers)
		{
			m_path = path;
			m_policy = policy;
			m_authorization = authorization;
		}

		public override bool IsValid ()
		{
			if (!base.IsValid())
			{
				return false;
			}
			if (!FileHelper.ExistFile(Path))
			{
				return false;
			}
			return true;
		}
	}

	public class UploadTaskRecordForBlocks : UploadTaskRecord
	{
		public UploadTaskRecordForBlocks(int id, string url, Dictionary<string, string> custom_headers) : base(id, url, custom_headers)
		{
			
		}

		public override bool IsValid ()
		{
			if (!base.IsValid())
			{
				return false;
			}
			return true;
		}
	}
}