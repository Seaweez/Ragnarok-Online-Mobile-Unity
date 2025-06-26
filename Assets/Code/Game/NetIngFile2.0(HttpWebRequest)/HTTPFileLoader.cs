using UnityEngine;
using System.Collections;
using System.Net;
using System;
using System.IO;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using RO;

// custom error code
// 2 : download response stream time out
// 3 : download file not full
// 11 : upload post stream time out
// 12 : upload file not full

public class HTTPFileLoader
{
	public enum E_State
	{
		None,
		Ing,
		Pause,
		End,
		Err
	}

	protected int m_id;
	public int ID
	{
		get{return m_id;}
	}
	protected string m_url;
	public string URL
	{
		get{return m_url;}
	}
	public Dictionary<string, string> m_customHeaders;
	protected E_State m_state;
	public E_State State
	{
		get{return m_state;}
	}
	protected float m_progress;
	public float Progress
	{
		get{return m_progress;}
	}

	public bool m_bS;
	public bool m_bP;
	public bool m_bC;
	public bool m_bE;

	public virtual void Start()
	{

	}

	public virtual void Pause()
	{

	}

	public virtual void Continue()
	{

	}

	public virtual void Stop()
	{

	}

	protected virtual void Release()
	{

	}

	public virtual void FireOnStart()
	{
		
	}
	
	public virtual void FireOnProgress()
	{
		
	}
	
	public virtual void FireOnComplete()
	{
		
	}
	
	public virtual void FireOnError()
	{
		
	}
}

public class HTTPFileDownloader : HTTPFileLoader
{	
	public enum E_Error
	{
		None,
		NetErr,
		FileNotFull,
		MD5Inconsistent,
	}

	private class RequestState
	{
		public const int BUFFER_SIZE = 1024;
		public byte[] m_buffer;
		public HttpWebRequest m_request;
		public HttpWebResponse m_response;
		public Stream m_responseStream;
		public Stream m_fileStream;

		public RequestState()
		{
			m_buffer = new byte[BUFFER_SIZE];
		}
	}

	private RequestState m_requestObject;

	private static int m_count;
	public static int Count
	{
		get{return m_count;}
	}

	// url
	private string m_fileName;
	private string m_path;
	public string Path
	{
		get{return m_path;}
	}
	private bool m_b = true;
	private bool m_bStop = true;
	// state
	// progress
	private E_Error m_err;
	private int m_errCode;
	private string m_errMessage;
	private Action m_actionOnStart;
	private Action<float> m_actionOnProgress;
	private Action<string> m_actionOnComplete;
	private Action<int, int, string> m_actionOnError;
	private bool m_bCheckMD5;
	public bool BCheckMD5
	{
		set
		{
			m_bCheckMD5 = value;
		}
	}
	
	public HTTPFileDownloader(string url, Dictionary<string, string> custum_headers, Action action_on_start, Action<float> action_on_progress, Action<string> action_on_complete, Action<int, int, string> action_on_error)
	{
		m_id = ++m_count;
		m_url = url;
		m_customHeaders = custum_headers;
		string[] urlSplitBySlash = m_url.Split('/');
		if (urlSplitBySlash != null && urlSplitBySlash.Length > 0)
			m_fileName = urlSplitBySlash[urlSplitBySlash.Length - 1];
		m_path = NetIngFileTaskManager.Me.RootDirectoryPath + "/" + m_fileName;
		m_state = E_State.None;
		m_progress = 0;
		m_err = E_Error.None;
		m_errCode = 0;
		m_errMessage = "";
		m_actionOnStart = action_on_start;
		m_actionOnProgress = action_on_progress;
		m_actionOnComplete = action_on_complete;
		m_actionOnError = action_on_error;
	}

	public override void Start()
	{
		HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(m_url);
		if (m_customHeaders != null) {
			foreach(KeyValuePair<string, string> kv in m_customHeaders)
			{
				request.Headers.Add (kv.Key);
				request.Headers.Add (kv.Value);
			}
		}
		m_requestObject = new RequestState();
		m_requestObject.m_request = request;
		request.BeginGetResponse(new AsyncCallback(OnResponse), m_requestObject);
		m_state = E_State.Ing;
		m_bS = true;
	}

	public override void Pause()
	{
		m_b = false;
	}

	public override void Continue()
	{
		m_b = true;
	}

	public override void Stop()
	{
		m_b = true;
		m_bStop = false;

		Release();

		m_state = E_State.End;
		m_progress = 0;
	}

