using UnityEngine;
using System.Collections;
using System.Net;
using System;
using System.IO;
using System.Collections.Generic;

namespace CloudFile
{
	/// <TestCase>
	/// call Start
	/// call Start, call Stop
	/// call Start, call Stop, call Restart
	/// call Start, callRestart
	/// </TestCase>
	public class CloudFileDownloader : CloudFileLoader
	{
		private DownloadTaskRecord m_record;
		public DownloadTaskRecord Record
		{
			get
			{
				return m_record;
			}
			set
			{
				m_record = value;
			}
		}

		private RequestObjectForDownload m_requestObject;

		public bool IsReadyForReadFromResponseStream
		{
			get
			{
				return false;
			}
		}

		private MonoForCloudFileDownloader m_mono;

		public override int TaskRecordID {
			get {
				return m_record.ID;
			}
		}

		private GameObject m_goMono;

		private bool m_bMD5Check;
		public bool BMD5Check
		{
			set
			{
				m_bMD5Check = value;
			}
		}

		public CloudFileDownloader(DownloadTaskRecord record)
		{
			m_record = record;

			m_goMono = GameObject.Find("MonoForCloudFileDownloader");
			if (m_goMono == null)
			{
				m_goMono = new GameObject("MonoForCloudFileDownloader");
				GameObject.DontDestroyOnLoad(m_goMono);
			}
			m_mono = m_goMono.AddComponent<MonoForCloudFileDownloader>();
			m_mono.Initialize(this);
		}
		
		public override void Start()
		{
			m_myState = E_LoaderState.Working;
			
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(m_record.URL);
			request.Method = "GET";
//			request.Headers.Add(UpYunServer.AUTH_KEY, UpYunServer.AUTH_VALUE);
//			request.Headers.Set("Date", DateTime.Now.ToString("r"));
			if (m_record.m_customHeaders != null) {
				foreach (KeyValuePair<string, string> kv in m_record.m_customHeaders) {
					request.Headers.Add (kv.Key, kv.Value);
				}
			}
			m_requestObject = new RequestObjectForDownload(m_idForRequestObject);
			m_requestObject.Request = request;
			try
			{
				request.BeginGetResponse(new AsyncCallback(OnResponse), m_requestObject);
				m_mono.CheckTimeout(request.Timeout / 1000, () => {
					Close();
					ReportTimeout();
				});
			}
			catch (WebException e)
			{
				Close();
				RO.LoggerUnused.LogWarning(e);

				m_error.Type = LoaderError.E_Type.OccurException;
				m_error.Message = e.ToString();
				ReportError();

				// not clear
				throw e;
			}
		}
		
		public override void Stop()
		{
			base.Stop();

			m_myState = E_LoaderState.Idle;
			if (m_requestObject != null)
			{
				m_requestObject.Release();
			}
		}

		/// <summary>
		/// No support.
		/// </summary>
		public override void Pause()
		{
			base.Pause();
		}

		public override void Restart()
		{
			base.Restart();

			Stop();
			if (m_mono == null) {
				m_mono = m_goMono.AddComponent<MonoForCloudFileDownloader>();
				m_mono.Initialize(this);
			}
			m_lengthOfHaveRead = 0;
			Start();
		}
		
		public override void Close()
		{
			base.Close();
		}

		public override void Reset()
		{
			base.Reset();

			m_record = null;
			m_lengthOfHaveRead = 0;
		}

		protected override void ReportProgress()
		{
			base.ReportProgress();

			m_record.State = E_TaskState.Progress;
			m_record.Progress = Progress;

			m_mono.m_progressFlag = true;
		}

		protected override void ReportError()
		{
			base.ReportError();

			m_record.State = E_TaskState.Error;

			m_mono.m_errorFlag = true;
			m_mono.m_errorMessage = m_error.Message;
			m_mono = null;
		}

		protected override void ReportTimeout()
		{
			base.ReportTimeout();

			m_record.State = E_TaskState.Error;

			m_mono.m_timeoutFlag = true;
			m_mono = null;
		}

