using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public class NGUIShaderManager : SingleTonGO<NGUIShaderManager>
	{
		public bool isOpen = true;
		public List<Shader> ShaderList;
		Dictionary<string, Shader> mShaderMap = new Dictionary<string, Shader> ();

		public void AddAllShader ()
		{
			
		}

		protected override void OnRegisteredMe ()
		{
			if (ShaderList != null) {
				for (int i = 0; i < ShaderList.Count; i++) {
					Shader shader = ShaderList [i];
					if (shader != null) {
						if (!mShaderMap.ContainsKey (shader.name)) {
							mShaderMap.Add (shader.name, shader);
						}
					}
				}
			}
		}

		public void ClearShaderList ()
		{
			ShaderList.Clear ();
		}

		public void AddShader (Shader shader)
		{
			ShaderList.Add (shader);
		}

		public Shader GetNGUIShader (string shaderName)
		{
			#if (UNITY_EDITOR_OSX || UNITY_EDITOR)
				return null;
			#else
				Shader shader = null;
				if (isOpen && mShaderMap.TryGetValue (shaderName, out shader)) {
					return shader;			
				}
				return shader;
			#endif	
		}
	}


}
// namespace RO
