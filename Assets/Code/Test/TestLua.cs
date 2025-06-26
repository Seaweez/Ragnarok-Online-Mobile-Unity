using UnityEngine;
using System.Collections.Generic;

namespace RO.Test
{
	public class TestLua : MonoBehaviour 
	{
		public TextAsset script = null;

		private LuaWorker worker = null;

		void Start()
		{
			worker = new LuaWorker();
			if (null != script)
			{
				worker.DoString(script.text);
			}
			else
			{
				worker.DoString("print('test lua')");
			}
		}

		void Update()
		{
			if (null != worker)
			{
				worker.CallLuaStaticMethod("Update");
			}
		}
	
	}
} // namespace RO.Test
