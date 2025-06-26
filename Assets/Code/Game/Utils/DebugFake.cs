using UnityEngine;

namespace RO
{
	public class DebugFake : MonoBehaviour 
	{
		void Awake()
		{
			#if DEBUG_FAKE
			Logger.LogFormat("<color=red>Debug Fake</color>");
			#else
			GameObject.DestroyImmediate(gameObject);
			#endif // DEBUG_FAKE
		}
	}
} // namespace RO