		protected override void ImDone ()
		{
			base.ImDone();

			m_record.State = E_TaskState.Complete;

			m_mono.m_doneFlag = true;
			m_mono = null;
		}

		private string m_md5FromServer;
		private void OnResponse(IAsyncResult result)
		{
			if (m_myState == E_LoaderState.Working)
			{
				RequestObjectForDownload requestObject = (RequestObjectForDownload)result.AsyncState;
				if (requestObject.ID == m_idForRequestObject)
				{
					m_mono.EndCheckTimeout();

					try
					{
						if (m_requestObject != null)
						{
							HttpWebRequest request = requestObject.Request;
							HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse;
							m_requestObject.Response = response;
							if (response.StatusCode == HttpStatusCode.OK)
							{
								if (m_bMD5Check)
								{
									m_md5FromServer = response.Headers["ETag"];
									if (!string.IsNullOrEmpty(m_md5FromServer))
									{
										m_md5FromServer = m_md5FromServer.Replace("\"", "");
									}
								}
								Stream stream = response.GetResponseStream();
								m_requestObject.ResponseStream = stream;
//								m_mono.DelegateCallReadFromResponseStream(this.ReadFromResponseStream);
								ReadFromResponseStream();
							}
							else
							{
								Close();
								m_error.Type = LoaderError.E_Type.ResponseStatusIsntOK;
								ReportError();
							}
						}
					}
					catch (WebException e)
					{
						Close();
						RO.LoggerUnused.LogWarning(e);

						m_error.Type = LoaderError.E_Type.OccurException;
						m_error.Message = e.ToString();
						ReportError();

						throw e;
					}
					catch (IOException e)
					{
						Close();
						RO.LoggerUnused.LogWarning(e);
						throw e;
					}
				}
			}
		}

		private void ReadFromResponseStream()
		{
			byte[] buffer = m_requestObject.Buffer;
			Stream stream = m_requestObject.ResponseStream;
			try
			{
				stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnReadFromStream), m_requestObject);
				m_mono.CheckTimeout(stream.ReadTimeout / 1000, () => {
					Close();
					ReportTimeout();
				});
			}
			catch (IOException e)
			{
				Close();
				RO.LoggerUnused.LogWarning(e);

				m_error.Type = LoaderError.E_Type.OccurException;
				m_error.Message = e.ToString();
				ReportError();

				throw e;
			}
		}

		private int m_lengthOfHaveRead;
		private void OnReadFromStream(IAsyncResult result)
		{
			if (m_myState == E_LoaderState.Working)
			{
				RequestObjectForDownload requestObject = (RequestObjectForDownload)result.AsyncState;
				if (requestObject.ID == m_idForRequestObject)
				{
					m_mono.EndCheckTimeout();

					try
					{
						int readBytesLength = requestObject.ResponseStream.EndRead(result);
						if (readBytesLength > 0)
						{
							FileHelper.AppendBytes(Record.Path, m_requestObject.Buffer, readBytesLength);
							m_lengthOfHaveRead += readBytesLength;
							Progress = (float)m_lengthOfHaveRead / m_requestObject.ContentLength;
							ReportProgress();
							ReadFromResponseStream();
						}
						else
						{
							Close();
							if (m_bMD5Check)
							{
								if (!string.IsNullOrEmpty(m_md5FromServer))
								{
									if (string.Equals(MyMD5.HashFile(m_record.Path), m_md5FromServer))
									{
										ImDone();	
									}
									else
									{
										m_error.Type = LoaderError.E_Type.MD5Error;
										ReportError();
									}
								}
								else
								{
									ImDone();
								}
							}
							else
							{
								ImDone();
							}
						}
					}
					catch (IOException e)
					{
						Close();
						RO.LoggerUnused.LogWarning(e);

						m_error.Type = LoaderError.E_Type.OccurException;
						m_error.Message = e.ToString();
						ReportError();

						throw e;
					}
				}
			}
		}
	}
}