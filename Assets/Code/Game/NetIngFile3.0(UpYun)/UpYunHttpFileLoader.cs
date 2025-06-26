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

public class UpYunHTTPFileLoader
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
	protected string m_path;
	public string Path
	{
		get{return m_path;}
	}
	protected string m_absolutePath;
	public string AbsolutePath
	{
		get{return m_absolutePath;}
	}

	public bool m_bS;
	public bool m_bP;
	public bool m_bC;
	public bool m_bE;

	public virtual void Initialize()
	{

	}
	
	public virtual void Start()
	{
		
	}

	public virtual void Restart()
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

public class UpYunHTTPFileDownloader : UpYunHTTPFileLoader
{	
	private enum E_Error
	{
		None,
		NetErr,
		FileNotFull
	}
	
	private class RequestState
	{
		public const int BUFFER_SIZE = 1024;
		public byte[] m_buffer;
		public HttpWebRequest m_request;
		public HttpWebResponse m_response;
		public Stream m_responseStream;
		
		public RequestState()
		{
			m_buffer = new byte[BUFFER_SIZE];
		}
	}
	
	private static int m_count;
	public static int Count
	{
		get{return m_count;}
	}
	
	// url
	private string m_fileName;
	private bool m_b = true;
	private bool m_bStop = true;
	// state
	// progress
	private E_Error m_err;
	private int m_errCode;
	private string m_errMessage;
	private Action m_actionOnStart;
	private Action<float> m_actionOnProgress;
	private Action m_actionOnComplete;
	private Action<int, int, string> m_actionOnError;

	public UpYunHTTPFileDownloader(string server_path, string local_path, Dictionary<string, string> custum_headers, Action action_on_start, Action<float> action_on_progress, Action action_on_complete, Action<int, int, string> action_on_error)
	{
		m_id = ++m_count;
		m_url = CloudServer.VISIT_DOMAIN + server_path;
		m_customHeaders = custum_headers;
		string[] urlSplitBySlash = m_url.Split('/');
		if (urlSplitBySlash != null && urlSplitBySlash.Length > 0)
			m_fileName = urlSplitBySlash[urlSplitBySlash.Length - 1];
		if (string.IsNullOrEmpty(local_path))
			m_path = m_fileName;
		else
			m_path = local_path;
		m_absolutePath = Application.persistentDataPath + "/" + m_path;
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
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_url);
		request.Headers.Add(CloudServer.AUTH_KEY, CloudServer.AUTH_VALUE);
		if (m_customHeaders != null) {
			foreach(KeyValuePair<string, string> kv in m_customHeaders)
			{
				request.Headers.Add (kv.Key);
				request.Headers.Add (kv.Value);
			}
		}
		RequestState rs = new RequestState();
		rs.m_request = request;
		request.BeginGetResponse(new AsyncCallback(OnResponse), rs);
		m_state = E_State.Ing;
		m_bS = true;
	}

	public override void Restart()
	{
		if (m_state == E_State.Ing || m_state == E_State.Pause) return;
		m_b = true;
		m_bStop = true;
		Start();
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
	
	private void OnResponse(IAsyncResult pResult)
	{
		try
		{
			RequestState rs = (RequestState)pResult.AsyncState;
			HttpWebRequest request = rs.m_request;
//			if (string.IsNullOrEmpty(request.Connection))
//			{
//				m_b = false;
//				m_state = E_State.Err;
//				m_err = E_Error.NetErr;
//				m_errCode = 404;
//				m_errMessage = "Connection is empty.";
//				m_bE = true;
//				request.Abort();
//				return;
//			}
			if (!request.HaveResponse)
			{
				m_b = false;
				m_state = E_State.Err;
				m_err = E_Error.NetErr;
				m_errCode = 404;
				m_errMessage = "No response.";
				m_bE = true;
				request.Abort();
				return;
			}
			HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(pResult);
			rs.m_response = response;
			HttpStatusCode statusCode = response.StatusCode;
			if (statusCode != HttpStatusCode.OK)
			{
				m_b = false;
				m_state = E_State.Err;
				m_err = E_Error.NetErr;
				m_errCode = (int)statusCode;
				m_errMessage = response.StatusDescription;
				m_bE = true;
				return;
			}
			int contentLength = (int)response.ContentLength;
			Stream responseStream = response.GetResponseStream();
			rs.m_responseStream = responseStream;
			FileStream fs = default(FileStream);
			if (File.Exists(m_absolutePath))
			{
				FileInfo fi = new FileInfo(m_absolutePath);
				fs = fi.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
			}
			else
			{
				fs = File.Create(m_absolutePath);
			}
			int deltaLength = 0;
			int writedLength = 0;
			DateTime pauseTime = default(DateTime);
			int outTime = responseStream.ReadTimeout / 1000;
			m_bP = true;
			while ((deltaLength = responseStream.Read(rs.m_buffer, 0, RequestState.BUFFER_SIZE)) > 0)
			{
				// stop
				if (!m_bStop)
				{
					m_state = E_State.End;
					m_progress = 0;
					fs.Close();
					if (File.Exists(m_absolutePath))
					{
						File.Delete(m_absolutePath);
					}
					responseStream.Close();
					response.Close();
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
						m_b = false;
						m_state = E_State.Err;
						m_err = E_Error.NetErr;
						m_errCode = 2;
						m_errMessage = "Response stream time out.";
						m_bE = true;
						fs.Close();
						responseStream.Close();
						response.Close();
						return;
					}
					pauseTime = default(DateTime);
				}
				while(!m_b){}
				
				fs.Write(rs.m_buffer, 0, deltaLength);
				writedLength += deltaLength;
				m_progress = (float)writedLength / contentLength;
				if (m_progress >= 1)
				{
					break;
				}
				else
				{
					m_bP = true;
				}
			}
			if (writedLength < contentLength)
			{
				m_b = false;
				m_state = E_State.Err;
				m_err = E_Error.FileNotFull;
				m_errCode = 405;
				m_errMessage = "File isn't download fully.";
				m_bE = true;
				fs.Close();
				responseStream.Close();
				response.Close();
				return;
			}
			fs.Close();
			responseStream.Close();
			response.Close();
			m_state = E_State.End;
			m_progress = 1;
			m_bC = true;
		}
		catch (Exception e)
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
			m_actionOnComplete();
	}
	
	public override void FireOnError()
	{
		if (m_actionOnError != null)
			m_actionOnError((int)m_err, m_errCode, m_errMessage);
	}
}

