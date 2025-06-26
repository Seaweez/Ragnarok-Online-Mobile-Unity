using UnityEngine;
using System.Collections;
using System;

namespace CloudFile
{
	public class MonoForCloudFileNormalUploader : MonoForCloudFileUploader
	{
		public bool m_progressFlag;
		public bool m_doneFlag;
		public bool m_errorFlag;
		public string m_errorMessage;
		public bool m_timeoutFlag;

		private CloudFileNormalUploader m_normalUploader;
		public void Initialize(CloudFileNormalUploader normalUploader)
		{
			m_normalUploader = normalUploader;
		}

		protected override void OnUpdate ()
		{
			if (m_progressFlag) {
				CloudFileManager.Ins._CloudFileCallbacks.FireProgress(m_normalUploader.Record.ID, m_normalUploader.Record.Progress);
				m_progressFlag = false;
			}
			if (m_doneFlag) {
				CloudFileManager.Ins._CloudFileCallbacks.FireSuccess(m_normalUploader.Record.ID);
				CloudFileManager.Ins._CloudFileCallbacks.UnregisterCallback (m_normalUploader.Record.ID);

				DestroyThis ();

				m_doneFlag = false;
			}
			if (m_errorFlag) {
				CloudFileManager.Ins._CloudFileCallbacks.FireError(m_normalUploader.Record.ID, m_errorMessage);
				CloudFileManager.Ins._CloudFileCallbacks.UnregisterCallback (m_normalUploader.Record.ID);

				DestroyThis ();

				m_errorFlag = false;
			}
			if (m_timeoutFlag) {
				CloudFileManager.Ins._CloudFileCallbacks.FireError(m_normalUploader.Record.ID, "Time out.");
				CloudFileManager.Ins._CloudFileCallbacks.UnregisterCallback (m_normalUploader.Record.ID);

				DestroyThis ();

				m_timeoutFlag = false;
			}
		}

		private void DestroyThis()
		{
			m_normalUploader = null;
			Component.DestroyImmediate (this);
		}
	}
}