	protected override void Release()
	{
		if(m_requestObject!=null)
		{
			m_requestObject.m_buffer = null;
			if (m_requestObject.m_request != null)
			{
				m_requestObject.m_request.Abort();
			}
			if (m_requestObject.m_response != null)
			{
				m_requestObject.m_response.Close();
			}
			if (m_requestObject.m_responseStream != null)
			{
				m_requestObject.m_responseStream.Close();
			}
			if (m_requestObject.m_fileStream != null)
			{
				m_requestObject.m_fileStream.Close();
			}
		}
	}

	private string m_md5FromServer;
	private void OnResponse(IAsyncResult pResult)
	{
		try
		{
			RequestState rs = (RequestState)pResult.AsyncState;
			HttpWebRequest request = rs.m_request;

			if (!request.HaveResponse)
			{
				Release();
				m_b = false;
				m_state = E_State.Err;
				m_err = E_Error.NetErr;
				m_errCode = 404;
				m_errMessage = "No response.";
				m_bE = true;
				return;
			}
			HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(pResult);
			rs.m_response = response;
			HttpStatusCode statusCode = response.StatusCode;
			if (statusCode != HttpStatusCode.OK)
			{
				Release();
				m_b = false;
				m_state = E_State.Err;
				m_err = E_Error.NetErr;
				m_errCode = (int)statusCode;
				m_errMessage = response.StatusDescription;
				m_bE = true;
				return;
			}
			m_md5FromServer = response.Headers["ETag"];
			if (!string.IsNullOrEmpty(m_md5FromServer))
			{
				m_md5FromServer = m_md5FromServer.Replace("\"", "");
			}
			int contentLength = (int)response.ContentLength;
			Stream responseStream = response.GetResponseStream();
			responseStream.ReadTimeout = 30000;
			rs.m_responseStream = responseStream;
			FileStream fs = default(FileStream);
			if (File.Exists(m_path))
			{
				FileInfo fi = new FileInfo(m_path);
				fs = fi.Open(FileMode.Open, FileAccess.Write);
			}
			else
			{
				fs = File.Create(m_path);
			}
			rs.m_fileStream = fs;
			int deltaLength = 0;
			int writedLength = 0;
			DateTime pauseTime = default(DateTime);
			int outTime = responseStream.ReadTimeout / 1000;
			m_bP = true;
			do
			{
				try
				{
					if (responseStream.CanRead)
					{
						deltaLength = responseStream.Read(rs.m_buffer, 0, RequestState.BUFFER_SIZE);
					}
					else
					{
						Release();
						m_b = false;
						m_state = E_State.Err;
						m_err = E_Error.NetErr;
						m_errCode = 406;
						m_errMessage = "Response stream can not read.";
						m_bE = true;
						return;
					}
				}
				catch (Exception e)
				{
					Release();
					m_b = false;
					m_state = E_State.Err;
					m_err = E_Error.NetErr;
					m_errCode = 407;
					m_errMessage = "Response stream read exception, " + e.ToString();
					m_bE = true;

					RO.LoggerUnused.LogWarning(e);

					return;
				}

				// stop
				if (!m_bStop)
				{
					return;
				}

				// pause
				if (!m_b)
				{
					pauseTime = DateTime.Now;
				}
				if (m_b && pauseTime != default(DateTime))
				{
					int deltaTime = (int)((DateTime.Now - pauseTime).TotalSeconds);
					if (deltaTime >= outTime)
					{
						Release();
						m_b = false;
						m_state = E_State.Err;
						m_err = E_Error.NetErr;
						m_errCode = 2;
						m_errMessage = "Response stream time out.";
						m_bE = true;
						return;
					}
					pauseTime = default(DateTime);
				}
				while(!m_b){}

				fs.Write(rs.m_buffer, 0, deltaLength);
				writedLength += deltaLength;
				m_progress = (float)writedLength / contentLength;
				m_bP = true;
			}
			while (deltaLength > 0);
			if (writedLength < contentLength)
			{
				Release();
				m_b = false;
				m_state = E_State.Err;
				m_err = E_Error.FileNotFull;
				m_errCode = 405;
				m_errMessage = "File isn't download fully.";
				m_bE = true;
				return;
			}
			Release();
			if (m_bCheckMD5)
			{
				if (!string.IsNullOrEmpty(m_md5FromServer))
				{
					if (string.Equals(MyMD5.HashFile(Path), m_md5FromServer))
					{
						m_state = E_State.End;
						m_progress = 1;
						m_bC = true;
					}
					else
					{
						m_b = false;
						m_state = E_State.Err;
						m_err = E_Error.MD5Inconsistent;
						m_errCode = 600;
						m_errMessage = "MD5 inconsistent.";
						m_bE = true;
						return;
					}
				}
				else
				{
					m_state = E_State.End;
					m_progress = 1;
					m_bC = true;
				}
			}
			else
			{
				m_state = E_State.End;
				m_progress = 1;
				m_bC = true;
			}
		}
		catch (Exception e)
		{
			Release();
			m_b = false;
			m_state = E_State.Err;
			m_err = E_Error.None;
			m_errCode = 500;
			m_errMessage = "Exception, " + e.ToString();
			m_bE = true;

			RO.LoggerUnused.LogWarning(e);
		}
	}