public class UpYunHTTPFileUploader : UpYunHTTPFileLoader
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

	
	private bool m_b = true;
	private bool m_bStop = true;
	// state
	private E_Error m_err;
	private int m_errCode;
	private string m_errMessage;
	
	// progress
	private Action m_actionOnStart;
	private Action<float> m_actionOnProgress;
	private Action m_actionOnComplete;
	private Action<int, int, string> m_actionOnError;

	public UpYunHTTPFileUploader(string path, string url, Dictionary<string, string> custom_headers, Action action_on_start, Action<float> action_on_progress, Action action_on_complete, Action<int, int, string> action_on_error)
	{
		m_id = ++m_count;
		m_path = path;
		m_absolutePath = Application.persistentDataPath + "/" + m_path;
		m_url = url;
		m_customHeaders = custom_headers;

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
		bool isExist = File.Exists(m_absolutePath);
		if (!isExist)
		{
			m_state = E_State.None;
			m_err = E_Error.FileNoExists;
			m_errCode = 1;
			m_errMessage = "File not exists.";
			m_bE = true;
			return;
		}
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_url);
		request.Headers.Add(CloudServer.AUTH_KEY, CloudServer.AUTH_VALUE);
		if (m_customHeaders != null) {
			foreach (KeyValuePair<string, string> kv in m_customHeaders) {
				request.Headers.Add (kv.Key, kv.Value);
			}
		}
		request.Method = "POST";
		FileInfo fi = new FileInfo(m_absolutePath);
		request.ContentLength = fi.Length;
		request.KeepAlive = true;
		string md5Value = MyMD5.HashFile(m_absolutePath);
		request.Headers.Add("Content-MD5", md5Value);
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

	public override void Restart()
	{
		if (m_state == E_State.Ing || m_state == E_State.Pause) return;
		m_b = true;
		m_bStop = true;
		Start();
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
			FileInfo fi = new FileInfo(m_absolutePath);
			int fileSize = (int)fi.Length;
			FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			byte[] buffer = new byte[1024];
			int deltaSize = 0;
			int writedSize = 0;
			int outtime = postStream.WriteTimeout / 1000;
			DateTime pauseTime = default(DateTime);
			m_bP = true;
			while ((deltaSize = fs.Read(buffer, 0, buffer.Length)) > 0)
			{
				// stop
				if (!m_bStop)
				{
					m_state = E_State.End;
					m_progress = 0;
					fs.Close();
					postStream.Close();
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
					if (deltaTime >= outtime)
					{
						m_state = E_State.Err;
						m_err = E_Error.NetErr;
						m_errCode = 11;
						m_errMessage = "Post stream time out.";
						m_bE = true;
						fs.Close();
						postStream.Close();
						return;
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
				fs.Close();
				postStream.Close();
				return;
			}
			fs.Close();
			postStream.Close();
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
				m_state = E_State.Err;
				m_err = E_Error.NetErr;
				m_errCode = (int)statusCode;
				m_errMessage = response.StatusDescription;
				m_bE = true;
				response.Close();
				return;
			}
			Stream stream = response.GetResponseStream();
			StreamReader streamReader = new StreamReader(stream);
			streamReader.ReadToEnd();
			m_state = E_State.End;
			m_progress = 1;
			m_bC = true;
			streamReader.Close();
			stream.Close();
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
			m_actionOnComplete();
	}
	
	public override void FireOnError()
	{
		if (m_actionOnError != null)
			m_actionOnError((int)m_err, m_errCode, m_errMessage);
	}
}

