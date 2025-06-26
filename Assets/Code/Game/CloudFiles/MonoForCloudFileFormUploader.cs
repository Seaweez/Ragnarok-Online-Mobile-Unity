using UnityEngine;
using System.Collections;

namespace CloudFile
{
	public class MonoForCloudFileFormUploader : MonoForCloudFileUploader
	{
		public bool m_progressFlag;
		public bool m_doneFlag;
		public bool m_errorFlag;
		public string m_errorMessage;
		public bool m_timeoutFlag;

		private CloudFileFormUploader m_formUploader;
		public void Initialize(CloudFileFormUploader formUploader)
		{
			m_formUploader = formUploader;
		}

		protected override void OnUpdate ()
		{
			if (m_progressFlag) {
				CloudFileManager.Ins._CloudFileCallbacks.FireProgress(m_formUploader.Record.ID, m_formUploader.Record.Progress);
				m_progressFlag = false;
			}
			if (m_doneFlag) {
				CloudFileManager.Ins._CloudFileCallbacks.FireSuccess(m_formUploader.Record.ID);
				CloudFileManager.Ins._CloudFileCallbacks.UnregisterCallback (m_formUploader.Record.ID);

				DestroyThis ();

				m_doneFlag = false;
			}
			if (m_errorFlag) {
				CloudFileManager.Ins._CloudFileCallbacks.FireError(m_formUploader.Record.ID, m_errorMessage);
				CloudFileManager.Ins._CloudFileCallbacks.UnregisterCallback (m_formUploader.Record.ID);

				DestroyThis ();

				m_errorFlag = false;
			}
			if (m_timeoutFlag) {
				CloudFileManager.Ins._CloudFileCallbacks.FireError(m_formUploader.Record.ID, "Time out.");
				CloudFileManager.Ins._CloudFileCallbacks.UnregisterCallback (m_formUploader.Record.ID);

				DestroyThis ();

				m_timeoutFlag = false;
			}
		}

		private void DestroyThis()
		{
			m_formUploader = null;
			Component.DestroyImmediate (this);
		}
	}
}