	public override void FireOnStart()
	{
		if (m_actionOnStart != null)
			m_actionOnStart();
	}

	public override void FireOnProgress()
	{
		if (m_actionOnProgress != null)
			m_actionOnProgress(m_progress);
	}

	public override void FireOnComplete()
	{
		if (m_actionOnComplete != null)
			m_actionOnComplete(m_path);
	}

	public override void FireOnError()
	{
		if (m_actionOnError != null)
			m_actionOnError((int)m_err, m_errCode, m_errMessage);
	}
}

public class HTTPFileUploader : HTTPFileLoader
{
	private enum E_Error
	{
		None,
		FileNoExists,
		NetErr,
		FileNotFull
	}

	private static int m_count;
	public static int Count
	{
		get {return m_count;}
	}

	private string m_path;
	// url
	private string m_fileURL;
	public string FileURL
	{
		get{return m_fileURL;}
	}

	private string m_boundary;
	private byte[] m_boundaryBytes;
	private string m_contentType;

	private bool m_b = true;
	private bool m_bStop = true;
	// state
	private E_Error m_err;
	private int m_errCode;
	private string m_errMessage;

	// progress
	private Action m_actionOnStart;
	private Action<float> m_actionOnProgress;
	private Action<string> m_actionOnComplete;
	private Action<int, int, string> m_actionOnError;

	public HTTPFileUploader(string path, string url, Dictionary<string, string> custom_headers, string content_type, Action action_on_start, Action<float> action_on_progress, Action<string> action_on_complete, Action<int, int, string> action_on_error)
	{
		m_id = ++m_count;
		m_path = Application.dataPath + "/" + path;
		m_url = url;
		m_customHeaders = custom_headers;
		m_contentType = content_type;
		m_state = E_State.None;
		m_err = E_Error.None;
		m_errCode = 0;
		m_errMessage = "";
		m_progress = 0;
		m_actionOnStart = action_on_start;
		m_actionOnProgress = action_on_progress;
		m_actionOnComplete = action_on_complete;
		m_actionOnError = action_on_error;
	}
	
