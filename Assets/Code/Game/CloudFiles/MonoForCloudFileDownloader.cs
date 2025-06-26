using UnityEngine;
using System.Collections;
using System;

namespace CloudFile
{
	public class MonoForCloudFileDownloader : MonoBehaviour
	{
		private CloudFileDownloader m_loader;

		public void Initialize(CloudFileDownloader loader)
		{
			m_loader = loader;
		}

		private float m_outtime;
		private float m_consumeTime;
		private bool m_isJudging;
		private Action m_callback;
		public void CheckTimeout(float outtime, Action callback)
		{
			m_outtime = outtime;
			m_consumeTime = 0;
			m_callback = callback;
			m_isJudging = true;
		}
		public void EndCheckTimeout()
		{
			m_isJudging = false;
		}

		public bool m_progressFlag;
		public bool m_doneFlag;
		public bool m_errorFlag;
		public string m_errorMessage;
		public bool m_timeoutFlag;

		void Update()
		{
			if (m_isJudging)
			{
				m_consumeTime += Time.deltaTime;
				if (m_consumeTime >= m_outtime)
				{
					if (m_callback != null)
						m_callback();
					m_isJudging = false;
				}
			}

			if (m_progressFlag) {
				CloudFileManager.Ins._CloudFileCallbacks.FireProgress(m_loader.Record.ID, m_loader.Record.Progress);
				m_progressFlag = false;
			}
			if (m_doneFlag) {
				CloudFileManager.Ins._CloudFileCallbacks.FireSuccess(m_loader.Record.ID);
				CloudFileManager.Ins._CloudFileCallbacks.UnregisterCallback (m_loader.Record.ID);

				DestroyThis ();

				m_doneFlag = false;
			}
			if (m_errorFlag) {
				CloudFileManager.Ins._CloudFileCallbacks.FireError(m_loader.Record.ID, m_errorMessage);
				CloudFileManager.Ins._CloudFileCallbacks.UnregisterCallback (m_loader.Record.ID);

				DestroyThis ();

				m_doneFlag = false;
			}
			if (m_timeoutFlag) {
				CloudFileManager.Ins._CloudFileCallbacks.FireError(m_loader.Record.ID, "Time out.");
				CloudFileManager.Ins._CloudFileCallbacks.UnregisterCallback (m_loader.Record.ID);

				DestroyThis ();

				m_doneFlag = false;
			}
		}

		private void DestroyThis()
		{
			m_loader = null;
			Component.DestroyImmediate (this);
		}
	}
}