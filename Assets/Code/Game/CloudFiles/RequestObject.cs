using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;

namespace CloudFile
{
	public class RequestObject
	{
		private int m_id;
		public int ID
		{
			get
			{
				return m_id;
			}
			set
			{
				m_id = value;
			}
		}

		private HttpWebRequest m_request;
		public HttpWebRequest Request
		{
			get
			{
				return m_request;
			}
			set
			{
				m_request = value;
			}
		}

		private HttpWebResponse m_response;
		public HttpWebResponse Response
		{
			get
			{
				return m_response;
			}
			set
			{
				m_response = value;
			}
		}
		public long ContentLength
		{
			get
			{
				return Response.ContentLength;
			}
		}

		private Stream m_responseStream;
		public Stream ResponseStream
		{
			get
			{
				return m_responseStream;
			}
			set
			{
				m_responseStream = value;
			}
		}

		private byte[] m_buffer;
		public byte[] Buffer
		{
			get
			{
				return m_buffer;
			}
			set
			{
				m_buffer = value;
			}
		}

		public RequestObject(int id)
		{
			m_id = id;
			m_buffer = new byte[4096];
		}

		public virtual void Release()
		{
			if (m_request != null)
			{
				m_request.Abort();
				m_request = null;
			}
			if (m_response != null)
			{
				m_response.Close();
				m_response = null;
			}
			if (m_responseStream != null)
			{
				m_responseStream.Close();
				m_responseStream = null;
			}
			m_buffer = null;
		}
	}
	
	public class RequestObjectForDownload:RequestObject
	{
		public RequestObjectForDownload(int id) : base(id)
		{

		}
	}

	public class RequestObjectForUpload:RequestObject
	{
		public RequestObjectForUpload(int id) : base(id)
		{
			
		}

		private Stream m_requestStream;
		public Stream RequestStream
		{
			get
			{
				return m_requestStream;
			}
			set
			{
				m_requestStream = value;
			}
		}

		public override void Release()
		{
			base.Release();

			if (m_requestStream != null)
			{
				m_requestStream.Close();
				m_requestStream = null;
			}
		}
	}

	public class RequestObjectForFormUpload:RequestObject
	{
		public RequestObjectForFormUpload(int id) : base(id)
		{

		}

		private Stream m_requestStream;
		public Stream RequestStream
		{
			get
			{
				return m_requestStream;
			}
			set
			{
				m_requestStream = value;
			}
		}

		public override void Release()
		{
			base.Release();

			if (m_requestStream != null)
			{
				m_requestStream.Close();
				m_requestStream = null;
			}
		}
	}

	public class RequestObjectForBlocksUpload : RequestObjectForUpload
	{
		public RequestObjectForBlocksUpload(int id) : base(id)
		{

		}

		private HttpWebRequest m_requestForInitialize;
		public HttpWebRequest RequestForInitialize
		{
			get
			{
				return m_requestForInitialize;
			}
			set
			{
				m_requestForInitialize = value;
			}
		}
		private Stream m_requestStreamForInitialize;
		public Stream RequestStreamForInitialize
		{
			get
			{
				return m_requestStreamForInitialize;
			}
			set
			{
				m_requestStreamForInitialize = value;
			}
		}
		private HttpWebResponse m_responseForInitialize;
		public HttpWebResponse ResponseForInitialize
		{
			get
			{
				return m_responseForInitialize;
			}
			set
			{
				m_responseForInitialize = value;
			}
		}
		private Stream m_responseStreamForInitialize;
		public Stream ResponseStreamForInitialize
		{
			get
			{
				return m_responseStreamForInitialize;
			}
			set
			{
				m_responseStreamForInitialize = value;
			}
		}

		private HttpWebRequest m_requestForUploadBlockData;
		public HttpWebRequest RequestForUploadBlockData
		{
			get
			{
				return m_requestForUploadBlockData;
			}
			set
			{
				if (m_requestForUploadBlockData != null)
				{
					m_requestForUploadBlockData.Abort();
				}
				m_requestForUploadBlockData = value;
			}
		}
		private Stream m_requestStreamForUploadBlockData;
		public Stream RequestStreamForUploadBlockData
		{
			get
			{
				return m_requestStreamForUploadBlockData;
			}
			set
			{
				if (m_requestStreamForUploadBlockData != null)
				{
					m_requestStreamForUploadBlockData.Close();
				}
				m_requestStreamForUploadBlockData = value;
			}
		}
		private HttpWebResponse m_responseForUploadBlockData;
		public HttpWebResponse ResponseForUploadBlockData
		{
			get
			{
				return m_responseForUploadBlockData;
			}
			set
			{
				if (m_responseForUploadBlockData != null)
				{
					m_responseForUploadBlockData.Close();
				}
				m_responseForUploadBlockData = value;
			}
		}
		private Stream m_responseStreamForUploadBlockData;
		public Stream ResponseStreamForUploadBlockData
		{
			get
			{
				return m_responseStreamForUploadBlockData;
			}
			set
			{
				if (m_responseStreamForUploadBlockData != null)
				{
					m_responseStreamForUploadBlockData.Close();
				}
				m_responseStreamForUploadBlockData = value;
			}
		}

		public override void Release()
		{
			base.Release();

			if (RequestForInitialize != null)
			{
				RequestForInitialize.Abort();
			}
			if (RequestStreamForInitialize != null)
			{
				RequestStreamForInitialize.Close();
			}
			if (ResponseForInitialize != null)
			{
				ResponseForInitialize.Close();
			}
			if (ResponseStreamForInitialize != null)
			{
				ResponseStreamForInitialize.Close();
			}

			if (RequestForUploadBlockData != null)
			{
				RequestForUploadBlockData.Abort();
			}
			if (RequestStreamForUploadBlockData != null)
			{
				RequestStreamForUploadBlockData.Close();
			}
			if (ResponseForUploadBlockData != null)
			{
				ResponseForUploadBlockData.Close();
			}
			if (ResponseStreamForUploadBlockData != null)
			{
				ResponseStreamForUploadBlockData.Close();
			}
		}
	}
}