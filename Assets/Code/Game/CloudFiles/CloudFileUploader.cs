using System.Collections;
using System.Net;
using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace CloudFile
{
	public class CloudFileUploader : CloudFileLoader
	{
		protected byte[] m_bytes;

		// bytes for upload encrypted file
		public CloudFileUploader(byte[] bytes)
		{
			m_bytes = bytes;
		}
		
		public override void Start()
		{
			base.Start();
		}
		
		public override void Stop()
		{
			base.Stop();

			m_bytes = null;
		}
		
		public override void Pause()
		{
			base.Pause();
		}
		
		public override void Restart()
		{
			base.Restart();
		}
		
		public override void Close()
		{
			base.Close();
		}
		
		public override void Reset()
		{
			base.Reset();
		}
		
		protected override void ReportProgress()
		{
			base.ReportProgress();
		}
		
		protected override void ReportError()
		{
			base.ReportError();
		}
		
		protected override void ReportTimeout()
		{
			base.ReportTimeout();
		}
		
		protected override void ImDone ()
		{
			base.ImDone();
		}
	}

	/// <TestCase>
	/// call Start
	/// call Start, call Stop
	/// call Start, call Stop, call Restart
	/// </TestCase>
	public class CloudFileNormalUploader : CloudFileUploader
	{
		private UploadTaskRecordForNormal m_record;
		public UploadTaskRecordForNormal Record
		{
			get
			{
				return m_record;
			}
		}

		private RequestObjectForUpload m_requestObject;

		private MonoForCloudFileNormalUploader m_mono;

		public override int TaskRecordID {
			get {
				return m_record.ID;
			}
		}

		private GameObject m_goMono;
		public CloudFileNormalUploader(UploadTaskRecordForNormal record, byte[] bytes) : base(bytes)
		{
			m_record = record;
			m_goMono = GameObject.Find("MonoForCloudFileNormalUploader");
			if (m_goMono == null)
			{
				m_goMono = new GameObject("MonoForCloudFileNormalUploader");
				GameObject.DontDestroyOnLoad(m_goMono);
			}
			m_mono = m_goMono.AddComponent<MonoForCloudFileNormalUploader>();
			m_mono.Initialize (this);
		}
		
		public override void Start()
		{
			
			#region(design defect)
			if (m_bytes == null)
			{
				m_bytes = FileHelper.LoadFile(m_record.Path);
			}
			#endregion
			if (m_bytes.Length > 0)
			{
				base.Start();
				
				m_myState = E_LoaderState.Working;
				
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_record.URL);
				request.Method = "PUT";
				request.Headers.Add(CloudServer.AUTH_KEY, CloudServer.AUTH_VALUE);
				string md5Value = MyMD5.HashFile(m_record.Path);
				request.Headers.Add("Content-MD5", md5Value);
				if (m_record.m_customHeaders != null) {
					foreach (KeyValuePair<string, string> kv in m_record.m_customHeaders) {
						request.Headers.Add (kv.Key, kv.Value);
					}
				}
				if (!string.IsNullOrEmpty(m_record.FileExtension))
				{
					request.ContentType = m_record.FileExtension;
				}
				request.ContentLength = m_bytes.Length;
				request.KeepAlive = true;
				request.AllowWriteStreamBuffering = false;
				m_requestObject = new RequestObjectForUpload(m_idForRequestObject);
				m_requestObject.Request = request;
				try
				{
					request.BeginGetRequestStream(new AsyncCallback(OnGetRequestStream), m_requestObject);
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

					throw e;
				}
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
			m_requestObject = null;
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
				m_mono = m_goMono.AddComponent<MonoForCloudFileNormalUploader>();
				m_mono.Initialize(this);
			}
			m_lengthOfHaveWrited = 0;
			indicator = 0;
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
			m_lengthOfHaveWrited = 0;
			indicator = 0;
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

		private void OnGetRequestStream(IAsyncResult result)
		{
			if (m_myState == E_LoaderState.Working)
			{
				RequestObjectForUpload requestObject = (RequestObjectForUpload)result.AsyncState;
				if (requestObject != null && requestObject.ID == m_idForRequestObject)
				{
					m_mono.EndCheckTimeout();

					HttpWebRequest request = requestObject.Request;
					Stream requestStream = null;
					try
					{
						requestStream = request.EndGetRequestStream(result);
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
					if (requestStream != null)
					{
						m_requestObject.RequestStream = requestStream;
						WriteToRequestStream();
					}
				}
			}
		}

		private int indicator = 0;
		private int validSize = 0;
		private void WriteToRequestStream()
		{
			Stream requestStream = m_requestObject.RequestStream;
			int totalSize = m_bytes.Length;
			if (totalSize > 0)
			{
				int leftSize = totalSize - indicator;
				if (leftSize > 0)
				{
					validSize = Math.Min(leftSize, m_requestObject.Buffer.Length);
					Array.Copy(m_bytes, indicator, m_requestObject.Buffer, 0, validSize);
					indicator += validSize;
					try
					{
						requestStream.BeginWrite(m_requestObject.Buffer, 0, validSize, OnWriteToRequestStream, m_requestObject);
						m_mono.CheckTimeout(requestStream.WriteTimeout / 1000, () => {
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
			}
		}

		// The last BeginWrite cost longer time.
		private int m_lengthOfHaveWrited;
		private void OnWriteToRequestStream(IAsyncResult result)
		{
			if (m_myState == E_LoaderState.Working)
			{
				RequestObjectForUpload requestObject = (RequestObjectForUpload)result.AsyncState;
				if (requestObject != null && requestObject.ID == m_idForRequestObject)
				{
					m_mono.EndCheckTimeout();

					Stream requestStream = requestObject.RequestStream;
					try
					{
						requestStream.EndWrite(result);
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
					m_lengthOfHaveWrited += validSize;
					Progress = (float)m_lengthOfHaveWrited / m_bytes.Length;
					ReportProgress();
					if (Progress >= 1)
					{
						requestStream.Close();
						requestObject.RequestStream = null;
						try
						{
							m_requestObject.Request.BeginGetResponse(new AsyncCallback(OnResponse), m_requestObject);
							m_mono.CheckTimeout(m_requestObject.Request.Timeout / 1000, () => {
								Close();
								ReportTimeout();
							});
						}
						catch(WebException e)
						{
							Close();
							RO.LoggerUnused.LogWarning(e);

							m_error.Type = LoaderError.E_Type.OccurException;
							m_error.Message = e.ToString();
							ReportError();

							throw e;
						}
					}
					else
					{
						WriteToRequestStream();
					}
				}
			}
		}

		private void OnResponse(IAsyncResult result)
		{
			if (m_myState == E_LoaderState.Working)
			{
				RequestObjectForUpload requestObject = (RequestObjectForUpload)result.AsyncState;
				if (requestObject != null && requestObject.ID == m_idForRequestObject)
				{
					m_mono.EndCheckTimeout();

					try
					{
						if (requestObject != null)
						{
							HttpWebRequest request = requestObject.Request;
							HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse;
							m_requestObject.Response = response;
							if (response.StatusCode == HttpStatusCode.OK)
							{
								Close();
								ImDone();
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

						m_error.Type = LoaderError.E_Type.OccurException;
						m_error.Message = e.ToString();
						ReportError();

						throw e;
					}
				}
			}
		}
	}

	public class CloudFileFormUploader : CloudFileUploader
	{
		private UploadTaskRecordForForm m_record;
		public UploadTaskRecordForForm Record
		{
			get
			{
				return m_record;
			}
		}

		private RequestObjectForFormUpload m_requestObject;

		private MonoForCloudFileFormUploader m_mono;

		public override int TaskRecordID {
			get {
				return m_record.ID;
			}
		}

		private GameObject m_goMono;

		string boundary = "------------------------4e222b2b31bda20c";

		public CloudFileFormUploader(UploadTaskRecordForForm record, byte[] bytes) : base(bytes)
		{
			m_record = record;
			m_goMono = GameObject.Find("MonoForCloudFileFormUploader");
			if (m_goMono == null)
			{
				m_goMono = new GameObject("MonoForCloudFileFormUploader");
				GameObject.DontDestroyOnLoad(m_goMono);
			}
			m_mono = m_goMono.AddComponent<MonoForCloudFileFormUploader>();
			m_mono.Initialize (this);
		}

		public override void Start()
		{
			#region(design defect)
			if (m_bytes == null)
			{
				m_bytes = FileHelper.LoadFile(m_record.Path);
			}
			#endregion
			if (m_bytes.Length > 0)
			{
				base.Start();

				m_myState = E_LoaderState.Working;

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_record.URL);
				request.Method = "POST";
				if (m_record.m_customHeaders != null) {
					foreach (KeyValuePair<string, string> kv in m_record.m_customHeaders) {
						request.Headers.Add (kv.Key, kv.Value);
					}
				}
				request.ContentType = "multipart/form-data; boundary=" + boundary;
				m_requestObject = new RequestObjectForFormUpload(m_idForRequestObject);
				m_requestObject.Request = request;
				try
				{
					request.BeginGetRequestStream(new AsyncCallback(OnGetRequestStream), m_requestObject);
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

					throw e;
				}
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
			m_requestObject = null;
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
				m_mono = m_goMono.AddComponent<MonoForCloudFileFormUploader>();
				m_mono.Initialize(this);
			}
			m_lengthOfHaveWrited = 0;
			indicator = 0;
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
			m_lengthOfHaveWrited = 0;
			indicator = 0;
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

		private void OnGetRequestStream(IAsyncResult result)
		{
			if (m_myState == E_LoaderState.Working)
			{
				RequestObjectForFormUpload requestObject = (RequestObjectForFormUpload)result.AsyncState;
				if (requestObject != null && requestObject.ID == m_idForRequestObject)
				{
					m_mono.EndCheckTimeout();

					HttpWebRequest request = requestObject.Request;
					Stream requestStream = null;
					try
					{
						requestStream = request.EndGetRequestStream(result);
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
					if (requestStream != null)
					{
						m_requestObject.RequestStream = requestStream;
						bool bWrited = false;
						try
						{
							WriteBoundary (requestStream);
							WriteLine (requestStream);
							WriteText (requestStream, "authorization", m_record.Authorization);
							WriteLine (requestStream);
							WriteBoundary (requestStream);
							WriteLine (requestStream);
							WriteFileProperty(requestStream, m_record.FileName, null);
							WriteLine(requestStream);
							WriteLine(requestStream);
							bWrited = true;
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
						if (bWrited)
						{
							WriteFileToRequestStream ();
						}
					}
				}
			}
		}

		private int indicator = 0;
		private int validSize = 0;
		private void WriteFileToRequestStream()
		{
			Stream requestStream = m_requestObject.RequestStream;
			int totalSize = m_bytes.Length;
			if (totalSize > 0)
			{
				int leftSize = totalSize - indicator;
				if (leftSize > 0)
				{
					validSize = Math.Min(leftSize, m_requestObject.Buffer.Length);
					Array.Copy(m_bytes, indicator, m_requestObject.Buffer, 0, validSize);
					indicator += validSize;
					try
					{
						requestStream.BeginWrite(m_requestObject.Buffer, 0, validSize, new AsyncCallback(OnWriteToRequestStream), m_requestObject);
						m_mono.CheckTimeout(requestStream.WriteTimeout / 1000, () => {
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
			}
		}

		void WriteText(Stream stream, string name, string value)
		{
			string property = string.Format ("Content-Disposition: form-data; name=\"{0}\"", name);
			byte[] bytesProperty = Encoding.UTF8.GetBytes (property);
			stream.Write (bytesProperty, 0, bytesProperty.Length);
			WriteLine (stream);
			WriteLine (stream);
			byte[] bytesValue = Encoding.UTF8.GetBytes (value);
			stream.Write (bytesValue, 0, bytesValue.Length);
		}

		void WriteLine(Stream stream)
		{
			byte[] bytesCRLF = Encoding.UTF8.GetBytes ("\r\n");
			stream.Write (bytesCRLF, 0, bytesCRLF.Length);
		}

		void WriteBoundary(Stream stream)
		{
			byte[] bytesBoundary = Encoding.UTF8.GetBytes ("--" + boundary); // must "--", or 400
			stream.Write (bytesBoundary, 0, bytesBoundary.Length);
		}

		void WriteCustom(Stream stream, string str)
		{
			byte[] bytesCustom = Encoding.UTF8.GetBytes (str);
			stream.Write (bytesCustom, 0, bytesCustom.Length);
		}

		void WriteFileProperty (Stream stream, string file_name, string file_content_type)
		{
			string property = string.Format ("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", "file", file_name); // <name> must be "file", or 400; <filename> must not be null of empty
			byte[] bytesProperty = Encoding.UTF8.GetBytes (property);
			stream.Write (bytesProperty, 0, bytesProperty.Length);
			WriteLine (stream);
			if (string.IsNullOrEmpty (file_content_type)) {
				property = "Content-Type: application/octet-stream"; // binary. When visit the file, download the file.
			} else {
				property = "Content-Type: " + file_content_type;
			}
			bytesProperty = Encoding.UTF8.GetBytes (property);
			stream.Write (bytesProperty, 0, bytesProperty.Length);
		}

		// The last BeginWrite cost longer time.
		private int m_lengthOfHaveWrited;
		private void OnWriteToRequestStream(IAsyncResult result)
		{
			if (m_myState == E_LoaderState.Working)
			{
				RequestObjectForFormUpload requestObject = (RequestObjectForFormUpload)result.AsyncState;
				if (requestObject != null && requestObject.ID == m_idForRequestObject)
				{
					m_mono.EndCheckTimeout();

					Stream requestStream = requestObject.RequestStream;
					try
					{
						requestStream.EndWrite(result);
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
					m_lengthOfHaveWrited += validSize;
					Progress = (float)m_lengthOfHaveWrited / m_bytes.Length;
					ReportProgress();
					if (Progress >= 1)
					{
						bool bWrited = false;
						try
						{
							WriteLine (requestStream);
							WriteBoundary (requestStream);
							WriteLine (requestStream);
							WriteText (requestStream, "policy", m_record.Policy);
							WriteLine (requestStream);
							WriteBoundary (requestStream);
							WriteCustom (requestStream, "--");
							WriteLine (requestStream);
							bWrited = true;
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
						if (bWrited)
						{
							// Need use BeginGetResponse by this way.
							// If not, no error occur when normal upload, error occur when form upload.
							requestStream.Close ();
							m_requestObject.RequestStream = null;
							try
							{
								m_requestObject.Request.BeginGetResponse (new AsyncCallback (OnResponse), m_requestObject);
								m_mono.CheckTimeout (m_requestObject.Request.Timeout / 1000, () => {
									Close ();
									ReportTimeout ();
								});
							}
							catch (WebException e)
							{
								Close();
								RO.LoggerUnused.LogWarning(e);

								m_error.Type = LoaderError.E_Type.OccurException;
								m_error.Message = e.ToString();
								ReportError();
							}
						}
					}
					else
					{
						WriteFileToRequestStream();
					}
				}
			}
		}

		private void OnResponse(IAsyncResult result)
		{
			if (m_myState == E_LoaderState.Working)
			{
				RequestObjectForFormUpload requestObject = (RequestObjectForFormUpload)result.AsyncState;
				if (requestObject != null && requestObject.ID == m_idForRequestObject)
				{
					m_mono.EndCheckTimeout();

					try
					{
						if (requestObject != null)
						{
							HttpWebRequest request = requestObject.Request;
							HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse;
							m_requestObject.Response = response;
							if (response.StatusCode == HttpStatusCode.OK)
							{
								Close();
								ImDone();
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

						m_error.Type = LoaderError.E_Type.OccurException;
						m_error.Message = e.ToString();
						ReportError();

						throw e;
					}
				}
			}
		}

//		string _signature = "";
//		string _policy = "";
//		string _authorization = "";
//		string _operatorName = "";
//		string _password = "";
//		string _bucket = "";
//		string _path = "";
//		string _method = "";
//		string GeneratePolicy()
//		{
//			Dictionary<string, string> dict = new Dictionary<string, string> ();
//			dict.Add ("bucket", _bucket);
//			dict.Add ("save-key", _path);
//			dict.Add("expiration", GetExpiration(30 * 60).ToString());
//			string strJson = Spine.Json.Serialize (dict);
//			byte[] bytes = Encoding.UTF8.GetBytes(strJson);
//			return Convert.ToBase64String(bytes);
//		}
//
//		private int GetExpiration(int seconds)
//		{
//			DateTime dtGreenwich = new DateTime(1970, 1, 1, 0, 0, 0);
//			DateTime dtNow = DateTime.Now; // 不能用utcnow, 否则认证过期
//			TimeSpan ts = dtNow.Subtract(dtGreenwich);
//			int expiration = (int)(ts.TotalSeconds + seconds);
//			return expiration;
//		}
//
//		string GenerateSignature()
//		{
//			string md5OfPassword = MyMD5.HashString(_password);
//			return Convert.ToBase64String(
//				HmacSha1(
//					md5OfPassword,
//					_method + "&" +
//					"/" + _bucket + "&" +
//					_policy
//				)
//			);
//		}
//
//		string GenerateAuthorization()
//		{
//			return "UPYUN " + _operatorName + ":" + _signature;
//		}
//
//		public static byte[] HmacSha1(string key, string input)
//		{
//			byte[] keyBytes = ASCIIEncoding.ASCII.GetBytes(key);
//			byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes(input);
//			System.Security.Cryptography.HMACSHA1 hmac = new System.Security.Cryptography.HMACSHA1(keyBytes);
//			byte[] hashBytes = hmac.ComputeHash(inputBytes);
//			return hashBytes;
//		}
	}

	public class CloudFileBlocksUploader : CloudFileUploader
	{
		private UploadTaskRecordForBlocks m_record;
		public UploadTaskRecordForBlocks Record
		{
			get
			{
				return m_record;
			}
		}
		
		private RequestObjectForBlocksUpload m_requestObject;
		
		private string m_saveTokenFromInitialize;
		public string SaveTokenFromInitialize
		{
			get
			{
				return m_saveTokenFromInitialize;
			}
			set
			{
				m_saveTokenFromInitialize = value;
			}
		}
		private string m_tokenSecretFromInitialize;
		public string TokenSecretFromInitialize
		{
			get
			{
				return m_tokenSecretFromInitialize;
			}
			set
			{
				m_tokenSecretFromInitialize = value;
			}
		}

		private string m_saveTokenFromUploadBlockData;
		public string SaveTokenFromUploadBlockData
		{
			get
			{
				return m_saveTokenFromUploadBlockData;
			}
			set
			{
				m_saveTokenFromUploadBlockData = value;
			}
		}
		private string m_tokenSecretFromUploadBlockData;
		public string TokenSecretFromUploadBlockData
		{
			get
			{
				return m_tokenSecretFromUploadBlockData;
			}
			set
			{
				m_tokenSecretFromUploadBlockData = value;
			}
		}
		
		public CloudFileBlocksUploader(UploadTaskRecordForBlocks record, byte[] bytes) : base(bytes)
		{
			m_record = record;
		}
		
		public override void Start()
		{
			if (m_bytes.Length > 0)
			{
				base.Start();
				
				m_myState = E_LoaderState.Working;
				
				m_requestObject = new RequestObjectForBlocksUpload(m_idForRequestObject);
				
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_record.URL);
				request.Method = "PUT";
				request.Headers.Add(UpYunServer.AUTH_KEY, UpYunServer.AUTH_VALUE);
				request.Headers.Add("X-Upyun-Multi-Stage", "initiate");
				request.Headers.Add("X-Upyun-Multi-Type", "image/png");
				request.Headers.Add("X-Upyun-Multi-Length", m_bytes.Length.ToString());
				
				m_requestObject.RequestForInitialize = request;
				
				try
				{
					request.BeginGetResponse(new AsyncCallback(OnResponseForInitialize), m_requestObject);
				}
				catch (WebException e)
				{
					Close();
					RO.LoggerUnused.LogWarning(e);
					throw e;
				}
			}
		}
		
		public override void Stop()
		{
			base.Stop();
		}
		
		public override void Pause()
		{
			base.Pause();
		}
		
		public override void Restart()
		{

		}
		
		public override void Close()
		{
			base.Close();
		}
		
		public override void Reset()
		{

		}
		
		protected override void ReportProgress()
		{
			base.ReportProgress();
		}
		
		protected override void ReportError()
		{
			base.ReportError();
		}
		
		protected override void ReportTimeout()
		{
			base.ReportTimeout();
		}
		
		protected override void ImDone()
		{
			base.ImDone();
		}

		private void OnResponseForInitialize(IAsyncResult result)
		{
			if (m_myState == E_LoaderState.Working)
			{
				RequestObjectForBlocksUpload requestObject = (RequestObjectForBlocksUpload)result.AsyncState;
				if (requestObject != null && requestObject.ID == m_idForRequestObject)
				{
					try
					{
						HttpWebRequest request = requestObject.RequestForInitialize;
						HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse;
						m_requestObject.ResponseForInitialize = response;
						if (response.StatusCode == HttpStatusCode.OK)
						{
							Stream responseStream = response.GetResponseStream();
							m_requestObject.ResponseStreamForInitialize = responseStream;
							StreamReader sr = new StreamReader(responseStream);
							string responseString = sr.ReadToEnd();
							Dictionary<string, object> dict = Spine.Json.Deserialize(new StringReader(responseString)) as Dictionary<string, object>;
							Debug.Log(dict["X-Upyun-Multi-UUID"]);
							Debug.Log(dict["X-Upyun-Next-Part-ID"]);
						}
						else
						{
							Close();
							m_error.Type = LoaderError.E_Type.ResponseStatusIsntOK;
							ReportError();
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

						m_error.Type = LoaderError.E_Type.OccurException;
						m_error.Message = e.ToString();
						ReportError();

						throw e;
					}
				}
			}
		}

		private void OnGetRequestStreamForUploadBlockData(IAsyncResult result)
		{
			if (m_myState == E_LoaderState.Working)
			{
				RequestObjectForBlocksUpload requestObject = (RequestObjectForBlocksUpload)result.AsyncState;
				if (requestObject != null && requestObject.ID == m_idForRequestObject)
				{
					HttpWebRequest request = requestObject.RequestForUploadBlockData;
					Stream requestStream = null;
					try
					{
						requestStream = request.EndGetRequestStream(result);
					}
					catch (WebException e)
					{
						Close();
						RO.LoggerUnused.LogWarning(e);
						throw e;
					}
					if (requestStream != null)
					{
						m_requestObject.RequestStreamForUploadBlockData = requestStream;
					}
				}
			}
		}

		private int validSize = 0;
		private int m_indicatorForBlockData = 0;
		private int m_totalSizeRelativeBlockData = 0;
		private void DoWriteToRequestStreamForUploadBlockData()
		{
			if (m_totalSizeRelativeBlockData > 0)
			{
				int leftSize = m_totalSizeRelativeBlockData - m_indicatorForBlockData;
				if (leftSize > 0)
				{
					validSize = Math.Min(leftSize, m_requestObject.Buffer.Length);
					Array.Copy(null, m_indicatorForBlockData, m_requestObject.Buffer, 0, validSize);
					m_indicatorForBlockData += validSize;
					
					Stream requestStream = m_requestObject.RequestStreamForUploadBlockData;
					try
					{
						requestStream.BeginWrite(m_requestObject.Buffer, 0, validSize, OnWriteToRequestStreamForUploadBlockData, m_requestObject);
					}
					catch (IOException e)
					{
						Close();
						RO.LoggerUnused.LogWarning(e);
						throw e;
					}
				}
			}
			else
			{
				Debug.LogError("m_totalSizeRelativeBlockData less than 1");
			}
		}

		private int m_lengthOfHaveWritedRelativeBlockData = 0;
		private void OnWriteToRequestStreamForUploadBlockData(IAsyncResult result)
		{
			if (m_myState == E_LoaderState.Working)
			{
				RequestObjectForBlocksUpload requestObject = (RequestObjectForBlocksUpload)result.AsyncState;
				if (requestObject != null && requestObject.ID == m_idForRequestObject)
				{
					Stream requestStream = requestObject.RequestStreamForUploadBlockData;
					try
					{
						requestStream.EndWrite(result);
					}
					catch (IOException e)
					{
						Close();
						RO.LoggerUnused.LogWarning(e);
						throw e;
					}
					m_lengthOfHaveWritedRelativeBlockData += validSize;
					if (m_lengthOfHaveWritedRelativeBlockData >= 0)
					{
						try
						{
							m_requestObject.RequestForUploadBlockData.BeginGetResponse(new AsyncCallback(OnResponseForUploadBlockData), m_requestObject);
						}
						catch (WebException e)
						{
							Close();
							RO.LoggerUnused.LogWarning(e);
							throw e;
						}
					}
					else
					{
						DoWriteToRequestStreamForUploadBlockData();
					}
				}
			}
		}

		private void OnResponseForUploadBlockData(IAsyncResult result)
		{
			if (m_myState == E_LoaderState.Working)
			{
				RequestObjectForBlocksUpload requestObject = (RequestObjectForBlocksUpload)result.AsyncState;
				if (requestObject != null && requestObject.ID == m_idForRequestObject)
				{
					try
					{
						HttpWebRequest request = requestObject.RequestForUploadBlockData;
						if (request.HaveResponse)
						{
							HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse;
							m_requestObject.ResponseForUploadBlockData = response;
							if (response.StatusCode == HttpStatusCode.OK)
							{
								Stream responseStream = response.GetResponseStream();
								m_requestObject.ResponseStreamForUploadBlockData = responseStream;
								StreamReader sr = new StreamReader(responseStream);
								string responseString = sr.ReadToEnd();
								Dictionary<string, object> dict = Spine.Json.Deserialize(new StringReader(responseString)) as Dictionary<string, object>;
								if (!dict.ContainsKey("save_token") || !dict.ContainsKey("token_secret"))
								{
									Close();
									m_error.Type = LoaderError.E_Type.BlocksUploadInitializeError;
									ReportError();
								}
								else
								{
									
								}
							}
							else
							{
								Close();
								m_error.Type = LoaderError.E_Type.ResponseStatusIsntOK;
								ReportError();
							}
						}
						else
						{
							Close();
							m_error.Type = LoaderError.E_Type.RequestNoResponse;
							ReportError();
						}
					}
					catch (WebException e)
					{
						Close();
						RO.LoggerUnused.LogWarning(e);
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
		
		public byte[] BytesOfFormBoundary
		{
			get
			{
				return System.Text.Encoding.ASCII.GetBytes(CRLF);
			}
		}
		private const string FIELD_DESCRIPTION_FORMAT_1 = "Content-Disposition: form-data; name=\"{0}\"";
		private const string FIELD_DESCRIPTION_FORMAT_2 = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"";
		private const string CRLF = "\r\n";
		public byte[] BytesOfCRLF
		{
			get
			{
				return System.Text.Encoding.ASCII.GetBytes(CRLF);
			}
		}
		private int m_currentBlockIndex;
	}
}