public class UpYunHTTPFileBlocksUploader : UpYunHTTPFileLoader
{
	public class InitializeRequestState
	{
		public HttpWebRequest m_request;
		public byte[] m_bytes;
	}

	public class MergeBlocksRequestState
	{
		public HttpWebRequest m_request;
		public byte[] m_bytes;
	}

	private enum E_Error
	{
		None,
		FileNoExists,
		NetErr,
		FileNotFull,
		CodeException
	}

	private static int m_count;
	public static int Count
	{
		get {return m_count;}
	}
	
	private string m_fileName;
	private MyFile m_myFile;
	private byte[][] m_blocks;
	private string m_md5;
	
	private bool m_b = true;
	private bool m_bStop = true;
	// state
	private E_Error m_err;
	private int m_errCode;
	private string m_errMessage;
	
	// progress
	private Action m_actionOnStart;
	private Action<float> m_actionOnProgress;
	private Action m_actionOnComplete;
	private Action<int, int, string> m_actionOnError;

	// request params
	private string m_signature;
	private string m_policy;
	private string m_boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
	private string m_cachedTokenSecret;
	private string m_cachedSaveToken;
	private string m_saveTokenFromInitializeSuccess;
	private int m_blockIndex;
	private int m_currentStep;

	private DateTime m_pauseTime;

