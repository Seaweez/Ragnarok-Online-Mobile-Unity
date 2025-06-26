using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudFile
{
	[SLua.CustomLuaClassAttribute]
	public class CloudFileCallback
	{
		public delegate void ProgressCallback(float progress);
		public delegate void SuccessCallback();
		public delegate void ErrorCallback(string error_message);

		private ProgressCallback m_progressCallback;
		public ProgressCallback _ProgressCallback
		{
			get
			{
				return m_progressCallback;
			}
			set
			{
				m_progressCallback = value;
			}
		}
		private SuccessCallback m_successCallback;
		public SuccessCallback _SuccessCallback
		{
			get
			{
				return m_successCallback;
			}
			set
			{
				m_successCallback = value;
			}
		}
		private ErrorCallback m_errorCallback;
		public ErrorCallback _ErrorCallback
		{
			get
			{
				return m_errorCallback;
			}
			set
			{
				m_errorCallback = value;
			}
		}

		public void FireProgress(float progress)
		{
			if (_ProgressCallback != null)
			{
				_ProgressCallback(progress);
			}
		}

		public void FireSuccess()
		{
			if (_SuccessCallback != null)
			{
				_SuccessCallback();
			}
		}

		public void FireError(string error_message)
		{
			if (_ErrorCallback != null)
			{
				_ErrorCallback(error_message);
			}
		}
	}

	[SLua.CustomLuaClassAttribute]
	public class CloudFileCallbacks
	{
		private Dictionary<int, List<CloudFileCallback>> m_callbacks;

		public void RegisterCallback(
			int id,
			CloudFileCallback.ProgressCallback progress_callback,
			CloudFileCallback.SuccessCallback success_callback,
			CloudFileCallback.ErrorCallback error_callback)
		{
			if (m_callbacks == null)
			{
				m_callbacks = new Dictionary<int, List<CloudFileCallback>>();
			}
			if (!m_callbacks.ContainsKey(id))
			{
				m_callbacks.Add(id, new List<CloudFileCallback>());
			}
			CloudFileCallback cloudFileCallback = new CloudFileCallback();
			cloudFileCallback._ProgressCallback = progress_callback;
			cloudFileCallback._SuccessCallback = success_callback;
			cloudFileCallback._ErrorCallback = error_callback;
			m_callbacks[id].Add(cloudFileCallback);
		}

		public void FireProgress(int id, float progress)
		{
			if (m_callbacks != null && m_callbacks.Count > 0)
			{
				if (m_callbacks.ContainsKey(id))
				{
					List<CloudFileCallback> listCloudFileCallback = m_callbacks[id];
					for (int i = 0; i < listCloudFileCallback.Count; ++i)
					{
						CloudFileCallback cloudFileCallback = listCloudFileCallback[i];
						cloudFileCallback.FireProgress(progress);
					}
				}
			}
		}

		public void FireSuccess(int id)
		{
			if (m_callbacks != null && m_callbacks.Count > 0)
			{
				if (m_callbacks.ContainsKey(id))
				{
					List<CloudFileCallback> listCloudFileCallback = m_callbacks[id];
					for (int i = 0; i < listCloudFileCallback.Count; ++i)
					{
						CloudFileCallback cloudFileCallback = listCloudFileCallback[i];
						cloudFileCallback.FireSuccess();
					}
				}
			}
		}

		public void FireError(int id, string error_message)
		{
			if (m_callbacks != null && m_callbacks.Count > 0)
			{
				if (m_callbacks.ContainsKey(id))
				{
					List<CloudFileCallback> listCloudFileCallback = m_callbacks[id];
					for (int i = 0; i < listCloudFileCallback.Count; ++i)
					{
						CloudFileCallback cloudFileCallback = listCloudFileCallback[i];
						cloudFileCallback.FireError(error_message);
					}
				}
			}
		}

		public void UnregisterCallback(int id)
		{
			if (m_callbacks != null && m_callbacks.Count > 0)
			{
				if (m_callbacks.ContainsKey(id))
				{
					m_callbacks.Remove(id);
				}
			}
		}
	}
}