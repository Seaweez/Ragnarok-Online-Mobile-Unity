using UnityEngine;
using System.Collections;

namespace CloudVoiceVideoTroops
{
	
	public abstract class CvMonoSingleton<T> : MonoBehaviour where T : CvMonoSingleton<T>

	{
		private static T m_Instance = null;

		public static T Instance
		{
			get {
				if (m_Instance == null) {
					m_Instance = GameObject.FindObjectOfType (typeof (T)) as T;
					if (m_Instance == null) {
						m_Instance = new GameObject ("YvSingleton of " + typeof(T).ToString(), typeof(T)).GetComponent<T> ();
						m_Instance.Init ();
					}
				}
				return m_Instance;
			}
		}
			
//		private void Awake()
//		{
//			if (m_Instance == null) {
//				m_Instance = this as T;
//			}
//		}

		public virtual void Init()
		{
		}

		private void OnApplicationQuit()
		{
		}
	}



}
