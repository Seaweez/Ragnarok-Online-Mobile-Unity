using System.Net;

public class DUWebClient : WebClient
{
	WebHeaderCollection m_headers;
	string m_method;
	string m_md5FromServer;
	long m_contentSize;
	HttpWebRequest _request;

	public HttpWebRequest request {
		get {
			return _request;
		}
	}

	public string Md5FromServer {
		get {
			return m_md5FromServer;
		}
	}

	protected override WebRequest GetWebRequest (System.Uri address)
	{
		_request = (HttpWebRequest)base.GetWebRequest (address);
		_request.Proxy = null;
		_request.AllowWriteStreamBuffering = false;
		if (m_contentSize != -1) {
			request.ContentLength = m_contentSize;
//			UnityEngine.Debug.LogFormat ("{0} {1}", m_contentSize, _request.ContentLength);
		}
		return _request;
	}

	public DUWebClient (WebHeaderCollection header, bool useProxy, long contentSize = -1)
	{
		m_headers = header;
		if (m_headers != null) {
			Headers = header;
		}
		m_contentSize = contentSize;
	}

	protected override WebResponse GetWebResponse (WebRequest request, System.IAsyncResult result)
	{
		WebResponse response = base.GetWebResponse (request, result);
		m_md5FromServer = response.Headers ["ETag"];
		if (!string.IsNullOrEmpty (m_md5FromServer)) {
			m_md5FromServer = m_md5FromServer.Replace ("\"", "");
		}
		return response;
	}

	protected override void Dispose (bool release_all)
	{
		_request = null;
		base.Dispose (release_all);
	}
}