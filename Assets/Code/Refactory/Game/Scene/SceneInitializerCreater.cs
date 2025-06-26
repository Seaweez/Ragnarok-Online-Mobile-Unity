using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class SceneInitializerCreater : MonoBehaviour 
	{
		public SceneInitializer initializer;
	
		#region behaviour
		void Awake()
		{
			if (null != initializer)
			{
				GameObject.Instantiate<SceneInitializer>(initializer);
			}
#if Internal
            GameObject.DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<DebugMono>();
#else
            GameObject.Destroy(gameObject);
#endif
		}
#endregion behaviour
	}
} // namespace RO
