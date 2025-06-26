using UnityEngine;
using System.Collections.Generic;

namespace RO.Test
{
	public class TestWeather : MonoBehaviour 
	{
		public string weatherName;

		private GameObject effect;

		public void Apply()
		{
			if (string.IsNullOrEmpty(weatherName))
			{
				return;
			}
			var prefab = ResourceManager.Me.SLoad<GameObject>(ResourcePathHelper.IDEffectWeather(weatherName));
			if (null != prefab)
			{
				if (null != effect)
				{
					GameObject.Destroy(effect);
				}
				effect = GameObject.Instantiate<GameObject>(prefab);
				effect.name = prefab.name;
			}
		}

		void Start()
		{
			Apply();
		}
	
	}
} // namespace RO.Test