	public UpYunHTTPFileBlocksUploader(string path, Dictionary<string, string> custom_headers, string signature, string policy, Action action_on_start, Action<float> action_on_progress, Action action_on_complete, Action<int, int, string> action_on_error)
	{
		m_id = ++m_count;
		m_path = path;
		m_absolutePath = Application.persistentDataPath + "/" + m_path;
		m_url = CloudServer.UPLOAD_BLOCKS_DOMAIN;
		m_customHeaders = custom_headers;
		m_signature = signature;
		m_policy = policy;

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

	public override void Initialize()
	{
		bool isExist = File.Exists(m_absolutePath);
		if (!isExist)
		{
			m_state = E_State.Err;
			m_err = E_Error.FileNoExists;
			m_errCode = 1;
			m_errMessage = "File not exists.";
			m_bE = true;
			return;
		}

		string[] pathSplitBySlash = m_path.Split('/');
		if (pathSplitBySlash != null && pathSplitBySlash.Length > 0)
			m_fileName = pathSplitBySlash[pathSplitBySlash.Length - 1];
		m_myFile = MyFileFactory.Ins.GetMyFile(m_absolutePath);
		m_blocks = m_myFile.GetBlocks();
		m_md5 = MyMD5.HashFile(m_absolutePath);

		m_pauseTime = default(DateTime);

		m_currentStep = 1;
		m_blockIndex = 0;
	}

	public override void Start()
	{
		m_state = E_State.Ing;
		m_bS = true;
		DoStepByStep();
	}

	public override void Restart()
	{
		if (m_state == E_State.Ing || m_state == E_State.Pause) return;
		m_b = true;
		m_bStop = true;
		Start();
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

	private void DoStepByStep()
	{
		if (!m_bStop) return;
		if (m_currentStep == 1)
		{
			InitializeUpload();
		}
		else if (m_currentStep == 2)
		{
			m_bP = true;
			UploadBlock();
		}
		else if (m_currentStep == 3)
		{
			m_progress = 1;
			m_bP = true;
			MergeBlocks();
		}
	}

	public void InitializeUpload()
	{
		int expiration = GetExpiration(80 * 60);
		Dictionary<string, object> dictParams = new Dictionary<string, object>();
		dictParams.Add("expiration", expiration);
		dictParams.Add("file_blocks", m_myFile.BlockCount);
		dictParams.Add("file_hash", m_md5);
		dictParams.Add("file_size", m_myFile.Size);
		dictParams.Add("path", "/game/scenery/11/user/47244647439/4.png");
		string signature = m_signature; //Signature.CreateWithFormAPIValue(dictParams, "BHsE9P9E5gr3Daf0WQTc58nshL4=");
		string policy = m_policy; //Signature.CreatePolicy(dictParams);

		string strPAndS = "policy=" + policy + "&signature=" + signature;
		byte[] bytesPAndS = System.Text.Encoding.UTF8.GetBytes(strPAndS);

		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_url);
		request.Headers.Add(CloudServer.AUTH_KEY, CloudServer.AUTH_VALUE);
		if (m_customHeaders != null) {
			foreach (KeyValuePair<string, string> kv in m_customHeaders) {
				request.Headers.Add (kv.Key, kv.Value);
			}
		}
		request.Method = "POST";
		request.KeepAlive = true;
		request.Credentials = CredentialCache.DefaultCredentials;
		request.ContentType = "application/x-www-form-urlencoded";
		request.ContentLength = bytesPAndS.Length;
		try
		{
			InitializeRequestState requestState = new InitializeRequestState() {m_request = request, m_bytes = bytesPAndS};
			request.BeginGetRequestStream(new AsyncCallback(OnInitializeRequestStream), requestState);
		}
		catch (Exception e)
		{
			RO.LoggerUnused.LogWarning(e);
		}
		m_state = E_State.Ing;
	}

	private void OnInitializeRequestStream(IAsyncResult result)
	{
		if (!m_bStop) return;
		try
		{
			InitializeRequestState requestState = (InitializeRequestState)result.AsyncState;
			HttpWebRequest request = requestState.m_request;
			byte[] bytes = requestState.m_bytes;

			Stream postStream = request.EndGetRequestStream(result);
			postStream.Write(bytes, 0, bytes.Length);
			postStream.Close();
			request.BeginGetResponse(new AsyncCallback(OnInitializeResponse), request);
		}
		catch (Exception e)
		{
			RO.LoggerUnused.LogWarning(e);
		}
	}

	private void OnInitializeResponse(IAsyncResult result)
	{
		if (!m_bStop) return;
		HttpWebResponse response = null;
		Stream stream = null;
		StreamReader streamReader = null;
		try
		{
			HttpWebRequest request = (HttpWebRequest)result.AsyncState;
			response = (HttpWebResponse)request.EndGetResponse(result);
			HttpStatusCode statusCode = response.StatusCode;
			if (statusCode != HttpStatusCode.OK)
			{
				m_state = E_State.Err;
				m_err = E_Error.NetErr;
				m_errCode = (int)statusCode;
				m_errMessage = response.StatusDescription;
				m_bE = true;
				response.Close();
				return;
			}
			stream = response.GetResponseStream();
			streamReader = new StreamReader(stream);
			string strResponse = streamReader.ReadToEnd();
			Dictionary<string, object> dict = Spine.Json.Deserialize(new StringReader(strResponse)) as Dictionary<string, object>;
			string tokenSecret = dict["token_secret"].ToString();
			string saveToken = dict["save_token"].ToString();
			m_saveTokenFromInitializeSuccess = saveToken;
			m_cachedTokenSecret = tokenSecret;
			m_cachedSaveToken = saveToken;
//			NetIngFileTaskRecord.ins.DynamicDataComing(m_path, "stfis", m_saveTokenFromInitializeSuccess);
//			NetIngFileTaskRecord.ins.DynamicDataComing(m_path, "ts", m_cachedTokenSecret);
//			NetIngFileTaskRecord.ins.DynamicDataComing(m_path, "st", m_cachedSaveToken);

			streamReader.Close();
			stream.Close();
			response.Close();

			m_currentStep = 2;
//			NetIngFileTaskRecord.ins.DynamicDataComing(m_path, "cs", m_currentStep);
			DoStepByStep();
		}
		catch(Exception e)
		{
			RO.LoggerUnused.LogWarning(e);
			m_state = E_State.Err;
			m_err = E_Error.CodeException;
			m_bE = true;
			if (response != null) response.Close();
			if (stream != null) stream.Close();
			if (streamReader != null) streamReader.Close();
		}
	}

	public void UploadBlock()
	{
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_url);
		request.Method = "POST";
		request.KeepAlive = true;
		request.Credentials = CredentialCache.DefaultCredentials;
		request.ContentType = "multipart/form-data; boundary=" + m_boundary;
		request.Headers.Add(CloudServer.AUTH_KEY, CloudServer.AUTH_VALUE);
		request.BeginGetRequestStream(new AsyncCallback(OnUploadBlockRequestStream), request);
	}

