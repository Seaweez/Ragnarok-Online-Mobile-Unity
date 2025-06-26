using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class Forever : MonoBehaviour 
	{
		void Awake()
		{
			GameObject.DontDestroyOnLoad(gameObject);
		}
	
	}
} // namespace RO
