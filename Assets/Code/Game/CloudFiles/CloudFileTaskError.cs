using UnityEngine;
using System.Collections;

namespace CloudFile
{
	public class TaskError
	{
		public enum E_ErrorType
		{
			NetInteractionError,
		}

		private E_ErrorType m_errorType;
		public E_ErrorType ErrorType
		{
			get
			{
				return m_errorType;
			}
			set
			{
				m_errorType = value;
			}
		}
		private string m_errorMessage;
		public string ErrorMessage
		{
			get
			{
				return m_errorMessage;
			}
			set
			{
				m_errorMessage = value;
			}
		}
	}
}