	private void OnUploadBlockRequestStream(IAsyncResult result)
	{
		if (!m_bStop) return;
		try
		{
			HttpWebRequest request = (HttpWebRequest)result.AsyncState;
			Stream postStream = request.EndGetRequestStream(result);
			byte[] blockBytes = m_blocks[m_blockIndex];

			if (blockBytes != null && blockBytes.Length > 0)
			{
				string strBlockHash = MyMD5.HashBytes(blockBytes);

				Dictionary<string, object> dictParams = new Dictionary<string, object>();
				dictParams.Add("block_hash", strBlockHash);
				dictParams.Add("block_index", m_blockIndex);
				dictParams.Add("expiration", GetExpiration(80 * 60));
				dictParams.Add("save_token", m_cachedSaveToken);
				string signature = Signature.CreateWithToken(dictParams, m_cachedTokenSecret);
				string policy = Signature.CreatePolicy(dictParams);
				Dictionary<string, object> dict = new Dictionary<string, object>();
				dict.Add("policy", policy);
				dict.Add("signature", signature);
				byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + m_boundary + "\r\n");
				string formDataTemplate = "Content-Disposition:form-data;name=\"{0}\"\r\n\r\n{1}";
				foreach (KeyValuePair<string, object> pair in dict)
				{
					postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
					string formItem = string.Format(formDataTemplate, pair.Key, pair.Value);
					byte[] formItemBytes = Encoding.UTF8.GetBytes(formItem);
					postStream.Write(formItemBytes, 0, formItemBytes.Length);
				}
				
				postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
				string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
				string header = string.Format(headerTemplate, "file", m_fileName, "xxx");
				byte[] headerBytes = Encoding.UTF8.GetBytes(header);
				postStream.Write(headerBytes, 0, headerBytes.Length);

				postStream.Write(blockBytes, 0, blockBytes.Length);
				
				byte[] trailerBytes = Encoding.ASCII.GetBytes("\r\n--" + m_boundary + "--\r\n");
				postStream.Write(trailerBytes, 0, trailerBytes.Length);

				request.BeginGetResponse(OnUploadBlockResponse, request);
			}
			postStream.Close();
		}
		catch (Exception e)
		{
			RO.LoggerUnused.LogWarning(e);
		}
	}

	private void OnUploadBlockResponse(IAsyncResult result)
	{
		if (!m_bStop) return;
		try
		{
			HttpWebRequest request = (HttpWebRequest)result.AsyncState;
			HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
			HttpStatusCode statusCode = response.StatusCode;
			if (statusCode != HttpStatusCode.OK)
			{
				m_state = E_State.Err;
				m_err = E_Error.NetErr;
				m_errCode = (int)statusCode;
				m_errMessage = response.StatusDescription;
				m_bE = true;
				response.Close();
				return;
			}
			Stream stream = response.GetResponseStream();
			StreamReader streamReader = new StreamReader(stream);
			string strResponse = streamReader.ReadToEnd();
			Dictionary<string, object> dict = Spine.Json.Deserialize(new StringReader(strResponse)) as Dictionary<string, object>;
			string tokenSecret = dict["token_secret"].ToString();
			string saveToken = dict["save_token"].ToString();
			m_cachedTokenSecret = tokenSecret;
			m_cachedSaveToken = saveToken;
//			NetIngFileTaskRecord.ins.DynamicDataComing(m_path, "ts", m_cachedTokenSecret);
//			NetIngFileTaskRecord.ins.DynamicDataComing(m_path, "st", m_cachedSaveToken);

			streamReader.Close();
			stream.Close();
			response.Close();

			m_progress = (float)(m_blockIndex + 1) / m_myFile.BlockCount;

			if (m_blockIndex == m_myFile.BlockCount - 1)
			{
				m_currentStep = 3;
			}
			else
			{
				m_blockIndex++;
			}
//			NetIngFileTaskRecord.ins.DynamicDataComing(m_path, "cs", m_currentStep);
//			NetIngFileTaskRecord.ins.DynamicDataComing(m_path, "bi", m_blockIndex);

			if (!m_bStop) return;
			if (!m_b)
			{
				m_pauseTime = DateTime.Now;
			}
			while (!m_b) {}
			if (m_b && m_pauseTime != default(DateTime))
			{
				int deltaTime = (int)DateTime.Now.Subtract(m_pauseTime).TotalSeconds;
				if (deltaTime > 80 * 60)
				{
					m_state = E_State.Err;
					m_err = E_Error.NetErr;
					m_errCode = 11;
					m_errMessage = "Time out.";
					m_bE = true;
					return;
				}
			}

			DoStepByStep();
		}
		catch(Exception e)
		{
			RO.LoggerUnused.LogWarning(e);
		}
	}

	public void MergeBlocks()
	{
		int expiration = GetExpiration(80 * 60);

		Dictionary<string, object> dictParams = new Dictionary<string, object>();
		dictParams.Add("expiration", expiration);
		dictParams.Add("save_token", m_saveTokenFromInitializeSuccess);
		string signature = Signature.CreateWithFormAPIValue(dictParams, m_cachedTokenSecret);
		string policy = Signature.CreatePolicy(dictParams);
		string strPAndS = "policy=" + policy + "&signature=" + signature;
		byte[] bytesPAndS = System.Text.Encoding.UTF8.GetBytes(strPAndS);

		HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(m_url);
		request.Method = "POST";
		request.KeepAlive = true;
		request.Credentials = CredentialCache.DefaultCredentials;
		request.ContentType = "application/x-www-form-urlencoded";
		request.ContentLength = bytesPAndS.Length;
		request.Headers.Add(CloudServer.AUTH_KEY, CloudServer.AUTH_VALUE);
		MergeBlocksRequestState requestState = new MergeBlocksRequestState(){m_request = request, m_bytes = bytesPAndS};
		request.BeginGetRequestStream(OnMergeBlocksRequestStream, requestState);
	}

	private void OnMergeBlocksRequestStream(IAsyncResult result)
	{
		if (!m_bStop) return;
		try
		{
			MergeBlocksRequestState requestState = (MergeBlocksRequestState)result.AsyncState;
			HttpWebRequest request = requestState.m_request;
			byte[] bytes = requestState.m_bytes;
			
			Stream postStream = request.EndGetRequestStream(result);
			postStream.Write(bytes, 0, bytes.Length);
			postStream.Close();
			request.BeginGetResponse(new AsyncCallback(OnMergeBlocksResponse), request);
		}
		catch (Exception e)
		{
			RO.LoggerUnused.LogWarning(e);
		}
	}
		
	private void OnMergeBlocksResponse(IAsyncResult result)
	{
		if (!m_bStop) return;
		HttpWebResponse response = null;
		Stream stream = null;
		try
		{
			HttpWebRequest request = (HttpWebRequest)result.AsyncState;
			response = (HttpWebResponse)request.EndGetResponse(result);
			HttpStatusCode statusCode = response.StatusCode;
			if (statusCode != HttpStatusCode.OK)
			{
				m_state = E_State.Err;
				m_err = E_Error.NetErr;
				m_errCode = (int)statusCode;
				m_errMessage = response.StatusDescription;
				m_bE = true;
				response.Close();
				return;
			}
			stream = response.GetResponseStream();
			StreamReader streamReader = new StreamReader(stream);
			streamReader.ReadToEnd();
			
			m_state = E_State.End;
			m_progress = 1;
			m_bC = true;
			streamReader.Close();
			stream.Close();
			response.Close();
		}
		catch(Exception e)
		{
			RO.LoggerUnused.LogWarning(e);
			if (response != null)
			{
				response.Close();
			}
			if (stream != null)
			{
				response.Close();
			}
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
			m_actionOnComplete();
	}
	
	public override void FireOnError()
	{
		if (m_actionOnError != null)
			m_actionOnError((int)m_err, m_errCode, m_errMessage);
	}

	private int GetExpiration(int seconds)
	{
		DateTime dtGreenwich = new DateTime(1970, 1, 1, 0, 0, 0);
		DateTime dtNow = DateTime.UtcNow;
		TimeSpan ts = dtNow.Subtract(dtGreenwich);
		int expiration = (int)(ts.TotalSeconds + seconds);
		return expiration;
	}
}