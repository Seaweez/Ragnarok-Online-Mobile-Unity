using UnityEngine;
using System.Collections;
using System;

namespace CloudFile
{
	public class MonoForCloudFileUploader : MonoBehaviour
	{
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

		protected virtual void OnUpdate()
		{
			
		}

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

			OnUpdate ();
		}
	}
}