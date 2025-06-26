using System.Collections;
using System.Net;
using System.IO;
using System;

namespace CloudFile
{
	public class LoaderError
	{
		public enum E_Type
		{
			None,
			RequestNoResponse,
			ResponseStatusIsntOK,
			BlocksUploadInitializeError,
			OccurException,
			MD5Error,
		}

		private E_Type m_type;
		public E_Type Type
		{
			get
			{
				return m_type;
			}
			set
			{
				m_type = value;
			}
		}

		private string m_message;
		public string Message
		{
			get
			{
				if (string.IsNullOrEmpty(m_message))
				{
					m_message = m_type.ToString();
				}
				return m_message;
			}
			set
			{
				m_message = value;
			}
		}

		public LoaderError()
		{
			m_type = E_Type.None;
		}

		public void Reset()
		{
			m_type = E_Type.None;
			m_message = null;
		}
	}

	public class CloudFileLoader
	{
		protected E_LoaderState m_myState;
		protected LoaderError m_error;
		private float m_progress;
		protected float Progress
		{
			get
			{
				if (m_progress < 0) return 0;
				if (m_progress > 1) return 1;
				return m_progress;
			}
			set
			{
				m_progress = value;
			}
		}
		protected int m_idForRequestObject;
		public virtual int TaskRecordID
		{
			get{
				return 0;
			}
		}

		public CloudFileLoader()
		{
			m_myState = E_LoaderState.Idle;
			m_error = new LoaderError();
			m_progress = 0;
			m_idForRequestObject = 1;
		}

		public virtual void Start()
		{

		}
		public virtual void Stop()
		{

		}
		public virtual void Pause()
		{

		}
		public virtual void Restart()
		{
			m_idForRequestObject++;
		}
		public virtual void Close()
		{
			Stop();
		}
		public virtual void Reset()
		{
			Stop();
			m_error.Reset();
		}
		protected virtual void ReportProgress()
		{
//			RO.LoggerUnused.Log(Progress);
		}
		protected virtual void ReportError()
		{
//			RO.LoggerUnused.Log("Error, " + m_error.Type + ", " + m_error.Message);
		}
		protected virtual void ReportTimeout()
		{
//			RO.LoggerUnused.Log("Timeout");
		}
		protected virtual void ImDone()
		{
//			RO.LoggerUnused.Log("Done");
		}
	}
}