	public override void Start()
	{
		bool isExist = File.Exists(m_path);
		if (!isExist)
		{
			m_state = E_State.None;
			m_err = E_Error.FileNoExists;
			m_errCode = 1;
			m_errMessage = "File not exists.";
			m_bE = true;
			return;
		}

		// separator symbol
		m_boundary = "------" + DateTime.Now.Ticks.ToString("x");
		m_boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + m_boundary + "\r\n");
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_url);
		request.ContentType = "multipart/form-data;boundary=" + m_boundary;
		request.Method = "POST";
		request.KeepAlive = true;
		request.Credentials = CredentialCache.DefaultCredentials;
		string md5Value = MyMD5.HashFile(m_path);
		request.Headers.Add("Content-MD5", md5Value);
		if (m_customHeaders != null) {
			foreach (KeyValuePair<string, string> kv in m_customHeaders) {
				request.Headers.Add (kv.Key, kv.Value);
			}
		}
		try
		{
			request.BeginGetRequestStream(new AsyncCallback(OnRequestStream), request);
		}
		catch (Exception e)
		{
			RO.LoggerUnused.LogWarning(e);
		}
		m_state = E_State.Ing;
		m_bS = true;
	}

	public override void Pause()
	{
		m_b = false;
	}

	public override void Continue()
	{
		m_b = true;
	}

	public override void Stop()
	{
		m_b = true;
		m_bStop = false;
	}

	private void OnRequestStream(IAsyncResult result)
	{
		try
		{
			HttpWebRequest request = (HttpWebRequest)result.AsyncState;

			Stream postStream = request.EndGetRequestStream(result);
			string formDataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict.Add("id", "TTR");
			dict.Add("btn-submit-photo", "Upload");
			foreach (KeyValuePair<string, string> pair in dict)
			{
				postStream.Write(m_boundaryBytes, 0, m_boundaryBytes.Length);
				string formItem = string.Format(formDataTemplate, pair.Key, pair.Value);
				byte[] formItemBytes = Encoding.UTF8.GetBytes(formItem);
				postStream.Write(formItemBytes, 0, formItemBytes.Length);
			}
			postStream.Write(m_boundaryBytes, 0, m_boundaryBytes.Length);
			string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
			string header = string.Format(headerTemplate, "file", m_path, m_contentType);
			byte[] headerBytes = Encoding.UTF8.GetBytes(header);
			postStream.Write(headerBytes, 0, headerBytes.Length);

			FileInfo fi = new FileInfo(m_path);
			int fileSize = (int)fi.Length;
			FileStream fs = fi.Open(FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[1024];
			int deltaSize = 0;
			int writedSize = 0;
			int outtime = postStream.WriteTimeout / 1000;
			DateTime pauseTime = default(DateTime);
			m_bP = true;
			while ((deltaSize = fs.Read(buffer, 0, buffer.Length)) > 0)
			{
				if (!m_bStop)
				{
					m_state = E_State.End;
					m_progress = 0;
					fs.Close();
					fs.Dispose();
					request.Abort();
					postStream.Flush();
					postStream.Close();
					postStream.Dispose();
					return;
				}
				if (!m_b)
				{
					pauseTime = DateTime.Now;
				}
				if (m_b)
				{
					if (pauseTime != default(DateTime))
					{
						int deltaTime = (int)((DateTime.Now - pauseTime).TotalSeconds);
						if (deltaTime >= outtime)
						{
							m_state = E_State.Err;
							m_err = E_Error.NetErr;
							m_errCode = 11;
							m_errMessage = "Post stream time out.";
							m_bE = true;
							fs.Flush();
							fs.Close();
							fs.Dispose();
							request.Abort();
							postStream.Flush();
							postStream.Close();
							postStream.Dispose();
							return;
						}
					}
					pauseTime = default(DateTime);
				}
				while (!m_b){}
				postStream.Write(buffer, 0, deltaSize);
				writedSize += deltaSize;
				m_progress = (float)writedSize / fileSize;
				m_bP = true;
			}
			if (writedSize < fileSize)
			{
				m_state = E_State.Err;
				m_err = E_Error.FileNotFull;
				m_errCode = 12;
				m_errMessage = "File isn't uploaded fully.";
				m_bE = true;
				fs.Flush();
				fs.Close();
				fs.Dispose();
				request.Abort();
				postStream.Flush();
				postStream.Close();
				postStream.Dispose();
				return;
			}
			fs.Flush();
			fs.Close();
			fs.Dispose();
			byte[] trailerBytes = Encoding.ASCII.GetBytes("\r\n--" + m_boundary + "--\r\n");
			postStream.Write(trailerBytes, 0, trailerBytes.Length);
			postStream.Flush();
			postStream.Close();
			postStream.Dispose();
			request.BeginGetResponse(new AsyncCallback(OnResponse), request);
		}
		catch (Exception e)
		{
			RO.LoggerUnused.LogWarning(e);
		}
	}

	private void OnResponse(IAsyncResult result)
	{
		try
		{
			HttpWebRequest request = (HttpWebRequest)result.AsyncState;
			HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
			HttpStatusCode statusCode = response.StatusCode;
			if (statusCode != HttpStatusCode.OK)
			{
				m_b = false;
				m_state = E_State.Err;
				m_err = E_Error.NetErr;
				m_errCode = (int)statusCode;
				m_errMessage = response.StatusDescription;
				m_bE = true;
				request.Abort();
				response.Close();
				return;
			}
			Stream stream = response.GetResponseStream();
			StreamReader streamReader = new StreamReader(stream);
			string strResponse = streamReader.ReadToEnd();
			//Logger.Log("Response string is \"" + strResponse + "\"");
			m_fileURL = strResponse;
			m_state = E_State.End;
			m_progress = 1;
			m_bC = true;
			streamReader.Close();
			request.Abort();
			stream.Flush();
			stream.Close();
			stream.Dispose();
			response.Close();
		}
		catch(Exception e)
		{
			RO.LoggerUnused.LogWarning(e);
		}
	}

	public override void FireOnStart()
	{
		if (m_actionOnStart != null)
			m_actionOnStart();
	}

	public override void FireOnProgress()
	{
		if (m_actionOnProgress != null)
			m_actionOnProgress(m_progress);
	}

	public override void FireOnComplete()
	{
		if (m_actionOnComplete != null)
			m_actionOnComplete(m_fileURL);
	}

	public override void FireOnError()
	{
		if (m_actionOnError != null)
			m_actionOnError((int)m_err, m_errCode, m_errMessage);
	}
}